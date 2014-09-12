using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualMachine
{
    public class TranslationLookasideBuffer
    {
        private TLBEntry[] _entries;

        public TranslationLookasideBuffer()
        {
            _entries = new TLBEntry[16];
        }

        public bool GetPFN(int vpn, out int pfn)
        {
            var entry = _entries.FirstOrDefault(e => e.VPN == vpn && e.Valid);

            if (entry != null)
            {
                pfn = entry.PFN;
                return true;
            }
            else
            {
                pfn = -1;
                return false;
            }
        }

        public void AddEntry(int vpn, int pfn)
        {
            bool success = false;

            for (int i = 0; i < _entries.Length; i++)
            {
                if (_entries[i] == null || !_entries[i].Valid)
                {
                    success = true;
                    _entries[i] = new TLBEntry()
                    {
                        Valid = true,
                        VPN = vpn,
                        PFN = pfn
                    };
                }
            }

            if (!success)
            {
                //Eject a random entry
                int index = new Random().Next(_entries.Length);
                _entries[index] = new TLBEntry()
                {
                    Valid = true,
                    VPN = vpn,
                    PFN = pfn
                };
            }
        }

        public void Invalidate()
        {
            for (int i = 0; i < _entries.Length; i++)
            {
                if (_entries[i] != null)
                {
                    _entries[i].Valid = false;
                }
            }
        }
    }

    public class TLBEntry
    {
        public bool Valid { get; set; }
        public int VPN { get; set; }
        public int PFN { get; set; }
    }
}
