using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LionComputerEmulator
{
    public static class DiskOperations
    {
        private static byte[] sector = new byte[512]; // for now a block is 1 sector 512 bytes

        public static string VHDPath = string.Empty;

        public static bool isDiskReadWrite = false;

        public static ushort ReadBlockToLionBuffer(int sectorno, int memorypoint)
        {
            ushort _ret = 0;
            if (!string.IsNullOrEmpty(VHDPath))
            {
                sectorno <<= 9;
                using (BinaryReader br = new BinaryReader(File.Open(VHDPath, FileMode.Open, isDiskReadWrite ? FileAccess.ReadWrite : FileAccess.Read, FileShare.ReadWrite)))
                {
                    br.BaseStream.Seek(sectorno, SeekOrigin.Begin);
                    sector = br.ReadBytes(sector.Length);
                    Buffer.BlockCopy(sector, 0, Memory.Data, memorypoint, sector.Length);
                    _ret = 0x0100;
                }
            }
            return _ret;
        }

        public static ushort WriteLionBufferToBlock(int memorypoint, int sectorno)
        {
            ushort _ret = 0;
            if (!string.IsNullOrEmpty(VHDPath) && isDiskReadWrite)
            {
                sectorno <<= 9;
                Buffer.BlockCopy(Memory.Data, memorypoint, sector, 0, sector.Length);
                using (BinaryWriter bw = new BinaryWriter(File.Open(VHDPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)))
                {
                    bw.BaseStream.Seek(sectorno, SeekOrigin.Begin);
                    bw.Write(sector);
                    _ret = 0x0100;
                }
            }
            return _ret;
        }
    }
}
