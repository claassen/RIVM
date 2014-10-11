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
                int dataStart = 512                     //MBR size
                              + numSectors * 4          //FAT table size  
                              + maxFiles * (8 + 4 + 4); //root directory size

                //Header
                fs.Write(numSectors);
                fs.Write(maxFiles);
                fs.Write(dataStart);

                int numKernelImageSectors = (int)Math.Ceiling(kernelImage.Length / 512.0);
                
                for (int i = 0; i < numKernelImageSectors; i++)
                {
                    fs.Write(i + 1);    
                }

                //FAT table
                for (int i = numKernelImageSectors; i < numSectors; i++)
                {
                    fs.Write(0);
                }

                //Root directory
                foreach (char c in "ker.sys".ToCharArray())
                {
                    fs.Write((byte)c);
                }
                fs.Write(0);

                fs.Write(0);
                fs.Write(kernelImage.Length);
                
                for (int i = 1; i < maxFiles; i++)
                {
                    fs.Write(0L); //Filename
                    fs.Write(0);  //Start sector
                    fs.Write(0);  //File size
                }

                fs.Write(kernelImage);
            }
        }
    }
}
