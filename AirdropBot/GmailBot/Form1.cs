using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Common;

namespace GmailBot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        public string MailAddress { get; set; }
        public string MailPwd { get; set; }

        public string Action { get; set; }
        protected string FileName { get; set; }
        public int MaxTries { get; set; }
        public string FindExpression { get; set; }

        private void Form1_Load(object sender, EventArgs e)
        {
            WBEmulator.SetBrowserEmulationVersion(BrowserEmulationVersion.Version7);
            Thread.Sleep(100);
            var args = Environment.GetCommandLineArgs();
            if (args.Length < 3)
            {
                MessageBox.Show("No address/pwd arg passed!");
                return;
            }
            MailAddress = Environment.GetCommandLineArgs()[1];
            MailPwd = Environment.GetCommandLineArgs()[2];
            if (args.Length > 3)
            {
                Action = args[3];
            }
            if (args.Length > 4)
            {
                if (Action == "Find")
                {
                    FindExpression = args[4];
                }
            }
            if (args.Length > 5)
            {
                if (Action == "Find")
                {
                    FileName = args[5];
                }
            } if (args.Length > 6)
            {
                if (Action == "Find")
                {
                    MaxTries = int.Parse(args[6]);
                }
            }
            browser.DocumentCompleted += browser_document_completed;
            browser.Navigate("www.gmail.com");

        }


        private bool searched = false;
        private bool fileWritten = false;
        private bool openedfirstmail = false;
        private int matchingCount = 0;
        private int tries = 0;
        private void browser_document_completed(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (e.Url.AbsolutePath != (sender as WebBrowser).Url.AbsolutePath)
                return;
            HtmlDocument doc = browser.Document;


            //sign out first
            if (e.Url.Host.Contains("mail.google.com"))
            {
                //<input type="submit" value="Load basic HTML" class="submit_as_link">
                foreach (HtmlElement el in doc.GetElementsByTagName("input"))
                {
                    if (el.GetAttribute("value") == "Load basic HTML")
                    {
                        el.InvokeMember("click");
                    }
                }


                var loggedInUser = "";
                var bs = doc.GetElementsByTagName("b");
                foreach (HtmlElement b in bs)
                {
                    if (b.GetAttribute("className") == "gb4")
                    {
                        loggedInUser = b.InnerText.Trim();
                    }
                }

                if (loggedInUser != "" && loggedInUser != MailAddress)
                {
                    var signoutElement = doc.GetElementById("gb_71");
                    if (signoutElement != null) signoutElement.InvokeMember("click");
                }
                //you are in!
                if (loggedInUser == MailAddress)
                {
                    if (Action == "Find")
                    {
                        if (!searched)
                        {

                            //<input type=submit name=nvp_site_mail class=search-form-submit value="Search 
                            var searchBox = doc.GetElementById("sbq");
                            if (searchBox != null)
                            {
                                searchBox.SetAttribute("Value", FindExpression);
                                var inputels = doc.GetElementsByTagName("input");
                                foreach (HtmlElement inputel in inputels)
                                {
                                    if (inputel.GetAttribute("type") == "submit" &&
                                        inputel.GetAttribute("name") == "nvp_site_mail")
                                    {
                                        inputel.InvokeMember("click");
                                        searched = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(FileName)) FileName = @"c:\temp\gmailbot.txt";
                            var htmlformat = "<html><body>{0}</body></html>";
                            if (!openedfirstmail)
                            {
                                //search results here...
                                var inputels = doc.GetElementsByTagName("input");
                                foreach (HtmlElement inputel in inputels)
                                {
                                    if (inputel.GetAttribute("type") == "checkbox" &&
                                        inputel.GetAttribute("name") == "t")
                                    {
                                        //found a matching one
                                        matchingCount++;
                                        var link = inputel.Parent.NextSibling.NextSibling.FirstChild;
                                        if (matchingCount == 1)
                                        {
                                            link.InvokeMember("click");
                                            openedfirstmail = true;
                                        }
                                    }
                                }
                                if (matchingCount == 0 && tries < MaxTries)
                                {
                                    tries++;
                                    Thread.Sleep(7500);
                                    var inputels2 = doc.GetElementsByTagName("input");
                                    foreach (HtmlElement inputel in inputels2)
                                    {
                                        if (inputel.GetAttribute("type") == "submit" &&
                                            inputel.GetAttribute("name") == "nvp_site_mail")
                                        {
                                            inputel.InvokeMember("click");
                                        }
                                    }
                                }
                                else
                                {
                                    File.WriteAllText(FileName,
                                                      string.Format(htmlformat, "<div>found " + matchingCount + "</div>"));
                                }
                            }
                            else
                            {
                                //read content
                                //<div class=msg>
                                var divs = doc.GetElementsByTagName("div");
                                foreach (HtmlElement div in divs)
                                {
                                    if (div.GetAttribute("className") == "msg" && !fileWritten)
                                    {
                                        //MessageBox.Show(div.OuterHtml);
                                        File.WriteAllText(FileName, string.Format(htmlformat, "<div>found " + matchingCount + " " + div.OuterHtml + "</div>"));
                                        fileWritten = true;
                                    }
                                }
                            }
                        }
                    }
                    //search for emails 

                }
            }

            if (e.Url.Host.Contains("accounts.google.com"))
            {

                var emailElement = doc.GetElementById("email");
                if (emailElement != null) emailElement.SetAttribute("Value", MailAddress);
                var passElement = doc.GetElementById("Passwd");
                var inputels = doc.GetElementsByTagName("input");

                foreach (HtmlElement inputel in inputels)
                {
                    if (inputel.GetAttribute("type") == "password")
                    {
                        inputel.SetAttribute("Value", MailPwd);
                    }
                }
                if (passElement != null) passElement.SetAttribute("Value", MailPwd);

                var emailNextElement = doc.GetElementById("next");
                if (emailNextElement == null) //list of users
                {
                    //<button type="submit" id="choose-account-1" name="Email" value="sallamalak@gmail.com" class="V2qUud" tabindex="0" jsname="mzNpsf" data-value="sallamalak@gmail.com">sallamalak@gmail.com</button>
                    var buttons = doc.GetElementsByTagName("button");
                    foreach (HtmlElement button in buttons)
                    {
                        if (button.GetAttribute("value") == MailAddress)
                        {
                            button.InvokeMember("click");
                        }
                    }

                }
                else
                {
                    emailNextElement.InvokeMember("click");

                }
                var signinElement = doc.GetElementById("signIn");
                if (signinElement != null) signinElement.InvokeMember("click");


            }

        }

        private void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

    }
}
