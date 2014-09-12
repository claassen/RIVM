
namespace VirtualMachine.VMInstructions
{
    public enum OpCodes : byte
    {
        HALT = 0,
        ADDR,
        ANDR,
        CALL,
        CMPI,
        CMPR,
        DIVR,
        JEQI,
        JGEI,
        JGTI,
        JMPI,
        JLEI,
        JLTI,
        JNEI,
        JMPR,
        LOADI,
        LOADR,
        LOADIR,
        MEMCPY,
        MOVI,
        MOVR,
        MULTR,
        NOOP,
        ORR,
        POPR,
        PUSHI,
        PUSHR,
        SHLR,
        SHRR,
        STOREI,
        STORER,
        STOREIR,
        SUBR,
        XORR,

        SYSENT,
        SYSEX,
        //FSREAD,
        //FSWRITE
    } //max 64 op codes
}