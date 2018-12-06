namespace LionComputerEmulator
{
    public enum OperationContext : int
    {
        Absolute,
        Relative
    }

    public enum OperationType : int
    {
        Undefined,
        Memory,
        Arithmetic,
        Branch,
        Conditional,
        InputOutput,
        Implicit
    }

    public enum OperationWidth : int
    {
        Word,
        Byte,
        Mixed
    }

    public enum AddressingMode : int
    {
        RegisterDirect = 0,
        RegisterReference = 2,
        Immediate = 1,
        MemoryReference = 3,
        ImmediateQuick,
        Internal,
        StatusRegister,
        StackPointer,
        ProgramCounter,
    }

    public enum OperandNotation : int
    {
        Source,
        Destination
    }
}
