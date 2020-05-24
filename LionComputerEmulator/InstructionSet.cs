using System;
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
                OpCode = Convert.ToUInt16("0000001"+"000000000",2), // 0x00200, //  
                Mnemonic = "MOV",
                ExecuteMethod = OperationProcessing.MovRegDirRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("0000001"+"000000001",2), // 0x00201, //  
                Mnemonic = "MOV",
                ExecuteMethod = OperationProcessing.MovRegDirImd,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("0000001"+"000000010",2), // 0x00202, //  
                Mnemonic = "MOV",
                ExecuteMethod = OperationProcessing.MovRegDirRegRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("0000001"+"000000011",2), // 0x00203, //  
                Mnemonic = "MOV",
                ExecuteMethod = OperationProcessing.MovRegDirMemRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("0111010"+"000000000",2), // 0x07400, //  
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
                OpCode = Convert.ToUInt16("1000101"+"000000000",2), // 0x08a00, //  
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
                OpCode = Convert.ToUInt16("0000010"+"000000000",2), // 0x00400, //  
                Mnemonic = "MOV",
                ExecuteMethod = OperationProcessing.MovRegRefRegDir,
                AddressingModeDestination = AddressingMode.RegisterReference,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("0000010"+"000000001",2), // 0x00401, //  
                Mnemonic = "MOV",
                ExecuteMethod = OperationProcessing.MovRegRefImd,
                AddressingModeDestination = AddressingMode.RegisterReference,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("0000010"+"000000010",2), // 0x00402, //  
                Mnemonic = "MOV",
                ExecuteMethod = OperationProcessing.MovRegRefRegRef,
                AddressingModeDestination = AddressingMode.RegisterReference,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("0000010"+"000000011",2), // 0x00403, //  
                Mnemonic = "MOV",
                ExecuteMethod = OperationProcessing.MovRegRefMemRef,
                AddressingModeDestination = AddressingMode.RegisterReference,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("1001001"+"000000001",2), // 0x09201, //  
                Mnemonic = "MOV",
                ExecuteMethod = OperationProcessing.MovMemRefRegDir,
                AddressingModeDestination = AddressingMode.MemoryReference,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("1100000"+"000000001",2), // 0x0c001, //  
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
                OpCode = Convert.ToUInt16("1100001"+"000000011",2), // 0x0c203, //  
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
                OpCode = Convert.ToUInt16("1001100"+"000000000",2), // 0x09800, //  
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
                OpCode = Convert.ToUInt16("1001100"+"000000001",2), // 0x09801, //  
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
                OpCode = Convert.ToUInt16("1001100"+"000000010",2), // 0x09802, //  
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
                OpCode = Convert.ToUInt16("1001100"+"000000011",2), // 0x09803, //  
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
                OpCode = Convert.ToUInt16("1001101"+"000000000",2), // 0x09a00, //  
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
                OpCode = Convert.ToUInt16("1001101"+"000000001",2), // 0x09a01, //  
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
                OpCode = Convert.ToUInt16("1001101"+"000000010",2), // 0x09a02, //  
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
                OpCode = Convert.ToUInt16("1001101"+"000000011",2), // 0x09a03, //  
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
                OpCode = Convert.ToUInt16("1001110"+"000000000",2), // 0x09c00, //  
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
                OpCode = Convert.ToUInt16("1001110"+"000000001",2), // 0x09c01, //  
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
                OpCode = Convert.ToUInt16("1001110"+"000000010",2), // 0x09c02, //  
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
                OpCode = Convert.ToUInt16("1001110"+"000000011",2), // 0x09c03, //  
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
                OpCode = Convert.ToUInt16("0100000"+"000000000",2), // 0x04000, //  
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
                OpCode = Convert.ToUInt16("0100010"+"000000000",2), // 0x04400, //  
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
                OpCode = Convert.ToUInt16("0010101"+"000000000",2), // 0x02a00, //  
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
                OpCode = Convert.ToUInt16("1111011"+"000000010",2), // 0x0f602, //  
                Mnemonic = "MOVR",
                ExecuteMethod = OperationProcessing.MovrRegDirRegRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Relative,
                Type = OperationType.Memory,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("1111011"+"000000001",2), // 0x0f601, //  
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
                OpCode = Convert.ToUInt16("1111011"+"000000011",2), // 0x0f603, //  
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
                OpCode = Convert.ToUInt16("1111100"+"000000000",2), // 0x0f600, //  
                Mnemonic = "MOVR",
                ExecuteMethod = OperationProcessing.MovrRegRefRegDir,
                AddressingModeDestination = AddressingMode.RegisterReference,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Relative,
                Type = OperationType.Memory,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("1111100"+"000000001",2), // 0x0f601, //  
                Mnemonic = "MOVR",
                ExecuteMethod = OperationProcessing.MovrRegRefImd,
                AddressingModeDestination = AddressingMode.RegisterReference,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Relative,
                Type = OperationType.Memory,
                Width = OperationWidth.Mixed,
                Length = 4
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("1111100"+"000000010",2), // 0x0f602, //  
                Mnemonic = "MOVR",
                ExecuteMethod = OperationProcessing.MovrRegRefRegRef,
                AddressingModeDestination = AddressingMode.RegisterReference,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Relative,
                Type = OperationType.Memory,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("1111100"+"000000011",2), // 0x0f803, //  
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
                OpCode = Convert.ToUInt16("0010011"+"000000000",2), // 0x02600, //  
                Mnemonic = "SETX",
                ExecuteMethod = OperationProcessing.SetxRegDir,
                Type = OperationType.Memory,
                Context = OperationContext.Absolute,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource =  AddressingMode.RegisterDirect,
                Length = 2
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("0010011"+"000000001",2), // 0x02601, //  
                Mnemonic = "SETX",
                ExecuteMethod = OperationProcessing.SetxImd,
                Type = OperationType.Memory,
                Context = OperationContext.Absolute,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource =  AddressingMode.Immediate,
                Length = 4
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("0010011"+"000000010",2), // 0x02602, //  
                Mnemonic = "SETX",
                ExecuteMethod = OperationProcessing.SetxRegRef,
                Type = OperationType.Memory,
                Context = OperationContext.Absolute,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource =  AddressingMode.RegisterReference,
                Length = 2
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("0010011"+"000000011",2), // 0x02603, //  
                Mnemonic = "SETX",
                ExecuteMethod = OperationProcessing.SetxMemRef,
                Type = OperationType.Memory,
                Context = OperationContext.Absolute,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource =  AddressingMode.MemoryReference,
                Length = 4
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("0001001"+"000000000",2), // 0x01200, //  
                Mnemonic = "SET",
                ExecuteMethod = OperationProcessing.SetSpRegDir,
                AddressingModeDestination = AddressingMode.StackPointer,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("0001001"+"000000001",2), // 0x01201, //  
                Mnemonic = "SET",
                ExecuteMethod = OperationProcessing.SetSpImd,
                AddressingModeDestination = AddressingMode.StackPointer,
                AddressingModeSource =  AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 4
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("0001001"+"000000010",2), // 0x01202, //  
                Mnemonic = "SET",
                ExecuteMethod = OperationProcessing.SetSpRegRef,
                AddressingModeDestination = AddressingMode.StackPointer,
                AddressingModeSource =  AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("0001001"+"000000011",2), // 0x01203, //  
                Mnemonic = "SET",
                ExecuteMethod = OperationProcessing.SetSpMemRef,
                AddressingModeDestination = AddressingMode.StackPointer,
                AddressingModeSource =  AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 4
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("0011111"+"000000000",2), // 0x03e00, //  
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
                OpCode = Convert.ToUInt16("0001000"+"000000000",2), // 0x01000, //  
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
                OpCode = Convert.ToUInt16("0111011"+"000000000",2), // 0x07600, //  
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
                OpCode = Convert.ToUInt16("0111011"+"000000001",2), // 0x07601, //  
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
                OpCode = Convert.ToUInt16("0111011"+"000000010",2), // 0x07602, //  
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
                OpCode = Convert.ToUInt16("0111011"+"000000011",2), // 0x07603, //  
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
                OpCode = Convert.ToUInt16("0111101"+"000000000",2), // 0x07a00, //  
                Mnemonic = "PUSH",
                ExecuteMethod = OperationProcessing.PushSrX,
                AddressingModeDestination = AddressingMode.Internal,
                AddressingModeSource = AddressingMode.StatusRegister,
                Context = OperationContext.Absolute,
                Type = OperationType.Memory,
                Length = 2
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("1000000"+"000000000",2), // 0x08000, //  
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
                OpCode = Convert.ToUInt16("0111111"+"000000000",2), // 0x07e00, //  
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
                OpCode = Convert.ToUInt16("0000011"+"000000000",2), // 0x00600, //  
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
                OpCode = Convert.ToUInt16("0000011"+"000000001",2), // 0x00601, //  
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
                OpCode = Convert.ToUInt16("0000011"+"000000010",2), // 0x00602, //  
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
                OpCode = Convert.ToUInt16("0000011"+"000000011",2), // 0x00603, //  
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
                OpCode = Convert.ToUInt16("1011110"+"000000000",2), // 0x0bc00, //  
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
                OpCode = Convert.ToUInt16("1011010"+"000000001",2), // 0x0B401, //  
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
                OpCode = Convert.ToUInt16("1011100"+"000000001",2), // 0x0B801, //  
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
                OpCode = Convert.ToUInt16("1100100"+"000000011",2), // 0x0C803, //  
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
                OpCode = Convert.ToUInt16("1011011"+"000000000",2), // 0x0b600, //  
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
                OpCode = Convert.ToUInt16("1011011"+"000000001",2), // 0x0b601, //  
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
                OpCode = Convert.ToUInt16("0000100"+"000000000",2), // 0x00800, //  
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
                OpCode = Convert.ToUInt16("0000100"+"000000001",2), // 0x00801, //  
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
                OpCode = Convert.ToUInt16("0000100"+"000000010",2), // 0x00802, //  
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
                OpCode = Convert.ToUInt16("0000100"+"000000011",2), // 0x00803, //  
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
                OpCode = Convert.ToUInt16("1011111"+"000000000",2), // 0x0be00, //  
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
                OpCode = Convert.ToUInt16("0110100"+"000000001",2), // 0x06801, //  
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
                OpCode = Convert.ToUInt16("1100101"+"000000001",2), // 0x0ca01, //  
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
                OpCode = Convert.ToUInt16("1011100"+"000000001",2), // 0x0b801, //  
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
                OpCode = Convert.ToUInt16("0111010"+"000000000",2), // 0x07400, //  
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
                OpCode = Convert.ToUInt16("0111010"+"000000001",2), // 0x07401, //  
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
                OpCode = Convert.ToUInt16("0000101"+"000000000",2), // 0x00a00, //  
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
                OpCode = Convert.ToUInt16("0000101"+"000000001",2), // 0x00a01, //  
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
                OpCode = Convert.ToUInt16("0000101"+"000000010",2), // 0x00a02, //  
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
                OpCode = Convert.ToUInt16("0000101"+"000000011",2), // 0x00a03, //  
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
                OpCode = Convert.ToUInt16("1010100"+"000000000",2), // 0x0a800, //  
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
                OpCode = Convert.ToUInt16("1010011"+"000000000",2), // 0x0a600, //  
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
                OpCode = Convert.ToUInt16("0011001"+"000000000",2), // 0x03200, //  
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
                OpCode = Convert.ToUInt16("0011010"+"000000000",2), // 0x03400, //  
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
                OpCode = Convert.ToUInt16("0011011"+"000000000",2), // 0x03600, //  
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
                OpCode = Convert.ToUInt16("1001111"+"000000000",2), // 0x09e00, //  
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
                OpCode = Convert.ToUInt16("0011100"+"000000000",2), // 0x03800, //  
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
                OpCode = Convert.ToUInt16("1010000"+"000000000",2), // 0x0a000, //  
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
                OpCode = Convert.ToUInt16("0110110"+"000000000",2), // 0x06c00, //  
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
                OpCode = Convert.ToUInt16("0010110"+"000000000",2), // 0x02c00, //  
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
                OpCode = Convert.ToUInt16("0001011"+"000000000",2), // 0x01600, //  
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
                OpCode = Convert.ToUInt16("0010111"+"000000000",2), // 0x02e00, //  
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
                OpCode = Convert.ToUInt16("1010001"+"000000000",2), // 0x0a200, //  
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
                OpCode = Convert.ToUInt16("0011000"+"000000000",2), // 0x03000, //  
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
                OpCode = Convert.ToUInt16("0011110"+"000000000",2), // 0x03c00, //  
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
                OpCode = Convert.ToUInt16("0001111"+"000000000",2), // 0x01e00, //  
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
                OpCode = Convert.ToUInt16("0001111"+"000000001",2), // 0x01e01, //  
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
                OpCode = Convert.ToUInt16("0001111"+"000000010",2), // 0x01e02, //  
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
                OpCode = Convert.ToUInt16("0001111"+"000000011",2), // 0x01e03, //  
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
                OpCode = Convert.ToUInt16("0010000"+"000000000",2), // 0x02000, //  
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
                OpCode = Convert.ToUInt16("0010000"+"000000001",2), // 0x02001, //  
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
                OpCode = Convert.ToUInt16("0010000"+"000000010",2), // 0x02002, //  
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
                OpCode = Convert.ToUInt16("0010000"+"000000011",2), // 0x02003, //  
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
                OpCode = Convert.ToUInt16("0010001"+"000000000",2), // 0x02200, //  
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
                OpCode = Convert.ToUInt16("0010001"+"000000001",2), // 0x02201, //  
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
                OpCode = Convert.ToUInt16("0010001"+"000000010",2), // 0x02202, //  
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
                OpCode = Convert.ToUInt16("0010001"+"000000011",2), // 0x02203, //  
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
                OpCode = Convert.ToUInt16("0001010"+"000000000",2), // 0x01400, //  
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
                OpCode = Convert.ToUInt16("0001010"+"000000001",2), // 0x01401, //  
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
                OpCode = Convert.ToUInt16("0001010"+"000000010",2), // 0x01402, //  
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
                OpCode = Convert.ToUInt16("0001010"+"000000011",2), // 0x01403, //  
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
                OpCode = Convert.ToUInt16("0010010"+"000000000",2), // 0x02400, //  
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
                OpCode = Convert.ToUInt16("1011001"+"000000000",2), // 0x0B200, //  
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
                OpCode = Convert.ToUInt16("1001011"+"000000000",2), // 0x09600, //  
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
                OpCode = Convert.ToUInt16("1010101"+"000000000",2), // 0x0aa00, //  
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
                OpCode = Convert.ToUInt16("1000111"+"000000000",2), // 0x08e00, //  
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
                OpCode = Convert.ToUInt16("1100100"+"000000011",2), // 0x0c803, //  
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
                OpCode = Convert.ToUInt16("1001000"+"000000000",2), // 0x09000, //  
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
                OpCode = Convert.ToUInt16("1100101"+"000000011",2), // 0x0ca03, //  
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
                OpCode = Convert.ToUInt16("0100110"+"000000000",2), // 0x04c00, //  
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
                OpCode = Convert.ToUInt16("1010010"+"000000000",2), // 0x0a400, //  
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
                OpCode = Convert.ToUInt16("0110000"+"000000000",2), // 0x06000, //  
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
                OpCode = Convert.ToUInt16("0001110"+"000000000",2), // 0x01c00, //  
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
                OpCode = Convert.ToUInt16("0001110"+"000000001",2), // 0x01c01, //  
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
                OpCode = Convert.ToUInt16("0001110"+"000000010",2), // 0x01c02, //  
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
                OpCode = Convert.ToUInt16("0001110"+"000000011",2), // 0x01c03, //  
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
                OpCode = Convert.ToUInt16("0100001"+"000000000",2), // 0x04200, //  
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
                OpCode = Convert.ToUInt16("0100001"+"000000001",2), // 0x04201, //  
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
                OpCode = Convert.ToUInt16("0100001"+"000000010",2), // 0x04202, //  
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
                OpCode = Convert.ToUInt16("0100001"+"000000011",2), // 0x04203, //  
                Mnemonic = "CMP",
                ExecuteMethod = OperationProcessing.CmpRegRefRegRef,
                AddressingModeDestination = AddressingMode.RegisterReference,
                AddressingModeSource = AddressingMode.MemoryReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Conditional,
                Width = OperationWidth.Mixed,
                Length = 2
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("0001101"+"000000011",2), // 0x01a03, //  
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
                OpCode = Convert.ToUInt16("1100010"+"000000011",2), // 0x0c403, //  
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
                OpCode = Convert.ToUInt16("0110010"+"000000000",2), // 0x06400, //  
                Mnemonic = "CMPH",
                ExecuteMethod = OperationProcessing.CmphRegDirRegDir,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterDirect,
                Context = OperationContext.Absolute,
                Type = OperationType.Conditional,
                Length = 2
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("0110010"+"000000001",2), // 0x06401, //  
                Mnemonic = "CMPH",
                ExecuteMethod = OperationProcessing.CmphRegDirImd,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.Immediate,
                Context = OperationContext.Absolute,
                Type = OperationType.Conditional,
                Length = 4
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("0110010"+"000000010",2), // 0x06402, //  
                Mnemonic = "CMPH",
                ExecuteMethod = OperationProcessing.CmphRegDirRegRef,
                AddressingModeDestination = AddressingMode.RegisterDirect,
                AddressingModeSource = AddressingMode.RegisterReference,
                Context = OperationContext.Absolute,
                Type = OperationType.Conditional,
                Length = 2
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("0110010"+"000000011",2), // 0x06403, //  
                Mnemonic = "CMPH",
                ExecuteMethod = OperationProcessing.CmphRegDirMemRef,
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
                OpCode = Convert.ToUInt16("1000001"+"000000000",2), // 0x08200, //  
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
                OpCode = Convert.ToUInt16("0100101"+"000000000",2), // 0x04a00, //  
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
                OpCode = Convert.ToUInt16("0100101"+"000000001",2), // 0x04a01, //  
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
                OpCode = Convert.ToUInt16("0100101"+"000000010",2), // 0x04a02, //  
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
                OpCode = Convert.ToUInt16("0100101"+"000000011",2), // 0x04a03, //  
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
                OpCode = Convert.ToUInt16("0100111"+"000000000",2), // 0x04e00, //  
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
                OpCode = Convert.ToUInt16("0100111"+"000000001",2), // 0x04e01, //  
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
                OpCode = Convert.ToUInt16("0100111"+"000000010",2), // 0x04e02, //  
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
                OpCode = Convert.ToUInt16("0100111"+"000000011",2), // 0x04e03, //  
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
                OpCode = Convert.ToUInt16("0101001"+"000000000",2), // 0x05200, //  
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
                OpCode = Convert.ToUInt16("0101001"+"000000001",2), // 0x05201, //  
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
                OpCode = Convert.ToUInt16("0101001"+"000000010",2), // 0x05202, //  
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
                OpCode = Convert.ToUInt16("0101001"+"000000011",2), // 0x05203, //  
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
                OpCode = Convert.ToUInt16("0110001"+"000000000",2), // 0x06200, //  
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
                OpCode = Convert.ToUInt16("0110001"+"000000001",2), // 0x06201, //  
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
                OpCode = Convert.ToUInt16("0110001"+"000000010",2), // 0x06202, //  
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
                OpCode = Convert.ToUInt16("0110001"+"000000011",2), // 0x06203, //  
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
                OpCode = Convert.ToUInt16("0101011"+"000000000",2), // 0x05600, //  
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
                OpCode = Convert.ToUInt16("0101011"+"000000001",2), // 0x05601, //  
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
                OpCode = Convert.ToUInt16("0101011"+"000000010",2), // 0x05602, //  
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
                OpCode = Convert.ToUInt16("0101011"+"000000011",2), // 0x05603, //  
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
                OpCode = Convert.ToUInt16("0101101"+"000000000",2), // 0x05a00, //  
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
                OpCode = Convert.ToUInt16("0101101"+"000000001",2), // 0x05a01, //  
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
                OpCode = Convert.ToUInt16("0101101"+"000000010",2), // 0x05a02, //  
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
                OpCode = Convert.ToUInt16("0101101"+"000000011",2), // 0x05a03, //  
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
                OpCode = Convert.ToUInt16("0101110"+"000000000",2), // 0x05c00, //  
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
                OpCode = Convert.ToUInt16("0101110"+"000000001",2), // 0x05c01, //  
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
                OpCode = Convert.ToUInt16("0101110"+"000000010",2), // 0x05c02, //  
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
                OpCode = Convert.ToUInt16("0101110"+"000000011",2), // 0x05c03, //  
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
                OpCode = Convert.ToUInt16("0101111"+"000000000",2), // 0x05e00, //  
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
                OpCode = Convert.ToUInt16("0101111"+"000000001",2), // 0x05e01, //  
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
                OpCode = Convert.ToUInt16("0101111"+"000000010",2), // 0x05e02, //  
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
                OpCode = Convert.ToUInt16("0101111"+"000000011",2), // 0x05e03, //  
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
                OpCode = Convert.ToUInt16("0011101"+"000000000",2), // 0x03a00, //  
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
                OpCode = Convert.ToUInt16("0011101"+"000000001",2), // 0x03a01, //  
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
                OpCode = Convert.ToUInt16("0011101"+"000000010",2), // 0x03a02, //  
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
                OpCode = Convert.ToUInt16("0011101"+"000000011",2), // 0x03a03, //  
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
                OpCode = Convert.ToUInt16("0110101"+"000000000",2), // 0x06a00, //  
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
                OpCode = Convert.ToUInt16("0110101"+"000000001",2), // 0x06a01, //  
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
                OpCode = Convert.ToUInt16("0110101"+"000000010",2), // 0x06a02, //  
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
                OpCode = Convert.ToUInt16("0110101"+"000000011",2), // 0x06a03, //  
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
                OpCode = Convert.ToUInt16("0111000"+"000000000",2), // 0x07000, //  
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
                OpCode = Convert.ToUInt16("0111000"+"000000001",2), // 0x07001, //  
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
                OpCode = Convert.ToUInt16("0111000"+"000000010",2), // 0x07002, //  
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
                OpCode = Convert.ToUInt16("0111000"+"000000011",2), // 0x07003, //  
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
                OpCode = Convert.ToUInt16("0111001"+"000000000",2), // 0x07200, //  
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
                OpCode = Convert.ToUInt16("0111001"+"000000001",2), // 0x07201, //  
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
                OpCode = Convert.ToUInt16("0111001"+"000000010",2), // 0x07202, //  
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
                OpCode = Convert.ToUInt16("0111001"+"000000011",2), // 0x07203, //  
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
                OpCode = Convert.ToUInt16("0100011"+"000000000",2), // 0x04600, //  
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
                OpCode = Convert.ToUInt16("0100011"+"000000001",2), // 0x04601, //  
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
                OpCode = Convert.ToUInt16("0100011"+"000000010",2), // 0x04602, //  
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
                OpCode = Convert.ToUInt16("0100011"+"000000011",2), // 0x04603, //  
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
                OpCode = Convert.ToUInt16("0010100"+"000000000",2), // 0x02800, //  
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
                OpCode = Convert.ToUInt16("0010100"+"000000001",2), // 0x02801, //  
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
                OpCode = Convert.ToUInt16("0010100"+"000000010",2), // 0x02802, //  
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
                OpCode = Convert.ToUInt16("0010100"+"000000011",2), // 0x02803, //  
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
                OpCode = Convert.ToUInt16("0110011"+"000000000",2), // 0x06600, //  
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
                OpCode = Convert.ToUInt16("0110011"+"000000001",2), // 0x06601, //  
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
                OpCode = Convert.ToUInt16("0110011"+"000000010",2), // 0x06602, //  
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
                OpCode = Convert.ToUInt16("0110011"+"000000011",2), // 0x06603, //  
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
                OpCode = Convert.ToUInt16("1110000"+"000000000",2), // 0x0e000, //  
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
                OpCode = Convert.ToUInt16("1110000"+"000000001",2), // 0x0e001, //  
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
                OpCode = Convert.ToUInt16("1110000"+"000000010",2), // 0x0e002, //  
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
                OpCode = Convert.ToUInt16("1110000"+"000000011",2), // 0x0e003, //  
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
                OpCode = Convert.ToUInt16("1110010"+"000000000",2), // 0x0e400, //  
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
                OpCode = Convert.ToUInt16("1110010"+"000000001",2), // 0x0e401, //  
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
                OpCode = Convert.ToUInt16("1110010"+"000000010",2), // 0x0e402, //  
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
                OpCode = Convert.ToUInt16("1110010"+"000000011",2), // 0x0e403, //  
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
                OpCode = Convert.ToUInt16("1110011"+"000000000",2), // 0x0e600, //  
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
                OpCode = Convert.ToUInt16("1110011"+"000000001",2), // 0x0e601, //  
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
                OpCode = Convert.ToUInt16("1110011"+"000000010",2), // 0x0e602, //  
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
                OpCode = Convert.ToUInt16("1110011"+"000000011",2), // 0x0e603, //  
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
                OpCode = Convert.ToUInt16("1110100"+"000000000",2), // 0x0e800, //  
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
                OpCode = Convert.ToUInt16("1110100"+"000000001",2), // 0x0e801, //  
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
                OpCode = Convert.ToUInt16("1110100"+"000000010",2), // 0x0e802, //  
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
                OpCode = Convert.ToUInt16("1110100"+"000000011",2), // 0x0e803, //  
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
                OpCode = Convert.ToUInt16("1110110"+"000000000",2), // 0x0ec00, //  
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
                OpCode = Convert.ToUInt16("1110110"+"000000001",2), // 0x0ec01, //  
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
                OpCode = Convert.ToUInt16("1110110"+"000000010",2), // 0x0ec02, //  
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
                OpCode = Convert.ToUInt16("1110110"+"000000011",2), // 0x0ec03, //  
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
                OpCode = Convert.ToUInt16("1110111"+"000000000",2), // 0x0ee00, //  
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
                OpCode = Convert.ToUInt16("1110111"+"000000001",2), // 0x0ee01, //  
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
                OpCode = Convert.ToUInt16("1110111"+"000000010",2), // 0x0ee02, //  
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
                OpCode = Convert.ToUInt16("1110111"+"000000011",2), // 0x0ee03, //  
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
                OpCode = Convert.ToUInt16("1111010"+"000000000",2), // 0x0f400, //  
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
                OpCode = Convert.ToUInt16("1111010"+"000000001",2), // 0x0f401, //  
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
                OpCode = Convert.ToUInt16("1111010"+"000000010",2), // 0x0f402, //  
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
                OpCode = Convert.ToUInt16("1111010"+"000000011",2), // 0x0f403, //  
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
                OpCode = Convert.ToUInt16("1111000"+"000000000",2), // 0x0f000, //  
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
                OpCode = Convert.ToUInt16("1111000"+"000000001",2), // 0x0f001, //  
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
                OpCode = Convert.ToUInt16("1111000"+"000000010",2), // 0x0f002, //  
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
                OpCode = Convert.ToUInt16("1111000"+"000000011",2), // 0x0f003, //  
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
                OpCode = Convert.ToUInt16("1111110"+"000000000",2), // 0x0fc00, //  
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
                OpCode = Convert.ToUInt16("1111110"+"000000001",2), // 0x0fc01, //  
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
                OpCode = Convert.ToUInt16("1111110"+"000000010",2), // 0x0fc02, //  
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
                OpCode = Convert.ToUInt16("1111110"+"000000011",2), // 0x0fc03, //  
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
                OpCode = Convert.ToUInt16("1110101"+"000000000",2), // 0x0ea00, //  
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
                OpCode = Convert.ToUInt16("1110101"+"000000001",2), // 0x0ea01, //  
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
                OpCode = Convert.ToUInt16("1110101"+"000000010",2), // 0x0ea02, //  
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
                OpCode = Convert.ToUInt16("1110101"+"000000011",2), // 0x0ea03, //  
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
                OpCode = Convert.ToUInt16("1111101"+"000000000",2), // 0x0fa00, //  
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
                OpCode = Convert.ToUInt16("1111101"+"000000001",2), // 0x0fa01, //  
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
                OpCode = Convert.ToUInt16("1111101"+"000000010",2), // 0x0fa02, //  
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
                OpCode = Convert.ToUInt16("1111101"+"000000011",2), // 0x0fa03, //  
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
                OpCode = Convert.ToUInt16("1111001"+"000000000",2), // 0x0f200, //  
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
                OpCode = Convert.ToUInt16("1111001"+"000000001",2), // 0x0f201, //  
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
                OpCode = Convert.ToUInt16("1111001"+"000000010",2), // 0x0f202, //  
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
                OpCode = Convert.ToUInt16("1111001"+"000000011",2), // 0x0f203, //  
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
                OpCode = Convert.ToUInt16("1111111"+"000000000",2), // 0x0fe00, //  
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
                OpCode = Convert.ToUInt16("1111111"+"000000001",2), // 0x0fe01, //  
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
                OpCode = Convert.ToUInt16("1111111"+"000000010",2), // 0x0fe02, //  
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
                OpCode = Convert.ToUInt16("1111111"+"000000011",2), // 0x0fe03, //  
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
                OpCode = Convert.ToUInt16("1110001"+"000000000",2), // 0x0e200, //  
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
                OpCode = Convert.ToUInt16("1110001"+"000000001",2), // 0x0e201, //  
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
                OpCode = Convert.ToUInt16("1110001"+"000000010",2), // 0x0e202, //  
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
                OpCode = Convert.ToUInt16("1110001"+"000000011",2), // 0x0e203, //  
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
                OpCode = Convert.ToUInt16("0000111"+"000000000",2), // 0x00e00, //  
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
                OpCode = Convert.ToUInt16("0000111"+"000000001",2), // 0x00e01, //  
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
                OpCode = Convert.ToUInt16("1010110"+"000000010",2), // 0x0ac02, //  
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
                OpCode = Convert.ToUInt16("1010110"+"000000001",2), // 0x0ac01, //  
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
                OpCode = Convert.ToUInt16("1100011"+"000000001",2), // 0x0ac01, //  
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
                OpCode = Convert.ToUInt16("1010111"+"000000000",2), // 0x0ae00, //  
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
                OpCode = Convert.ToUInt16("1010111"+"000000001",2), // 0x0ae01, //  
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
                OpCode = Convert.ToUInt16("1011000"+"000000001",2), // 0x0b001, //  
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
                OpCode = Convert.ToUInt16("1000110"+"000000000",2), // 0x08c00, //  
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
                OpCode = Convert.ToUInt16("1000110"+"000000001",2), // 0x08c01, //  
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
                OpCode = Convert.ToUInt16("1000110"+"000000010",2), // 0x08c00, //  
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
                OpCode = Convert.ToUInt16("1000110"+"000000011",2), // 0x08c00, //  
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
                OpCode = Convert.ToUInt16("0000000000000000",2), // 0x00, //  
                Mnemonic = "NOP",
                ExecuteMethod = OperationProcessing.Nop,
                Type = OperationType.Implicit,
                Length = 2
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("1000011000000000",2), // 0x08600, //  
                Mnemonic = "STI",
                ExecuteMethod = OperationProcessing.StiCli,
                Type = OperationType.Implicit,
                Length = 2
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("1000010000000000",2), // 0x08400, //  
                Mnemonic = "RETI",
                ExecuteMethod = OperationProcessing.Reti,
                Type = OperationType.Implicit,
                Length = 2
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("0110111000000000",2), // 0x06e00, //  
                Mnemonic = "RET",
                ExecuteMethod = OperationProcessing.Ret,
                Type = OperationType.Implicit,
                Length = 2
            },
            new Operation()
            {
                OpCode = Convert.ToUInt16("0111110000000000",2), // 0x07c00, //  
                Mnemonic = "POPX",
                Type = OperationType.Implicit,
                ExecuteMethod = OperationProcessing.PopX,
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
