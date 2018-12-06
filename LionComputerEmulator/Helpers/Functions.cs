namespace LionComputerEmulator
{
    /// <summary>
    /// Helper Functions
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// Two Bytes to Word Big Endian
        /// </summary>
        public static ushort BytesToWord(byte firstByte, byte nextByte)
        {
            return (ushort)((firstByte << 8) | nextByte);
        }

        /// <summary>
        /// Word to Two Bytes Big Endian
        /// </summary>
        public static byte[] WordToBytes(ushort value)
        {
            return new byte[2] { (byte)(value >> 8), (byte)(value) };
        }
    }
}
