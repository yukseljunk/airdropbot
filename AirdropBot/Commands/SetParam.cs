using System.Text.RegularExpressions;
using System.Xml;

namespace AirdropBot
{
    public class SetParam : ICommand
    {

        #region Implementation of ICommand

        public string Command(XmlNode node)
        {
            var param = node.Attributes["param"];
            var value = node.Attributes["value"];
            var regex = node.Attributes["regex"];
            var replace = node.Attributes["replace"];
            var eval = node.Attributes["eval"];

            if (param == null) return "Param  is not defined!";
            if (param.Value == "") return "Param is empty!";
            if (eval == null && value == null) return "eval or value is not defined!";
            
            var result = Helper.ReplaceTokens(value.Value);
            if(value.Value=="" && eval.Value!="")
            {
                result = Helper.Evaluate(Helper.ReplaceTokens(eval.Value));
            }

            if (regex != null && regex.Value != "")
            {
                var reg = new Regex(regex.Value);
                if (replace != null )
                {
                    result = reg.Replace(result, replace.Value);
                }
                else
                {

                    var match = reg.Match(result);
                    if (match.Success)
                    {
                        if (match.Groups.Count > 1)
                        {
                            result = match.Groups[1].Value;
                        }
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
            get { return "setparam"; }
        }
        public string Html
        {
            get
            {
                return "<setparam param=\"\" eval=\"\" value=\"\" regex=\"\" replace=\"\"/>";

            }
        }

        #endregion


    }
}