using CefSharp.WinForms;

namespace AirdropBot
{
    public interface IBrowserManipulator
    {
        ChromiumWebBrowser Browser { get; set; }
    }
}