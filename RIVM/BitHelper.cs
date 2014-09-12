using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualMachine
{
    public static class BitMaskHelper
    {
        public static uint CreateMask(int numBytes)
        {
            switch (numBytes)
            {
                case 1:
                    return 0x000000FF;
                case 2:
                    return 0x0000FFFF;
                case 3:
                    return 0x00FFFFFF;
                case 4:
                    return 0xFFFFFFFF;
                default:
                    throw new Exception("Maximum 4 bytes supported for bit mask");
            }
        }
    }
}
