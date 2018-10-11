using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace AirdropBot
{
    public class Google2FaCommand : ICommand
    {

        #region Implementation of ICommand

        public string Command(XmlNode node)
        {
            var param = node.Attributes["param"];
            var secret = node.Attributes["secret"];
            if (param == null || secret == null) return "param/secret not defined";
            if (string.IsNullOrEmpty(param.Value) || string.IsNullOrEmpty(secret.Value)) return "param/secret empty";

            var secretPure = Helper.ReplaceTokens(secret.Value);
            var g2fa = new Google2Fa();
            var result = g2fa.GetCode(secretPure);
            if (!Helper.Variables.ContainsKey(param.Value))
            {
                Helper.Variables.Add(param.Value, result);
            }
            Helper.Variables[param.Value] = result;

            return "";

        }

        public string Tag
        {
            get { return "google2fa"; }
        }
        public string Html
        {
            get
            {
                return "<google2fa param=\"\" secret=\"\"/>";

            }
        }

        #endregion


    }
}