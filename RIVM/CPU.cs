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
        public const int IVT_START         = 0x00000000;
        public const int IVT_END           = 0x000003FF;
        public const int IO_PORT_START     = 0x00000400;
        public const int IO_PORT_END       = 0x000004FF;
        public const int BIOS_STACK_START  = 0x00000500;
        public const int BIOS_STACK_END    = 0x00007BFF;
        public const int BOOT_SECTOR_START = 0x00007C00;
        public const int BOOT_SECTOR_END   = 0x00007DFF;
        public const int ISR_START         = 0x00007E00;
        public const int ISR_END           = 0x00008200;
        public const int VGA_MEMORY_START  = 0x000A0000;
        public const int VGA_MEMORY_END    = 0x000BFFFF;
        public const int BIOS_ROM_START    = 0x000F0000;
        public const int BIOS_ROM_END      = 0x000FFFFF;
        public const int RAM_START         = 0x00100000;
    }

    public class CPU
    {
        public Registers Registers;
        public MMU Memory;
        public CompareFlag Flag;
        public bool KernelMode;
        public bool Halted;
        public Queue<int> Interrupts;
        public bool InterruptsEnabled = false;

        public int IDTPointer;

        public int MemoryAccessSize { set { Memory.MemoryAccessSize = value; } }

        private bool _debugging;
        private Action _stepNotification;
        private Action _debuggingNotification;

        private Instruction _currentInstruction;

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
            Registers[Register.BP] = SystemMemoryMap.BIOS_STACK_START;
            Registers[Register.SP] = SystemMemoryMap.BIOS_STACK_START;

            //The CPU will run until a halt (0 value) instruction is executed, at which point it will simply stop 
            //so a kernel will need to ensure an infinite loop is maintained to keep the CPU running
            while (!Halted)
            {
                if (!_debugging)
                {
                    Step();
                }
            }
        }

        public void Continue()
        {
            _debugging = false;
        }

        public void Step()
        {
            try
            {
                int machineCode = Fetch();
                _currentInstruction = InstructionDecoder.Decode(machineCode);

                if (_currentInstruction is Break)
                {
                    _debugging = true;
                    _debuggingNotification();
                }

                if (_currentInstruction.HasImmediate)
                {
                    _currentInstruction.Immediate = Fetch();
                }

                _currentInstruction.Execute(this);
                
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
                //Index into Interrupt Descriptor Table
                int interrupt = ex.InterruptNumber;

                KernelMode = true;

                //TODO: Need to switch to kernel stack? Should this be handled by IR common stub?

                //Similar to a call, stored the address where code should resume executing on the stack
                Memory[Registers[Register.SP]] = Registers[Register.IP];
                Registers[Register.SP] += 4;

                //Set IP to fixed interrupt handler address                
                Registers[Register.IP] = Memory[IDTPointer + interrupt * 4];
            }

            if (_stepNotification != null && _debugging)
            {
                _stepNotification();
            }
        }

        public void RaiseInterrupt(int interrupt)
        {
            if (InterruptsEnabled)
            {
                lock (Interrupts)
                {
                    Interrupts.Enqueue(interrupt);
                }
            }
        }

        public void RegisterStepNotification(Action handler) 
        {
            _stepNotification = handler;    
        }

        public void RegisterDebuggingNotification(Action handler)
        {
            _debuggingNotification = handler;
        }

        public string GetCurrentInstruction()
        {
            return _currentInstruction.ToString();
        }

        private int Fetch()
        {
            MemoryAccessSize = 4;

            int instruction = Memory[Registers[Register.IP]];

            Registers[Register.IP] += 4;

            return instruction;
        }
    }
}
