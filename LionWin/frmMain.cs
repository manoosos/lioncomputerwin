using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Xml.Serialization;
using LionComputerEmulator;
using LionWin.Properties;
using System.Globalization;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace LionWin
{
    public partial class frmMain : Form
    {
        private int basProgAddress = 0;
        private int basProgLength = 0;
        private frmMemoryAddressInput MemAddressInputForm = new frmMemoryAddressInput();
        private Settings settings = Settings.Default;

        private static bool pauseEmulation = false;
        Thread runthread;
        private delegate void SetImageDlg(Bitmap img);
        private static Bitmap cmap = null;

        public frmMain()
        {
            InitializeComponent();

            while (InstructionSet.OperationsList == null)
            {
                Application.DoEvents();
            }
            Disassembler.GenerateInstructionsText();
            Display.InitScreen();
            Sound.Init();
        }


        private void btnRun_Click(object sender, EventArgs e)
        {
            if (State.PC == 0)
            {
                MessageBox.Show("Φόρτωσε ένα bin!", "Λάθος", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            Clipboard.Clear();
            btnReset.Enabled = false;
            btnRun.Enabled = false;
            mnuOpenBIN.Enabled = false;
            Cpu.isRunning = true;

            //execution loop
            runthread = new Thread(doRun);
            runthread.Start();

            while (Cpu.isRunning)
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(10);
            }
            runthread.Join();

            mnuOpenBIN.Enabled = true;
            btnRun.Enabled = true;
            btnReset.Enabled = true;
        }

        private void doRun()
        {
            Thread screenthread = new Thread(RefreshScreen);
            Thread soundthread1 = new Thread(Sound.PlayBeep1); // channel 1
            Thread soundthread2 = new Thread(Sound.PlayBeep2); // channel 2
            try
            {
                screenthread.SetApartmentState(ApartmentState.STA);
                screenthread.Start();
                soundthread1.Start();
                soundthread2.Start();
                while (Cpu.isRunning)
                {
                    if (pauseEmulation)
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    Cpu.Execute();
                }
                screenthread.Join();
                soundthread1.Join();
                soundthread2.Join();
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(@"CRASHED! Program Counter: {0} (0x0{1})

Exception: {2}

Stacktrace: {3}", State.PC, Convert.ToString(State.PC, 16).PadLeft(4, '0'), ex.Message, ex.StackTrace), "Undefined Instruction", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                Cpu.isRunning = false;
                if (runthread != null)
                    runthread.Abort();
                if (screenthread != null)
                    screenthread.Abort();
                if (soundthread1 != null)
                    soundthread1.Abort();
                if (soundthread2 != null)
                    soundthread2.Abort();
                Cpu.Reset();
            }
        }

        private void RefreshScreen()
        {
            DateTime lastRefresh = DateTime.MinValue;
            while (Cpu.isRunning)
            {
                if (pauseEmulation)
                {
                    Thread.Sleep(100);
                    continue;
                }

                if ((DateTime.Now - lastRefresh).TotalMilliseconds > 16)
                {
                    Keyboard.ScanKeysForJoystick();
                    SetImage(Display.Screen());
                    lastRefresh = DateTime.Now;
                }
                Thread.Sleep(1);
            }
        }

        private void SetImage(Bitmap img)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new SetImageDlg(SetImage), new object[] { img });
            }
            else
            {
                if (cmap != null) cmap.Dispose();
                Bitmap tmp;
                lock (Display.copylock)
                    tmp = (Bitmap)img.Clone();
                picScreen.Image = (Image)tmp;
                cmap = tmp;
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cpu.isRunning = false;
            if (runthread != null)
                runthread.Join();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Cpu.isRunning = false;
            if (runthread != null)
                runthread.Join();
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            // no keys to form children
            e.SuppressKeyPress = true;
            e.Handled = true;
        }

        private void frmMain_KeyUp(object sender, KeyEventArgs e)
        {
            KeysDispatcher(e.KeyData, e.KeyValue, false);
        }


        // essential for bypassing default handler for form keys
        // so not messup keystrokes with form buttons etc
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            return true;
        }

        private void frmMain_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            KeysDispatcher(e.KeyData, e.KeyValue, true);
        }

        private void KeysDispatcher(Keys keyData, int keyValue, bool keyDown)
        {
            if (!Cpu.isRunning)
                return;

            switch (keyData.ToString())
            {
                case "Up":
                case "Down":
                case "Left":
                case "Right":
                case "ControlKey, Control":
                case "ControlKey":
                    // don't pass joystick
                    break;

                default:
                    if (keyDown)
                        Keyboard.SendKeysToSerial(keyData.ToString());
                    break;
            }
        }

        private void mnuLoadBIN_Click(object sender, EventArgs e)
        {
            pauseEmulation = true;
            try
            {
                string fname;
                if (string.IsNullOrEmpty(fname = OpenFile("BIN files|*.bin|RBN files|*.rbn|All files|*.*")))
                    return;
                try
                {
                    MemAddressInputForm.ShowDialog(this);
                    if (MemAddressInputForm.cancel)
                    {
                        MessageBox.Show("Bin Not Loaded!");
                        return;
                    }

                    string memVal = settings.MemoryLoadAddress.ToLower();
                    int memAddr = 0;
                    if (memVal.Contains('x') || memVal.Contains('$'))
                    {
                        memVal = memVal.Replace("x", string.Empty).Replace("$", string.Empty);
                        memAddr = int.Parse(memVal, NumberStyles.HexNumber);
                    }
                    else
                    {
                        memAddr = int.Parse(memVal);
                    }

                    // even address
                    memAddr &= -2;

                    byte[] bin = File.ReadAllBytes(fname);
                    Buffer.BlockCopy(bin, 0, Memory.Data, memAddr, bin.Length + memAddr > Memory.MEMORY_TOP ? Memory.MEMORY_TOP - memAddr : bin.Length);

                    UpdateDasmSymbols(fname, Path.GetExtension(fname) == ".rbn" ? memAddr : 0);

                    List<string> dasmtext = Disassembler.Disassemble();
                    File.WriteAllLines("disassembly.lst", dasmtext, Encoding.ASCII);

                    MessageBox.Show(this, string.Format("Bin Loaded at {0}", memAddr));

                }
                catch (Exception ex)
                {
                    string msg = @"Exception: {0}
{1}

PC: {2}";
                    MessageBox.Show(string.Format(msg, ex.Message, ex.StackTrace, Convert.ToString(State.PC, 16).PadLeft(4, '0')), "Λάθος", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                pauseEmulation = false;
            }

        }

        private void mnuLoadBAS_Click(object sender, EventArgs e)
        {
            pauseEmulation = true;
            try
            {
                string fname;
                if (string.IsNullOrEmpty(fname = OpenFile("BAS files|*.bas|TXT files|*.txt|All files|*.*", true)))
                    return;
                try
                {
                    if (Path.GetExtension(fname).ToUpper() == ".BAS")
                    {
                        byte[] bin = File.ReadAllBytes(fname);
                        int length = basProgAddress + bin.Length;
                        if ((char)bin[bin.Length - 1] != '\r')
                        {
                            byte[] correctbin = new byte[bin.Length + 1];
                            Buffer.BlockCopy(bin, 0, correctbin, 0, bin.Length);
                            correctbin[bin.Length] = (byte)'\r';
                            bin = correctbin;
                            length = basProgAddress + bin.Length;
                        }
                        Buffer.BlockCopy(bin, 0, Memory.Data, basProgAddress, length > Memory.MEMORY_TOP ? Memory.MEMORY_TOP - basProgAddress : bin.Length);
                        byte[] len = Functions.WordToBytes((ushort)(length));
                        Memory.Data[basProgLength] = len[0];
                        Memory.Data[basProgLength + 1] = len[1];
                    }

                    if (Path.GetExtension(fname).ToUpper() == ".TXT")
                    {
                        string[] textlines = File.ReadAllLines(fname);
                        int memindex = basProgAddress;
                        foreach (string line in textlines)
                        {
                            string[] verbs = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            string[] clause = new string[verbs.Length - 1];
                            Array.Copy(verbs, 1, clause, 0, clause.Length);
                            byte[] asciiclause = ASCIIEncoding.ASCII.GetBytes(string.Join(" ", clause).Trim());
                            byte[] lineval = Functions.WordToBytes(Convert.ToUInt16(verbs[0]));
                            Memory.Data[memindex++] = lineval[0];
                            Memory.Data[memindex++] = lineval[1];
                            Buffer.BlockCopy(asciiclause, 0, Memory.Data, memindex, asciiclause.Length);
                            memindex += asciiclause.Length;
                            Memory.Data[memindex++] = (byte)'\r'; // cr
                        }
                        byte[] len = Functions.WordToBytes((ushort)(memindex));
                        Memory.Data[basProgLength] = len[0];
                        Memory.Data[basProgLength + 1] = len[1];
                    }
                }
                catch (Exception ex)
                {
                    string msg = @"Exception: {0}
{1}

PC: {2}";
                    MessageBox.Show(string.Format(msg, ex.Message, ex.StackTrace, Convert.ToString(State.PC, 16).PadLeft(4, '0')), "Λάθος", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                pauseEmulation = false;
            }

        }

        private void mnuOpenBIN_Click(object sender, EventArgs e)
        {
            pauseEmulation = true;
            try
            {
                btnReset.Enabled = false;
                btnRun.Enabled = false;
                btnStop.Enabled = false;

                string fname;
                if (string.IsNullOrEmpty(fname = OpenFile("BIN files|*.bin|All files|*.*")))
                    return;

                fname = fname.Replace(".STATE", string.Empty).Replace(".VRAM", string.Empty);
                if (!File.Exists(fname))
                {
                    MessageBox.Show("Bin File Not Exists!");
                    return;
                }

                try
                {
                    basProgAddress = 0;
                    basProgLength = 0;
                    if (File.Exists(Utilities.RegDumpFilename))
                        File.Delete(Utilities.RegDumpFilename);

                    byte[] bin = File.ReadAllBytes(fname);
                    Buffer.BlockCopy(bin, 0, Memory.Data, 0, bin.Length);
                    if (File.Exists(fname.Replace(".bin", ".VRAM.bin")))
                    {
                        bin = File.ReadAllBytes(fname.Replace(".bin", ".VRAM.bin"));
                        Buffer.BlockCopy(bin, 0, Display.Ram, 0, bin.Length);
                    }

                    UpdateDasmSymbols(fname);

                    List<string> dasmtext = Disassembler.Disassemble();
                    File.WriteAllLines("disassembly.lst", dasmtext, Encoding.ASCII);
                    if (File.Exists(fname.Replace(".bin", ".STATE.bin")))
                    {
                        byte[] _state = File.ReadAllBytes(fname.Replace(".bin", ".STATE.bin"));
                        int _bytndx = 0, _andx = 0;
                        if (_state.Length > 0)
                        {
                            State.A[_andx++] = (ushort)(_state[_bytndx++] << 8 | _state[_bytndx++]);
                            State.A[_andx++] = (ushort)(_state[_bytndx++] << 8 | _state[_bytndx++]);
                            State.A[_andx++] = (ushort)(_state[_bytndx++] << 8 | _state[_bytndx++]);
                            State.A[_andx++] = (ushort)(_state[_bytndx++] << 8 | _state[_bytndx++]);
                            State.A[_andx++] = (ushort)(_state[_bytndx++] << 8 | _state[_bytndx++]);
                            State.A[_andx++] = (ushort)(_state[_bytndx++] << 8 | _state[_bytndx++]);
                            State.A[_andx++] = (ushort)(_state[_bytndx++] << 8 | _state[_bytndx++]);
                            State.A[_andx++] = (ushort)(_state[_bytndx++] << 8 | _state[_bytndx++]);
                            State.PC = (ushort)(_state[_bytndx++] << 8 | _state[_bytndx++]);
                            State.SP = (ushort)(_state[_bytndx++] << 8 | _state[_bytndx++]);
                            State.SR = _state[_bytndx++];
                            State.X = (ushort)(_state[_bytndx++] << 8 | _state[_bytndx++]);
                        }
                        else
                        {
                            Cpu.Reset();
                        }
                    }
                    else
                    {
                        Cpu.Reset();
                    }
                }
                catch (Exception ex)
                {
                    string msg = @"Exception: {0}
{1}

PC: {2}";
                    MessageBox.Show(string.Format(msg, ex.Message, ex.StackTrace, Convert.ToString(State.PC, 16).PadLeft(4, '0')), "Λάθος", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                btnReset.Enabled = true;
                btnRun.Enabled = true;
                btnStop.Enabled = true;
            }
            finally
            {
                pauseEmulation = false;
            }

        }

        private void UpdateDasmSymbols(string binfilename, int offset = 0)
        {
            DasmRecord _filedasmsymbols = new DasmRecord();
            List<DasmSymbol> _addsymbols = new List<DasmSymbol>();

            string dasmFile = Path.Combine(Path.GetDirectoryName(binfilename), Path.GetFileNameWithoutExtension(binfilename) + ".xml");
            if (File.Exists(Path.Combine(Path.GetDirectoryName(binfilename), dasmFile)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(DasmRecord));
                using (StreamReader reader = new StreamReader(dasmFile))
                {
                    _filedasmsymbols = (DasmRecord)(serializer.Deserialize(reader));
                }

                foreach (DasmSymbol filesym in _filedasmsymbols.SymbolsList)
                {
                    if (Path.GetExtension(binfilename) == ".rbn")
                    {
                        filesym.DecimalValue += (uint)offset;
                        filesym.BinaryValue = Convert.ToString(filesym.DecimalValue, 2).PadLeft(16, '0');
                        filesym.HexValue = Convert.ToString(filesym.DecimalValue, 16).PadLeft(4, '0');
                    }

                    DasmSymbol _tmp = Disassembler.BinFileDasmRecord.SymbolsList.Where(w => w.DecimalValue == filesym.DecimalValue).FirstOrDefault();

                    if (_tmp == null)
                    {
                        _tmp = Disassembler.BinFileDasmRecord.SymbolsList.Where(w => w.Name == filesym.Name).FirstOrDefault();

                        if (_tmp != null)
                        {
                            if (filesym.DecimalValue != _tmp.DecimalValue)
                            {
                                _addsymbols.Add(new DasmSymbol()
                                {
                                    BinaryValue = filesym.BinaryValue,
                                    DecimalValue = filesym.DecimalValue,
                                    HexValue = filesym.HexValue,
                                    isLabel = filesym.isLabel,
                                    Name = filesym.Name + "_"
                                });
                            }
                        }
                        else
                        {
                            _addsymbols.Add(filesym);
                        }
                    }
                }

                if (_addsymbols.Count > 0)
                    Disassembler.BinFileDasmRecord.SymbolsList.AddRange(_addsymbols);
                foreach (DasmSymbol sym in _filedasmsymbols.SymbolsList)
                {
                    switch (sym.Name)
                    {
                        case "TXTBGN":
                            basProgAddress = (int)sym.DecimalValue;
                            break;

                        case "TXTUNF":
                            basProgLength = (int)sym.DecimalValue;
                            break;

                        case "COUNTER":
                            Cpu.COUNTER = (ushort)sym.DecimalValue;
                            break;
                    }
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Cpu.Reset();
        }

        private string OpenFile(string filter, bool isBasic = false)
        {
            if (isBasic)
            {
                if (basProgAddress == 0)
                {
                    MessageBox.Show("Δέν ξέρω που είναι το TXTBGN !!!\r\nΚάνε compile και φόρτωσε ένα σύστημα\r\nγια να φτιαχτούν τα Symbols!", "Λάθος", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return null;
                }
                if (basProgLength == 0)
                {
                    MessageBox.Show("Δέν ξέρω που είναι το TXTUNF !!!\r\nΚάνε compile και φόρτωσε ένα σύστημα\r\nγια να φτιαχτούν τα Symbols!", "Λάθος", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return null;
                }

            }

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.RestoreDirectory = true;
                ofd.Filter = filter;

                if (ofd.ShowDialog() == DialogResult.OK)
                    return ofd.FileName;
            }
            return null;
        }

        private string SaveFile(string filter, bool isBasic = false)
        {
            if (isBasic)
            {
                if (basProgAddress == 0)
                {
                    MessageBox.Show("Δέν ξέρω που είναι το TXTBGN !!!\r\nΚάνε compile και φόρτωσε ένα σύστημα\r\nγια να φτιαχτούν τα Symbols!", "Λάθος", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return null;
                }
                if (basProgLength == 0)
                {
                    MessageBox.Show("Δέν ξέρω που είναι το TXTUNF !!!\r\nΚάνε compile και φόρτωσε ένα σύστημα\r\nγια να φτιαχτούν τα Symbols!", "Λάθος", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return null;
                }

            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.RestoreDirectory = true;
                sfd.Filter = filter;

                if (sfd.ShowDialog() == DialogResult.OK)
                    return sfd.FileName;
            }
            return null;
        }

        private void mnuSaveBAS_Click(object sender, EventArgs e)
        {
            pauseEmulation = true;
            try
            {
                int proglength = (int)(Functions.BytesToWord(Memory.Data[basProgLength], Memory.Data[basProgLength + 1]) - basProgAddress);

                if (proglength < 1)
                {
                    MessageBox.Show("Δέν υπάρχει πρόγραμμα Basic στη μνήμη !", "Λάθος", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                string fname;
                if (string.IsNullOrEmpty(fname = SaveFile("Bas files|*.bas|All files|*.*", true)))
                    return;

                if (Path.GetExtension(fname).ToLower() != ".bas")
                    fname += ".bas";

                string asciifname = fname + ".txt";

                // in-memory bas
                byte[] basprogbytes = new byte[proglength];
                Buffer.BlockCopy(Memory.Data, basProgAddress, basprogbytes, 0, proglength);
                if (basprogbytes[basprogbytes.Length - 1] != (byte)'\r')
                {
                    byte[] correctbin = new byte[basprogbytes.Length + 1];
                    Buffer.BlockCopy(basprogbytes, 0, correctbin, 0, basprogbytes.Length);
                    correctbin[basprogbytes.Length] = (byte)'\r';
                    basprogbytes = correctbin;
                }
                File.WriteAllBytes(fname, basprogbytes);

                if (File.Exists(asciifname))
                    File.Delete(asciifname);
                List<string> proglines = new List<string>();
                string progline = string.Empty;
                for (int cnt = 0; cnt < proglength; cnt++)
                {
                    if (basprogbytes[cnt] == 0x0d && progline.Length > 0)
                    {
                        proglines.Add(progline);
                        progline = string.Empty;
                        cnt++;
                    }
                    if (cnt < proglength)
                    {
                        if (string.IsNullOrEmpty(progline))
                        {
                            progline += Functions.BytesToWord(basprogbytes[cnt], basprogbytes[cnt + 1]);
                            progline += ' ';
                            cnt++; ;
                        }
                        else
                        {
                            progline += Convert.ToChar(basprogbytes[cnt]);
                        }
                    }
                }
                if (progline.Length > 0)
                    proglines.Add(progline);

                if (proglines.Count > 0)
                    File.WriteAllLines(asciifname, proglines, Encoding.ASCII);
            }
            finally
            {
                pauseEmulation = false;
            }


        }

        private void mnuQuit_Click(object sender, EventArgs e)
        {
            Cpu.isRunning = false;
            Application.Exit();
        }

        private void mnuSaveMemory_Click(object sender, EventArgs e)
        {
            Cpu.isRunning = false;
            btnReset.Enabled = true;
            btnRun.Enabled = true;
            btnStop.Enabled = false;
            string fname;
            if (string.IsNullOrEmpty(fname = SaveFile("Bin files|*.bin|All files|*.*")))
            {
                btnStop.Enabled = true;
                return;
            }
            if (Path.GetExtension(fname).ToLower() != ".bin")
                fname += ".bin";
            //fname = Path.Combine(Path.GetDirectoryName(fname), string.Format("{0}.PC{1}.bin", Path.GetFileNameWithoutExtension(fname), State.PC));
            File.WriteAllBytes(fname, Memory.Data);
            Utilities.WriteObjectToXML(Disassembler.BinFileDasmRecord, fname.Replace(".bin", ".xml"));
            File.WriteAllBytes(fname.Replace(".bin", ".VRAM.bin"), Display.Ram);
            int _andx = 0;
            byte[] _state = new byte[]
            {
                (byte)(State.A[_andx]>>8),
                (byte)State.A[_andx++]   ,
                (byte)(State.A[_andx]>>8),
                (byte)State.A[_andx++],
                (byte)(State.A[_andx]>>8),
                (byte)State.A[_andx++],
                (byte)(State.A[_andx]>>8),
                (byte)State.A[_andx++],
                (byte)(State.A[_andx]>>8),
                (byte)State.A[_andx++],
                (byte)(State.A[_andx]>>8),
                (byte)State.A[_andx++],
                (byte)(State.A[_andx]>>8),
                (byte)State.A[_andx++],
                (byte)(State.A[_andx]>>8),
                (byte)State.A[_andx++],
                (byte)(State.PC>>8),
                (byte)State.PC,
                (byte)(State.SP>>8),
                (byte)State.SP,
                State.SR,
                (byte)(State.X>>8),
                (byte)State.X
            };
            File.WriteAllBytes(fname.Replace(".bin", ".STATE.bin"), _state);
            List<string> dasmtext = Disassembler.Disassemble();
            File.WriteAllLines("disassembly.lst", dasmtext, Encoding.ASCII);
            btnStop.Enabled = true;
        }

        private void mnuSaveBmp_Click(object sender, EventArgs e)
        {
            pauseEmulation = true;
            try
            {
                if (picScreen.Image == null)
                    return;
                string fname;
                if (string.IsNullOrEmpty(fname = SaveFile("Bitmap files|*.bmp|All files|*.*")))
                    return;
                if (Path.GetExtension(fname).ToLower() != ".bmp")
                    fname += ".bmp";
                picScreen.Image.Save(fname);
            }
            finally
            {
                pauseEmulation = false;
            }
        }

        private void mnuImportLeonImage_Click(object sender, EventArgs e)
        {
            pauseEmulation = true;
            try
            {
                string fname;
                if (string.IsNullOrEmpty(fname = OpenFile("SCR files|*.scr|All files|*.*")))
                    return;
                if (Path.GetExtension(fname).ToLower() != ".scr")
                    fname += ".scr";
                byte[] scr = File.ReadAllBytes(fname);
                int _imagemaxlen = Display.VIDEO_RAM_END_MODE1 - Display.VIDEO_RAM_START_MODE1;
                Buffer.BlockCopy(scr, 0, Display.Ram, Display.VIDEO_RAM_START_MODE1, scr.Length < _imagemaxlen ? scr.Length : _imagemaxlen);
            }
            catch (Exception ex)
            {
                string msg = @"Exception: {0}
{1}

PC: {2}";
                MessageBox.Show(string.Format(msg, ex.Message, ex.StackTrace, Convert.ToString(State.PC, 16).PadLeft(4, '0')), "Λάθος", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                pauseEmulation = false;
            }
        }

        private void mnuExportLeonImage_Click(object sender, EventArgs e)
        {
            pauseEmulation = true;
            try
            {
                string fname;
                if (string.IsNullOrEmpty(fname = SaveFile("SCR files|*.scr|All files|*.*")))
                    return;
                if (Path.GetExtension(fname).ToLower() != ".scr")
                    fname += ".scr";
                int _imagemaxlen = Display.VIDEO_RAM_END_MODE1 - Display.VIDEO_RAM_START_MODE1;

                byte[] scr = new byte[_imagemaxlen];
                Buffer.BlockCopy(Display.Ram, Display.VIDEO_RAM_START_MODE1, scr, 0, _imagemaxlen);
                File.WriteAllBytes(fname, scr);
            }
            finally
            {
                pauseEmulation = false;
            }
        }

        private void picMouseClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    if (Cpu.isRunning)
                    {
                        if (Clipboard.ContainsText())
                        {
                            string clipText = Clipboard.GetText();
                            for (int t = 0; t < clipText.Length; t++)
                            {
                                Keyboard.SendKeysToSerial(clipText.Substring(t, 1));
                                while ((Device.Port[Device.SERIAL_SKBD_STATUS] & 2) != 0) ;
                            }
                        }
                    }
                    break;
            }
        }
    }
}
