namespace LionWin
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
            this.picScreen = new System.Windows.Forms.PictureBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOpenVHD = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuOpenBIN = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuLoadBIN = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLoadBAS = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSaveBAS = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveMemory = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveBmp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuImportLeonImage = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExportLeonImage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.btnReset = new System.Windows.Forms.Button();
            this.pnlDebug = new System.Windows.Forms.Panel();
            this.lblMaxScreen = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblMinScreen = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblMaxCpu = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblMinCpu = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblPC = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picScreen)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.pnlDebug.SuspendLayout();
            this.SuspendLayout();
            // 
            // picScreen
            // 
            this.picScreen.Location = new System.Drawing.Point(12, 35);
            this.picScreen.Name = "picScreen";
            this.picScreen.Size = new System.Drawing.Size(640, 480);
            this.picScreen.TabIndex = 0;
            this.picScreen.TabStop = false;
            this.picScreen.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picMouseClick);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(658, 35);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(84, 33);
            this.btnRun.TabIndex = 2;
            this.btnRun.TabStop = false;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(658, 83);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(84, 33);
            this.btnStop.TabIndex = 3;
            this.btnStop.TabStop = false;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(751, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOpenVHD,
            this.toolStripSeparator5,
            this.mnuOpenBIN,
            this.toolStripSeparator1,
            this.mnuLoadBIN,
            this.mnuLoadBAS,
            this.toolStripSeparator2,
            this.mnuSaveBAS,
            this.mnuSaveMemory,
            this.mnuSaveBmp,
            this.toolStripSeparator3,
            this.mnuImportLeonImage,
            this.mnuExportLeonImage,
            this.toolStripSeparator4,
            this.mnuQuit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "File";
            // 
            // mnuOpenVHD
            // 
            this.mnuOpenVHD.Name = "mnuOpenVHD";
            this.mnuOpenVHD.Size = new System.Drawing.Size(180, 22);
            this.mnuOpenVHD.Text = "Open VHD Image";
            this.mnuOpenVHD.Click += new System.EventHandler(this.mnuOpenVHD_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(177, 6);
            // 
            // mnuOpenBIN
            // 
            this.mnuOpenBIN.Name = "mnuOpenBIN";
            this.mnuOpenBIN.Size = new System.Drawing.Size(180, 22);
            this.mnuOpenBIN.Text = "Open BIN";
            this.mnuOpenBIN.Click += new System.EventHandler(this.mnuOpenBIN_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // mnuLoadBIN
            // 
            this.mnuLoadBIN.Name = "mnuLoadBIN";
            this.mnuLoadBIN.Size = new System.Drawing.Size(180, 22);
            this.mnuLoadBIN.Text = "Load BIN at Address";
            this.mnuLoadBIN.Click += new System.EventHandler(this.mnuLoadBIN_Click);
            // 
            // mnuLoadBAS
            // 
            this.mnuLoadBAS.Name = "mnuLoadBAS";
            this.mnuLoadBAS.Size = new System.Drawing.Size(180, 22);
            this.mnuLoadBAS.Text = "Load BAS or Text";
            this.mnuLoadBAS.Click += new System.EventHandler(this.mnuLoadBAS_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // mnuSaveBAS
            // 
            this.mnuSaveBAS.Name = "mnuSaveBAS";
            this.mnuSaveBAS.Size = new System.Drawing.Size(180, 22);
            this.mnuSaveBAS.Text = "Save BAS and Text";
            this.mnuSaveBAS.Click += new System.EventHandler(this.mnuSaveBAS_Click);
            // 
            // mnuSaveMemory
            // 
            this.mnuSaveMemory.Name = "mnuSaveMemory";
            this.mnuSaveMemory.Size = new System.Drawing.Size(180, 22);
            this.mnuSaveMemory.Text = "Save Memory";
            this.mnuSaveMemory.Click += new System.EventHandler(this.mnuSaveMemory_Click);
            // 
            // mnuSaveBmp
            // 
            this.mnuSaveBmp.Name = "mnuSaveBmp";
            this.mnuSaveBmp.Size = new System.Drawing.Size(180, 22);
            this.mnuSaveBmp.Text = "Save Screen";
            this.mnuSaveBmp.Click += new System.EventHandler(this.mnuSaveBmp_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(177, 6);
            // 
            // mnuImportLeonImage
            // 
            this.mnuImportLeonImage.Name = "mnuImportLeonImage";
            this.mnuImportLeonImage.Size = new System.Drawing.Size(180, 22);
            this.mnuImportLeonImage.Text = "Import Leon Image";
            this.mnuImportLeonImage.Click += new System.EventHandler(this.mnuImportLeonImage_Click);
            // 
            // mnuExportLeonImage
            // 
            this.mnuExportLeonImage.Name = "mnuExportLeonImage";
            this.mnuExportLeonImage.Size = new System.Drawing.Size(180, 22);
            this.mnuExportLeonImage.Text = "Export Leon Image";
            this.mnuExportLeonImage.Click += new System.EventHandler(this.mnuExportLeonImage_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(177, 6);
            // 
            // mnuQuit
            // 
            this.mnuQuit.Name = "mnuQuit";
            this.mnuQuit.Size = new System.Drawing.Size(180, 22);
            this.mnuQuit.Text = "Quit";
            this.mnuQuit.Click += new System.EventHandler(this.mnuQuit_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(658, 132);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(84, 33);
            this.btnReset.TabIndex = 7;
            this.btnReset.TabStop = false;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // pnlDebug
            // 
            this.pnlDebug.Controls.Add(this.lblMaxScreen);
            this.pnlDebug.Controls.Add(this.label7);
            this.pnlDebug.Controls.Add(this.lblMinScreen);
            this.pnlDebug.Controls.Add(this.label5);
            this.pnlDebug.Controls.Add(this.lblMaxCpu);
            this.pnlDebug.Controls.Add(this.label6);
            this.pnlDebug.Controls.Add(this.lblMinCpu);
            this.pnlDebug.Controls.Add(this.label3);
            this.pnlDebug.Controls.Add(this.label2);
            this.pnlDebug.Controls.Add(this.lblPC);
            this.pnlDebug.Controls.Add(this.label1);
            this.pnlDebug.Location = new System.Drawing.Point(658, 187);
            this.pnlDebug.Name = "pnlDebug";
            this.pnlDebug.Size = new System.Drawing.Size(81, 173);
            this.pnlDebug.TabIndex = 17;
            this.pnlDebug.Visible = false;
            // 
            // lblMaxScreen
            // 
            this.lblMaxScreen.AutoSize = true;
            this.lblMaxScreen.Location = new System.Drawing.Point(6, 143);
            this.lblMaxScreen.Name = "lblMaxScreen";
            this.lblMaxScreen.Size = new System.Drawing.Size(41, 13);
            this.lblMaxScreen.TabIndex = 25;
            this.lblMaxScreen.Text = "mincpu";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 130);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "maxscreen";
            // 
            // lblMinScreen
            // 
            this.lblMinScreen.AutoSize = true;
            this.lblMinScreen.Location = new System.Drawing.Point(6, 113);
            this.lblMinScreen.Name = "lblMinScreen";
            this.lblMinScreen.Size = new System.Drawing.Size(41, 13);
            this.lblMinScreen.TabIndex = 23;
            this.lblMinScreen.Text = "mincpu";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "minscreen";
            // 
            // lblMaxCpu
            // 
            this.lblMaxCpu.AutoSize = true;
            this.lblMaxCpu.Location = new System.Drawing.Point(6, 83);
            this.lblMaxCpu.Name = "lblMaxCpu";
            this.lblMaxCpu.Size = new System.Drawing.Size(41, 13);
            this.lblMaxCpu.TabIndex = 21;
            this.lblMaxCpu.Text = "mincpu";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "maxcpu";
            // 
            // lblMinCpu
            // 
            this.lblMinCpu.AutoSize = true;
            this.lblMinCpu.Location = new System.Drawing.Point(6, 53);
            this.lblMinCpu.Name = "lblMinCpu";
            this.lblMinCpu.Size = new System.Drawing.Size(41, 13);
            this.lblMinCpu.TabIndex = 19;
            this.lblMinCpu.Text = "mincpu";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "mincpu";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "ticks @ PC";
            // 
            // lblPC
            // 
            this.lblPC.AutoSize = true;
            this.lblPC.Location = new System.Drawing.Point(32, 7);
            this.lblPC.Name = "lblPC";
            this.lblPC.Size = new System.Drawing.Size(13, 13);
            this.lblPC.TabIndex = 7;
            this.lblPC.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "PC";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(751, 528);
            this.Controls.Add(this.pnlDebug);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.picScreen);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.Text = "LionWin";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyUp);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.frmMain_PreviewKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.picScreen)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.pnlDebug.ResumeLayout(false);
            this.pnlDebug.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picScreen;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuLoadBIN;
        private System.Windows.Forms.ToolStripMenuItem mnuLoadBAS;
        private System.Windows.Forms.ToolStripMenuItem mnuOpenBIN;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveBAS;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveMemory;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mnuImportLeonImage;
        private System.Windows.Forms.ToolStripMenuItem mnuExportLeonImage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem mnuQuit;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveBmp;
        private System.Windows.Forms.Panel pnlDebug;
        private System.Windows.Forms.Label lblMaxScreen;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblMinScreen;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblMaxCpu;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblMinCpu;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblPC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem mnuOpenVHD;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    }
}

