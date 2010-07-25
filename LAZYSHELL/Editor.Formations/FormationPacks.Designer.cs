﻿namespace LAZYSHELL
{
    partial class FormationPacks
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
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.packNum = new ToolStripNumericUpDown();
            this.formationSet = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.searchFormationPacks = new System.Windows.Forms.ToolStripButton();
            this.packNameTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.packFormation3 = new System.Windows.Forms.NumericUpDown();
            this.packFormation1 = new System.Windows.Forms.NumericUpDown();
            this.packFormationButton1 = new System.Windows.Forms.Button();
            this.richTextBox4 = new System.Windows.Forms.RichTextBox();
            this.packFormationButton3 = new System.Windows.Forms.Button();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.packFormationButton2 = new System.Windows.Forms.Button();
            this.packFormation2 = new System.Windows.Forms.NumericUpDown();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.packFormation3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.packFormation1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.packFormation2)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip2
            // 
            this.toolStrip2.CanOverflow = false;
            this.toolStrip2.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.packNum,
            this.formationSet,
            this.toolStripSeparator1,
            this.searchFormationPacks,
            this.packNameTextBox});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip2.Size = new System.Drawing.Size(621, 25);
            this.toolStrip2.TabIndex = 563;
            // 
            // packNum
            // 
            this.packNum.AutoSize = false;
            this.packNum.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.packNum.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.packNum.ForeColor = System.Drawing.SystemColors.Control;
            this.packNum.Hexadecimal = false;
            this.packNum.Location = new System.Drawing.Point(7, 1);
            this.packNum.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.packNum.Name = "packNum";
            this.packNum.Size = new System.Drawing.Size(55, 22);
            this.packNum.Text = "0";
            this.packNum.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.packNum.ValueChanged += new System.EventHandler(this.packNum_ValueChanged);
            // 
            // formationSet
            // 
            this.formationSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.formationSet.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.formationSet.Items.AddRange(new object[] {
            "Use formations 0 - 255",
            "Use formations 256 - 512"});
            this.formationSet.Name = "formationSet";
            this.formationSet.Size = new System.Drawing.Size(145, 25);
            this.formationSet.SelectedIndexChanged += new System.EventHandler(this.formationSet_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // searchFormationPacks
            // 
            this.searchFormationPacks.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.searchFormationPacks.Image = global::LAZYSHELL.Properties.Resources.search;
            this.searchFormationPacks.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.searchFormationPacks.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.searchFormationPacks.Name = "searchFormationPacks";
            this.searchFormationPacks.Size = new System.Drawing.Size(23, 22);
            this.searchFormationPacks.Text = "Search for effect";
            // 
            // packNameTextBox
            // 
            this.packNameTextBox.Name = "packNameTextBox";
            this.packNameTextBox.Size = new System.Drawing.Size(185, 25);
            this.packNameTextBox.Visible = false;
            // 
            // packFormation3
            // 
            this.packFormation3.Dock = System.Windows.Forms.DockStyle.Top;
            this.packFormation3.Location = new System.Drawing.Point(0, 0);
            this.packFormation3.Maximum = new decimal(new int[] {
            511,
            0,
            0,
            0});
            this.packFormation3.Name = "packFormation3";
            this.packFormation3.Size = new System.Drawing.Size(195, 21);
            this.packFormation3.TabIndex = 94;
            this.packFormation3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.packFormation3.ValueChanged += new System.EventHandler(this.packFormation3_ValueChanged);
            // 
            // packFormation1
            // 
            this.packFormation1.Dock = System.Windows.Forms.DockStyle.Top;
            this.packFormation1.Location = new System.Drawing.Point(0, 0);
            this.packFormation1.Maximum = new decimal(new int[] {
            511,
            0,
            0,
            0});
            this.packFormation1.Name = "packFormation1";
            this.packFormation1.Size = new System.Drawing.Size(195, 21);
            this.packFormation1.TabIndex = 87;
            this.packFormation1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.packFormation1.ValueChanged += new System.EventHandler(this.packFormation1_ValueChanged);
            // 
            // packFormationButton1
            // 
            this.packFormationButton1.BackColor = System.Drawing.SystemColors.Control;
            this.packFormationButton1.FlatAppearance.BorderSize = 0;
            this.packFormationButton1.Location = new System.Drawing.Point(12, 165);
            this.packFormationButton1.Name = "packFormationButton1";
            this.packFormationButton1.Size = new System.Drawing.Size(195, 22);
            this.packFormationButton1.TabIndex = 89;
            this.packFormationButton1.Text = "LOAD";
            this.packFormationButton1.UseVisualStyleBackColor = false;
            this.packFormationButton1.Click += new System.EventHandler(this.packFormationButton1_Click);
            // 
            // richTextBox4
            // 
            this.richTextBox4.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox4.Location = new System.Drawing.Point(0, 21);
            this.richTextBox4.Name = "richTextBox4";
            this.richTextBox4.ReadOnly = true;
            this.richTextBox4.Size = new System.Drawing.Size(195, 110);
            this.richTextBox4.TabIndex = 95;
            this.richTextBox4.Text = "";
            // 
            // packFormationButton3
            // 
            this.packFormationButton3.BackColor = System.Drawing.SystemColors.Control;
            this.packFormationButton3.FlatAppearance.BorderSize = 0;
            this.packFormationButton3.Location = new System.Drawing.Point(414, 165);
            this.packFormationButton3.Name = "packFormationButton3";
            this.packFormationButton3.Size = new System.Drawing.Size(195, 22);
            this.packFormationButton3.TabIndex = 96;
            this.packFormationButton3.Text = "LOAD";
            this.packFormationButton3.UseVisualStyleBackColor = false;
            this.packFormationButton3.Click += new System.EventHandler(this.packFormationButton3_Click);
            // 
            // richTextBox2
            // 
            this.richTextBox2.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox2.Location = new System.Drawing.Point(0, 21);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ReadOnly = true;
            this.richTextBox2.Size = new System.Drawing.Size(195, 110);
            this.richTextBox2.TabIndex = 88;
            this.richTextBox2.Text = "";
            // 
            // packFormationButton2
            // 
            this.packFormationButton2.BackColor = System.Drawing.SystemColors.Control;
            this.packFormationButton2.FlatAppearance.BorderSize = 0;
            this.packFormationButton2.Location = new System.Drawing.Point(213, 165);
            this.packFormationButton2.Name = "packFormationButton2";
            this.packFormationButton2.Size = new System.Drawing.Size(195, 22);
            this.packFormationButton2.TabIndex = 93;
            this.packFormationButton2.Text = "LOAD";
            this.packFormationButton2.UseVisualStyleBackColor = false;
            this.packFormationButton2.Click += new System.EventHandler(this.packFormationButton2_Click);
            // 
            // packFormation2
            // 
            this.packFormation2.Dock = System.Windows.Forms.DockStyle.Top;
            this.packFormation2.Location = new System.Drawing.Point(0, 0);
            this.packFormation2.Maximum = new decimal(new int[] {
            511,
            0,
            0,
            0});
            this.packFormation2.Name = "packFormation2";
            this.packFormation2.Size = new System.Drawing.Size(195, 21);
            this.packFormation2.TabIndex = 90;
            this.packFormation2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.packFormation2.ValueChanged += new System.EventHandler(this.packFormation2_ValueChanged);
            // 
            // richTextBox3
            // 
            this.richTextBox3.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox3.Location = new System.Drawing.Point(0, 21);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.ReadOnly = true;
            this.richTextBox3.Size = new System.Drawing.Size(195, 110);
            this.richTextBox3.TabIndex = 92;
            this.richTextBox3.Text = "";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlText;
            this.panel1.Controls.Add(this.richTextBox2);
            this.panel1.Controls.Add(this.packFormation1);
            this.panel1.Location = new System.Drawing.Point(12, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(195, 131);
            this.panel1.TabIndex = 565;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlText;
            this.panel2.Controls.Add(this.richTextBox3);
            this.panel2.Controls.Add(this.packFormation2);
            this.panel2.Location = new System.Drawing.Point(213, 28);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(195, 131);
            this.panel2.TabIndex = 566;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.SystemColors.ControlText;
            this.panel6.Controls.Add(this.richTextBox4);
            this.panel6.Controls.Add(this.packFormation3);
            this.panel6.Location = new System.Drawing.Point(414, 28);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(195, 131);
            this.panel6.TabIndex = 567;
            // 
            // FormationPacks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 199);
            this.ControlBox = false;
            this.Controls.Add(this.packFormationButton1);
            this.Controls.Add(this.packFormationButton3);
            this.Controls.Add(this.packFormationButton2);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip2);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormationPacks";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.packFormation3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.packFormation1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.packFormation2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip2;
        private ToolStripNumericUpDown packNum;
        private System.Windows.Forms.ToolStripButton searchFormationPacks;
        private System.Windows.Forms.ToolStripTextBox packNameTextBox;
        private System.Windows.Forms.NumericUpDown packFormation3;
        private System.Windows.Forms.NumericUpDown packFormation1;
        private System.Windows.Forms.Button packFormationButton1;
        private System.Windows.Forms.RichTextBox richTextBox4;
        private System.Windows.Forms.Button packFormationButton3;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.Button packFormationButton2;
        private System.Windows.Forms.NumericUpDown packFormation2;
        private System.Windows.Forms.RichTextBox richTextBox3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.ToolStripComboBox formationSet;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}