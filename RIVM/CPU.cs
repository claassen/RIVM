using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RIVM.Instructions;

namespace RIVM
{
    [Flags]
    public enum CompareFlag
    {
        LT = 1,
        EQ = 2,
        GT = 4,
        LE = LT | EQ,
        GE = GT | EQ
    }

    public static class SystemMemoryMap
    {
        public static readonly int IVT_START         = 0x00000000;
        public static readonly int IVT_END           = 0x000003FF;
        public static readonly int IO_PORT_START     = 0x00000400;
        public static readonly int IO_PORT_END       = 0x000004FF;
        public static readonly int BIOS_STACK_START  = 0x00000500;
        public static readonly int BIOS_STACK_END    = 0x00007BFF;
        public static readonly int BOOT_SECTOR_START = 0x00007C00;
        public static readonly int BOOT_SECTOR_END   = 0x00007DFF;
        public static readonly int VGA_MEMORY_START  = 0x000A0000;
        public static readonly int VGA_MEMORY_END    = 0x000BFFFF;
        public static readonly int BIOS_ROM_START    = 0x000F0000;
        public static readonly int BIOS_ROM_END      = 0x000FFFFF;
        public static readonly int RAM_START         = 0x00100000;
    }

    public class CPU
    {
        public Registers Registers;
        public MMU Memory;
        public CompareFlag Flag;
        public bool KernelMode;
        public bool Halted;
        public Queue<int> Interrupts;
        public bool InterruptsEnabled;

        public int IDTPointer;

        public int MemoryAccessSize { set { Memory.MemoryAccessSize = value; } }
        
        public CPU(MMU memory)
        {
            Registers = new Registers();
            Memory = memory;
            Interrupts = new Queue<int>();
        }

        public void Start()
        {
            KernelMode = true;
            Memory.PTEnabled = false;
            Registers[Register.IP] = SystemMemoryMap.BIOS_ROM_START;
            
            while (true)
            {
                try
                {
                    if (!Halted)
                    {
                        int machineCode = Fetch();
                        var instruction = InstructionDecoder.Decode(machineCode);

                        if (instruction.HasImmediate)
                        {
                            instruction.Immediate = Fetch();
                        }

                        //Console.WriteLine(instruction);

                        instruction.Execute(this);
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }

                    if (Interrupts.Count > 0)
                    {
                        lock (Interrupts)
                        {
                            throw new InterruptException(Interrupts.Dequeue());
                        }
                    }
                }
                catch (InterruptException ex)
                {
                    Halted = false;

                    //index into Interrupt Descriptor Table
                    int interrupt = ex.InterruptNumber;

                    //Switch to kernel mode and set IP to interrupt handler address
                    KernelMode = true;
                    
                    //Context switch? (software) (kernel stack?) (IP* - need to set to either next instruction or re-execute the same instruction)

                    //Need more complex logic here (i.e. context switch needed for IR?)
                    Registers[Register.IP] = Memory[IDTPointer + interrupt];
                }
            }
        }

        public void RaiseInterrupt(int interrupt)
        {
            lock (Interrupts)
            {
                Interrupts.Enqueue(interrupt);
            }
        }

        private int Fetch()
        {
            int instruction = Memory[Registers[Register.IP]];

            Registers[Register.IP] += 4;

            return instruction;
        }
    }
}
