using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualMachine.IODevices
{
    public class DiskController
    {
        enum ControlCodes
        {
            SEEK = 0,
            READ,
            WRITE
        }

        private Stream _stream;
        private BinaryReader _br;
        private BinaryWriter _bw;

        private int _controlRegister;

        public int ControlRegister
        {
            get
            {
                return _controlRegister;
            }
            set
            {
                switch (value)
                {
                    case (int)ControlCodes.SEEK:
                        Seek();
                        break;
                    case (int)ControlCodes.READ:
                        Read();
                        break;
                    case (int)ControlCodes.WRITE:
                        Write();
                        break;
                    default:
                        throw new Exception("Unknown control code");
                }

                _controlRegister = value;
            }
        }

        public int AddressRegister { get; set; }

        public int DataRegister { get; set; }

        public int IOByteCountRegister { get; set; }

        public DiskController(string diskFile)
        {
            _stream = new FileStream(diskFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            _br = new BinaryReader(_stream);
            _bw = new BinaryWriter(_stream);
        }

        private void Seek()
        {
            _stream.Seek(AddressRegister, SeekOrigin.Begin);
        }

        private void Read()
        {
            switch (IOByteCountRegister)
            {
                case 1:
                    DataRegister = _br.ReadByte();
                    break;
                case 2:
                    DataRegister = _br.ReadInt16();
                    break;
                case 4:
                    DataRegister = _br.ReadInt32();
                    break;
                default:
                    throw new Exception("Invalid byte count");
            }
        }

        private void Write()
        {
            switch (IOByteCountRegister)
            {
                case 1:
                    _bw.Write((byte)DataRegister);
                    break;
                case 2:
                    _bw.Write((short)DataRegister);
                    break;
                case 4:
                    _bw.Write((int)DataRegister);
                    break;
                default:
                    throw new Exception("Invalid byte count");
            }
        }
    }
}
