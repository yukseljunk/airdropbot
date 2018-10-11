using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AirdropBot
{
    class Gcaptcha
    {
        public string TwoCaptchaAPIKey { get { return ConfigurationManager.AppSettings["2CaptchaAPIKey"]; } }
        private const string TwoCaptchaUrl = "http://2captcha.com/in.php";
        private const string TwoCaptchaResultUrl = "http://2captcha.com/res.php";

        public string SendRecaptchav2Request(string sitekey, string url, bool invisible = false)
        {
            //POST
            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                var request = (HttpWebRequest)WebRequest.Create(TwoCaptchaUrl);

                var postData = string.Format("key={0}&method=userrecaptcha&googlekey={1}&pageurl={2}&invisible={3}", TwoCaptchaAPIKey, sitekey, url, invisible ? 1 : 0);
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                //  GET
                if (responseString.Contains("OK|"))
                {
                    return responseString.Substring(3);
                }
                return "Error";
            }
            catch (Exception e)
            {
                string tt = e.Message;
                return tt;
            }

        }


        public string GetToken(string id)
        {
            var url = string.Format("{2}?key={0}&action=get&id={1}", TwoCaptchaAPIKey, id, TwoCaptchaResultUrl);
            return Get(url);
        }

        public string Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
