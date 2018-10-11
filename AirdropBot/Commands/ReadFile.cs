using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace AirdropBot
{
    public class ReadFile : ICommand
    {

        #region Implementation of ICommand

        public string Command(XmlNode node)
        {
            var path = node.Attributes["path"];
            if (path == null) return "path not defined";
            if (string.IsNullOrEmpty(path.Value)) return "path cannot be empty!";


            var variable = node.Attributes["variable"];
            if (variable == null) return "variable not defined";
            if (string.IsNullOrEmpty(variable.Value)) return "variable cannot be empty!";

            try
            {
                var contents = File.ReadAllText(Helper.ReplaceTokens(path.Value));
                if (!Helper.Variables.ContainsKey(variable.Value))
                {
                    Helper.Variables.Add(variable.Value, contents);
                }
                Helper.Variables[variable.Value] = contents;

            }
            catch (Exception exc)
            {
                return exc.Message;
            }

            return "";

        }

        public string Tag
        {
            get { return "readfile"; }
        }
        public string Html
        {
            get
            {
                return "<readfile path=\"c:\\temp\\out.txt\" variable=\"filecontent\" />";

            }
        }

        #endregion


    }
}