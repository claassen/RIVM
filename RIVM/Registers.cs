using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualMachine
{
    public enum Register
    {
        R0 = 0,
        R1,
        R2,
        R3,
        SP,
        BP,
        IP,
        IDT,
        CR
    }

    public class Registers
    {
        private int[] _registers;

        public Registers()
        {
            _registers = new int[Enum.GetNames(typeof(Register)).Length];
        }

        public int this[Register r]
        {
            get
            {
                return _registers[(int)r];
            }
            set
            {
                _registers[(int)r] = value;
            }
        }
    }
}
