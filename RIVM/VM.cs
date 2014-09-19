using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RIVM.IODevices;

namespace RIVM
{
    public class VM
    {
        private CPU _cpu;
        private MMU _memory;
        
        public VM()
        {
            _memory = new MMU((int)Math.Pow(2, 30), new BIOS(@"C:\VM\bios.exe"), CreateIODevices()); //1GB RAM 
            _cpu = new CPU(_memory);
        }

        private IOPort[] CreateIODevices()
        {
            IOPort[] ioPorts = new IOPort[SystemMemoryMap.IO_PORT_END - SystemMemoryMap.IO_PORT_START + 1];

            var disk = new DiskController(@"C:\VM\VM.disk");

            ioPorts[0] = new IOPort(() => disk.ControlRegister, (val) => disk.ControlRegister = val);
            ioPorts[1] = new IOPort(() => disk.AddressRegister, (val) => disk.AddressRegister = val);
            ioPorts[2] = new IOPort(() => disk.IOByteCountRegister, (val) => disk.IOByteCountRegister = val);
            ioPorts[3] = new IOPort(() => disk.DataRegister, (val) => disk.DataRegister = val);

            return ioPorts;
        }

        public void Start()
        {
            _cpu.Start();
        }

        public void RunProgram(string path)
        {
            using (var fs = new BinaryReader(new FileStream(path, FileMode.Open)))
            {
                int entryAddress = fs.ReadInt32();

                for (int i = 0; i < fs.BaseStream.Length - 8; i++)
                {
                    _memory[i] = fs.ReadByte();
                }

                _cpu.Registers[Register.IP] = entryAddress;
                _cpu.KernelMode = false;
                _cpu.Registers[Register.BP] = (int)fs.BaseStream.Length;
                _cpu.Registers[Register.SP] = (int)fs.BaseStream.Length;
                _cpu.Start();
            }
        }
    }
}
