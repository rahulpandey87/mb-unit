using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MbUnit.Framework
{
    internal sealed class ManualTestForm : Form
    {
        public ManualTestForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.GroupBox groupBox2;
            this.commentTextBox = new System.Windows.Forms.TextBox();
            this.testStepList = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.failureButton = new System.Windows.Forms.Button();
            groupBox1 = new System.Windows.Forms.GroupBox();
            groupBox2 = new System.Windows.Forms.GroupBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            this.SuspendLayout();
// 
// groupBox1
// 
            groupBox1.Controls.Add(this.commentTextBox);
            groupBox1.Location = new System.Drawing.Point(10, 163);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(329, 98);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "Comments";
// 
// commentTextBox
// 
            this.commentTextBox.AutoSize = false;
            this.commentTextBox.Location = new System.Drawing.Point(7, 20);
            this.commentTextBox.Multiline = true;
            this.commentTextBox.Name = "commentTextBox";
            this.commentTextBox.Size = new System.Drawing.Size(316, 70);
            this.commentTextBox.TabIndex = 0;
// 
// groupBox2
// 
            groupBox2.Controls.Add(this.testStepList);
            groupBox2.Location = new System.Drawing.Point(10, 6);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(329, 151);
            groupBox2.TabIndex = 7;
            groupBox2.TabStop = false;
            groupBox2.Text = "Test Steps";
// 
// testStepList
// 
            this.testStepList.Location = new System.Drawing.Point(7, 20);
            this.testStepList.Name = "testStepList";
            this.testStepList.Size = new System.Drawing.Size(316, 121);
            this.testStepList.TabIndex = 6;
// 
// button2
// 
            this.button2.BackColor = System.Drawing.Color.LawnGreen;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button2.Location = new System.Drawing.Point(10, 268);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(176, 50);
            this.button2.TabIndex = 3;
            this.button2.Text = "Success";
            this.button2.Click += new System.EventHandler(this.button2_Click);
// 
// failureButton
// 
            this.failureButton.BackColor = System.Drawing.Color.OrangeRed;
            this.failureButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.failureButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.failureButton.Location = new System.Drawing.Point(193, 268);
            this.failureButton.Name = "failureButton";
            this.failureButton.Size = new System.Drawing.Size(146, 50);
            this.failureButton.TabIndex = 4;
            this.failureButton.Text = "Failure";
            this.failureButton.Click += new System.EventHandler(this.failureButton_Click);
// 
// ManualTestForm
// 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(346, 323);
            this.Controls.Add(groupBox2);
            this.Controls.Add(this.failureButton);
            this.Controls.Add(this.button2);
            this.Controls.Add(groupBox1);
            this.Name = "ManualTestForm";
            this.Text = "ManualTestForm";
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox commentTextBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button failureButton;
        private System.Windows.Forms.ListBox testStepList;

        public string Comments
        {
            get { return this.commentTextBox.Text; }
        }

        public ListBox TestStepList
        {
            get { return this.testStepList; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void failureButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
       }

    }
}