using System;

namespace LionComputerEmulator
{
    public static class OperationProcessing
    {
        // xor bits and masks for arithmetic operations
        private const ushort wordHiBitMask = 0x08000;
        private const ushort wordAllBitsMask = 0x0ffff;
        private const ushort byteHiBitMask = 0x080;
        private const ushort byteAllBitsMask = 0x0ff;
        private const int byteCarryMask = 0x0100;

        // bytewide bit mask
        private const ushort bwb = 0x020;

        // vars used
        private static byte dstvalb;
        private static byte srcvalb;
        private static int dstndx;
        private static int srcndx;
        private static int portnum;
        private static int result;
        private static int calcValue;
        private static long resultlong;
        private static ushort valuew;
        private static ushort dstvalw;
        private static ushort srcvalw;

        /// <summary>
        /// Wait for n Cycles to emulate native speed
        /// </summary>
        private static void WaitForCycles(int cycles)
        {
            // todo: find a nice timebase to wait
            for (int _cnt = 0; _cnt < cycles; _cnt++) ;
        }

        /// <summary>
        /// Push a word to Stack
        /// </summary>
        private static void PushToStack(ushort value)
        {
            Memory.Data[State.SP] = (byte)(value >> 8);
            Memory.Data[State.SP + 1] = (byte)value;
            State.SP -= 2;
        }

        /// <summary>
        /// Pop a word from Stack
        /// </summary>
        private static ushort PopFromStack()
        {
            State.SP += 2;
            return (ushort)(Memory.Data[State.SP] << 8 | Memory.Data[State.SP + 1]);
        }

        /// <summary>
        /// Adder adds two Words, sets the Flags and returns a Word result
        /// </summary>
        private static ushort AddAndSetFlagsWord(ushort dstValue, ushort srcValue, bool useCarry, bool subtract)
        {
            calcValue = subtract ? (srcValue ^ wordAllBitsMask) + 1 : srcValue;
            result = dstValue + calcValue;
            if (useCarry)
                result += State.SR & State.C;

            State.SR &= 0x0fff0;

            if (((result & (wordAllBitsMask + 1)) != 0) ^ subtract)
                State.SR |= State.C;
            if ((result & wordAllBitsMask) == 0)
                State.SR |= State.Z;
            else if ((result & wordHiBitMask) == wordHiBitMask)
                State.SR |= State.N;
            // overflow on msbit: dst 0 src 0 res 1, dst 1 src 1 res 0
            if (((dstValue & wordHiBitMask) | (calcValue & wordHiBitMask) | ((result ^ wordAllBitsMask) & wordHiBitMask)) == 0)
                State.SR |= State.O;
            else if ((((dstValue ^ wordAllBitsMask) & wordHiBitMask) | ((calcValue ^ wordAllBitsMask) & wordHiBitMask) | (result & wordHiBitMask)) == 0)
                State.SR |= State.O;

            return (ushort)result;
        }

        /// <summary>
        /// Adder adds two Bytes, sets the Flags and returns a Byte result
        /// </summary>
        private static byte AddAndSetFlagsByte(ushort dstValue, ushort srcValue, bool useCarry, bool subtract)
        {
            srcValue &= byteAllBitsMask;
            dstValue &= byteAllBitsMask; // strip byte anyway

            calcValue = subtract ? (srcValue ^ byteAllBitsMask) + 1 : srcValue;
            result = dstValue + calcValue;
            if (useCarry)
                result += State.SR & State.C;

            State.SR &= 0x0fff0;

            if (((result & (byteCarryMask)) != 0) ^ subtract)
                State.SR |= State.C;
            if ((result & byteAllBitsMask) == 0)
                State.SR |= State.Z;
            else if ((result & byteHiBitMask) == byteHiBitMask)
                State.SR |= State.N;
            // overflow on msbit: dst 0 src 0 res 1, dst 1 src 1 res 0
            if (((dstValue & byteHiBitMask) | (calcValue & byteHiBitMask) | ((result ^ byteAllBitsMask) & byteHiBitMask)) == 0)
                State.SR |= State.O;
            else if ((((dstValue ^ byteAllBitsMask) & byteHiBitMask) | ((calcValue ^ byteAllBitsMask) & byteHiBitMask) | (result & byteHiBitMask)) == 0)
                State.SR |= State.O;

            return (byte)result;
        }

        /// <summary>
        /// Provides overriden Static GetType()
        /// </summary>
        public static new Type GetType()
        {
            return typeof(OperationProcessing);
        }

        #region Memory Operations

