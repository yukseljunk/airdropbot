using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
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
using Common;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.WindowsAPI;
using log4net;

namespace AirdropBot
{

    /// <summary>
    /// stop process de loadingwait lerde cikar
    /// </summary>
    public partial class FrmMain : Form
    {
        private static readonly ILog Log =
              LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ChromiumWebBrowser cbrowser;


        public FrmMain()
        {
            InitializeComponent();
        }

        private User ActiveUser { get; set; }


        public bool OnlyBrowser { get; set; }
        public string Scenario { get; set; }

        private void FillUsers()
        {
            lstUsers.Items.Clear();
            foreach (var user in Helper.Users)
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
            catch { }
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

                FillTemplates();
                return;
            }
            btnStop.Enabled = false;
            LoadUsers();
            RestoreLastScenario();
            RestoreLastSelectedUser();
            string[] args = Environment.GetCommandLineArgs();
            if (args.Count() > 1)
            {
                foreach (var i in args.Skip(1))
                {
                    lstUsers.SetItemChecked(int.Parse(i), true);
                }
                Thread.Sleep(1000);
                btnRunRest_Click(null, null);
            }
            FillTemplates();
            FillUserProps();
        }

        private void FillTemplates()
        {
            var templateDir = string.Format("{0}\\NewTemplates", CommonHelper.AssemblyDirectory);
            foreach (var templateFile in Directory.EnumerateFiles(templateDir, "*.xml"))
            {
                var onlyFileName = Path.GetFileNameWithoutExtension(templateFile);
                var NLMenu = new ToolStripMenuItem(onlyFileName);
                callTemplateToolStripMenuItem.DropDownItems.Add(NLMenu);
                NLMenu.Click += NLMenu_Click;
            }
        }

        private void FillUserProps()
        {
            foreach (PropertyInfo prop in typeof(User).GetProperties())
            {
                var NLMenu = new ToolStripMenuItem(prop.Name);
                otherUserToolStripMenuItem.DropDownItems.Add(NLMenu);
                NLMenu.Click += UserMenu_Click;
            }
        }

