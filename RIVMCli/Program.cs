using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualMachine;
using VirtualMachine.Util;
using VirtualMachine.VMInstructions;

namespace VirtualMachineCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            DiskFormatter.Format(@"C:\VM\VM.disk", @"C:\VM\bootloader.exe");

            VM vm = new VM();

            vm.Start();
        }
    }
}
