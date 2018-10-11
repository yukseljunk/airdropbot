using System.Xml;
using CefSharp;

namespace AirdropBot
{
    public class SetAttribute : AbstractBrowserManipulator, ICommand
    {

        #region Implementation of ICommand

        public string Command(XmlNode node)
        {
            var value = node.Attributes["value"];
            if (value == null) return "No value is defined!";
            var attr = node.Attributes["attr"];
            if (attr == null) return "No attr is defined!";

            return SetCElementAttr(node, attr.Value, Helper.ReplaceTokens(value.Value));
        }

        private string SetCElementAttr(XmlNode node, string attr, string newValue)
        {
            var xpath = node.Attributes["xpath"];
            if (xpath != null && xpath.Value != "")
            {
                string scr = string.Format("{3} function x(){{ if(getElementByXpath(\"{0}\")==null)  return 'Cannot find element!'; getElementByXpath(\"{0}\").setAttribute(\"{1}\", \"{2}\"); }} x(); ", Helper.ReplaceTokens(xpath.Value), attr, newValue, FindXPathScript);
                var resp = "";
                Browser.EvaluateScriptAsync(scr).ContinueWith(x =>
                {
                    var response = x.Result;

                    if (response.Success && response.Result != null)
                    {
                        resp = response.Result.ToString();
                        //startDate is the value of a HTML element.
                    }
                }).Wait();

                return resp;
            }
            return "Xpath not defined!";
        }


        public string Tag
        {
            get { return "setattr"; }
        }
        public string Html
        {
            get
            {
                return "<setattr attr=\"\"  value=\"\" xpath=\"\"/>";

            }
        }

        #endregion

       
    }
}