using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Common
{
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