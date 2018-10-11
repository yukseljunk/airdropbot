using CefSharp.WinForms;

namespace AirdropBot
{
    public abstract class AbstractBrowserManipulator:IBrowserManipulator
    {
        #region Implementation of IBrowserManipulator

        public ChromiumWebBrowser Browser { get; set; }

        #endregion

        public string FindXPathScript =
            "function getElementByXpath(path) {{return document.evaluate(path, document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;}}";

    }
}