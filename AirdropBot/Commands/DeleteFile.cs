using System;
using System.IO;
using System.Xml;

namespace AirdropBot
{
    public class DeleteFile : ICommand
    {

        #region Implementation of ICommand

        public string Command(XmlNode node)
        {
            var path = node.Attributes["path"];
            if (path == null) return "path not defined";
            if (string.IsNullOrEmpty(path.Value)) return "path cannot be empty!";

            try
            {
                File.Delete(Helper.ReplaceTokens(path.Value));
                
            }
            catch (Exception exc)
            {
                return exc.Message;
            }

            return "";

        }

        public string Tag
        {
            get { return "deletefile"; }
        }
        public string Html
        {
            get
            {
                return "<deletefile path=\"c:\\temp\\out.txt\" />";

            }
        }

        #endregion


    }
}