using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RIVM.IODevices;

namespace RIVM
{
    public class MMU
    {
        public int MemoryAccessSize = 4;
        public bool PTEnabled;
        public int PTPointer;

        private byte[] _ram;
        private IOPort[] _ioPorts;
        private VideoCard _videoCard;
        private BIOS _bios;
        private TLB _tlb;
       
        public MMU(int size, BIOS bios, IOPort[] ioPorts, VideoCard videoCard)
        {
            _bios = bios;
            _ioPorts = ioPorts;
            _videoCard = videoCard;
            _ram = new byte[size];
            
            _tlb = new TLB();
        }

        //public IEnumerable<int> GetMemoryForDebugging(int start, int end)
        //{
        //    for (int i = start; i < end /*(SystemMemoryMap.RAM_START + _ram.Length / 4) / 4*/; i += 4)
        //    {
        //        yield return Get(i, false);
        //    }
        //}

        public void InvalidateTLB()
        {
            _tlb.Invalidate();
        }

        public int this[int address]
        {
            get
            {
                return Get(address, true);
            }
            set
            {
                int physicalAddress = GetPhysicalAddress(address);

                if (physicalAddress == 15)
                {
                    throw new Exception("wtf");
                }

                if (physicalAddress >= SystemMemoryMap.IO_PORT_START && physicalAddress <= SystemMemoryMap.IO_PORT_END)
                {
                    _ioPorts[(physicalAddress - SystemMemoryMap.IO_PORT_START) / 4].Register = value;
                }
                else if (physicalAddress >= SystemMemoryMap.VGA_MEMORY_START && physicalAddress <= SystemMemoryMap.VGA_MEMORY_END)
                {
                    _videoCard[physicalAddress - SystemMemoryMap.VGA_MEMORY_START] = (byte)value; //TODO: fix this
                }
                else if (physicalAddress >= SystemMemoryMap.BIOS_ROM_START && physicalAddress <= SystemMemoryMap.BIOS_ROM_END)
                {
                    throw new InterruptException((int)HardwareInterrupt.PROTECTION_FAULT);
                }
                else
                {
                    byte[] val = BitConverter.GetBytes(value);

                    for (int i = 0; i < MemoryAccessSize; i++)
                    {
                        _ram[physicalAddress + MemoryAccessSize - (i + 1)] = val[i];
                    }
                }
            }
        }

        public int Get(int address, bool enforceProtection)
        {
            int physicalAddress = GetPhysicalAddress(address);

            if (physicalAddress >= SystemMemoryMap.IO_PORT_START && physicalAddress <= SystemMemoryMap.IO_PORT_END)
            {
                //IO Ports
                IOPort ioPort = _ioPorts[(physicalAddress - SystemMemoryMap.IO_PORT_START) / 4]; 

                return ioPort != null ? ioPort.Register : 0;
            }
            else if (physicalAddress >= SystemMemoryMap.VGA_MEMORY_START && physicalAddress <= SystemMemoryMap.VGA_MEMORY_END)
            {
                //VGA memory
                if (enforceProtection)
                {
                    throw new InterruptException((int)HardwareInterrupt.PROTECTION_FAULT);
                }
                else
                {
                    return _videoCard[physicalAddress - SystemMemoryMap.VGA_MEMORY_START];
                }
            }
            else if (physicalAddress >= SystemMemoryMap.BIOS_ROM_START && physicalAddress <= SystemMemoryMap.BIOS_ROM_END)
            {
                return _bios[physicalAddress - SystemMemoryMap.BIOS_ROM_START];
            }

            byte[] bytes = new byte[4];

            for (int i = 0; i < MemoryAccessSize; i++)
            {
                bytes[i] = _ram[physicalAddress + i];
            }

            return BitConverter.ToInt32(bytes.Reverse().ToArray(), 0);
        }

        private int GetPhysicalAddress(int address)
        {
            if (!PTEnabled)
            {
                return address;
            }
            else
            {
                int vpn = (address & Constants.VPN_MASK) >> Constants.VPN_SHIFT;
                int pfn;
            RETRY:
                if (_tlb.GetPFN(vpn, out pfn))
                {
                    int offset = (address & Constants.OFFSET_MASK);
                    int physicalAddress = (pfn << Constants.PPN_SHIFT) | offset;

                    return physicalAddress;
                }
                else
                {
                    //TLB miss

                    //Either this will update the TLB with correct entry or throw an exception (page fault)
                    GetTLBEntryFromPageTable(vpn);

                    goto RETRY;
                }
            }
        }

        private void GetTLBEntryFromPageTable(int vpn)
        {
            int pfn = -1;
            //TODO: Search memory for page table entry that matches VPN and extract PFN
            //(the page table MUST be in memory otherwise we would cause another TLB miss and be in serious trouble)

            if (pfn != -1)
            {
                //Update TLB with valid entry
                _tlb.AddEntry(vpn, pfn);
            }
            else
            {
                throw new InterruptException((int)HardwareInterrupt.PAGE_FAULT);
            }
        }
    }
}
