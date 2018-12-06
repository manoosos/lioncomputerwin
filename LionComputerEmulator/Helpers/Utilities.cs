using System;
using System.Text;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace LionComputerEmulator
{
    public static class Utilities
    {
        public const string RegDumpFilename = "rundump.txt";

        public static string AppRunPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string ProcessName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
        private static string logFileName;

        static Utilities()
        {
            logFileName = "Log_" + ProcessName + ".txt";
        }

#if DEBUG
        public static void Log(string strToLog, params object[] args)
        {
            Console.WriteLine(strToLog, args);

            using (StreamWriter f = File.AppendText(Path.Combine(AppRunPath, logFileName)))
            {
                f.WriteLine(string.Format("{0}", args.Length > 0 ? string.Format(strToLog, args) : strToLog));
            }
        }
#endif
        public static string Bin2Hex(string binum)
        {
            string _ret = string.Empty;
            char[] bits = binum.Reverse().ToArray();
            ushort res = 0;
            for (int bitpos = 0; bitpos < bits.Length; bitpos++)
            {
                if (bits[bitpos] == '1')
                {
                    res |= (ushort)(1 << bitpos);
                }
            }
            _ret = Convert.ToString(res, 16).PadLeft(4, '0');
            return _ret;
        }

        /// <summary>
        /// write an object to xml file
        /// </summary>
        public static void WriteObjectToXML(object theObject, string fname = "")
        {
            try
            {
                if (string.IsNullOrEmpty(fname))
                    fname = theObject.ToString() + ".xml";

                XmlSerializer x = new XmlSerializer(theObject.GetType());
                using (XmlTextWriter xmlwr = new XmlTextWriter(Path.Combine(AppRunPath, fname), Encoding.UTF8))
                {
                    xmlwr.Formatting = Formatting.Indented;
                    var ns = new XmlSerializerNamespaces();
                    ns.Add(string.Empty, string.Empty);
                    x.Serialize(xmlwr, theObject, ns);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Log("EXCEPTION in WriteObjectToXML ( {0} ) : {1} {2}", fname, ex.Message, ex.InnerException);
#endif
            }
        }
    }
}
