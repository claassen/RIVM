using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualMachine
{
    public static class Constants
    {
        //32 bit real mode address space
        public static readonly int MEMORY_BITS = 32;
        public static readonly long MEMORY_SIZE = (int)Math.Pow(2, MEMORY_BITS);

        //16 bit virtual address space
        public static readonly int ADDRESS_SPACE_BITS = 16;
        public static readonly int ADDRESS_SPACE_SIZE = (int)Math.Pow(2, ADDRESS_SPACE_BITS);
        
        public static readonly int PAGE_SIZE = 1024;

        public static readonly int NUM_VPN_PAGES = ADDRESS_SPACE_SIZE / PAGE_SIZE;
        public static readonly int VPN_SHIFT = ADDRESS_SPACE_BITS - (int)Math.Log(NUM_VPN_PAGES, 2);
        public static readonly int VPN_MASK = (NUM_VPN_PAGES - 1) << VPN_SHIFT;

        public static readonly int NUM_PPN_PAGES = (int)(MEMORY_SIZE / PAGE_SIZE);
        public static readonly int PPN_SHIFT = MEMORY_BITS - (int)Math.Log(NUM_PPN_PAGES, 2);
        public static readonly int PPN_MASK = (NUM_PPN_PAGES - 1) << PPN_SHIFT;

        public static readonly int OFFSET_MASK = PAGE_SIZE - 1;
    }
}
