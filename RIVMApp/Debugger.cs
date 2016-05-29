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
                txtCurrentInstruction.Text = _vm.cpu.GetCurrentInstruction();
            }));
        }

        private void updateMemoryDisplay()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("address", typeof(string)));
            dt.Columns.Add(new DataColumn("value", typeof(string)));
            dt.Columns.Add(new DataColumn("instruction", typeof(String)));
 
            int start = _vm.cpu.Registers[Register.IP];
            int end = start + 40;

            for (int address = start; address < end;)
            {
                int machineCode = _vm.cpu.Memory.Get(address, false);

                object[] firstRowData = new object[3];
                firstRowData[0] = address;
                firstRowData[1] = machineCode;

                address += 4;

                Instruction instruction = InstructionDecoder.Decode(machineCode);

                if (instruction.HasImmediate)
                {
                    instruction.Immediate = _vm.cpu.Memory.Get(address, false);
                    address += 4;
                }

                firstRowData[2] = instruction.ToString();
                dt.Rows.Add(firstRowData);

                if (instruction.HasImmediate)
                {
                    dt.Rows.Add(address, instruction.Immediate, "[immediate value]");
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

            dt.Columns.Add(new DataColumn("address", typeof(string)));
            dt.Columns.Add(new DataColumn("value", typeof(string)));

            int start = _vm.cpu.Registers[Register.BP];
            int end = _vm.cpu.Registers[Register.SP];

            for (int address = start; address < end;)
            {
                int value = _vm.cpu.Memory.Get(address, false);
                address += 4;

                dt.Rows.Add(address, value);
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
    }
}
