using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RIVM;
using RIVM.Util;

namespace RIVMApp
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            DiskFormatter.Format(@"C:\VM\vm.disk", @"C:\VM\bootloader.exe", @"C:\VM\kernel.exe");

            Application.Run(new MainDisplay());
        }
    }
}
