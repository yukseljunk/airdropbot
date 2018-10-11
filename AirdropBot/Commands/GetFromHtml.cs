using System;
using System.Text.RegularExpressions;
using System.Xml;
using HtmlAgilityPack;

namespace AirdropBot
{
    public class GetFromHtml : ICommand
    {

        #region Implementation of ICommand

        public string Command(XmlNode node)
        {
            var html = node.Attributes["html"];
            var param = node.Attributes["param"];
            var what = node.Attributes["what"];
            var regex = node.Attributes["regex"];
            var xpath = node.Attributes["xpath"];

            if (param == null || what == null || xpath == null || html==null) return "Param or what or xpath or html is not defined!";
            if (param.Value == "" || what.Value == "" || xpath.Value == "" || html.Value=="") return "Param or what or xpath or html is empty!";
            //agility to be here...
            var document= new HtmlDocument();
            try
            {
                document.LoadHtml(Helper.ReplaceTokens(html.Value));
            }
            catch(Exception exc)
            {
                return exc.ToString();
            }

            var xnode = document.DocumentNode.SelectSingleNode(xpath.Value);
            if(xnode==null)
            {
                return "";
            }
            var result = "";
            switch(what.Value.ToLower())
            {
                case "innertext":
                    result= xnode.InnerText;
                    break;
                case "innerhtml":
                    result= xnode.InnerHtml;
                    break;
                case "outerhtml":
                    result=xnode.OuterHtml;
                    break;
                case "getattribute":
                    result= xnode.Attributes["abc"].Value;
                    break;
                
            }

            if (regex != null && regex.Value != "")
            {
                var reg = new Regex(regex.Value);

                var match = reg.Match(result);
                if (match.Success)
                {
                    if (match.Groups.Count > 1)
                    {
                        result = match.Groups[1].Value;
                    }
                }
            }

            if (!Helper.Variables.ContainsKey(param.Value))
            {
                Helper.Variables.Add(param.Value, result);
            }
            Helper.Variables[param.Value] = result;


            return "";

        }

        public string Tag
        {
            get { return "getfromhtml"; }
        }
        public string Html
        {
            get
            {
                return "<getfromhtml html=\"\" param=\"var\" what=\"innerText\" xpath=\"//*[@id='']\" regex=\"\" />";

            }
        }

        #endregion


    }
}