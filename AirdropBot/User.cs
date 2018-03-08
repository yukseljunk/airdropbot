using System.Collections.Generic;
using System.Reflection;

namespace AirdropBot
{
    public class User
    {
        public string Name { get; set; }
        public string LastName { get; set; }

        public string Mail { get; set; }
        public string MailPwd { get; set; }

        public string FBUser { get; set; }
        public string FBPwd { get; set; }
        public string FBProfile{ get; set; }

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

        public void FillToDictionary(Dictionary<string,string> dict)
        {
            foreach(PropertyInfo prop in this.GetType().GetProperties())
            {
                var key = "User" + prop.Name;
                if(!dict.ContainsKey(key))
                {
                    dict.Add(key, "");
                }
                var propValue = prop.GetValue(this, null);
                dict[key] = propValue==null? "":propValue.ToString();
            }
        }
    }
}
