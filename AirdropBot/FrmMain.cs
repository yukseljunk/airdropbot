using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using CefSharp;
using CefSharp.WinForms;
using TestStack.White.WindowsAPI;

namespace AirdropBot
{
    /// <summary>
    /// stop process de loadingwait lerde cikar
    /// </summary>
    public partial class FrmMain : Form
    {
        private ChromiumWebBrowser cbrowser;

        public Dictionary<string, string> Variables
        {
            get { return localVariables; }
        }
        private Dictionary<string, string> localVariables = new Dictionary<string, string>();

        public FrmMain()
        {
            InitializeComponent();
        }

        private Dictionary<int, User> Users { get; set; }
        private User ActiveUser { get; set; }


        public bool OnlyBrowser { get; set; }
        public string Scenario { get; set; }

        private void FillUsers()
        {
            lstUsers.Items.Clear();
            foreach (var user in Users)
            {
                lstUsers.Items.Add(new string(' ', 4 - user.Value.Id.ToString().Length) + user.Value.Id + ". " + user.Value.Name + " " + user.Value.LastName);
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            try
            {
                ConfigureCef();
            }
            catch{}
            if (OnlyBrowser)
            {
                this.WindowState = FormWindowState.Maximized;
                pnlUsers.Visible = false;
                //menuStrip1.Visible = false;
                pnlScenario.Location = new Point(pnlUsers.Top, pnlScenario.Left);
                pnlScenario.Size = new Size(pnlScenario.Width, pnlUsers.Height + pnlScenario.Height);
                btnRunRest.Visible = false;
                btnApplyScenario.Text = "Run Scenario";
                btnStop.Location = btnRunRest.Location;
                txtScenario.Text = Scenario;
                return;
            }
            
            btnStop.Enabled = false;
            LoadUsers();
            RestoreLastScenario();
        }

        private string lastScenarioFile = Helper.AssemblyDirectory + @"\\lastsc.txt";
        private void RestoreLastScenario()
        {
            if (File.Exists(lastScenarioFile))
            {
                var lastScFile = File.ReadAllText(lastScenarioFile);
                if(File.Exists(lastScFile))
                {
                    txtScenario.Text = File.ReadAllText(lastScFile);
                    scenarioFileName = lastScFile;
                    this.Text = scenarioFileName;

                }
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(scenarioFileName!="")
            {
                if(!this.Text.EndsWith("*"))
                {
                    this.Text += "*";
                }
            }
        }

        private bool cloadingFinished = false;

        private bool stopped = false;
        private void btnApplyScenario_Click(object sender, EventArgs e)
        {

            if (txtScenario.Text == "") return;
            EnDis(true);
            stopped = false;
            var scenario = txtScenario.Text;
            if (txtScenario.SelectionLength > 0)
            {
                scenario = txtScenario.SelectedText;
            }
            Run(scenario);
            EnDis(false);

        }

        private void EnDis(bool state)
        {
            btnStop.Enabled = state;
            btnApplyScenario.Enabled = !state;
            btnRunRest.Enabled = !state;

        }

        private void Run(string cmd)
        {
            var xml = cmd;
            if (!cmd.Contains("?xml"))
            {
                xml = string.Format("<?xml version=\"1.0\"?><steps>{0}</steps>", cmd);
            }
            var doc = new XmlDocument();
            try
            {
                doc.LoadXml(xml);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid xml! \r\n" + xml);
                return;
            }
            var nodeList = doc.SelectNodes("/steps/*");
            if (nodeList == null) return;
            var stepNo = 1;
            foreach (XmlNode node in nodeList)
            {
                var command = node.Name.ToLower();
                if (command != "") Debug.WriteLine(command + " " + DateTime.Now.ToString("hh:mm:ss"));
                if (stopped) break;
                var commandResult = "";
                if (command == "navigate")
                {
                    commandResult = NavigateCommand(node);
                }
                if (command == "scroll")
                {
                    commandResult = ScrollCommand(node);
                }
                if (command == "snap")
                {
                    commandResult = SnapCommand(node);
                }
                if (command == "set")
                {
                    commandResult = SetCommand(node);
                }
                if (command == "get")
                {
                    commandResult = GetCommand(node);
                }
                if (command == "waittill")
                {
                    commandResult = WaitTillCommand(node);

                }
                if (command == "failif")
                {
                    commandResult = FailIfCommand(node);

                }
                if (command == "continueif")
                {
                    commandResult = ContinueIfCommand(node);

                }
                if (command == "click")
                {
                    commandResult = ClickCommand(node);
                }

                if (command == "delete")
                {
                    commandResult = DeleteCommand(node);
                }
                if (command == "submit")
                {
                    commandResult = SubmitCommand(node);
                }

                if (command == "repeat")
                {
                    commandResult = RepeatCommand(node);
                }

                if (command == "wait")
                {
                    commandResult = WaitCommand(node);
                }
                if (command == "mail")
                {
                    commandResult = MailCommand(node);
                }
                if (command == "twitter")
                {
                    commandResult = TwitterCommand(node);
                }
                if (command == "facebook")
                {
                    commandResult = FacebookCommand(node);
                }
                if (command == "clearcookies")
                {
                    commandResult = SuppressCookiePersistence();
                }
                if (command == "telegram")
                {
                    this.WindowState = FormWindowState.Minimized;
                    commandResult = TelegramCommandMemu(node);
                    this.WindowState = FormWindowState.Maximized;

                }
                if (command == "info")
                {
                    commandResult = InfoCommand(node);
                }
                if (command == "sendkey")
                {
                    commandResult = SendKeyCommand(node);
                }
                if (command == "bringtofront")
                {
                    commandResult = BringToFrontCommand(node);
                }
                if (command == "focus")
                {
                    commandResult = FocusCommand(node);

                }
                if (command == "createtg")
                {
                    commandResult = CreateTgCommand(node);

                }
                if (command == "if")
                {
                    commandResult = IfCommand(node);
                }
                if (command == "ifnot")
                {
                    commandResult = IfNotCommand(node);
                }
                if (command == "kucoin")
                {
                    commandResult = KucoinRetweet(node);
                }
                if (command == "screenshot")
                {
                    commandResult = ScreenShotCommand(node);
                }
                if (commandResult != "")
                {
                    MessageBox.Show("Error in " + command + " @" + stepNo + ".step: " + commandResult);
                    stopped = true;
                    break;
                }
                stepNo++;
            }

        }

        private string RepeatCommand(XmlNode node)
        {
            var times = node.Attributes["times"];


            if (times == null) return "Repeat times is not defined!";
            if (times.Value == "") return "Repeat times is empty!";
            for (int i = 0; i < int.Parse(times.Value); i++)
            {
                Run(node.InnerXml);
            }
            return "";
        }


        private string KucoinRetweet(XmlNode node)
        {//
            //txtScenario.SelectedText = "<kucoin postno=\"\" twApiCust=\"\" twApiCustSec=\"\"  twApiToken=\"\" twApiTokenSec=\"\" twitteruser=\"\" fullname=\"\" kucoinemail=\"\"/>"; 
            var postno = node.Attributes["postno"];
            if (postno == null) return "Post number is not defined!";
            if (postno.Value == "") return "Post number is empty!";

            var consumerKey = ActiveUser.TwConsumerKey;
            var twcustnode = node.Attributes["twApiCust"];
            if (twcustnode != null && twcustnode.Value != "")
            {
                consumerKey = twcustnode.Value;
            }

            var consumerSecret = ActiveUser.TwConsumerSecret;
            var twcustsecnode = node.Attributes["twApiCustSec"];
            if (twcustsecnode != null && twcustsecnode.Value != "")
            {
                consumerSecret = twcustsecnode.Value;
            }
            var twtokenNode = node.Attributes["twApiToken"];
            var twTokenSecNode = node.Attributes["twApiTokenSec"];
            if (twtokenNode == null || twTokenSecNode == null) return "Not defined access tokens!";
            if (twtokenNode.Value == "" || twTokenSecNode.Value == "") return "Empty access tokens!";

            var twUser = ActiveUser.TwUserName;
            var twUserNode = node.Attributes["twitteruser"];
            if (twUserNode != null && twUserNode.Value != "")
            {
                twUser = twUserNode.Value;
            }

            var fullName = ActiveUser.Name + " " + ActiveUser.LastName;
            var fullNameNode = node.Attributes["fullname"];
            if (fullNameNode != null && fullNameNode.Value != "")
            {
                fullName = fullNameNode.Value;
            }
            var kucoinUser = ActiveUser.KucoinUser;
            var kucoinemailnode = node.Attributes["kucoinemail"];
            if (kucoinemailnode != null && kucoinemailnode.Value != "")
            {
                kucoinUser = kucoinemailnode.Value;
            }
            var formlink = "";
            var flink = node.Attributes["formlink"];
            if(flink!=null && flink.Value!="")
            {
                formlink = flink.Value;
            }

            var kucoinTemplate = File.ReadAllText(Helper.AssemblyDirectory + "\\Templates\\KucoinRetweet.xml");
            kucoinTemplate = kucoinTemplate.Replace("${0}", ReplaceTokens(postno.Value)).Replace("${1}", consumerKey).Replace("${2}", consumerSecret)
                .Replace("${3}", ReplaceTokens(twUser)).Replace("${4}", ReplaceTokens(fullName)).Replace("${5}", ReplaceTokens(kucoinUser))
                .Replace("${6}", twtokenNode.Value).Replace("${7}", twTokenSecNode.Value);
            if(formlink!="")
            {
                kucoinTemplate = kucoinTemplate.Replace("${formlink}", formlink);
            }
            Run(kucoinTemplate);
            return "";

        }

        private string ScreenShotCommand(XmlNode node)
        {
            var fileNode = node.Attributes["file"];
            var fileName = "c:\\temp\\test.jpg";
            if (fileNode != null && fileNode.Value != "")
            {
                fileName = fileNode.Value;
            }
            Rectangle bounds = ContentPanel.Bounds;
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);

                }
                bitmap.Save(fileName, ImageFormat.Jpeg);
            }
            return "";
        }


