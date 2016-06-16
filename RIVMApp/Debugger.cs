using RIVM;
using RIVM.Instructions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RIVMApp
{
    public partial class Debugger : Form
    {
        private VM _vm;

        private int _memoryDisplayStartAddress;
        private int _memoryDisplaySize = 4;

        public Debugger(VM vm)
        {
            InitializeComponent();

            _vm = vm;

            _vm.cpu.RegisterDebuggingNotification(() =>
            {
                setSteppingControlsEnabled(true);
            });

            _vm.cpu.RegisterStepNotification(() =>
            {
                updateRegisterState(txtR0, Register.R0);
                updateRegisterState(txtR1, Register.R1);
                updateRegisterState(txtR2, Register.R2);
                updateRegisterState(txtR3, Register.R3);
                updateRegisterState(txtBP, Register.BP);
                updateRegisterState(txtIP, Register.IP);
                updateRegisterState(txtSP, Register.SP);
                updateCurrentInstruction();
                _memoryDisplayStartAddress = _vm.cpu.Registers[Register.IP];
                updateMemoryDisplay();
                updateStackDisplay();
            });
        }

        private void updateRegisterState(TextBox txt, Register register)
        {
            txt.Invoke((MethodInvoker)(() =>
            {
                txt.Text = _vm.cpu.Registers[register].ToString();
            }));
        }

        private void updateCurrentInstruction()
        {
            txtCurrentInstruction.Invoke((MethodInvoker)(() =>
            {
                txtCurrentInstruction.Text = getNextInstruction().ToString();
            }));
        }

        private Instruction getNextInstruction()
        {
            int address = _vm.cpu.Registers[Register.IP];

            int machineCode = _vm.cpu.Memory.Get(address, false, 4);
            address += 4;

            Instruction instruction = InstructionDecoder.Decode(machineCode);

            if (instruction.HasImmediate)
            {
                instruction.Immediate = _vm.cpu.Memory.Get(address, false, 4);
            }

            return instruction;
        }

        private void updateMemoryDisplay()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("address", typeof(string)));
            dt.Columns.Add(new DataColumn("value", typeof(string)));
            dt.Columns.Add(new DataColumn("instruction", typeof(String)));

            int start = _memoryDisplayStartAddress;
            int end = start + 1000;

            for (int address = start; address < end;)
            {
                if (_memoryDisplaySize == 4)
                {
                    int machineCode = _vm.cpu.Memory.Get(address, false, 4);

                    object[] firstRowData = new object[3];
                    firstRowData[0] = address;
                    firstRowData[1] = machineCode;

                    address += 4;

                    Instruction instruction = InstructionDecoder.Decode(machineCode);

                    if (instruction.HasImmediate)
                    {
                        instruction.Immediate = _vm.cpu.Memory.Get(address, false, 4);
                        address += 4;
                    }

                    firstRowData[2] = instruction.ToString();
                    dt.Rows.Add(firstRowData);

                    if (instruction.HasImmediate)
                    {
                        dt.Rows.Add(address - 4, instruction.Immediate, "[immediate value]");
                    }
                }
                else
                {
                    int value = BitHelper.ExtractBytes(_vm.cpu.Memory.Get(address, false, 1), 1);
                    dt.Rows.Add(address, value, "");
                    address++;
                }
            }

            memoryGridView.Invoke((MethodInvoker)(() =>
            {
                memoryGridView.DataSource = dt;
            }));
        }

        private void updateStackDisplay()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("info", typeof(string)));
            dt.Columns.Add(new DataColumn("address", typeof(string)));
            dt.Columns.Add(new DataColumn("value", typeof(string)));

            int start = _vm.cpu.Registers[Register.BP] - 20;
            int end = _vm.cpu.Registers[Register.SP] + 20;

            for (int address = start; address < end;)
            {
                int value = _vm.cpu.Memory.Get(address, false, 4);

                string info = "";

                if(address == _vm.cpu.Registers[Register.BP])
                {
                    info += "bp";
                }

                if(address == _vm.cpu.Registers[Register.SP])
                {
                    info += "sp";
                }
                
                dt.Rows.Add(info, address, value);

                address += 4;
            }

            stackGridView.Invoke((MethodInvoker)(() =>
            {
                stackGridView.DataSource = dt;
            }));
        }

        private void setSteppingControlsEnabled(bool enabled)
        {
            btnStep.Invoke((MethodInvoker)(() =>
            {
                btnStep.Enabled = enabled;
            }));

            btnContinue.Invoke((MethodInvoker)(() =>
            {
                btnContinue.Enabled = enabled;
            }));
        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            _vm.Step();
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            setSteppingControlsEnabled(false);
            _vm.Continue();
        }

        private void btnGoToMem_Click(object sender, EventArgs e)
        {
            _memoryDisplayStartAddress = Convert.ToInt32(txtGoToMem.Text);
            updateMemoryDisplay();
        }

        private void rd4Bytes_CheckedChanged(object sender, EventArgs e)
        {
            _memoryDisplaySize = 4;
            updateMemoryDisplay();
        }

        private void rd1Byte_CheckedChanged(object sender, EventArgs e)
        {
            _memoryDisplaySize = 1;
            updateMemoryDisplay();
        }
    }
}
