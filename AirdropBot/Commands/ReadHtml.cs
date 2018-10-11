using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml;

namespace AirdropBot
{
    public class ReadHtml : ICommand
    {

        #region Implementation of ICommand

        public string Command(XmlNode node)
        {
            var url = node.Attributes["url"];
            if (url == null) return "url not defined";
            if (string.IsNullOrEmpty(url.Value)) return "url cannot be empty!";
            var html = "";
            try
            {
                using (WebClient client = new WebClient())
                {
                    html = client.DownloadString(url.Value);
                }
            }
            catch(Exception exc)
            {
                return exc.ToString();
            }

            var variable = node.Attributes["variable"];
            if (variable == null) return "variable not defined";
            if (string.IsNullOrEmpty(variable.Value)) return "variable cannot be empty!";

            try
            {
                if (!Helper.Variables.ContainsKey(variable.Value))
                {
                    Helper.Variables.Add(variable.Value, html);
                }
                Helper.Variables[variable.Value] = html;

            }
            catch (Exception exc)
            {
                return exc.Message;
            }

            return "";

        }

        public string Tag
        {
            get { return "readhtml"; }
        }
        public string Html
        {
            get
            {
                return "<readhtml url=\"https://www.catex.io/\" variable=\"htmlcontent\" />";

            }
        }

        #endregion


    }
}