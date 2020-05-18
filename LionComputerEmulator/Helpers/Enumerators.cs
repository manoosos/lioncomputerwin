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

    public enum Interrupt : int
    {
        INT_SPI = 4,
        SPI_INIT = 11,
        SPISEND = 12,
        READSEC = 13,
        WRITESEC = 14
    }
}
