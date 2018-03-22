using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AirdropBot
{
    public class Helper
    {
        public static string UsersFile = Helper.AssemblyDirectory + "\\users.csv";


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

        // Get a handle to an application window.
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName,
                                               string lpWindowName);

        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);


        public static string OpenTelegram(string user, string args, string password)
        {
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

            StartProcess("runas",
                         string.Format(
                             "/user:{0} \"C:\\Users\\{0}\\AppData\\Roaming\\Telegram Desktop\\Telegram.exe {1}\" ",
                             user,
                             args));
            Thread.Sleep(2000);

            // Get a handle to the tg application. The window class
            // and window name were obtained using the Spy++ tool.
            IntPtr runasHandle = FindWindow("ConsoleWindowClass", @"C:\Windows\system32\runas.exe");

            // Verify that Calculator is a running process.
            if (runasHandle == IntPtr.Zero)
            {
                {
                    return "Runas.exe is not running.";
                }
            }

            // Make tg the foreground application and send it 
            // a set of calculations.
            SetForegroundWindow(runasHandle);
            SendKeys.SendWait(password + "{ENTER}");
            //wait for telegram to open
            Thread.Sleep(5000);
            return "";
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        public static string OpenTelegramMemu(string user, string mindex, string url, out Rect location, bool closeAll = true)
        {
            IntPtr runasHandle = FindWindow("Qt5QWindowIcon", "(" + user + ")");

            // Verify that Calculator is a running process.
            var emulatorOpen = runasHandle != IntPtr.Zero;
            uint openEmulatorPidForUser = 0;
            if (emulatorOpen) GetWindowThreadProcessId(runasHandle, out openEmulatorPidForUser);

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
            StartProcess(@"D:\Program Files\Microvirt\MEmu\MemuConsole.exe", arg, false, @"D:\Program Files\Microvirt\MEmu\");
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
                    runasHandle = FindWindow("Qt5QWindowIcon", "(" + user + ")");

                    // Verify that Calculator is a running process.
                    if (runasHandle != IntPtr.Zero)
                    {
                        break;
                    }
                    Application.DoEvents();
                }
                //give half min for emu to start
                Thread.Sleep(35000);
            }

            SetForegroundWindow(runasHandle);

            location = new Rect();
            GetWindowRect(runasHandle, ref location);

            var browserPos = CalculateAbsolut(location, 20, 80);
            var tgPos = CalculateAbsolut(location, 80, 30);
            //click home 
            Thread.Sleep(100);
            ClickOnPointTool.ClickOnPoint(runasHandle, new Point(location.Right - 10, location.Bottom - 90));
            //click telegram, positioned on first row 5th col
            Thread.Sleep(100);
            ClickOnPointTool.ClickOnPoint(runasHandle, tgPos);

            Thread.Sleep(100);
            return "";
        }

        private static Point CalculateAbsolut(Rect location, int percX, int percY)
        {
            int calcX = location.Left + (int)((location.Right - location.Left) * percX / (double)100);
            int calcY = location.Top + (int)((location.Bottom - location.Top) * percY / (double)100);
            return new Point(calcX, calcY);
        }


        public static string StartProcess(string app, string args, bool output = false, string dir = null)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.FileName = app;

            if (output)
            {
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.CreateNoWindow = true;
            }

            startInfo.Arguments = args;
            if (dir != null) startInfo.WorkingDirectory = dir;
            process.StartInfo = startInfo;
            process.Start();
            var result = "";
            if (output)
            {
                var res = new List<string>();
                while (!process.StandardOutput.EndOfStream)
                {
                    res.Add(process.StandardOutput.ReadLine());
                }
                result = string.Join("\r\n", res);
            }
            return result;
        }
    }
}

