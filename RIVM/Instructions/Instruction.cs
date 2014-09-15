using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIVM.Instructions
{
    //11111111 1xxx 1111 1111 1111 1111     1111    [next instruction]
    //OPCODE   MB   r1   r2   r3   <unused> # bytes immediate value (MB = 1)
    public abstract class Instruction
    {
        protected OpCodes _opCode;
        protected Register _r1;
        protected Register _r2;
        protected Register _r3;
        protected int _operandBytes;

        public bool HasImmediate;
        public int Immediate;

        public Instruction(int value)
        {
            _opCode = (OpCodes)((value & 0xFF000000) >> 24);
            HasImmediate =     (value & 0x00100000) != 0;
            _r1 =   (Register)((value & 0x000F0000) >> 16);
            _r2 =   (Register)((value & 0x0000F000) >> 12);
            _r3 =   (Register)((value & 0x00000F00) >> 8);
            _operandBytes =      value & 0x0000000F;
        }

        public int Encode()
        {
            return ((int)_opCode << 24) |
                   ((HasImmediate ? 1 : 0) << 20 |
                   ((int)_r1 << 16) |
                   ((int)_r2 << 12) |
                   ((int)_r3 << 8) |
                    (byte)_operandBytes);
        }

        public void Execute(CPU cpu)
        {
            if (KernelModeOnly && !cpu.KernelMode)
            {
                throw new Exception("Attemp to execute privileged instruction from user mode");
            }

            ExecuteImpl(cpu);
        }

        protected abstract bool KernelModeOnly { get; }
        protected abstract void ExecuteImpl(CPU cpu);
    }

    public abstract class UserModeInstruction : Instruction
    {
        public UserModeInstruction(int instr)
            : base(instr)
        {
        }

        protected override bool KernelModeOnly
        {
            get
            {
                return false;
            }
        }
    }

    public abstract class KernelModeInstruction : Instruction
    {
        public KernelModeInstruction(int instr)
            : base(instr)
        {
        }

        protected override bool KernelModeOnly
        {
            get 
            { 
                return true; 
            }
        }
    }

    public class AddR : UserModeInstruction
    {
        public AddR(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.Registers[_r1] = cpu.Registers[_r2] + cpu.Registers[_r3];
        }
    }

    public class AndR : UserModeInstruction
    {
        public AndR(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.Registers[_r1] = cpu.Registers[_r2] & cpu.Registers[_r3];
        }
    }

    public class Call : UserModeInstruction
    {
        public Call(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.Memory[cpu.Registers[Register.SP]] = cpu.Registers[Register.IP];

            cpu.Registers[Register.SP] += 4;

            cpu.Registers[Register.IP] = Immediate;
        }
    }

    public class Ret : UserModeInstruction
    {
        public Ret(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.Registers[Register.SP] -= 4;
            cpu.Registers[Register.IP] = cpu.Memory[cpu.Registers[Register.SP]];
        }
    }

    public class CmpI : UserModeInstruction
    {
        public CmpI(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            int diff = cpu.Registers[_r1] - Immediate;

            if (diff < 0)
            {
                cpu.Flag = CompareFlag.LT;
            }
            else if (diff == 0)
            {
                cpu.Flag = CompareFlag.EQ;
            }
            else if (diff > 0)
            {
                cpu.Flag = CompareFlag.GT;
            }
        }
    }

    public class CmpR : UserModeInstruction
    {
        public CmpR(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            int diff = cpu.Registers[_r1] - cpu.Registers[_r2];

            if (diff < 0)
            {
                cpu.Flag = CompareFlag.LT;
            }
            else if (diff == 0)
            {
                cpu.Flag = CompareFlag.EQ;
            }
            else if (diff > 0)
            {
                cpu.Flag = CompareFlag.GT;
            }
        }
    }

    public class DivR : UserModeInstruction
    {
        public DivR(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.Registers[_r1] = cpu.Registers[_r2] / cpu.Registers[_r3];
        }
    }

    public class Halt : UserModeInstruction
    {
        public Halt(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.Halted = true;
        }
    }

    public class JEqI : UserModeInstruction
    {
        public JEqI(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            if (cpu.Flag == CompareFlag.EQ)
            {
                cpu.Registers[Register.IP] = Immediate;
            }
        }
    }

    public class JGeI : UserModeInstruction
    {
        public JGeI(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            if (cpu.Flag == CompareFlag.GE)
            {
                cpu.Registers[Register.IP] = Immediate;
            }
        }
    }

    public class JGtI : UserModeInstruction
    {
        public JGtI(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            if (cpu.Flag == CompareFlag.GT)
            {
                cpu.Registers[Register.IP] = Immediate;
            }
        }
    }

    public class JmpI : UserModeInstruction
    {
        public JmpI(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.Registers[Register.IP] = Immediate;
        }
    }

    public class JLeI : UserModeInstruction
    {
        public JLeI(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            if (cpu.Flag == CompareFlag.LE)
            {
                cpu.Registers[Register.IP] = Immediate;
            }
        }
    }

    public class JLtI : UserModeInstruction
    {
        public JLtI(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            if (cpu.Flag == CompareFlag.LT)
            {
                cpu.Registers[Register.IP] = Immediate;
            }
        }
    }

    public class JNeI : UserModeInstruction
    {
        public JNeI(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            if (!cpu.Flag.HasFlag(CompareFlag.EQ))
            {
                cpu.Registers[Register.IP] = Immediate;
            }
        }
    }

    public class JmpR : UserModeInstruction
    {
        public JmpR(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.Registers[Register.IP] = cpu.Registers[_r1];
        }
    }

    public class LoadI : UserModeInstruction
    {
        public LoadI(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.Registers[_r1] = cpu.Memory[Immediate] & (int)BitMaskHelper.CreateMask(_operandBytes);
        }
    }

    public class LoadR : UserModeInstruction
    {
        public LoadR(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.Registers[_r1] = cpu.Memory[cpu.Registers[_r2]] & (int)BitMaskHelper.CreateMask(_operandBytes);
        }
    }

    public class LoadIR : UserModeInstruction
    {
        public LoadIR(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.Registers[_r1] = cpu.Memory[cpu.Registers[_r2] + Immediate] & (int)BitMaskHelper.CreateMask(_operandBytes);
        }
    }

    public class MemCpy : UserModeInstruction
    {
        public MemCpy(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            //TODO: this might be totally broken
            for (int i = 0; i < Immediate / 4; i++)
            {
                cpu.Memory[cpu.Registers[_r1] + i] = cpu.Memory[cpu.Registers[_r2] + i];
            }
        }
    }

    public class MovI : UserModeInstruction
    {
        public MovI(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.Registers[_r1] = Immediate;

            if (_r1 == Register.CR && Immediate == 1)
            {
                //Setting CR to 1 turns on page table
                cpu.Memory.PageTableEnabled = true;
            }
        }
    }

    public class MovR : UserModeInstruction
    {
        public MovR(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.Registers[_r1] = cpu.Registers[_r2];

            if (_r1 == Register.CR && cpu.Registers[_r2] == 1)
            {
                //Setting CR to 1 turns on page table
                cpu.Memory.PageTableEnabled = true;
            }
        }
    }

    public class MultR : UserModeInstruction
    {
        public MultR(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.Registers[_r1] = cpu.Registers[_r2] * cpu.Registers[_r3];
        }
    }

    public class Noop : UserModeInstruction
    {
        public Noop(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            //NOOP
        }
    }

    public class OrR : UserModeInstruction
    {
        public OrR(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.Registers[_r1] = cpu.Registers[_r2] | cpu.Registers[_r3];
        }
    }

    public class PopR : UserModeInstruction
    {
        public PopR(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.Registers[Register.SP] -= 4;

            cpu.Registers[_r1] = cpu.Memory[cpu.Registers[Register.SP]];

            //byte[] bytes = new byte[4];

            //for (int i = 0; i < 4; i++)
            //{
            //    bytes[i] = cpu.Memory[cpu.Registers[Register.SP] + i];
            //}

            //cpu.Registers[_r1] = BitConverter.ToInt32(bytes.Reverse().ToArray(), 0);
        }
    }

    public class PushI : UserModeInstruction
    {
        public PushI(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.Memory[cpu.Registers[Register.SP]] = Immediate;

            //byte[] bytes = BitConverter.GetBytes(Immediate).Reverse().ToArray();

            //for (int i = 0; i < 4; i++)
            //{
            //    cpu.Memory[cpu.Registers[Register.SP] + i] = bytes[i];
            //}

            cpu.Registers[Register.SP] += 4;
        }
    }

    public class PushR : UserModeInstruction
    {
        public PushR(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.Memory[cpu.Registers[Register.SP]] = cpu.Registers[_r1];

            cpu.Registers[Register.SP] += 4;
        }
    }

    public class ShlR : UserModeInstruction
    {
        public ShlR(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.Registers[_r1] = cpu.Registers[_r2] << cpu.Registers[_r3];
        }
    }

    public class ShrR : UserModeInstruction
    {
        public ShrR(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.Registers[_r1] = cpu.Registers[_r2] >> cpu.Registers[_r3];
        }
    }

    public class StoreI : UserModeInstruction
    {
        public StoreI(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.Memory[Immediate] = cpu.Registers[_r1] & (int)BitMaskHelper.CreateMask(_operandBytes);

            //byte[] bytes = BitConverter.GetBytes(cpu.Registers[_r1]).Reverse().ToArray();

            //for (int i = 0; i < _operandBytes; i++)
            //{
            //    cpu.Memory[Immediate + i] = bytes[i];
            //}
        }
    }

    public class StoreR : UserModeInstruction
    {
        public StoreR(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.Memory[cpu.Registers[_r1]] = cpu.Registers[_r2] & (int)BitMaskHelper.CreateMask(_operandBytes);

            //byte[] bytes = BitConverter.GetBytes(cpu.Registers[_r2]).Reverse().ToArray();

            //for (int i = 0; i < _operandBytes; i++)
            //{
            //    cpu.Memory[cpu.Registers[_r1] + i] = bytes[i];
            //}
        }
    }

    public class StoreIR : UserModeInstruction
    {
        public StoreIR(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.Memory[cpu.Registers[_r2] + Immediate] = cpu.Registers[_r1] & (int)BitMaskHelper.CreateMask(_operandBytes);

            //byte[] bytes = BitConverter.GetBytes(cpu.Registers[_r1]).Reverse().ToArray();

            //for (int i = 0; i < _operandBytes; i++)
            //{
            //    cpu.Memory[cpu.Registers[_r2] + Immediate + i] = bytes[i];
            //}
        }
    }

    public class SubR : UserModeInstruction
    {
        public SubR(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.Registers[_r1] = cpu.Registers[_r2] - cpu.Registers[_r3];
        }
    }

    public class XorR : UserModeInstruction
    {
        public XorR(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.Registers[_r1] = cpu.Registers[_r2] ^ cpu.Registers[_r3];
        }
    }

    public class Sysent : UserModeInstruction
    {
        public Sysent(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            //Context switch?
            //throw exception instead, or is this how software interrupts work? vs hardware are generated by exception
            cpu.KernelMode = true;
            cpu.Registers[Register.IP] = cpu.Memory[cpu.Registers[Register.IDT] + Immediate];
        }
    }

    public class Sysex : UserModeInstruction
    {
        public Sysex(int instr)
            : base(instr)
        {
        }

        protected override void ExecuteImpl(CPU cpu)
        {
            cpu.KernelMode = false;
        }
    }

    //get rid of this
    //public class FSRead : KernelModeInstruction
    //{
    //    public FSRead(int instr)
    //        : base(instr)
    //    {
    //    }

    //    protected override void ExecuteImpl(CPU cpu)
    //    {
    //        for (int i = 0; i < Immediate; i++)
    //        {
    //            cpu.Memory[cpu.Registers[_r1] + i] = cpu.Disk[cpu.Registers[_r2] + i];
    //        }
    //    }
    //}

    //get rid of this
    //public class FSWrite : KernelModeInstruction
    //{
    //    public FSWrite(int instr)
    //        : base(instr)
    //    {
    //    }

    //    protected override void ExecuteImpl(CPU cpu)
    //    {
    //        for (int i = 0; i < Immediate; i++)
    //        {
    //            cpu.Disk[cpu.Registers[_r1] + i] = cpu.Memory[cpu.Registers[_r2] + i];
    //        }
    //    }
    //}
}
