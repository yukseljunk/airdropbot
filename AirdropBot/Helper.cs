using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Common;

namespace AirdropBot
{
    public class Helper
    {
        public static Dictionary<string, string> Variables
        {
            get { return localVariables; }
        }
        private static Dictionary<string, string> localVariables = new Dictionary<string, string>();

        private static Dictionary<int, User> _users;
        public static Dictionary<int, User> Users
        {
            get
            {
                if (_users != null) return _users;
                if (File.Exists(CommonHelper.UsersFile))
                {
                    var userFactory = new UserFactory();
                    _users = userFactory.GetUsers(CommonHelper.UsersFile, false);
                }
                return _users;
            }
        }

        public static string ReplaceTokens(string value)
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
            //${UserIndex}
            //${Random(1,10)}
            itemRegex = new Regex(@"\$\{Random\((\d+)\,(\d+)\)\}");
            foreach (Match ItemMatch in itemRegex.Matches(result))
            {
                var randFrom = ItemMatch.Groups[1].Value;
                var randTo = ItemMatch.Groups[2].Value;
                var rnd = new Random();
                var randVal = rnd.Next(int.Parse(randFrom), int.Parse(randTo));
                result = result.Replace(ItemMatch.ToString(), randVal.ToString());

            }
            //${RandomExcept(1,10,5)}
            itemRegex = new Regex(@"\$\{RandomExcept\((\d+)\,(\d+)\,(\d+)\)\}");
            foreach (Match ItemMatch in itemRegex.Matches(result))
            {
                var randFrom = ItemMatch.Groups[1].Value;
                var randTo = ItemMatch.Groups[2].Value;
                var randExcept = int.Parse(ItemMatch.Groups[3].Value);
                var rnd = new Random();
                var randVal = rnd.Next(int.Parse(randFrom), int.Parse(randTo));
                while (randVal == randExcept)
                {
                    randVal = rnd.Next(int.Parse(randFrom), int.Parse(randTo));
                }
                result = result.Replace(ItemMatch.ToString(), randVal.ToString());
            }

            //${RandomText(ali,veli,kirkk doggkuz)}
            itemRegex = new Regex(@"\$\{RandomText\(([^\)]*)\)\}");
            foreach (Match ItemMatch in itemRegex.Matches(result))
            {
                var randStrs = ItemMatch.Groups[1].Value;
                var items = randStrs.Split(new char[] { ',' }, StringSplitOptions.None);
                if (items.Any())
                {
                    var rnd = new Random();
                    var randVal = rnd.Next(0, items.Length);
                    result = result.Replace(ItemMatch.ToString(), items[randVal]);
                }
            }

            //${Eval(3+5*2)}
            itemRegex = new Regex(@"\$\{Eval\(([^\)]*)\)\}");
            foreach (Match ItemMatch in itemRegex.Matches(result))
            {
                var expr = ItemMatch.Groups[1].Value;
                DataTable dt = new DataTable();
                var evalRes = dt.Compute(expr, "");
                result = result.Replace(ItemMatch.ToString(), evalRes.ToString());

            }

            //${User0Name}
            itemRegex = new Regex(@"\$\{User(\d+)(\w+)\}");
            foreach (Match ItemMatch in itemRegex.Matches(result))
            {
                var index = int.Parse(ItemMatch.Groups[1].Value);
                var prop = ItemMatch.Groups[2].Value;

                if (Users.ContainsKey(index))
                {
                    var propDict = new Dictionary<string, string>();
                    Users[index].FillToDictionary(propDict);
                    if (propDict.ContainsKey("User" + prop))
                    {
                        result = result.Replace(ItemMatch.ToString(), propDict["User" + prop]);
                    }
                }
            }

            return result;
        }


