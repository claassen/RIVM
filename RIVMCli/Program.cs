using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RIVM.Util;

namespace RIVMCli
{
    class Program
    {
        static void Main(string[] args)
        {
            DiskFormatter.Format(@"C:\VM\vm.disk", @"C:\VM\bootloader.exe", @"C:\VM\kernel.exe");
        }
    }
}
