using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;

namespace WashTradeMachine
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private Dictionary<int, User> Users { get; set; }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadUsers();
        }
        private void LoadUsers()
        {
            var usersFile = @"C:\code\Airdrop\AirdropBot\bin\x64\Debug\users.csv";
            if (File.Exists(usersFile))
            {
                var userFactory = new UserFactory();
                Users = userFactory.GetUsers(usersFile, false);
                FillUsers();
            }
        }

        private void FillUsers()
        {
            lstUsers.Items.Clear();
            foreach (var user in Users)
            {
                lstUsers.Items.Add(new string(' ', 4 - user.Value.Id.ToString().Length) + user.Value.Id + ". " + user.Value.Name + " " + user.Value.LastName);
            }
        }

        private void btnCalculatePermutations_Click(object sender, EventArgs e)
        {
            _stopAsked = false;
            var perm2run = GetRunningList((int)numRepeat.Value);
            foreach (var index in perm2run)
            {
                if (_stopAsked) break;
                lstUsers.SelectedIndex = index-1;
                var botfile = string.Format(@"C:\temp\watch\bot{0}.txt", index);
                var readyFile = botfile + ".ready";
                File.WriteAllText(botfile, txtPrice.Text + ":" + txtAmount.Text);
                var returnedFile = WaitForFile(readyFile, 30);
                if (!returnedFile)
                {
                    MessageBox.Show("Timeout waiting for file " + readyFile);
                    break;
                }
                var readyFileContent = File.ReadAllText(readyFile);
                if (readyFileContent != "")
                {
                    MessageBox.Show("ERROR on " + index + ":" + readyFileContent);
                    break;
                }
                File.Delete(readyFile);
            }
        }

        private bool WaitForFile(string file, int timeout)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                if (_stopAsked) break;
                Application.DoEvents();
                if (File.Exists(file)) return true;
                if (sw.ElapsedMilliseconds >= timeout * 1000)
                {
                    break;
                }
            }
            return false;
        }

        private static void Wait(int secs)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                if (_stopAsked) break;
                Application.DoEvents();
                if (sw.ElapsedMilliseconds >= secs * 1000)
                {
                    break;
                }
            }
        }

        private List<int> GetRunningList(int times)
        {
            var selectedUsers = GetSelectedUserIndices();
            var allperm = new List<List<int>>();

            Permutation.GetPer(selectedUsers.ToArray(), allperm);

            var perm2run = new List<int>();
            var lastNumber = -1;
            for (int i = 0; i < times; i++)
            {
                //select from allperms which are not starting with lastNumber
                int lNumber = lastNumber;
                var subperm = allperm.Where(ap => ap.First() != lNumber).ToList();

                var randIndex = CommonHelper.RandomInteger(0, subperm.Count);
                perm2run.AddRange(subperm[randIndex]);

                lastNumber = perm2run.Last();
            }
            return perm2run;
        }


        private List<int> GetSelectedUserIndices()
        {
            var selectedUsers = new List<int>();
            foreach (var x in lstUsers.CheckedIndices)
            {
                selectedUsers.Add(int.Parse(x.ToString()) + 1);
            }
            return selectedUsers;
        }

        private static bool _stopAsked = false;
        private void btnStop_Click(object sender, EventArgs e)
        {
            _stopAsked = true;
        }
    }
}
