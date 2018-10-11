using System.Xml;
using CefSharp;

namespace AirdropBot
{
    public class DeleteElement : AbstractBrowserManipulator, ICommand
    {

        #region Implementation of ICommand

        public string Command(XmlNode node)
        {
            var el = DeleteEl(node);

            return el;

        }

        private string DeleteEl(XmlNode node)
        {

            var xpath = node.Attributes["xpath"];
            if (xpath != null && xpath.Value != "")
            {
                string scr = string.Format("{1} function x(){{ if(getElementByXpath(\"{0}\")==null)  return 'UNDEF'; getElementByXpath(\"{0}\").outerHTML='';}} x(); ", Helper.ReplaceTokens(xpath.Value), FindXPathScript);
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

                if (resp == "UNDEF")
                {
                    return "Element cannot be found!";
                }
                return resp;
            }
            return "XPath not specified!";
        }
        public string Tag
        {
            get { return "delete"; }
        }
        public string Html
        {
            get
            {
                return "<delete xpath=\"\"/>";

            }
        }

        #endregion

       
    }
}