using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIVM
{
    public enum BIOSInterruptHandlerAddress
    {
        BOOT = 0
    }

    public class BIOS
    {
        public const int MAX_ROM_SIZE = SystemMemoryMap.BIOS_ROM_END - SystemMemoryMap.BIOS_ROM_START + 1; 

        private byte[] _rom;

        public BIOS(string romFile)
        {
            _rom = new byte[MAX_ROM_SIZE];

            using (var fs = new FileStream(romFile, FileMode.Open))
            {
                if (fs.Length > MAX_ROM_SIZE)
                {
                    throw new Exception("Provided BIOS code is too large. Max ROM size is " + MAX_ROM_SIZE + "bytes and the provided object code is " + fs.Length + " bytes.");
                }

                fs.Read(_rom, 0, _rom.Length);
            }
        }

        public BIOS(byte[] objectCode)
        {
            if (objectCode.Length > MAX_ROM_SIZE)
            {
                throw new Exception("Provided BIOS code is too large. Max ROM size is " + MAX_ROM_SIZE + "bytes and the provided object code is " + objectCode.Length + " bytes.");
            }

            _rom = new byte[MAX_ROM_SIZE];

            Buffer.BlockCopy(objectCode, 0, _rom, 0, objectCode.Length);
        }

        public byte this[int index]
        {
            get
            {
                return _rom[index];
            }
            private set
            {
                _rom[index] = value;
            }
        }
    }
}
