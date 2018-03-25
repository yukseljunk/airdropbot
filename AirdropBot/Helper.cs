using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
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
            var memuFolder = ConfigurationManager.AppSettings["memupath"];
            StartProcess(memuFolder + "MemuConsole.exe", arg, false, memuFolder);
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
                var mws = int.Parse(ConfigurationManager.AppSettings["memuwaitsec"]);
                Thread.Sleep(mws * 1000);
            }

            SetForegroundWindow(runasHandle);

            location = new Rect();
            GetWindowRect(runasHandle, ref location);

            var browserPos = CalculateAbsolut(location, 15, 50);
            var tgPos = CalculateAbsolut(location, 80, 30);
            var cirlePos = new Point(location.Right - 10, location.Bottom - 90);
            var trianglePos = new Point(location.Right - 10, location.Bottom - 120);
            //click home 
            Thread.Sleep(100);
            ClickOnPointTool.ClickOnPoint(cirlePos);

            //go to browser if url is not empty
            if (!string.IsNullOrEmpty(url))
            {
                Thread.Sleep(2000);
                ClickOnPointTool.ClickOnPoint(browserPos);
                Thread.Sleep(500);
                for (int i = 0; i < 10; i++)
                {
                    ClickOnPointTool.ClickOnPoint(trianglePos);
                    Thread.Sleep(50);
                }
                Thread.Sleep(1500);
                ClickOnPointTool.ClickOnPoint(browserPos);
                Thread.Sleep(1000);

                
                ClickOnPointTool.ClickOnPoint(CalculateAbsolut(location, 50, 30));
                Thread.Sleep(2000);
                SendKeys.SendWait(url);
                Thread.Sleep(1000);
                ClickOnPointTool.ClickOnPoint(CalculateAbsolut(location, 95, 10));
                Thread.Sleep(3000);
                ClickOnPointTool.ClickOnPoint(new Point(CalculateAbsolut(location, 50, 10).X, location.Bottom - 10));

                Thread.Sleep(1000);

            }
            else
            {
                //click telegram, positioned on first row 5th col
                Thread.Sleep(100);
                ClickOnPointTool.ClickOnPoint(tgPos);
            }
            Thread.Sleep(100);
            return "";
        }


        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);


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

    public class WindowHandleInfo
    {
        private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

        private IntPtr _MainHandle;

        public WindowHandleInfo(IntPtr handle)
        {
            this._MainHandle = handle;
        }

        public List<string> GetAllChildHandles()
        {
            List<IntPtr> childHandles = new List<IntPtr>();
            var result = new List<string>();

            GCHandle gcChildhandlesList = GCHandle.Alloc(childHandles);
            IntPtr pointerChildHandlesList = GCHandle.ToIntPtr(gcChildhandlesList);

            try
            {
                EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
                EnumChildWindows(this._MainHandle, childProc, pointerChildHandlesList);
            }
            finally
            {
                gcChildhandlesList.Free();
            }

            if (childHandles.Count > 0)
            {
                foreach (var childHandle in childHandles)
                {
                    int capacity = GetWindowTextLength(new HandleRef(this, childHandle)) * 2;
                    StringBuilder stringBuilder = new StringBuilder(capacity);
                    var caption = GetWindowText(new HandleRef(this, childHandle), stringBuilder, stringBuilder.Capacity);
                    result.Add(stringBuilder.ToString());
                }
            }

            return result;
        }

        private bool EnumWindow(IntPtr hWnd, IntPtr lParam)
        {
            GCHandle gcChildhandlesList = GCHandle.FromIntPtr(lParam);

            if (gcChildhandlesList == null || gcChildhandlesList.Target == null)
            {
                return false;
            }

            List<IntPtr> childHandles = gcChildhandlesList.Target as List<IntPtr>;
            childHandles.Add(hWnd);

            return true;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(HandleRef hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(HandleRef hWnd, StringBuilder lpString, int nMaxCount);
    }
}

