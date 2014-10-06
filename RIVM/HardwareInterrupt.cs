using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIVM
{
    public enum HardwareInterrupt
    {
        PROTECTION_FAULT,
        PAGE_FAULT
    }

    public enum SoftwareInterrupt
    {
        SYSCALL = 80
    }

    public class InterruptException : Exception
    {
        public int InterruptNumber { get; set; }

        public InterruptException(int interrupt)
        {
            InterruptNumber = interrupt;
        }
    }
}