        /// <summary>
        /// Move Register Word to Register
        /// </summary>
        public static void MovRegDirRegDir(Operation operation)
        {
            valuew = State.A[(operation.OpCodeValue >> 2) & 0x07];
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            if ((operation.OpCodeValue & bwb) != 0)
            {
                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= (byte)valuew;
            }
            else
            {
                State.A[dstndx] = valuew;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Move Immediate Word to Register
        /// </summary>
        public static void MovRegDirImd(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            if ((operation.OpCodeValue & bwb) != 0)
            {
                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= Memory.Data[State.PC + 3];
            }
            else
            {
                State.A[dstndx] = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Move Register Reference to Register
        /// </summary>
        public static void MovRegDirRegRef(Operation operation)
        {
            srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            if ((operation.OpCodeValue & bwb) != 0)
            {
                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= Memory.Data[srcndx];
            }
            else
            {
                State.A[dstndx] = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Move Memory Reference to Register
        /// </summary>
        public static void MovRegDirMemRef(Operation operation)
        {
            srcndx = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            if ((operation.OpCodeValue & bwb) != 0)
            {
                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= Memory.Data[srcndx];
            }
            else
            {
                State.A[dstndx] = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Move Register Word to Register Reference
        /// </summary>
        public static void MovRegRefRegDir(Operation operation)
        {
            srcvalw = State.A[(operation.OpCodeValue >> 2) & 0x07];
            dstndx = State.A[(operation.OpCodeValue >> 6) & 0x07];
            if ((operation.OpCodeValue & bwb) == 0)
            {
                Memory.Data[dstndx++] = (byte)(srcvalw >> 8);
            }
            Memory.Data[dstndx] = (byte)srcvalw;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Move Immediate Word to Register Reference
        /// </summary>
        public static void MovRegRefImd(Operation operation)
        {
            dstndx = State.A[(operation.OpCodeValue >> 6) & 0x07];
            if ((operation.OpCodeValue & bwb) != 0)
            {
                Memory.Data[dstndx] = Memory.Data[State.PC + 3];
            }
            else
            {
                srcvalw = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                Memory.Data[dstndx++] = (byte)(srcvalw >> 8);
                Memory.Data[dstndx] = (byte)srcvalw;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Move Register Reference to Register Reference
        /// </summary>
        public static void MovRegRefRegRef(Operation operation)
        {
            srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
            dstndx = State.A[(operation.OpCodeValue >> 6) & 0x07];
            if ((operation.OpCodeValue & bwb) != 0)
            {
                Memory.Data[dstndx] = Memory.Data[srcndx];
            }
            else
            {
                srcvalw = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
                Memory.Data[dstndx++] = (byte)(srcvalw >> 8);
                Memory.Data[dstndx] = (byte)srcvalw;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Move Memory Reference to Register Reference
        /// </summary>
        public static void MovRegRefMemRef(Operation operation)
        {
            srcndx = Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3];
            dstndx = State.A[(operation.OpCodeValue >> 6) & 0x07];
            if ((operation.OpCodeValue & bwb) != 0)
            {
                Memory.Data[dstndx] = Memory.Data[srcndx];
            }
            else
            {
                srcvalw = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
                Memory.Data[dstndx++] = (byte)(srcvalw >> 8);
                Memory.Data[dstndx] = (byte)srcvalw;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Move Register Word to Memory Reference
        /// </summary>
        public static void MovMemRefRegDir(Operation operation)
        {
            srcvalw = State.A[(operation.OpCodeValue >> 2) & 0x07];
            dstndx = Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3];
            if ((operation.OpCodeValue & bwb) != 0)
            {
                Memory.Data[dstndx] = (byte)srcvalw;
            }
            else
            {
                Memory.Data[dstndx++] = (byte)(srcvalw >> 8);
                Memory.Data[dstndx] = (byte)srcvalw;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Move Immediate Word to Memory Reference
        /// </summary>
        public static void MovMemRefImd(Operation operation)
        {
            srcvalw = (ushort)(Memory.Data[State.PC + 4] << 8 | Memory.Data[State.PC + 5]);
            dstndx = Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3];
            Memory.Data[dstndx++] = (byte)(srcvalw >> 8);
            Memory.Data[dstndx] = (byte)srcvalw;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Move Memory Reference to Memory Reference
        /// </summary>
        public static void MovByteMemRefImd(Operation operation)
        {
            Memory.Data[Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]] = Memory.Data[State.PC + 5];
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Move Relative Immediate to Register
        /// </summary>
        public static void Gadr(Operation operation)
        {
            State.A[(operation.OpCodeValue >> 6) & 0x07] = (ushort)((Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]) + State.PC + operation.Length);
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Move Relative Register to Register
        /// </summary>
        public static void MovrRegDirRegRef(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07] + State.PC + operation.Length;
            if ((operation.OpCodeValue & bwb) == 0)
            {
                State.A[dstndx] = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
            }
            else
            {
                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= Memory.Data[srcndx];
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Move Relative Memory to Register
        /// </summary>
        public static void MovrRegDirMemRef(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcndx = (ushort)((Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]) + State.PC + operation.Length);
            if ((operation.OpCodeValue & bwb) == 0)
            {
                State.A[dstndx] = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
            }
            else
            {
                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= Memory.Data[srcndx];
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Move Register to Relative Memory
        /// </summary>
        public static void MovrMemRefRegDir(Operation operation)
        {
            srcvalw = State.A[(operation.OpCodeValue >> 2) & 0x07];
            dstndx = (ushort)((Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]) + State.PC + operation.Length);
            if ((operation.OpCodeValue & bwb) == 0)
            {
                Memory.Data[dstndx++] = (byte)(srcvalw >> 8);
                Memory.Data[dstndx] = (byte)srcvalw;
            }
            else
            {
                Memory.Data[dstndx] = (byte)srcvalw;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Move Relative Register to Relative Register
        /// </summary>
        public static void MovrRegRefRegRef(Operation operation)
        {
            dstndx = State.A[(operation.OpCodeValue >> 6) & 0x07] + State.PC + operation.Length;
            srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07] + State.PC + operation.Length;
            if ((operation.OpCodeValue & bwb) == 0)
            {
                Memory.Data[dstndx++] = Memory.Data[srcndx++];
            }
            Memory.Data[dstndx] = Memory.Data[srcndx];
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Move Register to Relative Register
        /// </summary>
        public static void MovrRegRefRegDir(Operation operation)
        {
            dstndx = State.A[(operation.OpCodeValue >> 6) & 0x07] + State.PC + operation.Length;
            srcvalw = State.A[(operation.OpCodeValue >> 2) & 0x07];
            if ((operation.OpCodeValue & bwb) == 0)
            {
                Memory.Data[dstndx++] = (byte)(srcvalw >> 8);
            }
            Memory.Data[dstndx] = (byte)srcvalw;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Move Immediate to Relative Register
        /// </summary>
        public static void MovrRegRefImd(Operation operation)
        {
            dstndx = State.A[(operation.OpCodeValue >> 6) & 0x07] + State.PC + operation.Length;
            srcvalw = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            if ((operation.OpCodeValue & bwb) == 0)
            {
                Memory.Data[dstndx++] = (byte)(srcvalw >> 8);
            }
            Memory.Data[dstndx] = (byte)srcvalw;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Move Tiny Immediate Word to Address
        /// </summary>
        public static void MoviRegDirImdQuick(Operation operation)
        {
            State.A[(operation.OpCodeValue >> 6) & 0x07] = (ushort)((operation.OpCodeValue >> 2) & 0x0f);
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Move Tiny Address Byte to Register
        /// </summary>
        public static void MoviByteRegDirImdQuick(Operation operation)
        {
            srcvalb = (byte)((operation.OpCodeValue >> 2) & 0x0f);
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            State.A[dstndx] &= 0x0ff00;
            State.A[dstndx] |= srcvalb;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Move Address High Byte to High Register Byte
        /// </summary>
        public static void MovhhRegDirRegDir(Operation operation)
        {
            valuew = (ushort)(State.A[(operation.OpCodeValue >> 2) & 0x07] & 0x0ff00);
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            State.A[dstndx] &= 0x0ff;
            State.A[dstndx] |= (ushort)valuew;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void MovhhRegDirImd(Operation operation)
        {
            valuew = (ushort)(Memory.Data[State.PC + 2] << 8);
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            State.A[dstndx] &= 0x0ff;
            State.A[dstndx] |= valuew;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void MovhhRegDirRegRef(Operation operation)
        {
            valuew = (ushort)(Memory.Data[State.A[(operation.OpCodeValue >> 2) & 0x07]] << 8);
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            State.A[dstndx] &= 0x0ff;
            State.A[dstndx] |= valuew;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void MovhhRegDirMemRef(Operation operation)
        {
            valuew = (ushort)(Memory.Data[Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]] << 8);
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            State.A[dstndx] &= 0x0ff;
            State.A[dstndx] |= valuew;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Move Address Low Byte to Register High Byte
        /// </summary>
        public static void MovhlRegDirRegDir(Operation operation)
        {
            valuew = (ushort)(State.A[(operation.OpCodeValue >> 2) & 0x07] << 8);
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            State.A[dstndx] &= 0x0ff;
            State.A[dstndx] |= (ushort)valuew;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void MovhlRegDirImd(Operation operation)
        {
            valuew = (ushort)(Memory.Data[State.PC + 3] << 8);
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            State.A[dstndx] &= 0x0ff;
            State.A[dstndx] |= valuew;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void MovhlRegDirRegRef(Operation operation)
        {
            valuew = (ushort)(Memory.Data[State.A[(operation.OpCodeValue >> 2) & 0x07]] << 8);
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            State.A[dstndx] &= 0x0ff;
            State.A[dstndx] |= valuew;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void MovhlRegDirMemRef(Operation operation)
        {
            valuew = (ushort)(Memory.Data[Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]] << 8);
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            State.A[dstndx] &= 0x0ff;
            State.A[dstndx] |= valuew;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Move Address High Byte to Register Low Byte
        /// </summary>
        public static void MovlhRegDirRegDir(Operation operation)
        {
            srcvalb = (byte)(State.A[(operation.OpCodeValue >> 2) & 0x07] >> 8);
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            State.A[dstndx] &= 0x0ff00;
            State.A[dstndx] |= srcvalb;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void MovlhRegDirImd(Operation operation)
        {
            srcvalb = Memory.Data[State.PC + 2];
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            State.A[dstndx] &= 0x0ff00;
            State.A[dstndx] |= srcvalb;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void MovlhRegDirRegRef(Operation operation)
        {
            srcvalb = Memory.Data[State.A[(operation.OpCodeValue >> 2) & 0x07]];
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            State.A[dstndx] &= 0x0ff00;
            State.A[dstndx] |= srcvalb;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void MovlhRegDirMemRef(Operation operation)
        {
            srcvalb = Memory.Data[Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]];
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            State.A[dstndx] &= 0x0ff00;
            State.A[dstndx] |= srcvalb;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void MovRegDirPc(Operation operation)
        {
            State.A[(operation.OpCodeValue >> 6) & 0x07] = State.PC;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void MovRegDirSp(Operation operation)
        {
            State.A[(operation.OpCodeValue >> 6) & 0x07] = State.SP;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Swap Register High - Low Byte
        /// </summary>
        public static void SwapRegDir(Operation operation)
        {
            valuew = State.A[(operation.OpCodeValue >> 6) & 0x07];
            State.A[(operation.OpCodeValue >> 6) & 0x07] = (ushort)((valuew << 8) | (valuew >> 8));
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Get Stack Pointer to Register
        /// </summary>
        public static void GetSpRegDir(Operation operation)
        {
            State.A[(operation.OpCodeValue >> 6) & 0x07] = State.SP;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Exchange Register with Register
        /// </summary>
        public static void XchgRegDirRegDir(Operation operation)
        {
            srcndx = (operation.OpCodeValue >> 2) & 0x07;
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            dstvalw = State.A[dstndx];
            State.A[dstndx] = State.A[srcndx];
            State.A[srcndx] = dstvalw;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Set Address Word to X Register
        /// </summary>
        public static void SetxRegDir(Operation operation)
        {
            State.X = State.A[(operation.OpCodeValue >> 2) & 0x07];
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void SetxImd(Operation operation)
        {
            State.X = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void SetxRegRef(Operation operation)
        {
            srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
            State.X = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void SetxMemRef(Operation operation)
        {
            valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            State.X = (ushort)(Memory.Data[valuew++] << 8 | Memory.Data[valuew]);
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Set Address Word to Stack Pointer
        /// </summary>
        public static void SetSpRegDir(Operation operation)
        {
            State.SP = State.A[(operation.OpCodeValue >> 2) & 0x07];
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void SetSpImd(Operation operation)
        {
            State.SP = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void SetSpRegRef(Operation operation)
        {
            srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
            State.SP = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void SetSpMemRef(Operation operation)
        {
            valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            State.SP = (ushort)(Memory.Data[valuew++] << 8 | Memory.Data[valuew]);
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Move Register Word to X Register
        /// </summary>
        //public static void MOVX(Operation operation)
        public static void MovxRegDir(Operation operation)
        {
            State.A[(operation.OpCodeValue >> 6) & 0x07] = State.X;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Pop Register from Stack
        /// </summary>
        public static void PopRegDir(Operation operation)
        {
            State.A[(operation.OpCodeValue >> 6) & 0x07] = PopFromStack();
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Push Register Direct to Stack
        /// </summary>
        public static void PushRegDir(Operation operation)
        {
            PushToStack(State.A[(operation.OpCodeValue >> 6) & 0x07]);
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Push Immediate to Stack
        /// </summary>
        public static void PushImd(Operation operation)
        {
            PushToStack((ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]));
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Push Register Reference to Stack
        /// </summary>
        public static void PushRegRef(Operation operation)
        {
            srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
            valuew = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
            PushToStack(valuew);
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Push Memory Reference to Stack
        /// </summary>
        public static void PushMemRef(Operation operation)
        {
            srcndx = Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3];
            valuew = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
            PushToStack(valuew);
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        #endregion

        #region Arithmetic-Logic Operations

        /// <summary>
        /// Add Register to Register
        /// </summary>
        public static void AddRegDirRegDir(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcvalw = State.A[(operation.OpCodeValue >> 2) & 0x07];
            dstvalw = State.A[dstndx];
            if ((operation.OpCodeValue & bwb) != 0)
            {
                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= AddAndSetFlagsByte(dstvalw, srcvalw, false, false);
            }
            else
            {
                State.A[dstndx] = AddAndSetFlagsWord(dstvalw, srcvalw, false, false);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Add Register to RegisterRef
        /// </summary>
        public static void AddRegRefRegDir(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcvalw = State.A[(operation.OpCodeValue >> 2) & 0x07];
            dstvalw = State.A[dstndx];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                valuew = (ushort)(Memory.Data[dstvalw]);
                result = AddAndSetFlagsByte(valuew, srcvalw, false, false);
                Memory.Data[dstvalw] = (byte)(result & 0x00FF);
            }
            else
            {
                valuew = (ushort)((Memory.Data[dstvalw] << 8) + Memory.Data[dstvalw + 1]);
                result = AddAndSetFlagsWord(valuew, srcvalw, false, false);
                Memory.Data[dstvalw] = (byte)(result >> 8);
                Memory.Data[dstvalw + 1] = (byte)(result & 0x00FF);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Add Immediate to RegisterRef
        /// </summary>
        public static void AddRegRefImd(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcvalw = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            dstvalw = State.A[dstndx];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                valuew = (ushort)(Memory.Data[dstvalw]);
                result = AddAndSetFlagsByte(valuew, srcvalw, false, false);
                Memory.Data[dstvalw] = (byte)(result & 0x00FF);
            }
            else
            {
                valuew = (ushort)((Memory.Data[dstvalw] << 8) + Memory.Data[dstvalw + 1]);
                result = AddAndSetFlagsWord(valuew, srcvalw, false, false);
                Memory.Data[dstvalw] = (byte)(result >> 8);
                Memory.Data[dstvalw + 1] = (byte)(result & 0x00FF);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Add Immediate to Register
        /// </summary>
        public static void AddRegDirImd(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcvalw = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            dstvalw = State.A[dstndx];
            if ((operation.OpCodeValue & bwb) != 0)
            {
                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= AddAndSetFlagsByte(dstvalw, srcvalw, false, false);
            }
            else
            {
                State.A[dstndx] = AddAndSetFlagsWord(dstvalw, srcvalw, false, false);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Add Immediate to Memory Referenced 
        /// </summary>
        public static void AddMemRefImd(Operation operation)
        {
            srcvalw = (ushort)(Memory.Data[State.PC + 4] << 8 | Memory.Data[State.PC + 5]);
            dstvalw = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);

            if ((operation.OpCodeValue & bwb) != 0)
            {
                valuew = (ushort)(Memory.Data[dstvalw]);
                result = AddAndSetFlagsByte(valuew, srcvalw, false, false);
                Memory.Data[dstvalw] = (byte)(result & 0x00FF);
            }
            else
            {
                valuew = (ushort)((Memory.Data[dstvalw] << 8) + Memory.Data[dstvalw + 1]);
                result = AddAndSetFlagsWord(valuew, srcvalw, false, false);
                Memory.Data[dstvalw] = (byte)(result >> 8);
                Memory.Data[dstvalw + 1] = (byte)(result & 0x00FF);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Add Register Direct to Memory Referenced 
        /// </summary>
        public static void AddMemRefRegDir(Operation operation)
        {
            srcvalw = State.A[(operation.OpCodeValue >> 2) & 0x07];
            dstvalw = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);

            if ((operation.OpCodeValue & bwb) != 0)
            {
                valuew = (ushort)(Memory.Data[dstvalw]);
                result = AddAndSetFlagsByte(valuew, srcvalw, false, false);
                Memory.Data[dstvalw] = (byte)(result & 0x00FF);
            }
            else
            {
                valuew = (ushort)((Memory.Data[dstvalw] << 8) + Memory.Data[dstvalw + 1]);
                result = AddAndSetFlagsWord(valuew, srcvalw, false, false);
                Memory.Data[dstvalw] = (byte)(result >> 8);
                Memory.Data[dstvalw + 1] = (byte)(result & 0x00FF);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Add Register Reference to Register
        /// </summary>
        public static void AddRegDirRegRef(Operation operation)
        {
            srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            dstvalw = State.A[dstndx];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= AddAndSetFlagsByte(dstvalw, Memory.Data[srcndx], false, false);
            }
            else
            {
                srcvalw = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
                State.A[dstndx] = AddAndSetFlagsWord(dstvalw, srcvalw, false, false);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Add Memory Reference to Register
        /// </summary>
        public static void AddRegDirMemRef(Operation operation)
        {
            srcndx = Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3];
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            dstvalw = State.A[dstndx];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= AddAndSetFlagsByte(dstvalw, Memory.Data[srcndx], false, false);
            }
            else
            {
                srcvalw = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
                State.A[dstndx] = AddAndSetFlagsWord(dstvalw, srcvalw, false, false);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Add Register Direct to Stack Pointer
        /// </summary>
        public static void AddSpRegDir(Operation operation)
        {
            dstvalw = State.SP;
            if ((operation.OpCodeValue & bwb) != 0)
            {
                State.SP = AddAndSetFlagsByte(dstvalw, State.A[(operation.OpCodeValue >> 6) & 0x07], false, false);
            }
            else
            {
                State.SP = AddAndSetFlagsWord(dstvalw, State.A[(operation.OpCodeValue >> 6) & 0x07], false, false);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Add Immediate to Stack Pointer
        /// </summary>
        public static void AddSpImd(Operation operation)
        {
            dstvalw = State.SP;
            if ((operation.OpCodeValue & bwb) != 0)
                State.SP = AddAndSetFlagsByte(dstvalw, Memory.Data[State.PC + 3], false, false);
            else
                State.SP = AddAndSetFlagsWord(dstvalw, (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]), false, false);
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Subtract Tiny Immediate Word from Address
        /// </summary>
        public static void SubiRegDirImdQuick(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcvalw = (ushort)((operation.OpCodeValue >> 2) & 0x0f);
            dstvalw = State.A[dstndx];
            State.A[dstndx] = AddAndSetFlagsWord(dstvalw, srcvalw, false, true);
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Sub Register from RegisterRef
        /// </summary>
        public static void SubRegRefRegDir(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcvalw = State.A[(operation.OpCodeValue >> 2) & 0x07];
            dstvalw = State.A[dstndx];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                valuew = (ushort)(Memory.Data[dstvalw]);
                result = AddAndSetFlagsByte(valuew, srcvalw, false, true);
                Memory.Data[dstvalw] = (byte)(result & 0x00FF);
            }
            else
            {
                valuew = (ushort)((Memory.Data[dstvalw] << 8) + Memory.Data[dstvalw + 1]);
                result = AddAndSetFlagsWord(valuew, srcvalw, false, true);
                Memory.Data[dstvalw] = (byte)(result >> 8);
                Memory.Data[dstvalw + 1] = (byte)(result & 0x00FF);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Sub Immediate from RegisterRef
        /// </summary>
        public static void SubRegRefImd(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcvalw = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            dstvalw = State.A[dstndx];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                valuew = (ushort)(Memory.Data[dstvalw]);
                result = AddAndSetFlagsByte(valuew, srcvalw, false, true);
                Memory.Data[dstvalw] = (byte)(result & 0x00FF);
            }
            else
            {
                valuew = (ushort)((Memory.Data[dstvalw] << 8) + Memory.Data[dstvalw + 1]);
                result = AddAndSetFlagsWord(valuew, srcvalw, false, true);
                Memory.Data[dstvalw] = (byte)(result >> 8);
                Memory.Data[dstvalw + 1] = (byte)(result & 0x00FF);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Sub Immediate from Memory Referenced 
        /// </summary>
        public static void SubMemRefImd(Operation operation)
        {
            srcvalw = (ushort)(Memory.Data[State.PC + 4] << 8 | Memory.Data[State.PC + 5]);
            dstvalw = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);

            if ((operation.OpCodeValue & bwb) != 0)
            {
                valuew = (ushort)(Memory.Data[dstvalw]);
                result = AddAndSetFlagsByte(valuew, srcvalw, false, true);
                Memory.Data[dstvalw] = (byte)(result & 0x00FF);
            }
            else
            {
                valuew = (ushort)((Memory.Data[dstvalw] << 8) + Memory.Data[dstvalw + 1]);
                result = AddAndSetFlagsWord(valuew, srcvalw, false, true);
                Memory.Data[dstvalw] = (byte)(result >> 8);
                Memory.Data[dstvalw + 1] = (byte)(result & 0x00FF);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Sub Register Direct from Memory Referenced 
        /// </summary>
        public static void SubMemRefRegDir(Operation operation)
        {
            srcvalw = State.A[(operation.OpCodeValue >> 2) & 0x07];
            dstvalw = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);

            if ((operation.OpCodeValue & bwb) != 0)
            {
                valuew = (ushort)(Memory.Data[dstvalw]);
                result = AddAndSetFlagsByte(valuew, srcvalw, false, true);
                Memory.Data[dstvalw] = (byte)(result & 0x00FF);
            }
            else
            {
                valuew = (ushort)((Memory.Data[dstvalw] << 8) + Memory.Data[dstvalw + 1]);
                result = AddAndSetFlagsWord(valuew, srcvalw, false, true);
                Memory.Data[dstvalw] = (byte)(result >> 8);
                Memory.Data[dstvalw + 1] = (byte)(result & 0x00FF);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Subtract Register Direct from Stack Pointer
        /// </summary>
        public static void SubSpRegDir(Operation operation)
        {
            dstvalw = State.SP;
            State.SP = AddAndSetFlagsWord(dstvalw, State.A[(operation.OpCodeValue >> 6) & 0x07], false, true);
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Subtract Immediate from Stack Pointer
        /// </summary>
        public static void SubSpImd(Operation operation)
        {
            dstvalw = State.SP;
            State.SP = AddAndSetFlagsWord(dstvalw, (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]), false, true);
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Add Tiny Immediate Word to Address
        /// </summary>
        public static void AddiRegDirImdQuick(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcvalw = (ushort)((operation.OpCodeValue >> 2) & 0x0f);
            dstvalw = State.A[dstndx];
            State.A[dstndx] = AddAndSetFlagsWord(dstvalw, srcvalw, false, false);
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Subtract Register from Register
        /// </summary>
        public static void SubRegDirRegDir(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcvalw = State.A[(operation.OpCodeValue >> 2) & 0x07];
            dstvalw = State.A[dstndx];
            if ((operation.OpCodeValue & bwb) != 0)
            {
                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= AddAndSetFlagsByte(dstvalw, srcvalw, false, true);
            }
            else
            {
                State.A[dstndx] = AddAndSetFlagsWord(dstvalw, srcvalw, false, true);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Subtract Immediate from Register
        /// </summary>
        public static void SubRegDirImd(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcvalw = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            dstvalw = State.A[dstndx];
            if ((operation.OpCodeValue & bwb) != 0)
            {
                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= AddAndSetFlagsByte(dstvalw, srcvalw, false, true);
            }
            else
            {
                State.A[dstndx] = AddAndSetFlagsWord(dstvalw, srcvalw, false, true);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Subtract Register Reference from Register
        /// </summary>
        public static void SubRegDirRegRef(Operation operation)
        {
            srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            dstvalw = State.A[dstndx];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= AddAndSetFlagsByte(dstvalw, Memory.Data[srcndx], false, true);
            }
            else
            {
                srcvalw = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
                State.A[dstndx] = AddAndSetFlagsWord(dstvalw, srcvalw, false, true);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Subtract Memory Reference from Register
        /// </summary>
        public static void SubRegDirMemRef(Operation operation)
        {
            srcndx = Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3];
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            dstvalw = State.A[dstndx];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= AddAndSetFlagsByte(dstvalw, Memory.Data[srcndx], false, true);
            }
            else
            {
                srcvalw = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
                State.A[dstndx] = AddAndSetFlagsWord(dstvalw, srcvalw, false, true);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Add with Carry Register to Register
        /// </summary>
        public static void AdcRegDirRegDir(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcvalw = State.A[(operation.OpCodeValue >> 2) & 0x07];
            dstvalw = State.A[dstndx];
            if ((operation.OpCodeValue & bwb) != 0)
            {
                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= AddAndSetFlagsByte(dstvalw, srcvalw, true, false);
            }
            else
            {
                State.A[dstndx] = AddAndSetFlagsWord(dstvalw, srcvalw, true, false);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Add with Carry Immediate to Register
        /// </summary>
        public static void AdcRegDirImd(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcvalw = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            dstvalw = State.A[dstndx];
            if ((operation.OpCodeValue & bwb) != 0)
            {
                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= AddAndSetFlagsByte(dstvalw, srcvalw, true, false);
            }
            else
            {
                State.A[dstndx] = AddAndSetFlagsWord(dstvalw, srcvalw, true, false);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Add with Carry Register Reference to Register
        /// </summary>
        public static void AdcRegDirRegRef(Operation operation)
        {
            srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            dstvalw = State.A[dstndx];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= AddAndSetFlagsByte(dstvalw, Memory.Data[srcndx], true, false);
            }
            else
            {
                srcvalw = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
                State.A[dstndx] = AddAndSetFlagsWord(dstvalw, srcvalw, true, false);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Add with Carry Memory Reference to Register
        /// </summary>
        public static void AdcRegDirMemRef(Operation operation)
        {
            srcndx = Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3];
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            dstvalw = State.A[dstndx];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= AddAndSetFlagsByte(dstvalw, Memory.Data[srcndx], true, false);
            }
            else
            {
                srcvalw = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
                State.A[dstndx] = AddAndSetFlagsWord(dstvalw, srcvalw, true, false);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Multiply Register to Register
        /// </summary>
        public static void MuluRegDirRegDir(Operation operation)
        {
            State.SR &= 0x0fff0;

            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcndx = (operation.OpCodeValue >> 2) & 0x07;
            srcvalw = State.A[srcndx];
            dstvalw = State.A[dstndx];
            if ((operation.OpCodeValue & bwb) != 0)
            {
                result = (byte)dstvalw * (byte)srcvalw;

                if ((result & wordAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & wordHiBitMask) == wordHiBitMask)
                    State.SR |= State.N;
                // o: src bit7, dst bit7, res bit15
                if (((dstvalw & byteHiBitMask) | (srcvalw & byteHiBitMask) | (ushort)(((result >> 8) ^ wordAllBitsMask) & wordHiBitMask)) == 0)
                    State.SR |= State.O;
                else if ((((dstvalw ^ byteAllBitsMask) & byteHiBitMask) | ((srcvalw ^ byteAllBitsMask) & byteHiBitMask) | (ushort)((result >> 16) & wordHiBitMask)) == 0)
                    State.SR |= State.O;

                State.A[dstndx] = (ushort)result;
            }
            else
            {
                result = dstvalw * srcvalw;

                if ((result & wordAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & 0x080000000) == 0x080000000)
                    State.SR |= State.N;
                // o: src bit15, dst bit15, res bit31
                if (((dstvalw & wordHiBitMask) | (srcvalw & wordHiBitMask) | (ushort)(((result >> 16) ^ wordAllBitsMask) & wordHiBitMask)) == 0)
                    State.SR |= State.O;
                else if ((((dstvalw ^ wordAllBitsMask) & wordHiBitMask) | ((srcvalw ^ wordAllBitsMask) & wordHiBitMask) | (ushort)((result >> 16) & wordHiBitMask)) == 0)
                    State.SR |= State.O;

                // low res in dest
                State.A[dstndx] = (ushort)result;
                // hi res only if reg to src reg
                State.A[srcndx] = (ushort)(result >> 16);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Multiply Immediate to Register
        /// </summary>
        public static void MuluRegDirImd(Operation operation)
        {
            State.SR &= 0x0fff0;

            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcvalw = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            dstvalw = State.A[dstndx];
            if ((operation.OpCodeValue & bwb) != 0)
            {
                result = (byte)dstvalw * (byte)srcvalw;

                if ((result & wordAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & wordHiBitMask) == wordHiBitMask)
                    State.SR |= State.N;
                // o: src bit7, dst bit7, res bit15
                if (((dstvalw & byteHiBitMask) | (srcvalw & byteHiBitMask) | (ushort)(((result >> 8) ^ wordAllBitsMask) & wordHiBitMask)) == 0)
                    State.SR |= State.O;
                else if ((((dstvalw ^ byteAllBitsMask) & byteHiBitMask) | ((srcvalw ^ byteAllBitsMask) & byteHiBitMask) | (ushort)((result >> 16) & wordHiBitMask)) == 0)
                    State.SR |= State.O;

                State.A[dstndx] = (ushort)result;
            }
            else
            {
                result = dstvalw * srcvalw;

                if ((result & wordAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & 0x080000000) == 0x080000000)
                    State.SR |= State.N;
                // o: src bit15, dst bit15, res bit31
                if (((dstvalw & wordHiBitMask) | (srcvalw & wordHiBitMask) | (ushort)(((result >> 16) ^ wordAllBitsMask) & wordHiBitMask)) == 0)
                    State.SR |= State.O;
                else if ((((dstvalw ^ wordAllBitsMask) & wordHiBitMask) | ((srcvalw ^ wordAllBitsMask) & wordHiBitMask) | (ushort)((result >> 16) & wordHiBitMask)) == 0)
                    State.SR |= State.O;

                // low res in dest
                State.A[dstndx] = (ushort)result;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Multiply Register Reference to Register
        /// </summary>
        public static void MuluRegDirRegRef(Operation operation)
        {
            State.SR &= 0x0fff0;

            srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            dstvalw = State.A[dstndx];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                srcvalw = Memory.Data[srcndx];
                result = (byte)dstvalw * (byte)srcvalw;

                if ((result & wordAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & wordHiBitMask) == wordHiBitMask)
                    State.SR |= State.N;
                // o: src bit7, dst bit7, res bit15
                if (((dstvalw & byteHiBitMask) | (srcvalw & byteHiBitMask) | (ushort)(((result >> 8) ^ wordAllBitsMask) & wordHiBitMask)) == 0)
                    State.SR |= State.O;
                else if ((((dstvalw ^ byteAllBitsMask) & byteHiBitMask) | ((srcvalw ^ byteAllBitsMask) & byteHiBitMask) | (ushort)((result >> 16) & wordHiBitMask)) == 0)
                    State.SR |= State.O;

                State.A[dstndx] = (ushort)result;
            }
            else
            {
                srcvalw = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
                result = dstvalw * srcvalw;

                if ((result & wordAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & 0x080000000) == 0x080000000)
                    State.SR |= State.N;
                // o: src bit15, dst bit15, res bit31
                if (((dstvalw & wordHiBitMask) | (srcvalw & wordHiBitMask) | (ushort)(((result >> 16) ^ wordAllBitsMask) & wordHiBitMask)) == 0)
                    State.SR |= State.O;
                else if ((((dstvalw ^ wordAllBitsMask) & wordHiBitMask) | ((srcvalw ^ wordAllBitsMask) & wordHiBitMask) | (ushort)((result >> 16) & wordHiBitMask)) == 0)
                    State.SR |= State.O;

                // low res in dest
                State.A[dstndx] = (ushort)result;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Multiply Memory Reference to Register
        /// </summary>
        public static void MuluRegDirMemRef(Operation operation)
        {
            State.SR &= 0x0fff0;

            srcndx = Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3];
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            dstvalw = State.A[dstndx];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                srcvalw = Memory.Data[srcndx];
                result = (byte)dstvalw * (byte)srcvalw;

                if ((result & wordAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & wordHiBitMask) == wordHiBitMask)
                    State.SR |= State.N;
                // o: src bit7, dst bit7, res bit15
                if (((dstvalw & byteHiBitMask) | (srcvalw & byteHiBitMask) | (ushort)(((result >> 8) ^ wordAllBitsMask) & wordHiBitMask)) == 0)
                    State.SR |= State.O;
                else if ((((dstvalw ^ byteAllBitsMask) & byteHiBitMask) | ((srcvalw ^ byteAllBitsMask) & byteHiBitMask) | (ushort)((result >> 16) & wordHiBitMask)) == 0)
                    State.SR |= State.O;

                State.A[dstndx] = (ushort)result;
            }
            else
            {
                srcvalw = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
                result = dstvalw * srcvalw;

                if ((result & wordAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & 0x080000000) == 0x080000000)
                    State.SR |= State.N;
                // o: src bit15, dst bit15, res bit31
                if (((dstvalw & wordHiBitMask) | (srcvalw & wordHiBitMask) | (ushort)(((result >> 16) ^ wordAllBitsMask) & wordHiBitMask)) == 0)
                    State.SR |= State.O;
                else if ((((dstvalw ^ wordAllBitsMask) & wordHiBitMask) | ((srcvalw ^ wordAllBitsMask) & wordHiBitMask) | (ushort)((result >> 16) & wordHiBitMask)) == 0)
                    State.SR |= State.O;

                // low res in dest
                State.A[dstndx] = (ushort)result;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Test Register Bit, result in Z Flag
        /// </summary>
        public static void BtstRegDirImdQuick(Operation operation)
        {
            result = (ushort)(State.A[(operation.OpCodeValue >> 6) & 0x07] & (1 << ((operation.OpCodeValue >> 2) & 0x0f)));
            State.SR &= (byte)(State.Z ^ byteAllBitsMask);
            if (result == 0)
                State.SR |= State.Z;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Test Register Bit, result in Z Flag
        /// </summary>
        public static void BtstRegDirRegDir(Operation operation)
        {
            result = (ushort)(State.A[(operation.OpCodeValue >> 6) & 0x07] & (1 << State.A[(operation.OpCodeValue >> 2) & 0x07]));
            State.SR &= (byte)(State.Z ^ byteAllBitsMask);
            if (result == 0)
                State.SR |= State.Z;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Set Register Bit
        /// </summary>
        public static void BsetRegDirImdQuick(Operation operation)
        {
            State.A[(operation.OpCodeValue >> 6) & 0x07] |= (ushort)(1 << ((operation.OpCodeValue >> 2) & 0x0f));
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Set Register Bit
        /// </summary>
        public static void BsetRegDirRegDir(Operation operation)
        {
            State.A[(operation.OpCodeValue >> 6) & 0x07] |= (ushort)(1 << State.A[(operation.OpCodeValue >> 2) & 0x07]);
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Clear Register Bit
        /// </summary>
        public static void BclrRegDirImdQuick(Operation operation)
        {
            State.A[(operation.OpCodeValue >> 6) & 0x07] &= (ushort)((1 << ((operation.OpCodeValue >> 2) & 0x0f)) ^ wordAllBitsMask);
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Clear Register Bit
        /// </summary>
        public static void BclrRegDirRegDir(Operation operation)
        {
            State.A[(operation.OpCodeValue >> 6) & 0x07] &= (ushort)((1 << State.A[(operation.OpCodeValue >> 2) & 0x07]) ^ wordAllBitsMask);
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Decrement Memory
        /// </summary>
        public static void DecMemRef(Operation operation)
        {
            dstndx = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            if ((operation.OpCodeValue & bwb) != 0)
            {
                Memory.Data[dstndx] = AddAndSetFlagsByte(Memory.Data[dstndx], 1, false, true);
            }
            else
            {
                dstvalw = AddAndSetFlagsWord((ushort)(Memory.Data[dstndx] << 8 | Memory.Data[dstndx + 1]), 1, false, true);
                Memory.Data[dstndx++] = (byte)(dstvalw >> 8);
                Memory.Data[dstndx] = (byte)dstvalw;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Increment Memory
        /// </summary>
        public static void IncMemRef(Operation operation)
        {
            dstndx = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            if ((operation.OpCodeValue & bwb) != 0)
            {
                Memory.Data[dstndx] = AddAndSetFlagsByte(Memory.Data[dstndx], 1, false, false);
            }
            else
            {
                dstvalw = AddAndSetFlagsWord((ushort)(Memory.Data[dstndx] << 8 | Memory.Data[dstndx + 1]), 1, false, false);
                Memory.Data[dstndx++] = (byte)(dstvalw >> 8);
                Memory.Data[dstndx] = (byte)dstvalw;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Increment Register
        /// </summary>
        public static void IncRegDir(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;

            if ((operation.OpCodeValue & bwb) != 0)
            {
                result = AddAndSetFlagsByte(State.A[dstndx], 1, false, false);
                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= (byte)result;
            }
            else
            {
                result = AddAndSetFlagsWord(State.A[dstndx], 1, false, false);
                State.A[dstndx] = (ushort)result;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Decrement Register
        /// </summary>
        public static void DecRegDir(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;

            if ((operation.OpCodeValue & bwb) != 0)
            {
                result = AddAndSetFlagsByte(State.A[dstndx], 1, false, true);
                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= (byte)result;
            }
            else
            {
                result = AddAndSetFlagsWord(State.A[dstndx], 1, false, true);
                State.A[dstndx] = (ushort)result;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Logic AND Register to Register
        /// </summary>
        public static void AndRegDirRegDir(Operation operation)
        {
            State.SR &= 0x0fff0;
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcvalw = State.A[(operation.OpCodeValue >> 2) & 0x07];
            dstvalw = State.A[dstndx];
            result = (ushort)(dstvalw & srcvalw);
            if ((operation.OpCodeValue & bwb) != 0)
            {
                if ((result & byteAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & byteHiBitMask) == byteHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= (byte)result;
            }
            else
            {
                if ((result & wordAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & wordHiBitMask) == wordHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] = (ushort)result;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Logic AND Immediate to Register
        /// </summary>
        public static void AndRegDirImd(Operation operation)
        {
            State.SR &= 0x0fff0;
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcvalw = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            dstvalw = State.A[dstndx];
            result = (ushort)(dstvalw & srcvalw);
            if ((operation.OpCodeValue & bwb) != 0)
            {
                if ((result & byteAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & byteHiBitMask) == byteHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= (byte)result;
            }
            else
            {
                if ((result & wordAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & wordHiBitMask) == wordHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] = (ushort)result;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Logic AND Register Reference to Register
        /// </summary>
        public static void AndRegDirRegRef(Operation operation)
        {
            State.SR &= 0x0fff0;
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
            dstvalw = State.A[dstndx];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                srcvalw = Memory.Data[srcndx];
                result = (byte)(dstvalw & srcvalw);

                if ((result & byteAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & byteHiBitMask) == byteHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= (ushort)result;
            }
            else
            {
                srcvalw = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
                result = (ushort)(dstvalw & srcvalw);

                if ((result & wordAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & wordHiBitMask) == wordHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] = (ushort)result;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Logic AND Memory Reference to Register
        /// </summary>
        public static void AndRegDirMemRef(Operation operation)
        {
            State.SR &= 0x0fff0;
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcndx = Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3];
            dstvalw = State.A[dstndx];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                srcvalw = Memory.Data[srcndx];
                result = (byte)(dstvalw & srcvalw);

                if ((result & byteAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & byteHiBitMask) == byteHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= (ushort)result;
            }
            else
            {
                srcvalw = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
                result = (ushort)(dstvalw & srcvalw);

                if ((result & wordAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & wordHiBitMask) == wordHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] = (ushort)result;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Logic OR Register to Register
        /// </summary>
        public static void OrRegDirRegDir(Operation operation)
        {
            State.SR &= 0x0fff0;
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcvalw = State.A[(operation.OpCodeValue >> 2) & 0x07];
            dstvalw = State.A[dstndx];
            result = (ushort)(dstvalw | srcvalw);
            if ((operation.OpCodeValue & bwb) != 0)
            {
                if ((result & byteAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & byteHiBitMask) == byteHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= (byte)result;
            }
            else
            {
                if ((result & wordAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & wordHiBitMask) == wordHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] = (ushort)result;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Logic OR Immediate to Register
        /// </summary>
        public static void OrRegDirImd(Operation operation)
        {
            State.SR &= 0x0fff0;
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcvalw = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            dstvalw = State.A[dstndx];
            result = (ushort)(dstvalw | srcvalw);
            if ((operation.OpCodeValue & bwb) != 0)
            {
                if ((result & byteAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & byteHiBitMask) == byteHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= (byte)result;
            }
            else
            {
                if ((result & wordAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & wordHiBitMask) == wordHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] = (ushort)result;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Logic OR Register Reference to Register
        /// </summary>
        public static void OrRegDirRegRef(Operation operation)
        {
            State.SR &= 0x0fff0;
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
            dstvalw = State.A[dstndx];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                srcvalw = Memory.Data[srcndx];
                result = (byte)(dstvalw | srcvalw);

                if ((result & byteAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & byteHiBitMask) == byteHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= (ushort)result;
            }
            else
            {
                srcvalw = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
                result = (ushort)(dstvalw | srcvalw);

                if ((result & wordAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & wordHiBitMask) == wordHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] = (ushort)result;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Logic OR Memory Reference to Register
        /// </summary>
        public static void OrRegDirMemRef(Operation operation)
        {
            State.SR &= 0x0fff0;
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcndx = Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3];
            dstvalw = State.A[dstndx];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                srcvalw = Memory.Data[srcndx];
                result = (byte)(dstvalw | srcvalw);

                if ((result & byteAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & byteHiBitMask) == byteHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= (ushort)result;
            }
            else
            {
                srcvalw = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
                result = (ushort)(dstvalw | srcvalw);

                if ((result & wordAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & wordHiBitMask) == wordHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] = (ushort)result;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Logic XOR Register to Register
        /// </summary>
        public static void XorRegDirRegDir(Operation operation)
        {
            State.SR &= 0x0fff0;
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcvalw = State.A[(operation.OpCodeValue >> 2) & 0x07];
            dstvalw = State.A[dstndx];
            result = (ushort)(dstvalw ^ srcvalw);
            if ((operation.OpCodeValue & bwb) != 0)
            {
                if ((result & byteAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & byteHiBitMask) == byteHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= (byte)result;
            }
            else
            {
                if ((result & wordAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & wordHiBitMask) == wordHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] = (ushort)result;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Logic XOR Immediate to Register
        /// </summary>
        public static void XorRegDirImd(Operation operation)
        {
            State.SR &= 0x0fff0;
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcvalw = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            dstvalw = State.A[dstndx];
            result = (ushort)(dstvalw ^ srcvalw);
            if ((operation.OpCodeValue & bwb) != 0)
            {
                if ((result & byteAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & byteHiBitMask) == byteHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= (byte)result;
            }
            else
            {
                if ((result & wordAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & wordHiBitMask) == wordHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] = (ushort)result;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Logic XOR Register Reference to Register
        /// </summary>
        public static void XorRegDirRegRef(Operation operation)
        {
            State.SR &= 0x0fff0;
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
            dstvalw = State.A[dstndx];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                srcvalw = Memory.Data[srcndx];
                result = (byte)(dstvalw ^ srcvalw);

                if ((result & byteAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & byteHiBitMask) == byteHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= (ushort)result;
            }
            else
            {
                srcvalw = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
                result = (ushort)(dstvalw ^ srcvalw);

                if ((result & wordAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & wordHiBitMask) == wordHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] = (ushort)result;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Logic XOR Memory Reference to Register
        /// </summary>
        public static void XorRegDirMemRef(Operation operation)
        {
            State.SR &= 0x0fff0;
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcndx = Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3];
            dstvalw = State.A[dstndx];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                srcvalw = Memory.Data[srcndx];
                result = (byte)(dstvalw ^ srcvalw);

                if ((result & byteAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & byteHiBitMask) == byteHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= (ushort)result;
            }
            else
            {
                srcvalw = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
                result = (ushort)(dstvalw ^ srcvalw);

                if ((result & wordAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & wordHiBitMask) == wordHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] = (ushort)result;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Logic NOT Register
        /// </summary>
        public static void NotRegDir(Operation operation)
        {
            State.SR &= 0x0fff0;
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            result = (ushort)(State.A[dstndx] ^ wordAllBitsMask);
            if ((operation.OpCodeValue & bwb) != 0)
            {
                if ((result & byteAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & byteHiBitMask) == byteHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= (byte)result;
            }
            else
            {
                if ((result & wordAllBitsMask) == 0)
                    State.SR |= State.Z;
                else if ((result & wordHiBitMask) == wordHiBitMask)
                    State.SR |= State.N;

                State.A[dstndx] = (ushort)result;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Negate Register
        /// </summary>
        public static void NegRegDir(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            dstvalw = (ushort)(State.A[dstndx] ^ wordAllBitsMask);

            if ((operation.OpCodeValue & bwb) != 0)
            {
                result = AddAndSetFlagsByte(dstvalw, 1, false, false);
                State.A[dstndx] &= 0x0ff00;
                State.A[dstndx] |= (ushort)result;
            }
            else
            {
                result = AddAndSetFlagsWord(dstvalw, 1, false, false);
                State.A[dstndx] = (ushort)result;
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Rotate Left Register Word Bits
        /// </summary>
        public static void RolRegDirImdQuick(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            dstvalw = State.A[dstndx];
            srcvalw = (ushort)((operation.OpCodeValue >> 2) & 0x0f);

            uint bigrol = (uint)(((dstvalw << 16) | dstvalw) << srcvalw);
            result = (ushort)(bigrol >> 16);

            State.SR &= 0x0fff0;

            if ((result & wordAllBitsMask) == 0)
                State.SR |= State.Z;
            else if ((result & wordHiBitMask) == wordHiBitMask)
                State.SR |= State.N;

            State.A[dstndx] = (ushort)result;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Arithmetic Shift Right Register Word Bits
        /// </summary>
        public static void SraRegDirImdQuick(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            dstvalw = State.A[dstndx];
            srcvalw = (ushort)((operation.OpCodeValue >> 2) & 0x0f);

            result = (ushort)(dstvalw >> srcvalw);

            if ((dstvalw & wordHiBitMask) != 0)
                result = (ushort)(result | ((wordAllBitsMask & ((1 << (16 - srcvalw)) - 1)) ^ wordAllBitsMask));

            State.SR &= 0x0fff0;

            if ((dstvalw & (1 << (srcvalw - 1))) != 0)
                State.SR |= State.C;
            if ((result & wordAllBitsMask) == 0)
                State.SR |= State.Z;
            else if ((result & wordHiBitMask) == wordHiBitMask)
                State.SR |= State.N;

            State.A[dstndx] = (ushort)result;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Arithmetic Shift Left Register Word Bits
        /// </summary>
        public static void SlaRegDirImdQuick(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            dstvalw = State.A[dstndx];
            srcvalw = (ushort)((operation.OpCodeValue >> 2) & 0x0f);

            result = (ushort)(dstvalw << srcvalw);

            State.SR &= 0x0fff0;

            if ((dstvalw & (1 << (16 - srcvalw))) != 0)
                State.SR |= State.C;
            if ((result & wordAllBitsMask) == 0)
                State.SR |= State.Z;
            else if ((result & wordHiBitMask) == wordHiBitMask)
                State.SR |= State.N;

            State.A[dstndx] = (ushort)result;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Logical Shift Right Register Word Bits
        /// </summary>
        public static void SrlRegDirImdQuick(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            dstvalw = State.A[dstndx];
            srcvalw = (ushort)((operation.OpCodeValue >> 2) & 0x0f);

            result = (ushort)(dstvalw >> srcvalw);

            State.SR &= 0x0fff0;

            if ((dstvalw & (1 << (srcvalw - 1))) != 0)
                State.SR |= State.C;
            if ((result & wordAllBitsMask) == 0)
                State.SR |= State.Z;
            else if ((result & wordHiBitMask) == wordHiBitMask)
                State.SR |= State.N;

            State.A[dstndx] = (ushort)result;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Logical Shift Right Long Register DWord Bits
        /// </summary>
        public static void SrllRegDirRegDir(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcndx = (operation.OpCodeValue >> 2) & 0x07;
            dstvalw = State.A[dstndx];
            srcvalw = State.A[srcndx];

            resultlong = ((dstvalw << 16) + srcvalw) >> 1;

            State.SR &= 0x0fff0;
            State.A[dstndx] = (ushort)(resultlong >> 16);
            State.A[srcndx] = (ushort)(resultlong & 0x0000FFFF);

            if ((srcvalw & 1) != 0)
                State.SR |= State.C;
            if ((resultlong) == 0)
                State.SR |= State.Z;
            else if ((State.A[dstndx] & wordHiBitMask) == wordHiBitMask)
                State.SR |= State.N;

            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Logical Shift Right Register Byte Bits
        /// </summary>
        public static void SrlByteRegDirImdQuick(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            dstvalb = (byte)State.A[dstndx];
            srcvalb = (byte)((operation.OpCodeValue >> 2) & 0x0f);

            result = (ushort)(dstvalb >> srcvalb);

            State.SR &= 0x0fff0;

            if ((dstvalb & (1 << (srcvalb - 1))) != 0)
                State.SR |= State.C;
            if ((result & byteAllBitsMask) == 0)
                State.SR |= State.Z;
            else if ((result & byteHiBitMask) == byteHiBitMask)
                State.SR |= State.N;

            State.A[dstndx] &= 0x0ff00;
            State.A[dstndx] |= (ushort)result;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Logical Shift Left Register Word Bits
        /// </summary>
        public static void SllRegDirImdQuick(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            dstvalw = State.A[dstndx];
            srcvalw = (ushort)((operation.OpCodeValue >> 2) & 0x0f);

            result = (ushort)(dstvalw << srcvalw);

            State.SR &= 0x0fff0;

            if ((dstvalw & (1 << (16 - srcvalw))) != 0)
                State.SR |= State.C;
            if ((result & wordAllBitsMask) == 0)
                State.SR |= State.Z;
            else if ((result & wordHiBitMask) == wordHiBitMask)
                State.SR |= State.N;

            State.A[dstndx] = (ushort)result;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Logical Shift Left Long Register DWord Bits
        /// </summary>
        public static void SlllRegDirRegDir(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            srcndx = (operation.OpCodeValue >> 2) & 0x07;
            dstvalw = State.A[dstndx];
            srcvalw = State.A[srcndx];

            resultlong = ((dstvalw << 16) + srcvalw) << 1;

            State.SR &= 0x0fff0;
            State.A[dstndx] = (ushort)(resultlong >> 16);
            State.A[srcndx] = (ushort)(resultlong & 0x0000FFFF);

            if ((dstvalw & (1 << (16 - 1))) != 0)
                State.SR |= State.C;
            if ((resultlong) == 0)
                State.SR |= State.Z;
            else if ((State.A[dstndx] & wordHiBitMask) == wordHiBitMask)
                State.SR |= State.N;

            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Logical Shift Left Register Byte Bits
        /// </summary>
        public static void SllByteRegDirImdQuick(Operation operation)
        {
            dstndx = (operation.OpCodeValue >> 6) & 0x07;
            dstvalb = (byte)State.A[dstndx];
            srcvalb = (byte)((operation.OpCodeValue >> 2) & 0x0f);

            result = (ushort)(dstvalb << srcvalb);

            State.SR &= 0x0fff0;

            if ((dstvalb & (1 << (8 - srcvalb))) != 0)
                State.SR |= State.C;
            if ((result & byteAllBitsMask) == 0)
                State.SR |= State.Z;
            else if ((result & byteHiBitMask) == byteHiBitMask)
                State.SR |= State.N;

            State.A[dstndx] &= 0x0ff00;
            State.A[dstndx] |= (ushort)result;
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Clear Status Register Bit (bwb=1: Set)
        /// </summary>
        public static void SrClr(Operation operation)
        {
            if ((operation.OpCodeValue & bwb) == 0)
            {

                State.SR &= (byte)((1 << ((operation.OpCodeValue >> 2) & 0x07)) ^ 0x0ff);
            }
            else
            {
                State.SR |= (byte)(1 << (operation.OpCodeValue >> 2) & 0x07);
            }

            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        #endregion

        #region Conditional Operations

        /// <summary>
        /// Move Register Word to Memory Reference
        /// </summary>
        public static void CmpMemRefRegDir(Operation operation)
        {
            dstndx = Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                result = AddAndSetFlagsByte(Memory.Data[dstndx], State.A[(operation.OpCodeValue >> 2) & 0x07], false, true);
            }
            else
            {
                result = AddAndSetFlagsWord((ushort)(Memory.Data[dstndx++] << 8 | Memory.Data[dstndx]), State.A[(operation.OpCodeValue >> 2) & 0x07], false, true);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Move Immediate Word to Memory Reference
        /// </summary>
        public static void CmpMemRefImd(Operation operation)
        {
            dstndx = Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                result = AddAndSetFlagsByte(Memory.Data[dstndx], Memory.Data[State.PC + 5], false, true);
            }
            else
            {
                result = AddAndSetFlagsWord((ushort)(Memory.Data[dstndx++] << 8 | Memory.Data[dstndx]), (ushort)(Memory.Data[State.PC + 4] << 8 | Memory.Data[State.PC + 5]), false, true);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Compare Register to Register
        /// </summary>
        public static void CmpRegDirRegDir(Operation operation)
        {
            dstvalw = State.A[(operation.OpCodeValue >> 6) & 0x07];
            srcvalw = State.A[(operation.OpCodeValue >> 2) & 0x07];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                result = AddAndSetFlagsByte(dstvalw, srcvalw, false, true);
            }
            else
            {
                result = AddAndSetFlagsWord(dstvalw, srcvalw, false, true);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Compare Immediate to Register
        /// </summary>
        public static void CmpRegDirImd(Operation operation)
        {
            dstvalw = State.A[(operation.OpCodeValue >> 6) & 0x07];
            //srcvalw = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);

            if ((operation.OpCodeValue & bwb) != 0)
            {
                result = AddAndSetFlagsByte(dstvalw, Memory.Data[State.PC + 3], false, true);
            }
            else
            {
                result = AddAndSetFlagsWord(dstvalw, (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]), false, true);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Compare Register Reference to Register
        /// </summary>
        public static void CmpRegDirRegRef(Operation operation)
        {
            dstvalw = State.A[(operation.OpCodeValue >> 6) & 0x07];
            srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                srcvalw = Memory.Data[srcndx];
                result = AddAndSetFlagsByte(dstvalw, srcvalw, false, true);
            }
            else
            {
                srcvalw = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
                result = AddAndSetFlagsWord(dstvalw, srcvalw, false, true);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Compare Memory Reference to Register
        /// </summary>
        public static void CmpRegDirMemRef(Operation operation)
        {
            dstvalw = State.A[(operation.OpCodeValue >> 6) & 0x07];
            srcndx = Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                srcvalw = Memory.Data[srcndx];
                result = AddAndSetFlagsByte(dstvalw, srcvalw, false, true);
            }
            else
            {
                srcvalw = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
                result = AddAndSetFlagsWord(dstvalw, srcvalw, false, true);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Compare Register to Register Reference
        /// </summary>
        public static void CmpRegRefRegDir(Operation operation)
        {
            dstndx = State.A[(operation.OpCodeValue >> 6) & 0x07];
            srcvalw = State.A[(operation.OpCodeValue >> 2) & 0x07];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                dstvalw = Memory.Data[dstndx];
                result = AddAndSetFlagsByte(dstvalw, srcvalw, false, true);
            }
            else
            {
                dstvalw = (ushort)(Memory.Data[dstndx++] << 8 | Memory.Data[dstndx]);
                result = AddAndSetFlagsWord(dstvalw, srcvalw, false, true);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Compare Immediate to Register Reference
        /// </summary>
        public static void CmpRegRefImd(Operation operation)
        {
            dstndx = State.A[(operation.OpCodeValue >> 6) & 0x07];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                result = AddAndSetFlagsByte(Memory.Data[dstndx], Memory.Data[State.PC + 3], false, true);
            }
            else
            {
                //dstvalw = (ushort)(Memory.Data[dstndx++] << 8 | Memory.Data[dstndx]);
                result = AddAndSetFlagsWord((ushort)(Memory.Data[dstndx++] << 8 | Memory.Data[dstndx]), (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]), false, true);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Compare Register Reference to Register Reference
        /// </summary>
        public static void CmpRegRefRegRef(Operation operation)
        {
            dstndx = State.A[(operation.OpCodeValue >> 6) & 0x07];
            srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];

            if ((operation.OpCodeValue & bwb) != 0)
            {
                result = AddAndSetFlagsByte(Memory.Data[dstndx], Memory.Data[srcndx], false, true);
            }
            else
            {
                result = AddAndSetFlagsWord((ushort)(Memory.Data[dstndx++] << 8 | Memory.Data[dstndx]), (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]), false, true);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Compare Tiny Immediate to Register Byte
        /// </summary>
        public static void CmpiByteRegDirImdQuick(Operation operation)
        {
            result = AddAndSetFlagsByte(State.A[(operation.OpCodeValue >> 6) & 0x07], (ushort)((operation.OpCodeValue >> 2) & 0x0f), false, true);
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Compare Tiny Immediate to Register Word
        /// </summary>
        public static void CmpiRegDirImdQuick(Operation operation)
        {
            result = AddAndSetFlagsWord(State.A[(operation.OpCodeValue >> 6) & 0x07], (ushort)((operation.OpCodeValue >> 2) & 0x0f), false, true);
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Compare Register Low Byte to Register High Byte
        /// </summary>
        public static void CmphRegDirRegDir(Operation operation)
        {
            dstvalb = (byte)(State.A[(operation.OpCodeValue >> 6) & 0x07] >> 8);
            srcvalb = (byte)(State.A[(operation.OpCodeValue >> 2) & 0x07]);
            //srcvalb = (byte)(State.A[(operation.OpCodeValue >> 2) & 0x07] >> 8);
            result = AddAndSetFlagsByte(dstvalb, srcvalb, false, true);
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Compare Immediate Low Byte to Register High Byte
        /// </summary>
        public static void CmphRegDirImd(Operation operation)
        {
            dstvalb = (byte)(State.A[(operation.OpCodeValue >> 6) & 0x07] >> 8);
            srcvalb = Memory.Data[State.PC];
            //srcvalb = Memory.Data[State.PC + 2];
            result = AddAndSetFlagsByte(dstvalb, srcvalb, false, true);
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Compare Register Reference Low Byte to Register High Byte
        /// </summary>
        public static void CmphRegDirRegRef(Operation operation)
        {
            dstvalb = (byte)(State.A[(operation.OpCodeValue >> 6) & 0x07] >> 8);
            srcvalb = Memory.Data[State.A[(operation.OpCodeValue >> 2) & 0x07]];
            //srcvalb = Memory.Data[(State.A[(operation.OpCodeValue >> 2) & 0x07]) - 1];
            result = AddAndSetFlagsByte(dstvalb, srcvalb, false, true);
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Compare Memory Reference Low Byte to Register High Byte
        /// </summary>
        public static void CmphRegDirMemRef(Operation operation)
        {
            dstvalb = (byte)(State.A[(operation.OpCodeValue >> 6) & 0x07] >> 8);
            srcndx = Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3];
            srcvalb = Memory.Data[srcndx];
            //srcvalb = Memory.Data[srcndx - 1];
            result = AddAndSetFlagsByte(dstvalb, srcvalb, false, true);
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        #endregion

        #region Branch Operations

        /// <summary>
        /// Jump on Not Zero X and Decrement, to Register Value
        /// </summary>
        public static void JmpxRegDir(Operation operation)
        {
            if (State.X != 0)
            {
                State.PC = State.A[(operation.OpCodeValue >> 2) & 0x07];
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
            State.X--;
        }

        /// <summary>
        /// Jump on Not Zero X and Decrement, to Immediate Value
        /// </summary>
        public static void JmpxImd(Operation operation)
        {
            if (State.X != 0)
            {
                State.PC = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
            State.X--;
        }

        /// <summary>
        /// Jump on Not Zero X and Decrement, to Register Reference
        /// </summary>
        public static void JmpxRegRef(Operation operation)
        {
            if (State.X != 0)
            {
                srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
                State.PC = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
            State.X--;
        }

        /// <summary>
        /// Jump on Not Zero X and Decrement, to Memory
        /// </summary>
        public static void JmpxMemRef(Operation operation)
        {
            if (State.X != 0)
            {
                valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                State.PC = (ushort)(Memory.Data[valuew++] << 8 | Memory.Data[valuew]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
            State.X--;
        }

        /// <summary>
        /// Inc/Dec Ax, Jump on Not Zero X and Decrement, to Register Value
        /// </summary>
        public static void JxaRegDir(Operation operation)
        {
            if (State.X != 0)
            {
                dstndx = (operation.OpCodeValue >> 6) & 0x07;
                dstvalw = State.A[dstndx];
                if ((operation.OpCodeValue & bwb) == 0)
                    if ((State.SR & State.D) == 0)
                        dstvalw += 2;
                    else
                        dstvalw -= 2;
                else
                    if ((State.SR & State.D) == 0)
                    dstvalw += 1;
                else
                    dstvalw -= 1;
                State.A[dstndx] = dstvalw;
                State.PC = State.A[(operation.OpCodeValue >> 2) & 0x07];
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
            State.X--;
        }

        /// <summary>
        /// Inc/Dec Ax, Jump on Not Zero X and Decrement, to Immediate Value
        /// </summary>
        public static void JxaImd(Operation operation)
        {
            if (State.X != 0)
            {
                dstndx = (operation.OpCodeValue >> 6) & 0x07;
                dstvalw = State.A[dstndx];
                if ((operation.OpCodeValue & bwb) == 0)
                    if ((State.SR & State.D) == 0)
                        dstvalw += 2;
                    else
                        dstvalw -= 2;
                else
                    if ((State.SR & State.D) == 0)
                    dstvalw += 1;
                else
                    dstvalw -= 1;
                State.A[dstndx] = dstvalw;
                State.PC = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
            State.X--;
        }

        /// <summary>
        /// Inc/Dec Ax, Jump on Not Zero X and Decrement, to Register Reference
        /// </summary>
        public static void JxaRegRef(Operation operation)
        {
            if (State.X != 0)
            {
                dstndx = (operation.OpCodeValue >> 6) & 0x07;
                dstvalw = State.A[dstndx];
                if ((operation.OpCodeValue & bwb) == 0)
                    if ((State.SR & State.D) == 0)
                        dstvalw += 2;
                    else
                        dstvalw -= 2;
                else
                    if ((State.SR & State.D) == 0)
                    dstvalw += 1;
                else
                    dstvalw -= 1;
                State.A[dstndx] = dstvalw;
                srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
                State.PC = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
            State.X--;
        }

        /// <summary>
        /// Inc/Dec Ax, Jump on Not Zero X and Decrement, to Memory
        /// </summary>
        public static void JxaMemRef(Operation operation)
        {
            if (State.X != 0)
            {
                dstndx = (operation.OpCodeValue >> 6) & 0x07;
                dstvalw = State.A[dstndx];
                if ((operation.OpCodeValue & bwb) == 0)
                    if ((State.SR & State.D) == 0)
                        dstvalw += 2;
                    else
                        dstvalw -= 2;
                else
                    if ((State.SR & State.D) == 0)
                    dstvalw += 1;
                else
                    dstvalw -= 1;
                State.A[dstndx] = dstvalw;
                valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                State.PC = (ushort)(Memory.Data[valuew++] << 8 | Memory.Data[valuew]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
            State.X--;
        }

        /// <summary>
        /// Jump on Above or Equal to Register Value
        /// </summary>
        public static void JaeRegDir(Operation operation)
        {
            // c=0 or z=1
            if ((State.SR & State.C) == 0 || (State.SR & State.Z) != 0)
            {
                State.PC = State.A[(operation.OpCodeValue >> 2) & 0x07];
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Above or Equal to Immediate Value
        /// </summary>
        public static void JaeImd(Operation operation)
        {
            // c=0 or z=1
            if ((State.SR & State.C) == 0 || (State.SR & State.Z) != 0)
            {
                State.PC = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Above or Equal to Register Reference
        /// </summary>
        public static void JaeRegRef(Operation operation)
        {
            // c=0 or z=1
            if ((State.SR & State.C) == 0 || (State.SR & State.Z) != 0)
            {
                srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
                State.PC = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Above or Equal to Memory
        /// </summary>
        public static void JaeMemRef(Operation operation)
        {
            // c=0 or z=1
            if ((State.SR & State.C) == 0 || (State.SR & State.Z) != 0)
            {
                valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                State.PC = (ushort)(Memory.Data[valuew++] << 8 | Memory.Data[valuew]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump to Register Value
        /// </summary>
        public static void JmpRegDir(Operation operation)
        {
            State.PC = State.A[(operation.OpCodeValue >> 2) & 0x07];
        }

        /// <summary>
        /// Jump to Immediate Value
        /// </summary>
        public static void JmpImd(Operation operation)
        {
            State.PC = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
        }

        /// <summary>
        /// Jump to Register Reference
        /// </summary>
        public static void JmpRegRef(Operation operation)
        {
            srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
            State.PC = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
        }

        /// <summary>
        /// Jump to Memory
        /// </summary>
        public static void JmpMemRef(Operation operation)
        {
            valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            State.PC = (ushort)(Memory.Data[valuew++] << 8 | Memory.Data[valuew]);
        }

        /// <summary>
        /// Jump on not Zero to Register Value
        /// </summary>
        public static void JnzRegDir(Operation operation)
        {
            if ((State.SR & State.Z) == ((operation.OpCodeValue & bwb) >> 3))
            {
                State.PC = State.A[(operation.OpCodeValue >> 2) & 0x07];
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on not Zero to Immediate Value
        /// </summary>
        public static void JnzImd(Operation operation)
        {
            if ((State.SR & State.Z) == ((operation.OpCodeValue & bwb) >> 3))
            {
                State.PC = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on not Zero to Register Reference
        /// </summary>
        public static void JnzRegRef(Operation operation)
        {
            if ((State.SR & State.Z) == ((operation.OpCodeValue & bwb) >> 3))
            {
                srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
                State.PC = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on not Zero to Memory
        /// </summary>
        public static void JnzMemRef(Operation operation)
        {
            if ((State.SR & State.Z) == ((operation.OpCodeValue & bwb) >> 3))
            {
                valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                State.PC = (ushort)(Memory.Data[valuew++] << 8 | Memory.Data[valuew]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on not Overflow to Register Value
        /// </summary>
        public static void JnoRegDir(Operation operation)
        {
            if ((State.SR & State.O) == ((operation.OpCodeValue & bwb) >> 4))
            {
                State.PC = State.A[(operation.OpCodeValue >> 2) & 0x07];
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on not Overflow to Immediate Value
        /// </summary>
        public static void JnoImd(Operation operation)
        {
            if ((State.SR & State.O) == ((operation.OpCodeValue & bwb) >> 4))
            {
                State.PC = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on not Overflow to Register Reference
        /// </summary>
        public static void JnoRegRef(Operation operation)
        {
            if ((State.SR & State.O) == ((operation.OpCodeValue & bwb) >> 4))
            {
                srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
                State.PC = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on not Overflow to Memory
        /// </summary>
        public static void JnoMemRef(Operation operation)
        {
            if ((State.SR & State.O) == ((operation.OpCodeValue & bwb) >> 4))
            {
                valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                State.PC = (ushort)(Memory.Data[valuew++] << 8 | Memory.Data[valuew]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on not Carry to Register Value
        /// </summary>
        public static void JncRegDir(Operation operation)
        {
            if ((State.SR & State.C) == ((operation.OpCodeValue & bwb) >> 5))
            {
                State.PC = State.A[(operation.OpCodeValue >> 2) & 0x07];
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on not Carry to Immediate Value
        /// </summary>
        public static void JncImd(Operation operation)
        {
            if ((State.SR & State.C) == ((operation.OpCodeValue & bwb) >> 5))
            {
                State.PC = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on not Carry to Register Reference
        /// </summary>
        public static void JncRegRef(Operation operation)
        {
            if ((State.SR & State.C) == ((operation.OpCodeValue & bwb) >> 5))
            {
                srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
                State.PC = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Carry or not Carry to Memory (bwb=0:JNC, bwb=1:JC)
        /// </summary>
        public static void JncMemRef(Operation operation)
        {
            if ((State.SR & State.C) == ((operation.OpCodeValue & bwb) >> 5))
            {
                valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                State.PC = (ushort)(Memory.Data[valuew++] << 8 | Memory.Data[valuew]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Positive to Register Value
        /// </summary>
        public static void JpRegDir(Operation operation)
        {
            if ((State.SR & State.N) == ((operation.OpCodeValue & bwb) >> 2))
            {
                State.PC = State.A[(operation.OpCodeValue >> 2) & 0x07];
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Positive to Immediate Value
        /// </summary>
        public static void JpImd(Operation operation)
        {
            if ((State.SR & State.N) == ((operation.OpCodeValue & bwb) >> 2))
            {
                State.PC = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Positive to Register Reference
        /// </summary>
        public static void JpRegRef(Operation operation)
        {
            if ((State.SR & State.N) == ((operation.OpCodeValue & bwb) >> 2))
            {
                srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
                State.PC = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Positive to Memory
        /// </summary>
        public static void JpMemRef(Operation operation)
        {
            if ((State.SR & State.N) == ((operation.OpCodeValue & bwb) >> 2))
            {
                valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                State.PC = (ushort)(Memory.Data[valuew++] << 8 | Memory.Data[valuew]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Below or Equal to Register Value
        /// </summary>
        public static void JbeRegDir(Operation operation)
        {
            // c=1 or z=1 
            if (((State.SR & State.C) | (State.SR & State.Z)) != 0)
            {
                State.PC = State.A[(operation.OpCodeValue >> 2) & 0x07];
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Below or Equal to Immediate Value
        /// </summary>
        public static void JbeImd(Operation operation)
        {
            // c=1 or z=1 
            if (((State.SR & State.C) | (State.SR & State.Z)) != 0)
            {
                State.PC = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Below or Equal to Register Reference
        /// </summary>
        public static void JbeRegRef(Operation operation)
        {
            // c=1 or z=1 
            if (((State.SR & State.C) | (State.SR & State.Z)) != 0)
            {
                srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
                State.PC = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Below or Equal to Memory
        /// </summary>
        public static void JbeMemRef(Operation operation)
        {
            // c=1 or z=1 
            if (((State.SR & State.C) | (State.SR & State.Z)) != 0)
            {
                valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                State.PC = (ushort)(Memory.Data[valuew++] << 8 | Memory.Data[valuew]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Above to Register Value
        /// </summary>
        public static void JaRegDir(Operation operation)
        {
            // 	c = 0 and z = 0
            if ((State.SR & State.C) == 0 && (State.SR & State.Z) == 0)
            {
                State.PC = State.A[(operation.OpCodeValue >> 2) & 0x07];
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Above to Immediate Value
        /// </summary>
        public static void JaImd(Operation operation)
        {
            // 	c = 0 and z = 0
            if ((State.SR & State.C) == 0 && (State.SR & State.Z) == 0)
            {
                State.PC = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Above to Register Reference
        /// </summary>
        public static void JaRegRef(Operation operation)
        {
            // 	c = 0 and z = 0
            if ((State.SR & State.C) == 0 && (State.SR & State.Z) == 0)
            {
                srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
                State.PC = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Above to Memory
        /// </summary>
        public static void JaMemRef(Operation operation)
        {
            // 	c = 0 and z = 0
            if ((State.SR & State.C) == 0 && (State.SR & State.Z) == 0)
            {
                valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                State.PC = (ushort)(Memory.Data[valuew++] << 8 | Memory.Data[valuew]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Less to Register Value
        /// </summary>
        public static void JlRegDir(Operation operation)
        {
            // z=0 and n<>o
            if ((State.SR & State.Z) == 0 && (State.SR & State.N) != ((State.SR & State.O) << 2))
            {
                State.PC = State.A[(operation.OpCodeValue >> 2) & 0x07];
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Less to Immediate Value
        /// </summary>
        public static void JlImd(Operation operation)
        {
            // z=0 and n<>o
            if ((State.SR & State.Z) == 0 && (State.SR & State.N) != ((State.SR & State.O) << 2))
            {
                State.PC = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Less to Register Reference
        /// </summary>
        public static void JlRegRef(Operation operation)
        {
            // z=0 and n<>o
            if ((State.SR & State.Z) == 0 && (State.SR & State.N) != ((State.SR & State.O) << 2))
            {
                srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
                State.PC = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Less to Memory
        /// </summary>
        public static void JlMemRef(Operation operation)
        {
            // z=0 and n<>o
            if ((State.SR & State.Z) == 0 && (State.SR & State.N) != ((State.SR & State.O) << 2))
            {
                valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                State.PC = (ushort)(Memory.Data[valuew++] << 8 | Memory.Data[valuew]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Subroutine to Register Value
        /// </summary>
        public static void JsrRegDir(Operation operation)
        {
            PushToStack((ushort)(State.PC + operation.Length));
            State.PC = State.A[(operation.OpCodeValue >> 2) & 0x07];
        }

        /// <summary>
        /// Jump on Subroutine to Immediate Value
        /// </summary>
        public static void JsrImd(Operation operation)
        {
            PushToStack((ushort)(State.PC + operation.Length));
            State.PC = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
        }

        /// <summary>
        /// Jump on Subroutine to Register Reference
        /// </summary>
        public static void JsrRegRef(Operation operation)
        {
            PushToStack((ushort)(State.PC + operation.Length));
            srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
            State.PC = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
        }

        /// <summary>
        /// Jump on Subroutine to Memory
        /// </summary>
        public static void JsrMemRef(Operation operation)
        {
            PushToStack((ushort)(State.PC + operation.Length));
            valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            State.PC = (ushort)(Memory.Data[valuew++] << 8 | Memory.Data[valuew]);
        }

        /// <summary>
        /// Jump on Less or Equal to Register Value
        /// </summary>
        public static void JleRegDir(Operation operation)
        {
            // z=1 or n<>o
            if (((State.SR & State.Z) != 0) || (State.SR & State.N) != ((State.SR & State.O) << 2))
            {
                State.PC = State.A[(operation.OpCodeValue >> 2) & 0x07];
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Less or Equal to Immediate Value
        /// </summary>
        public static void JleImd(Operation operation)
        {
            // z=1 or n<>o
            if (((State.SR & State.Z) != 0) || (State.SR & State.N) != ((State.SR & State.O) << 2))
            {
                State.PC = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Less or Equal to Register Reference
        /// </summary>
        public static void JleRegRef(Operation operation)
        {
            // z=1 or n<>o
            if (((State.SR & State.Z) != 0) || (State.SR & State.N) != ((State.SR & State.O) << 2))
            {
                srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
                State.PC = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Less or Equal to Memory
        /// </summary>
        public static void JleMemRef(Operation operation)
        {
            // z=1 or n<>o
            if (((State.SR & State.Z) != 0) || (State.SR & State.N) != ((State.SR & State.O) << 2))
            {
                valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                State.PC = (ushort)(Memory.Data[valuew++] << 8 | Memory.Data[valuew]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Greater to Register Value
        /// </summary>
        public static void JgRegDir(Operation operation)
        {
            //z=0 and n=o
            if ((State.SR & State.Z) == 0 && (State.SR & State.N) == ((State.SR & State.O) << 2))
            {
                State.PC = State.A[(operation.OpCodeValue >> 2) & 0x07];
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Greater to Immediate Value
        /// </summary>
        public static void JgImd(Operation operation)
        {
            //z=0 and n=o
            if ((State.SR & State.Z) == 0 && (State.SR & State.N) == ((State.SR & State.O) << 2))
            {
                State.PC = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Greater to Register Reference
        /// </summary>
        public static void JgRegRef(Operation operation)
        {
            //z=0 and n=o
            if ((State.SR & State.Z) == 0 && (State.SR & State.N) == ((State.SR & State.O) << 2))
            {
                srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
                State.PC = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Greater to Memory
        /// </summary>
        public static void JgMemRef(Operation operation)
        {
            //z=0 and n=o
            if ((State.SR & State.Z) == 0 && (State.SR & State.N) == ((State.SR & State.O) << 2))
            {
                valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                State.PC = (ushort)(Memory.Data[valuew++] << 8 | Memory.Data[valuew]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Greater or Equal to Register Value
        /// </summary>
        public static void JgeRegDir(Operation operation)
        {
            //z=1 or n=o
            if ((State.SR & State.Z) == 1 || (State.SR & State.N) == ((State.SR & State.O) << 2))
            {
                State.PC = State.A[(operation.OpCodeValue >> 2) & 0x07];
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Greater or Equal to Immediate Value
        /// </summary>
        public static void JgeImd(Operation operation)
        {
            //z=1 or n=o
            if ((State.SR & State.Z) == 1 || (State.SR & State.N) == ((State.SR & State.O) << 2))
            {
                State.PC = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Greater or Equal to Register Reference
        /// </summary>
        public static void JgeRegRef(Operation operation)
        {
            //z=1 or n=o
            if ((State.SR & State.Z) == 1 || (State.SR & State.N) == ((State.SR & State.O) << 2))
            {
                srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
                State.PC = (ushort)(Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Jump on Greater or Equal to Memory
        /// </summary>
        public static void JgeMemRef(Operation operation)
        {
            //z=1 or n=o
            if ((State.SR & State.Z) == 1 || (State.SR & State.N) == ((State.SR & State.O) << 2))
            {
                valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                State.PC = (ushort)(Memory.Data[valuew++] << 8 | Memory.Data[valuew]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Interrupt Service to Tiny Immediate Value
        /// </summary>
        public static void IntImdQuick(Operation operation)
        {
            // load vector
            dstndx = (operation.OpCodeValue >> 2) & 0x0f;
            // treat spi-disk interrupt and functions
            Interrupt intfunction = (Interrupt)State.A[0];
            if (intfunction >= Interrupt.SPI_INIT && intfunction <= Interrupt.WRITESEC && (Interrupt)dstndx == Interrupt.INT_SPI)
            {
                switch (intfunction)
                {
                    case Interrupt.SPI_INIT:
                        State.A[0] = (ushort)(!string.IsNullOrEmpty(DiskOperations.VHDPath) ? 0x0100 : 0);
                        break;

                    case Interrupt.SPISEND:
                        // not used
                        break;

                    // INT4T13	DA		READSEC  ; read in buffer at A2, n in A1
                    case Interrupt.READSEC:
                        State.A[0] = DiskOperations.ReadBlockToLionBuffer(State.A[1], State.A[2]);
                        break;

                    // INT4T14	DA		WRITESEC ; WRITE BUFFER at A2 TO A1 BLOCK
                    case Interrupt.WRITESEC:
                        State.A[0] = DiskOperations.WriteLionBufferToBlock(State.A[2], State.A[1]);
                        break;
                }
                State.PC += operation.Length;
            }
            else
            {
                dstndx <<= 1;
                PushToStack((ushort)(State.PC + operation.Length));
                PushToStack((ushort)State.SR);
                State.PC = (ushort)(Memory.Data[dstndx++] << 8 | Memory.Data[dstndx]);
            }
        }

        /// <summary>
        /// Relative Jump to Register Value
        /// </summary>
        public static void JrRegDir(Operation operation)
        {
            State.PC = (ushort)(State.PC + operation.Length + State.A[(operation.OpCodeValue >> 2) & 0x07]);
        }

        /// <summary>
        /// Relative Jump to Immediate Value
        /// </summary>
        public static void JrImd(Operation operation)
        {
            State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]));
        }

        /// <summary>
        /// Relative Jump to Register Reference
        /// </summary>
        public static void JrRegRef(Operation operation)
        {
            srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
            State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]));
        }

        /// <summary>
        /// Relative Jump to Memory
        /// </summary>
        public static void JrMemRef(Operation operation)
        {
            valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[valuew++] << 8 | Memory.Data[valuew]));
        }

        /// <summary>
        /// Relative Jump on not Zero to Register Value
        /// </summary>
        public static void JrnzRegDir(Operation operation)
        {
            if ((State.SR & State.Z) == ((operation.OpCodeValue & bwb) >> 3))
            {
                State.PC = (ushort)(State.PC + operation.Length + State.A[(operation.OpCodeValue >> 2) & 0x07]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on not Zero to Immediate Value
        /// </summary>
        public static void JrnzImd(Operation operation)
        {
            if ((State.SR & State.Z) == ((operation.OpCodeValue & bwb) >> 3))
            {
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on not Zero to Register Reference
        /// </summary>
        public static void JrnzRegRef(Operation operation)
        {
            if ((State.SR & State.Z) == ((operation.OpCodeValue & bwb) >> 3))
            {
                srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on not Zero to Memory
        /// </summary>
        public static void JrnzMemRef(Operation operation)
        {
            if ((State.SR & State.Z) == ((operation.OpCodeValue & bwb) >> 3))
            {
                valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[valuew++] << 8 | Memory.Data[valuew]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Above to Register Value
        /// </summary>
        public static void JraRegDir(Operation operation)
        {
            // 	c = 0 and z = 0
            if ((State.SR & State.C) == 0 && (State.SR & State.Z) == 0)
            {
                State.PC = (ushort)(State.PC + operation.Length + State.A[(operation.OpCodeValue >> 2) & 0x07]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Above to Immediate Value
        /// </summary>
        public static void JraImd(Operation operation)
        {
            // 	c = 0 and z = 0
            if ((State.SR & State.C) == 0 && (State.SR & State.Z) == 0)
            {
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Above to Register Reference
        /// </summary>
        public static void JraRegRef(Operation operation)
        {
            // 	c = 0 and z = 0
            if ((State.SR & State.C) == 0 && (State.SR & State.Z) == 0)
            {
                srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Above to Memory
        /// </summary>
        public static void JraMemRef(Operation operation)
        {
            // 	c = 0 and z = 0
            if ((State.SR & State.C) == 0 && (State.SR & State.Z) == 0)
            {
                valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[valuew++] << 8 | Memory.Data[valuew]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Negative to Register Value
        /// </summary>
        public static void JrnRegDir(Operation operation)
        {
            if ((State.SR & State.N) != 0)
            {
                State.PC = (ushort)(State.PC + operation.Length + State.A[(operation.OpCodeValue >> 2) & 0x07]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Negative to Immediate Value
        /// </summary>
        public static void JrnImd(Operation operation)
        {
            if ((State.SR & State.N) != 0)
            {
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Negative to Register Reference
        /// </summary>
        public static void JrnRegRef(Operation operation)
        {
            if ((State.SR & State.N) != 0)
            {
                srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Negative to Memory
        /// </summary>
        public static void JrnMemRef(Operation operation)
        {
            if ((State.SR & State.N) != 0)
            {
                valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[valuew++] << 8 | Memory.Data[valuew]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Overflow to Register Value
        /// </summary>
        public static void JroRegDir(Operation operation)
        {
            if ((State.SR & State.O) != 0)
            {
                State.PC = (ushort)(State.PC + operation.Length + State.A[(operation.OpCodeValue >> 2) & 0x07]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Overflow to Immediate Value
        /// </summary>
        public static void JroImd(Operation operation)
        {
            if ((State.SR & State.O) != 0)
            {
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Overflow to Register Reference
        /// </summary>
        public static void JroRegRef(Operation operation)
        {
            if ((State.SR & State.O) != 0)
            {
                srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Overflow to Memory
        /// </summary>
        public static void JroMemRef(Operation operation)
        {
            if ((State.SR & State.O) != 0)
            {
                valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[valuew++] << 8 | Memory.Data[valuew]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Carry to Register Value
        /// </summary>
        public static void JrcRegDir(Operation operation)
        {
            if ((State.SR & State.C) != 0)
            {
                State.PC = (ushort)(State.PC + operation.Length + State.A[(operation.OpCodeValue >> 2) & 0x07]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Carry to Immediate Value
        /// </summary>
        public static void JrcImd(Operation operation)
        {
            if ((State.SR & State.C) != 0)
            {
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Carry to Register Reference
        /// </summary>
        public static void JrcRegRef(Operation operation)
        {
            if ((State.SR & State.C) != 0)
            {
                srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Carry to Memory
        /// </summary>
        public static void JrcMemRef(Operation operation)
        {
            if ((State.SR & State.C) != 0)
            {
                valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[valuew++] << 8 | Memory.Data[valuew]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump Subroutine to Register Value
        /// </summary>
        public static void JrsrRegDir(Operation operation)
        {
            PushToStack((ushort)(State.PC + operation.Length));
            State.PC = (ushort)(State.PC + operation.Length + State.A[(operation.OpCodeValue >> 2) & 0x07]);
        }

        /// <summary>
        /// Relative Jump Subroutine to Immediate Value
        /// </summary>
        public static void JrsrImd(Operation operation)
        {
            PushToStack((ushort)(State.PC + operation.Length));
            State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]));
        }

        /// <summary>
        /// Relative Jump Subroutine to Register Reference
        /// </summary>
        public static void JrsrRegRef(Operation operation)
        {
            PushToStack((ushort)(State.PC + operation.Length));
            srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
            State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]));
        }

        /// <summary>
        /// Relative Jump Subroutine to Memory
        /// </summary>
        public static void JrsrMemRef(Operation operation)
        {
            PushToStack((ushort)(State.PC + operation.Length));
            valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[valuew++] << 8 | Memory.Data[valuew]));
        }

        /// <summary>
        /// Relative Jump on Below or Equal to Register Value
        /// </summary>
        public static void JrbeRegDir(Operation operation)
        {
            // c=1 or z=1 
            if (((State.SR & State.C) | (State.SR & State.Z)) != 0)
            {
                State.PC = (ushort)(State.PC + operation.Length + State.A[(operation.OpCodeValue >> 2) & 0x07]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Below or Equal to Immediate Value
        /// </summary>
        public static void JrbeImd(Operation operation)
        {
            // c=1 or z=1 
            if (((State.SR & State.C) | (State.SR & State.Z)) != 0)
            {
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Below or Equal to Register Reference
        /// </summary>
        public static void JrbeRegRef(Operation operation)
        {
            // c=1 or z=1 
            if (((State.SR & State.C) | (State.SR & State.Z)) != 0)
            {
                srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Below or Equal to Memory
        /// </summary>
        public static void JrbeMemRef(Operation operation)
        {
            // c=1 or z=1 
            if (((State.SR & State.C) | (State.SR & State.Z)) != 0)
            {
                valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[valuew++] << 8 | Memory.Data[valuew]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Less or Equal to Register Value
        /// </summary>
        public static void JrleRegDir(Operation operation)
        {
            // z=1 or n<>o
            if (((State.SR & State.Z) != 0) || (State.SR & State.N) != ((State.SR & State.O) << 2))
            {
                State.PC = (ushort)(State.PC + operation.Length + State.A[(operation.OpCodeValue >> 2) & 0x07]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Less or Equal to Immediate Value
        /// </summary>
        public static void JrleImd(Operation operation)
        {
            // z=1 or n<>o
            if (((State.SR & State.Z) != 0) || (State.SR & State.N) != ((State.SR & State.O) << 2))
            {
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Less or Equal to Register Reference
        /// </summary>
        public static void JrleRegRef(Operation operation)
        {
            // z=1 or n<>o
            if (((State.SR & State.Z) != 0) || (State.SR & State.N) != ((State.SR & State.O) << 2))
            {
                srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Less or Equal to Memory
        /// </summary>
        public static void JrleMemRef(Operation operation)
        {
            // z=1 or n<>o
            if (((State.SR & State.Z) != 0) || (State.SR & State.N) != ((State.SR & State.O) << 2))
            {
                valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[valuew++] << 8 | Memory.Data[valuew]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Less to Register Value
        /// </summary>
        public static void JrlRegDir(Operation operation)
        {
            // z=0 and n<>o
            if ((State.SR & State.Z) == 0 && (State.SR & State.N) != ((State.SR & State.O) << 2))
            {
                State.PC = (ushort)(State.PC + operation.Length + State.A[(operation.OpCodeValue >> 2) & 0x07]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Less to Immediate Value
        /// </summary>
        public static void JrlImd(Operation operation)
        {
            // z=0 and n<>o
            if ((State.SR & State.Z) == 0 && (State.SR & State.N) != ((State.SR & State.O) << 2))
            {
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Less to Register Reference
        /// </summary>
        public static void JrlRegRef(Operation operation)
        {
            // z=0 and n<>o
            if ((State.SR & State.Z) == 0 && (State.SR & State.N) != ((State.SR & State.O) << 2))
            {
                srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Less to Memory
        /// </summary>
        public static void JrlMemRef(Operation operation)
        {
            // z=0 and n<>o
            if ((State.SR & State.Z) == 0 && (State.SR & State.N) != ((State.SR & State.O) << 2))
            {
                valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[valuew++] << 8 | Memory.Data[valuew]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Not Zero X and Decrement, to Register Value
        /// </summary>
        public static void JrxRegDir(Operation operation)
        {
            if (State.X != 0)
            {
                State.PC = (ushort)(State.PC + operation.Length + State.A[(operation.OpCodeValue >> 2) & 0x07]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
            State.X--;
        }

        /// <summary>
        /// Relative Jump on Not Zero X and Decrement, to Immediate Value
        /// </summary>
        public static void JrxImd(Operation operation)
        {
            if (State.X != 0)
            {
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
            State.X--;
        }

        /// <summary>
        /// Relative Jump on Not Zero X and Decrement, to Register Reference
        /// </summary>
        public static void JrxRegRef(Operation operation)
        {
            if (State.X != 0)
            {
                srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
            State.X--;
        }

        /// <summary>
        /// Relative Jump on Not Zero X and Decrement, to Memory
        /// </summary>
        public static void JrxMemRef(Operation operation)
        {
            if (State.X != 0)
            {
                valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[valuew++] << 8 | Memory.Data[valuew]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
            State.X--;
        }

        /// <summary>
        /// Inc/Dec Ax, Relative Jump on Not Zero X and Decrement, to Register Value
        /// </summary>
        public static void JrxaRegDir(Operation operation)
        {
            if (State.X != 0)
            {
                dstndx = (operation.OpCodeValue >> 6) & 0x07;
                dstvalw = State.A[dstndx];
                if ((operation.OpCodeValue & bwb) == 0)
                    if ((State.SR & State.D) == 0)
                        dstvalw += 2;
                    else
                        dstvalw -= 2;
                else
                    if ((State.SR & State.D) == 0)
                    dstvalw += 1;
                else
                    dstvalw -= 1;
                State.A[dstndx] = dstvalw;
                State.PC = (ushort)(State.PC + operation.Length + State.A[(operation.OpCodeValue >> 2) & 0x07]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
            State.X--;
        }

        /// <summary>
        /// Inc/Dec Ax, Relative Jump on Not Zero X and Decrement, to Immediate Value
        /// </summary>
        public static void JrxaImd(Operation operation)
        {
            if (State.X != 0)
            {
                dstndx = (operation.OpCodeValue >> 6) & 0x07;
                dstvalw = State.A[dstndx];
                if ((operation.OpCodeValue & bwb) == 0)
                    if ((State.SR & State.D) == 0)
                        dstvalw += 2;
                    else
                        dstvalw -= 2;
                else
                    if ((State.SR & State.D) == 0)
                    dstvalw += 1;
                else
                    dstvalw -= 1;
                State.A[dstndx] = dstvalw;
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
            State.X--;
        }

        /// <summary>
        /// Inc/Dec Ax, Relative Jump on Not Zero X and Decrement, to Register Reference
        /// </summary>
        public static void JrxaRegRef(Operation operation)
        {
            if (State.X != 0)
            {
                dstndx = (operation.OpCodeValue >> 6) & 0x07;
                dstvalw = State.A[dstndx];
                if ((operation.OpCodeValue & bwb) == 0)
                    if ((State.SR & State.D) == 0)
                        dstvalw += 2;
                    else
                        dstvalw -= 2;
                else
                    if ((State.SR & State.D) == 0)
                    dstvalw += 1;
                else
                    dstvalw -= 1;
                State.A[dstndx] = dstvalw;
                srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
            State.X--;
        }

        /// <summary>
        /// Inc/Dec Ax, Relative Jump on Not Zero X and Decrement, to Memory
        /// </summary>
        public static void JrxaMemRef(Operation operation)
        {
            if (State.X != 0)
            {
                dstndx = (operation.OpCodeValue >> 6) & 0x07;
                dstvalw = State.A[dstndx];
                if ((operation.OpCodeValue & bwb) == 0)
                    if ((State.SR & State.D) == 0)
                        dstvalw += 2;
                    else
                        dstvalw -= 2;
                else
                    if ((State.SR & State.D) == 0)
                    dstvalw += 1;
                else
                    dstvalw -= 1;
                State.A[dstndx] = dstvalw;
                valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[valuew++] << 8 | Memory.Data[valuew]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
            State.X--;
        }

        /// <summary>
        /// Relative Jump on Greater to Register Value
        /// </summary>
        public static void JrgRegDir(Operation operation)
        {
            //z=0 and n=o
            if ((State.SR & State.Z) == 0 && (State.SR & State.N) == ((State.SR & State.O) << 2))
            {
                State.PC = (ushort)(State.PC + operation.Length + State.A[(operation.OpCodeValue >> 2) & 0x07]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Greater to Immediate Value
        /// </summary>
        public static void JrgImd(Operation operation)
        {
            //z=0 and n=o
            if ((State.SR & State.Z) == 0 && (State.SR & State.N) == ((State.SR & State.O) << 2))
            {
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Greater to Register Reference
        /// </summary>
        public static void JrgRegRef(Operation operation)
        {
            //z=0 and n=o
            if ((State.SR & State.Z) == 0 && (State.SR & State.N) == ((State.SR & State.O) << 2))
            {
                srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Greater to Memory
        /// </summary>
        public static void JrgMemRef(Operation operation)
        {
            //z=0 and n=o
            if ((State.SR & State.Z) == 0 && (State.SR & State.N) == ((State.SR & State.O) << 2))
            {
                valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[valuew++] << 8 | Memory.Data[valuew]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Greater or Equal to Register Value
        /// </summary>
        public static void JrgeRegDir(Operation operation)
        {
            //z=1 or n=o
            if ((State.SR & State.Z) == 1 || (State.SR & State.N) == ((State.SR & State.O) << 2))
            {
                State.PC = (ushort)(State.PC + operation.Length + State.A[(operation.OpCodeValue >> 2) & 0x07]);
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Greater or Equal to Register Value
        /// </summary>
        public static void JrgeImd(Operation operation)
        {
            //z=1 or n=o
            if ((State.SR & State.Z) == 1 || (State.SR & State.N) == ((State.SR & State.O) << 2))
            {
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Greater or Equal to Register Value
        /// </summary>
        public static void JrgeRegRef(Operation operation)
        {
            //z=1 or n=o
            if ((State.SR & State.Z) == 1 || (State.SR & State.N) == ((State.SR & State.O) << 2))
            {
                srcndx = State.A[(operation.OpCodeValue >> 2) & 0x07];
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[srcndx++] << 8 | Memory.Data[srcndx]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }

        /// <summary>
        /// Relative Jump on Greater or Equal to Register Value
        /// </summary>
        public static void JrgeMemRef(Operation operation)
        {
            //z=1 or n=o
            if ((State.SR & State.Z) == 1 || (State.SR & State.N) == ((State.SR & State.O) << 2))
            {
                valuew = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
                State.PC = (ushort)(State.PC + operation.Length + (Memory.Data[valuew++] << 8 | Memory.Data[valuew]));
            }
            else
            {
                //WaitForCycles(operation.Cycles);
                State.PC += operation.Length;
            }
        }
        #endregion

        #region Input/Output Operations

        public static void OutRegDirRegDir(Operation operation)
        {
            portnum = State.A[(operation.OpCodeValue >> 6) & 0x07];
            srcndx = (operation.OpCodeValue >> 2) & 0x07;

            if (portnum >= Device.MAX_DEVICE_PORTS)
            {
                portnum &= -2;
                Display.Ram[portnum++] = (byte)(State.A[srcndx] >> 8);
                Display.Ram[portnum] = (byte)State.A[srcndx];
            }
            else
            {
                Device.Port[portnum] = State.A[srcndx];
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void OutByteRegDirRegDir(Operation operation)
        {
            portnum = State.A[(operation.OpCodeValue >> 6) & 0x07];

            if (portnum >= Device.MAX_DEVICE_PORTS)
            {
                Display.Ram[portnum] = (byte)State.A[(operation.OpCodeValue >> 2) & 0x07];
            }
            else
            {
                Device.Port[portnum] = (byte)State.A[(operation.OpCodeValue >> 2) & 0x07];
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void OutRegDirRegRef(Operation operation)
        {
            portnum = State.A[(operation.OpCodeValue >> 6) & 0x07];
            srcvalw = State.A[(operation.OpCodeValue >> 2) & 0x07];

            if (portnum >= Device.MAX_DEVICE_PORTS)
            {
                portnum &= -2;
                Display.Ram[portnum++] = Memory.Data[srcvalw++];
                Display.Ram[portnum] = Memory.Data[srcvalw];
            }
            else
            {
                Device.Port[portnum] = (ushort)(Memory.Data[srcvalw++] << 8 | Memory.Data[srcvalw]);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void OutRegDirImd(Operation operation)
        {
            portnum = State.A[(operation.OpCodeValue >> 6) & 0x07];

            if (portnum >= Device.MAX_DEVICE_PORTS)
            {
                portnum &= -2;
                Display.Ram[portnum++] = Memory.Data[State.PC + 2];
                Display.Ram[portnum] = Memory.Data[State.PC + 3];
            }
            else
            {
                Device.Port[portnum] = (ushort)(Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void OutByteRegDirImd(Operation operation)
        {
            portnum = State.A[(operation.OpCodeValue >> 6) & 0x07];

            if (portnum >= Device.MAX_DEVICE_PORTS)
            {
                Display.Ram[portnum] = Memory.Data[State.PC + 3];
            }
            else
            {
                Device.Port[portnum] = Memory.Data[State.PC + 3];
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void OutImdImd(Operation operation)
        {
            portnum = Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3];

            if (portnum >= Device.MAX_DEVICE_PORTS)
            {
                portnum &= -2;
                Display.Ram[portnum++] = Memory.Data[State.PC + 4];
                Display.Ram[portnum] = Memory.Data[State.PC + 5];
            }
            else
            {
                Device.Port[portnum] = (ushort)(Memory.Data[State.PC + 4] << 8 | Memory.Data[State.PC + 5]);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void OutImdRegDir(Operation operation)
        {
            portnum = Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3];
            srcndx = (operation.OpCodeValue >> 2) & 0x07;

            if (portnum >= Device.MAX_DEVICE_PORTS)
            {
                portnum &= -2;
                Display.Ram[portnum++] = (byte)(State.A[srcndx] >> 8);
                Display.Ram[portnum] = (byte)State.A[srcndx];
            }
            else
            {
                Device.Port[portnum] = State.A[srcndx];
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void OutByteImdRegDir(Operation operation)
        {
            portnum = Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3];

            if (portnum >= Device.MAX_DEVICE_PORTS)
            {
                Display.Ram[portnum] = (byte)State.A[(operation.OpCodeValue >> 2) & 0x07];
            }
            else
            {
                Device.Port[portnum] = (byte)State.A[(operation.OpCodeValue >> 2) & 0x07];
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void InRegDirRegDir(Operation operation)
        {
            portnum = State.A[(operation.OpCodeValue >> 2) & 0x07];
            if ((operation.OpCodeValue & bwb) != 0)
            {
                dstndx = (operation.OpCodeValue >> 6) & 0x07;
                State.A[dstndx] &= 0x0ff00;
                if (portnum >= Device.MAX_DEVICE_PORTS)
                {
                    State.A[dstndx] |= Display.Ram[portnum];
                }
                else
                {
                    State.A[dstndx] |= Device.Port[portnum];
                }
            }
            else
            {
                if (portnum >= Device.MAX_DEVICE_PORTS)
                {
                    portnum &= -2;
                    State.A[(operation.OpCodeValue >> 6) & 0x07] = (ushort)(Display.Ram[portnum++] << 8 | Display.Ram[portnum]);
                }
                else
                {
                    State.A[(operation.OpCodeValue >> 6) & 0x07] = Device.Port[portnum];
                }
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void InRegDirImd(Operation operation)
        {
            portnum = Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3];
            if ((operation.OpCodeValue & bwb) != 0)
            {
                dstndx = (operation.OpCodeValue >> 6) & 0x07;
                State.A[dstndx] &= 0x0ff00;
                if (portnum >= Device.MAX_DEVICE_PORTS)
                {
                    State.A[dstndx] |= Display.Ram[portnum];
                }
                else
                {
                    State.A[dstndx] |= Device.Port[portnum];
                }
            }
            else
            {
                if (portnum >= Device.MAX_DEVICE_PORTS)
                {
                    portnum &= -2;
                    State.A[(operation.OpCodeValue >> 6) & 0x07] = (ushort)(Display.Ram[portnum++] << 8 | Display.Ram[portnum]);
                }
                else
                {
                    State.A[(operation.OpCodeValue >> 6) & 0x07] = Device.Port[portnum];
                }
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void InRegDirRegRef(Operation operation)
        {
            portnum = Memory.Data[State.A[(operation.OpCodeValue >> 2) & 0x07]];
            if ((operation.OpCodeValue & bwb) != 0)
            {
                dstndx = (operation.OpCodeValue >> 6) & 0x07;
                State.A[dstndx] &= 0x0ff00;
                if (portnum >= Device.MAX_DEVICE_PORTS)
                {
                    State.A[dstndx] |= Display.Ram[portnum];
                }
                else
                {
                    State.A[dstndx] |= Device.Port[portnum];
                }
            }
            else
            {
                if (portnum >= Device.MAX_DEVICE_PORTS)
                {
                    portnum &= -2;
                    State.A[(operation.OpCodeValue >> 6) & 0x07] = (ushort)(Display.Ram[portnum++] << 8 | Display.Ram[portnum]);
                }
                else
                {
                    State.A[(operation.OpCodeValue >> 6) & 0x07] = Device.Port[portnum];
                }
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        public static void InRegDirMemRef(Operation operation)
        {
            portnum = Memory.Data[Memory.Data[State.PC + 2] << 8 | Memory.Data[State.PC + 3]];
            if ((operation.OpCodeValue & bwb) != 0)
            {
                dstndx = (operation.OpCodeValue >> 6) & 0x07;
                State.A[dstndx] &= 0x0ff00;

                if (portnum >= Device.MAX_DEVICE_PORTS)
                {
                    State.A[dstndx] |= Display.Ram[portnum];
                }
                else
                {
                    State.A[dstndx] |= Device.Port[portnum];
                }
            }
            else
            {
                if (portnum >= Device.MAX_DEVICE_PORTS)
                {
                    portnum &= -2;
                    State.A[(operation.OpCodeValue >> 6) & 0x07] = (ushort)(Display.Ram[portnum++] << 8 | Display.Ram[portnum]);
                }
                else
                {
                    State.A[(operation.OpCodeValue >> 6) & 0x07] = Device.Port[portnum];
                }
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        #endregion

        #region Implicit Operations

        /// <summary>
        /// Set - Clear Interrupt Flag (bwb 0, 1)
        /// </summary>
        public static void StiCli(Operation operation)
        {
            if ((operation.OpCodeValue & bwb) == 0)
            {
                State.SR |= State.I;
            }
            else
            {
                State.SR &= (byte)(State.I ^ 0x0ff);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Return from Interrupt Service
        /// </summary>
        public static void Reti(Operation operation)
        {
            // restore sr we came
            State.SR = (byte)PopFromStack();
            // pop pc
            State.PC = PopFromStack();
            //WaitForCycles(operation.Cycles);
        }

        /// <summary>
        /// Return from Subroutine
        /// </summary>
        public static void Ret(Operation operation)
        {
            // pop pc
            State.PC = PopFromStack();
            //WaitForCycles(operation.Cycles);
        }

        /// <summary>
        /// Pop X from Stack
        /// </summary>
        public static void PopX(Operation operation)
        {
            State.X = PopFromStack();
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Pop Status Register from Stack
        /// </summary>
        public static void PopSr(Operation operation)
        {
            State.SR = (byte)PopFromStack();
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// Push to Stack bwb=1:Status Register, bwb=0:X
        /// </summary>
        public static void PushSrX(Operation operation)
        {
            if ((operation.OpCodeValue & bwb) != 0)
            {
                PushToStack(State.SR);
            }
            else
            {
                PushToStack(State.X);
            }
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// NOP
        /// </summary>
        public static void Nop(Operation operation)
        {
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
        }

        /// <summary>
        /// UNDEFINED same as NOP
        /// </summary>
        public static void UNDEFINED(Operation operation)
        {
            //WaitForCycles(operation.Cycles);
            State.PC += operation.Length;
            //throw new Exception(string.Format("PC:{0} opcode:{1} mnemonic:{2} error:ILLEGAL INSTRUCTION!!!", Convert.ToString(State.PC, 16).ToUpper().PadLeft(4, '0'), operation.Operands[0].Symbol, operation.Mnemonic));
        }

        #endregion
    }
}
