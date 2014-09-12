using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualMachine.Util
{
    public static class DiskFormatter
    {
        public static void Format(string diskFile, string bootLoaderExeFile)
        {
            int maxFiles = 100;

            using (var fs = new BinaryWriter(new FileStream(diskFile, FileMode.Open, FileAccess.Write)))
            {
                int size = (int)fs.BaseStream.Length;

                //MBR
                using (var fs2 = new BinaryReader(new FileStream(bootLoaderExeFile, FileMode.Open)))
                {
                    byte[] bootLoader = new byte[512];

                    fs2.Read(bootLoader, 0, (int)Math.Min(512, fs2.BaseStream.Length));

                    fs.Write(bootLoader);
                }

                fs.Seek(512, SeekOrigin.Begin);

                int numSectors = size / 512;
                int dataStart = 512 + numSectors * 4 + maxFiles * (8 + 4 + 4);

                //Header
                fs.Write(numSectors);
                fs.Write(maxFiles);
                fs.Write(dataStart);

                //FAT table
                for (int i = 0; i < numSectors; i++)
                {
                    fs.Write(0);
                }

                //Root directory
                for (int i = 0; i < maxFiles; i++)
                {
                    fs.Write(0L); //Filename
                    fs.Write(0);  //Start sector
                    fs.Write(0);  //File size
                }
            }
        }
    }
}