        //
        public static string OpenFirefox(string user, string password)
        {
            try
            {
                //close all instances of telegram first
                foreach (var p in Process.GetProcessesByName("firefox"))
                {
                    // p.Kill();
                }
            }
            catch
            {
            }

            CommonHelper.StartProcess("runas",
                         string.Format(
                             "/user:{0} \"C:\\Program Files\\Mozilla Firefox\\firefox.exe\" ",
                             user));
            Thread.Sleep(2000);

            // Get a handle to the tg application. The window class
            // and window name were obtained using the Spy++ tool.
            IntPtr runasHandle = CommonHelper.FindWindow("ConsoleWindowClass", @"C:\Windows\system32\runas.exe");

            // Verify that Calculator is a running process.
            if (runasHandle == IntPtr.Zero)
            {
                {
                    return "Runas.exe is not running.";
                }
            }

            // Make tg the foreground application and send it 
            // a set of calculations.
            CommonHelper.SetForegroundWindow(runasHandle);
            SendKeys.SendWait(password + "{ENTER}");
            //wait for firefox to open
            Thread.Sleep(1000);
            return "";
        }

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);


        public static string OpenTelegram(string user, string args, string password)
        {
            try
            {
                //close all instances of telegram first
                foreach (var p in Process.GetProcessesByName("Telegram"))
                {
                    //  p.Kill();
                }
            }
            catch
            {
            }

            CommonHelper.StartProcess("runas",
                         string.Format(
                             "/user:{0} \"C:\\Users\\{0}\\AppData\\Roaming\\Telegram Desktop\\Telegram.exe {1}\" ",
                             user,
                             args));
            Thread.Sleep(2000);

            // Get a handle to the tg application. The window class
            // and window name were obtained using the Spy++ tool.
            IntPtr runasHandle = CommonHelper.FindWindow("ConsoleWindowClass", @"C:\Windows\system32\runas.exe");

            // Verify that Calculator is a running process.
            if (runasHandle == IntPtr.Zero)
            {
                {
                    return "Runas.exe is not running.";
                }
            }

            // Make tg the foreground application and send it 
            // a set of calculations.
            CommonHelper.SetForegroundWindow(runasHandle);
            SendKeys.SendWait(password + "{ENTER}");
            //wait for telegram to open
            Thread.Sleep(1000);
            return "";
        }


        public static string OpenTelegramMemu(string user, string mindex, string url, out Rect location, bool closeAll = true)
        {
            IntPtr runasHandle = CommonHelper.FindWindow("Qt5QWindowIcon", "(" + user + ")");

            // Verify that Calculator is a running process.
            var emulatorOpen = runasHandle != IntPtr.Zero;
            uint openEmulatorPidForUser = 0;
            if (emulatorOpen) CommonHelper.GetWindowThreadProcessId(runasHandle, out openEmulatorPidForUser);

            if (closeAll)
            {
                try
                {
                    //close all instances of memu first
                    foreach (var p in Process.GetProcessesByName("Memu"))
                    {
                        if (p.Id == openEmulatorPidForUser) continue;
                        p.Kill();
                    }
                }
                catch
                {
                }
            }
            var arg = "MEmu";
            if (mindex != "0") arg += "_" + mindex.ToString();
            var memuFolder = ConfigurationManager.AppSettings["memupath"];
            CommonHelper.StartProcess(memuFolder + "MemuConsole.exe", arg, false, memuFolder);
            Thread.Sleep(100);


            if (!emulatorOpen)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var browsertimeoutSecs = 60;
                while (true)
                {
                    if (sw.ElapsedMilliseconds >= browsertimeoutSecs * 1000)
                    {
                        location = new Rect();
                        return "Timeout after secs " + browsertimeoutSecs * 1000; //timeout
                    }
                    // Get a handle to the tg application. The window class
                    // and window name were obtained using the Spy++ tool.
                    runasHandle = CommonHelper.FindWindow("Qt5QWindowIcon", "(" + user + ")");

                    // Verify that Calculator is a running process.
                    if (runasHandle != IntPtr.Zero)
                    {
                        var initialDims = new Rect();
                        GetWindowRect(runasHandle, ref initialDims);
                        Thread.Sleep(5000);

                        while (true)
                        {
                            if (sw.ElapsedMilliseconds >= browsertimeoutSecs * 1000)
                            {
                                location = new Rect();
                                return "Timeout after secs " + browsertimeoutSecs * 1000; //timeout
                            }
                            var currDims = new Rect();
                            GetWindowRect(runasHandle, ref currDims);
                            if (initialDims.Right != currDims.Right) //bir oncekine gore saga dogru acilmissa tamamdir
                            {
                                if (initialDims.Right < currDims.Right && initialDims.Left == currDims.Left &&
                                    initialDims.Top == currDims.Top && initialDims.Bottom == currDims.Bottom)
                                {
                                    break;
                                }
                                initialDims = currDims;
                            }
                            Application.DoEvents();
                        }

                        break;
                    }

                    Application.DoEvents();
                }
                Thread.Sleep(1000);
            }
            runasHandle = CommonHelper.FindWindow("Qt5QWindowIcon", "(" + user + ")");
            CommonHelper.SetForegroundWindow(runasHandle);

            location = new Rect();
            GetWindowRect(runasHandle, ref location);
            if (location.Top < 0 || location.Bottom < 0 || location.Left < 0 || location.Right < 0)
            {
                return "Minus Location coming!";
            }
            Thread.Sleep(1000);

            var tgPos = CommonHelper.CalculateAbsolut(location, 80, 30);

            //go to browser if url is not empty
            if (!string.IsNullOrEmpty(url))
            {
                Thread.Sleep(1000);

                var scenarioPlayPos = new Point(location.Right - 12, location.Top + 88);
                for (int i = 0; i < 10; i++)
                {
                    ClickOnPointTool.ClickOnPoint(scenarioPlayPos);
                    Thread.Sleep(250);
                }

                var scenarioFile = ConfigurationManager.AppSettings["memuscenariofile"];
                File.Copy(CommonHelper.AssemblyDirectory + "\\Templates\\MemuOpenBrowser1.txt", scenarioFile, true);

                /* for (int i = 0; i < 10; i++)
                 {
                     File.AppendAllText(scenarioFile, (8000000 + i * 90000) + "--VINPUT--KBDPR:158:0\r\n");
                     File.AppendAllText(scenarioFile, (8000100 + i * 90000) + "--VINPUT--KBDRL:158:0\r\n");
                 }
                 var template2 = File.ReadAllText(AssemblyDirectory + "\\Templates\\MemuOpenBrowser2.txt");
                 File.AppendAllText(scenarioFile, template2);*/

                for (int i = 0; i < url.Length; i++)
                {
                    File.AppendAllText(scenarioFile, (7000000 + i * 100000) + "--CLIPBOARD--" + url[i] + "\r\n");
                }
                File.AppendAllText(scenarioFile, (8000000 + url.Length * 100000) + "--VINPUT--KBDPR:28:1\r\n");
                File.AppendAllText(scenarioFile, (8500000 + url.Length * 100000) + "--VINPUT--KBDRL:28:0\r\n");
                Thread.Sleep(1000);

                var replayScenarioPos = new Point((location.Right + location.Left) / 2 + 120, (location.Top + location.Bottom) / 2 - 26);
                ClickOnPointTool.ClickOnPoint(replayScenarioPos);
                Thread.Sleep(12000);
                var closeScenarioPos = new Point((location.Right + location.Left) / 2 + 195, (location.Top + location.Bottom) / 2 - 135);
                ClickOnPointTool.ClickOnPoint(closeScenarioPos);
                Thread.Sleep(1000);

            }
            else
            {
                //click telegram, positioned on first row 5th col
                Thread.Sleep(1000);
                ClickOnPointTool.ClickOnPoint(tgPos);
                Thread.Sleep(1000);
            }
            Thread.Sleep(100);
            return "";
        }


        public static string OpenChrome(string user, string password)
        {
            try
            {
                //close all instances of telegram first
                foreach (var p in Process.GetProcessesByName("firefox"))
                {
                    //p.Kill();
                }
            }
            catch
            {
            }

            CommonHelper.StartProcess("runas",
                         string.Format(
                             "/user:{0} \"C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe\" ",
                             user));
            Thread.Sleep(2000);

            // Get a handle to the tg application. The window class
            // and window name were obtained using the Spy++ tool.
            IntPtr runasHandle = CommonHelper.FindWindow("ConsoleWindowClass", @"C:\Windows\system32\runas.exe");

            // Verify that Calculator is a running process.
            if (runasHandle == IntPtr.Zero)
            {
                {
                    return "Runas.exe is not running.";
                }
            }

            // Make tg the foreground application and send it 
            // a set of calculations.
            CommonHelper.SetForegroundWindow(runasHandle);
            SendKeys.SendWait(password + "{ENTER}");
            //wait for firefox to open
            Thread.Sleep(1000);
            return "";
        }

        public static string Evaluate(string expresssion)
        {
            DataTable dt = new DataTable();
            return dt.Compute(expresssion, "").ToString();

        }
    }

}

