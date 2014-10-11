using System;
using System.Collections.Generic;
using System.Drawing;
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
        private VideoCard _video;
        
        public VM(int memorySize, string biosExe, string diskFile, Graphics graphics)
        {
            _video = new VideoCard(graphics);

            _cpu = new CPU(new MMU(memorySize, new BIOS(biosExe), CreateIODevices(diskFile), _video));
        }

        private IOPort[] CreateIODevices(string diskFile)
        {
            IOPort[] ioPorts = new IOPort[SystemMemoryMap.IO_PORT_END - SystemMemoryMap.IO_PORT_START + 1];

            var disk = new DiskController(@"C:\VM\vm.disk");

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

        public void ResizeDisplay(Graphics newGraphics)
        {
            _video.ResizeDisplay(newGraphics);
        }
    }
}
