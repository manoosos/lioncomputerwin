namespace LionComputerEmulator
{
    /// <summary>
    /// Static Memory and Memory Map
    /// </summary>
    public static class Memory
    {
        /// <summary>
        /// Memory size (64K)
        /// </summary>
        public const int MEMORY_SIZE = 0x010000;

        /// <summary>
        /// Top of Memory Address
        /// </summary>
        public const int MEMORY_TOP = MEMORY_SIZE - 1;

        /// <summary>
        /// Start of ROM Address
        /// </summary>
        public const int ROM_START = 0x020;

        /// <summary>
        /// End of ROM Address
        /// </summary>
        public const int ROM_END = 0x01fff;

        /// <summary>
        /// Start of RAM Address
        /// </summary>
        public const int RAM_START = 0x02000;

        /// <summary>
        /// End of RAM Address
        /// </summary>
        public const int RAM_END = 0x0ffff;

        #region public members

        /*
        /// <summary>
        /// Burning ROM flag
        /// </summary>
        public static bool AllowBurnRom = false;
        */

        /// <summary>
        /// Memory Data Byte Array
        /// </summary>
        public static byte[] Data = new byte[MEMORY_SIZE];

        #endregion
    }
}
