using System;
using System.Collections.Generic;
using System.Linq;

namespace LionComputerEmulator
{
    public static class Cpu
    {
        public static bool isRunning;

        // in memory vbl counter
        public static ushort COUNTER = 0;

        /// <summary>
        /// Reset method
        /// </summary>
        public static void Reset()
        {
            State.A = new ushort[8];
            State.SR = 0;
            State.SP = 0x03ffc;
            State.PC = 0x020;
        }

        /// <summary>
        /// Execute PC method
        /// </summary>
        public static void Execute()
        {
            ushort memoryOpCodeValue = (ushort)((Memory.Data[State.PC] << 8) | Memory.Data[State.PC + 1]);
            int memoryOpCode = (memoryOpCodeValue & 0x0fe03);
            InstructionSet.OperationsList[memoryOpCode].OpCodeValue = memoryOpCodeValue;
            InstructionSet.OperationsList[memoryOpCode].Execute();
        }
    }
}
