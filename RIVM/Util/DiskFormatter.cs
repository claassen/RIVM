using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIVM.Util
{
    public static class DiskFormatter
    {
        //0 - 511          : MBR
        //512 - 515        : # sectors
        //516 - 519        : max # files
        //520 - 523        : data start
        //524 - ???        : FAT Table (# sectors * 4 bytes) 
        //                   next sector, 0 = last sector in file
        //??? - data start : root directory (max # files * 16 bytes)
        //                   0 - 7   : file name
        //                   8 - 11  : start sector
        //                   12 - 15 : file size
        public static void Format(string diskFile, string bootLoaderExeFile, string kernelImageFile)
        {
            int maxFiles = 100;

            byte[] kernelImage;

            using (var fs = new BinaryReader(new FileStream(kernelImageFile, FileMode.Open)))
            {
                kernelImage = new byte[fs.BaseStream.Length];

                fs.Read(kernelImage, 0, (int)fs.BaseStream.Length);
            }

            using (var fs = new FileStream(diskFile, FileMode.OpenOrCreate, FileAccess.Write))
            using (var bw = new BinaryWriter(fs))
            {
                long diskSize = 1024L * 1024L * 1024L;

                fs.SetLength(diskSize);

                int size = (int)bw.BaseStream.Length;

                //MBR
                using (var fs2 = new BinaryReader(new FileStream(bootLoaderExeFile, FileMode.Open)))
                {
                    byte[] bootLoader = new byte[512];

                    fs2.Read(bootLoader, 0, (int)Math.Min(512, fs2.BaseStream.Length));

                    bw.Write(bootLoader);
                }

                bw.Seek(512, SeekOrigin.Begin);

                int numSectors = size / 512;
                int dataStart = 524                     //MBR size
                              + numSectors * 4          //FAT table size  
                              + maxFiles * (8 + 4 + 4); //root directory size

                //Header
                bw.Write(BitConverter.ToInt32(BitConverter.GetBytes(numSectors).Reverse().ToArray(), 0));
                bw.Write(BitConverter.ToInt32(BitConverter.GetBytes(maxFiles).Reverse().ToArray(), 0));
                bw.Write(BitConverter.ToInt32(BitConverter.GetBytes(dataStart).Reverse().ToArray(), 0));

                int numKernelImageSectors = (int)Math.Ceiling(kernelImage.Length / 512.0);
                
                for (int i = 0; i < numKernelImageSectors; i++)
                {
                    bw.Write(BitConverter.ToInt32(BitConverter.GetBytes(i + 1).Reverse().ToArray(), 0));    
                }

                //FAT table
                for (int i = numKernelImageSectors; i < numSectors; i++)
                {
                    bw.Write(0);
                }

                //Root directory
                foreach (char c in "ker.sys".ToCharArray())
                {
                    bw.Write((byte)c);
                }
                bw.Write((byte)0); //8th byte of file name

                bw.Write((int)0); //Start sector

                bw.Write(kernelImage.Length);
                
                for (int i = 1; i < maxFiles; i++)
                {
                    bw.Write(0L); //Filename
                    bw.Write(0);  //Start sector
                    bw.Write(0);  //File size
                }

                bw.Write(kernelImage);
            }
        }
    }
}
