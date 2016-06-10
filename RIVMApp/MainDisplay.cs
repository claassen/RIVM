using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RIVM;

namespace RIVMApp
{
    public partial class MainDisplay : Form
    {
        private VM _vm;

        public MainDisplay()
        {
            InitializeComponent();

            Init();

            var debugger = new Debugger(_vm);

            debugger.Show();

            new Thread(() => { _vm.Start(); }).Start();
        }

        private void Init()
        {
            this.Width = 800;
            this.Height = 600;

            _vm = new VM((int)Math.Pow(2, 29), @"C:\VM\bios.exe", @"C:\VM\VM.disk", this.CreateGraphics());

            this.SizeChanged += MainDisplay_SizeChanged;
            this.FormClosed += MainDisplay_FormClosed;
        }

        void MainDisplay_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        void MainDisplay_SizeChanged(object sender, EventArgs e)
        {
            _vm.ResizeDisplay(this.CreateGraphics());
        }
    }
}
