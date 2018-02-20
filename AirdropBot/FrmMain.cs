using System;
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
                        if (!localVariables.ContainsKey(param.Value))
                        {
                            localVariables.Add(param.Value, result);
                        }
                        localVariables[param.Value] = result;
                    }
                }
                if (command == "click")
                {

                    var element = GetElement(node, new List<string>() { "tag", "param", "what", "waitforbrowser", "innertext" });
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
            }
        }

        private HtmlElement GetElement(XmlNode node, List<string> discardAttrs = null)
        {
            var id = node.Attributes["id"];
            if (id != null)
            {
                return browser.Document.GetElementById(id.Value);
            }
            var defaultDiscardedAttrs = new List<string>() { "value", "tag", "param", "what", "innertext" };
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

                if (tag != null)
                {
                    allSatisfied = allSatisfied && (element.TagName.ToLower() == tag.Value);
                }
                if (innertextcriterion != null && element.InnerText != null)
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
                txtScenario.Text= File.ReadAllText(openScenarioFile.FileName);
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
            txtScenario.Select(32,1);
            txtScenario.Focus();
        }
    }

}
