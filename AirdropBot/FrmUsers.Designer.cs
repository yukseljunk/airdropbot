﻿namespace AirdropBot
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgUsers = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sallaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byIgnoringFirstLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.doNotIgnoreAnyLinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSelectedOnesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.passwordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createStrongPasswordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gmailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createGmailAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkGmailAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.twitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createTwitterAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createTwitterFollowersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mEWToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createMewAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBalanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kucoinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createKucoinAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkKucoinAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.telegramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createWindowsUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideWindowsUserInLoginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.openTelegramForSelectedUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.facebookToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createFBAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkFBAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openCsvFile = new System.Windows.Forms.OpenFileDialog();
            this.saveCsvFile = new System.Windows.Forms.SaveFileDialog();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.createStrongPasswordsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.gmailToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.twitterToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mEWToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.kucoinToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.telegramToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createFollowersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.checkToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.createToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.checkToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.createWindowsUserToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.hideWindowsUToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openTelegramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgUsers)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgUsers
            // 
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.Color.White;
            this.dgUsers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle17;
            this.dgUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgUsers.Location = new System.Drawing.Point(12, 38);
            this.dgUsers.Name = "dgUsers";
            this.dgUsers.Size = new System.Drawing.Size(1375, 620);
            this.dgUsers.TabIndex = 0;
            this.dgUsers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgUsers_CellClick);
            this.dgUsers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgUsers_CellContentClick);
            this.dgUsers.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgUsers_CellEndEdit);
            this.dgUsers.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgUsers_CellMouseDown);
            this.dgUsers.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgUsers_CellValidating);
            this.dgUsers.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgUsers_CellValueChanged);
            this.dgUsers.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgUsers_RowEnter);
            this.dgUsers.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgUsers_RowsAdded);
            this.dgUsers.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgUsers_RowsRemoved);
            this.dgUsers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgUsers_KeyDown);
            this.dgUsers.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgUsers_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sallaToolStripMenuItem,
            this.toolStripMenuItem2,
            this.createStrongPasswordsToolStripMenuItem1,
            this.toolStripMenuItem3,
            this.gmailToolStripMenuItem1,
            this.twitterToolStripMenuItem1,
            this.mEWToolStripMenuItem1,
            this.kucoinToolStripMenuItem1,
            this.telegramToolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(205, 170);
            // 
            // sallaToolStripMenuItem
            // 
            this.sallaToolStripMenuItem.Name = "sallaToolStripMenuItem";
            this.sallaToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.sallaToolStripMenuItem.Text = "Delete";
            this.sallaToolStripMenuItem.Click += new System.EventHandler(this.sallaToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.usersToolStripMenuItem,
            this.passwordsToolStripMenuItem,
            this.gmailToolStripMenuItem,
            this.twitterToolStripMenuItem,
            this.mEWToolStripMenuItem,
            this.kucoinToolStripMenuItem,
            this.telegramToolStripMenuItem,
            this.facebookToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1399, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.byIgnoringFirstLineToolStripMenuItem,
            this.doNotIgnoreAnyLinToolStripMenuItem});
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.importToolStripMenuItem.Text = "Import...";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // byIgnoringFirstLineToolStripMenuItem
            // 
            this.byIgnoringFirstLineToolStripMenuItem.Name = "byIgnoringFirstLineToolStripMenuItem";
            this.byIgnoringFirstLineToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.byIgnoringFirstLineToolStripMenuItem.Text = "By ignoring first line";
            this.byIgnoringFirstLineToolStripMenuItem.Click += new System.EventHandler(this.byIgnoringFirstLineToolStripMenuItem_Click);
            // 
            // doNotIgnoreAnyLinToolStripMenuItem
            // 
            this.doNotIgnoreAnyLinToolStripMenuItem.Name = "doNotIgnoreAnyLinToolStripMenuItem";
            this.doNotIgnoreAnyLinToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.doNotIgnoreAnyLinToolStripMenuItem.Text = "Do not ignore any line";
            this.doNotIgnoreAnyLinToolStripMenuItem.Click += new System.EventHandler(this.doNotIgnoreAnyLinToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.exportToolStripMenuItem.Text = "Export...";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // usersToolStripMenuItem
            // 
            this.usersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAllToolStripMenuItem,
            this.deleteSelectedOnesToolStripMenuItem});
            this.usersToolStripMenuItem.Name = "usersToolStripMenuItem";
            this.usersToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.usersToolStripMenuItem.Text = "Users";
            // 
            // saveAllToolStripMenuItem
            // 
            this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.saveAllToolStripMenuItem.Text = "Save All";
            this.saveAllToolStripMenuItem.Click += new System.EventHandler(this.saveAllToolStripMenuItem_Click);
            // 
            // deleteSelectedOnesToolStripMenuItem
            // 
            this.deleteSelectedOnesToolStripMenuItem.Name = "deleteSelectedOnesToolStripMenuItem";
            this.deleteSelectedOnesToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.deleteSelectedOnesToolStripMenuItem.Text = "Delete selected ones";
            this.deleteSelectedOnesToolStripMenuItem.Click += new System.EventHandler(this.deleteSelectedOnesToolStripMenuItem_Click);
            // 
            // passwordsToolStripMenuItem
            // 
            this.passwordsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createStrongPasswordsToolStripMenuItem});
            this.passwordsToolStripMenuItem.Name = "passwordsToolStripMenuItem";
            this.passwordsToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.passwordsToolStripMenuItem.Text = "Passwords";
            // 
            // createStrongPasswordsToolStripMenuItem
            // 
            this.createStrongPasswordsToolStripMenuItem.Name = "createStrongPasswordsToolStripMenuItem";
            this.createStrongPasswordsToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.createStrongPasswordsToolStripMenuItem.Text = "Create Strong Passwords";
            this.createStrongPasswordsToolStripMenuItem.Click += new System.EventHandler(this.createStrongPasswordsToolStripMenuItem_Click);
            // 
            // gmailToolStripMenuItem
            // 
            this.gmailToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createGmailAccountToolStripMenuItem,
            this.checkGmailAccountToolStripMenuItem});
            this.gmailToolStripMenuItem.Name = "gmailToolStripMenuItem";
            this.gmailToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.gmailToolStripMenuItem.Text = "Gmail";
            // 
            // createGmailAccountToolStripMenuItem
            // 
            this.createGmailAccountToolStripMenuItem.Name = "createGmailAccountToolStripMenuItem";
            this.createGmailAccountToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.createGmailAccountToolStripMenuItem.Text = "Create gmail account";
            this.createGmailAccountToolStripMenuItem.Click += new System.EventHandler(this.createGmailAccountToolStripMenuItem_Click);
            // 
            // checkGmailAccountToolStripMenuItem
            // 
            this.checkGmailAccountToolStripMenuItem.Name = "checkGmailAccountToolStripMenuItem";
            this.checkGmailAccountToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.checkGmailAccountToolStripMenuItem.Text = "Check gmail account";
            this.checkGmailAccountToolStripMenuItem.Click += new System.EventHandler(this.checkGmailAccountToolStripMenuItem_Click);
            // 
            // twitterToolStripMenuItem
            // 
            this.twitterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createTwitterAccountToolStripMenuItem,
            this.createTwitterFollowersToolStripMenuItem});
            this.twitterToolStripMenuItem.Name = "twitterToolStripMenuItem";
            this.twitterToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.twitterToolStripMenuItem.Text = "Twitter";
            // 
            // createTwitterAccountToolStripMenuItem
            // 
            this.createTwitterAccountToolStripMenuItem.Name = "createTwitterAccountToolStripMenuItem";
            this.createTwitterAccountToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.createTwitterAccountToolStripMenuItem.Text = "Create twitter account";
            this.createTwitterAccountToolStripMenuItem.Click += new System.EventHandler(this.createTwitterAccountToolStripMenuItem_Click);
            // 
            // createTwitterFollowersToolStripMenuItem
            // 
            this.createTwitterFollowersToolStripMenuItem.Name = "createTwitterFollowersToolStripMenuItem";
            this.createTwitterFollowersToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.createTwitterFollowersToolStripMenuItem.Text = "Create twitter followers";
            this.createTwitterFollowersToolStripMenuItem.Click += new System.EventHandler(this.createTwitterFollowersToolStripMenuItem_Click);
            // 
            // mEWToolStripMenuItem
            // 
            this.mEWToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createMewAccountToolStripMenuItem,
            this.checkBalanceToolStripMenuItem});
            this.mEWToolStripMenuItem.Name = "mEWToolStripMenuItem";
            this.mEWToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.mEWToolStripMenuItem.Text = "MEW";
            // 
            // createMewAccountToolStripMenuItem
            // 
            this.createMewAccountToolStripMenuItem.Name = "createMewAccountToolStripMenuItem";
            this.createMewAccountToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.createMewAccountToolStripMenuItem.Text = "Create Mew Account";
            this.createMewAccountToolStripMenuItem.Click += new System.EventHandler(this.createMewAccountToolStripMenuItem_Click);
            // 
            // checkBalanceToolStripMenuItem
            // 
            this.checkBalanceToolStripMenuItem.Name = "checkBalanceToolStripMenuItem";
            this.checkBalanceToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.checkBalanceToolStripMenuItem.Text = "Check Balance";
            this.checkBalanceToolStripMenuItem.Click += new System.EventHandler(this.checkBalanceToolStripMenuItem_Click);
            // 
            // kucoinToolStripMenuItem
            // 
            this.kucoinToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createKucoinAccountToolStripMenuItem,
            this.checkKucoinAccountToolStripMenuItem});
            this.kucoinToolStripMenuItem.Name = "kucoinToolStripMenuItem";
            this.kucoinToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.kucoinToolStripMenuItem.Text = "Kucoin";
            // 
            // createKucoinAccountToolStripMenuItem
            // 
            this.createKucoinAccountToolStripMenuItem.Name = "createKucoinAccountToolStripMenuItem";
            this.createKucoinAccountToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.createKucoinAccountToolStripMenuItem.Text = "Create Kucoin Account";
            this.createKucoinAccountToolStripMenuItem.Click += new System.EventHandler(this.createKucoinAccountToolStripMenuItem_Click);
            // 
            // checkKucoinAccountToolStripMenuItem
            // 
            this.checkKucoinAccountToolStripMenuItem.Name = "checkKucoinAccountToolStripMenuItem";
            this.checkKucoinAccountToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.checkKucoinAccountToolStripMenuItem.Text = "Check Kucoin Account";
            this.checkKucoinAccountToolStripMenuItem.Click += new System.EventHandler(this.checkKucoinAccountToolStripMenuItem_Click);
            // 
            // telegramToolStripMenuItem
            // 
            this.telegramToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createWindowsUserToolStripMenuItem,
            this.hideWindowsUserInLoginToolStripMenuItem,
            this.toolStripMenuItem1,
            this.openTelegramForSelectedUserToolStripMenuItem});
            this.telegramToolStripMenuItem.Name = "telegramToolStripMenuItem";
            this.telegramToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.telegramToolStripMenuItem.Text = "Telegram";
            // 
            // createWindowsUserToolStripMenuItem
            // 
            this.createWindowsUserToolStripMenuItem.Name = "createWindowsUserToolStripMenuItem";
            this.createWindowsUserToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.createWindowsUserToolStripMenuItem.Text = "Create windows user";
            this.createWindowsUserToolStripMenuItem.Click += new System.EventHandler(this.createWindowsUserToolStripMenuItem_Click);
            // 
            // hideWindowsUserInLoginToolStripMenuItem
            // 
            this.hideWindowsUserInLoginToolStripMenuItem.Name = "hideWindowsUserInLoginToolStripMenuItem";
            this.hideWindowsUserInLoginToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.hideWindowsUserInLoginToolStripMenuItem.Text = "Hide windows user in login";
            this.hideWindowsUserInLoginToolStripMenuItem.Click += new System.EventHandler(this.hideWindowsUserInLoginToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(239, 6);
            // 
            // openTelegramForSelectedUserToolStripMenuItem
            // 
            this.openTelegramForSelectedUserToolStripMenuItem.Name = "openTelegramForSelectedUserToolStripMenuItem";
            this.openTelegramForSelectedUserToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.openTelegramForSelectedUserToolStripMenuItem.Text = "Open telegram for selected user";
            this.openTelegramForSelectedUserToolStripMenuItem.Click += new System.EventHandler(this.openTelegramForSelectedUserToolStripMenuItem_Click);
            // 
            // facebookToolStripMenuItem
            // 
            this.facebookToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createFBAccountToolStripMenuItem,
            this.checkFBAccountToolStripMenuItem});
            this.facebookToolStripMenuItem.Name = "facebookToolStripMenuItem";
            this.facebookToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
            this.facebookToolStripMenuItem.Text = "Facebook";
            // 
            // createFBAccountToolStripMenuItem
            // 
            this.createFBAccountToolStripMenuItem.Name = "createFBAccountToolStripMenuItem";
            this.createFBAccountToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.createFBAccountToolStripMenuItem.Text = "Create FB account";
            this.createFBAccountToolStripMenuItem.Visible = false;
            // 
            // checkFBAccountToolStripMenuItem
            // 
            this.checkFBAccountToolStripMenuItem.Name = "checkFBAccountToolStripMenuItem";
            this.checkFBAccountToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.checkFBAccountToolStripMenuItem.Text = "Check FB account";
            this.checkFBAccountToolStripMenuItem.Click += new System.EventHandler(this.checkFBAccountToolStripMenuItem_Click);
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
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(201, 6);
            // 
            // createStrongPasswordsToolStripMenuItem1
            // 
            this.createStrongPasswordsToolStripMenuItem1.Name = "createStrongPasswordsToolStripMenuItem1";
            this.createStrongPasswordsToolStripMenuItem1.Size = new System.Drawing.Size(204, 22);
            this.createStrongPasswordsToolStripMenuItem1.Text = "Create Strong Passwords";
            this.createStrongPasswordsToolStripMenuItem1.Click += new System.EventHandler(this.createStrongPasswordsToolStripMenuItem1_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(201, 6);
            // 
            // gmailToolStripMenuItem1
            // 
            this.gmailToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createToolStripMenuItem,
            this.checkToolStripMenuItem});
            this.gmailToolStripMenuItem1.Name = "gmailToolStripMenuItem1";
            this.gmailToolStripMenuItem1.Size = new System.Drawing.Size(204, 22);
            this.gmailToolStripMenuItem1.Text = "Gmail";
            // 
            // twitterToolStripMenuItem1
            // 
            this.twitterToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createAccountToolStripMenuItem,
            this.createFollowersToolStripMenuItem});
            this.twitterToolStripMenuItem1.Name = "twitterToolStripMenuItem1";
            this.twitterToolStripMenuItem1.Size = new System.Drawing.Size(204, 22);
            this.twitterToolStripMenuItem1.Text = "Twitter";
            // 
            // mEWToolStripMenuItem1
            // 
            this.mEWToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createToolStripMenuItem1,
            this.checkToolStripMenuItem1});
            this.mEWToolStripMenuItem1.Name = "mEWToolStripMenuItem1";
            this.mEWToolStripMenuItem1.Size = new System.Drawing.Size(204, 22);
            this.mEWToolStripMenuItem1.Text = "MEW";
            // 
            // kucoinToolStripMenuItem1
            // 
            this.kucoinToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createToolStripMenuItem2,
            this.checkToolStripMenuItem2});
            this.kucoinToolStripMenuItem1.Name = "kucoinToolStripMenuItem1";
            this.kucoinToolStripMenuItem1.Size = new System.Drawing.Size(204, 22);
            this.kucoinToolStripMenuItem1.Text = "Kucoin";
            // 
            // telegramToolStripMenuItem1
            // 
            this.telegramToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createWindowsUserToolStripMenuItem1,
            this.hideWindowsUToolStripMenuItem,
            this.openTelegramToolStripMenuItem});
            this.telegramToolStripMenuItem1.Name = "telegramToolStripMenuItem1";
            this.telegramToolStripMenuItem1.Size = new System.Drawing.Size(204, 22);
            this.telegramToolStripMenuItem1.Text = "Telegram";
            // 
            // createToolStripMenuItem
            // 
            this.createToolStripMenuItem.Name = "createToolStripMenuItem";
            this.createToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.createToolStripMenuItem.Text = "Create";
            this.createToolStripMenuItem.Click += new System.EventHandler(this.createToolStripMenuItem_Click);
            // 
            // checkToolStripMenuItem
            // 
            this.checkToolStripMenuItem.Name = "checkToolStripMenuItem";
            this.checkToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.checkToolStripMenuItem.Text = "Check";
            this.checkToolStripMenuItem.Click += new System.EventHandler(this.checkToolStripMenuItem_Click);
            // 
            // createAccountToolStripMenuItem
            // 
            this.createAccountToolStripMenuItem.Name = "createAccountToolStripMenuItem";
            this.createAccountToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.createAccountToolStripMenuItem.Text = "Create account";
            this.createAccountToolStripMenuItem.Click += new System.EventHandler(this.createAccountToolStripMenuItem_Click);
            // 
            // createFollowersToolStripMenuItem
            // 
            this.createFollowersToolStripMenuItem.Name = "createFollowersToolStripMenuItem";
            this.createFollowersToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.createFollowersToolStripMenuItem.Text = "Create Followers";
            this.createFollowersToolStripMenuItem.Click += new System.EventHandler(this.createFollowersToolStripMenuItem_Click);
            // 
            // createToolStripMenuItem1
            // 
            this.createToolStripMenuItem1.Name = "createToolStripMenuItem1";
            this.createToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.createToolStripMenuItem1.Text = "Create";
            this.createToolStripMenuItem1.Click += new System.EventHandler(this.createToolStripMenuItem1_Click);
            // 
            // checkToolStripMenuItem1
            // 
            this.checkToolStripMenuItem1.Name = "checkToolStripMenuItem1";
            this.checkToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.checkToolStripMenuItem1.Text = "Check";
            this.checkToolStripMenuItem1.Click += new System.EventHandler(this.checkToolStripMenuItem1_Click);
            // 
            // createToolStripMenuItem2
            // 
            this.createToolStripMenuItem2.Name = "createToolStripMenuItem2";
            this.createToolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.createToolStripMenuItem2.Text = "Create";
            this.createToolStripMenuItem2.Click += new System.EventHandler(this.createToolStripMenuItem2_Click);
            // 
            // checkToolStripMenuItem2
            // 
            this.checkToolStripMenuItem2.Name = "checkToolStripMenuItem2";
            this.checkToolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.checkToolStripMenuItem2.Text = "Check";
            this.checkToolStripMenuItem2.Click += new System.EventHandler(this.checkToolStripMenuItem2_Click);
            // 
            // createWindowsUserToolStripMenuItem1
            // 
            this.createWindowsUserToolStripMenuItem1.Name = "createWindowsUserToolStripMenuItem1";
            this.createWindowsUserToolStripMenuItem1.Size = new System.Drawing.Size(186, 22);
            this.createWindowsUserToolStripMenuItem1.Text = "Create Windows User";
            this.createWindowsUserToolStripMenuItem1.Click += new System.EventHandler(this.createWindowsUserToolStripMenuItem1_Click);
            // 
            // hideWindowsUToolStripMenuItem
            // 
            this.hideWindowsUToolStripMenuItem.Name = "hideWindowsUToolStripMenuItem";
            this.hideWindowsUToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.hideWindowsUToolStripMenuItem.Text = "Hide Windows User";
            this.hideWindowsUToolStripMenuItem.Click += new System.EventHandler(this.hideWindowsUToolStripMenuItem_Click);
            // 
            // openTelegramToolStripMenuItem
            // 
            this.openTelegramToolStripMenuItem.Name = "openTelegramToolStripMenuItem";
            this.openTelegramToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.openTelegramToolStripMenuItem.Text = "Open Telegram";
            this.openTelegramToolStripMenuItem.Click += new System.EventHandler(this.openTelegramToolStripMenuItem_Click);
            // 
            // FrmUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1399, 680);
            this.Controls.Add(this.dgUsers);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmUsers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmUsers";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmUsers_FormClosing);
            this.Load += new System.EventHandler(this.FrmUsers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgUsers)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgUsers;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openCsvFile;
        private System.Windows.Forms.SaveFileDialog saveCsvFile;
        private System.Windows.Forms.ToolStripMenuItem byIgnoringFirstLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem doNotIgnoreAnyLinToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteSelectedOnesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem passwordsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createStrongPasswordsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gmailToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createGmailAccountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkGmailAccountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem twitterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createTwitterAccountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createTwitterFollowersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mEWToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createMewAccountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkBalanceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kucoinToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createKucoinAccountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkKucoinAccountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem telegramToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createWindowsUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideWindowsUserInLoginToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openTelegramForSelectedUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem facebookToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createFBAccountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkFBAccountToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem sallaToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem createStrongPasswordsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem gmailToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem twitterToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mEWToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem kucoinToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem telegramToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createAccountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createFollowersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem checkToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem checkToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem createWindowsUserToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem hideWindowsUToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openTelegramToolStripMenuItem;

    }
}