using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualMachine
{
    public class MemoryController
    {
        public bool PageTableEnabled;
        public int _ptBase;
        private byte[] _ram;
        private IOPort[] _ioPorts;
        private BIOS _bios;
        private TranslationLookasideBuffer _tlb;

        public MemoryController(int size, BIOS bios, IOPort[] ioPorts)
        {
            _bios = bios;
            _ioPorts = ioPorts;
            _ram = new byte[size];
            
            _tlb = new TranslationLookasideBuffer();
        }

        public int this[int address]
        {
            get
            {
                int physicalAddress = GetPhysicalAddress(address);

                if (physicalAddress <= SystemMemoryMap.BIOS_ROM_END)
                {
                    return _bios[physicalAddress - SystemMemoryMap.BIOS_ROM_START];
                }
                else if (physicalAddress <= SystemMemoryMap.IO_PORT_END)
                {
                    return _ioPorts[physicalAddress - SystemMemoryMap.IO_PORT_START].Register;
                }
                else
                {
                    return BitConverter.ToInt32(_ram, physicalAddress - SystemMemoryMap.RAM_START);
                }
            }
            set
            {
                int physicalAddress = GetPhysicalAddress(address);

                if (physicalAddress <= SystemMemoryMap.BIOS_ROM_END)
                {
                    throw new HardwareInterruptException(HardwareInterrupt.PROTECTION_FAULT);
                }
                else if (physicalAddress <= SystemMemoryMap.IO_PORT_END)
                {
                    _ioPorts[physicalAddress - SystemMemoryMap.IO_PORT_START].Register = value;
                }
                else
                {
                    byte[] val = BitConverter.GetBytes(value).Reverse().ToArray();

                    for (int i = 0; i < 4; i++)
                    {
                        _ram[physicalAddress - SystemMemoryMap.RAM_START + i] = val[i];
                    }
                }
            }
        }

        private int GetPhysicalAddress(int address)
        {
            if (!PageTableEnabled)
            {
                return address;
            }
            else
            {
                int vpn = (address & Constants.VPN_MASK) >> Constants.VPN_SHIFT;
                int pfn;

                if (_tlb.GetPFN(vpn, out pfn))
                {
                    int offset = (address & Constants.OFFSET_MASK);
                    int physicalAddress = (pfn << Constants.PPN_SHIFT) | offset;

                    return physicalAddress;
                }
                else
                {
                    throw new HardwareInterruptException(HardwareInterrupt.TLB_MISS);
                }
            }
        }
    }
}
