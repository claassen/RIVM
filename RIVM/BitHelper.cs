using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIVM
{
    public static class BitHelper
    {
        public static int ExtractBytes(int val, int numBytes)
        {
            return (val & (int)CreateMask(numBytes)) >> ((4 - numBytes) * 8);
        }

        private static uint CreateMask(int numBytes)
        {
            switch (numBytes)
            {
                case 1:
                    return 0xFF000000;
                case 2:
                    return 0xFFFF0000;
                case 4:
                    return 0xFFFFFFFF;
                default:
                    throw new Exception("Maximum 4 bytes supported for bit mask");
            }
        }
    }
}
