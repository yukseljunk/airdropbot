using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using CefSharp;
using CefSharp.WinForms;
using Microsoft.Win32;
using TestStack.White.WindowsAPI;

namespace AirdropBot
{
    /// <summary>
    /// gmail calismiyor
    /// stop process de loadingwait lerde cikar
    /// </summary>
    public partial class FrmMain : Form
    {
        private ChromiumWebBrowser cbrowser;

        private Dictionary<string, string> localVariables = new Dictionary<string, string>();

        public FrmMain()
        {
            InitializeComponent();
        }

        private Dictionary<string, User> Users { get; set; }
        private User ActiveUser { get; set; }


        private void ParseCsvFile(string fileName)
        {
            var contents = File.ReadAllLines(fileName);
            foreach (var content in contents.Skip(1))
            {
                var fields = content.Split(new char[] { ';', ',' }, StringSplitOptions.None);
                if (fields.Length < 32) continue;
                var user = new User()
                               {
                                   Name = fields[1],
                                   LastName = fields[2],
                                   Mail = fields[3],
                                   MailPwd = fields[4],
                                   MailPhone = fields[27],
                                   FBUser = fields[5],
                                   FBPwd = fields[6],
                                   TwName = fields[8],
                                   TwUserName = fields[9],
                                   TwPwd = fields[10],
                                   BtcTalkUser = fields[12],
                                   BtcTalkPwd = fields[13],
                                   BtcTalkProfileLink = fields[14],
                                   WinUser = fields[15],
                                   WinPwd = fields[16],
                                   TgPhone = fields[17],
                                   TgUser = fields[18],
                                   ReddUser = fields[19],
                                   ReddPwd = fields[20],
                                   EthAddress = fields[21],
                                   EthPrivateKey = fields[22],
                                   EthPass = fields[23],
                                   ProxyIp = fields[29],
                                   ProxyPort = fields[30],
                                   StrongPassword = fields[31],
                                   StrongPwdWithSign = fields[32]

                               };

                Users.Add(user.Mail, user);
            }
            FillUsers();
        }

