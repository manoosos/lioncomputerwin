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
        /// SR TRAP Interrupt 15 on-1/off-0
        /// </summary>
        public const byte T = 0x020;

        /// <summary>
        /// SR Global Interrupts
        /// </summary>
        public const byte I = 0x40;

        /// <summary>
        /// SR Stack Ram Page Mask
        /// </summary>
        public const ushort StackPageMask = 0x0380;

        /// <summary>
        /// SR Data Ram Page Mask
        /// </summary>
        public const ushort DataPageMask = 0x01C00;

        /// <summary>
        /// SR Code Ram Page Mask
        /// </summary>
        public const ushort CodePageMask = 0x0E000;

        /// <summary>
        /// Memory Register Array
        /// </summary>
        public static ushort[] A = new ushort[8];

        /// <summary>
        /// Internal Index Register
        /// </summary>
        public static ushort X = 0;


/*
SR register bits:
(as with all registers, SR(0) is the rightmost less significant bit )
SR(0) = Carry flag
SR(1) = Overflow flag
SR(2) = Zero flag
SR(3) = Negative flag
SR(4) = JXA increase or decrease register
SR(5) = TRAP Interrupt 15 on/off (off=0 the default value)
SR(6) = Interrupt disable (default disabled)
SR(7..9)   Current StackPage
SR(10..12) Current Data Page
SR(13..15) Current Code Page
* */

        /// <summary>
        /// Status Register
        /// </summary>
        public static ushort SR = 0; // CPG DPG SPG I T D Z N O C

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