        private void UserMenu_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${User1" + ((ToolStripMenuItem)sender).Text + "}";
        }


        private void NLMenu_Click(object sender, EventArgs e)
        {
            var name = ((ToolStripMenuItem)sender).Text;
            var templateFile = string.Format("{0}\\NewTemplates\\{1}.xml", CommonHelper.AssemblyDirectory, name);
            if (!File.Exists(templateFile))
            {
                MessageBox.Show("cannot find template under newtemplates folder: " + name);
                return;
            }
            var parameters = new List<string>();
            var template = File.ReadAllText(templateFile);
            var itemRegex = new Regex(@"\$\{(\w+)\}");
            foreach (Match ItemMatch in itemRegex.Matches(template))
            {
                var token = ItemMatch.Groups[1].Value;
                if (!Helper.Variables.ContainsKey(token))
                {
                    parameters.Add(token);
                }
            }
            parameters.Add("");
            var paramXml = string.Join("=\"\" ", parameters);
            txtScenario.SelectedText = "<template name=\"" + name + "\" " + paramXml + "/>";
        }

        private void RestoreLastSelectedUser()
        {
            if (File.Exists(lastUserFile))
            {
                var lastUserIndex = File.ReadAllText(lastUserFile);
                if (lastUserIndex != "")
                {
                    try
                    {
                        lstUsers.SelectedIndex = int.Parse(lastUserIndex);
                        lstUsers.TopIndex = lstUsers.SelectedIndex;
                    }
                    catch
                    {
                    }
                }
            }
        }

        private string lastScenarioFile = CommonHelper.AssemblyDirectory + @"\\lastsc.txt";
        private string lastUserFile = CommonHelper.AssemblyDirectory + @"\\lastusr.txt";
        private void RestoreLastScenario()
        {
            if (File.Exists(lastScenarioFile))
            {
                var lastScFile = File.ReadAllText(lastScenarioFile);
                if (File.Exists(lastScFile))
                {
                    txtScenario.Text = File.ReadAllText(lastScFile);
                    scenarioFileName = lastScFile;
                    this.Text = scenarioFileName;

                }
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (scenarioFileName != "")
            {
                if (!this.Text.EndsWith("*"))
                {
                    this.Text += "*";
                }
            }
        }

        private bool cloadingFinished = false;

        private bool stopped = false;
        private void btnApplyScenario_Click(object sender, EventArgs e)
        {
            Log.Info("Scenario started...");
            if (txtScenario.Text == "") return;
            EnDis(true);
            stopped = false;
            var scenario = txtScenario.Text;
            if (txtScenario.SelectionLength > 0)
            {
                scenario = txtScenario.SelectedText;
            }
            try
            {
                Run(scenario);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
                EnDis(false);

            }
            EnDis(false);

        }

        private void EnDis(bool state)
        {
            btnStop.Enabled = state;
            btnApplyScenario.Enabled = !state;
            btnRunRest.Enabled = !state;

        }

        private string Run(string cmd, bool showErrorBox = true)
        {
            var xml = cmd;
            if (lstUsers.SelectedItem != null)
            {
                xml = xml.Replace("${UserIndex}", (lstUsers.SelectedIndex + 1).ToString());
            }

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
                return "";
            }
            var nodeList = doc.SelectNodes("/steps/*");
            if (nodeList == null) return "";
            var stepNo = 1;
            foreach (XmlNode node in nodeList)
            {
                if (breakCame) return "";

                var command = node.Name.ToLower();
                if (command != "") Debug.WriteLine(command + " " + DateTime.Now.ToString("hh:mm:ss"));
                if (stopped) break;
                var commandResult = "";

                var commandFactory = new CommandFactory();
                commandFactory.CBrowser = cbrowser;
                var commandClass = commandFactory.GetCommand(command);
                if (commandClass != null)
                {
                    commandResult = commandClass.Command(node);
                }
                if (command == "navigate")
                {
                    commandResult = NavigateCommand(node);
                }
                if (command == "restart")
                {
                    commandResult = RestartCommand(node);
                }
                if (command == "try")
                {
                    commandResult = TryCommand(node);
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
                if (command == "log")
                {
                    commandResult = LogCommand(node);
                }
                if (command == "sendkey")
                {
                    commandResult = SendKeyCommand(node);
                }
                if (command == "bringtofront")
                {
                    commandResult = BringToFrontCommand(node);
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
                if (command == "recaptcha")
                {
                    commandResult = RecaptchaCommand(node);
                }
                if (command == "screenshot")
                {
                    commandResult = ScreenShotCommand(node);
                }
                if (command == "template")
                {
                    commandResult = CallTemplate(node);
                }
                if (command == "setstatuscolor")
                {
                    commandResult = SetStatusColor(node);
                }
                if (command == "xif")
                {
                    commandResult = XIfCommand(node);

                }
                if (command == "break")
                {
                    commandResult = BreakCommand(node);
                    return "";

                }
                if (command == "continue")
                {
                    commandResult = ContinueCommand(node);
                }

                if (commandResult != "")
                {
                    var res = "Error in " + command + " @" + stepNo + ".step: " + commandResult +
                              "\r\nCommand is : \r\n" + node.OuterXml;
                    if (showErrorBox) MessageBox.Show(res);
                    Log.Error(res);
                    stopped = true;
                    return res;
                }
                stepNo++;
            }
            return "";
        }

        private string TryCommand(XmlNode node)
        {

            var runResult = Run(node.InnerXml, false);
            if (!Helper.Variables.ContainsKey("Exception"))
            {
                Helper.Variables.Add("Exception", runResult);
            }
            Helper.Variables["Exception"] = runResult;
            if (node.NextSibling != null && node.NextSibling.Name == "catch")
            {
                stopped = false;
                Run(node.NextSibling.InnerXml);
            }
            return "";
        }

        private bool continueCame = false;
        private string ContinueCommand(XmlNode node)
        {
            continueCame = true;
            return "";
        }

        private string BreakCommand(XmlNode node)
        {
            breakCame = true;
            return "";
        }

        private string SetStatusColor(XmlNode node)
        {
            //code
            var code = node.Attributes["code"];
            this.BackColor = Color.LightGray;
            if (code == null)
            {
                return "";
            }
            if (string.IsNullOrEmpty(code.Value))
            {
                return "";
            }
            this.BackColor = Color.FromName(code.Value);
            return "";
        }


        private string CallTemplate(XmlNode node)
        {
            var name = node.Attributes["name"];
            if (name == null) return "template name not defined";
            if (string.IsNullOrEmpty(name.Value)) return "template name cannot be empty!";
            var parameters = new Dictionary<string, string>();
            foreach (XmlAttribute attr in node.Attributes)
            {
                if (attr.Name == "name" || attr.Name == "alias") continue;
                parameters.Add(attr.Name, Helper.ReplaceTokens(attr.Value));
            }
            var templateFile = string.Format("{0}\\NewTemplates\\{1}.xml", CommonHelper.AssemblyDirectory, name.Value);
            if (!File.Exists(templateFile)) return "cannot find template under newtemplates folder: " + name.Value;

            var template = File.ReadAllText(templateFile);
            foreach (var parameter in parameters)
            {
                template = template.Replace("${" + parameter.Key + "}", parameter.Value);
            }
            Run(template);

            return "";

        }

        private string RecaptchaCommand(XmlNode node)
        {
            var xpath = node.Attributes["xpath"];
            if (xpath == null) return "xpath not defined";
            if (string.IsNullOrEmpty(xpath.Value)) return "xpath empty";

            bool invisibleCaptcha = false;
            var invisible = node.Attributes["invisible"];
            if (invisible != null && invisible.Value != "true")
            {
                invisibleCaptcha = true;
            }
            var gcaptcha = new Gcaptcha();

            XmlAttribute att = node.OwnerDocument.CreateAttribute("what");
            att.InnerText = "getAttribute('data-sitekey')";
            node.Attributes.Append(att);

            var sitekey = GetCElement(node);

            if (sitekey == "UNDEF" || sitekey == "")
            {

                //if no data-sitekey defined, take it from src
                node.Attributes.Remove(att);

                XmlAttribute attSrc = node.OwnerDocument.CreateAttribute("what");
                attSrc.InnerText = "getAttribute('src')";
                node.Attributes.Append(attSrc);
                /*
                XmlAttribute attRegex = node.OwnerDocument.CreateAttribute("regex");
                attRegex.InnerText = "k=(.*)&";
                node.Attributes.Append(attRegex);*/

                var source = GetCElement(node);
                if (source == "UNDEF" || source == "")
                {
                    return "Sitekey cannot be found! Source cannot be found!";
                }
                var reg = new Regex("k=([^&]*)");
                var match = reg.Match(source);
                if (match.Success)
                {
                    if (match.Groups.Count > 1)
                    {
                        sitekey = match.Groups[1].Value;
                    }
                }
            }
            node.Attributes.Remove(att);

            XmlAttribute att2 = node.OwnerDocument.CreateAttribute("what");
            att2.InnerText = "getAttribute('data-callback')";
            node.Attributes.Append(att2);

            var callback = GetCElement(node);

            if (callback == "UNDEF")
            {
                //submit form by hand???

                // return "Sitekey cannot be found!";
            }


            var cid = gcaptcha.SendRecaptchav2Request(sitekey, cbrowser.Address, invisibleCaptcha);

            string token = "";
            do
            {
                Wait(20);
                token = gcaptcha.GetToken(cid);
                if (token != "CAPCHA_NOT_READY") break;


            } while (true);
            if (token.StartsWith("ERROR"))
            {
                return token;
            }
            token = token.Substring(3);//remove ok|

            string scr = string.Format(" function x(){{ document.getElementById(\"g-recaptcha-response\").innerHTML=\"{0}\";}} x(); ", token);
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

            //submit form somehow

            if (callback == "UNDEF") return "";

            Wait(2);

            scr = string.Format(" {0}(); ", callback);
            resp = "";
            cbrowser.EvaluateScriptAsync(scr).ContinueWith(x =>
            {
                var response = x.Result;

                if (response.Success && response.Result != null)
                {
                    resp = response.Result.ToString();
                    //startDate is the value of a HTML element.
                }
            }).Wait();

            return "";

        }

        private string RestartCommand(XmlNode node)
        {
            if (lstUsers.SelectedIndex != -1)
            {
                lstUsers.SetItemChecked(lstUsers.SelectedIndex, false);
            }
            StartAnotherInstanceWithCheckedItems();
            return "";
        }
        private bool breakCame = false;

        private string RepeatCommand(XmlNode node)
        {
            var times = node.Attributes["times"];
            if (times == null) return "Repeat times is not defined!";
            if (times.Value == "") return "Repeat times is empty!";
            var variable = node.Attributes["variable"];
            var iterVariable = "";
            if (variable != null)
            {
                iterVariable = variable.Value;
            }
            for (int i = 0; i < int.Parse(Helper.ReplaceTokens(times.Value)); i++)
            {
                if (breakCame)
                {
                    breakCame = false;
                    break;
                }
                if (stopped)
                {
                    break;
                }
                if (continueCame)
                {
                    continueCame = false;
                    continue;
                }
                if (!Helper.Variables.ContainsKey(iterVariable))
                {
                    Helper.Variables.Add(iterVariable, i.ToString());
                }
                Helper.Variables[iterVariable] = i.ToString();
                Run(node.InnerXml);
            }
            return "";
        }

        private string ScreenShotCommand(XmlNode node)
        {
            var fileNode = node.Attributes["file"];
            var fileName = "c:\\temp\\test.jpg";
            if (fileNode != null && fileNode.Value != "")
            {
                fileName = Helper.ReplaceTokens(fileNode.Value);
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
            if (compare == null) return "Compare is not defined!";
            if (compare.Value == "") return "Compare is empty!";
            var itemValue = GetItemValue(node);
            if (!itemValue.Item1)
            {
                return itemValue.Item2;
            }
            if (itemValue.Item2 == Helper.ReplaceTokens(compare.Value)) return "";
            Run(node.InnerXml);
            return "";
        }

        private string IfCommand(XmlNode node)
        {
            var compare = node.Attributes["compare"];
            if (compare == null) return "Compare is not defined!";
            if (compare.Value == "") return "Compare is empty!";
            var itemValue = GetItemValue(node);
            if (!itemValue.Item1)
            {
                return itemValue.Item2;
            }
            if (itemValue.Item2 != Helper.ReplaceTokens(compare.Value)) return "";
            Run(node.InnerXml);
            return "";
        }

        private Tuple<bool, string> GetItemValue(XmlNode node, bool returnErrorWhenNotFound = false)
        {
            var what = node.Attributes["what"];
            var regex = node.Attributes["regex"];
            var xpath = node.Attributes["xpath"];
            if (what == null || xpath == null) return new Tuple<bool, string>(false, "What or xpath is not defined!");
            if (what.Value == "" || xpath.Value == "") return new Tuple<bool, string>(false, "What or xpath is empty!");
            var result = GetCElement(node);
            if (result == "UNDEF" && returnErrorWhenNotFound)
            {
                return new Tuple<bool, string>(false, "Element cannot be found!");
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
            return new Tuple<bool, string>(true, result);
        }

        private string XIfCommand(XmlNode node)
        {
            var test = node.Attributes["test"];

            if (test == null) return "test is not defined!";
            if (test.Value == "") return "test is empty!";

            var result = Helper.Evaluate(Helper.ReplaceTokens(test.Value)).ToLower();

            if (new List<string>() { "1", "t", "true", "y", "yes" }.Contains(result))//criteria met
            {
                Run(node.InnerXml);
            }

            return "";
        }

        private string ContinueIfCommand(XmlNode node)
        {
            var compare = node.Attributes["compare"];
            if (compare == null) return "Compare is not defined!";
            if (compare.Value == "") return "Compare is empty!";
            var itemValue = GetItemValue(node);
            if (!itemValue.Item1)
            {
                return itemValue.Item2;
            }
            return itemValue.Item2 != Helper.ReplaceTokens(compare.Value) ? "Criteria not met, not continuing..." : "";
        }

        private string FailIfCommand(XmlNode node)
        {
            var compare = node.Attributes["compare"];
            if (compare == null) return "Compare is not defined!";
            if (compare.Value == "") return "Compare is empty!";
            var itemValue = GetItemValue(node);
            if (!itemValue.Item1)
            {
                return itemValue.Item2;
            }
            return itemValue.Item2 == Helper.ReplaceTokens(compare.Value) ? "Criteria met, failing" : "";
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

            string txt = Regex.Replace(Helper.ReplaceTokens(xnode.Value), "[+^%~()]", "{$0}");

            SendKeys.Send(txt);
            return "";
        }

        private string LogCommand(XmlNode node)
        {
            var xnode = node.Attributes["message"];
            if (xnode == null) return "";
            Log.Info(Helper.ReplaceTokens(xnode.Value));
            return "";
        }
        private string InfoCommand(XmlNode node)
        {
            var xnode = node.Attributes["value"];
            if (xnode == null) return "";
            MessageBox.Show(Helper.ReplaceTokens(xnode.Value));
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



                string scr = string.Format("{1} function x(){{ if(getElementByXpath(\"{0}\")==null)  return 'Cannot find element!'; var rect= getElementByXpath(\"{0}\").getBoundingClientRect(); return rect.top + ':'+ rect.right+ ':'+ rect.bottom+ ':'+ rect.left;}} x(); ", Helper.ReplaceTokens(xpath.Value), FindXPathScript);
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

            MouseOperations.SetCursorPosition(this.Left + ContentPanel.Left + splitContainer1.Left + splitContainer1.Panel2.Left + x, this.Top + splitContainer1.Top + ContentPanel.Top + y + tabBrowser.Top + 20);
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


        private void RunTemplate(string templateName, params  string[] args)
        {
            var template = File.ReadAllText(CommonHelper.AssemblyDirectory + "\\Templates\\" + templateName + ".xml");
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
            var apiCommands = new HashSet<string>() { "follow", "like", "retweet", "retweetc" };
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
                        RunTemplate("TwitterSearch", Helper.ReplaceTokens(textNode.Value));

                    }
                    else if (subNode.Name == "follow")
                    {
                        var addressNode = subNode.Attributes["address"];
                        if (addressNode == null) continue;
                        if (addressNode.Value == "") continue;

                        RunTweetApi("follow", Helper.ReplaceTokens(addressNode.Value), consumerkey.Value, consumersecret.Value, accesstoken.Value, accesstokensecret.Value);
                    }
                    else if (subNode.Name == "like")
                    {
                        var postNode = subNode.Attributes["post"];
                        if (postNode == null) continue;
                        if (postNode.Value == "") continue;
                        RunTweetApi("like", Helper.ReplaceTokens(postNode.Value), consumerkey.Value, consumersecret.Value, accesstoken.Value, accesstokensecret.Value);
                    }
                    else if (subNode.Name == "retweet")
                    {
                        //https://twitter.com/JonErlichman/status/971536891924439040
                        var postNode = subNode.Attributes["post"];
                        if (postNode == null) continue;
                        if (postNode.Value == "") continue;
                        RunTweetApi("retweetc", Helper.ReplaceTokens(postNode.Value), consumerkey.Value, consumersecret.Value, accesstoken.Value, accesstokensecret.Value);
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

            var botExe = CommonHelper.AssemblyDirectory + "\\TwitterBot\\TwitterBot.exe";
            var output = CommonHelper.StartProcess(botExe,
                                string.Format("{0} {1} {2} {3} {4} {5}", Helper.ReplaceTokens(consumerkey), Helper.ReplaceTokens(consumersecret), Helper.ReplaceTokens(accesstoken),
                                              Helper.ReplaceTokens(accesstokensecret), command, commandarg), true);
            //wait for bot to respond
            Wait(5);
            Debug.WriteLine(output);

            var htmlFile = CommonHelper.AssemblyDirectory + "\\temp2.html";
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
                        RunTemplate("FBSearch", Helper.ReplaceTokens(textNode.Value));

                    }
                    else if (subNode.Name == "follow")
                    {
                        var pageNod = subNode.Attributes["page"];
                        if (pageNod == null) continue;
                        if (pageNod.Value == "") continue;
                        var url = Helper.ReplaceTokens(pageNod.Value);
                        if (!url.StartsWith("http")) url = "https://www.facebook.com/" + url;
                        RunTemplate("FBFollow", url);
                    }
                    else if (subNode.Name == "like")
                    {
                        var pageNode = subNode.Attributes["page"];
                        if (pageNode != null)
                        {
                            if (pageNode.Value == "") continue;
                            var url = Helper.ReplaceTokens(pageNode.Value);
                            if (!url.StartsWith("http")) url = "https://www.facebook.com/" + url;
                            RunTemplate("FBLikePage", url);
                        }
                        var postNode = subNode.Attributes["post"];
                        if (postNode != null)
                        {
                            if (postNode.Value == "") continue;
                            var url = Helper.ReplaceTokens(postNode.Value);
                            if (!url.StartsWith("http")) url = "https://www.facebook.com/" + url;
                            RunTemplate("FBLikePost", url);
                        }

                    }
                    else if (subNode.Name == "share")
                    {

                        var postNode = subNode.Attributes["post"];
                        if (postNode == null) continue;
                        if (postNode.Value == "") continue;
                        var url = Helper.ReplaceTokens(postNode.Value);
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
            if (!Helper.ReplaceTokens(user.Value).StartsWith("m_"))
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

            Common.Rect location = new Common.Rect();
            var memuRes = Helper.OpenTelegramMemu(Helper.ReplaceTokens(user.Value), Helper.ReplaceTokens(password.Value), url,
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
                            File.Copy(CommonHelper.AssemblyDirectory + "\\Templates\\MemuTgMessage1.txt", scenarioFile, true);

                            var msg = Helper.ReplaceTokens(text.Value);
                            for (int i = 0; i < msg.Length; i++)
                            {
                                File.AppendAllText(scenarioFile, (50000 + i * 100000) + "--CLIPBOARD--" + msg[i] + "\r\n");
                            }
                            var template2 = File.ReadAllText(CommonHelper.AssemblyDirectory + "\\Templates\\MemuTgMessage2.txt");
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
                extraArgs = "-- tg://resolve/?domain=" + Helper.ReplaceTokens(groupName).Replace("?", "&");
            }
            if (chat != null && chat.Value != "")
            {
                var chatName = chat.Value;
                if (chatName.StartsWith("http"))
                {
                    chatName = new Uri(chatName).PathAndQuery.Substring(1);
                }
                extraArgs = "-- tg://join/?invite=" + Helper.ReplaceTokens(chatName).Replace("?", "&");
            }

            if (Helper.OpenTelegram(Helper.ReplaceTokens(user.Value), extraArgs, Helper.ReplaceTokens(password.Value)) != "") return "Runas exe is not running!";

            var pid = GetPid(Helper.ReplaceTokens(user.Value));
            //butun telegramlari kapat bu pid haricinde
            try
            {
                //close all instances of telegram first
                foreach (var p in Process.GetProcessesByName("Telegram"))
                {
                    if (p.Id == pid) continue;
                    p.Kill();
                }
            }
            catch
            {
            }

            Window mainWindow = null;
            int waitsecs = 5;
            int maxTryCount = 5;
            int tryCount = 0;
            var windowFound = false;
            while (tryCount < maxTryCount)
            {
                tryCount++;
                TestStack.White.Application app = TestStack.White.Application.Attach(@"Telegram");
                if (app.GetWindows().Count > 0)
                {
                    mainWindow = app.GetWindows()[0];
                    mainWindow.DisplayState = TestStack.White.UIItems.WindowItems.DisplayState.Maximized;
                    windowFound = true;
                    break;
                }

                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (true)
                {
                    Application.DoEvents();
                    if (sw.ElapsedMilliseconds >= waitsecs)
                    {
                        break;
                    }
                }
            }
            if (!windowFound)
            {
                return "Telegram window could not be found by testwhite!";
            }

            var pointToClick = new System.Windows.Point(mainWindow.Bounds.Right / 2, mainWindow.Bounds.Bottom - 25);
            mainWindow.Mouse.Location = pointToClick;

            var search = "[69][69][69][69][69][69][69][69]0000000000000";
            var search2 = "000000000000000000000000000000000000000000000000000000000000000000000000000000000000000099999999999";


            Tuple<int, int, int> ltb = null;
            int right = 0;

            //join or open group/send message
            if (@group != null && @group.Value.Trim() != "")
            {
                /*Process p = Process.GetProcessesByName("Telegram")[0];

                SetForegroundWindow(p.Handle); access denied*/
                /*Debug.WriteLine(string.Format("{0} {1} {2} {3}", mainWindow.Bounds.Top, mainWindow.Bounds.Left,
                                              mainWindow.Bounds.Bottom, mainWindow.Bounds.Right));*/

                using (Bitmap bitmap = CaptureScreen(true, (int)mainWindow.Bounds.Right, (int)mainWindow.Bounds.Bottom, 0, 0))
                {
                    var h = GetHash(bitmap);
                    ltb = FindMost(h.ToArray(), search);
                    var r = FindMost(h.ToArray(), search2, search2.Length - 10);
                    right = r.Item1;
                    if (r.Item1 < 1)
                    {
                        right = (int)mainWindow.Bounds.Right;
                    }

                    stopped = false;
                    Stopwatch sw = new Stopwatch();
                    sw.Start();

                    var wsecs = 30000;//30sec timeout

                    while (ltb.Item2 == -1 || ltb.Item2 < 30)
                    {
                        Thread.Sleep(1000);

                        using (Bitmap bitmap2 = CaptureScreen(true, (int)mainWindow.Bounds.Right, (int)mainWindow.Bounds.Bottom, 0, 0))
                        {
                            var h2 = GetHash(bitmap2);
                            ltb = FindMost(h2.ToArray(), search);
                            r = FindMost(h2.ToArray(), search2, search2.Length - 10);
                        }
                        if (stopped) return "";
                        Application.DoEvents();
                        if (sw.ElapsedMilliseconds >= wsecs)
                        {
                            return "Timeout opening telegram group!";
                        }
                    }
                    right = r.Item1;
                    if (r.Item1 < 1)
                    {
                        right = (int)mainWindow.Bounds.Right;
                    }

                    pointToClick = new System.Windows.Point((ltb.Item1 + right) / 2, ltb.Item3 + 20);
                    mainWindow.Mouse.Location = pointToClick;

                }
                mainWindow.Mouse.Click();


                if (message != null && message.Value.Trim() != "")
                {
                    Thread.Sleep(1000);
                    mainWindow.Mouse.Click();
                    mainWindow.Keyboard.Enter(Helper.ReplaceTokens(message.Value));
                    mainWindow.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
                }
            }
            if (chat != null && chat.Value != "")
            {

                using (Bitmap bitmap = CaptureScreen(true, (int)mainWindow.Bounds.Right, (int)mainWindow.Bounds.Bottom, 0, 0))
                {
                    var h = GetWhiteBox(bitmap);
                    ltb = FindMost(h.ToArray(), "111111111111111111111000000000000000000000000", 20);

                    stopped = false;
                    Stopwatch sw = new Stopwatch();
                    sw.Start();

                    var wsecs = 30000; //30sec timeout

                    while (ltb.Item2 == -1)
                    {
                        Thread.Sleep(1000);

                        using (Bitmap bitmap2 = CaptureScreen(true, (int)mainWindow.Bounds.Right, (int)mainWindow.Bounds.Bottom, 0, 0))
                        {
                            var h2 = GetWhiteBox(bitmap2);


                            ltb = FindMost(h2.ToArray(), search);
                        }
                        if (stopped) return "";
                        Application.DoEvents();
                        if (sw.ElapsedMilliseconds >= wsecs)
                        {
                            return "Timeout opening telegram chat!";
                        }
                    }

                    pointToClick = new System.Windows.Point(ltb.Item1 - 20, ltb.Item3 - 20);
                    mainWindow.Mouse.Location = pointToClick;
                    mainWindow.Mouse.Click();

                    Thread.Sleep(2000);
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
                        var xval = x.Value.Trim();
                        var yval = y.Value.Trim();
                        if (xval == "") xval = "%50";
                        if (yval == "") yval = "-35";

                        var xnegative = xval.StartsWith("-");
                        var ynegative = yval.StartsWith("-");
                        var xpositive = xval.StartsWith("+");
                        var ypositive = yval.StartsWith("+");
                        var xrelative = xval.StartsWith("%");
                        var yrelative = yval.StartsWith("%");
                        xval = xval.Replace("-", "").Replace("%", "");
                        yval = yval.Replace("-", "").Replace("%", "");
                        int xpoint = 0;
                        int.TryParse(xval, out xpoint);
                        int ypoint = 0;
                        int.TryParse(yval, out ypoint);

                        if (xrelative) xpoint = Convert.ToInt32(mainWindow.Bounds.Right * xpoint / 100);
                        if (yrelative) ypoint = Convert.ToInt32(mainWindow.Bounds.Bottom * ypoint / 100);
                        if (xnegative) xpoint = Convert.ToInt32(mainWindow.Bounds.Right - xpoint);
                        if (ynegative) ypoint = Convert.ToInt32(mainWindow.Bounds.Bottom - ypoint);
                        if (xpositive) xpoint = Convert.ToInt32(mainWindow.Bounds.Left + xpoint);
                        if (ypositive) ypoint = Convert.ToInt32(mainWindow.Bounds.Top + ypoint);

                        pointToClick = new System.Windows.Point(xpoint, ypoint);

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
                            mainWindow.Keyboard.Enter(Helper.ReplaceTokens(text.Value));
                            mainWindow.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
                        }
                    }
                    if (subNode.Name == "waitforresponse")
                    {
                        var wsecs = 30000; //30sec timeout

                        var timeout = subNode.Attributes["timeout"];

                        if (timeout != null && timeout.Value.Trim() != "")
                        {
                            wsecs = int.Parse(timeout.Value.Trim()) * 1000;
                        }
                        var capw = right - ltb.Item1 - 200;
                        if (capw < 100) capw = 300;
                        using (Bitmap bitmapSub = CaptureScreen(false, capw, 200, ltb.Item1 + 200, ltb.Item3 - 200))
                        {
                            bitmapSub.Save(@"c:\temp\bitmapsub.jpg", ImageFormat.Jpeg);
                            var hsub = GetHash(bitmapSub);


                            stopped = false;
                            Stopwatch sw = new Stopwatch();
                            sw.Start();

                            var index = 0;
                            while (true)
                            {
                                index++;

                                using (Bitmap bitmapSubSub = CaptureScreen(false, capw, 200, ltb.Item1 + 200, ltb.Item3 - 200))
                                {
                                    bitmapSubSub.Save(@"c:\temp\bitmapsubsub" + index + ".jpg", ImageFormat.Jpeg);

                                    var hsubsub = GetHash(bitmapSubSub);
                                    var nonequalct = 0;
                                    var equal = true;
                                    for (int i = 0; i < hsub.Count; i++)
                                    {
                                        if (hsub[i] != hsubsub[i])
                                        {
                                            nonequalct++;
                                        }
                                    }
                                    if (nonequalct > 50)
                                    {
                                        break; //%75 resemblance is OK
                                    }

                                }
                                Thread.Sleep(50);

                                if (stopped) return "";
                                Application.DoEvents();
                                if (sw.ElapsedMilliseconds >= wsecs)
                                {
                                    return "Timeout opening while waiting for response!";
                                }
                            }


                        }


                    }
                    if (subNode.Name == "detectbox")
                    {
                        Thread.Sleep(1000);

                        using (Bitmap bitmap = CaptureScreen(true, (int)mainWindow.Bounds.Right, (int)mainWindow.Bounds.Bottom, 0, 0))
                        {
                            var h = GetHash(bitmap);
                            ltb = FindMost(h.ToArray(), search);
                            var r = FindMost(h.ToArray(), search2, search2.Length - 10);
                            right = r.Item1;
                            if (r.Item1 < 1)
                            {
                                right = (int)mainWindow.Bounds.Right;
                            }
                            for (int i = ltb.Item2; i < ltb.Item3; i += 10)
                            {
                                pointToClick = new System.Windows.Point(ltb.Item1, i);
                                mainWindow.Mouse.Location = pointToClick;
                                Thread.Sleep(10);
                            }
                            for (int i = ltb.Item1; i < right; i += 10)
                            {
                                pointToClick = new System.Windows.Point(i, ltb.Item3);
                                mainWindow.Mouse.Location = pointToClick;
                                Thread.Sleep(10);
                            }
                            for (int i = ltb.Item3; i > ltb.Item2; i -= 10)
                            {
                                pointToClick = new System.Windows.Point(right, i);
                                mainWindow.Mouse.Location = pointToClick;
                                Thread.Sleep(10);
                            }
                            for (int i = right; i > ltb.Item1; i -= 10)
                            {
                                pointToClick = new System.Windows.Point(i, ltb.Item2);
                                mainWindow.Mouse.Location = pointToClick;
                                Thread.Sleep(10);
                            }

                        }


                    }
                    if (subNode.Name == "wait")
                    {
                        WaitCommand(subNode);
                    }

                }
            }
            mainWindow.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.ESCAPE);

            return "";
        }

        private static Tuple<int, int, int> FindMost(string[] readLines, string search, int offset = 8)
        {
            var found = new Dictionary<int, int>();
            var lineNo = 0;
            var top = -1;
            var bottom = -1;
            foreach (var line in readLines)
            {
                lineNo++;
                var x = new Regex(search);
                foreach (Match match in x.Matches(line))
                {
                    var foundIndex = match.Index;
                    foundIndex += offset;
                    if (!found.ContainsKey(foundIndex))
                    {
                        found.Add(foundIndex, 0);
                    }
                    found[foundIndex]++;
                    bottom = lineNo;
                    if (top == -1) top = lineNo;
                }
            }
            int left = -1;
            if (found.Any())
            {
                var maxval = found.Values.Max();
                var maxCol = found.Where(x => x.Value == maxval).OrderBy(x => x.Key);
                left = int.MaxValue;
                foreach (var keyValuePair in maxCol)
                {
                    if (left > keyValuePair.Key)
                    {
                        left = keyValuePair.Key;
                    }
                }
            }
            return new Tuple<int, int, int>(left, top, bottom);
        }

        public static List<string> GetWhiteBox(Bitmap bmpMin)
        {
            var lResult = new List<string>();
            //create new image with 16x16 pixel
            for (int j = 0; j < bmpMin.Height; j++)
            {
                var t = new List<int>();
                for (int i = 0; i < bmpMin.Width; i++)
                {
                    //reduce colors to true / false             
                    var br = bmpMin.GetPixel(i, j).GetBrightness();
                    if (br > 0.99999f)
                    {
                        t.Add(1);
                    }
                    else
                    {
                        t.Add(0);
                    }

                }
                lResult.Add(string.Join("", t));
            }
            return lResult;
        }


        public static List<string> GetHash(Bitmap bmpMin)
        {
            var lResult = new List<string>();
            //create new image with 16x16 pixel
            for (int j = 0; j < bmpMin.Height; j++)
            {
                var t = new List<int>();
                for (int i = 0; i < bmpMin.Width; i++)
                {
                    //reduce colors to true / false             
                    var br = bmpMin.GetPixel(i, j).GetBrightness();
                    if (br < 0.1f)
                    {
                        t.Add(0);
                    }
                    else if (br < 0.2f)
                    {
                        t.Add(2);
                    }
                    else if (br < 0.3f)
                    {
                        t.Add(3);
                    }
                    else if (br < 0.4f)
                    {
                        t.Add(4);
                    }
                    else if (br < 0.5f)
                    {
                        t.Add(5);
                    }
                    else if (br < 0.6f)
                    {
                        t.Add(6);
                    }
                    else if (br < 0.7f)
                    {
                        t.Add(7);
                    }
                    else if (br < 0.8f)
                    {
                        t.Add(8);
                    }
                    else
                    {
                        t.Add(9);
                    }
                }
                lResult.Add(string.Join("", t));
            }
            return lResult;
        }

        private int GetPid(string tgUserName)
        {
            var pid = 0;
            Dictionary<string, int> tgUsrs = new Dictionary<string, int>();
            tgUsrs = CommonHelper.GetProcessUsers("Telegram.exe");
            if (tgUsrs.ContainsKey(tgUserName))
            {
                pid = tgUsrs[tgUserName];
            }
            return pid;
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
            var variable = "";
            var regex = "";
            var variableNode = node.Attributes["variable"];
            var regexNode = node.Attributes["regex"];
            if (variableNode != null && !string.IsNullOrEmpty(variableNode.Value))
            {
                variable = variableNode.Value.Trim();
            }
            if (regexNode != null && !string.IsNullOrEmpty(regexNode.Value))
            {
                regex = regexNode.Value;
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

                    var typeNode = subNode.Attributes["type"];
                    var type = "subject";
                    if (typeNode != null && typeNode.Value != "")
                    {
                        type = typeNode.Value;
                    }

                    RunMailApi("search", type, Helper.ReplaceTokens(textNode.Value), Helper.ReplaceTokens(user.Value), Helper.ReplaceTokens(password.Value), variable, regex);

                }
                else if (subNode.Name == "searchtill")//<searchtill text=\"\" retrytimes=\"1\" retrywaitsecs=\"3\"/>
                {
                    var textNode = subNode.Attributes["text"];
                    if (textNode == null) continue;
                    if (textNode.Value == "") continue;
                    var retryTimesNode = subNode.Attributes["retrytimes"];
                    var retryTimes = int.Parse(retryTimesNode.Value);
                    var retryWaitSecsNode = subNode.Attributes["retrywaitsecs"];
                    var retryWaitSecs = int.Parse(retryWaitSecsNode.Value);
                    var typeNode = subNode.Attributes["type"];
                    var type = "subject";
                    if (typeNode != null && typeNode.Value != "")
                    {
                        type = typeNode.Value;
                    }
                    stopped = false;
                    for (int i = 0; i < retryTimes; i++)
                    {
                        if (RunMailApi("search", type, Helper.ReplaceTokens(textNode.Value), Helper.ReplaceTokens(user.Value),
                                   Helper.ReplaceTokens(password.Value), variable, regex)) break;
                        Wait(retryWaitSecs);
                        if (stopped) break;
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
        private bool RunMailApi(string action, string type, string value, string user, string pass, string variable, string regex)
        {
            var botExe = CommonHelper.AssemblyDirectory + "\\MailBot\\MailBot.exe";
            var output = CommonHelper.StartProcess(botExe,
                                string.Format("{0} {1} {2} {3}", user, pass, type, value), true);
            //wait for bot to respond
            Wait(5);
            if (variable != "")
            {
                if (!Helper.Variables.ContainsKey(variable))
                {
                    Helper.Variables.Add(variable, "");
                }


                if (regex != "")
                {
                    var reg = new Regex(regex);
                    var match = reg.Match(output);
                    if (match.Success)
                    {
                        if (match.Groups.Count > 1)
                        {
                            output = match.Groups[1].Value;
                        }
                    }
                }

                Helper.Variables[variable] = output;

                return !output.StartsWith("No email found for ");
            }
            var htmlFile = CommonHelper.AssemblyDirectory + "\\temp.html";
            File.WriteAllText(htmlFile, output);
            CreateCBrowser(htmlFile + "?nonce=" + Guid.NewGuid(), "");
            return !output.StartsWith("No email found for ");
        }


        private string WaitCommand(XmlNode node)
        {
            var waitsecs = 1000;
            var secs = node.Attributes["for"];
            if (secs != null)
            {
                var forAmount = node.Attributes["for"].Value;

                waitsecs = int.Parse(Helper.ReplaceTokens(forAmount)) * 1000;
            }
            var milisecs = node.Attributes["formilisec"];
            if (milisecs != null && milisecs.Value != "")
            {
                waitsecs = int.Parse(Helper.ReplaceTokens(milisecs.Value));
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


        private string GetCSubmit(XmlNode node)
        {
            var xpath = node.Attributes["xpath"];
            if (xpath != null && xpath.Value != "")
            {
                string scr = string.Format("{1} function x(){{ if(getElementByXpath(\"{0}\")==null)  return 'UNDEF'; getElementByXpath(\"{0}\").submit();}} x(); ", Helper.ReplaceTokens(xpath.Value), FindXPathScript);
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
                if (!cbrowser.IsBrowserInitialized) return "";
                string scr = string.Format("{1} function x(){{ if(getElementByXpath(\"{0}\")==null)  return 'UNDEF'; getElementByXpath(\"{0}\").click();}} x(); ", Helper.ReplaceTokens(xpath.Value), FindXPathScript);
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
                if (result == Helper.ReplaceTokens(compare.Value))
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

            if (param == null) return "Param is not defined!";
            if (param.Value == "") return "Param is empty!";

            var result = GetItemValue(node, true);
            if (!result.Item1) return result.Item2;
            if (!Helper.Variables.ContainsKey(param.Value))
            {
                Helper.Variables.Add(param.Value, result.Item2);
            }
            Helper.Variables[param.Value] = result.Item2;
            return "";

        }

        private string SetCommand(XmlNode node)
        {
            var value = node.Attributes["value"];
            if (value == null) return "No value is defined!";
            return SetCElement(node, Helper.ReplaceTokens(value.Value));
        }
        private int browsertimeoutSecs = 60;

        private string NavigateCommand(XmlNode node)
        {

            var useProxyAttr = node.Attributes["proxy"];
            var proxy = "";
            var c_proxy = "";
            if (useProxyAttr != null)
            {
                proxy = Helper.ReplaceTokens(useProxyAttr.Value);

                if (Regex.IsMatch(proxy, @"\d+:\d+"))
                {
                    c_proxy = proxy;
                }
            }

            var stopElementXpath = "";
            var stopWhenElementRendered = node.Attributes["stopWhenElementRendered"];
            if (stopWhenElementRendered != null && !string.IsNullOrEmpty(stopWhenElementRendered.Value))
            {
                stopElementXpath = stopWhenElementRendered.Value;
            }

            cloadingFinished = false;
            try
            {
                var url = Helper.ReplaceTokens(node.Attributes["url"].Value);
                CreateCBrowser(url, c_proxy);
                tabBrowser.TabPages[0].Text = "Navigating to " + url + " ...";
                Stopwatch sw = new Stopwatch();
                sw.Start();
                stopped = false;
                while (!cloadingFinished && !stopped)
                {
                    Application.DoEvents();
                    //check every sec for stopelemenrender
                    if (stopElementXpath != "" && sw.ElapsedMilliseconds > 0 && sw.ElapsedMilliseconds % 1000 == 0)
                    {
                        if (ElementExists(stopElementXpath))
                        {
                            tabBrowser.TabPages[0].Text = url;

                            return "";
                        }
                    }

                    if (sw.ElapsedMilliseconds >= browsertimeoutSecs * 1000)
                    {
                        return "Timeout after secs " + browsertimeoutSecs * 1000;//timeout
                    }
                }
                tabBrowser.TabPages[0].Text = url;
            }
            catch (Exception exception)
            {
                return exception.ToString();
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
                if (!cbrowser.IsBrowserInitialized) return "UNDEF";
                string scr = string.Format("{1} function x(){{ if(getElementByXpath(\"{0}\")==null)  return 'UNDEF'; return getElementByXpath(\"{0}\").{2}; }} x(); ", Helper.ReplaceTokens(xpath.Value), FindXPathScript, what == null ? "tagName" : what.Value);
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

        private bool ElementExists(string xpath)
        {
            if (string.IsNullOrEmpty(xpath)) return false;
            if (!cbrowser.IsBrowserInitialized) return false;

            string scr = string.Format("{1} function x(){{ if(getElementByXpath(\"{0}\")==null)  return 'UNDEF'; return 'DEF'; }} x(); ", xpath, FindXPathScript);
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

            return resp == "DEF";

        }

        private string SetCElement(XmlNode node, string newValue)
        {
            var xpath = node.Attributes["xpath"];
            if (xpath != null && xpath.Value != "")
            {
                string scr = string.Format("{2} function x(){{ if(getElementByXpath(\"{0}\")==null)  return 'Cannot find element!'; getElementByXpath(\"{0}\").value =\"{1}\";}} x(); ", Helper.ReplaceTokens(xpath.Value), newValue, FindXPathScript);
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


        private void lstUsers_SelectedValueChanged(object sender, EventArgs e)
        {
            var text = lstUsers.SelectedItem.ToString();
            if (string.IsNullOrEmpty(text)) return;
            var no = int.Parse(text.Substring(0, text.IndexOf(".")));
            ActiveUser = Helper.Users[no];
            ActiveUser.FillToDictionary(Helper.Variables);
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
            txtScenario.SelectedText = "<navigate url=\"\" proxy=\"\" stopWhenElementRendered=\"\" />";
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
            txtScenario.SelectedText = "<telegram user=\"${UserWinUser}\" pass=\"${UserWinPwd}\" group=\"\" chat=\"\">\r\n\t<click x=\"\" y=\"\"/>\r\n\t<message text=\"\"/>\r\n<waitforresponse timeout=\"30\"/>\r\n<detectbox/>\r\n</telegram>";

        }

        private void clearCookiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = GetHtmlForCommand("clearcookies");

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
            txtScenario.SelectedText = GetHtmlForCommand("scroll");
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
            txtScenario.SelectedText = GetHtmlForCommand("focus");

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
            try
            {
                if (lstUsers.SelectedIndex != -1)
                {
                    File.WriteAllText(lastUserFile, lstUsers.SelectedIndex.ToString());
                }
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

        private void SetGetCommand(string xpath)
        {
            txtScenario.SelectedText = "<get param=\"\" what=\"\" xpath=\"" + xpath + "\" regex=\"\"/>";
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
            if (frmUsers.IsEdited)
            {
                LoadUsers();
            }
        }

        private void LoadUsers()
        {
            FillUsers();
        }

        private void toElementToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<snap x=\"%50\" y=\"%150\"/>";

        }

        private void emptyToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<snap xpath=\"\" x=\"%50\" y=\"%150\"/>";

        }

        private void byIdToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<snap xpath=\"//*[@id='']\" x=\"%50\" y=\"%150\"/>";

        }

        private void byNameToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<snap xpath=\"//*[@name='']\" x=\"%50\" y=\"%150\"/>";

        }

        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<snap xpath=\"//*[@class='']\" x=\"%50\" y=\"%150\"/>";

        }

        private void byTagToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<snap xpath=\"TAG\" x=\"%50\" y=\"%150\"/>";

        }

        private void byTextToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<snap xpath=\"//*[text()='']\" x=\"%50\" y=\"%150\"/>";

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
            txtScenario.SelectedText = "<mail user=\"${UserMail}\" pass=\"${UserMailPwd}\" variable=\"\" regex=\"\">\r\n<search text=\"\" type=\"subject\"/>\r\n<searchtill text=\"\" type=\"subject\" retrytimes=\"3\" retrywaitsecs=\"10\"/>\r\n</mail>";

        }

        private void repeatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<repeat variable=\"i\" times=\"10\">\r\n\r\n<wait formilisec=\"1\"/>\r\n\r\n<break/>\r\n</repeat>";

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
            txtScenario.SelectedText = GetHtmlForCommand("delete");
        }


        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartAnotherInstanceWithCheckedItems();
        }

        private void StartAnotherInstanceWithCheckedItems()
        {
            var exe = CommonHelper.AssemblyDirectory + "\\AirdropBot.exe";
            CommonHelper.StartProcess(exe, GetArgList());
            Thread.Sleep(100);
            this.Close();
        }

        private string GetArgList()
        {
            if (lstUsers.SelectedIndex == -1) return "";
            var arglist = new List<string>();
            foreach (var i in lstUsers.CheckedIndices)
            {
                arglist.Add(i.ToString());
            }
            if (!arglist.Any()) return "";
            return string.Join(" ", arglist);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem24_Click(object sender, EventArgs e)
        {

            txtScenario.SelectedText = "${UserFBProfile}";

        }

        private void toolStripMenuItem26_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserNeoAddress}";
        }


        [StructLayout(LayoutKind.Sequential)]
        struct CURSORINFO
        {
            public Int32 cbSize;
            public Int32 flags;
            public IntPtr hCursor;
            public POINTAPI ptScreenPos;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct POINTAPI
        {
            public int x;
            public int y;
        }

        [DllImport("user32.dll")]
        static extern bool GetCursorInfo(out CURSORINFO pci);

        [DllImport("user32.dll")]
        static extern bool DrawIcon(IntPtr hDC, int X, int Y, IntPtr hIcon);

        const Int32 CURSOR_SHOWING = 0x00000001;

        public static Bitmap CaptureScreen(bool CaptureMouse, int w, int h, int x, int y)
        {
            Bitmap result = new Bitmap(w, h, PixelFormat.Format24bppRgb);

            try
            {
                using (Graphics g = Graphics.FromImage(result))
                {
                    g.CopyFromScreen(new Point(x, y), Point.Empty, new Size(w, h), CopyPixelOperation.SourceCopy);


                    if (CaptureMouse)
                    {
                        CURSORINFO pci;
                        pci.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(CURSORINFO));

                        if (GetCursorInfo(out pci))
                        {
                            if (pci.flags == CURSOR_SHOWING)
                            {
                                DrawIcon(g.GetHdc(), pci.ptScreenPos.x, pci.ptScreenPos.y, pci.hCursor);
                                g.ReleaseHdc();
                            }
                        }
                    }
                }
            }
            catch
            {
                result = null;
            }

            return result;
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<restart/>";

        }

        private void randomTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${RandomText(text1,text2,text3)}";

        }


        private void google2FAToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            txtScenario.SelectedText = GetHtmlForCommand("google2fa");
        }

        private void recaptchaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<recaptcha invisible=\"false\" xpath=\"//*[@id='recaptcha']\"/>";

        }

        private void setVariableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = GetHtmlForCommand("setparam");
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void evaluateExpressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${Eval(3+5*2)}";
        }

        private void toolStripMenuItem25_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${UserIndex}";

        }

        private void callTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void writeToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void randomExceptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${RandomExcept(1,10,5)}";

        }

        private void nameToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "${User1Name}";

        }

        private void lastNameToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void databaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = GetHtmlForCommand("database");

        }

        private string GetHtmlForCommand(string command)
        {
            var commandFactory = new CommandFactory();
            var commandClass = commandFactory.GetCommand(command);
            if (commandClass != null) return commandClass.Html;
            return "";
        }


        private void setStatusColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<setstatuscolor code=\"lightgreen\"/>";

        }

        private void toolStripMenuItem28_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = GetHtmlForCommand("setattr");
        }

        private void xIfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<xif test=\"\">\r\n\r\n</xif>";

        }

        private void toolStripMenuItem30_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = GetHtmlForCommand("readhtml");

        }

        private void toolStripMenuItem31_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = GetHtmlForCommand("getfromhtml");

        }

        private void writeToFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = GetHtmlForCommand("writetofile");

        }

        private void readFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = GetHtmlForCommand("readfile");

        }

        private void deleteFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = GetHtmlForCommand("deletefile");

        }

        private void waitForFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = GetHtmlForCommand("waitforfile");

        }

        private void tryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<try>\r\n\r\n\r\n</try>\r\n<catch>\r\n<info value=\"${Exception}\"/>\r\n</catch>";
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtScenario.SelectedText = "<log message=\"\"/>";

        }
    }
}
