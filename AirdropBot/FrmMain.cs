﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Common;
using TestStack.White.UIItems;
using TestStack.White.WindowsAPI;

namespace AirdropBot
{
    public partial class FrmMain : Form, IOleClientSite, IServiceProvider, IAuthenticate
    {
        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption,
            IntPtr lpBuffer, int lpdwBufferLength);
        private Guid IID_IAuthenticate = new Guid("79eac9d0-baf9-11ce-8c82-00aa004ba90b");
        private const int INET_E_DEFAULT_ACTION = unchecked((int)0x800C0011);
        private const int S_OK = unchecked((int)0x00000000);
        private const int INTERNET_OPTION_PROXY = 38;
        private const int INTERNET_OPEN_TYPE_DIRECT = 1;
        private const int INTERNET_OPEN_TYPE_PROXY = 3;
        private string _currentUsername;
        private string _currentPassword;

        private Dictionary<string, string> localVariables = new Dictionary<string, string>();
        private Dictionary<string, string> attrTranslations = new Dictionary<string, string>() { { "for", "htmlfor" }, { "class", "className" } };

        public FrmMain()
        {
            InitializeComponent();
        }

        private void SetProxyServer(string proxy)
        {
            //Create structure
            INTERNET_PROXY_INFO proxyInfo = new INTERNET_PROXY_INFO();

            if (proxy == null)
            {
                proxyInfo.dwAccessType = INTERNET_OPEN_TYPE_DIRECT;
            }
            else
            {
                proxyInfo.dwAccessType = INTERNET_OPEN_TYPE_PROXY;
                proxyInfo.proxy = Marshal.StringToHGlobalAnsi(proxy);
                proxyInfo.proxyBypass = Marshal.StringToHGlobalAnsi("local");
            }

            // Allocate memory
            IntPtr proxyInfoPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(proxyInfo));

