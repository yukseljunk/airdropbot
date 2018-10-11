using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Common;

namespace AirdropBot
{
    public partial class FrmUsers : Form
    {
        private Dictionary<int, User> Users = new Dictionary<int, User>();

        public FrmUsers()
        {
            InitializeComponent();
        }

        private void FrmUsers_Load(object sender, EventArgs e)
        {
            var cols = new List<string>()
                           {
                            "no", 
                            "name", 
                            "lastname", 
                            "email", 
                            "email pwd",
                            "phone",
                            "strong pwd",
                             "very strong pwd",
                             "eth address",
                             "eth private key",
                             "eth pwd",
                             "proxy url",
                             "proxy port",
                             "win login",
                             "win pwd",
                             "telegram user",
                             "twitter user",
                             "twitter pwd",
                             "twitter name",
                             "kucoin user",
                             "kucoin pwd",
                             "fb user",
                             "fb pwd",
                             "fb profile link",
                             "reddit usr",
                             "reddit pwd",
                             "btctalk usr",
                             "btctalk pwd",
                             "btctalk profile",
                             "Twitter Consumer Key",
                             "Twitter Consumer Secret",
                            "Twitter Access Token",
                            "Twitter Access Token Secret",
                            "Neo Address",
                            "Neo Private Key",
                            "Kucoin GSecret",
                            "GSecret1",
                            "GSecret2",
                            "GSecret3",
                            "GSecret4",
                            "GSecret5",
                            "CustomField1",
                            "CustomField2",
                            "CustomField3",
                            "CustomField4",
                            "CustomField5",
                            "CustomField6",
                            "CustomField7",
                            "CustomField8",
                            "CustomField9"

                           };
            foreach (var col in cols)
            {
                dgUsers.Columns.Add(col.Replace(" ", "_"), col);

            }
            dgUsers.Columns[0].ReadOnly = true;
            if (File.Exists(CommonHelper.UsersFile))
            {
                var userFactory = new UserFactory();
                Users = userFactory.GetUsers(CommonHelper.UsersFile, false);
                FillUsers();
            }
            IsDirty = false;
            IsEdited = false;
        }

        private void dgUsers_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            IndexRows();
        }

        private void IndexRows()
        {
            for (int i = 0; i < dgUsers.Rows.Count; i++)
            {
                dgUsers.Rows[i].Cells[0].Value = i + 1;
            }
        }

