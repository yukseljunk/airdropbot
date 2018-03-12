namespace AirdropBot
{
    partial class FrmUsers
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgUsers = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openCsvFile = new System.Windows.Forms.OpenFileDialog();
            this.saveCsvFile = new System.Windows.Forms.SaveFileDialog();
            this.btnGmail = new System.Windows.Forms.Button();
            this.btnCreatePwd = new System.Windows.Forms.Button();
            this.btnMew = new System.Windows.Forms.Button();
            this.btnTwit = new System.Windows.Forms.Button();
            this.btnKucoin = new System.Windows.Forms.Button();
            this.gbUserOps = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnDelUser = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgUsers)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.gbUserOps.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgUsers
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            this.dgUsers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgUsers.Location = new System.Drawing.Point(12, 38);
            this.dgUsers.Name = "dgUsers";
            this.dgUsers.Size = new System.Drawing.Size(1200, 530);
            this.dgUsers.TabIndex = 0;
            this.dgUsers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgUsers_CellContentClick);
            this.dgUsers.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgUsers_CellEndEdit);
            this.dgUsers.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgUsers_CellValidating);
            this.dgUsers.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgUsers_CellValueChanged);
            this.dgUsers.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgUsers_RowEnter);
            this.dgUsers.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgUsers_RowsAdded);
            this.dgUsers.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgUsers_RowsRemoved);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1238, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem1,
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(129, 22);
            this.toolStripMenuItem2.Text = "Save Users";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(126, 6);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.importToolStripMenuItem.Text = "Import...";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.exportToolStripMenuItem.Text = "Export...";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // openCsvFile
            // 
            this.openCsvFile.Filter = "CSV Files|*.csv";
            this.openCsvFile.RestoreDirectory = true;
            // 
            // saveCsvFile
            // 
            this.saveCsvFile.DefaultExt = "csv";
            this.saveCsvFile.Filter = "CSV Files|*.csv";
            this.saveCsvFile.RestoreDirectory = true;
            // 
            // btnGmail
            // 
            this.btnGmail.Location = new System.Drawing.Point(252, 21);
            this.btnGmail.Name = "btnGmail";
            this.btnGmail.Size = new System.Drawing.Size(228, 25);
            this.btnGmail.TabIndex = 4;
            this.btnGmail.Text = "Create gmail account";
            this.btnGmail.UseVisualStyleBackColor = true;
            this.btnGmail.Click += new System.EventHandler(this.btnGmail_Click);
            // 
            // btnCreatePwd
            // 
            this.btnCreatePwd.Location = new System.Drawing.Point(7, 21);
            this.btnCreatePwd.Name = "btnCreatePwd";
            this.btnCreatePwd.Size = new System.Drawing.Size(228, 25);
            this.btnCreatePwd.TabIndex = 5;
            this.btnCreatePwd.Text = "Create strong passwords";
            this.btnCreatePwd.UseVisualStyleBackColor = true;
            this.btnCreatePwd.Click += new System.EventHandler(this.btnCreatePwd_Click);
            // 
            // btnMew
            // 
            this.btnMew.Location = new System.Drawing.Point(758, 19);
            this.btnMew.Name = "btnMew";
            this.btnMew.Size = new System.Drawing.Size(228, 25);
            this.btnMew.TabIndex = 6;
            this.btnMew.Text = "Create mew account";
            this.btnMew.UseVisualStyleBackColor = true;
            this.btnMew.Click += new System.EventHandler(this.btnMew_Click);
            // 
            // btnTwit
            // 
            this.btnTwit.Location = new System.Drawing.Point(511, 21);
            this.btnTwit.Name = "btnTwit";
            this.btnTwit.Size = new System.Drawing.Size(228, 25);
            this.btnTwit.TabIndex = 7;
            this.btnTwit.Text = "Create twitter account";
            this.btnTwit.UseVisualStyleBackColor = true;
            this.btnTwit.Click += new System.EventHandler(this.btnTwit_Click);
            // 
            // btnKucoin
            // 
            this.btnKucoin.Location = new System.Drawing.Point(1010, 19);
            this.btnKucoin.Name = "btnKucoin";
            this.btnKucoin.Size = new System.Drawing.Size(172, 25);
            this.btnKucoin.TabIndex = 8;
            this.btnKucoin.Text = "Create kucoin account";
            this.btnKucoin.UseVisualStyleBackColor = true;
            this.btnKucoin.Click += new System.EventHandler(this.btnKucoin_Click);
            // 
            // gbUserOps
            // 
            this.gbUserOps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbUserOps.Controls.Add(this.button4);
            this.gbUserOps.Controls.Add(this.button3);
            this.gbUserOps.Controls.Add(this.button2);
            this.gbUserOps.Controls.Add(this.button1);
            this.gbUserOps.Controls.Add(this.btnDelUser);
            this.gbUserOps.Controls.Add(this.btnCreatePwd);
            this.gbUserOps.Controls.Add(this.btnKucoin);
            this.gbUserOps.Controls.Add(this.btnMew);
            this.gbUserOps.Controls.Add(this.btnGmail);
            this.gbUserOps.Controls.Add(this.btnTwit);
            this.gbUserOps.Enabled = false;
            this.gbUserOps.Location = new System.Drawing.Point(12, 574);
            this.gbUserOps.Name = "gbUserOps";
            this.gbUserOps.Size = new System.Drawing.Size(1200, 112);
            this.gbUserOps.TabIndex = 10;
            this.gbUserOps.TabStop = false;
            this.gbUserOps.Text = "User Operations";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(758, 65);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(228, 25);
            this.button3.TabIndex = 12;
            this.button3.Text = "Check balance";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(511, 65);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(228, 25);
            this.button2.TabIndex = 11;
            this.button2.Text = "Create twitter followers";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(252, 65);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(228, 25);
            this.button1.TabIndex = 10;
            this.button1.Text = "Check gmail account";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnDelUser
            // 
            this.btnDelUser.Location = new System.Drawing.Point(7, 65);
            this.btnDelUser.Name = "btnDelUser";
            this.btnDelUser.Size = new System.Drawing.Size(228, 25);
            this.btnDelUser.TabIndex = 9;
            this.btnDelUser.Text = "Delete Selected Users";
            this.btnDelUser.UseVisualStyleBackColor = true;
            this.btnDelUser.Click += new System.EventHandler(this.btnDelUser_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(1010, 65);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(172, 25);
            this.button4.TabIndex = 13;
            this.button4.Text = "Check Kucoin Account";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // FrmUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1238, 698);
            this.Controls.Add(this.gbUserOps);
            this.Controls.Add(this.dgUsers);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmUsers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmUsers";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmUsers_FormClosing);
            this.Load += new System.EventHandler(this.FrmUsers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgUsers)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.gbUserOps.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgUsers;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.OpenFileDialog openCsvFile;
        private System.Windows.Forms.SaveFileDialog saveCsvFile;
        private System.Windows.Forms.Button btnGmail;
        private System.Windows.Forms.Button btnCreatePwd;
        private System.Windows.Forms.Button btnMew;
        private System.Windows.Forms.Button btnTwit;
        private System.Windows.Forms.Button btnKucoin;
        private System.Windows.Forms.GroupBox gbUserOps;
        private System.Windows.Forms.Button btnDelUser;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button4;

    }
}