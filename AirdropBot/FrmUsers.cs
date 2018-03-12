using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

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
                             "btctalk profile"
                           };
            foreach (var col in cols)
            {
                dgUsers.Columns.Add(col.Replace(" ", "_"), col);

            }
            dgUsers.Columns[0].ReadOnly = true;
            if (File.Exists(Helper.UsersFile))
            {
                var userFactory = new UserFactory();
                Users = userFactory.GetUsers(Helper.UsersFile, false);
                FillUsers();
            }
            IsDirty = false;
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
            DialogResult result = openCsvFile.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                var userFactory = new UserFactory();
                Users = userFactory.GetUsers(openCsvFile.FileName, true);
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
                                 u.BtcTalkPwd, u.BtcTalkProfileLink);
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }


        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SaveUsers(Helper.UsersFile);
            IsDirty = false;
        }

        private void SaveUsers(string file)
        {
            File.WriteAllText(file, "");
            foreach (DataGridViewRow row in dgUsers.Rows)
            {
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

        private bool _isDirty;
        public bool IsDirty { get { return _isDirty; } set { _isDirty = value; } }

        private void dgUsers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            IsDirty = true;
        }

        private void FrmUsers_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && IsDirty)
            {
                if (MessageBox.Show("Do you want to save your changes to users?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SaveUsers(Helper.UsersFile);
                }
            }
        }

        private void dgUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgUsers_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            gbUserOps.Enabled = true;
        }

        private void btnDelUser_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dgUsers.SelectedRows)
            {
                try
                {
                    dgUsers.Rows.RemoveAt(item.Index);
                }
                catch { }
            }
        }

        private void btnCreatePwd_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dgUsers.SelectedRows)
            {
                CreateStrongPasswords(item.Index);
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
            foreach (DataGridViewRow item in dgUsers.SelectedRows)
            {
                CreateGmail(item.Index);
                break;
            }

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
                var gmailTemplate = File.ReadAllText(Helper.AssemblyDirectory + "\\Templates\\GmailReg.xml");
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
            foreach (DataGridViewRow item in dgUsers.SelectedRows)
            {
                CreateTwitter(item.Index);
                break;
            }
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
                var twitterTemplate = File.ReadAllText(Helper.AssemblyDirectory + "\\Templates\\TwitterReg.xml");
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
            foreach (DataGridViewRow item in dgUsers.SelectedRows)
            {
                CheckGmail(item.Index);
                break;
            }
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
                var gmailTemplate = File.ReadAllText(Helper.AssemblyDirectory + "\\Templates\\GmailCheck.xml");
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
            foreach (DataGridViewRow item in dgUsers.SelectedRows)
            {
                CheckEthBalance(item.Index);
                break;
            }
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
                var gmailTemplate = File.ReadAllText(Helper.AssemblyDirectory + "\\Templates\\EthBalance.xml");
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
            foreach (DataGridViewRow item in dgUsers.SelectedRows)
            {
                CreateTwitterFollower(item.Index);
                break;
            }
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
                var twitterTemplate = File.ReadAllText(Helper.AssemblyDirectory + "\\Templates\\TwitterFollower.xml");
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
            foreach (DataGridViewRow item in dgUsers.SelectedRows)
            {
                CreateMew(item.Index);
                break;
            }
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
                var mewTemplate = File.ReadAllText(Helper.AssemblyDirectory + "\\Templates\\MewReg.xml");
                mewTemplate = mewTemplate.Replace("${0}", GetCell(index, 10));

                var frmMain = new FrmMain() { OnlyBrowser = true, Scenario = mewTemplate };

                frmMain.ShowDialog(this);

                try
                {
                    if (frmMain.Variables.ContainsKey("prkey") && EmptyCell(index, 9))
                    {
                        SetCell(index, 9, frmMain.Variables["prkey"]);
                    }

                    if (frmMain.Variables.ContainsKey("pbkey") && EmptyCell(index, 8))
                    {
                        SetCell(index, 8, frmMain.Variables["pbkey"]);
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
    }
}
