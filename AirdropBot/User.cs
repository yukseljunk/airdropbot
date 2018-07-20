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

        public string TwConsumerKey { get; set; }
        public string TwConsumerSecret { get; set; }
        public string TwAccessToken { get; set; }
        public string TwAccessTokenSecret { get; set; }
        

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

        public string NeoAddress { get; set; }
        public string NeoPrivateKey { get; set; }
        
        public string ProxyIp { get; set; }
        public string ProxyPort { get; set; }

        public string StrongPassword { get; set; }
        public string StrongPwdWithSign { get; set; }

        public string KucoinUser { get; set; }
        public string KucoinPass { get; set; }
        public string KucoinGSecret { get; set; }

        public string GSecret1 { get; set; }
        public string GSecret2 { get; set; }
        public string GSecret3 { get; set; }
        public string GSecret4 { get; set; }
        public string GSecret5 { get; set; }

        public string CustomField1 { get; set; }
        public string CustomField2 { get; set; }
        public string CustomField3 { get; set; }
        public string CustomField4 { get; set; }
        public string CustomField5 { get; set; }
        public string CustomField6 { get; set; }
        public string CustomField7 { get; set; }
        public string CustomField8 { get; set; }
        public string CustomField9 { get; set; }


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
                    BtcTalkProfileLink = fields[28],
                };

                if(fields.Length>32)
                {
                    user.TwConsumerKey = fields[29];
                    user.TwConsumerSecret = fields[30];
                    user.TwAccessToken = fields[31];
                    user.TwAccessTokenSecret = fields[32];
                }
                if(fields.Length>34)
                {
                    user.NeoAddress = fields[33];
                    user.NeoPrivateKey = fields[34];
                }
                if(fields.Length>49)
                {
                    user.KucoinGSecret = fields[35];
                    user.GSecret1 = fields[36];
                    user.GSecret2 = fields[37];
                    user.GSecret3 = fields[38];
                    user.GSecret4 = fields[39];
                    user.GSecret5 = fields[40];
                    user.CustomField1 = fields[41];
                    user.CustomField2 = fields[42];
                    user.CustomField3 = fields[43];
                    user.CustomField4 = fields[44];
                    user.CustomField5 = fields[45];
                    user.CustomField6 = fields[46];
                    user.CustomField7 = fields[47];
                    user.CustomField8 = fields[48];
                    user.CustomField9 = fields[49];
                }


                users.Add(user.Id, user);
            }
            return users;
        }
    }
}
