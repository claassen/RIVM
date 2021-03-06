using System;
using System.Collections.Generic;

namespace RIVM.Instructions
{
    public static class InstructionDecoder
    {
        public static Dictionary<OpCodes, Func<int, Instruction>> _instructions = new Dictionary<OpCodes, Func<int, Instruction>>() 
        {
            { OpCodes.HALT,    (Value) => new Halt(Value) },
            { OpCodes.ADDR,    (value) => new AddR(value) },
            { OpCodes.ANDR,    (value) => new AndR(value) },
            { OpCodes.CALL,    (value) => new Call(value) },
            { OpCodes.RET,     (value) => new Ret(value) },
            { OpCodes.CMPI,    (value) => new CmpI(value) },
            { OpCodes.CMPR,    (value) => new CmpR(value) },
            { OpCodes.DIVR,    (value) => new DivR(value) },
            { OpCodes.JEQI,    (value) => new JEqI(value) },
            { OpCodes.JGEI,    (value) => new JGeI(value) },
            { OpCodes.JGTI,    (value) => new JGtI(value) },
            { OpCodes.JMPI,    (value) => new JmpI(value) },
            { OpCodes.JLEI,    (value) => new JLeI(value) },
            { OpCodes.JLTI,    (value) => new JLtI(value) },
            { OpCodes.JNEI,    (value) => new JNeI(value) },
            { OpCodes.JMPR,    (value) => new JmpR(value) },
            { OpCodes.LOADI,   (value) => new LoadI(value) },
            { OpCodes.LOADR,   (value) => new LoadR(value) },
            { OpCodes.LOADIR,  (value) => new LoadIR(value) },
            { OpCodes.MEMCPY,  (value) => new MemCpy(value) },
            { OpCodes.MOVI,    (value) => new MovI(value) },
            { OpCodes.MOVR,    (value) => new MovR(value) },
            { OpCodes.MULTR,   (value) => new MultR(value) },
            { OpCodes.NOOP,    (value) => new Noop(value) },
            { OpCodes.ORR,     (value) => new OrR(value) },
            { OpCodes.POPR,    (value) => new PopR(value) },
            { OpCodes.PUSHI,   (value) => new PushI(value) },
            { OpCodes.PUSHR,   (value) => new PushR(value) },
            { OpCodes.SHLR,    (value) => new ShlR(value) },
            { OpCodes.SHRR,    (value) => new ShrR(value) },
            { OpCodes.STOREI,  (value) => new StoreI(value) },
            { OpCodes.STORER,  (value) => new StoreR(value) },
            { OpCodes.STOREIR, (value) => new StoreIR(value) },
            { OpCodes.SUBR,    (value) => new SubR(value) },
            { OpCodes.XORR,    (value) => new XorR(value) },

            { OpCodes.INT,     (value) => new Int(value) },
            { OpCodes.IRET,    (value) => new IRet(value) },
            { OpCodes.CLI,     (value) => new Cli(value) },
            { OpCodes.STI,     (value) => new Sti(value) },
            { OpCodes.SETIDT,  (value) => new SetIDT(value) },
            { OpCodes.SETPT,   (value) => new SetPT(value) },
            { OpCodes.TLBI,    (value) => new Tlbi(value) },

            { OpCodes.BRK,     (value) => new Break(value) }
        };

        public static Instruction Decode(int value)
        {
            OpCodes opCode = (OpCodes)((value & 0xFF000000) >> 24);

            if (_instructions.ContainsKey(opCode))
            {
                return _instructions[opCode](value);
            }
            else
            {
                return new Halt(0);

                throw new Exception("Unknown opcode: " + opCode.ToString());
            }
        }
    }
}