using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AirdropBot
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        public string Mail { get; set; }
        public string MailPwd { get; set; }

        public string FBUser { get; set; }
        public string FBPwd { get; set; }
        public string FBProfile { get; set; }

        public string WinUser { get; set; }
        public string WinPwd { get; set; }

        public string TwName { get; set; }
        public string TwUserName { get; set; }
        public string TwPwd { get; set; }

        public string BtcTalkUser { get; set; }
        public string BtcTalkPwd { get; set; }
        public string BtcTalkProfileLink { get; set; }

        public string TgUser { get; set; }
        public string Phone { get; set; }

        public string ReddUser { get; set; }
        public string ReddPwd { get; set; }

        public string EthAddress { get; set; }
        public string EthPrivateKey { get; set; }
        public string EthPass { get; set; }

        public string ProxyIp { get; set; }
        public string ProxyPort { get; set; }

        public string StrongPassword { get; set; }
        public string StrongPwdWithSign { get; set; }

        public string KucoinUser { get; set; }
        public string KucoinPass { get; set; }

        public void FillToDictionary(Dictionary<string, string> dict)
        {
            foreach (PropertyInfo prop in this.GetType().GetProperties())
            {
                var key = "User" + prop.Name;
                if (!dict.ContainsKey(key))
                {
                    dict.Add(key, "");
                }
                var propValue = prop.GetValue(this, null);
                dict[key] = propValue == null ? "" : propValue.ToString();
            }
        }
    }

    public class UserFactory
    {
        /// <summary>
        /// read users from file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Dictionary<int, User> GetUsers(string fileName, bool firstLineIsHeader)
        {
            var users = new Dictionary<int, User>();
            var contents = File.ReadAllLines(fileName);
            foreach (var content in contents.Skip(firstLineIsHeader ? 1 : 0))
            {
                var fields = content.Split(new[] { ';', ',' }, StringSplitOptions.None);
                if (fields.Length < 28) continue;
                var user = new User()
                {
                    Id = int.Parse(fields[0]),
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

                users.Add(user.Id, user);
            }
            return users;
        }
    }
}