        private string IfNotCommand(XmlNode node)
        {
            var compare = node.Attributes["compare"];
            var what = node.Attributes["what"];
            var regex = node.Attributes["regex"];
            var xpath = node.Attributes["xpath"];

            if (compare == null || what == null || xpath == null) return "Compare or what or xpath is not defined!";
            if (compare.Value == "" || what.Value == "" || xpath.Value == "") return "Compare or what or xpath is empty!";

            var result = GetCElement(node);
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
            if (result != ReplaceTokens(compare.Value))//criteria not met
            {
                Run(node.InnerXml);
            }

            return "";
        }

        private string IfCommand(XmlNode node)
        {
            var compare = node.Attributes["compare"];
            var what = node.Attributes["what"];
            var regex = node.Attributes["regex"];
            var xpath = node.Attributes["xpath"];

            if (compare == null || what == null || xpath == null) return "Compare or what or xpath is not defined!";
            if (compare.Value == "" || what.Value == "" || xpath.Value == "") return "Compare or what or xpath is empty!";

            var result = GetCElement(node);
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
            if (result == ReplaceTokens(compare.Value))//criteria met
            {
                Run(node.InnerXml);
            }

            return "";
        }

        private string ContinueIfCommand(XmlNode node)
        {
            var compare = node.Attributes["compare"];
            var what = node.Attributes["what"];
            var regex = node.Attributes["regex"];
            var xpath = node.Attributes["xpath"];

            if (compare == null || what == null || xpath == null) return "Compare or what or xpath is not defined!";
            if (compare.Value == "" || what.Value == "" || xpath.Value == "") return "Compare or what or xpath is empty!";

            var result = GetCElement(node);
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
            if (result != ReplaceTokens(compare.Value))
            {
                return "Criteria not met, not continuing...";
            }


            return "";
        }

        private string FailIfCommand(XmlNode node)
        {
            var compare = node.Attributes["compare"];
            var what = node.Attributes["what"];
            var regex = node.Attributes["regex"];
            var xpath = node.Attributes["xpath"];

            if (compare == null || what == null || xpath == null) return "Compare or what or xpath is not defined!";
            if (compare.Value == "" || what.Value == "" || xpath.Value == "") return "Compare or what or xpath is empty!";

            var result = GetCElement(node);
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
            if (result == ReplaceTokens(compare.Value))
            {
                return "Criteria met, failing";
            }

            return "";
        }

        private string CreateTgCommand(XmlNode node)
        {
            //name, password


            return "";
        }

