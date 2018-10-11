using System;
using System.Xml;
using CefSharp;

namespace AirdropBot
{
    public class ScrollElement : AbstractBrowserManipulator, ICommand
    {

        #region Implementation of ICommand

        public string Command(XmlNode node)
        {
            var height = 100;
            var secs = node.Attributes["height"];
            if (secs != null)
            {
                int.TryParse(node.Attributes["height"].Value, out height);
            }

            Browser.ExecuteScriptAsync(String.Format("window.scrollBy({0}, {1});", 0, height));
            return "";

        }

        public string Tag
        {
            get { return "scroll"; }
        }
        public string Html
        {
            get
            {
                return "<scroll height=\"100\"/>";

            }
        }

        #endregion

       
    }
}