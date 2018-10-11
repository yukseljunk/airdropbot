using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace AirdropBot
{
    public class WriteToFileCommand : ICommand
    {
     
        #region Implementation of ICommand

        public string Command(XmlNode node)
        {
            var path = node.Attributes["path"];
            if (path == null) return "path not defined";
            if (string.IsNullOrEmpty(path.Value)) return "path cannot be empty!";

            var txt = Environment.NewLine;
            var text = node.Attributes["text"];
            if (text != null)
            {
                txt = Helper.ReplaceTokens(text.Value) + Environment.NewLine;
            }
            var appendtofile = true;
            var append = node.Attributes["append"];
            if (append != null && Helper.ReplaceTokens(append.Value).ToLower().StartsWith("f"))
            {
                appendtofile = false;
            }

            try
            {
                if (appendtofile)
                {
                    File.AppendAllText(Helper.ReplaceTokens(path.Value), txt);
                }
                else
                {
                    File.WriteAllText(Helper.ReplaceTokens(path.Value), txt);
                }
            }
            catch (Exception exc)
            {
                return exc.Message;
            }

            return "";

        }

        public string Tag
        {
            get { return "writetofile"; }
        }
        public string Html
        {
            get
            {
                return "<writetofile path=\"c:\\temp\\out.txt\" text=\"\" append=\"true\"/>";

            }
        }

        #endregion

       
    }
}