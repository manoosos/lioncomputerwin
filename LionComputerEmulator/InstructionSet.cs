using System.Collections.Generic;

namespace LionComputerEmulator
{
    public static class InstructionSet
    {
        public static List<Operation> OperationsListInt = new List<Operation>()
        {
#region Memory Operations
            new Operation()
            {
                OpCode = 0x00200, // 0000001 000000000
                Mnemonic = "MOV",
                ExecuteMethod = OperationProcessing.MovRegDirRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x00201, // 0000001 000000001
                Mnemonic = "MOV",
                ExecuteMethod = OperationProcessing.MovRegDirImd,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x00202, // 0000001 000000010
                Mnemonic = "MOV",
                ExecuteMethod = OperationProcessing.MovRegDirRegRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x00203, // 0000001 000000011
                Mnemonic = "MOV",
                ExecuteMethod = OperationProcessing.MovRegDirMemRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x07400, // 0111010 000000000
                Mnemonic = "MOV",
                ExecuteMethod = OperationProcessing.MovRegDirPc,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.ProgramCounter,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x08a00, // 1000101 000000000
                Mnemonic = "MOV",
                ExecuteMethod = OperationProcessing.MovRegDirSp,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.StackPointer,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x00400, // 0000010 000000000
                Mnemonic = "MOV",
                ExecuteMethod = OperationProcessing.MovRegRefRegDir,
                AddressingModeDestination = AddressingMode.RegisterReference,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x00401, // 0000010 000000001
                Mnemonic = "MOV",
                ExecuteMethod = OperationProcessing.MovRegRefImd,
                AddressingModeDestination = AddressingMode.RegisterReference,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x00402, // 0000010 000000010
                Mnemonic = "MOV",
                ExecuteMethod = OperationProcessing.MovRegRefRegRef,
                AddressingModeDestination = AddressingMode.RegisterReference,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x00403, // 0000010 000000011
                Mnemonic = "MOV",
                ExecuteMethod = OperationProcessing.MovRegRefMemRef,
                AddressingModeDestination = AddressingMode.RegisterReference,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x09201, // 1001001 000000001
                Mnemonic = "MOV",
                ExecuteMethod = OperationProcessing.MovMemRefRegDir,
                AddressingModeDestination = AddressingMode.MemoryReference,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0c001, // 1100000 000000001
                Mnemonic = "MOV",
                ExecuteMethod = OperationProcessing.MovMemRefImd,
                AddressingModeDestination = AddressingMode.MemoryReference,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 6
            },
            new Operation()
            {
                OpCode = 0x00c00, // 0000110 000000000
                Mnemonic = "MOV.B",
                ExecuteMethod = OperationProcessing.MovByteRegDirRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x00c01, // 0000110 000000001
                Mnemonic = "MOV.B",
                ExecuteMethod = OperationProcessing.MovByteRegDirImd,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x00c02, // 0000110 000000010
                Mnemonic = "MOV.B",
                ExecuteMethod = OperationProcessing.MovByteRegDirRegRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x00c03, // 0000110 000000011
                Mnemonic = "MOV.B",
                ExecuteMethod = OperationProcessing.MovByteRegDirMemRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x01800, // 0001100 000000000
                Mnemonic = "MOV.B",
                ExecuteMethod = OperationProcessing.MovByteRegRefRegDir,
                AddressingModeDestination = AddressingMode.RegisterReference,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x01801, // 0001100 000000001
                Mnemonic = "MOV.B",
                ExecuteMethod = OperationProcessing.MovByteRegRefImd,
                AddressingModeDestination = AddressingMode.RegisterReference,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x01802, // 0001100 000000010
                Mnemonic = "MOV.B",
                ExecuteMethod = OperationProcessing.MovByteRegRefRegRef,
                AddressingModeDestination = AddressingMode.RegisterReference,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x01803, // 0001100 000000011
                Mnemonic = "MOV.B",
                ExecuteMethod = OperationProcessing.MovByteRegRefMemRef,
                AddressingModeDestination = AddressingMode.RegisterReference,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x09403, // 1001010 000000011
                Mnemonic = "MOV.B",
                ExecuteMethod = OperationProcessing.MovByteMemRefRegDir,
                AddressingModeDestination = AddressingMode.MemoryReference,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0c203, // 1100001 000000011
                Mnemonic = "MOV.B",
                ExecuteMethod = OperationProcessing.MovByteMemRefImd,
                AddressingModeDestination = AddressingMode.MemoryReference,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 6
            },
            new Operation()
            {
                OpCode = 0x09800, // 1001100 000000000
                Mnemonic = "MOVHL",
                ExecuteMethod = OperationProcessing.MovhlRegDirRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x09801, // 1001100 000000001
                Mnemonic = "MOVHL",
                ExecuteMethod = OperationProcessing.MovhlRegDirImd,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x09802, // 1001100 000000010
                Mnemonic = "MOVHL",
                ExecuteMethod = OperationProcessing.MovhlRegDirRegRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x09803, // 1001100 000000011
                Mnemonic = "MOVHL",
                ExecuteMethod = OperationProcessing.MovhlRegDirMemRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x09a00, // 1001101 000000000
                Mnemonic = "MOVLH",
                ExecuteMethod = OperationProcessing.MovlhRegDirRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x09a01, // 1001101 000000001
                Mnemonic = "MOVLH",
                ExecuteMethod = OperationProcessing.MovlhRegDirImd,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x09a02, // 1001101 000000010
                Mnemonic = "MOVLH",
                ExecuteMethod = OperationProcessing.MovlhRegDirRegRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x09a03, // 1001101 000000011
                Mnemonic = "MOVLH",
                ExecuteMethod = OperationProcessing.MovlhRegDirMemRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x09c00, // 1001110 000000000
                Mnemonic = "MOVHH",
                ExecuteMethod = OperationProcessing.MovhhRegDirRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x09c01, // 1001110 000000001
                Mnemonic = "MOVHH",
                ExecuteMethod = OperationProcessing.MovhhRegDirImd,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x09c02, // 1001110 000000010
                Mnemonic = "MOVHH",
                ExecuteMethod = OperationProcessing.MovhhRegDirRegRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x09c03, // 1001110 000000011
                Mnemonic = "MOVHH",
                ExecuteMethod = OperationProcessing.MovhhRegDirMemRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x04000, // 0100000 000000000
                Mnemonic = "MOVI",
                ExecuteMethod = OperationProcessing.MoviRegDirImdQuick,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.ImmediateQuick,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x04400, // 0100010 000000000
                Mnemonic = "MOVI.B",
                ExecuteMethod = OperationProcessing.MoviByteRegDirImdQuick,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.ImmediateQuick,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x02a00, // 0010101 000000000
                Mnemonic = "MOVX",
                ExecuteMethod = OperationProcessing.MovxRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Internal,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0f600, // 1111011 000000000
                Mnemonic = "MOVR",
                ExecuteMethod = OperationProcessing.Nop,
                AddressingModeDestination = AddressingMode.RegisterReference,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Relative,
                Type = OperationType.Memory,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0f601, // 1111011 000000001
                Mnemonic = "GADR",
                ExecuteMethod = OperationProcessing.Gadr,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Relative,
                Type = OperationType.Memory,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0f602, // 1111011 000000010
                Mnemonic = "MOVR",
                ExecuteMethod = OperationProcessing.Nop,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Relative,
                Type = OperationType.Memory,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0f603, // 1111011 000000011
                Mnemonic = "MOVR",
                ExecuteMethod = OperationProcessing.MovrRegDirMemRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Relative,
                Type = OperationType.Memory,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0f803, // 1111100 000000011
                Mnemonic = "MOVR",
                ExecuteMethod = OperationProcessing.MovrMemRefRegDir,
                AddressingModeDestination = AddressingMode.MemoryReference,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Relative,
                Type = OperationType.Memory,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                Mnemonic = "SETX",
                OpCode = 0x02600, // 0010011 000000000
                ExecuteMethod = OperationProcessing.SetxRegDir,
                Type = OperationType.Memory,
                Context = OperationContext.Absolute,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource =  AddressingMode.RegisterDirect,
                Length = 2
            },
            new Operation()
            {
                Mnemonic = "SETX",
                OpCode = 0x02601, // 0010011 000000001
                ExecuteMethod = OperationProcessing.SetxImd,
                Type = OperationType.Memory,
                Context = OperationContext.Absolute,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource =  AddressingMode.Immediate,
                Length = 4
            },
            new Operation()
            {
                Mnemonic = "SETX",
                OpCode = 0x02602, // 0010011 000000010
                ExecuteMethod = OperationProcessing.SetxRegRef,
                Type = OperationType.Memory,
                Context = OperationContext.Absolute,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource =  AddressingMode.RegisterReference,
                Length = 2
            },
            new Operation()
            {
                Mnemonic = "SETX",
                OpCode = 0x02603, // 0010011 000000011
                ExecuteMethod = OperationProcessing.SetxMemRef,
                Type = OperationType.Memory,
                Context = OperationContext.Absolute,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource =  AddressingMode.MemoryReference,
                Length = 4
            },
            new Operation()
            {
                Mnemonic = "SET",
                OpCode = 0x01200, // 0001001 000000000
                ExecuteMethod = OperationProcessing.SetSpRegDir,
                AddressingModeDestination = AddressingMode.StackPointer,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                Mnemonic = "SET",
                OpCode = 0x01201, // 0001001 000000001
                ExecuteMethod = OperationProcessing.SetSpImd,
                AddressingModeDestination = AddressingMode.StackPointer,
                AddressingModeSource =  AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 4
            },
            new Operation()
            {
                Mnemonic = "SET",
                OpCode = 0x01202, // 0001001 000000010
                ExecuteMethod = OperationProcessing.SetSpRegRef,
                AddressingModeDestination = AddressingMode.StackPointer,
                AddressingModeSource =  AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                Mnemonic = "SET",
                OpCode = 0x01203, // 0001001 000000011
                ExecuteMethod = OperationProcessing.SetSpMemRef,
                AddressingModeDestination = AddressingMode.StackPointer,
                AddressingModeSource =  AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x03e00, // 0011111 000000000
                Mnemonic = "XCHG",
                ExecuteMethod = OperationProcessing.XchgRegDirRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x01000, // 0001000 000000000
                Mnemonic = "SWAP",
                ExecuteMethod = OperationProcessing.SwapRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Internal,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x07600, // 0111011 000000000
                Mnemonic = "PUSH",
                ExecuteMethod = OperationProcessing.PushRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x07601, // 0111011 000000001
                Mnemonic = "PUSH",
                ExecuteMethod = OperationProcessing.PushImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x07602, // 0111011 000000010
                Mnemonic = "PUSH",
                ExecuteMethod = OperationProcessing.PushRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x07603, // 0111011 000000011
                Mnemonic = "PUSH",
                ExecuteMethod = OperationProcessing.PushMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x07800, // 0111100 000000000
                Mnemonic = "PUSH",
                ExecuteMethod = OperationProcessing.PushSr,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.StatusRegister,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x08000, // 1000000 000000000
                Mnemonic = "POP",
                ExecuteMethod = OperationProcessing.PopRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Internal,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x07e00, // 0111111 000000000
                Mnemonic = "POP",
                ExecuteMethod = OperationProcessing.PopSr,
                AddressingModeDestination = AddressingMode.StatusRegister,
                AddressingModeSource = AddressingMode.Internal,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
#endregion

#region Arithmetic - Logic Operations
            new Operation()
            {
                OpCode = 0x00600, // 0000011 000000000
                Mnemonic = "ADD",
                ExecuteMethod = OperationProcessing.AddRegDirRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x00601, // 0000011 000000001
                Mnemonic = "ADD",
                ExecuteMethod = OperationProcessing.AddRegDirImd,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x00602, // 0000011 000000010
                Mnemonic = "ADD",
                ExecuteMethod = OperationProcessing.AddRegDirRegRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x00603, // 0000011 000000011
                Mnemonic = "ADD",
                ExecuteMethod = OperationProcessing.AddRegDirMemRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0BC02, // 1011110 000000010
                Mnemonic = "ADD",
                ExecuteMethod = OperationProcessing.AddRegRefRegDir,
                AddressingModeDestination = AddressingMode.RegisterReference,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0B401, // 1011010 000000001
                Mnemonic = "ADD",
                ExecuteMethod = OperationProcessing.AddRegRefImd,
                AddressingModeDestination = AddressingMode.RegisterReference,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0B801, // 1011100 000000001
                Mnemonic = "ADD",
                ExecuteMethod = OperationProcessing.AddMemRefRegDir,
                AddressingModeDestination = AddressingMode.MemoryReference,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0C801, // 1100100 000000001
                Mnemonic = "ADD",
                ExecuteMethod = OperationProcessing.AddMemRefImd,
                AddressingModeDestination = AddressingMode.MemoryReference,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 6
            },
            new Operation()
            {
                OpCode = 0x0b600, // 1011011 000000000
                Mnemonic = "ADD",
                ExecuteMethod = OperationProcessing.AddSpRegDir,
                AddressingModeDestination = AddressingMode.StackPointer,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0b601, // 1011011 000000001
                Mnemonic = "ADD",
                ExecuteMethod = OperationProcessing.AddSpImd,
                AddressingModeDestination = AddressingMode.StackPointer,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x00800, // 0000100 000000000
                Mnemonic = "SUB",
                ExecuteMethod = OperationProcessing.SubRegDirRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x00801, // 0000100 000000001
                Mnemonic = "SUB",
                ExecuteMethod = OperationProcessing.SubRegDirImd,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x00802, // 0000100 000000010
                Mnemonic = "SUB",
                ExecuteMethod = OperationProcessing.SubRegDirRegRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x00803, // 0000100 000000011
                Mnemonic = "SUB",
                ExecuteMethod = OperationProcessing.SubRegDirMemRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0be00, // 1011111 000000000
                Mnemonic = "SUB",
                ExecuteMethod = OperationProcessing.SubRegRefRegDir,
                AddressingModeDestination = AddressingMode.RegisterReference,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x06801, // 0110100 000000001
                Mnemonic = "SUB",
                ExecuteMethod = OperationProcessing.SubRegRefImd,
                AddressingModeDestination = AddressingMode.RegisterReference,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0ca01, // 1100101 000000001
                Mnemonic = "SUB",
                ExecuteMethod = OperationProcessing.SubMemRefImd,
                AddressingModeDestination = AddressingMode.MemoryReference,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 6
            },
            new Operation()
            {

                OpCode = 0x0ba01, // 1011101 000000001
                Mnemonic = "SUB",
                ExecuteMethod = OperationProcessing.SubMemRefRegDir,
                AddressingModeDestination = AddressingMode.MemoryReference,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x07400, // 0111010 000000000
                Mnemonic = "SUB",
                ExecuteMethod = OperationProcessing.SubSpRegDir,
                AddressingModeDestination = AddressingMode.StackPointer,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x07401, // 0111010 000000001
                Mnemonic = "SUB",
                ExecuteMethod = OperationProcessing.SubSpImd,
                AddressingModeDestination = AddressingMode.StackPointer,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x00a00, // 0000101 000000000
                Mnemonic = "ADC",
                ExecuteMethod = OperationProcessing.AdcRegDirRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x00a01, // 0000101 000000001
                Mnemonic = "ADC",
                ExecuteMethod = OperationProcessing.AdcRegDirImd,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x00a02, // 0000101 000000010
                Mnemonic = "ADC",
                ExecuteMethod = OperationProcessing.AdcRegDirRegRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x00a03, // 0000101 000000011
                Mnemonic = "ADC",
                ExecuteMethod = OperationProcessing.AdcRegDirMemRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0a800, // 1010100 000000000
                Mnemonic = "ADDI",
                ExecuteMethod = OperationProcessing.AddiRegDirImdQuick,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.ImmediateQuick,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0a600, // 1010011 000000000
                Mnemonic = "SUBI",
                ExecuteMethod = OperationProcessing.SubiRegDirImdQuick,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.ImmediateQuick,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x03200, // 0011001 000000000
                Mnemonic = "SRA",
                ExecuteMethod = OperationProcessing.SraRegDirImdQuick,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.ImmediateQuick,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x03400, // 0011010 000000000
                Mnemonic = "SLA",
                ExecuteMethod = OperationProcessing.SlaRegDirImdQuick,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.ImmediateQuick,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x03600, // 0011011 000000000
                Mnemonic = "SRL",
                ExecuteMethod = OperationProcessing.SrlRegDirImdQuick,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.ImmediateQuick,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x09e00, // 1001111 000000000
                Mnemonic = "SRL.B",
                ExecuteMethod = OperationProcessing.SrlByteRegDirImdQuick,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.ImmediateQuick,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x03800, // 0011100 000000000
                Mnemonic = "SLL",
                ExecuteMethod = OperationProcessing.SllRegDirImdQuick,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.ImmediateQuick,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0a000, // 1010000 000000000
                Mnemonic = "SLL.B",
                ExecuteMethod = OperationProcessing.SllByteRegDirImdQuick,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.ImmediateQuick,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x06c00, // 0110110 000000000
                Mnemonic = "ROL",
                ExecuteMethod = OperationProcessing.RolRegDirImdQuick,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.ImmediateQuick,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x02c00, // 0010110 000000000
                Mnemonic = "BTST",
                ExecuteMethod = OperationProcessing.BtstRegDirImdQuick,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.ImmediateQuick,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x01600, // 0001011 000000000
                Mnemonic = "BTST",
                ExecuteMethod = OperationProcessing.BtstRegDirRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x02e00, // 0010111 000000000
                Mnemonic = "BSET",
                ExecuteMethod = OperationProcessing.BsetRegDirImdQuick,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.ImmediateQuick,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0a200, // 1010001 000000000
                Mnemonic = "BSET",
                ExecuteMethod = OperationProcessing.BsetRegDirRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x03000, // 0011000 000000000
                Mnemonic = "BCLR",
                ExecuteMethod = OperationProcessing.BclrRegDirImdQuick,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.ImmediateQuick,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x03c00, // 0011110 000000000
                Mnemonic = "BCLR",
                ExecuteMethod = OperationProcessing.BclrRegDirRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x01e00, // 0001111 000000000
                Mnemonic = "AND",
                ExecuteMethod = OperationProcessing.AndRegDirRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x01e01, // 0001111 000000001
                Mnemonic = "AND",
                ExecuteMethod = OperationProcessing.AndRegDirImd,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x01e02, // 0001111 000000010
                Mnemonic = "AND",
                ExecuteMethod = OperationProcessing.AndRegDirRegRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x01e03, // 0001111 000000011
                Mnemonic = "AND",
                ExecuteMethod = OperationProcessing.AndRegDirMemRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x02000, // 0010000 000000000
                Mnemonic = "OR",
                ExecuteMethod = OperationProcessing.OrRegDirRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x02001, // 0010000 000000001
                Mnemonic = "OR",
                ExecuteMethod = OperationProcessing.OrRegDirImd,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x02002, // 0010000 000000010
                Mnemonic = "OR",
                ExecuteMethod = OperationProcessing.OrRegDirRegRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x02003, // 0010000 000000011
                Mnemonic = "OR",
                ExecuteMethod = OperationProcessing.OrRegDirMemRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x02200, // 0010001 000000000
                Mnemonic = "XOR",
                ExecuteMethod = OperationProcessing.XorRegDirRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x02201, // 0010001 000000001
                Mnemonic = "XOR",
                ExecuteMethod = OperationProcessing.XorRegDirImd,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x02202, // 0010001 000000010
                Mnemonic = "XOR",
                ExecuteMethod = OperationProcessing.XorRegDirRegRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x02203, // 0010001 000000011
                Mnemonic = "XOR",
                ExecuteMethod = OperationProcessing.XorRegDirMemRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x01400, // 0001010 000000000
                Mnemonic = "MULU",
                ExecuteMethod = OperationProcessing.MuluRegDirRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x01401, // 0001010 000000001
                Mnemonic = "MULU",
                ExecuteMethod = OperationProcessing.MuluRegDirImd,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x01402, // 0001010 000000010
                Mnemonic = "MULU",
                ExecuteMethod = OperationProcessing.MuluRegDirRegRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x01403, // 0001010 000000011
                Mnemonic = "MULU",
                ExecuteMethod = OperationProcessing.MuluRegDirMemRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x02400, // 0010010 000000000
                Mnemonic = "NOT",
                ExecuteMethod = OperationProcessing.NotRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Internal,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0B200, // 1011001 000000000
                Mnemonic = "SRLL",
                ExecuteMethod = OperationProcessing.SrllRegDirRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x09600, // 1001011 000000000
                Mnemonic = "SLLL",
                ExecuteMethod = OperationProcessing.SlllRegDirRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0aa00, // 1010101 000000000
                Mnemonic = "NEG",
                ExecuteMethod = OperationProcessing.NegRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Internal,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x08e00, // 1000111 000000000
                Mnemonic = "INC",
                ExecuteMethod = OperationProcessing.IncRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Internal,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width =  OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0c803, // 1100100 000000000
                Mnemonic = "INC",
                ExecuteMethod = OperationProcessing.AddMemRefImd,
                AddressingModeDestination = AddressingMode.MemoryReference,
                AddressingModeSource = AddressingMode.Internal,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width =  OperationWidth.Mixed,
                Length = 6
            },
            new Operation()
            {
                OpCode = 0x09000, // 1001000 000000000
                Mnemonic = "DEC",
                ExecuteMethod = OperationProcessing.DecRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Internal,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width =  OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0ca03, // 1100101 000000011
                Mnemonic = "DEC",
                ExecuteMethod = OperationProcessing.SubMemRefImd,
                AddressingModeDestination = AddressingMode.MemoryReference,
                AddressingModeSource = AddressingMode.Internal,
                Context = OperationContext.Absolute,
                Type = OperationType.Arithmetic,
                Width =  OperationWidth.Mixed,
                Length = 6
            },
            new Operation()
            {
                OpCode = 0x04c00, // 0100110 000000000
                Mnemonic = "SRSET",
                ExecuteMethod = OperationProcessing.SrSet,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.ImmediateQuick,
                Type = OperationType.Arithmetic,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x05400, // 0101010 000000000
                Mnemonic = "SRCLR",
                ExecuteMethod = OperationProcessing.SrClr,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.ImmediateQuick,
                Type = OperationType.Arithmetic,
                Length = 2
            },
#endregion

#region Conditional Operations
            new Operation()
            {
                OpCode = 0x0a400, // 1010010 000000000
                Mnemonic = "CMPI",
                ExecuteMethod = OperationProcessing.CmpiRegDirImdQuick,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.ImmediateQuick,
                Context = OperationContext.Absolute,
                Type = OperationType.Conditional,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x06000, // 0110000 000000000
                Mnemonic = "CMPI.B",
                ExecuteMethod = OperationProcessing.CmpiByteRegDirImdQuick,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.ImmediateQuick,
                Context = OperationContext.Absolute,
                Type = OperationType.Conditional,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x01c00, // 0001110 000000000
                Mnemonic = "CMP",
                ExecuteMethod = OperationProcessing.CmpRegDirRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Conditional,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x01c01, // 0001110 000000001
                Mnemonic = "CMP",
                ExecuteMethod = OperationProcessing.CmpRegDirImd,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Conditional,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x01c02, // 0001110 000000010
                Mnemonic = "CMP",
                ExecuteMethod = OperationProcessing.CmpRegDirRegRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Conditional,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x01c03, // 0001110 000000011
                Mnemonic = "CMP",
                ExecuteMethod = OperationProcessing.CmpRegDirMemRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Conditional,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x04200, // 0100001 000000000
                Mnemonic = "CMP",
                ExecuteMethod = OperationProcessing.CmpRegRefRegDir,
                AddressingModeDestination = AddressingMode.RegisterReference,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Conditional,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x04201, // 0100001 000000001
                Mnemonic = "CMP",
                ExecuteMethod = OperationProcessing.CmpRegRefImd,
                AddressingModeDestination = AddressingMode.RegisterReference,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Conditional,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x04202, // 0100001 000000010
                Mnemonic = "CMP",
                ExecuteMethod = OperationProcessing.CmpRegRefRegRef,
                AddressingModeDestination = AddressingMode.RegisterReference,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Conditional,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x01a03, // 0100001 000000011
                Mnemonic = "CMP",
                ExecuteMethod = OperationProcessing.CmpMemRefRegDir,
                AddressingModeDestination = AddressingMode.MemoryReference,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Conditional,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0c403, // 1100010 000000011
                Mnemonic = "CMP",
                ExecuteMethod = OperationProcessing.CmpMemRefImd,
                AddressingModeDestination = AddressingMode.MemoryReference,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Conditional,
                Width = OperationWidth.Mixed,
                Length = 6
            },
            new Operation()
            {
                OpCode = 0x06400, // 0110010 000000000
                Mnemonic = "CMPHL",
                ExecuteMethod = OperationProcessing.CmphlRegDirRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Conditional,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x06401, // 0110010 000000001
                Mnemonic = "CMPHL",
                ExecuteMethod = OperationProcessing.CmphlRegDirImd,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Conditional,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x06402, // 0110010 000000010
                Mnemonic = "CMPHL",
                ExecuteMethod = OperationProcessing.CmphlRegDirRegRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Conditional,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x06403, // 0110010 000000011
                Mnemonic = "CMPHL",
                ExecuteMethod = OperationProcessing.CmphlRegDirMemRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Conditional,
                Length = 4
            },
#endregion

#region Branch Absolute Operations
            new Operation()
            {
                OpCode = 0x08200, // 1000001 000000000
                Mnemonic = "INT",
                ExecuteMethod = OperationProcessing.IntImdQuick,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.ImmediateQuick,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x04a00, // 0100101 000000000
                Mnemonic = "JMP",
                ExecuteMethod = OperationProcessing.JmpRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x04a01, // 0100101 000000001
                Mnemonic = "JMP",
                ExecuteMethod = OperationProcessing.JmpImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x04a02, // 0100101 000000010
                Mnemonic = "JMP",
                ExecuteMethod = OperationProcessing.JmpRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x04a03, // 0100101 000000011
                Mnemonic = "JMP",
                ExecuteMethod = OperationProcessing.JmpMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x04e00, // 0100111 000000000
                Mnemonic = "JNZ",
                ExecuteMethod = OperationProcessing.JnzRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x04e01, // 0100111 000000001
                Mnemonic = "JNZ",
                ExecuteMethod = OperationProcessing.JnzImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x04e02, // 0100111 000000010
                Mnemonic = "JNZ",
                ExecuteMethod = OperationProcessing.JnzRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x04e03, // 0100111 000000011
                Mnemonic = "JNZ",
                ExecuteMethod = OperationProcessing.JnzMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x05200, // 0101001 000000000
                Mnemonic = "JNO",
                ExecuteMethod = OperationProcessing.JnoRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x05201, // 0101001 000000001
                Mnemonic = "JNO",
                ExecuteMethod = OperationProcessing.JnoImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x05202, // 0101001 000000010
                Mnemonic = "JNO",
                ExecuteMethod = OperationProcessing.JnoRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x05203, // 0101001 000000011
                Mnemonic = "JNO",
                ExecuteMethod = OperationProcessing.JnoMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x06200, // 0110001 000000000
                Mnemonic = "JL",
                ExecuteMethod = OperationProcessing.JlRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x06201, // 0110001 000000001
                Mnemonic = "JL",
                ExecuteMethod = OperationProcessing.JlImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x06202, // 0110001 000000010
                Mnemonic = "JL",
                ExecuteMethod = OperationProcessing.JlRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x06203, // 0110001 000000011
                Mnemonic = "JL",
                ExecuteMethod = OperationProcessing.JlMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x05600, // 0101011 000000000
                Mnemonic = "JNC",
                ExecuteMethod = OperationProcessing.JncRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x05601, // 0101011 000000001
                Mnemonic = "JNC",
                ExecuteMethod = OperationProcessing.JncImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x05602, // 0101011 000000010
                Mnemonic = "JNC",
                ExecuteMethod = OperationProcessing.JncRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x05603, // 0101011 000000011
                Mnemonic = "JNC",
                ExecuteMethod = OperationProcessing.JncMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x05a00, // 0101101 000000000
                Mnemonic = "JP",
                ExecuteMethod = OperationProcessing.JpRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x05a01, // 0101101 000000001
                Mnemonic = "JP",
                ExecuteMethod = OperationProcessing.JpImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x05a02, // 0101101 000000010
                Mnemonic = "JP",
                ExecuteMethod = OperationProcessing.JpRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x05a03, // 0101101 000000011
                Mnemonic = "JP",
                ExecuteMethod = OperationProcessing.JpMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x05c00, // 0101110 000000000
                Mnemonic = "JBE",
                ExecuteMethod = OperationProcessing.JbeRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x05c01, // 0101110 000000001
                Mnemonic = "JBE",
                ExecuteMethod = OperationProcessing.JbeImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x05c02, // 0101110 000000010
                Mnemonic = "JBE",
                ExecuteMethod = OperationProcessing.JbeRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x05c03, // 0101110 000000011
                Mnemonic = "JBE",
                ExecuteMethod = OperationProcessing.JbeMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x05e00, // 0101111 000000000
                Mnemonic = "JA",
                ExecuteMethod = OperationProcessing.JaRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x05e01, // 0101111 000000001
                Mnemonic = "JA",
                ExecuteMethod = OperationProcessing.JaImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x05e02, // 0101111 000000010
                Mnemonic = "JA",
                ExecuteMethod = OperationProcessing.JaRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x05e03, // 0101111 000000011
                Mnemonic = "JA",
                ExecuteMethod = OperationProcessing.JaMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x03a00, // 0011101 000000000
                Mnemonic = "JAE",
                ExecuteMethod = OperationProcessing.JaeRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x03a01, // 0011101 000000001
                Mnemonic = "JAE",
                ExecuteMethod = OperationProcessing.JaeImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x03a02, // 0011101 000000010
                Mnemonic = "JAE",
                ExecuteMethod = OperationProcessing.JaeRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x03a03, // 0011101 000000011
                Mnemonic = "JAE",
                ExecuteMethod = OperationProcessing.JaeMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x06a00, // 0110101 000000000
                Mnemonic = "JSR",
                ExecuteMethod = OperationProcessing.JsrRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x06a01, // 0110101 000000001
                Mnemonic = "JSR",
                ExecuteMethod = OperationProcessing.JsrImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x06a02, // 0110101 000000010
                Mnemonic = "JSR",
                ExecuteMethod = OperationProcessing.JsrRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x06a03, // 0110101 000000011
                Mnemonic = "JSR",
                ExecuteMethod = OperationProcessing.JsrMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x07000, // 0111000 000000000
                Mnemonic = "JLE",
                ExecuteMethod = OperationProcessing.JleRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x07001, // 0111000 000000001
                Mnemonic = "JLE",
                ExecuteMethod = OperationProcessing.JleImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x07002, // 0111000 000000010
                Mnemonic = "JLE",
                ExecuteMethod = OperationProcessing.JleRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x07003, // 0111000 000000011
                Mnemonic = "JLE",
                ExecuteMethod = OperationProcessing.JleMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x07200, // 0111001 000000000
                Mnemonic = "JG",
                ExecuteMethod = OperationProcessing.JgRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x07201, // 0111001 000000001
                Mnemonic = "JG",
                ExecuteMethod = OperationProcessing.JgImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x07202, // 0111001 000000010
                Mnemonic = "JG",
                ExecuteMethod = OperationProcessing.JgRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x07203, // 0111001 000000011
                Mnemonic = "JG",
                ExecuteMethod = OperationProcessing.JgMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x04600, // 0100011 000000000
                Mnemonic = "JGE",
                ExecuteMethod = OperationProcessing.JgeRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x04601, // 0100011 000000001
                Mnemonic = "JGE",
                ExecuteMethod = OperationProcessing.JgeImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x04602, // 0100011 000000010
                Mnemonic = "JGE",
                ExecuteMethod = OperationProcessing.JgeRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x04603, // 0100011 000000011
                Mnemonic = "JGE",
                ExecuteMethod = OperationProcessing.JgeMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x02800, // 0010100 000000000
                Mnemonic = "JMPX",
                ExecuteMethod = OperationProcessing.JmpxRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x02801, // 0010100 000000001
                Mnemonic = "JMPX",
                ExecuteMethod = OperationProcessing.JmpxImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x02802, // 0010100 000000010
                Mnemonic = "JMPX",
                ExecuteMethod = OperationProcessing.JmpxRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x02803, // 0010100 000000011
                Mnemonic = "JMPX",
                ExecuteMethod = OperationProcessing.JmpxMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x06600, // 0110011 000000000
                Mnemonic = "JXA",
                ExecuteMethod = OperationProcessing.JxaRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x06601, // 0110011 000000001
                Mnemonic = "JXA",
                ExecuteMethod = OperationProcessing.JxaImd,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x06602, // 0110011 000000010
                Mnemonic = "JXA",
                ExecuteMethod = OperationProcessing.JxaRegRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x06603, // 0110011 000000011
                Mnemonic = "JXA",
                ExecuteMethod = OperationProcessing.JxaMemRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Branch,
                Width = OperationWidth.Mixed,
                Length = 4
            },

#endregion

#region Branch Relative Operations
            new Operation()
            {
                OpCode = 0x0e000, // 1110000 000000000
                Mnemonic = "JR",
                ExecuteMethod = OperationProcessing.JrRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0e001, // 1110000 000000001
                Mnemonic = "JR",
                ExecuteMethod = OperationProcessing.JrImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0e002, // 1110000 000000010
                Mnemonic = "JR",
                ExecuteMethod = OperationProcessing.JrRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0e003, // 1110000 000000011
                Mnemonic = "JR",
                ExecuteMethod = OperationProcessing.JrMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0e200, // 1110001 000000000
                Mnemonic = "JRZ",
                ExecuteMethod = OperationProcessing.JrzRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0e201, // 1110001 000000001
                Mnemonic = "JRZ",
                ExecuteMethod = OperationProcessing.JrzImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0e202, // 1110001 000000010
                Mnemonic = "JRZ",
                ExecuteMethod = OperationProcessing.JrzRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0e203, // 1110001 000000011
                Mnemonic = "JRZ",
                ExecuteMethod = OperationProcessing.JrzMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0e400, // 1110010 000000000
                Mnemonic = "JRN",
                ExecuteMethod = OperationProcessing.JrnRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0e401, // 1110010 000000001
                Mnemonic = "JRN",
                ExecuteMethod = OperationProcessing.JrnImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0e402, // 1110010 000000010
                Mnemonic = "JRN",
                ExecuteMethod = OperationProcessing.JrnRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0e403, // 1110010 000000011
                Mnemonic = "JRN",
                ExecuteMethod = OperationProcessing.JrnMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0e600, // 1110011 000000000
                Mnemonic = "JRO",
                ExecuteMethod = OperationProcessing.JroRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0e601, // 1110011 000000001
                Mnemonic = "JRO",
                ExecuteMethod = OperationProcessing.JroImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0e602, // 1110011 000000010
                Mnemonic = "JRO",
                ExecuteMethod = OperationProcessing.JroRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0e603, // 1110011 000000011
                Mnemonic = "JRO",
                ExecuteMethod = OperationProcessing.JroMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0e800, // 1110100 000000000
                Mnemonic = "JRC",
                ExecuteMethod = OperationProcessing.JrcRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0e801, // 1110100 000000001
                Mnemonic = "JRC",
                ExecuteMethod = OperationProcessing.JrcImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0e802, // 1110100 000000010
                Mnemonic = "JRC",
                ExecuteMethod = OperationProcessing.JrcRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0e803, // 1110100 000000011
                Mnemonic = "JRC",
                ExecuteMethod = OperationProcessing.JrcMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0ec00, // 1110110 000000000
                Mnemonic = "JRSR",
                ExecuteMethod = OperationProcessing.JrsrRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0ec01, // 1110110 000000001
                Mnemonic = "JRSR",
                ExecuteMethod = OperationProcessing.JrsrImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0ec02, // 1110110 000000010
                Mnemonic = "JRSR",
                ExecuteMethod = OperationProcessing.JrsrRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0ec03, // 1110110 000000011
                Mnemonic = "JRSR",
                ExecuteMethod = OperationProcessing.JrsrMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0ee00, // 1110111 000000000
                Mnemonic = "JRBE",
                ExecuteMethod = OperationProcessing.JrbeRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0ee01, // 1110111 000000001
                Mnemonic = "JRBE",
                ExecuteMethod = OperationProcessing.JrbeImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0ee02, // 1110111 000000010
                Mnemonic = "JRBE",
                ExecuteMethod = OperationProcessing.JrbeRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0ee03, // 1110111 000000011
                Mnemonic = "JRBE",
                ExecuteMethod = OperationProcessing.JrbeMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0f400, // 1111010 000000000
                Mnemonic = "JRA",
                ExecuteMethod = OperationProcessing.JraRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0f401, // 1111010 000000001
                Mnemonic = "JRA",
                ExecuteMethod = OperationProcessing.JraImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0f402, // 1111010 000000010
                Mnemonic = "JRA",
                ExecuteMethod = OperationProcessing.JraRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0f403, // 1111010 000000011
                Mnemonic = "JRA",
                ExecuteMethod = OperationProcessing.JraMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0f000, // 1111000 000000000
                Mnemonic = "JRLE",
                ExecuteMethod = OperationProcessing.JrleRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0f001, // 1111000 000000001
                Mnemonic = "JRLE",
                ExecuteMethod = OperationProcessing.JrleImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0f002, // 1111000 000000010
                Mnemonic = "JRLE",
                ExecuteMethod = OperationProcessing.JrleRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0f003, // 1111000 000000011
                Mnemonic = "JRLE",
                ExecuteMethod = OperationProcessing.JrleMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0fc00, // 1111110 000000000
                Mnemonic = "JRL",
                ExecuteMethod = OperationProcessing.JrlRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0fc01, // 1111110 000000001
                Mnemonic = "JRL",
                ExecuteMethod = OperationProcessing.JrlImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0fc02, // 1111110 000000010
                Mnemonic = "JRL",
                ExecuteMethod = OperationProcessing.JrlRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0fc03, // 1111110 000000011
                Mnemonic = "JRL",
                ExecuteMethod = OperationProcessing.JrlMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0ea00, // 1110101 000000000
                Mnemonic = "JRG",
                ExecuteMethod = OperationProcessing.JrgRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0ea01, // 1110101 000000001
                Mnemonic = "JRG",
                ExecuteMethod = OperationProcessing.JrgImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0ea02, // 1110101 000000010
                Mnemonic = "JRG",
                ExecuteMethod = OperationProcessing.JrgRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0ea03, // 1110101 000000011
                Mnemonic = "JRG",
                ExecuteMethod = OperationProcessing.JrgMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0fa00, // 1111101 000000000
                Mnemonic = "JRGE",
                ExecuteMethod = OperationProcessing.JrgeRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0fa01, // 1111101 000000001
                Mnemonic = "JRGE",
                ExecuteMethod = OperationProcessing.JrgeImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0fa02, // 1111101 000000010
                Mnemonic = "JRGE",
                ExecuteMethod = OperationProcessing.JrgeRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0fa03, // 1111101 000000011
                Mnemonic = "JRGE",
                ExecuteMethod = OperationProcessing.JrgeMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0f200, // 1111001 000000000
                Mnemonic = "JRNZ",
                ExecuteMethod = OperationProcessing.JrnzRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0f201, // 1111001 000000001
                Mnemonic = "JRNZ",
                ExecuteMethod = OperationProcessing.JrnzImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0f202, // 1111001 000000010
                Mnemonic = "JRNZ",
                ExecuteMethod = OperationProcessing.JrnzRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0f203, // 1111001 000000011
                Mnemonic = "JRNZ",
                ExecuteMethod = OperationProcessing.JrnzMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0fe00, // 1111111 000000000
                Mnemonic = "JRX",
                ExecuteMethod = OperationProcessing.JrxRegDir,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0fe01, // 1111111 000000001
                Mnemonic = "JRX",
                ExecuteMethod = OperationProcessing.JrxImd,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0fe02, // 1111111 000000010
                Mnemonic = "JRX",
                ExecuteMethod = OperationProcessing.JrxRegRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0fe03, // 1111111 000000011
                Mnemonic = "JRX",
                ExecuteMethod = OperationProcessing.JrxMemRef,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0d200, // 1101001 000000000
                Mnemonic = "JRXA",
                ExecuteMethod = OperationProcessing.JrxaRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0d201, // 1101001 000000001
                Mnemonic = "JRXA",
                ExecuteMethod = OperationProcessing.JrxaImd,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0d202, // 1101001 000000010
                Mnemonic = "JRXA",
                ExecuteMethod = OperationProcessing.JrxaRegRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0d203, // 1101001 000000011
                Mnemonic = "JRXA",
                ExecuteMethod = OperationProcessing.JrxaMemRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Relative,
                Type = OperationType.Branch,
                Width = OperationWidth.Mixed,
                Length = 4
            },
#endregion

#region InputOutput Operations
            new Operation()
            {
                OpCode = 0x00e00, // 0000111 000000000
                Mnemonic = "OUT",
                ExecuteMethod = OperationProcessing.OutRegDirRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.InputOutput,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x00e01, // 0000111 000000001
                Mnemonic = "OUT",
                ExecuteMethod = OperationProcessing.OutImdRegDir,
                AddressingModeDestination = AddressingMode.Immediate,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.InputOutput,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0ac02, // 1010110 000000010
                Mnemonic = "OUT",
                ExecuteMethod = OperationProcessing.OutRegDirRegRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.InputOutput,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0ac01, // 1010110 000000001
                Mnemonic = "OUT",
                ExecuteMethod = OperationProcessing.OutRegDirImd,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.InputOutput,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0c601, // 1100011 000000001
                Mnemonic = "OUT",
                ExecuteMethod = OperationProcessing.OutImdImd,
                AddressingModeDestination = AddressingMode.Immediate,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.InputOutput,
                Length = 6
            },
            new Operation()
            {
                OpCode = 0x0ae00, // 1010111 000000000
                Mnemonic = "OUT.B",
                ExecuteMethod = OperationProcessing.OutByteRegDirRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.InputOutput,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x0ae01, // 1010111 000000001
                Mnemonic = "OUT.B",
                ExecuteMethod = OperationProcessing.OutByteImdRegDir,
                AddressingModeDestination = AddressingMode.Immediate,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.InputOutput,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x0b001, // 1011000 000000001
                Mnemonic = "OUT.B",
                ExecuteMethod = OperationProcessing.OutByteRegDirImd,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.InputOutput,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x08c00, // 1000110 000000000
                Mnemonic = "IN",
                ExecuteMethod = OperationProcessing.InRegDirRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.InputOutput,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x08c01, // 1000110 000000001
                Mnemonic = "IN",
                ExecuteMethod = OperationProcessing.InRegDirImd,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.InputOutput,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = 0x08c02, // 1000110 000000010
                Mnemonic = "IN",
                ExecuteMethod = OperationProcessing.InRegDirRegRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.InputOutput,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x08c03, // 1000110 000000011
                Mnemonic = "IN",
                ExecuteMethod = OperationProcessing.InRegDirMemRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.InputOutput,
                Width = OperationWidth.Mixed,
                Length = 4
            },
#endregion

#region Implicit Operations
            new Operation()
            {
                OpCode = 0x00000, // 0000000 000000000
                Mnemonic = "NOP",
                ExecuteMethod = OperationProcessing.Nop,
                Type = OperationType.Implicit,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x08800, // 1000100 000000000
                Mnemonic = "STI",
                ExecuteMethod = OperationProcessing.Sti,
                Type = OperationType.Implicit,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x08600, // 1000011 000000000
                Mnemonic = "CLI",
                ExecuteMethod = OperationProcessing.Cli,
                Type = OperationType.Implicit,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x08400, // 1000010 000000000
                Mnemonic = "RETI",
                ExecuteMethod = OperationProcessing.Reti,
                Type = OperationType.Implicit,
                Length = 2
            },
            new Operation()
            {
                OpCode = 0x06e00, // 0110111 000000000
                Mnemonic = "RET",
                ExecuteMethod = OperationProcessing.Ret,
                Type = OperationType.Implicit,
                Length = 2
            },
            new Operation()
            {
                Mnemonic = "POPX",
                OpCode = 0x07c00, // 0111110 000000000
                Type = OperationType.Implicit,
                ExecuteMethod = OperationProcessing.PopX,
                Length = 2
            },
            new Operation()
            {
                Mnemonic = "PUSHX",
                OpCode = 0x07a00, // 0111101 000000000
                Type = OperationType.Implicit,
                ExecuteMethod = OperationProcessing.PushX,
                Length = 2
            },
#endregion
        };

        public static List<Operation> OperationsList = InitializeList();

        private static List<Operation> InitializeList()
        {
            var l = new List<Operation>(ushort.MaxValue);

            for (ushort i = 0; i < ushort.MaxValue; i++)
                l.Add(null);

            foreach (var op in OperationsListInt)
                l[op.OpCode] = op;

            return l;
        }
    }
}
