using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace LionComputerEmulator
{
    public static class Disassembler
    {
        public static bool doMonitor = false;

        public static DasmRecord BinFileDasmRecord = new DasmRecord();

        public static void GenerateInstructionsText()
        {
            List<string> txtLines = new List<string>();

            for (int i = 0; i < InstructionSet.OperationsList.Count; i++)
            {
                Operation instruction = InstructionSet.OperationsList[i];
                if (instruction != null)
                {
                    Dictionary<int, string> columns = new Dictionary<int, string>()
                        {
                            {0 ,"{0,-20}"},
                            {1 ,"{0,-7}"},
                            {2 ,"{0,-6}"},
                            {3 ,"{0,-17}"},
                            {4 ,"{0,-2}"},
                            {5 ,"{0,-17}"},
                            {6 ,"{0,-11}"},
                            {7 ,"{0,-3}"},
                            {8 ,"{0,-22}"},
                        };
                    string dummystr = string.Empty;
                    dummystr = Convert.ToString(instruction.OpCode, 2).PadLeft(16, '0');
                    dummystr = string.Format(columns[0], string.Format("{0}-{1}-{2}-{3}-{4}", dummystr.Substring(0, 7), dummystr.Substring(7, 3), dummystr.Substring(10, 1), dummystr.Substring(11, 3), dummystr.Substring(14, 2)));
                    columns[0] = dummystr;
                    dummystr = string.Format(columns[1], string.Format("0x{0}", Convert.ToString(instruction.OpCode, 16).PadLeft(5, '0')));
                    columns[1] = dummystr;
                    columns[2] = string.Format(columns[2], instruction.Mnemonic);
                    if (instruction.Type != OperationType.Implicit)
                    {
                        if (instruction.AddressingModeDestination != AddressingMode.Internal && instruction.AddressingModeSource != AddressingMode.Internal)
                        {
                            columns[3] = string.Format(columns[3], Enum.GetName(typeof(AddressingMode), instruction.AddressingModeSource));
                            columns[4] = string.Format(columns[4], "to");
                            columns[5] = string.Format(columns[5], Enum.GetName(typeof(AddressingMode), instruction.AddressingModeDestination));
                        }
                        else
                        {
                            if (instruction.AddressingModeDestination != AddressingMode.Internal)
                                columns[3] = string.Format(columns[3], Enum.GetName(typeof(AddressingMode), instruction.AddressingModeDestination));
                            if (instruction.AddressingModeSource != AddressingMode.Internal)
                                columns[3] = string.Format(columns[3], Enum.GetName(typeof(AddressingMode), instruction.AddressingModeSource));
                            columns[4] = string.Format(columns[4], string.Empty);
                            columns[5] = string.Format(columns[5], string.Empty);
                        }
                    }
                    else
                    {
                        columns[3] = string.Format(columns[3], string.Empty);
                        columns[4] = string.Format(columns[4], string.Empty);
                        columns[5] = string.Format(columns[5], string.Empty);
                    }
                    columns[6] = string.Format(columns[6], Enum.GetName(typeof(OperationType), instruction.Type));
                    columns[7] = string.Format(columns[7], instruction.Width == OperationWidth.Mixed ? "bwb" : string.Empty);
                    if (instruction.ExecuteMethod == null)
                        columns[8] = string.Format(columns[8], "!!!NullExec!!!");
                    else
                        columns[8] = string.Format(columns[8], instruction.ExecuteMethod.Method.Name);
                    string txtLine = string.Empty;
                    txtLine += string.Join(" ", columns.Values.ToArray());
                    txtLines.Add(txtLine);
                }
            }
            File.WriteAllLines("instructions.txt", txtLines);
        }

        // disassemble all memory in a list of text lines
        public static List<string> Disassemble()
        {
            List<string> disassemblyTextLines = new List<string>();

            uint memIndex = 0;
            while (memIndex <= Memory.MEMORY_TOP)
            {
                Operation instruction = Disassembly(memIndex);

                string[] memContent = new string[instruction.Length / 2];

                // memory content by operation length
                for (int _cnt = 0; _cnt < instruction.Length / 2; _cnt++)
                    memContent[_cnt] = Convert.ToString(Functions.BytesToWord(Memory.Data[memIndex + _cnt * 2], Memory.Data[memIndex + _cnt * 2 + 1]), 16).PadLeft(4, '0');
                string memContentFormatted = string.Join(" ", memContent).PadRight(14, ' '); // room for 3 hex words left aligned
                // memory ascii by operation length
                string memascii = string.Empty;
                for (int _cnt = 0; _cnt < instruction.Length; _cnt++)
                {
                    byte membyt = Memory.Data[memIndex + _cnt];
                    if (membyt >= 32 && membyt <= 127)
                    {
                        memascii += (char)membyt;
                    }
                    else
                    {
                        memascii += ' ';
                    }
                }
                // labels
                DasmSymbol symb = BinFileDasmRecord.SymbolsList.Where(w => w.DecimalValue == memIndex).FirstOrDefault();
                string label = symb == null ? string.Empty : string.Format("{0}{1}", symb.Name, symb.isLabel ? ":" : string.Empty);
                // symbolic assembly
                string assembly = instruction.DebugText;
                disassemblyTextLines.Add(string.Format("{0}  {1}  {2}  {3}  {4} {5}", Convert.ToString(memIndex).PadLeft(5, '0'), Convert.ToString(memIndex, 16).PadLeft(4, '0'), memContentFormatted.ToUpper(), memascii.PadRight(6), label.PadLeft(9, ' '), assembly.Trim().ToUpper()));

                memIndex += instruction.Length;
            }

            return disassemblyTextLines;
        }

        // debug helper
        private const string stateDebugString = @"PC:{0} SP:{1}  SR:{2}  X:{3}
{4}
{5}
{6}
{7}";

        private static string StatusRegisterValue()
        {
            return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                (State.SR & State.I) == State.I ? "I" : "-",
                (State.SR & 0x040) == 0x040 ? "?" : "-",
                (State.SR & State.T) == State.T ? "T" : "-",
                (State.SR & State.D) == State.D ? "D" : "-",
                (State.SR & State.N) == State.N ? "N" : "-",
                (State.SR & State.Z) == State.Z ? "Z" : "-",
                (State.SR & State.O) == State.O ? "O" : "-",
                (State.SR & State.C) == State.C ? "C" : "-");
        }

        // debug helper
        public static Operation Disassembly(uint memIndex)
        {
            DasmSymbol symbol = BinFileDasmRecord.SymbolsList.Where(w => w.DecimalValue == memIndex).FirstOrDefault();
            Operation instruction = InstructionDecode(memIndex);
            if (symbol != null)
            {
                if (!symbol.isLabel)
                {
                    // make variable
                    ushort dataWordValue = Functions.BytesToWord(Memory.Data[memIndex], Memory.Data[memIndex + 1]);
                    DasmSymbol valueSymbol = BinFileDasmRecord.SymbolsList.Where(w => w.isLabel == true && w.DecimalValue == dataWordValue).FirstOrDefault();

                    instruction = new Operation()
                    {
                        OpCode = dataWordValue,
                        OpCodeValue = dataWordValue,
                        Length = 2,
                        Mnemonic = "DW",
                        Type = OperationType.Undefined,
                        Operands = new List<Operand>()
                        {
                            new Operand()
                            {
                                Value = dataWordValue,
                                Symbol = valueSymbol == null ?
                                string.Format("${0}", Convert.ToString(dataWordValue, 16).ToUpper().PadLeft(4, '0')) :
                                valueSymbol.Name
                            }
                        }
                    };
                }
            }

            // undefined mnemonic passed as is
            if (instruction.Type != OperationType.Undefined)
            {
                if (instruction.Width == OperationWidth.Mixed)
                    instruction.Mnemonic += (instruction.OpCodeValue & 0x020) != 0 ? ".B" : string.Empty;

                foreach (Operand op in instruction.Operands)
                {
                    DasmSymbol valueSymbol = BinFileDasmRecord.SymbolsList.Where(w => w.DecimalValue == op.Value).FirstOrDefault();

                    // jumps see labels
                    if (instruction.Type != OperationType.Branch && valueSymbol != null)
                    {
                        // not jumps see vars reference
                        if (valueSymbol.isLabel && instruction.Context != OperationContext.Relative)
                            valueSymbol = null;
                    }

                    // lovely operands values to dump if doMonitor
                    switch (op.AddressingMode)
                    {
                        case AddressingMode.RegisterDirect:
                            op.DebugText = string.Format("{0}={1}", op.Symbol, Convert.ToString(State.A[op.Value], 16).PadLeft(4, '0'));
                            break;

                        case AddressingMode.RegisterReference:
                            op.DebugText = string.Format("{0}={1}", op.Symbol, Convert.ToString(Functions.BytesToWord(Memory.Data[State.A[op.Value]], Memory.Data[State.A[op.Value] + 1]), 16).PadLeft(4, '0'));
                            break;

                        case AddressingMode.ImmediateQuick:
                            op.DebugText = string.Format("{0}={1}", op.Symbol, op.Value);
                            break;

                        case AddressingMode.Immediate:
                            if (valueSymbol != null)// && instruction.Type == OperationType.Branch)
                                op.Symbol = valueSymbol.Name;
                            op.DebugText = string.Format("{0}={1}", op.Symbol, Convert.ToString(op.Value, 16).PadLeft(4, '0'));
                            break;

                        case AddressingMode.MemoryReference:
                            if (valueSymbol != null)
                                op.Symbol = string.Format("({0})", valueSymbol.Name);
                            op.DebugText = string.Format("{0}={1}", op.Symbol, Convert.ToString(Functions.BytesToWord(Memory.Data[op.Value], Memory.Data[op.Value + 1]), 16).PadLeft(4, '0'));
                            break;

                        case AddressingMode.StackPointer:
                            op.DebugText = string.Format("{0}={1}", "SP", Convert.ToString(State.SP, 16).PadLeft(4, '0'));
                            break;

                        case AddressingMode.StatusRegister:
                            op.DebugText = string.Format("{0}={1}", "SR", StatusRegisterValue());
                            break;

                        case AddressingMode.ProgramCounter:
                            op.DebugText = string.Format("{0}={1}", "PC", Convert.ToString(State.PC, 16).PadLeft(4, '0')); ;
                            break;
                    }
                }
            }

            instruction.DebugText = instruction.Mnemonic;
            if (instruction.Type == OperationType.Undefined || instruction.AddressingModeSource == AddressingMode.Internal || instruction.AddressingModeDestination == AddressingMode.Internal)
            {
                instruction.DebugText += " " + instruction.Operands[0].Symbol;
            }
            else if (instruction.Type == OperationType.Memory && instruction.AddressingModeDestination == AddressingMode.StackPointer)
            {
                instruction.DebugText += instruction.Operands[0].Symbol + " ";
            }
            else if (instruction.Type == OperationType.Memory && instruction.AddressingModeSource == AddressingMode.Internal)
            {
                instruction.DebugText += " " + instruction.Operands[0].Symbol + " ";
            }
            else
            {
                instruction.DebugText += instruction.Operands.Count() > 0 ? " " + instruction.Operands[0].Symbol + "," : string.Empty;
            }
            instruction.DebugText += instruction.Operands.Count() > 1 ? instruction.Operands[1].Symbol : string.Empty;

            return instruction;
        }

        // debug helper
        public static void Monitor(ushort memIndex, bool printState = false)
        {

            if (!doMonitor)
                return;

            List<string> _memstr = new List<string>();
            Operation debugOp = Disassembler.Disassembly(memIndex);
            string dasm = debugOp.DebugText;
            dasm += debugOp.Operands.Count() > 0 ? " " + debugOp.Operands[0].DebugText : string.Empty;
            dasm += debugOp.Operands.Count() > 1 ? "," + debugOp.Operands[1].DebugText : string.Empty;
            if (!printState)
            {

                dasm = "PC:" + Convert.ToString(State.PC, 16).ToUpper().PadLeft(4, '0') + " " + dasm;
                _memstr.Add(dasm);
            }
            else
            {
                string _state = string.Format(stateDebugString,
                    Convert.ToString(State.PC, 16).ToUpper().PadLeft(4, '0'),
                    Convert.ToString(State.SP, 16).ToUpper().PadLeft(4, '0'),
                    StatusRegisterValue(),
                    Convert.ToString(State.X, 16).ToUpper().PadLeft(4, '0'),
                    string.Format("A{0}:{1} {2}  A{3}:{4} {5}", 0, Convert.ToString(State.A[0], 16).ToUpper().PadLeft(4, '0'), Convert.ToString(State.A[0], 2).PadLeft(16, '0'), 1, Convert.ToString(State.A[1], 16).ToUpper().PadLeft(4, '0'), Convert.ToString(State.A[1], 2).PadLeft(16, '0')),
                    string.Format("A{0}:{1} {2}  A{3}:{4} {5}", 2, Convert.ToString(State.A[2], 16).ToUpper().PadLeft(4, '0'), Convert.ToString(State.A[2], 2).PadLeft(16, '0'), 3, Convert.ToString(State.A[3], 16).ToUpper().PadLeft(4, '0'), Convert.ToString(State.A[3], 2).PadLeft(16, '0')),
                    string.Format("A{0}:{1} {2}  A{3}:{4} {5}", 4, Convert.ToString(State.A[4], 16).ToUpper().PadLeft(4, '0'), Convert.ToString(State.A[4], 2).PadLeft(16, '0'), 5, Convert.ToString(State.A[5], 16).ToUpper().PadLeft(4, '0'), Convert.ToString(State.A[5], 2).PadLeft(16, '0')),
                    string.Format("A{0}:{1} {2}  A{3}:{4} {5}", 6, Convert.ToString(State.A[6], 16).ToUpper().PadLeft(4, '0'), Convert.ToString(State.A[6], 2).PadLeft(16, '0'), 7, Convert.ToString(State.A[7], 16).ToUpper().PadLeft(4, '0'), Convert.ToString(State.A[7], 2).PadLeft(16, '0'))
                    );

                // dump memory
                _memstr = new List<string>()
                {
                    _state,
                    dasm,
                    "--------------------------------------------------"
                };
            }
            File.AppendAllLines(Utilities.RegDumpFilename, _memstr, Encoding.ASCII);
            _memstr.Add("       0  1  2  3  4  5  6  7  8  9  A  B  C  D  E  F");
            int cnt = 0;
            // dump the first $200 bytes, 64K are too much and in debug we can write to rom for test        
            while (cnt < 0x0200)
            {
                string _log = Convert.ToString(cnt, 16).ToUpper().PadLeft(4, '0') + ": ";
                for (int __c = 0; __c < 16; __c++)
                    _log += Convert.ToString(Memory.Data[cnt + __c], 16).ToUpper().PadLeft(2, '0') + " ";
                _memstr.Add(_log.Trim());
                cnt += 16;
            }
            File.WriteAllLines("dump.txt", _memstr, Encoding.ASCII);

            if (doMonitor)
            {
                //doMonitor = false;
            }
        }

        /// <summary>
        /// Operand dispatch
        /// </summary>
        private static Operand DispatchOperand(ushort word, AddressingMode addressingMode, OperandNotation notation, ushort relativeValue)
        {
            Operand _ret = new Operand()
            {
                AddressingMode = addressingMode,
                Notation = notation
            };

            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    // immediate number
                    word += relativeValue;
                    _ret.Value = word;
                    _ret.Symbol = string.Format("${0}", Convert.ToString(word, 16).ToUpper().PadLeft(4, '0'));
                    break;

                case AddressingMode.MemoryReference:
                    // memory location
                    word += relativeValue;
                    _ret.Value = word;
                    _ret.Symbol = string.Format("(${0})", Convert.ToString(word, 16).ToUpper().PadLeft(4, '0'));
                    break;

                case AddressingMode.ImmediateQuick:
                    // immediate nibble operand bits 111100
                    _ret.Value = (ushort)((word >> 2) & 0x0f);
                    _ret.Symbol = string.Format("{0}", _ret.Value);
                    break;

                case AddressingMode.RegisterDirect:
                    //  register index
                    _ret.Value = (ushort)((word >> (notation == OperandNotation.Destination ? 6 : 2)) & 0x07);
                    _ret.Symbol = string.Format("A{0}", _ret.Value);
                    break;

                case AddressingMode.RegisterReference:
                    //  register index
                    _ret.Value = (ushort)((word >> (notation == OperandNotation.Destination ? 6 : 2)) & 0x07);
                    _ret.Symbol = string.Format("(A{0})", _ret.Value);
                    break;
            }

            return _ret;
        }

        /// <summary>
        /// Instruction Decoder for Disassembly
        /// </summary>
        private static Operation InstructionDecode(uint MemoryLocation)
        {
            ushort memoryOpCode = Functions.BytesToWord(Memory.Data[MemoryLocation], Memory.Data[MemoryLocation + 1]);
            // a new operation does not interfere with the executing
            Operation instruction = null;
            if (InstructionSet.OperationsList[(ushort)(memoryOpCode & 0x0fe03)] != null)
            {
                instruction = new Operation()
                {
                    AddressingModeDestination = InstructionSet.OperationsList[(ushort)(memoryOpCode & 0x0fe03)].AddressingModeDestination,
                    AddressingModeSource = InstructionSet.OperationsList[(ushort)(memoryOpCode & 0x0fe03)].AddressingModeSource,
                    Context = InstructionSet.OperationsList[(ushort)(memoryOpCode & 0x0fe03)].Context,
                    Length = InstructionSet.OperationsList[(ushort)(memoryOpCode & 0x0fe03)].Length,
                    Mnemonic = InstructionSet.OperationsList[(ushort)(memoryOpCode & 0x0fe03)].Mnemonic,
                    OpCode = InstructionSet.OperationsList[(ushort)(memoryOpCode & 0x0fe03)].OpCode,
                    OpCodeValue = memoryOpCode,
                    Type = InstructionSet.OperationsList[(ushort)(memoryOpCode & 0x0fe03)].Type,
                    Width = InstructionSet.OperationsList[(ushort)(memoryOpCode & 0x0fe03)].Width,
                };
            }

            if (MemoryLocation < 0x020 || instruction == null)
            {
                return new Operation()
                {
                    OpCode = memoryOpCode,
                    OpCodeValue = memoryOpCode,
                    Type = OperationType.Undefined,
                    Width = OperationWidth.Word,
                    Mnemonic = "DW",
                    Cycles = 0,
                    Length = 2,
                    Operands = new List<Operand>()
                    {
                      new Operand()
                        {
                            AddressingMode=AddressingMode.Immediate,
                            Notation=OperandNotation.Destination,
                            Symbol = string.Format("${0}", Convert.ToString(memoryOpCode, 16).ToUpper() .PadLeft(4, '0')),
                            Value = memoryOpCode
                        }
                    }
                };
            }
            //else if (instruction == null)
            //    throw new Exception("oh no no no no...");

            if (instruction.Type == OperationType.Implicit)
            {
                // decode nothing, opcode passed as is
                return instruction;
            }
            else
            {
                // operands with addressing modes
                // destination addressing
                switch (instruction.AddressingModeDestination)
                {
                    case AddressingMode.MemoryReference:
                    case AddressingMode.Immediate:
                        instruction.Operands.Add(DispatchOperand(Functions.BytesToWord(Memory.Data[MemoryLocation + 2], Memory.Data[MemoryLocation + 3]), instruction.AddressingModeDestination, OperandNotation.Destination, (ushort)(instruction.Context == OperationContext.Relative ? MemoryLocation + instruction.Length : 0)));
                        break;

                    case AddressingMode.RegisterDirect:
                    case AddressingMode.RegisterReference:
                        instruction.Operands.Add(DispatchOperand(memoryOpCode, instruction.AddressingModeDestination, OperandNotation.Destination, (ushort)(instruction.Context == OperationContext.Relative ? MemoryLocation + instruction.Length : 0)));
                        break;

                    case AddressingMode.StackPointer:
                        instruction.Operands.Add(new Operand()
                        {
                            AddressingMode = AddressingMode.StackPointer,
                            Notation = OperandNotation.Destination,
                            Symbol = "SP",
                            Value = 0,
                        });
                        break;

                    case AddressingMode.ProgramCounter:
                        instruction.Operands.Add(new Operand()
                        {
                            AddressingMode = AddressingMode.ProgramCounter,
                            Notation = OperandNotation.Destination,
                            Symbol = "PC",
                            Value = 0,
                        });
                        break;

                    case AddressingMode.StatusRegister:
                        instruction.Operands.Add(new Operand()
                        {
                            AddressingMode = AddressingMode.StatusRegister,
                            Notation = OperandNotation.Destination,
                            Symbol = "SR",
                            Value = 0,
                        });
                        break;
                }

                // source addressing
                switch (instruction.AddressingModeSource)
                {
                    case AddressingMode.Immediate:
                    case AddressingMode.MemoryReference:
                        if (instruction.AddressingModeDestination == AddressingMode.MemoryReference || instruction.AddressingModeDestination == AddressingMode.Immediate)
                        {
                            instruction.Operands.Add(DispatchOperand(Functions.BytesToWord(Memory.Data[MemoryLocation + 4], Memory.Data[MemoryLocation + 5]), instruction.AddressingModeSource, OperandNotation.Source, (ushort)(instruction.Context == OperationContext.Relative ? MemoryLocation + instruction.Length : 0)));
                        }
                        else
                        {
                            // register reference
                            instruction.Operands.Add(DispatchOperand(Functions.BytesToWord(Memory.Data[MemoryLocation + 2], Memory.Data[MemoryLocation + 3]), instruction.AddressingModeSource, OperandNotation.Source, (ushort)(instruction.Context == OperationContext.Relative ? MemoryLocation + instruction.Length : 0)));
                        }
                        break;

                    case AddressingMode.ImmediateQuick:
                    case AddressingMode.RegisterDirect:
                    case AddressingMode.RegisterReference:
                        if (instruction.AddressingModeDestination == AddressingMode.Internal)
                        {
                            instruction.Operands.Add(DispatchOperand(memoryOpCode, instruction.AddressingModeSource, OperandNotation.Destination, 0));
                        }
                        else
                        {
                            instruction.Operands.Add(DispatchOperand(memoryOpCode, instruction.AddressingModeSource, OperandNotation.Source, 0));
                        }
                        break;

                    case AddressingMode.StackPointer:
                        instruction.Operands.Add(new Operand()
                        {
                            AddressingMode = AddressingMode.StackPointer,
                            Notation = OperandNotation.Source,
                            Symbol = "SP",
                            Value = 0,
                        });
                        break;

                    case AddressingMode.ProgramCounter:
                        instruction.Operands.Add(new Operand()
                        {
                            AddressingMode = AddressingMode.ProgramCounter,
                            Notation = OperandNotation.Source,
                            Symbol = "PC",
                            Value = 0,
                        });
                        break;

                    case AddressingMode.StatusRegister:
                        instruction.Operands.Add(new Operand()
                        {
                            AddressingMode = AddressingMode.StatusRegister,
                            Notation = OperandNotation.Source,
                            Symbol = "SR",
                            Value = 0,
                        });
                        break;
                }
            }

            return instruction;
        }
    }
}
