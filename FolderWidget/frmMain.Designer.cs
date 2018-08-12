namespace FolderWidget
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblFileName = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDeployToProd = new System.Windows.Forms.Button();
            this.tblIcons = new System.Windows.Forms.TableLayoutPanel();
            this.tblMain.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tblMain
            // 
            this.tblMain.AllowDrop = true;
            this.tblMain.BackColor = System.Drawing.Color.Transparent;
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.Controls.Add(this.panel1, 0, 0);
            this.tblMain.Controls.Add(this.btnDeployToProd, 0, 2);
            this.tblMain.Controls.Add(this.tblIcons, 0, 1);
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMain.Location = new System.Drawing.Point(0, 0);
            this.tblMain.Margin = new System.Windows.Forms.Padding(0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 3;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tblMain.Size = new System.Drawing.Size(150, 110);
            this.tblMain.TabIndex = 0;
            this.tblMain.DragDrop += new System.Windows.Forms.DragEventHandler(this.DragDropForm);
            this.tblMain.DragEnter += new System.Windows.Forms.DragEventHandler(this.DragEnterForm);
            // 
            // panel1
            // 
            this.panel1.AllowDrop = true;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(172)))), ((int)(((byte)(242)))));
            this.panel1.Controls.Add(this.lblFileName);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(150, 30);
            this.panel1.TabIndex = 2;
            this.panel1.DragDrop += new System.Windows.Forms.DragEventHandler(this.DragDropForm);
            this.panel1.DragEnter += new System.Windows.Forms.DragEventHandler(this.DragEnterForm);
            this.panel1.DoubleClick += new System.EventHandler(this.lblFileName_DoubleClick);
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblFileName.ForeColor = System.Drawing.SystemColors.Control;
            this.lblFileName.Location = new System.Drawing.Point(3, 10);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(0, 16);
            this.lblFileName.TabIndex = 4;
            this.lblFileName.DragDrop += new System.Windows.Forms.DragEventHandler(this.DragDropForm);
            this.lblFileName.DragEnter += new System.Windows.Forms.DragEventHandler(this.DragEnterForm);
            this.lblFileName.DoubleClick += new System.EventHandler(this.lblFileName_DoubleClick);
            this.lblFileName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // btnClose
            // 
            this.btnClose.AllowDrop = true;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnClose.Location = new System.Drawing.Point(120, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(30, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.TabStop = false;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.btnClose.DragDrop += new System.Windows.Forms.DragEventHandler(this.DragDropForm);
            this.btnClose.DragEnter += new System.Windows.Forms.DragEventHandler(this.DragEnterForm);
            // 
            // btnDeployToProd
            // 
            this.btnDeployToProd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnDeployToProd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDeployToProd.FlatAppearance.BorderSize = 0;
            this.btnDeployToProd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeployToProd.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnDeployToProd.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnDeployToProd.Location = new System.Drawing.Point(0, 80);
            this.btnDeployToProd.Margin = new System.Windows.Forms.Padding(0);
            this.btnDeployToProd.Name = "btnDeployToProd";
            this.btnDeployToProd.Size = new System.Drawing.Size(150, 30);
            this.btnDeployToProd.TabIndex = 4;
            this.btnDeployToProd.TabStop = false;
            this.btnDeployToProd.Text = "Production";
            this.btnDeployToProd.UseVisualStyleBackColor = false;
            this.btnDeployToProd.Click += new System.EventHandler(this.btnDeployToProd_Click);
            // 
            // tblIcons
            // 
            this.tblIcons.AllowDrop = true;
            this.tblIcons.BackColor = System.Drawing.Color.Transparent;
            this.tblIcons.ColumnCount = 3;
            this.tblIcons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tblIcons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tblIcons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tblIcons.Location = new System.Drawing.Point(0, 30);
            this.tblIcons.Margin = new System.Windows.Forms.Padding(0);
            this.tblIcons.Name = "tblIcons";
            this.tblIcons.RowCount = 1;
            this.tblIcons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblIcons.Size = new System.Drawing.Size(150, 50);
            this.tblIcons.TabIndex = 5;
            this.tblIcons.DragDrop += new System.Windows.Forms.DragEventHandler(this.DragDropForm);
            this.tblIcons.DragEnter += new System.Windows.Forms.DragEventHandler(this.DragEnterForm);
            // 
            // frmMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(150, 110);
            this.Controls.Add(this.tblMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FolderWidget";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.DragDropForm);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.DragEnterForm);
            this.tblMain.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblMain;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDeployToProd;
        private System.Windows.Forms.TableLayoutPanel tblIcons;
        private System.Windows.Forms.Label lblFileName;
    }
}

