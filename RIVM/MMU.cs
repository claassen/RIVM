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

        public void InvalidateTLB()
        {
            _tlb.Invalidate();
        }

        //[
        public int this[int address]
        {
            get
            {
                return Get(address, true, MemoryAccessSize);
            }
            set
            {
                int physicalAddress = GetPhysicalAddress(address);

                if (physicalAddress >= SystemMemoryMap.IO_PORT_START && physicalAddress <= SystemMemoryMap.IO_PORT_END)
                {
                    if (physicalAddress % 4 != 0)
                    {
                        throw new Exception("IO ports are only addressable on 4 byte boundaries");
                    }

                    _ioPorts[(physicalAddress - SystemMemoryMap.IO_PORT_START) / 4].Register = value;
                }
                else if (physicalAddress >= SystemMemoryMap.VGA_MEMORY_START && physicalAddress <= SystemMemoryMap.VGA_MEMORY_END)
                {
                    //Video RAM can only be set with a byte value, so any extra bits in value will simply be truncated
                    _videoCard[physicalAddress - SystemMemoryMap.VGA_MEMORY_START] = (byte)value;
                }
                else if (physicalAddress >= SystemMemoryMap.BIOS_ROM_START && physicalAddress <= SystemMemoryMap.BIOS_ROM_END)
                {
                    throw new InterruptException((int)HardwareInterrupt.PROTECTION_FAULT);
                }
                else
                {
                    //Native system is big endian, so LSB will be at start of byte array
                    byte[] val = BitConverter.GetBytes(value);

                    //Our system is little endian, so we need to put LSB at end of byte array
                    for (int i = 0; i < MemoryAccessSize; i++)
                    {
                        _ram[physicalAddress + MemoryAccessSize - (i + 1)] = val[i];
                    }
                }
            }
        }

        public int Get(int address, bool enforceProtection, int memoryAccessSize)
        {
            int physicalAddress = GetPhysicalAddress(address);

            if (physicalAddress >= SystemMemoryMap.IO_PORT_START && physicalAddress <= SystemMemoryMap.IO_PORT_END)
            {
                if (physicalAddress % 4 != 0)
                {
                    throw new Exception("IO ports are only addressable on 4 byte boundaries");
                }

                IOPort ioPort = _ioPorts[(physicalAddress - SystemMemoryMap.IO_PORT_START) / 4]; 

                return ioPort != null ? ioPort.Register : 0;
            }
            else if (physicalAddress >= SystemMemoryMap.VGA_MEMORY_START && physicalAddress <= SystemMemoryMap.VGA_MEMORY_END)
            {
                if (enforceProtection)
                {
                    throw new InterruptException((int)HardwareInterrupt.PROTECTION_FAULT);
                }
                else
                {
                    //For debugging only
                    return _videoCard[physicalAddress - SystemMemoryMap.VGA_MEMORY_START];
                }
            }
            else if (physicalAddress >= SystemMemoryMap.BIOS_ROM_START && physicalAddress <= SystemMemoryMap.BIOS_ROM_END)
            {
                byte[] bytes = new byte[4];

                for (int i = 0; i < memoryAccessSize; i++)
                {
                    bytes[i] = _bios[physicalAddress - SystemMemoryMap.BIOS_ROM_START + i];
                }

                return BitConverter.ToInt32(bytes.Reverse().ToArray(), 0);
            }
            else
            {
                byte[] bytes = new byte[4];

                for (int i = 0; i < memoryAccessSize; i++)
                {
                    bytes[i] = _ram[physicalAddress + i];
                }

                return BitConverter.ToInt32(bytes.Reverse().ToArray(), 0);
            }
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
