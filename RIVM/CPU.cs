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
        GT = 3,
        LE = LT | EQ,
        GE = GT | EQ
    }

    public static class SystemMemoryMap
    {
        public static readonly int BIOS_ROM_START = 0;
        public static readonly int BIOS_ROM_END = 1023;
        public static readonly int IO_PORT_START = 1024;
        public static readonly int IO_PORT_END = 2047;
        public static readonly int RAM_START = 2048;
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
            Registers[Register.IP] = 8;
            
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
