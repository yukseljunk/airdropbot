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
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openCsvFile.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                Users = new Dictionary<string, User>();
                ParseCsvFile(openCsvFile.FileName);
            }

        }


        private void ParseCsvFile(string fileName)
        {
            var contents = File.ReadAllLines(fileName);
            foreach (var content in contents.Skip(1))
            {
                var fields = content.Split(new [] { ';', ',' }, StringSplitOptions.None);
                if (fields.Length < 32) continue;
                var user = new User()
                {
                    Name = fields[1],
                    LastName = fields[2],
                    Mail = fields[3],
                    MailPwd = fields[4],
                    Phone = fields[5],
                    StrongPassword = fields[6],
                    StrongPwdWithSign = fields[7],
                    EthAddress = fields[8],
                    EthPrivateKey = fields[9],
                    EthPass = fields[10],
                    ProxyIp = fields[11],
                    ProxyPort = fields[12],
                    WinUser = fields[13],
                    WinPwd = fields[14],
                    TgUser = fields[15],
                    TwUserName = fields[16],
                    TwPwd = fields[17],
                    TwName = fields[18],
                    KucoinUser = fields[19],
                    KucoinPass = fields[20],
                    FBUser = fields[21],
                    FBPwd = fields[22],
                    FBProfile = fields[23],
                    ReddUser = fields[24],
                    ReddPwd = fields[25],
                    BtcTalkUser = fields[26],
                    BtcTalkPwd = fields[27],
                    BtcTalkProfileLink = fields[28]

                };

                Users.Add(user.Mail, user);
            }
            FillUsers();
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
            
        }

    }
}
