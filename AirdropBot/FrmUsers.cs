using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AirdropBot
{
    public partial class FrmUsers : Form
    {
        private Dictionary<string, User> Users = new Dictionary<string, User>();

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
            if (e.ColumnIndex == 3 && e.FormattedValue.ToString() == "")
            {
                dgUsers.Rows[e.RowIndex].ErrorText =
                    "Cannot have empty email!";
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

    }
}
