
namespace RIVM.Instructions
{
    public enum OpCodes : byte
    {
        HALT = 0,
        ADDR,
        ANDR,
        CALL,
        RET,
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

        INT,    //software interrupt
        IRET,   //return from interrupt
        CLI,    //disable interrupts
        STI,    //enable interrupts
        SETIDT, //set IDT base pointer
        SETPT,  //set page table base pointer
        TLBI    //invalidate TLB
    } //max 64 op codes
}