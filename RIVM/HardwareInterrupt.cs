using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIVM
{
    public enum HardwareInterrupt
    {
        SEG_FAULT = 0,
        PROTECTION_FAULT,
        TLB_MISS,
        DMA_COMPLETE
    }

    public class HardwareInterruptException : Exception
    {
        public HardwareInterrupt Interrupt { get; set; }

        public HardwareInterruptException(HardwareInterrupt interrupt)
        {
            Interrupt = interrupt;
        }
    }
}
