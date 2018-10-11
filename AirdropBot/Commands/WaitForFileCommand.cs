using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace AirdropBot
{
    public class WaitForFileCommand : ICommand
    {
        private int _defaultTimeout = 60;

        #region Implementation of ICommand

        public string Command(XmlNode node)
        {
            var filename = node.Attributes["filename"];
            var timeout = node.Attributes["timeout"];
            var tout = _defaultTimeout;

            if (filename == null) return "Filename is not defined!";
            if (filename.Value == "") return "Filename is empty!";
            if(timeout!=null && timeout.Value!="")
            {
                tout = int.Parse(Helper.ReplaceTokens(timeout.Value));
            }
            var file = Helper.ReplaceTokens(filename.Value);
            WaitForFile(file, tout);
            return "";

        }

        public string Tag
        {
            get { return "waitforfile"; }
        }
        public string Html
        {
            get
            {
                return "<waitforfile  filename=\"c:\\temp\\watchfile\\bot1.txt\" timeout=\"10\"/>";
            }
        }

        #endregion

        private bool WaitForFile(string file, int timeout)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                Application.DoEvents();
                if (File.Exists(file)) return true;
                if (sw.ElapsedMilliseconds >= timeout * 1000)
                {
                    break;
                }
            }
            return false;
        }
    }
}