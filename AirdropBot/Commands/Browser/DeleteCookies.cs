using System.Xml;
using CefSharp;

namespace AirdropBot
{
    public class DeleteCookies : AbstractBrowserManipulator, ICommand
    {

        #region Implementation of ICommand

        public string Command(XmlNode node)
        {

            return SuppressCookiePersistence();

        }

        private static string SuppressCookiePersistence()
        {
            try
            {
                Cef.GetGlobalCookieManager().DeleteCookies("", "");
            }
            catch
            {
            }
            return "";
        }

       
        public string Tag
        {
            get { return "clearcookies"; }
        }
        public string Html
        {
            get
            {
                return "<navigate url=\"about:blank\"/><clearcookies/>";

            }
        }

        #endregion

       
    }
}