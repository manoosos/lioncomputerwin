using System.Collections.Generic;
using System.Reflection;

namespace LionComputerEmulator
{
    public class Operation
    {
        /// <summary>
        /// Debug Text
        /// </summary>
        public string DebugText;

        /// <summary>
        /// Operation Type
        /// </summary>
        public OperationType Type;

        /// <summary>
        /// Source Addressing Mode
        /// </summary>
        public AddressingMode AddressingModeSource;

        /// <summary>
        /// Destination Addressing Mode
        /// </summary>
        public AddressingMode AddressingModeDestination;

        /// <summary>
        /// Operation Context
        /// </summary>
        public OperationContext Context;

        /// <summary>
        /// Operation Width
        /// </summary>
        public OperationWidth Width;

        /// <summary>
        /// Operation Code
        /// </summary>
        public ushort OpCode;

        /// <summary>
        /// Operation Code Value from Memory
        /// </summary>
        public ushort OpCodeValue;

        /// <summary>
        /// Assembly Mnemonic
        /// </summary>
        public string Mnemonic;

        /// <summary>
        /// Length in Bytes
        /// </summary>
        public ushort Length;

        /// <summary>
        /// Execution Machine Cycles
        /// </summary>
        public int Cycles;

        /// <summary>
        /// Opcode provides the Destination Addressing Mode
        /// </summary>
        public bool EncodesDestinationAddressingMode;

        /// <summary>
        /// Opcode provides the Source Addressing Mode
        /// </summary>
        public bool EncodesSourceAddressingMode;

        /// <summary>
        /// Operands involved
        /// </summary>
        public List<Operand> Operands = new List<Operand>();

        public ExecInfo ExecuteMethod;
        public delegate void ExecInfo(Operation op);

        /// <summary>
        /// Execution Method of the Operation
        /// </summary>
        public void Execute()
        {
            ExecuteMethod(this);
        }
    }
}
