namespace LionComputerEmulator
{
    public class Operand
    {
        /// <summary>
        /// Operand Addressing Mode
        /// </summary>
        public AddressingMode AddressingMode;

        /// <summary>
        /// Operand Notation (Destination, Source)
        /// </summary>
        public OperandNotation Notation;

        /// <summary>
        /// Assembly Symbol
        /// </summary>
        public string Symbol;

        /// <summary>
        /// Debug Text holds address value
        /// </summary>
        public string DebugText;

        /// <summary>
        /// Operand Value
        /// </summary>
        public ushort Value;
    }
}
