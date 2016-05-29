﻿namespace RIVMApp
{
    partial class Debugger
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStep = new System.Windows.Forms.Button();
            this.btnContinue = new System.Windows.Forms.Button();
            this.R0 = new System.Windows.Forms.Label();
            this.txtR0 = new System.Windows.Forms.TextBox();
            this.txtR1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtR2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtR3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSP = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBP = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.memoryGridView = new System.Windows.Forms.DataGridView();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCurrentInstruction = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.stackGridView = new System.Windows.Forms.DataGridView();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.memoryGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stackGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStep
            // 
            this.btnStep.Location = new System.Drawing.Point(12, 877);
            this.btnStep.Name = "btnStep";
            this.btnStep.Size = new System.Drawing.Size(170, 59);
            this.btnStep.TabIndex = 0;
            this.btnStep.Text = "Step";
            this.btnStep.UseVisualStyleBackColor = true;
            this.btnStep.Click += new System.EventHandler(this.btnStep_Click);
            // 
            // btnContinue
            // 
            this.btnContinue.Location = new System.Drawing.Point(197, 877);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(172, 59);
            this.btnContinue.TabIndex = 1;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // R0
            // 
            this.R0.AutoSize = true;
            this.R0.Location = new System.Drawing.Point(1158, 44);
            this.R0.Name = "R0";
            this.R0.Size = new System.Drawing.Size(30, 20);
            this.R0.TabIndex = 2;
            this.R0.Text = "R0";
            // 
            // txtR0
            // 
            this.txtR0.Location = new System.Drawing.Point(1233, 41);
            this.txtR0.Name = "txtR0";
            this.txtR0.Size = new System.Drawing.Size(100, 26);
            this.txtR0.TabIndex = 3;
            // 
            // txtR1
            // 
            this.txtR1.Location = new System.Drawing.Point(1233, 73);
            this.txtR1.Name = "txtR1";
            this.txtR1.Size = new System.Drawing.Size(100, 26);
            this.txtR1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1158, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "R1";
            // 
            // txtR2
            // 
            this.txtR2.Location = new System.Drawing.Point(1233, 105);
            this.txtR2.Name = "txtR2";
            this.txtR2.Size = new System.Drawing.Size(100, 26);
            this.txtR2.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1158, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "R2";
            // 
            // txtR3
            // 
            this.txtR3.Location = new System.Drawing.Point(1233, 137);
            this.txtR3.Name = "txtR3";
            this.txtR3.Size = new System.Drawing.Size(100, 26);
            this.txtR3.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1158, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "R3";
            // 
            // txtSP
            // 
            this.txtSP.Location = new System.Drawing.Point(1233, 169);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(100, 26);
            this.txtSP.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1158, 172);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 20);
            this.label4.TabIndex = 10;
            this.label4.Text = "SP";
            // 
            // txtBP
            // 
            this.txtBP.Location = new System.Drawing.Point(1233, 201);
            this.txtBP.Name = "txtBP";
            this.txtBP.Size = new System.Drawing.Size(100, 26);
            this.txtBP.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1158, 204);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 20);
            this.label5.TabIndex = 12;
            this.label5.Text = "BP";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(1233, 233);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(100, 26);
            this.txtIP.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1158, 236);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 20);
            this.label6.TabIndex = 14;
            this.label6.Text = "IP";
            // 
            // memoryGridView
            // 
            this.memoryGridView.AllowUserToAddRows = false;
            this.memoryGridView.AllowUserToDeleteRows = false;
            this.memoryGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.memoryGridView.Location = new System.Drawing.Point(12, 44);
            this.memoryGridView.Name = "memoryGridView";
            this.memoryGridView.ReadOnly = true;
            this.memoryGridView.RowTemplate.Height = 28;
            this.memoryGridView.Size = new System.Drawing.Size(585, 814);
            this.memoryGridView.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(427, 901);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(141, 20);
            this.label7.TabIndex = 17;
            this.label7.Text = "Current Instruction";
            // 
            // txtCurrentInstruction
            // 
            this.txtCurrentInstruction.Location = new System.Drawing.Point(587, 898);
            this.txtCurrentInstruction.Name = "txtCurrentInstruction";
            this.txtCurrentInstruction.Size = new System.Drawing.Size(323, 26);
            this.txtCurrentInstruction.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 20);
            this.label8.TabIndex = 19;
            this.label8.Text = "Memory";
            // 
            // stackGridView
            // 
            this.stackGridView.AllowUserToAddRows = false;
            this.stackGridView.AllowUserToDeleteRows = false;
            this.stackGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.stackGridView.Location = new System.Drawing.Point(620, 44);
            this.stackGridView.Name = "stackGridView";
            this.stackGridView.ReadOnly = true;
            this.stackGridView.RowTemplate.Height = 28;
            this.stackGridView.Size = new System.Drawing.Size(521, 814);
            this.stackGridView.TabIndex = 20;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(616, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 20);
            this.label9.TabIndex = 21;
            this.label9.Text = "Stack";
            // 
            // Debugger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1345, 948);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.stackGridView);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtCurrentInstruction);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.memoryGridView);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtBP);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtSP);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtR3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtR2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtR1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtR0);
            this.Controls.Add(this.R0);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.btnStep);
            this.Name = "Debugger";
            this.Text = "Debugger";
            ((System.ComponentModel.ISupportInitialize)(this.memoryGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stackGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStep;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.Label R0;
        private System.Windows.Forms.TextBox txtR0;
        private System.Windows.Forms.TextBox txtR1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtR2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtR3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBP;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView memoryGridView;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtCurrentInstruction;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridView stackGridView;
        private System.Windows.Forms.Label label9;

    }
}