        private void FillUsers()
        {
            lstUsers.Items.Clear();
            foreach (var user in Users)
            {
                lstUsers.Items.Add(user.Value.Mail);
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
        }



        private void textBox1_TextChanged(object sender, EventArgs e)
        {

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
            doc.LoadXml(xml);
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

                if (command == "submit")
                {
                    commandResult = SubmitCommand(node);
                }

                if (command == "wait")
                {
                    commandResult = WaitCommand(node);
                }
                if (command == "gmail")
                {
                    commandResult = GmailCommand(node);
                }
                if (command == "gmailsignout")
                {
                    commandResult = GmailSignOutCommand(node);
                }

                if (command == "clearcookies")
                {
                    commandResult = SuppressCookiePersistence();
                }
                if (command == "telegram")
                {
                    this.WindowState = FormWindowState.Minimized;
                    commandResult = TelegramCommand(node);
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
                if (commandResult != "")
                {
                    MessageBox.Show("Error in " + command + " @" + stepNo + ".step: " + commandResult);
                    stopped = true;
                    break;
                }
                stepNo++;
            }

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
            var name = node.Attributes["name"];
            var password = node.Attributes["password"];
            if (name == null || password == null) return "name/password not defined";
            if (name.Value == "" || password.Value == "") return "name/password empty";
            RunasAdmin("C:\\code\\scripts\\CreateUser.bat",
                       string.Format("{0} {1}", ReplaceTokens(name.Value), ReplaceTokens(password.Value)));

            /*            RunasAdmin("net", string.Format("user {0} {1} /add", ReplaceTokens(name.Value), ReplaceTokens(password.Value)));
                        RunasAdmin("runas", string.Format("/env /profile /user:{0} cmd.exe", ReplaceTokens(name.Value)));
                        RunasAdmin("mkdir", string.Format("\"c:\\users\\{0}\\appdata\\roaming\\Telegram Desktop\\\"", ReplaceTokens(name.Value)));
                        RunasAdmin("copy", string.Format("\"c:\\users\\yuksel\\appdata\\roaming\\Telegram Desktop\" \"c:\\users\\{0}\\appdata\\roaming\\Telegram Desktop\\\"", ReplaceTokens(name.Value)));
            */
            /*
             * sendkeys did not work here :(
             Wait(5);

            // Get a handle to the Calculator application. The window class
            // and window name were obtained using the Spy++ tool.
            IntPtr runasHandle = FindWindow("ConsoleWindowClass", @"C:\Windows\System32\runas.exe");

            // Verify that Calculator is a running process.
            if (runasHandle == IntPtr.Zero)
            {
                return "Runas.exe is not running.";
            }

            // Make Calculator the foreground application and send it 
            // a set of calculations.
            SetForegroundWindow(runasHandle);
            SendKeys.SendWait(ReplaceTokens(password.Value) + "{ENTER}");*/


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

        private void RunasAdmin(string file, string args)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo
                                {
                                    WindowStyle = ProcessWindowStyle.Normal,
                                    FileName = file,
                                    Arguments = args,
                                    Verb = "runas"
                                };

            process.StartInfo = startInfo;
            process.Start();
        }

        private string FocusCommand(XmlNode node)
        {
            var xpath = node.Attributes["xpath"];
            if (xpath != null && xpath.Value != "")
            {
                string scr = string.Format("{1} function x(){{ if(getElementByXpath(\"{0}\")==null)  return 'UNDEF'; getElementByXpath(\"{0}\").focus();}} x(); ", xpath.Value, FindXPathScript);
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

        public void Configure(string proxy)
        {
            CefSettings cfsettings = new CefSettings();
            cfsettings.CefCommandLineArgs.Add("proxy-server", proxy);
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
            SendKeys.Send(ReplaceTokens(xnode.Value));
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
                int.TryParse(node.Attributes["x"].Value, out x);
            }
            var ynode = node.Attributes["y"];
            if (ynode != null)
            {
                int.TryParse(node.Attributes["y"].Value, out y);

            }
            MouseOperations.SetCursorPosition(this.Left + ContentPanel.Left + x, this.Top + ContentPanel.Top + y);
            MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);
            MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);

            return "";
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


        private string TelegramCommand(XmlNode node)
        {
            var user = node.Attributes["user"];
            var password = node.Attributes["pass"];
            var group = node.Attributes["group"];
            var chat = node.Attributes["chat"];
            var message = node.Attributes["message"];
            if (user == null || password == null) return "User/pass not defined";

            try
            {
                //close all instances of telegram first
                foreach (var p in Process.GetProcessesByName("Telegram"))
                {
                    p.Kill();
                }
            }
            catch
            {
            }

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            startInfo.FileName = "runas";

            //tg://join/?invite=AAAAAE7c308RYtaVAyyomw
            var extraArgs = "";
            if (group != null && group.Value != "")
            {
                extraArgs = "-- tg://resolve/?domain=" + ReplaceTokens(group.Value).Replace("?", "&");
            }
            if (chat != null && chat.Value != "")
            {
                extraArgs = "-- tg://join/?invite=" + ReplaceTokens(chat.Value).Replace("?", "&");
            }
            startInfo.Arguments =
            string.Format("/user:{0} \"C:\\Users\\{0}\\AppData\\Roaming\\Telegram Desktop\\Telegram.exe {1}\" ",
                  ReplaceTokens(user.Value),
                  extraArgs);

            process.StartInfo = startInfo;
            process.Start();
            Thread.Sleep(2000);

            // Get a handle to the Calculator application. The window class
            // and window name were obtained using the Spy++ tool.
            IntPtr runasHandle = FindWindow("ConsoleWindowClass", @"C:\Windows\system32\runas.exe");

            // Verify that Calculator is a running process.
            if (runasHandle == IntPtr.Zero)
            {
                return "Runas.exe is not running.";
            }

            // Make Calculator the foreground application and send it 
            // a set of calculations.
            SetForegroundWindow(runasHandle);
            SendKeys.SendWait(ReplaceTokens(password.Value) + "{ENTER}");
            //join or open group/send message
            if (@group != null && @group.Value.Trim() != "")
            {
                //wait for telegram to open
                Thread.Sleep(5000);

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
            return "";
        }


        private string GmailCommand(XmlNode node)
        {
            //user, pass, search
            var user = node.Attributes["user"];
            var password = node.Attributes["pass"];
            var search = node.Attributes["search"];
            if (user == null || password == null) return "User/password empty or not defined";

            var gmailTemplate = File.ReadAllText(AssemblyDirectory + "\\Templates\\GmailFind.xml");
            gmailTemplate = gmailTemplate.Replace("${0}", user.Value).Replace("${1}", password.Value).Replace("${2}", search == null ? "" : search.Value);
            Run(gmailTemplate);
            return "";
        }


        private string GmailSignOutCommand(XmlNode node)
        {
            var gmailTemplate = File.ReadAllText(AssemblyDirectory + "\\Templates\\GmailSignout.xml");
            Run(gmailTemplate);
            return "";
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
        private string WaitCommand(XmlNode node)
        {
            var waitsecs = 1;
            var secs = node.Attributes["for"];
            if (secs != null)
            {
                var forAmount = node.Attributes["for"].Value;

                waitsecs = int.Parse(ReplaceTokens(forAmount));
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                Application.DoEvents();
                if (sw.ElapsedMilliseconds >= waitsecs * 1000)
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

        private string GetCSubmit(XmlNode node)
        {
            var xpath = node.Attributes["xpath"];
            if (xpath != null && xpath.Value != "")
            {
                string scr = string.Format("{1} function x(){{ if(getElementByXpath(\"{0}\")==null)  return 'UNDEF'; getElementByXpath(\"{0}\").submit();}} x(); ", xpath.Value, FindXPathScript);
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
                string scr = string.Format("{1} function x(){{ if(getElementByXpath(\"{0}\")==null)  return 'UNDEF'; getElementByXpath(\"{0}\").click();}} x(); ", xpath.Value, FindXPathScript);
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

            if (compare == null || what == null || xpath == null) return "Compare or what or xpath is not defined!";
            if (compare.Value == "" || what.Value == "" || xpath.Value == "") return "Compare or what or xpath is empty!";

            Stopwatch sw = new Stopwatch();
            var timeoutsecs = 60;
            sw.Start();
            string result = "";
            while (true)
            {
                Application.DoEvents();
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
                if (sw.ElapsedMilliseconds >= timeoutsecs * 1000)
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
            Cef.GetGlobalCookieManager().DeleteCookies("", "");

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
                string scr = string.Format("{1} function x(){{ if(getElementByXpath(\"{0}\")==null)  return 'UNDEF'; return getElementByXpath(\"{0}\").{2}; }} x(); ", xpath.Value, FindXPathScript, what.Value);
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
                string scr = string.Format("{2} function x(){{ if(getElementByXpath(\"{0}\")==null)  return 'Cannot find element!'; getElementByXpath(\"{0}\").value =\"{1}\";}} x(); ", xpath.Value, newValue, FindXPathScript);
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
            var email = lstUsers.SelectedItem.ToString();
            if (string.IsNullOrEmpty(email)) return;
            ActiveUser = Users[email];
            ActiveUser.FillToDictionary(localVariables);
        }




        private void openUsersFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openCsvFile.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                Users = new Dictionary<string, User>();
                ParseCsvFile(openCsvFile.FileName);
            }
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
            txtScenario.SelectedText = "<set value=\"\" xpath=\"\"/>";
        }

        private void getFieldToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<get param=\"\" what=\"\" xpath=\"\" regex=\"\"/>";

        }

        private void clickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<click xpath=\"\" waitforbrowser=\"true\"/>";

        }


        private void waitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<wait for=\"2\"/>";

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
            txtScenario.SelectedText = "<telegram user=\"\" pass=\"\" group=\"\" chat=\"\" message=\"\"/>";

        }

        private void clearCookiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<navigate url=\"about:blank\"/><clearcookies/>";

        }


        // Get a handle to an application window.
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName,
            string lpWindowName);

        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

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
            ChkUnChkLstUserAll(true);
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
            txtScenario.SelectedText = "<snap x=\"0\" y=\"0\"/>";
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
            Cef.Shutdown();
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
            txtScenario.SelectedText = "<get param=\"\" what=\"\" xpath=\"*[@id='']\" regex=\"\"/>";

        }

        private void byNameToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<get param=\"\" what=\"\" xpath=\"*[@name='']\" regex=\"\"/>";

        }

        private void byClassToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<get param=\"\" what=\"\" xpath=\"*[@class='']\" regex=\"\"/>";

        }

        private void byTagToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<get param=\"\" what=\"\" xpath=\"//TAG\" regex=\"\"/>";

        }

        private void waitTillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<waittill compare=\"\" what=\"\" xpath=\"\" regex=\"\"/>";

        }

        private void failIfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<failif compare=\"\" what=\"\" xpath=\"\" regex=\"\"/>";

        }

        private void continueIfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<continueif compare=\"\" what=\"\" xpath=\"\" regex=\"\"/>";

        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<gmail user=\"\" pass=\"\"/>";

        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<gmail user=\"\" pass=\"\" search=\"\"/>";

        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<gmailsignout/>";

        }
    }
}