        private void dgUsers_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            IndexRows();

        }

        private void dgUsers_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Clear the row error in case the user presses ESC.   
            dgUsers.Rows[e.RowIndex].ErrorText = String.Empty;
        }

        private void dgUsers_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // Confirm that the cell is not empty.
            if (e.FormattedValue.ToString().Contains(","))
            {
                dgUsers.Rows[e.RowIndex].ErrorText =
                    "Cannot contain comma(,) character!";
                e.Cancel = true;
            }
            if (e.ColumnIndex == 3 && e.FormattedValue.ToString() != "")
            {
                var emails = GetEmails();
                var foundIndex = emails.IndexOf(e.FormattedValue.ToString());
                if (foundIndex != -1 && foundIndex != e.RowIndex)
                {
                    dgUsers.Rows[e.RowIndex].ErrorText =
                        "Duplicate email!";
                    e.Cancel = true;
                }

            }
        }

        private List<string> GetEmails()
        {
            var cellVals = new List<string>();
            foreach (DataGridViewRow row in dgUsers.Rows)
            {
                var rowIndex = int.Parse(row.Cells[0].Value.ToString());
                if (rowIndex == dgUsers.Rows.Count)
                {
                    break;
                }
                cellVals.Add(row.Cells[3].Value == null ? "" : row.Cells[3].Value.ToString());
            }
            return cellVals;
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ImportUsers(bool ignoreHeader)
        {
            DialogResult result = openCsvFile.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                var userFactory = new UserFactory();
                Users = userFactory.GetUsers(openCsvFile.FileName, ignoreHeader);
                FillUsers();
            }
        }


        private void FillUsers()
        {
            dgUsers.Rows.Clear();
            foreach (var user in Users)
            {
                var u = user.Value;
                dgUsers.Rows.Add(1, u.Name, u.LastName, u.Mail, u.MailPwd, u.Phone, u.StrongPassword, u.StrongPwdWithSign,
                                 u.EthAddress, u.EthPrivateKey, u.EthPass, u.ProxyIp, u.ProxyPort,
                                 u.WinUser, u.WinPwd, u.TgUser, u.TwUserName, u.TwPwd, u.TwName, u.KucoinUser,
                                 u.KucoinPass, u.FBUser, u.FBPwd, u.FBProfile, u.ReddUser, u.ReddPwd, u.BtcTalkUser,
                                 u.BtcTalkPwd, u.BtcTalkProfileLink, u.TwConsumerKey, u.TwConsumerSecret, u.TwAccessToken, u.TwAccessTokenSecret, u.NeoAddress, u.NeoPrivateKey,
                                 u.KucoinGSecret, u.GSecret1, u.GSecret2, u.GSecret3, u.GSecret4, u.GSecret5,
                                 u.CustomField1, u.CustomField2, u.CustomField3, u.CustomField4, u.CustomField5, u.CustomField6, u.CustomField7, u.CustomField8, u.CustomField9);
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }


        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void SaveUsers(string file)
        {
            File.WriteAllText(file, "");
            var rowCounter = 0;
            foreach (DataGridViewRow row in dgUsers.Rows)
            {
                rowCounter++;
                var rowIndex = int.Parse(row.Cells[0].Value.ToString());
                if (rowIndex == dgUsers.Rows.Count)
                {
                    break;
                }
                var cellVals = new List<string>();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cellVals.Add(cell.Value == null ? "" : cell.Value.ToString());
                }
                cellVals[0] = rowCounter.ToString();
                var singleRow = string.Join(",", cellVals) + "\r\n";
                File.AppendAllText(file, singleRow);
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = saveCsvFile.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                SaveUsers(saveCsvFile.FileName);
            }
        }

        public bool IsEdited
        {
            get { return _isEdited; }
            set { _isEdited = value; }
        }

        private bool _isDirty = false;
        private bool _isEdited = false;
        public bool IsDirty { get { return _isDirty; } set { _isDirty = value; } }

        private void dgUsers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            IsDirty = true;
            _isEdited = true;
        }

        private void FrmUsers_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && IsDirty)
            {
                if (MessageBox.Show("Do you want to save your changes to users?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SaveUsers(CommonHelper.UsersFile);
                }
            }
        }

        private void dgUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgUsers_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            // gbUserOps.Enabled = true;
        }

        private void btnDelUser_Click(object sender, EventArgs e)
        {
            var indexes = GetSelectedRows();

            foreach (var rowIndex in indexes)
            {
                try
                {
                    if (MessageBox.Show("Are you sure you want to delete row " + (rowIndex + 1).ToString() + "?", "Confirm Deletion", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        dgUsers.Rows.RemoveAt(rowIndex);

                    }
                }
                catch { }
            }
        }

        private HashSet<int> GetSelectedRows()
        {
            var indexes = new HashSet<int>() { };
            indexes.Add(ActiveRowIndex);
            foreach (DataGridViewRow item in dgUsers.SelectedRows)
            {
                indexes.Add(item.Index);
            }
            return indexes;
        }

        private void btnCreatePwd_Click(object sender, EventArgs e)
        {
            var indexes = GetSelectedRows();
            foreach (var rowIndex in indexes)
            {
                CreateStrongPasswords(rowIndex);
            }
        }

        private void CreateStrongPasswords(int index)
        {

            try
            {
                if (EmptyCell(index, 1) || EmptyCell(index, 2))
                {
                    MessageBox.Show("Cannot create password for empty name and surname for row " + (index + 1));
                    return;
                }
                var name = GetCell(index, 1);
                var lastname = GetCell(index, 2);

                SetCellChecked(index, 4, CreatePassword(name, lastname, false));
                Thread.Sleep(45);
                SetCellChecked(index, 6, CreatePassword(name, lastname, false));
                Thread.Sleep(50);
                SetCellChecked(index, 7, CreatePassword(name, lastname, true));
                Thread.Sleep(40);
                SetCellChecked(index, 10, CreatePassword(name, lastname, false));
                Thread.Sleep(40);
                SetCellChecked(index, 14, CreatePassword(name, lastname, false));
                Thread.Sleep(40);
                SetCellChecked(index, 17, CreatePassword(name, lastname, false));
                Thread.Sleep(40);
                SetCellChecked(index, 20, CreatePassword(name, lastname, false));
                Thread.Sleep(40);
                SetCellChecked(index, 22, CreatePassword(name, lastname, false));
                Thread.Sleep(40);
                SetCellChecked(index, 25, CreatePassword(name, lastname, false));
                Thread.Sleep(40);
                SetCellChecked(index, 27, CreatePassword(name, lastname, false));
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private string CreatePassword(string name, string lastname, bool punctuation)
        {
            var sname = name.Replace("i", "").Replace("I", "").ToLower();
            var slastname = lastname.Replace("i", "").Replace("I", "").ToLower();

            var result = "";
            var rnd = new Random();

            if (sname.Length + slastname.Length > 12)
            {
                var crop = rnd.Next(1, 2);
                if (crop == 1)
                {
                    if (sname.Length > 5) sname = sname.Substring(0, rnd.Next(2, 5));
                    if (slastname.Length > 8) slastname = slastname.Substring(0, rnd.Next(5, 8));
                }
                else
                {
                    if (sname.Length > 8) sname = sname.Substring(0, rnd.Next(5, 8));
                    if (slastname.Length > 5) slastname = slastname.Substring(0, rnd.Next(2, 5));

                }
            }

            var pos = rnd.Next(1, 4);
            var addition = CreatePasswordNumbers(rnd.Next(1, 3));
            if (pos == 1)
            {
                result = addition + sname + slastname;
            }
            if (pos == 2)
            {
                result = sname + addition + slastname;
            }
            if (pos == 3)
            {
                result = sname + slastname + addition;
            }
            if (pos == 4)
            {
                result = sname + slastname;
                var maxrep = rnd.Next(1, 2);
                var rep = 0;
                var numarize = new Dictionary<string, string>() { { "o", "0" }, { "a", "6" }, { "e", "5" }, { "l", "1" }, { "s", "5" }, { "i", "1" } };
                foreach (var num in numarize)
                {
                    if (result.Contains(num.Key))
                    {
                        result = result.Replace(num.Key, num.Value);
                        rep++;
                        if (rep >= maxrep) break;
                    }

                }
            }

            var digsToUp = new HashSet<int>();
            var digitsToUpper = rnd.Next(0, result.Length - 2);
            while (true)
            {
                var index = rnd.Next(0, result.Length - 1);
                if (Char.IsNumber(result[index]))
                {
                    continue;
                }
                digsToUp.Add(rnd.Next(0, result.Length));
                if (digsToUp.Count >= digitsToUpper)
                {
                    break;
                }
            }
            var res = new List<char>();
            for (int i = 0; i < result.Length; i++)
            {
                if (digsToUp.Contains(i))
                {
                    res.Add(Char.ToUpper(result[i]));
                    continue;
                }
                res.Add(result[i]);
            }
            if (punctuation)
            {
                var punctLoc = rnd.Next(1, res.Count);
                var punctiationInd = rnd.Next(0, 2);
                var punctuations = new List<char>() { '+', '-', '*' };
                var punctuationToAdd = punctuations[punctiationInd];
                res.Insert(punctLoc, punctuationToAdd);
            }
            return string.Join("", res);
        }
        public string CreatePasswordSmall(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyz";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
        public string CreatePasswordNumbers(int length)
        {
            const string valid = "0123456789";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
        private void SetCellChecked(int row, int col, string val)
        {
            if (EmptyCell(row, col))
            {
                SetCell(row, col, val);
            }
        }


        private void SetCell(int row, int col, string val)
        {
            dgUsers.Rows[row].Cells[col].Value = val;
        }

        private bool EmptyCell(int row, int col)
        {
            return dgUsers.Rows[row].Cells[col].Value == null || dgUsers.Rows[row].Cells[col].Value.ToString().Trim() == "";
        }
        private string GetCell(int row, int col)
        {
            if (EmptyCell(row, col)) return "";
            return dgUsers.Rows[row].Cells[col].Value.ToString().Trim();
        }

        private void btnGmail_Click(object sender, EventArgs e)
        {
            CreateGmail(GetSelectedRow());
        }

        private int GetSelectedRow()
        {
            foreach (DataGridViewRow item in dgUsers.SelectedRows)
            {
                return item.Index;
            }
            return ActiveRowIndex;
        }

        private void CreateGmail(int index)
        {
            try
            {
                if (EmptyCell(index, 1) || EmptyCell(index, 2) || EmptyCell(index, 3) || EmptyCell(index, 4))
                {
                    MessageBox.Show("Cannot create gmail address for empty name, lastname, mail address and mail password! " + (index + 1));
                    return;
                }
                var gmailTemplate = File.ReadAllText(CommonHelper.AssemblyDirectory + "\\Templates\\GmailReg.xml");
                gmailTemplate = gmailTemplate.Replace("${0}", GetCell(index, 1)).Replace("${1}", GetCell(index, 2)).Replace("${2}", GetCell(index, 3).Replace("@gmail.com", "")).Replace("${3}", GetCell(index, 4));

                var frmMain = new FrmMain() { OnlyBrowser = true, Scenario = gmailTemplate };

                frmMain.Show(this);

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void btnTwit_Click(object sender, EventArgs e)
        {
            CreateTwitter(GetSelectedRow());
        }
        private void CreateTwitter(int index)
        {
            try
            {
                if (EmptyCell(index, 1) || EmptyCell(index, 2) || EmptyCell(index, 16) || EmptyCell(index, 17) || EmptyCell(index, 18))
                {
                    MessageBox.Show("Cannot create twitter address for empty name, lastname, twitter user, twitter password and twitter name! " + (index + 1));
                    return;
                }
                var twitterTemplate = File.ReadAllText(CommonHelper.AssemblyDirectory + "\\Templates\\TwitterReg.xml");
                twitterTemplate = twitterTemplate.Replace("${0}", GetCell(index, 1)).Replace("${1}", GetCell(index, 2)).Replace("${2}", GetCell(index, 16)).Replace("${3}", GetCell(index, 17)).Replace("${4}", GetCell(index, 18));

                var frmMain = new FrmMain() { OnlyBrowser = true, Scenario = twitterTemplate };

                frmMain.Show(this);

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CheckGmail(GetSelectedRow());
        }
        private void CheckGmail(int index)
        {
            try
            {
                if (EmptyCell(index, 3) || EmptyCell(index, 4))
                {
                    MessageBox.Show("Cannot check gmail address for empty mail address and mail password! " + (index + 1));
                    return;
                }
                var gmailTemplate = File.ReadAllText(CommonHelper.AssemblyDirectory + "\\Templates\\GmailLogin.xml");
                gmailTemplate = gmailTemplate.Replace("${0}", GetCell(index, 3)).Replace("${1}", GetCell(index, 4));

                var frmMain = new FrmMain() { OnlyBrowser = true, Scenario = gmailTemplate };

                frmMain.Show(this);

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CheckEthBalance(GetSelectedRow());

        }

        private void CheckEthBalance(int index)
        {

            try
            {
                if (EmptyCell(index, 8))
                {
                    MessageBox.Show("Cannot show balance for empty address! " + (index + 1));
                    return;
                }
                var gmailTemplate = File.ReadAllText(CommonHelper.AssemblyDirectory + "\\Templates\\EthBalance.xml");
                gmailTemplate = gmailTemplate.Replace("${0}", GetCell(index, 8));

                var frmMain = new FrmMain() { OnlyBrowser = true, Scenario = gmailTemplate };

                frmMain.Show(this);

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CreateTwitterFollower(GetSelectedRow());
        }

        private void CreateTwitterFollower(int index)
        {
            try
            {
                if (EmptyCell(index, 1) || EmptyCell(index, 2) || EmptyCell(index, 18))
                {
                    MessageBox.Show("Cannot create twitter followers for empty name, lastname, twitter name! " + (index + 1));
                    return;
                }
                var twitterTemplate = File.ReadAllText(CommonHelper.AssemblyDirectory + "\\Templates\\TwitterFollower.xml");
                twitterTemplate = twitterTemplate.Replace("${0}", GetCell(index, 1)).Replace("${1}", GetCell(index, 2)).Replace("${2}", GetCell(index, 18));

                var frmMain = new FrmMain() { OnlyBrowser = true, Scenario = twitterTemplate };

                frmMain.Show(this);

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void btnMew_Click(object sender, EventArgs e)
        {
            CreateMew(GetSelectedRow());

        }

        private void CreateMew(int index)
        {
            try
            {
                if (EmptyCell(index, 10))
                {
                    MessageBox.Show("Cannot create mew address for empty eth password! " + (index + 1));
                    return;
                }
                var mewTemplate = File.ReadAllText(CommonHelper.AssemblyDirectory + "\\Templates\\MewReg.xml");
                mewTemplate = mewTemplate.Replace("${0}", GetCell(index, 10));

                var frmMain = new FrmMain() { OnlyBrowser = true, Scenario = mewTemplate };

                frmMain.ShowDialog(this);

                try
                {
                    if (Helper.Variables.ContainsKey("prkey") && EmptyCell(index, 9))
                    {
                        SetCell(index, 9, Helper.Variables["prkey"]);
                    }

                    if (Helper.Variables.ContainsKey("pbkey") && EmptyCell(index, 8))
                    {
                        SetCell(index, 8, Helper.Variables["pbkey"]);
                    }
                }
                catch
                {
                }


            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void btnKucoin_Click(object sender, EventArgs e)
        {
            CreateKuCoin(GetSelectedRow());

        }

        private void CreateKuCoin(int index)
        {
            if (EmptyCell(index, 3) || EmptyCell(index, 4) || EmptyCell(index, 20))
            {
                MessageBox.Show("Cannot create kucoin address for empty email, email pwd or kucoin password! " + (index + 1));//email, pass, proxy
                return;
            }
            var mewTemplate = File.ReadAllText(CommonHelper.AssemblyDirectory + "\\Templates\\KucoinReg.xml");
            mewTemplate = mewTemplate.Replace("${0}", GetCell(index, 3)).Replace("${1}", GetCell(index, 20)).Replace("${2}", GetCell(index, 11) + ":" + GetCell(index, 12)).Replace("${3}", GetCell(index, 4));

            var frmMain = new FrmMain() { OnlyBrowser = true, Scenario = mewTemplate };

            frmMain.ShowDialog(this);


        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoginKuCoin(GetSelectedRow());
        }

        private void LoginKuCoin(int index)
        {
            if (EmptyCell(index, 19) || EmptyCell(index, 20))
            {
                MessageBox.Show("Cannot create kucoin address for empty kucoin user or kucoin password! " + (index + 1));//email, pass, proxy
                return;
            }
            var mewTemplate = File.ReadAllText(CommonHelper.AssemblyDirectory + "\\NewTemplates\\KucoinLogin.xml");
            mewTemplate = mewTemplate.Replace("${UserKucoinUser}", GetCell(index, 19)).Replace("${UserKucoinPass}", GetCell(index, 20)).Replace("${UserProxyIp}", GetCell(index, 11)).Replace("${UserProxyPort}", GetCell(index, 12)).Replace("${UserKucoinGSecret}", GetCell(index, 35));
            //
            var frmMain = new FrmMain() { OnlyBrowser = true, Scenario = mewTemplate };

            frmMain.ShowDialog(this);
        }

        private int ActiveRowIndex { get; set; }

        private void dgUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ActiveRowIndex = e.RowIndex;
        }

        private void gbUserOps_Enter(object sender, EventArgs e)
        {

        }



        private void CopyToClipboard()
        {
            //Copy to clipboard
            DataObject dataObj = dgUsers.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        private void PasteClipboard()
        {
            try
            {
                string s = Clipboard.GetText();
                string[] lines = s.Split('\n');

                int iRow = dgUsers.CurrentCell.RowIndex;
                int iCol = dgUsers.CurrentCell.ColumnIndex;
                DataGridViewCell oCell;
                if (iRow + lines.Length > dgUsers.Rows.Count - 1)
                {
                    bool bFlag = false;
                    foreach (string sEmpty in lines)
                    {
                        if (sEmpty == "")
                        {
                            bFlag = true;
                        }
                    }

                    int iNewRows = iRow + lines.Length - dgUsers.Rows.Count;
                    if (iNewRows > 0)
                    {
                        if (bFlag)
                            dgUsers.Rows.Add(iNewRows);
                        else
                            dgUsers.Rows.Add(iNewRows + 1);
                    }
                    else
                        dgUsers.Rows.Add(iNewRows + 1);
                }
                foreach (string line in lines)
                {
                    if (iRow < dgUsers.RowCount && line.Length > 0)
                    {
                        string[] sCells = line.Split('\t');
                        for (int i = 0; i < sCells.GetLength(0); ++i)
                        {
                            if (iCol + i < this.dgUsers.ColumnCount)
                            {
                                oCell = dgUsers[iCol + i, iRow];
                                oCell.Value = Convert.ChangeType(sCells[i].Replace("\r", ""), oCell.ValueType);
                            }
                            else
                            {
                                break;
                            }
                        }
                        iRow++;
                    }
                    else
                    {
                        break;
                    }
                }
                Clipboard.Clear();
            }
            catch (FormatException)
            {
                MessageBox.Show("The data you pasted is in the wrong format for the cell");
                return;
            }
        }

        private void dgUsers_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Modifiers == Keys.Control)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.C:
                            CopyToClipboard();
                            break;

                        case Keys.V:
                            PasteClipboard();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Copy/paste operation failed. " + ex.Message, "Copy/Paste", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void button5_Click_1(object sender, EventArgs e)
        {
            CreateWinUser(GetSelectedRow());

        }

        private void CreateWinUser(int index)
        {
            if (EmptyCell(index, 13) || EmptyCell(index, 14))
            {
                MessageBox.Show("Cannot create windows account for empty win user or win pwd ! " + (index + 1));//email, pass, proxy
                return;
            }
            RunasAdmin(CommonHelper.AssemblyDirectory + "\\CreateUser.bat",
                     string.Format("{0} {1}", GetCell(index, 13), GetCell(index, 14)));
        }

        private void RunasAdmin(string file, string args)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Normal,
                FileName = file,
                Arguments = args,
                Verb = "runas"
            };

            process.StartInfo = startInfo;
            process.Start();
        }

        private void button6_Click(object sender, EventArgs e)
        {

            HideWinUser(GetSelectedRow());
        }

        private void HideWinUser(int index)
        {
            if (EmptyCell(index, 13))
            {
                MessageBox.Show("Cannot hide windows account for empty win user !" + (index + 1));//email, pass, proxy
                return;
            }
            RunasAdmin(CommonHelper.AssemblyDirectory + "\\HideUser.bat",
                     string.Format("{0}", GetCell(index, 13)));
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenTelegram(GetSelectedRow());
        }

        private void OpenTelegram(int index)
        {
            if (EmptyCell(index, 13) || EmptyCell(index, 14))
            {
                MessageBox.Show("Cannot open telegram for empty win user or win pwd !" + (index + 1));//email, pass, proxy
                return;
            }

            if (GetCell(index, 13).StartsWith("m_"))
            {

                Rect rect;
                Helper.OpenTelegramMemu(GetCell(index, 13), GetCell(index, 14), "", out rect);
                return;
            }
            Helper.OpenTelegram(GetCell(index, 13), "", GetCell(index, 14));


        }

        private void byIgnoringFirstLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportUsers(true);
        }

        private void doNotIgnoreAnyLinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportUsers(false);
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveUsers(CommonHelper.UsersFile);
            IsDirty = false;
        }

        private void createStrongPasswordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var indexes = GetSelectedRows();
            foreach (var rowIndex in indexes)
            {
                CreateStrongPasswords(rowIndex);
            }
        }

        private void deleteSelectedOnesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var indexes = GetSelectedRows();

            foreach (var rowIndex in indexes)
            {
                try
                {
                    if (MessageBox.Show("Are you sure you want to delete row " + (rowIndex + 1).ToString() + "?", "Confirm Deletion", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        dgUsers.Rows.RemoveAt(rowIndex);

                    }
                }
                catch { }
            }
        }

        private void createGmailAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateGmail(GetSelectedRow());
        }

        private void checkGmailAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckGmail(GetSelectedRow());
        }

        private void createTwitterAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateTwitter(GetSelectedRow());
        }

        private void createTwitterFollowersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateTwitterFollower(GetSelectedRow());
        }

        private void createMewAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateMew(GetSelectedRow());
        }

        private void checkBalanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckEthBalance(GetSelectedRow());
        }

        private void createKucoinAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateKuCoin(GetSelectedRow());
        }

        private void checkKucoinAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoginKuCoin(GetSelectedRow());
        }

        private void createWindowsUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateWinUser(GetSelectedRow());
        }

        private void hideWindowsUserInLoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideWinUser(GetSelectedRow());
        }

        private void openTelegramForSelectedUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenTelegram(GetSelectedRow());
        }

        private void dgUsers_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void dgUsers_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int currentMouseOverRow = dgUsers.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    contextMenuStrip1.Show(dgUsers, new Point(e.X, e.Y));
                    contextMenuStrip1.Tag = currentMouseOverRow;

                }


            }
        }

        private void sallaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rowIndex = GetRowIndexForContextMenu();
            if (rowIndex == -1) return;
            dgUsers.Rows.RemoveAt(rowIndex);
        }

        private int GetRowIndexForContextMenu()
        {
            if (contextMenuStrip1.Tag == null) return -1;
            if (contextMenuStrip1.Tag.ToString() == "") return -1;
            var rowIndex = int.Parse(contextMenuStrip1.Tag.ToString());
            if (rowIndex == dgUsers.Rows.Count - 1) return -1;
            return rowIndex;
        }

        private void createStrongPasswordsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var rowIndex = GetRowIndexForContextMenu();
            if (rowIndex == -1) return;
            CreateStrongPasswords(rowIndex);
        }

        private void hideWindowsUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rowIndex = GetRowIndexForContextMenu();
            if (rowIndex == -1) return;
            HideWinUser(rowIndex);
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rowIndex = GetRowIndexForContextMenu();
            if (rowIndex == -1) return;
            CreateGmail(rowIndex);
        }

        private void checkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rowIndex = GetRowIndexForContextMenu();
            if (rowIndex == -1) return;
            CheckGmail(rowIndex);
        }

        private void createAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rowIndex = GetRowIndexForContextMenu();
            if (rowIndex == -1) return;
            CreateTwitter(rowIndex);
        }

        private void createFollowersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rowIndex = GetRowIndexForContextMenu();
            if (rowIndex == -1) return;
            CreateTwitterFollower(rowIndex);
        }

        private void createToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var rowIndex = GetRowIndexForContextMenu();
            if (rowIndex == -1) return;
            CreateMew(rowIndex);
        }

        private void checkToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var rowIndex = GetRowIndexForContextMenu();
            if (rowIndex == -1) return;
            CheckEthBalance(rowIndex);
        }

        private void createToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            var rowIndex = GetRowIndexForContextMenu();
            if (rowIndex == -1) return;
            CreateKuCoin(rowIndex);
        }

        private void checkToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            var rowIndex = GetRowIndexForContextMenu();
            if (rowIndex == -1) return;
            LoginKuCoin(rowIndex);
        }

        private void createWindowsUserToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var rowIndex = GetRowIndexForContextMenu();
            if (rowIndex == -1) return;
            CreateWinUser(rowIndex);
        }

        private void openTelegramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rowIndex = GetRowIndexForContextMenu();
            if (rowIndex == -1) return;
            OpenTelegram(rowIndex);
        }

        private void checkFBAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoginFBAccount(GetSelectedRow());
        }

        private void LoginFBAccount(int index)
        {
            try
            {
                if (EmptyCell(index, 21) || EmptyCell(index, 22))
                {
                    MessageBox.Show("Cannot check fb for empty fb user and fb password! " + (index + 1));
                    return;
                }
                var fbTemplate = File.ReadAllText(CommonHelper.AssemblyDirectory + "\\Templates\\FBLogin.xml");
                fbTemplate = fbTemplate.Replace("${0}", GetCell(index, 21)).Replace("${1}", GetCell(index, 22));

                var frmMain = new FrmMain() { OnlyBrowser = true, Scenario = fbTemplate };

                frmMain.Show(this);

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void createFBAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateFBAccount(GetSelectedRow());
        }

        private void CreateFBAccount(int index)
        {
            try
            {
                if (EmptyCell(index, 1) || EmptyCell(index, 2) || EmptyCell(index, 21) || EmptyCell(index, 22))
                {
                    MessageBox.Show("Cannot create facebook address for empty name, lastname, fb user and fb password! " + (index + 1));
                    return;
                }
                var fbTemplate = File.ReadAllText(CommonHelper.AssemblyDirectory + "\\Templates\\FBReg.xml");
                fbTemplate = fbTemplate.Replace("${0}", GetCell(index, 1)).Replace("${1}", GetCell(index, 2)).
                    Replace("${2}", GetCell(index, 21)).Replace("${3}", GetCell(index, 22));

                var frmMain = new FrmMain() { OnlyBrowser = true, Scenario = fbTemplate };

                frmMain.Show(this);

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void createToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            var rowIndex = GetRowIndexForContextMenu();
            if (rowIndex == -1) return;
            CreateFBAccount(rowIndex);
        }

        private void checkToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            var rowIndex = GetRowIndexForContextMenu();
            if (rowIndex == -1) return;
            LoginFBAccount(rowIndex);
        }

        private void checkTwitterAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {

            CheckTwitter(GetSelectedRow());
        }

        private void CheckTwitter(int index)
        {
            try
            {
                if (EmptyCell(index, 16) || EmptyCell(index, 17))
                {
                    MessageBox.Show("Cannot check twitter address for empty twitter user or twitter password! " + (index + 1));
                    return;
                }
                var twitterTemplate = File.ReadAllText(CommonHelper.AssemblyDirectory + "\\Templates\\TwitterLogin.xml");
                twitterTemplate = twitterTemplate.Replace("${0}", GetCell(index, 16)).Replace("${1}", GetCell(index, 17));

                var frmMain = new FrmMain() { OnlyBrowser = true, Scenario = twitterTemplate };

                frmMain.Show(this);

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void checkToolStripMenuItem4_Click(object sender, EventArgs e)
        {

            var rowIndex = GetRowIndexForContextMenu();
            if (rowIndex == -1) return;
            CheckTwitter(rowIndex);
        }

        private void createAPIKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateTwApiKeys(GetSelectedRow());
        }

        private void CreateTwApiKeys(int index)
        {
            try
            {
                if (EmptyCell(index, 16) || EmptyCell(index, 17) || EmptyCell(index, 18))
                {
                    MessageBox.Show("Cannot check twitter address for empty twitter user or twitter password! " + (index + 1));
                    return;
                }
                var twLoginTemplate = File.ReadAllText(CommonHelper.AssemblyDirectory + "\\NewTemplates\\TwitterLogin.xml");
                twLoginTemplate = twLoginTemplate.Replace("${UserTwUserName}", GetCell(index, 16)).Replace("${UserTwPwd}", GetCell(index, 17));

                var twApiTemplate = File.ReadAllText(CommonHelper.AssemblyDirectory + "\\NewTemplates\\TwitterCreateApi.xml");
                twApiTemplate = twApiTemplate.Replace("${UserTwName}", GetCell(index, 18));

                var doc = new XmlDocument();
                try
                {
                    doc.LoadXml(twLoginTemplate);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid Twitter login xml! \r\n" + twLoginTemplate);
                    return;
                }

                var doc2 = new XmlDocument();
                try
                {
                    doc2.LoadXml(twApiTemplate);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid TwitterCreateApi xml! \r\n" + twApiTemplate);
                    return;
                }

                var frmMain = new FrmMain() { OnlyBrowser = true, Scenario = doc.DocumentElement.InnerXml + "\r\n\r\n" + doc2.DocumentElement.InnerXml };

                frmMain.ShowDialog(this);

                try
                {
                    if (Helper.Variables.ContainsKey("consumerkey"))
                    {
                        SetCell(index, 29, Helper.Variables["consumerkey"]);
                    }

                    if (Helper.Variables.ContainsKey("consumersecret"))
                    {
                        SetCell(index, 30, Helper.Variables["consumersecret"]);
                    }
                    if (Helper.Variables.ContainsKey("accesstoken"))
                    {
                        SetCell(index, 31, Helper.Variables["accesstoken"]);
                    }
                    if (Helper.Variables.ContainsKey("accesstokensecret"))
                    {
                        SetCell(index, 32, Helper.Variables["accesstokensecret"]);
                    }
                }
                catch
                {
                }


            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void createAPIKeysToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var rowIndex = GetRowIndexForContextMenu();
            if (rowIndex == -1) return;
            CreateTwApiKeys(rowIndex);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            UpdateTwApiKeys(GetSelectedRow());

        }

        private void UpdateTwApiKeys(int index)
        {
            try
            {
                if (EmptyCell(index, 16) || EmptyCell(index, 17) || EmptyCell(index, 18))
                {
                    MessageBox.Show("Cannot check twitter address for empty twitter user or twitter password! " + (index + 1));
                    return;
                }
                var twLoginTemplate = File.ReadAllText(CommonHelper.AssemblyDirectory + "\\Templates\\TwitterLogin.xml");
                twLoginTemplate = twLoginTemplate.Replace("${0}", GetCell(index, 16)).Replace("${1}", GetCell(index, 17));

                var twApiTemplate = File.ReadAllText(CommonHelper.AssemblyDirectory + "\\Templates\\TwitterUpdateApi.xml");
                twApiTemplate = twApiTemplate.Replace("${0}", GetCell(index, 18));


                var doc = new XmlDocument();
                try
                {
                    doc.LoadXml(twLoginTemplate);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid Twitter login xml! \r\n" + twLoginTemplate);
                    return;
                }

                var doc2 = new XmlDocument();
                try
                {
                    doc2.LoadXml(twApiTemplate);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid TwitterCreateApi xml! \r\n" + twApiTemplate);
                    return;
                }

                var frmMain = new FrmMain() { OnlyBrowser = true, Scenario = doc.DocumentElement.InnerXml + "\r\n\r\n" + doc2.DocumentElement.InnerXml };

                frmMain.ShowDialog(this);

                try
                {
                    if (Helper.Variables.ContainsKey("consumerkey"))
                    {
                        SetCell(index, 29, Helper.Variables["consumerkey"]);
                    }

                    if (Helper.Variables.ContainsKey("consumersecret"))
                    {
                        SetCell(index, 30, Helper.Variables["consumersecret"]);
                    }
                    if (Helper.Variables.ContainsKey("accesstoken"))
                    {
                        SetCell(index, 31, Helper.Variables["accesstoken"]);
                    }
                    if (Helper.Variables.ContainsKey("accesstokensecret"))
                    {
                        SetCell(index, 32, Helper.Variables["accesstokensecret"]);
                    }
                }
                catch
                {
                }


            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            var rowIndex = GetRowIndexForContextMenu();
            if (rowIndex == -1) return;
            UpdateTwApiKeys(rowIndex);

        }

        private void closeAllInstancesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CommonHelper.CloseProcessAllInstances("Telegram");
        }

        private void closeAllInstancesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommonHelper.CloseProcessAllInstances("Telegram");

        }

        private void createWinUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateWinUser(GetSelectedRow());

        }

        private void hideWinUserFromLogonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideWinUser(GetSelectedRow());
        }

        private void createWinUserToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var rowIndex = GetRowIndexForContextMenu();
            if (rowIndex == -1) return;

            CreateWinUser(rowIndex);
        }

        private void hideWinUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rowIndex = GetRowIndexForContextMenu();
            if (rowIndex == -1) return;
            HideWinUser(rowIndex);

        }

        private void createToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            CreateNeo(GetSelectedRow());
        }
        private void CreateNeo(int index)
        {
            try
            {
                var neoTemplate = File.ReadAllText(CommonHelper.AssemblyDirectory + "\\Templates\\NeoReg.xml");
                neoTemplate = neoTemplate.Replace("${0}", GetCell(index, 6));

                var frmMain = new FrmMain() { OnlyBrowser = true, Scenario = neoTemplate };

                frmMain.ShowDialog(this);

                try
                {
                    if (Helper.Variables.ContainsKey("prkey") && EmptyCell(index, 34))
                    {
                        SetCell(index, 34, Helper.Variables["prkey"]);
                    }

                    if (Helper.Variables.ContainsKey("pbkey") && EmptyCell(index, 33))
                    {
                        SetCell(index, 33, Helper.Variables["pbkey"]);
                    }
                }
                catch
                {
                }


            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void checkToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            CheckNeoBalance(GetSelectedRow());
        }
        private void CheckNeoBalance(int index)
        {

            try
            {
                if (EmptyCell(index, 33))
                {
                    MessageBox.Show("Cannot show balance for empty NEO address! " + (index + 1));
                    return;
                }
                var template = File.ReadAllText(CommonHelper.AssemblyDirectory + "\\Templates\\NeoCheck.xml");
                template = template.Replace("${0}", GetCell(index, 33));

                var frmMain = new FrmMain() { OnlyBrowser = true, Scenario = template };

                frmMain.Show(this);

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void createToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            var rowIndex = GetRowIndexForContextMenu();
            if (rowIndex == -1) return;
            CreateNeo(rowIndex);
        }

        private void checkToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            var rowIndex = GetRowIndexForContextMenu();
            if (rowIndex == -1) return;
            CheckNeoBalance(rowIndex);
        }

        private void followEachOtherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TwitterFollowEachOther(GetSelectedRow());

        }

        private void TwitterFollowEachOther(int index)
        {
            try
            {
                if (EmptyCell(index, 18))
                {
                    MessageBox.Show("Cannot create twitter followers for empty twitter name! " + (index + 1));
                    return;
                }
                var twitterTemplate = File.ReadAllText(CommonHelper.AssemblyDirectory + "\\NewTemplates\\TwitterFollowEachOther.xml");
                twitterTemplate = twitterTemplate.Replace("${0}", GetCell(index, 18));

                var frmMain = new FrmMain() { OnlyBrowser = true, Scenario = twitterTemplate };

                frmMain.Show(this);

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void openForUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFirefox(GetSelectedRow());
        }

        private void OpenFirefox(int index)
        {

            if (EmptyCell(index, 13) || EmptyCell(index, 14))
            {
                MessageBox.Show("Cannot open firefox for empty win user or win pwd !" + (index + 1));//email, pass, proxy
                return;
            }

            Helper.OpenFirefox(GetCell(index, 13), GetCell(index, 14));
        }

        private void firefoxToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var rowIndex = GetRowIndexForContextMenu();
            if (rowIndex == -1) return;
            OpenFirefox(rowIndex);
        }

        private void chromeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rowIndex = GetRowIndexForContextMenu();
            if (rowIndex == -1) return;
            OpenChrome(rowIndex);
       
        }

        private void OpenChrome(int index)
        {
            if (EmptyCell(index, 13) || EmptyCell(index, 14))
            {
                MessageBox.Show("Cannot open chrome for empty win user or win pwd !" + (index + 1));//email, pass, proxy
                return;
            }

            Helper.OpenChrome(GetCell(index, 13), GetCell(index, 14));
        }

        private void openForUserToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenChrome(GetSelectedRow());

        }
    }
}
