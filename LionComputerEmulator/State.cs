namespace LionComputerEmulator
{
    /// <summary>
    /// The machine state
    /// </summary>
    public static class State
    {
        /// <summary>
        /// SR Carry Flag
        /// </summary>
        public const byte C = 1;

        /// <summary>
        /// SR Overflow Flag
        /// </summary>
        public const byte O = 2;

        /// <summary>
        /// SR Zero Flag
        /// </summary>
        public const byte Z = 4;

        /// <summary>
        /// SR Negative Flag
        /// </summary>
        public const byte N = 8;

        /// <summary>
        /// SR Direction Flag
        /// </summary>
        public const byte D = 0x010;

        /// <summary>
        /// SR Trace Flag
        /// </summary>
        public const byte T = 0x020;

        /// <summary>
        /// SR Global Interrupts
        /// </summary>
        public const byte I = 0x80;

        /// <summary>
        /// Memory Register Array
        /// </summary>
        public static ushort[] A = new ushort[8];

        /// <summary>
        /// Internal Index Register
        /// </summary>
        public static ushort X = 0;

        /// <summary>
        /// Status Register
        /// </summary>
        public static byte SR = 0; //  I ? T D Z N O C

        /// <summary>
        /// Stack Pointer
        /// </summary>
        public static ushort SP = 0;

        /// <summary>
        /// Program Counter
        /// </summary>
        public static ushort PC = 0;
    }
}