        /// <summary>
        /// waits for secs, without sleeping
        /// </summary>
        /// <param name="secs"></param>
        private static void Wait(int secs)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                Application.DoEvents();
                if (sw.ElapsedMilliseconds >= secs * 1000)
                {
                    break;
                }
            }
        }



        private string FocusCommand(XmlNode node)
        {
            var xpath = node.Attributes["xpath"];
            if (xpath != null && xpath.Value != "")
            {
                string scr = string.Format("{1} function x(){{ if(getElementByXpath(\"{0}\")==null)  return 'UNDEF'; getElementByXpath(\"{0}\").focus();}} x(); ", ReplaceTokens(xpath.Value), FindXPathScript);
                var resp = "";
                cbrowser.EvaluateScriptAsync(scr).ContinueWith(x =>
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

        private string SubmitCommand(XmlNode node)
        {
            cloadingFinished = false;
            var el = GetCSubmit(node);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            stopped = false;
            while (!cloadingFinished && !stopped)
            {
                Application.DoEvents();
                if (sw.ElapsedMilliseconds >= browsertimeoutSecs * 1000)
                {
                    return "Timeout after secs " + browsertimeoutSecs * 1000;//timeout
                }
            }
            return el;
        }

        public void ConfigureCef()
        {
            CefSettings cfsettings = new CefSettings();
            cfsettings.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:59.0) Gecko/20100101 Firefox/59.0";
            //cfsettings.CachePath = @"c:\temp\cefcache";
            //cfsettings.CefCommandLineArgs.Add("proxy-server", proxy);
            Cef.Initialize(cfsettings);
        }
        private void CreateCBrowser(string url, string proxy)
        {

            ContentPanel.Controls.Clear();
            // Configure(proxy);
            cproxy = proxy;
            if (cbrowser != null) cbrowser.Dispose();
            cbrowser = new ChromiumWebBrowser(url)
            {
                Dock = DockStyle.Fill
            };
            cbrowser.BrowserSettings.FileAccessFromFileUrls = CefState.Enabled;
            cbrowser.BrowserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
            ContentPanel.Controls.Add(cbrowser);
            cbrowser.IsBrowserInitializedChanged += cbrowser_initalize;

            cbrowser.LoadingStateChanged += OnLoadingStateChanged;
            /*            browser.ConsoleMessage += OnBrowserConsoleMessage;
                        browser.StatusMessage += OnBrowserStatusMessage;
                        browser.TitleChanged += OnBrowserTitleChanged;
                        browser.AddressChanged += OnBrowserAddressChanged;
              */
        }

        private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)//loading done
            {
                cloadingFinished = true;
            }
        }

        private string cproxy;

        private void cbrowser_initalize(object sender, IsBrowserInitializedChangedEventArgs e)
        {
            if (e.IsBrowserInitialized)
            {
                string error;
                if (cproxy != "" && cproxy != ":")
                {
                    Cef.UIThreadTaskFactory.StartNew(delegate
                                                         {
                                                             var rc = cbrowser.GetBrowser().GetHost().RequestContext;
                                                             var v = new Dictionary<string, object>();
                                                             v["mode"] = "fixed_servers";
                                                             v["server"] = "http://" + cproxy;
                                                             bool success = rc.SetPreference("proxy", v, out error);
                                                         });
                }
                else
                {
                    Cef.UIThreadTaskFactory.StartNew(delegate
                    {
                        var rc = cbrowser.GetBrowser().GetHost().RequestContext;
                        var v = new Dictionary<string, object>();
                        v["mode"] = "direct";
                        bool success = rc.SetPreference("proxy", v, out error);
                    });

                }
            }
        }

        private string BringToFrontCommand(XmlNode node)
        {
            this.WindowState = FormWindowState.Minimized;
            this.Show();
            this.WindowState = FormWindowState.Maximized;
            return "";
        }

        private string SendKeyCommand(XmlNode node)
        {

            var xnode = node.Attributes["value"];
            if (xnode == null) return "";

            string txt = Regex.Replace(ReplaceTokens(xnode.Value), "[+^%~()]", "{$0}");

            SendKeys.Send(txt);
            return "";
        }

        private string InfoCommand(XmlNode node)
        {
            var xnode = node.Attributes["value"];
            if (xnode == null) return "";
            MessageBox.Show(ReplaceTokens(xnode.Value));
            return "";
        }

        private string SnapCommand(XmlNode node)
        {
            var x = 0;
            var y = 0;
            var xnode = node.Attributes["x"];
            if (xnode != null)
            {
                int.TryParse(xnode.Value, out x);
            }
            var ynode = node.Attributes["y"];
            if (ynode != null)
            {
                int.TryParse(ynode.Value, out y);

            }

            var xpath = node.Attributes["xpath"];
            if (xpath != null && xpath.Value != "")
            {
                cbrowser.ExecuteScriptAsync(String.Format("window.scrollBy({0}, {1});", -10000, -10000));



                string scr = string.Format("{1} function x(){{ if(getElementByXpath(\"{0}\")==null)  return 'Cannot find element!'; var rect= getElementByXpath(\"{0}\").getBoundingClientRect(); return rect.top + ':'+ rect.right+ ':'+ rect.bottom+ ':'+ rect.left;}} x(); ", ReplaceTokens(xpath.Value), FindXPathScript);
                var resp = "";
                cbrowser.EvaluateScriptAsync(scr).ContinueWith(xabc =>
                {
                    var response = xabc.Result;

                    if (response.Success && response.Result != null)
                    {
                        resp = response.Result.ToString();
                        //startDate is the value of a HTML element.
                    }
                }).Wait();
                if (!resp.Contains(":")) return resp;
                var rect = resp.Split(new char[] { ':' });
                if (rect.Length < 4) return "Invalid location info " + resp;
                int top = FindIntegerPart(rect[0]);
                int right = FindIntegerPart(rect[1]);
                int bottom = FindIntegerPart(rect[2]);
                int left = FindIntegerPart(rect[3]);

                var xval = xnode.Value;
                var yval = ynode.Value;
                var xnegative = xval.StartsWith("-");
                var ynegative = yval.StartsWith("-");
                var xrelative = xval.StartsWith("%");
                var yrelative = yval.StartsWith("%");
                xval = xval.Replace("-", "").Replace("%", "");
                yval = yval.Replace("-", "").Replace("%", "");
                int xpoint = int.Parse(xval);
                int ypoint = int.Parse(yval);
                if (xrelative) xpoint = Convert.ToInt32((right - left) * xpoint / 100);
                if (yrelative) ypoint = Convert.ToInt32((bottom - top) * ypoint / 100);
                if (xnegative) xpoint = -1 * xpoint;
                if (ynegative) ypoint = -1 * ypoint;

                x = left + xpoint;
                y = top + ypoint;

            }

            MouseOperations.SetCursorPosition(this.Left + ContentPanel.Left + x, this.Top + ContentPanel.Top + y);
            MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);
            MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);

            return "";
        }

        private int FindIntegerPart(string s)
        {
            if (s.Contains(",") || s.Contains("."))
            {
                //var notLastItem = s.Contains(",") && s.Contains(".") ? 1 : 0;
                var result = "";
                var parts = s.Split(new[] { '.', ',' });
                for (int i = 0; i < parts.Length - 1; i++)
                {
                    result += parts[i];
                }
                return int.Parse(result);
            }

            return int.Parse(s);
        }

        private string ScrollCommand(XmlNode node)
        {
            var height = 100;
            var secs = node.Attributes["height"];
            if (secs != null)
            {
                int.TryParse(node.Attributes["height"].Value, out height);
            }

            cbrowser.ExecuteScriptAsync(String.Format("window.scrollBy({0}, {1});", 0, height));
            return "";
        }
        private void RunTemplate(string templateName, params  string[] args)
        {
            var template = File.ReadAllText(Helper.AssemblyDirectory + "\\Templates\\" + templateName + ".xml");
            var argIndex = 0;
            foreach (var s in args)
            {
                template = template.Replace("${" + argIndex + "}", s);
                argIndex++;
            }
            Run(template);
        }

        private string TwitterCommand(XmlNode node)
        {
            /*            
             <twitter user=\"\" pass=\"\" consumerkey=\"\" consumersecret=\"\" accesstoken=\"\" accesstokensecret=\"\">";
*/
            //user, pass, search
            var user = node.Attributes["user"];
            var password = node.Attributes["pass"];

            var consumerkey = node.Attributes["consumerkey"];
            var consumersecret = node.Attributes["consumersecret"];
            var accesstoken = node.Attributes["accesstoken"];
            var accesstokensecret = node.Attributes["accesstokensecret"];


            var commands = new HashSet<string>();
            if (node.HasChildNodes)
            {
                foreach (XmlNode subNode in node.ChildNodes)
                {
                    commands.Add(subNode.Name);
                }
            }
            commands.Remove("wait");
            var apiCommands = new HashSet<string>() { "follow", "like", "retweet" };
            var onlyAPI = commands.Any() && commands.IsSubsetOf(apiCommands);
            var someAPI = commands.Intersect(apiCommands).Any();

            if (someAPI) //only validate API params when there is a call to API
            {

                if (consumerkey == null || consumersecret == null || accesstoken == null || accesstokensecret == null)
                    return "API Keys not defined";
                if (consumerkey.Value == "" || consumersecret.Value == "" || accesstoken.Value == "" ||
                    accesstokensecret.Value == "") return "API Keys empty";
            }
            if (!onlyAPI)
            {
                if (user == null || password == null) return "User/password not defined";
                if (user.Value == "" || password.Value == "") return "User/password empty";

            }
            if (!onlyAPI) RunTemplate("TwitterLogin", user.Value, password.Value);//only use browser when there is some other call then API

            if (node.HasChildNodes)
            {
                stopped = false;
                foreach (XmlNode subNode in node.ChildNodes)
                {
                    if (stopped) break;
                    if (subNode.Name == "search")
                    {
                        var textNode = subNode.Attributes["text"];
                        if (textNode == null) continue;
                        if (textNode.Value == "") continue;
                        RunTemplate("TwitterSearch", ReplaceTokens(textNode.Value));

                    }
                    else if (subNode.Name == "follow")
                    {
                        var addressNode = subNode.Attributes["address"];
                        if (addressNode == null) continue;
                        if (addressNode.Value == "") continue;

                        RunTweetApi("follow", ReplaceTokens(addressNode.Value), consumerkey.Value, consumersecret.Value, accesstoken.Value, accesstokensecret.Value);
                    }
                    else if (subNode.Name == "like")
                    {
                        var postNode = subNode.Attributes["post"];
                        if (postNode == null) continue;
                        if (postNode.Value == "") continue;
                        RunTweetApi("like", ReplaceTokens(postNode.Value), consumerkey.Value, consumersecret.Value, accesstoken.Value, accesstokensecret.Value);
                    }
                    else if (subNode.Name == "retweet")
                    {
                        //https://twitter.com/JonErlichman/status/971536891924439040
                        var postNode = subNode.Attributes["post"];
                        if (postNode == null) continue;
                        if (postNode.Value == "") continue;
                        RunTweetApi("retweet", ReplaceTokens(postNode.Value), consumerkey.Value, consumersecret.Value, accesstoken.Value, accesstokensecret.Value);
                    }
                    else
                    {
                        Run(subNode.OuterXml);
                    }
                }
            }
            //logout
            if (!onlyAPI) RunTemplate("TwitterLogout");
            return "";
        }

        private void RunTweetApi(string command, string commandarg, string consumerkey, string consumersecret, string accesstoken, string accesstokensecret)
        {

            var botExe = Helper.AssemblyDirectory + "\\TwitterBot\\TwitterBot.exe";
            var output = Helper.StartProcess(botExe,
                                string.Format("{0} {1} {2} {3} {4} {5}", ReplaceTokens(consumerkey), ReplaceTokens(consumersecret), ReplaceTokens(accesstoken),
                                              ReplaceTokens(accesstokensecret), command, commandarg), true);
            //wait for bot to respond
            Wait(5);
            Debug.WriteLine(output);

            var htmlFile = Helper.AssemblyDirectory + "\\temp2.html";
            File.WriteAllText(htmlFile, output);
            CreateCBrowser(htmlFile + "?nonce=" + Guid.NewGuid(), "");

        }


        private string FacebookCommand(XmlNode node)
        {
            //user, pass, search
            var user = node.Attributes["user"];
            var password = node.Attributes["pass"];
            if (user == null || password == null) return "User/password empty or not defined";
            RunTemplate("FBLogin", user.Value, password.Value);

            //txtScenario.SelectedText = "<facebook user=\"\" pass=\"\">\r\n<search text=\"\"/>\r\n<follow page=\"\"/>\r\n<like post=\"\"/>\r\n<like page=\"\"/>\r\n<share post=\"\"/>\r\n</facebook>";

            if (node.HasChildNodes)
            {
                stopped = false;
                foreach (XmlNode subNode in node.ChildNodes)
                {
                    if (stopped) break;
                    if (subNode.Name == "search")
                    {
                        var textNode = subNode.Attributes["text"];
                        if (textNode == null) continue;
                        if (textNode.Value == "") continue;
                        RunTemplate("FBSearch", ReplaceTokens(textNode.Value));

                    }
                    else if (subNode.Name == "follow")
                    {
                        var pageNod = subNode.Attributes["page"];
                        if (pageNod == null) continue;
                        if (pageNod.Value == "") continue;
                        var url = ReplaceTokens(pageNod.Value);
                        if (!url.StartsWith("http")) url = "https://www.facebook.com/" + url;
                        RunTemplate("FBFollow", url);
                    }
                    else if (subNode.Name == "like")
                    {
                        var pageNode = subNode.Attributes["page"];
                        if (pageNode != null)
                        {
                            if (pageNode.Value == "") continue;
                            var url = ReplaceTokens(pageNode.Value);
                            if (!url.StartsWith("http")) url = "https://www.facebook.com/" + url;
                            RunTemplate("FBLikePage", url);
                        }
                        var postNode = subNode.Attributes["post"];
                        if (postNode != null)
                        {
                            if (postNode.Value == "") continue;
                            var url = ReplaceTokens(postNode.Value);
                            if (!url.StartsWith("http")) url = "https://www.facebook.com/" + url;
                            RunTemplate("FBLikePost", url);
                        }

                    }
                    else if (subNode.Name == "share")
                    {

                        var postNode = subNode.Attributes["post"];
                        if (postNode == null) continue;
                        if (postNode.Value == "") continue;
                        var url = ReplaceTokens(postNode.Value);
                        if (!url.StartsWith("http")) url = "https://www.facebook.com/" + url;
                        RunTemplate("FBShare", url);
                        /*
                        
                         
                         */
                    }
                    else
                    {
                        Run(subNode.OuterXml);
                    }


                }
            }
            //logout
            RunTemplate("FBLogout");
            return "";
        }

        private string TelegramCommandMemu(XmlNode node)
        {
            var user = node.Attributes["user"];
            var password = node.Attributes["pass"];
            var group = node.Attributes["group"];
            var chat = node.Attributes["chat"];
            if (user == null || password == null) return "User/password not defined";
            if (!ReplaceTokens(user.Value).StartsWith("m_"))
            {
                return TelegramCommand(node);
            }
            var url = "";

            if (group != null && group.Value != "")
            {
                url = group.Value;
            }
            if (chat != null && chat.Value != "")
            {

                url = chat.Value;
            }

            Rect location = new Rect();
            var memuRes = Helper.OpenTelegramMemu(ReplaceTokens(user.Value), ReplaceTokens(password.Value), url,
                                                  out location, true);
            if (memuRes != "") return memuRes;

            if (node.HasChildNodes)
            {
                stopped = false;
                foreach (XmlNode subNode in node.ChildNodes)
                {
                    if (stopped) break;
                    if (subNode.Name == "click")
                    {
                        var x = subNode.Attributes["x"];
                        var y = subNode.Attributes["y"];
                        if (x == null || y == null) continue;
                        var xval = x.Value;
                        var yval = y.Value;
                        var xnegative = xval.StartsWith("-");
                        var ynegative = yval.StartsWith("-");
                        var xpositive = xval.StartsWith("+");
                        var ypositive = yval.StartsWith("+");
                        var xrelative = xval.StartsWith("%");
                        var yrelative = yval.StartsWith("%");
                        xval = xval.Replace("-", "").Replace("+", "").Replace("%", "");
                        yval = yval.Replace("-", "").Replace("+", "").Replace("%", "");
                        int xpoint = int.Parse(xval);
                        int ypoint = int.Parse(yval);
                        if (xrelative) xpoint = location.Left + Convert.ToInt32((location.Right - location.Left) * xpoint / 100);
                        if (yrelative) ypoint = location.Top + Convert.ToInt32((location.Bottom - location.Top) * ypoint / 100);
                        if (xnegative) xpoint = Convert.ToInt32(location.Right - xpoint);
                        if (ynegative) ypoint = Convert.ToInt32(location.Bottom - ypoint);
                        if (xpositive) xpoint = Convert.ToInt32(location.Left + xpoint);
                        if (ypositive) ypoint = Convert.ToInt32(location.Top + ypoint);

                        ClickOnPointTool.ClickOnPoint(new Point(xpoint, ypoint));


                    }
                    else if (subNode.Name == "message")
                    {

                        //<message text
                        var text = subNode.Attributes["text"];

                        if (text != null && text.Value.Trim() != "")
                        {

                            var scenarioPlayPos = new Point(location.Right - 12, location.Top + 88);
                            ClickOnPointTool.ClickOnPoint(scenarioPlayPos);
                            Thread.Sleep(1000);
                            var scenarioFile = ConfigurationManager.AppSettings["memuscenariofile"];
                            File.Copy(Helper.AssemblyDirectory + "\\Templates\\MemuTgMessage1.txt", scenarioFile, true);

                            var msg = ReplaceTokens(text.Value);
                            for (int i = 0; i < msg.Length; i++)
                            {
                                File.AppendAllText(scenarioFile, (50000 + i * 100000) + "--CLIPBOARD--" + msg[i] + "\r\n");
                            }
                            var template2 = File.ReadAllText(Helper.AssemblyDirectory + "\\Templates\\MemuTgMessage2.txt");
                            File.AppendAllText(scenarioFile, template2);

                            var replayScenarioPos = new Point((location.Right + location.Left) / 2 + 120, (location.Top + location.Bottom) / 2 - 30);
                            ClickOnPointTool.ClickOnPoint(replayScenarioPos);
                            Thread.Sleep(1000);

                            var closeScenarioPos = new Point((location.Right + location.Left) / 2 + 195, (location.Top + location.Bottom) / 2 - 135);
                            ClickOnPointTool.ClickOnPoint(closeScenarioPos);
                            Thread.Sleep(1000);

                        }
                    }
                    else
                    {
                        Run(subNode.OuterXml);
                    }

                }
            }
            return "";


        }

        private string TelegramCommand(XmlNode node)
        {
            var user = node.Attributes["user"];
            var password = node.Attributes["pass"];
            var group = node.Attributes["group"];
            var chat = node.Attributes["chat"];
            var message = node.Attributes["message"];
            if (user == null || password == null) return "User/pass not defined";


            //tg://join/?invite=AAAAAE7c308RYtaVAyyomw
            var extraArgs = "";
            if (group != null && group.Value != "")
            {
                var groupName = group.Value;
                if (groupName.StartsWith("http"))
                {
                    groupName = new Uri(groupName).PathAndQuery.Substring(1);
                }
                extraArgs = "-- tg://resolve/?domain=" + ReplaceTokens(groupName).Replace("?", "&");
            }
            if (chat != null && chat.Value != "")
            {
                var chatName = chat.Value;
                if (chatName.StartsWith("http"))
                {
                    chatName = new Uri(chatName).PathAndQuery.Substring(1);
                }
                extraArgs = "-- tg://join/?invite=" + ReplaceTokens(chatName).Replace("?", "&");
            }

            if (Helper.OpenTelegram(ReplaceTokens(user.Value), extraArgs, ReplaceTokens(password.Value)) != "") return "Runas exe is not running!";

            //this may require closing off all telegram instances
            TestStack.White.Application app = TestStack.White.Application.Attach(@"Telegram");
            var mainWindow = app.GetWindows()[0];
            try
            {
                mainWindow.DisplayState = TestStack.White.UIItems.WindowItems.DisplayState.Maximized;
            }
            catch
            {
            }

            //join or open group/send message
            if (@group != null && @group.Value.Trim() != "")
            {
                /*Process p = Process.GetProcessesByName("Telegram")[0];

                SetForegroundWindow(p.Handle); access denied*/
                /*Debug.WriteLine(string.Format("{0} {1} {2} {3}", mainWindow.Bounds.Top, mainWindow.Bounds.Left,
                                              mainWindow.Bounds.Bottom, mainWindow.Bounds.Right));*/
                var pointToClick = new System.Windows.Point(mainWindow.Bounds.Right / 2, mainWindow.Bounds.Bottom - 25);

                mainWindow.Mouse.Location = pointToClick;
                mainWindow.Mouse.Click();
                if (message != null && message.Value.Trim() != "")
                {
                    Thread.Sleep(1000);
                    mainWindow.Mouse.Click();
                    mainWindow.Keyboard.Enter(ReplaceTokens(message.Value));
                    mainWindow.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
                }
            }
            if (node.HasChildNodes)
            {
                stopped = false;
                foreach (XmlNode subNode in node.ChildNodes)
                {
                    if (stopped) break;
                    if (subNode.Name == "click")
                    {
                        var x = subNode.Attributes["x"];
                        var y = subNode.Attributes["y"];
                        if (x == null || y == null) continue;
                        var xval = x.Value;
                        var yval = y.Value;
                        var xnegative = xval.StartsWith("-");
                        var ynegative = yval.StartsWith("-");
                        var xpositive = xval.StartsWith("+");
                        var ypositive = yval.StartsWith("+");
                        var xrelative = xval.StartsWith("%");
                        var yrelative = yval.StartsWith("%");
                        xval = xval.Replace("-", "").Replace("%", "");
                        yval = yval.Replace("-", "").Replace("%", "");
                        int xpoint = int.Parse(xval);
                        int ypoint = int.Parse(yval);
                        if (xrelative) xpoint = Convert.ToInt32(mainWindow.Bounds.Right * xpoint / 100);
                        if (yrelative) ypoint = Convert.ToInt32(mainWindow.Bounds.Bottom * ypoint / 100);
                        if (xnegative) xpoint = Convert.ToInt32(mainWindow.Bounds.Right - xpoint);
                        if (ynegative) ypoint = Convert.ToInt32(mainWindow.Bounds.Bottom - ypoint);
                        if (xpositive) xpoint = Convert.ToInt32(mainWindow.Bounds.Left + xpoint);
                        if (ypositive) ypoint = Convert.ToInt32(mainWindow.Bounds.Top + ypoint);

                        var pointToClick = new System.Windows.Point(xpoint, ypoint);

                        mainWindow.Mouse.Location = pointToClick;
                        mainWindow.Mouse.Click();


                    }
                    if (subNode.Name == "message")
                    {

                        //<message text
                        var text = subNode.Attributes["text"];

                        if (text != null && text.Value.Trim() != "")
                        {
                            Thread.Sleep(1000);
                            mainWindow.Mouse.Click();
                            mainWindow.Keyboard.Enter(ReplaceTokens(text.Value));
                            mainWindow.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
                        }
                    }
                    if (subNode.Name == "wait")
                    {
                        WaitCommand(subNode);
                    }

                }
            }
            return "";
        }




        private string MailCommand(XmlNode node)
        {
            //user, pass, search
            var user = node.Attributes["user"];
            var password = node.Attributes["pass"];
            if (user == null || password == null) return "User/password empty or not defined";

            if (!node.HasChildNodes)//no more action, just log in
            {
                return "No action specified for email!";
            }
            //more than login
            stopped = false;
            foreach (XmlNode subNode in node.ChildNodes)
            {
                if (stopped) break;
                if (subNode.Name == "search")
                {
                    var textNode = subNode.Attributes["text"];
                    if (textNode == null) continue;
                    if (textNode.Value == "") continue;
                    RunMailApi("search", "subject", ReplaceTokens(textNode.Value), ReplaceTokens(user.Value), ReplaceTokens(password.Value));

                }
                else if (subNode.Name == "searchtill")//<searchtill text=\"\" retrytimes=\"1\" retrywaitsecs=\"3\" xpath=\"\"/>
                {
                    var textNode = subNode.Attributes["text"];
                    if (textNode == null) continue;
                    if (textNode.Value == "") continue;
                    var retryTimesNode = subNode.Attributes["retrytimes"];
                    var retryTimes = int.Parse(retryTimesNode.Value);
                    var retryWaitSecsNode = subNode.Attributes["retrywaitsecs"];
                    var retryWaitSecs = int.Parse(retryWaitSecsNode.Value);

                    var xpathNode = subNode.Attributes["xpath"];
                    if (xpathNode == null) continue;
                    if (xpathNode.Value == "") continue;
                    RunMailApi("search", "subject", ReplaceTokens(textNode.Value), ReplaceTokens(user.Value), ReplaceTokens(password.Value));

                    for (int i = 0; i < retryTimes; i++)
                    {
                        var elementTag = GetCElement(subNode);
                        if (elementTag != "UNDEF") break;
                        Wait(retryWaitSecs);
                        RunMailApi("search", "subject", ReplaceTokens(textNode.Value), ReplaceTokens(user.Value), ReplaceTokens(password.Value));
                    }

                }
                else
                {
                    Run(subNode.OuterXml);
                }
            }

            return "";
        }

        //default is search by subject
        private void RunMailApi(string action, string type, string value, string user, string pass)
        {
            var botExe = Helper.AssemblyDirectory + "\\MailBot\\MailBot.exe";
            var output = Helper.StartProcess(botExe,
                                string.Format("{0} {1} {2} {3}", user, pass, type, value), true);
            //wait for bot to respond
            Wait(5);
            var htmlFile = Helper.AssemblyDirectory + "\\temp.html";
            File.WriteAllText(htmlFile, output);
            CreateCBrowser(htmlFile + "?nonce=" + Guid.NewGuid(), "");
        }


        private string WaitCommand(XmlNode node)
        {
            var waitsecs = 1000;
            var secs = node.Attributes["for"];
            if (secs != null)
            {
                var forAmount = node.Attributes["for"].Value;

                waitsecs = int.Parse(ReplaceTokens(forAmount)) * 1000;
            }
            var milisecs = node.Attributes["formilisec"];
            if (milisecs != null && milisecs.Value != "")
            {
                waitsecs = int.Parse(ReplaceTokens(milisecs.Value));
            }

            stopped = false;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                if (stopped) return "";
                Application.DoEvents();
                if (sw.ElapsedMilliseconds >= waitsecs)
                {
                    return "";

                }
            }
        }

        private string ClickCommand(XmlNode node)
        {
            var wait4browser = node.Attributes["waitforbrowser"] != null &&
                               node.Attributes["waitforbrowser"].Value == "true";
            if (wait4browser)
            {
                cloadingFinished = false;
            }
            var el = GetCClick(node);

            if (wait4browser)
            {

                Stopwatch sw = new Stopwatch();
                sw.Start();
                stopped = false;
                while (!cloadingFinished && !stopped)
                {
                    Application.DoEvents();
                    if (sw.ElapsedMilliseconds >= browsertimeoutSecs * 1000)
                    {
                        return "Timeout after secs " + browsertimeoutSecs * 1000;//timeout
                    }
                }
            }
            return el;
        }

        private string DeleteCommand(XmlNode node)
        {
            var el = DeleteElement(node);

            return el;
        }

        private string GetCSubmit(XmlNode node)
        {
            var xpath = node.Attributes["xpath"];
            if (xpath != null && xpath.Value != "")
            {
                string scr = string.Format("{1} function x(){{ if(getElementByXpath(\"{0}\")==null)  return 'UNDEF'; getElementByXpath(\"{0}\").submit();}} x(); ", ReplaceTokens(xpath.Value), FindXPathScript);
                var resp = "";
                cbrowser.EvaluateScriptAsync(scr).ContinueWith(x =>
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


        private string GetCClick(XmlNode node)
        {
            //buradayiz
            var xpath = node.Attributes["xpath"];
            if (xpath != null && xpath.Value != "")
            {
                string scr = string.Format("{1} function x(){{ if(getElementByXpath(\"{0}\")==null)  return 'UNDEF'; getElementByXpath(\"{0}\").click();}} x(); ", ReplaceTokens(xpath.Value), FindXPathScript);
                var resp = "";
                cbrowser.EvaluateScriptAsync(scr).ContinueWith(x =>
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
        private string DeleteElement(XmlNode node)
        {
            //buradayiz
            var xpath = node.Attributes["xpath"];
            if (xpath != null && xpath.Value != "")
            {
                string scr = string.Format("{1} function x(){{ if(getElementByXpath(\"{0}\")==null)  return 'UNDEF'; getElementByXpath(\"{0}\").outerHTML='';}} x(); ", ReplaceTokens(xpath.Value), FindXPathScript);
                var resp = "";
                cbrowser.EvaluateScriptAsync(scr).ContinueWith(x =>
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

        private string WaitTillCommand(XmlNode node)
        {
            var compare = node.Attributes["compare"];
            var what = node.Attributes["what"];
            var regex = node.Attributes["regex"];
            var xpath = node.Attributes["xpath"];
            var timeout = node.Attributes["timeout"];

            if (compare == null || what == null || xpath == null) return "Compare or what or xpath is not defined!";
            if (compare.Value == "" || what.Value == "" || xpath.Value == "") return "Compare or what or xpath is empty!";


            Stopwatch sw = new Stopwatch();
            var timeoutsecs = 180;
            if (timeout != null && timeout.Value != "")
            {
                int.TryParse(timeout.Value, out timeoutsecs);
            }

            sw.Start();
            string result = "";
            stopped = false;
            while (true)
            {
                Application.DoEvents();
                if (stopped) break;
                result = GetCElement(node);
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
                if (result == ReplaceTokens(compare.Value))
                {
                    break;
                }
                if (timeoutsecs > 0 && sw.ElapsedMilliseconds >= timeoutsecs * 1000)
                {
                    return "TIMEOUT While waiting...";
                }
            }
            return "";
        }

        private string GetCommand(XmlNode node)
        {
            var param = node.Attributes["param"];
            var what = node.Attributes["what"];
            var regex = node.Attributes["regex"];
            var xpath = node.Attributes["xpath"];

            if (param == null || what == null || xpath == null) return "Param or what or xpath is not defined!";
            if (param.Value == "" || what.Value == "" || xpath.Value == "") return "Param or what or xpath is empty!";
            var result = GetCElement(node);
            if (result == "UNDEF")
            {
                return "Element cannot be found!";
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

            if (!localVariables.ContainsKey(param.Value))
            {
                localVariables.Add(param.Value, result);
            }
            localVariables[param.Value] = result;
            return "";

        }

        private string SetCommand(XmlNode node)
        {
            var value = node.Attributes["value"];
            if (value == null) return "No value is defined!";
            return SetCElement(node, ReplaceTokens(value.Value));
        }
        private int browsertimeoutSecs = 60;

        private string NavigateCommand(XmlNode node)
        {

            var useProxyAttr = node.Attributes["proxy"];
            var proxy = "";
            var c_proxy = "";
            if (useProxyAttr != null)
            {
                proxy = ReplaceTokens(useProxyAttr.Value);

                if (Regex.IsMatch(proxy, @"\d+:\d+"))
                {
                    c_proxy = proxy;
                }
            }

            cloadingFinished = false;
            try
            {
                CreateCBrowser(ReplaceTokens(node.Attributes["url"].Value), c_proxy);

                Stopwatch sw = new Stopwatch();
                sw.Start();
                stopped = false;
                while (!cloadingFinished && !stopped)
                {
                    Application.DoEvents();
                    if (sw.ElapsedMilliseconds >= browsertimeoutSecs * 1000)
                    {
                        return "Timeout after secs " + browsertimeoutSecs * 1000;//timeout
                    }
                }
            }
            catch (Exception exception)
            {
                return exception.ToString();
            }
            return "";
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



        private string FindXPathScript =
            "function getElementByXpath(path) {{return document.evaluate(path, document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;}}";

        private string GetCElement(XmlNode node)
        {
            var what = node.Attributes["what"];
            var xpath = node.Attributes["xpath"];
            if (xpath != null && xpath.Value != "")
            {
                string scr = string.Format("{1} function x(){{ if(getElementByXpath(\"{0}\")==null)  return 'UNDEF'; return getElementByXpath(\"{0}\").{2}; }} x(); ", ReplaceTokens(xpath.Value), FindXPathScript, what == null ? "tagName" : what.Value);
                var resp = "";
                cbrowser.EvaluateScriptAsync(scr).ContinueWith(x =>
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
            return "NOTFOUND";
        }

        private string SetCElement(XmlNode node, string newValue)
        {
            var xpath = node.Attributes["xpath"];
            if (xpath != null && xpath.Value != "")
            {
                string scr = string.Format("{2} function x(){{ if(getElementByXpath(\"{0}\")==null)  return 'Cannot find element!'; getElementByXpath(\"{0}\").value =\"{1}\";}} x(); ", ReplaceTokens(xpath.Value), newValue, FindXPathScript);
                var resp = "";
                cbrowser.EvaluateScriptAsync(scr).ContinueWith(x =>
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

        private string ReplaceTokens(string value)
        {
            var result = value;
            var itemRegex = new Regex(@"\$\{(\w+)\}");
            foreach (Match ItemMatch in itemRegex.Matches(value))
            {
                var token = ItemMatch.Groups[1].Value;
                if (localVariables.ContainsKey(token))
                {
                    result = result.Replace(ItemMatch.ToString(), localVariables[token]);
                }
                if (token == "Clipboard")
                {
                    result = result.Replace(ItemMatch.ToString(), Clipboard.GetText());
                }

            }
            //${Random(1,10)}
            itemRegex = new Regex(@"\$\{Random\((\d+)\,(\d+)\)\}");
            foreach (Match ItemMatch in itemRegex.Matches(value))
            {
                var randFrom = ItemMatch.Groups[1].Value;
                var randTo = ItemMatch.Groups[2].Value;
                var rnd = new Random();
                var randVal = rnd.Next(int.Parse(randFrom), int.Parse(randTo));
                result = result.Replace(ItemMatch.ToString(), randVal.ToString());

            }

            return result;
        }

        private void lstUsers_SelectedValueChanged(object sender, EventArgs e)
        {
            var text = lstUsers.SelectedItem.ToString();
            if (string.IsNullOrEmpty(text)) return;
            var no = int.Parse(text.Substring(0, text.IndexOf(".")));
            ActiveUser = Users[no];
            ActiveUser.FillToDictionary(localVariables);
        }





        private string scenarioFileName = "";
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openScenarioFile.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                txtScenario.Text = File.ReadAllText(openScenarioFile.FileName);
                scenarioFileName = openScenarioFile.FileName;
                this.Text = scenarioFileName;
            }

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (scenarioFileName != "")
            {
                File.WriteAllText(scenarioFileName, txtScenario.Text);
                this.Text = scenarioFileName;
            }
            else
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = saveScenarioFile.ShowDialog();
            if (result == DialogResult.OK)
            {
                File.WriteAllText(saveScenarioFile.FileName, txtScenario.Text);
                scenarioFileName = saveScenarioFile.FileName;
                this.Text = scenarioFileName;

            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Text = "New Scenario";
            txtScenario.Text = "<?xml version=\"1.0\"?>\r\n<steps>\r\n\r\n\r\n</steps>";
            scenarioFileName = "";
            txtScenario.Select(32, 1);
            txtScenario.Focus();

        }

        private void navigateToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void navigateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<navigate url=\"\" proxy=\"\"/>";
        }

        private void setFieldToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void getFieldToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void clickToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        private void waitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<wait for=\"2\" formilisec=\"\"/>";

        }

        private void variableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${Variable}";

        }

        private void nameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserName}";

        }

        private void lastNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserLastName}";

        }

        private void mailAddressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserMail}";

        }

        private void mailPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserMailPwd}";

        }

        private void fBUsernameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserFBUser}";

        }

        private void fBPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserFBPwd}";

        }

        private void winUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserWinUser}";

        }

        private void winPassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserWinPwd}";

        }

        private void twitterNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserTwName}";

        }

        private void twitterEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserTwUserName}";
        }

        private void twitterPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserTwPwd}";

        }

        private void ethAddressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserEthAddress}";


        }

        private void ethPassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserEthPass}";

        }

        private void ethPrivateKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserEthPrivateKey}";

        }

        private void proxyIPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserProxyIp}";

        }

        private void proxyPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserProxyPort}";

        }

        private void redditUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserReddUser}";

        }

        private void redditPassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserReddPwd}";

        }

        private void btcTalkUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserBtcTalkUser}";

        }

        private void btcTalkPassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserBtcTalkPwd}";

        }

        private void btcTalkProfileLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserBtcTalkProfileLink}";

        }

        private void getFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<telegram user=\"${UserWinUser}\" pass=\"${UserWinPwd}\" group=\"\" chat=\"\">\r\n\t<click x=\"\" y=\"\"/>\r\n\t<message text=\"\"/>\r\n</telegram>";

        }

        private void clearCookiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<navigate url=\"about:blank\"/><clearcookies/>";

        }



        private void btnRunRest_Click(object sender, EventArgs e)
        {
            stopped = false;
            if (lstUsers.Items.Count == 0) return;
            foreach (var idx in lstUsers.CheckedIndices)
            {
                if (stopped) break;
                lstUsers.SelectedIndex = (int)idx;
                Thread.Sleep(1000);
                btnApplyScenario_Click(sender, e);
            }
        }

        private void llCheckAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }
        private void ChkUnChkLstUserAll(bool state)
        {
            for (int i = 0; i < lstUsers.Items.Count; i++)
            {
                lstUsers.SetItemChecked(i, state);
            }

        }

        private void llUncheckAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ChkUnChkLstUserAll(false);

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            stopped = true;
        }

        private void scrollToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //scroll height
            txtScenario.SelectedText = "<scroll height=\"100\"/>";

        }

        private void snapToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void inoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // var xnode = node.Attributes["x"];
            txtScenario.SelectedText = "<info value=\"${Variable}\"/>";

        }

        private void sendKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<sendkey value=\"\"/>";

        }

        private void clipboardValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${Clipboard}";

        }

        private void bringWindowFronToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<bringtofront/>";

        }

        private void telegramUseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserTgUser}";

        }

        private void ContentPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void showDevToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cbrowser == null) return;
            cbrowser.ShowDevTools();
        }

        private void submitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<submit xpath=\"\"/>";

        }

        private void strongPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserStrongPassword}";

        }

        private void strongPasswordWithPunctuationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserStrongPwdWithSign}";

        }

        private void focusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<focus xpath=\"\"/>";

        }

        private void createTelegramProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<createtg name=\"\" password=\"\"/>";

        }

        private void randomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${Random(1,10)}";

        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (cbrowser != null) cbrowser.Dispose();
            }
            catch
            {
            }
            try
            {
                if (!OnlyBrowser) Cef.Shutdown();
            }
            catch
            {
            }

            try
            {
                File.WriteAllText(lastScenarioFile, scenarioFileName);
            }
            catch
            {
                
            }
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserTgPhone}";

        }

        private void emptyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<set value=\"\" xpath=\"\"/>";

        }

        private void byIdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<set value=\"\" xpath=\"//*[@id='']\"/>";

        }

        private void byNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<set value=\"\" xpath=\"//*[@name='']\"/>";

        }

        private void byClassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<set value=\"\" xpath=\"//*[@class='']\"/>";

        }

        private void byTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<set value=\"\" xpath=\"//TAG\"/>";

        }

        private void emptyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<get param=\"\" what=\"\" xpath=\"\" regex=\"\"/>";

        }

        private void byIdToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<get param=\"\" what=\"\" xpath=\"//*[@id='']\" regex=\"\"/>";

        }

        private void byNameToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<get param=\"\" what=\"\" xpath=\"//*[@name='']\" regex=\"\"/>";

        }

        private void byClassToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<get param=\"\" what=\"\" xpath=\"//*[@class='']\" regex=\"\"/>";

        }

        private void byTagToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<get param=\"\" what=\"\" xpath=\"//TAG\" regex=\"\"/>";

        }

        private void waitTillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<waittill compare=\"\" what=\"\" timeout=\"0\" xpath=\"\" regex=\"\"/>";

        }

        private void failIfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<failif compare=\"\" what=\"\" xpath=\"\" regex=\"\"/>";

        }

        private void continueIfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<continueif compare=\"\" what=\"\" xpath=\"\" regex=\"\"/>";

        }

        private void byTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<set value=\"\" xpath=\"//*[text()='']\"/>";

        }

        private void byTextToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<get param=\"\" what=\"\" xpath=\"//*[text()='']\" regex=\"\"/>";

        }


        private void emptyToolStripMenuItem2_Click_1(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<click xpath=\"\" waitforbrowser=\"true\"/>";

        }

        private void byIdToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<click xpath=\"//*[@id='']\" waitforbrowser=\"true\"/>";

        }

        private void byNameToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<click xpath=\"//*[@name='']\" waitforbrowser=\"true\"/>";

        }

        private void byClassToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<click xpath=\"//*[@class='']\" waitforbrowser=\"true\"/>";

        }

        private void byTagToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<click xpath=\"//TAG\" waitforbrowser=\"true\"/>";

        }

        private void byTextToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<click xpath=\"//*[text()='']\" waitforbrowser=\"true\"/>";

        }

        private void manageUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmUsers = new FrmUsers();
            frmUsers.ShowDialog(this);
            LoadUsers();
        }

        private void LoadUsers()
        {
            if (File.Exists(Helper.UsersFile))
            {
                var userFactory = new UserFactory();
                Users = userFactory.GetUsers(Helper.UsersFile, false);
                FillUsers();
            }
        }

        private void toElementToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<snap x=\"0\" y=\"0\"/>";

        }

        private void emptyToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<snap xpath=\"\" x=\"0\" y=\"0\"/>";

        }

        private void byIdToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<snap xpath=\"//*[@id='']\" x=\"0\" y=\"0\"/>";

        }

        private void byNameToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<snap xpath=\"//*[@name='']\" x=\"0\" y=\"0\"/>";

        }

        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<snap xpath=\"//*[@class='']\" x=\"0\" y=\"0\"/>";

        }

        private void byTagToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<snap xpath=\"TAG\" x=\"0\" y=\"0\"/>";

        }

        private void byTextToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<snap xpath=\"//*[text()='']\" x=\"0\" y=\"0\"/>";

        }

        private void lstUsers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void llCheckAll_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ChkUnChkLstUserAll(true);

        }

        private void llUncheckAll_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ChkUnChkLstUserAll(false);

        }

        private void ifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<if compare=\"\" what=\"\" xpath=\"\" regex=\"\">\r\n\r\n</if>";

        }

        private void twitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<twitter user=\"${UserTwName}\" pass=\"${UserTwPwd}\" consumerkey=\"${UserTwConsumerKey}\" consumersecret=\"${UserTwConsumerSecret}\" accesstoken=\"${UserTwAccessToken}\" accesstokensecret=\"${UserTwAccessTokenSecret}\">\r\n<search text=\"\"/>\r\n<follow address=\"\"/>\r\n<like post=\"\"/>\r\n<retweet post=\"\"/>\r\n</twitter>";
        }

        private void ifnotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<ifnot compare=\"\" what=\"\" xpath=\"\" regex=\"\">\r\n\r\n</ifnot>";
        }

        private void toolStripMenuItem15_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<screenshot file=\"\"/>";
        }

        private void kucoinUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserKucoinUser}";
        }

        private void kucoinPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserKucoinPass}";
        }

        private void kucoinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<kucoin postno=\"\" formlink=\"\" twApiCust=\"${UserTwConsumerKey}\" twApiCustSec=\"${UserTwConsumerSecret}\"  twApiToken=\"${UserTwAccessToken}\" twApiTokenSec=\"${UserTwAccessTokenSecret}\" twitteruser=\"${UserTwName}\" fullname=\"${UserName} ${UserLastName}\" kucoinemail=\"${UserKucoinUser}\"/>";
        }

        private void facebookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<facebook user=\"${UserFBUser}\" pass=\"${UserFBPwd}\">\r\n<search text=\"\"/>\r\n<follow page=\"\"/>\r\n<like page=\"\"/>\r\n<like post=\"\"/>\r\n<share post=\"\"/>\r\n</facebook>";

        }

        private void toolStripMenuItem17_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserTwConsumerKey}";

        }

        private void toolStripMenuItem20_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserTwConsumerSecret}";

        }

        private void toolStripMenuItem19_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserTwAccessToken}";

        }

        private void toolStripMenuItem18_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserTwAccessTokenSecret}";

        }

        private void setFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<mail user=\"${UserMail}\" pass=\"${UserMailPwd}\">\r\n<search text=\"\"/>\r\n<searchtill text=\"\" retrytimes=\"1\" retrywaitsecs=\"3\" xpath=\"\"/>\r\n</mail>";

        }

        private void repeatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<repeat times=\"10\">\r\n\r\n<wait formilisec=\"1\"/>\r\n</repeat>";

        }

        private void byTelegramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openScenarios.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {

                txtScenario.Text = "<?xml version=\"1.0\"?>\r\n<steps>\r\n";

                foreach (var fileName in openScenarios.FileNames)
                {

                    var doc = new XmlDocument();
                    try
                    {
                        doc.LoadXml(File.ReadAllText(fileName));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Invalid xml on file : " + fileName);
                        continue;
                    }

                    var nodeList = doc.SelectNodes("/steps/telegram");
                    if (nodeList == null) continue;
                    var stepNo = 1;
                    foreach (XmlNode node in nodeList)
                    {
                        txtScenario.AppendText(node.OuterXml.Replace("<", "\r\n<"));
                        txtScenario.AppendText("\r\n\r\n");
                    }

                }
                txtScenario.AppendText("\r\n</steps>");
            }
        }

        private void openScenarios_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void zoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private double zlevel = 1.0;
        private void toolStripMenuItem23_Click(object sender, EventArgs e)
        {
            if (cbrowser == null) return;
            zlevel *= 2;
            cbrowser.SetZoomLevel(zlevel);

        }

        private void outToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cbrowser == null) return;
            zlevel /= 2;
            cbrowser.SetZoomLevel(zlevel);

        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cbrowser == null) return;
            cbrowser.Reload(true);

        }

        private void emptyToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<delete xpath=\"\"/>";

        }
    }
}
