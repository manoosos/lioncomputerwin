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
        /// Operand dispatch
        /// </summary>
        private static Operand DispatchOperand(ushort word, AddressingMode addressingMode, OperandNotation notation, ushort relativeValue)
        {
            Operand _ret = new Operand()
            {
                AddressingMode = addressingMode,
                Notation = notation
            };

            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    // immediate number
                    word += relativeValue;
                    _ret.Value = word;
                    _ret.Symbol = string.Format("${0}", Convert.ToString(word, 16).ToUpper().PadLeft(4, '0'));
                    break;

                case AddressingMode.MemoryReference:
                    // memory location
                    word += relativeValue;
                    _ret.Value = word;
                    _ret.Symbol = string.Format("(${0})", Convert.ToString(word, 16).ToUpper().PadLeft(4, '0'));
                    break;

                case AddressingMode.ImmediateQuick:
                    // immediate nibble operand bits 111100
                    _ret.Value = (ushort)((word >> 2) & 0x0f);
                    _ret.Symbol = string.Format("{0}", _ret.Value);
                    break;

                case AddressingMode.RegisterDirect:
                    //  register index
                    _ret.Value = (ushort)((word >> (notation == OperandNotation.Destination ? 6 : 2)) & 0x07);
                    _ret.Symbol = string.Format("A{0}", _ret.Value);
                    break;

                case AddressingMode.RegisterReference:
                    //  register index
                    _ret.Value = (ushort)((word >> (notation == OperandNotation.Destination ? 6 : 2)) & 0x07);
                    _ret.Symbol = string.Format("(A{0})", _ret.Value);
                    break;
            }

            return _ret;
        }

        /// <summary>
        /// Instruction Decoder for Disassembly
        /// </summary>
        public static Operation InstructionDecode(uint MemoryLocation)
        {
            ushort memoryOpCode = Functions.BytesToWord(Memory.Data[MemoryLocation], Memory.Data[MemoryLocation + 1]);
            // a new operation does not interfere with the executing
            Operation instruction = null;
            if (InstructionSet.OperationsList[(ushort)(memoryOpCode & 0x0fe03)] != null)
            {
                instruction = new Operation()
                {
                    AddressingModeDestination = InstructionSet.OperationsList[(ushort)(memoryOpCode & 0x0fe03)].AddressingModeDestination,
                    AddressingModeSource = InstructionSet.OperationsList[(ushort)(memoryOpCode & 0x0fe03)].AddressingModeSource,
                    Context = InstructionSet.OperationsList[(ushort)(memoryOpCode & 0x0fe03)].Context,
                    Length = InstructionSet.OperationsList[(ushort)(memoryOpCode & 0x0fe03)].Length,
                    Mnemonic = InstructionSet.OperationsList[(ushort)(memoryOpCode & 0x0fe03)].Mnemonic,
                    OpCode = InstructionSet.OperationsList[(ushort)(memoryOpCode & 0x0fe03)].OpCode,
                    OpCodeValue = memoryOpCode,
                    Type = InstructionSet.OperationsList[(ushort)(memoryOpCode & 0x0fe03)].Type,
                    Width = InstructionSet.OperationsList[(ushort)(memoryOpCode & 0x0fe03)].Width,
                };
            }

            if (MemoryLocation < 0x020 || instruction == null)
            {
                return new Operation()
                {
                    OpCode = memoryOpCode,
                    OpCodeValue = memoryOpCode,
                    Type = OperationType.Undefined,
                    Width = OperationWidth.Word,
                    Mnemonic = "DW",
                    Cycles = 0,
                    Length = 2,
                    Operands = new List<Operand>()
                    {
                      new Operand()
                        {
                            AddressingMode=AddressingMode.Immediate,
                            Notation=OperandNotation.Destination,
                            Symbol = string.Format("${0}", Convert.ToString(memoryOpCode, 16).ToUpper() .PadLeft(4, '0')),
                            Value = memoryOpCode
                        }
                    }
                };
            }
            //else if (instruction == null)
            //    throw new Exception("oh no no no no...");

            if (instruction.Type == OperationType.Implicit)
            {
                // decode nothing, opcode passed as is
                return instruction;
            }
            else
            {
                // operands with addressing modes
                // destination addressing
                switch (instruction.AddressingModeDestination)
                {
                    case AddressingMode.MemoryReference:
                    case AddressingMode.Immediate:
                        instruction.Operands.Add(DispatchOperand(Functions.BytesToWord(Memory.Data[MemoryLocation + 2], Memory.Data[MemoryLocation + 3]), instruction.AddressingModeDestination, OperandNotation.Destination, (ushort)(instruction.Context == OperationContext.Relative ? MemoryLocation + instruction.Length : 0)));
                        break;

                    case AddressingMode.RegisterDirect:
                    case AddressingMode.RegisterReference:
                        instruction.Operands.Add(DispatchOperand(memoryOpCode, instruction.AddressingModeDestination, OperandNotation.Destination, (ushort)(instruction.Context == OperationContext.Relative ? MemoryLocation + instruction.Length : 0)));
                        break;

                    case AddressingMode.StackPointer:
                        instruction.Operands.Add(new Operand()
                        {
                            AddressingMode = AddressingMode.StackPointer,
                            Notation = OperandNotation.Destination,
                            Symbol = "SP",
                            Value = 0,
                        });
                        break;

                    case AddressingMode.ProgramCounter:
                        instruction.Operands.Add(new Operand()
                        {
                            AddressingMode = AddressingMode.ProgramCounter,
                            Notation = OperandNotation.Destination,
                            Symbol = "PC",
                            Value = 0,
                        });
                        break;

                    case AddressingMode.StatusRegister:
                        instruction.Operands.Add(new Operand()
                        {
                            AddressingMode = AddressingMode.StatusRegister,
                            Notation = OperandNotation.Destination,
                            Symbol = "SR",
                            Value = 0,
                        });
                        break;
                }

                // source addressing
                switch (instruction.AddressingModeSource)
                {
                    case AddressingMode.Immediate:
                    case AddressingMode.MemoryReference:
                        if (instruction.AddressingModeDestination == AddressingMode.MemoryReference || instruction.AddressingModeDestination == AddressingMode.Immediate)
                        {
                            instruction.Operands.Add(DispatchOperand(Functions.BytesToWord(Memory.Data[MemoryLocation + 4], Memory.Data[MemoryLocation + 5]), instruction.AddressingModeSource, OperandNotation.Source, (ushort)(instruction.Context == OperationContext.Relative ? MemoryLocation + instruction.Length : 0)));
                        }
                        else
                        {
                            // register reference
                            instruction.Operands.Add(DispatchOperand(Functions.BytesToWord(Memory.Data[MemoryLocation + 2], Memory.Data[MemoryLocation + 3]), instruction.AddressingModeSource, OperandNotation.Source, (ushort)(instruction.Context == OperationContext.Relative ? MemoryLocation + instruction.Length : 0)));
                        }
                        break;

                    case AddressingMode.ImmediateQuick:
                    case AddressingMode.RegisterDirect:
                    case AddressingMode.RegisterReference:
                        if (instruction.AddressingModeDestination == AddressingMode.Internal)
                        {
                            instruction.Operands.Add(DispatchOperand(memoryOpCode, instruction.AddressingModeSource, OperandNotation.Destination, 0));
                        }
                        else
                        {
                            instruction.Operands.Add(DispatchOperand(memoryOpCode, instruction.AddressingModeSource, OperandNotation.Source, 0));
                        }
                        break;

                    case AddressingMode.StackPointer:
                        instruction.Operands.Add(new Operand()
                        {
                            AddressingMode = AddressingMode.StackPointer,
                            Notation = OperandNotation.Source,
                            Symbol = "SP",
                            Value = 0,
                        });
                        break;

                    case AddressingMode.ProgramCounter:
                        instruction.Operands.Add(new Operand()
                        {
                            AddressingMode = AddressingMode.ProgramCounter,
                            Notation = OperandNotation.Source,
                            Symbol = "PC",
                            Value = 0,
                        });
                        break;

                    case AddressingMode.StatusRegister:
                        instruction.Operands.Add(new Operand()
                        {
                            AddressingMode = AddressingMode.StatusRegister,
                            Notation = OperandNotation.Source,
                            Symbol = "SR",
                            Value = 0,
                        });
                        break;
                }
            }

            return instruction;
        }

        /// <summary>
        /// Reset method
        /// </summary>
        public static void Reset()
        {
            State.A = new ushort[8];
            State.SR = 0;
            State.SP = 0x03ffc; // debug // 0x01e0;
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
