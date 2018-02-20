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
            this.openCsvFile = new System.Windows.Forms.OpenFileDialog();
            this.lstUsers = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtScenario = new System.Windows.Forms.TextBox();
            this.btnApplyScenario = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openUsersFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.scenarioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openScenarioFile = new System.Windows.Forms.OpenFileDialog();
            this.saveScenarioFile = new System.Windows.Forms.SaveFileDialog();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.commandsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.navigateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setFieldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getFieldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.navigateToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.setFieldToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.getFieldToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.clickToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.waitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tokensToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.variableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lastNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mailAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mailPasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.fBUsernameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fBPasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.winUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.winPassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.twitterNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.twitterEmailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.twitterPasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.ethAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ethPassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ethPrivateKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.proxyIPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.proxyPortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.redditUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redditPassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.btcTalkUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btcTalkPassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btcTalkProfileLinkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.browser = new System.Windows.Forms.WebBrowser();
            this.clearCookiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openCsvFile
            // 
            this.openCsvFile.Filter = "CSV Files|*.csv";
            this.openCsvFile.RestoreDirectory = true;
            // 
            // lstUsers
            // 
            this.lstUsers.FormattingEnabled = true;
            this.lstUsers.Location = new System.Drawing.Point(16, 64);
            this.lstUsers.Name = "lstUsers";
            this.lstUsers.Size = new System.Drawing.Size(368, 186);
            this.lstUsers.TabIndex = 1;
            this.lstUsers.SelectedIndexChanged += new System.EventHandler(this.lstUsers_SelectedIndexChanged);
            this.lstUsers.SelectedValueChanged += new System.EventHandler(this.lstUsers_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Users";
            // 
            // txtScenario
            // 
            this.txtScenario.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtScenario.Location = new System.Drawing.Point(12, 290);
            this.txtScenario.Multiline = true;
            this.txtScenario.Name = "txtScenario";
            this.txtScenario.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtScenario.Size = new System.Drawing.Size(368, 341);
            this.txtScenario.TabIndex = 8;
            this.txtScenario.Text = resources.GetString("txtScenario.Text");
            this.txtScenario.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btnApplyScenario
            // 
            this.btnApplyScenario.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnApplyScenario.Location = new System.Drawing.Point(200, 648);
            this.btnApplyScenario.Name = "btnApplyScenario";
            this.btnApplyScenario.Size = new System.Drawing.Size(180, 23);
            this.btnApplyScenario.TabIndex = 9;
            this.btnApplyScenario.Text = "Run Scenario For Selected User";
            this.btnApplyScenario.UseVisualStyleBackColor = true;
            this.btnApplyScenario.Click += new System.EventHandler(this.btnApplyScenario_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.scenarioToolStripMenuItem,
            this.commandsToolStripMenuItem,
            this.tokensToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(989, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openUsersFileToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openUsersFileToolStripMenuItem
            // 
            this.openUsersFileToolStripMenuItem.Name = "openUsersFileToolStripMenuItem";
            this.openUsersFileToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.openUsersFileToolStripMenuItem.Text = "Open Users File...";
            this.openUsersFileToolStripMenuItem.Click += new System.EventHandler(this.openUsersFileToolStripMenuItem_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 263);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Scenario";
            // 
            // scenarioToolStripMenuItem
            // 
            this.scenarioToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.scenarioToolStripMenuItem.Name = "scenarioToolStripMenuItem";
            this.scenarioToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.scenarioToolStripMenuItem.Text = "Scenario";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadToolStripMenuItem.Text = "Load...";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // openScenarioFile
            // 
            this.openScenarioFile.Filter = "Scenario Files|*.xml";
            this.openScenarioFile.RestoreDirectory = true;
            // 
            // saveScenarioFile
            // 
            this.saveScenarioFile.DefaultExt = "xml";
            this.saveScenarioFile.Filter = "Scenario Files|*.xml";
            this.saveScenarioFile.RestoreDirectory = true;
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveAsToolStripMenuItem.Text = "Save As ...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem1.Text = "New";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // commandsToolStripMenuItem
            // 
            this.commandsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.navigateToolStripMenuItem,
            this.setFieldToolStripMenuItem,
            this.getFieldToolStripMenuItem,
            this.waitToolStripMenuItem});
            this.commandsToolStripMenuItem.Name = "commandsToolStripMenuItem";
            this.commandsToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.commandsToolStripMenuItem.Text = "Commands";
            // 
            // navigateToolStripMenuItem
            // 
            this.navigateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.navigateToolStripMenuItem1,
            this.setFieldToolStripMenuItem1,
            this.getFieldToolStripMenuItem1,
            this.clickToolStripMenuItem,
            this.clearCookiesToolStripMenuItem});
            this.navigateToolStripMenuItem.Name = "navigateToolStripMenuItem";
            this.navigateToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.navigateToolStripMenuItem.Text = "Browser";
            this.navigateToolStripMenuItem.Click += new System.EventHandler(this.navigateToolStripMenuItem_Click);
            // 
            // setFieldToolStripMenuItem
            // 
            this.setFieldToolStripMenuItem.Name = "setFieldToolStripMenuItem";
            this.setFieldToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.setFieldToolStripMenuItem.Text = "Gmail";
            this.setFieldToolStripMenuItem.Click += new System.EventHandler(this.setFieldToolStripMenuItem_Click);
            // 
            // getFieldToolStripMenuItem
            // 
            this.getFieldToolStripMenuItem.Name = "getFieldToolStripMenuItem";
            this.getFieldToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.getFieldToolStripMenuItem.Text = "Telegram";
            this.getFieldToolStripMenuItem.Click += new System.EventHandler(this.getFieldToolStripMenuItem_Click);
            // 
            // navigateToolStripMenuItem1
            // 
            this.navigateToolStripMenuItem1.Name = "navigateToolStripMenuItem1";
            this.navigateToolStripMenuItem1.Size = new System.Drawing.Size(121, 22);
            this.navigateToolStripMenuItem1.Text = "Navigate";
            this.navigateToolStripMenuItem1.Click += new System.EventHandler(this.navigateToolStripMenuItem1_Click);
            // 
            // setFieldToolStripMenuItem1
            // 
            this.setFieldToolStripMenuItem1.Name = "setFieldToolStripMenuItem1";
            this.setFieldToolStripMenuItem1.Size = new System.Drawing.Size(121, 22);
            this.setFieldToolStripMenuItem1.Text = "Set Field";
            this.setFieldToolStripMenuItem1.Click += new System.EventHandler(this.setFieldToolStripMenuItem1_Click);
            // 
            // getFieldToolStripMenuItem1
            // 
            this.getFieldToolStripMenuItem1.Name = "getFieldToolStripMenuItem1";
            this.getFieldToolStripMenuItem1.Size = new System.Drawing.Size(121, 22);
            this.getFieldToolStripMenuItem1.Text = "Get Field";
            this.getFieldToolStripMenuItem1.Click += new System.EventHandler(this.getFieldToolStripMenuItem1_Click);
            // 
            // clickToolStripMenuItem
            // 
            this.clickToolStripMenuItem.Name = "clickToolStripMenuItem";
            this.clickToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.clickToolStripMenuItem.Text = "Click";
            this.clickToolStripMenuItem.Click += new System.EventHandler(this.clickToolStripMenuItem_Click);
            // 
            // waitToolStripMenuItem
            // 
            this.waitToolStripMenuItem.Name = "waitToolStripMenuItem";
            this.waitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.waitToolStripMenuItem.Text = "Wait";
            this.waitToolStripMenuItem.Click += new System.EventHandler(this.waitToolStripMenuItem_Click);
            // 
            // tokensToolStripMenuItem
            // 
            this.tokensToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.userToolStripMenuItem,
            this.variableToolStripMenuItem});
            this.tokensToolStripMenuItem.Name = "tokensToolStripMenuItem";
            this.tokensToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.tokensToolStripMenuItem.Text = "Tokens";
            // 
            // userToolStripMenuItem
            // 
            this.userToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nameToolStripMenuItem,
            this.lastNameToolStripMenuItem,
            this.toolStripMenuItem3,
            this.mailAddressToolStripMenuItem,
            this.mailPasswordToolStripMenuItem,
            this.toolStripMenuItem2,
            this.fBUsernameToolStripMenuItem,
            this.fBPasswordToolStripMenuItem,
            this.toolStripMenuItem4,
            this.winUserToolStripMenuItem,
            this.winPassToolStripMenuItem,
            this.toolStripMenuItem6,
            this.twitterNameToolStripMenuItem,
            this.twitterEmailToolStripMenuItem,
            this.twitterPasswordToolStripMenuItem,
            this.toolStripMenuItem5,
            this.ethAddressToolStripMenuItem,
            this.ethPassToolStripMenuItem,
            this.ethPrivateKeyToolStripMenuItem,
            this.toolStripMenuItem7,
            this.proxyIPToolStripMenuItem,
            this.proxyPortToolStripMenuItem,
            this.toolStripMenuItem8,
            this.redditUserToolStripMenuItem,
            this.redditPassToolStripMenuItem,
            this.toolStripMenuItem9,
            this.btcTalkUserToolStripMenuItem,
            this.btcTalkPassToolStripMenuItem,
            this.btcTalkProfileLinkToolStripMenuItem});
            this.userToolStripMenuItem.Name = "userToolStripMenuItem";
            this.userToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.userToolStripMenuItem.Text = "User";
            // 
            // nameToolStripMenuItem
            // 
            this.nameToolStripMenuItem.Name = "nameToolStripMenuItem";
            this.nameToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.nameToolStripMenuItem.Text = "Name";
            this.nameToolStripMenuItem.Click += new System.EventHandler(this.nameToolStripMenuItem_Click);
            // 
            // variableToolStripMenuItem
            // 
            this.variableToolStripMenuItem.Name = "variableToolStripMenuItem";
            this.variableToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.variableToolStripMenuItem.Text = "Get Variable";
            this.variableToolStripMenuItem.Click += new System.EventHandler(this.variableToolStripMenuItem_Click);
            // 
            // lastNameToolStripMenuItem
            // 
            this.lastNameToolStripMenuItem.Name = "lastNameToolStripMenuItem";
            this.lastNameToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.lastNameToolStripMenuItem.Text = "LastName";
            this.lastNameToolStripMenuItem.Click += new System.EventHandler(this.lastNameToolStripMenuItem_Click);
            // 
            // mailAddressToolStripMenuItem
            // 
            this.mailAddressToolStripMenuItem.Name = "mailAddressToolStripMenuItem";
            this.mailAddressToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.mailAddressToolStripMenuItem.Text = "Mail Address";
            this.mailAddressToolStripMenuItem.Click += new System.EventHandler(this.mailAddressToolStripMenuItem_Click);
            // 
            // mailPasswordToolStripMenuItem
            // 
            this.mailPasswordToolStripMenuItem.Name = "mailPasswordToolStripMenuItem";
            this.mailPasswordToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.mailPasswordToolStripMenuItem.Text = "Mail Password";
            this.mailPasswordToolStripMenuItem.Click += new System.EventHandler(this.mailPasswordToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(172, 6);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(172, 6);
            // 
            // fBUsernameToolStripMenuItem
            // 
            this.fBUsernameToolStripMenuItem.Name = "fBUsernameToolStripMenuItem";
            this.fBUsernameToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.fBUsernameToolStripMenuItem.Text = "FB Username";
            this.fBUsernameToolStripMenuItem.Click += new System.EventHandler(this.fBUsernameToolStripMenuItem_Click);
            // 
            // fBPasswordToolStripMenuItem
            // 
            this.fBPasswordToolStripMenuItem.Name = "fBPasswordToolStripMenuItem";
            this.fBPasswordToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.fBPasswordToolStripMenuItem.Text = "FB Password";
            this.fBPasswordToolStripMenuItem.Click += new System.EventHandler(this.fBPasswordToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(172, 6);
            // 
            // winUserToolStripMenuItem
            // 
            this.winUserToolStripMenuItem.Name = "winUserToolStripMenuItem";
            this.winUserToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.winUserToolStripMenuItem.Text = "Win User";
            this.winUserToolStripMenuItem.Click += new System.EventHandler(this.winUserToolStripMenuItem_Click);
            // 
            // winPassToolStripMenuItem
            // 
            this.winPassToolStripMenuItem.Name = "winPassToolStripMenuItem";
            this.winPassToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.winPassToolStripMenuItem.Text = "Win Pass";
            this.winPassToolStripMenuItem.Click += new System.EventHandler(this.winPassToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(172, 6);
            // 
            // twitterNameToolStripMenuItem
            // 
            this.twitterNameToolStripMenuItem.Name = "twitterNameToolStripMenuItem";
            this.twitterNameToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.twitterNameToolStripMenuItem.Text = "Twitter Name";
            this.twitterNameToolStripMenuItem.Click += new System.EventHandler(this.twitterNameToolStripMenuItem_Click);
            // 
            // twitterEmailToolStripMenuItem
            // 
            this.twitterEmailToolStripMenuItem.Name = "twitterEmailToolStripMenuItem";
            this.twitterEmailToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.twitterEmailToolStripMenuItem.Text = "Twitter Email";
            this.twitterEmailToolStripMenuItem.Click += new System.EventHandler(this.twitterEmailToolStripMenuItem_Click);
            // 
            // twitterPasswordToolStripMenuItem
            // 
            this.twitterPasswordToolStripMenuItem.Name = "twitterPasswordToolStripMenuItem";
            this.twitterPasswordToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.twitterPasswordToolStripMenuItem.Text = "Twitter Password";
            this.twitterPasswordToolStripMenuItem.Click += new System.EventHandler(this.twitterPasswordToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(172, 6);
            // 
            // ethAddressToolStripMenuItem
            // 
            this.ethAddressToolStripMenuItem.Name = "ethAddressToolStripMenuItem";
            this.ethAddressToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.ethAddressToolStripMenuItem.Text = "Eth Address";
            this.ethAddressToolStripMenuItem.Click += new System.EventHandler(this.ethAddressToolStripMenuItem_Click);
            // 
            // ethPassToolStripMenuItem
            // 
            this.ethPassToolStripMenuItem.Name = "ethPassToolStripMenuItem";
            this.ethPassToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.ethPassToolStripMenuItem.Text = "Eth Pass";
            this.ethPassToolStripMenuItem.Click += new System.EventHandler(this.ethPassToolStripMenuItem_Click);
            // 
            // ethPrivateKeyToolStripMenuItem
            // 
            this.ethPrivateKeyToolStripMenuItem.Name = "ethPrivateKeyToolStripMenuItem";
            this.ethPrivateKeyToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.ethPrivateKeyToolStripMenuItem.Text = "Eth Private Key";
            this.ethPrivateKeyToolStripMenuItem.Click += new System.EventHandler(this.ethPrivateKeyToolStripMenuItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(172, 6);
            // 
            // proxyIPToolStripMenuItem
            // 
            this.proxyIPToolStripMenuItem.Name = "proxyIPToolStripMenuItem";
            this.proxyIPToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.proxyIPToolStripMenuItem.Text = "Proxy IP";
            this.proxyIPToolStripMenuItem.Click += new System.EventHandler(this.proxyIPToolStripMenuItem_Click);
            // 
            // proxyPortToolStripMenuItem
            // 
            this.proxyPortToolStripMenuItem.Name = "proxyPortToolStripMenuItem";
            this.proxyPortToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.proxyPortToolStripMenuItem.Text = "Proxy Port";
            this.proxyPortToolStripMenuItem.Click += new System.EventHandler(this.proxyPortToolStripMenuItem_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(172, 6);
            // 
            // redditUserToolStripMenuItem
            // 
            this.redditUserToolStripMenuItem.Name = "redditUserToolStripMenuItem";
            this.redditUserToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.redditUserToolStripMenuItem.Text = "Reddit User";
            this.redditUserToolStripMenuItem.Click += new System.EventHandler(this.redditUserToolStripMenuItem_Click);
            // 
            // redditPassToolStripMenuItem
            // 
            this.redditPassToolStripMenuItem.Name = "redditPassToolStripMenuItem";
            this.redditPassToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.redditPassToolStripMenuItem.Text = "Reddit Pass";
            this.redditPassToolStripMenuItem.Click += new System.EventHandler(this.redditPassToolStripMenuItem_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(172, 6);
            // 
            // btcTalkUserToolStripMenuItem
            // 
            this.btcTalkUserToolStripMenuItem.Name = "btcTalkUserToolStripMenuItem";
            this.btcTalkUserToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.btcTalkUserToolStripMenuItem.Text = "BtcTalk User";
            this.btcTalkUserToolStripMenuItem.Click += new System.EventHandler(this.btcTalkUserToolStripMenuItem_Click);
            // 
            // btcTalkPassToolStripMenuItem
            // 
            this.btcTalkPassToolStripMenuItem.Name = "btcTalkPassToolStripMenuItem";
            this.btcTalkPassToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.btcTalkPassToolStripMenuItem.Text = "BtcTalk Pass";
            this.btcTalkPassToolStripMenuItem.Click += new System.EventHandler(this.btcTalkPassToolStripMenuItem_Click);
            // 
            // btcTalkProfileLinkToolStripMenuItem
            // 
            this.btcTalkProfileLinkToolStripMenuItem.Name = "btcTalkProfileLinkToolStripMenuItem";
            this.btcTalkProfileLinkToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.btcTalkProfileLinkToolStripMenuItem.Text = "BtcTalk Profile Link";
            this.btcTalkProfileLinkToolStripMenuItem.Click += new System.EventHandler(this.btcTalkProfileLinkToolStripMenuItem_Click);
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
            // clearCookiesToolStripMenuItem
            // 
            this.clearCookiesToolStripMenuItem.Name = "clearCookiesToolStripMenuItem";
            this.clearCookiesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.clearCookiesToolStripMenuItem.Text = "Clear Cookies";
            this.clearCookiesToolStripMenuItem.Click += new System.EventHandler(this.clearCookiesToolStripMenuItem_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 694);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnApplyScenario);
            this.Controls.Add(this.txtScenario);
            this.Controls.Add(this.browser);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstUsers);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmMain";
            this.Text = "Airdrop Bot v1.0";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openCsvFile;
        private System.Windows.Forms.ListBox lstUsers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.WebBrowser browser;
        private System.Windows.Forms.TextBox txtScenario;
        private System.Windows.Forms.Button btnApplyScenario;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openUsersFileToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem scenarioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openScenarioFile;
        private System.Windows.Forms.SaveFileDialog saveScenarioFile;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem commandsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem navigateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setFieldToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getFieldToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem navigateToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem setFieldToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem getFieldToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem clickToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem waitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tokensToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem variableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lastNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mailAddressToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mailPasswordToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem fBUsernameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fBPasswordToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem winUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem winPassToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem twitterNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem twitterEmailToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem twitterPasswordToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem ethAddressToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ethPassToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ethPrivateKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem proxyIPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem proxyPortToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem redditUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redditPassToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem btcTalkUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btcTalkPassToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btcTalkProfileLinkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearCookiesToolStripMenuItem;
    }
}