            // Convert structure to IntPtr
            Marshal.StructureToPtr(proxyInfo, proxyInfoPtr, true);
            bool returnValue = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_PROXY,
                proxyInfoPtr, Marshal.SizeOf(proxyInfo));
        }
        private Dictionary<string, User> Users { get; set; }
        private User ActiveUser { get; set; }
        private void btnOpenInputFile_Click(object sender, EventArgs e)
        {

        }

        private void ParseCsvFile(string fileName)
        {
            var contents = File.ReadAllLines(fileName);
            foreach (var content in contents.Skip(1))
            {
                var fields = content.Split(new char[] { ';', ',' }, StringSplitOptions.None);
                if (fields.Length < 3) continue;
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
                                   ProxyPort = fields[30]

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

        private const string GMAILBOT_OUTPUT = "c:\\temp\\gmailbot3.html";
        private void StartGoogleBot(string args, bool checkOutput = false)
        {
            if (File.Exists(GMAILBOT_OUTPUT)) File.Delete(GMAILBOT_OUTPUT);

            foreach (var p in Process.GetProcessesByName("Gmailbot"))
            {
                p.Kill();
            }
            var process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = "Gmailbot.exe";
            process.StartInfo.Arguments = args;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;

            process.Start();

            this.Enabled = false;
            //process.WaitForExit();// Waits here for the process to exit.
            var checkCount = 0;
            var maxTry = 60;
            var sleep = 1000;
            while (!File.Exists(GMAILBOT_OUTPUT))
            {
                checkCount++;
                if (checkCount > maxTry) break;
                Thread.Sleep(sleep);
            }
            this.Enabled = true;

        }

        private bool gmailbotrefreshed = false;
        private void browser_document_completed(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (e.Url.AbsolutePath != (sender as WebBrowser).Url.AbsolutePath)
                return;
            Debug.WriteLine("loadingfinished " + DateTime.Now.ToString("hh:mm:ss"));
            loadingFinished = true;
            HtmlDocument doc = browser.Document;
            if (!gmailbotrefreshed && e.Url.ToString().ToLower().Contains(GMAILBOT_OUTPUT.Replace("\\", "/")))
            {
                browser.Refresh();
                Thread.Sleep(1500);
                browser.Refresh();
                Thread.Sleep(1500);
                browser.Refresh();
                gmailbotrefreshed = true;
            }


        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            if (!WBEmulator.IsBrowserEmulationSet())
            {
                WBEmulator.SetBrowserEmulationVersion();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WBEmulator.SetBrowserEmulationVersion();
            Thread.Sleep(2000);

            browser.Navigate("https://stackoverflow.com/questions/18808990/get-current-webbrowser-dom-as-html");
            browser.DocumentCompleted += browser_document_completed;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnFindInEmail_Click(object sender, EventArgs e)
        {
            var content = File.ReadAllText(GMAILBOT_OUTPUT);

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private bool loadingFinished = false;

        private void btnApplyScenario_Click(object sender, EventArgs e)
        {
            if (txtScenario.Text == "") return;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(txtScenario.Text);
            var nodeList = doc.SelectNodes("/steps/*");
            if (nodeList == null) return;
            foreach (XmlNode node in nodeList)
            {
                var command = node.Name.ToLower();
                if (command != "") Debug.WriteLine(command + " " + DateTime.Now.ToString("hh:mm:ss"));

                if (command == "navigate")
                {
                    object obj = browser.ActiveXInstance;
                    IOleObject oc = obj as IOleObject;
                    oc.SetClientSite(this as IOleClientSite);

                    WinInetHelper.EndBrowserSession();
                    WinInetHelper.SupressCookiePersist();
                    Thread.Sleep(1000);
                    SetProxyServer(null);
                    Thread.Sleep(1000);

                    var useProxyAttr = node.Attributes["proxy"];
                    if (useProxyAttr != null)
                    {
                        var proxy = ReplaceTokens(useProxyAttr.Value);

                        if (Regex.IsMatch(proxy, @"\d+:\d+"))
                        {
                            WinInetHelper.EndBrowserSession();
                            WinInetHelper.SupressCookiePersist();
                            Thread.Sleep(1000);
                            SetProxyServer(proxy);
                            Thread.Sleep(100);

                        }
                    }

                    loadingFinished = false;
                    browser.DocumentCompleted += browser_document_completed;

                    browser.Navigate(node.Attributes["url"].Value);
                    while (!loadingFinished)
                    {
                        Application.DoEvents();
                    }
                }
                if (command == "set")
                {

                    var value = node.Attributes["value"];
                    if (value == null) continue;
                    HtmlElement element = GetElement(node);
                    if (element != null) element.SetAttribute("value", ReplaceTokens(value.Value));
                }

                if (command == "get")
                {

                    var param = node.Attributes["param"];
                    var what = node.Attributes["what"];
                    var regex = node.Attributes["regex"];
                    if (param == null || what == null) continue;
                    var element = GetElement(node);
                    if (element != null)
                    {
                        var result = "";
                        switch (what.Value.ToLower())
                        {
                            case "value":
                                result = element.GetAttribute("value");
                                break;
                            case "innertext":
                                result = element.InnerText;
                                break;
                            case "outertext":
                                result = element.OuterText;
                                break;
                            case "innerhtml":
                                result = element.InnerHtml;
                                break;
                            case "outerhtml":
                                result = element.OuterHtml;
                                break;
                        }

                        if (regex!=null && regex.Value!="")
                        {
                            var reg = new Regex(regex.Value);
                            var match = reg.Match(result);
                            if(match.Success)
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
                    }
                }
                if (command == "click")
                {

                    var element = GetElement(node, new List<string>() { "tag", "param", "what", "waitforbrowser", "innertext","regex" });
                    if (element != null)
                    {
                        var wait4browser = node.Attributes["waitforbrowser"] != null && node.Attributes["waitforbrowser"].Value == "true";
                        if (wait4browser)
                        {
                            loadingFinished = false;
                            browser.DocumentCompleted += browser_document_completed;
                        }
                        element.InvokeMember("click");
                        if (wait4browser)
                        {
                            while (!loadingFinished)
                            {
                                Application.DoEvents();
                            }
                        }

                    }
                }

                if (command == "wait")
                {

                    var waitsecs = 1;
                    var secs = node.Attributes["for"];
                    if (secs != null) waitsecs = int.Parse(node.Attributes["for"].Value);
                    Thread.Sleep(1000 * waitsecs);

                }
                if (command == "gmail")
                {

                    //user, pass, search
                    var user = node.Attributes["user"];
                    var password = node.Attributes["pass"];
                    var search = node.Attributes["search"];
                    var maxtries = 1;
                    var maxtry = node.Attributes["maxtry"];
                    if (user == null || password == null) return;
                    if (maxtry != null) maxtries = int.Parse(maxtry.Value);
                    if (search == null)
                    {
                        StartGoogleBot(ReplaceTokens(user.Value) + " " + ReplaceTokens(password.Value));
                    }
                    else
                    {
                        StartGoogleBot(ReplaceTokens(user.Value) + " " + ReplaceTokens(password.Value) + " Find \"" + search.Value + "\" \"" + GMAILBOT_OUTPUT + "\" " + maxtries.ToString());

                        gmailbotrefreshed = false;
                        browser.Navigate("file:///" + GMAILBOT_OUTPUT.Replace("\\", "/") + "?nonce=" + Guid.NewGuid());
                        browser.DocumentCompleted += browser_document_completed;
                        while (!gmailbotrefreshed)
                        {
                            Application.DoEvents();

                        }
                    }
                }
                if (command == "clearcookies")
                {
                    SuppressCookiePersistence();
                }
                if (command == "telegram")
                {
                    //<telegram user=\"\" pass=\"\" group=\"\" message=\"\"/>
                    var user = node.Attributes["user"];
                    var password = node.Attributes["pass"];
                    var group = node.Attributes["group"];
                    var message = node.Attributes["message"];
                    if (user == null || password == null) return;

                    //close all instances of telegram first
                    foreach (var p in Process.GetProcessesByName("Telegram"))
                    {
                        p.Kill();
                    }

                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                    startInfo.FileName = "runas";
                    startInfo.Arguments = string.Format("/user:{0} \"C:\\Users\\{0}\\AppData\\Roaming\\Telegram Desktop\\Telegram.exe {1}\" ", ReplaceTokens(user.Value), group == null ? "" : "-- tg://resolve/?domain=" + ReplaceTokens(group.Value));
                    process.StartInfo = startInfo;
                    process.Start();
                    Thread.Sleep(2000);

                    // Get a handle to the Calculator application. The window class
                    // and window name were obtained using the Spy++ tool.
                    IntPtr runasHandle = FindWindow("ConsoleWindowClass", @"C:\Windows\system32\runas.exe");

                    // Verify that Calculator is a running process.
                    if (runasHandle == IntPtr.Zero)
                    {
                        MessageBox.Show("runas.exe is not running.");
                        return;
                    }

                    // Make Calculator the foreground application and send it 
                    // a set of calculations.
                    SetForegroundWindow(runasHandle);
                    SendKeys.SendWait(ReplaceTokens(password.Value) + "{ENTER}");
                    //join or open group/send message
                    if (group != null && group.Value.Trim() != "")
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
                        Debug.WriteLine(string.Format("{0} {1} {2} {3}", mainWindow.Bounds.Top, mainWindow.Bounds.Left,
                                                      mainWindow.Bounds.Bottom, mainWindow.Bounds.Right));
                        mainWindow.Mouse.Location = new System.Windows.Point(mainWindow.Bounds.Right / 2, mainWindow.Bounds.Bottom - 25);
                        mainWindow.Mouse.Click();
                        if (message != null && message.Value.Trim() != "")
                        {
                            Thread.Sleep(1000);
                            mainWindow.Mouse.Click();
                            mainWindow.Keyboard.Enter(ReplaceTokens(message.Value));
                            mainWindow.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
                        }




                    }

                }
            }
        }
        private static void SuppressCookiePersistence()
        {
            int flag = INTERNET_SUPPRESS_COOKIE_PERSIST;
            if (!InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SUPPRESS_BEHAVIOR, ref flag, sizeof(int)))
            {
                var ex = Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
                throw ex;
            }
        }

        const int INTERNET_OPTION_SUPPRESS_BEHAVIOR = 81;
        const int INTERNET_SUPPRESS_COOKIE_PERSIST = 3;

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, ref int flag, int dwBufferLength);

        private HtmlElement GetElement(XmlNode node, List<string> discardAttrs = null)
        {
            var id = node.Attributes["id"];
            if (id != null)
            {
                return browser.Document.GetElementById(id.Value);
            }
            var defaultDiscardedAttrs = new List<string>() { "value", "tag", "param", "regex", "what", "innertext" };
            if (discardAttrs == null)
            {
                discardAttrs = defaultDiscardedAttrs;
            }
            var tag = node.Attributes["tag"];
            var innertextcriterion = node.Attributes["innertext"];
            var attrs = new Dictionary<string, string>();
            foreach (XmlAttribute attr in node.Attributes)
            {
                if (discardAttrs.Contains(attr.Name.ToLower())) continue;
                attrs.Add(attr.Name, attr.Value);
            }
            var allElements = browser.Document.All;
            foreach (HtmlElement element in allElements)
            {
                var allSatisfied = true;
                foreach (KeyValuePair<string, string> attr in attrs)
                {
                    var attrName = attr.Key;
                    if (attrTranslations.ContainsKey(attrName)) attrName = attrTranslations[attrName];
                    if (element.GetAttribute(attrName) != attr.Value)
                    {
                        allSatisfied = false;
                        break;
                    }
                }

                if (tag != null && !string.IsNullOrEmpty(tag.Value))
                {
                    allSatisfied = allSatisfied && (element.TagName.ToLower() == tag.Value);
                }
                if (innertextcriterion != null && !string.IsNullOrEmpty(innertextcriterion.Value) && element.InnerText != null)
                {
                    allSatisfied = allSatisfied && (element.InnerText.Trim() == innertextcriterion.Value.Trim());
                }
                if (allSatisfied)
                {
                    return element;
                }

            }
            return null;

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


        #region IOleClientSite Members

        public void SaveObject()
        {
            // TODO:  Add Form1.SaveObject implementation
        }

        public void GetMoniker(uint dwAssign, uint dwWhichMoniker, object ppmk)
        {
            // TODO:  Add Form1.GetMoniker implementation
        }

        public void GetContainer(object ppContainer)
        {
            ppContainer = this;
        }

        public void ShowObject()
        {
            // TODO:  Add Form1.ShowObject implementation
        }

        public void OnShowWindow(bool fShow)
        {
            // TODO:  Add Form1.OnShowWindow implementation
        }

        public void RequestNewObjectLayout()
        {
            // TODO:  Add Form1.RequestNewObjectLayout implementation
        }

        #endregion

        #region IServiceProvider Members

        public int QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject)
        {
            int nRet = guidService.CompareTo(IID_IAuthenticate);
            if (nRet == 0)
            {
                nRet = riid.CompareTo(IID_IAuthenticate);
                if (nRet == 0)
                {
                    ppvObject = Marshal.GetComInterfaceForObject(this, typeof(IAuthenticate));
                    return S_OK;
                }
            }

            ppvObject = new IntPtr();
            return INET_E_DEFAULT_ACTION;
        }

        #endregion

        #region IAuthenticate Members

        public int Authenticate(ref IntPtr phwnd, ref IntPtr pszUsername, ref IntPtr pszPassword)
        {
            IntPtr sUser = Marshal.StringToCoTaskMemAuto(_currentUsername);
            IntPtr sPassword = Marshal.StringToCoTaskMemAuto(_currentPassword);

            pszUsername = sUser;
            pszPassword = sPassword;
            return S_OK;
        }

        #endregion

        private void lstUsers_SelectedIndexChanged(object sender, EventArgs e)
        {

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
            txtScenario.SelectedText = "<set value=\"\" id=\"\" name=\"\" class=\"\" tag=\"\"/>";
        }

        private void getFieldToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<get param=\"\" what=\"\" id=\"\" name=\"\" regex=\"\" class=\"\" tag=\"\"/>";

        }

        private void clickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<click id=\"\" name=\"\" class=\"\" tag=\"\" waitforbrowser=\"true\"/>";

        }

        private void setFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<gmail user=\"\" pass=\"\" search=\"\" maxtry=\"\"/>";

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
            txtScenario.SelectedText = "<telegram user=\"\" pass=\"\" group=\"\" message=\"\"/>";

        }

        private void clearCookiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<clearcookies/>";

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
            if (lstUsers.Items.Count == 0) return;
            var selectedIndex = lstUsers.SelectedIndex;
            if (selectedIndex == -1) return;
            for (int i = selectedIndex; i < lstUsers.Items.Count; i++)
            {
                lstUsers.SelectedIndex = i;
                Thread.Sleep(1000);
                btnApplyScenario_Click(sender, e);
            }
        }
    }

}
