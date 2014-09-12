using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualMachine.VMInstructions;

namespace VirtualMachine
{
    public enum BIOSInterruptHandlerAddress
    {
        BOOT = 0
    }

    public class BIOS
    {
        private byte[] _rom;

        public BIOS(string romFile)
        {
            _rom = new byte[SystemMemoryMap.BIOS_ROM_END - SystemMemoryMap.BIOS_ROM_START + 1];

            using (var fs = new FileStream(romFile, FileMode.Open))
            {
                fs.Read(_rom, 0, _rom.Length);
            }
        }

        public int this[int index]
        {
            get
            {
                return BitConverter.ToInt32(_rom, index);
            }
            private set
            {
                byte[] val = BitConverter.GetBytes(value).Reverse().ToArray();

                for (int i = 0; i < 4; i++)
                {
                    _rom[index + i] = val[i];
                }
            }
        }
    }
}
