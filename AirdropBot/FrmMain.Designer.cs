namespace AirdropBot
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.btnOpenInputFile = new System.Windows.Forms.Button();
            this.openCsvFile = new System.Windows.Forms.OpenFileDialog();
            this.lstUsers = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLoginMail = new System.Windows.Forms.Button();
            this.browser = new System.Windows.Forms.WebBrowser();
            this.button1 = new System.Windows.Forms.Button();
            this.txtMailFind = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtScenario = new System.Windows.Forms.TextBox();
            this.btnApplyScenario = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOpenInputFile
            // 
            this.btnOpenInputFile.Location = new System.Drawing.Point(12, 12);
            this.btnOpenInputFile.Name = "btnOpenInputFile";
            this.btnOpenInputFile.Size = new System.Drawing.Size(138, 23);
            this.btnOpenInputFile.TabIndex = 0;
            this.btnOpenInputFile.Text = "OpenInputFile";
            this.btnOpenInputFile.UseVisualStyleBackColor = true;
            this.btnOpenInputFile.Click += new System.EventHandler(this.btnOpenInputFile_Click);
            // 
            // openCsvFile
            // 
            this.openCsvFile.Filter = "CSV Files|*.csv";
            this.openCsvFile.RestoreDirectory = true;
            // 
            // lstUsers
            // 
            this.lstUsers.FormattingEnabled = true;
            this.lstUsers.Location = new System.Drawing.Point(16, 89);
            this.lstUsers.Name = "lstUsers";
            this.lstUsers.Size = new System.Drawing.Size(368, 186);
            this.lstUsers.TabIndex = 1;
            this.lstUsers.SelectedIndexChanged += new System.EventHandler(this.lstUsers_SelectedIndexChanged);
            this.lstUsers.SelectedValueChanged += new System.EventHandler(this.lstUsers_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Users";
            // 
            // btnLoginMail
            // 
            this.btnLoginMail.Location = new System.Drawing.Point(279, 278);
            this.btnLoginMail.Name = "btnLoginMail";
            this.btnLoginMail.Size = new System.Drawing.Size(101, 23);
            this.btnLoginMail.TabIndex = 3;
            this.btnLoginMail.Text = "SearchInMail";
            this.btnLoginMail.UseVisualStyleBackColor = true;
            this.btnLoginMail.Click += new System.EventHandler(this.btnLoginMail_Click);
            // 
            // browser
            // 
            this.browser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.browser.Location = new System.Drawing.Point(410, 89);
            this.browser.MinimumSize = new System.Drawing.Size(20, 20);
            this.browser.Name = "browser";
            this.browser.ScriptErrorsSuppressed = true;
            this.browser.Size = new System.Drawing.Size(556, 582);
            this.browser.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(212, 307);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(158, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Some other site";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtMailFind
            // 
            this.txtMailFind.Location = new System.Drawing.Point(76, 281);
            this.txtMailFind.Name = "txtMailFind";
            this.txtMailFind.Size = new System.Drawing.Size(197, 20);
            this.txtMailFind.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 288);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Search";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // txtScenario
            // 
            this.txtScenario.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtScenario.Location = new System.Drawing.Point(12, 369);
            this.txtScenario.Multiline = true;
            this.txtScenario.Name = "txtScenario";
            this.txtScenario.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtScenario.Size = new System.Drawing.Size(368, 217);
            this.txtScenario.TabIndex = 8;
            this.txtScenario.Text = resources.GetString("txtScenario.Text");
            this.txtScenario.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btnApplyScenario
            // 
            this.btnApplyScenario.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnApplyScenario.Location = new System.Drawing.Point(266, 593);
            this.btnApplyScenario.Name = "btnApplyScenario";
            this.btnApplyScenario.Size = new System.Drawing.Size(114, 23);
            this.btnApplyScenario.TabIndex = 9;
            this.btnApplyScenario.Text = "Apply Scenario";
            this.btnApplyScenario.UseVisualStyleBackColor = true;
            this.btnApplyScenario.Click += new System.EventHandler(this.btnApplyScenario_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 694);
            this.Controls.Add(this.btnApplyScenario);
            this.Controls.Add(this.txtScenario);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMailFind);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.browser);
            this.Controls.Add(this.btnLoginMail);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstUsers);
            this.Controls.Add(this.btnOpenInputFile);
            this.Name = "FrmMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenInputFile;
        private System.Windows.Forms.OpenFileDialog openCsvFile;
        private System.Windows.Forms.ListBox lstUsers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLoginMail;
        private System.Windows.Forms.WebBrowser browser;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtMailFind;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtScenario;
        private System.Windows.Forms.Button btnApplyScenario;
    }
}

