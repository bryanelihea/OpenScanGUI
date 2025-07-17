namespace OpenScanGUI
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btnScanChain = new System.Windows.Forms.Button();
            this.cmbControllers = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pbxChain = new System.Windows.Forms.PictureBox();
            this.rtbDebug = new System.Windows.Forms.RichTextBox();
            this.tmrDebug = new System.Windows.Forms.Timer(this.components);
            this.btnAssign = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbxChain)).BeginInit();
            this.SuspendLayout();
            // 
            // btnScanChain
            // 
            this.btnScanChain.Location = new System.Drawing.Point(289, 12);
            this.btnScanChain.Name = "btnScanChain";
            this.btnScanChain.Size = new System.Drawing.Size(75, 23);
            this.btnScanChain.TabIndex = 0;
            this.btnScanChain.Text = "Scan Chain";
            this.btnScanChain.UseVisualStyleBackColor = true;
            this.btnScanChain.Click += new System.EventHandler(this.btnScanChain_Click);
            // 
            // cmbControllers
            // 
            this.cmbControllers.FormattingEnabled = true;
            this.cmbControllers.Location = new System.Drawing.Point(72, 12);
            this.cmbControllers.Name = "cmbControllers";
            this.cmbControllers.Size = new System.Drawing.Size(211, 21);
            this.cmbControllers.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Controller:";
            // 
            // pbxChain
            // 
            this.pbxChain.Location = new System.Drawing.Point(12, 39);
            this.pbxChain.Name = "pbxChain";
            this.pbxChain.Size = new System.Drawing.Size(760, 200);
            this.pbxChain.TabIndex = 3;
            this.pbxChain.TabStop = false;
            this.pbxChain.DoubleClick += new System.EventHandler(this.pbxChain_DoubleClick);
            // 
            // rtbDebug
            // 
            this.rtbDebug.Location = new System.Drawing.Point(12, 396);
            this.rtbDebug.Name = "rtbDebug";
            this.rtbDebug.ReadOnly = true;
            this.rtbDebug.Size = new System.Drawing.Size(760, 153);
            this.rtbDebug.TabIndex = 4;
            this.rtbDebug.Text = "";
            // 
            // tmrDebug
            // 
            this.tmrDebug.Tick += new System.EventHandler(this.tmrDebug_Tick);
            // 
            // btnAssign
            // 
            this.btnAssign.Location = new System.Drawing.Point(697, 245);
            this.btnAssign.Name = "btnAssign";
            this.btnAssign.Size = new System.Drawing.Size(75, 23);
            this.btnAssign.TabIndex = 5;
            this.btnAssign.Text = "Assign";
            this.btnAssign.UseVisualStyleBackColor = true;
            this.btnAssign.Click += new System.EventHandler(this.btnAssign_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.btnAssign);
            this.Controls.Add(this.rtbDebug);
            this.Controls.Add(this.pbxChain);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbControllers);
            this.Controls.Add(this.btnScanChain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "OpenScan GUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbxChain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnScanChain;
        private System.Windows.Forms.ComboBox cmbControllers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pbxChain;
        private System.Windows.Forms.RichTextBox rtbDebug;
        private System.Windows.Forms.Timer tmrDebug;
        private System.Windows.Forms.Button btnAssign;
    }
}

