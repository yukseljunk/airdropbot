﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            startInfo.FileName = "runas";

            startInfo.Arguments =
                string.Format("/user:{0} \"C:\\Users\\{0}\\AppData\\Roaming\\Telegram Desktop\\Telegram.exe {1}\" ",
                              user,
                              args);

            process.StartInfo = startInfo;
            process.Start();
            Thread.Sleep(2000);

            // Get a handle to the Calculator application. The window class
            // and window name were obtained using the Spy++ tool.
            IntPtr runasHandle = FindWindow("ConsoleWindowClass", @"C:\Windows\system32\runas.exe");

            // Verify that Calculator is a running process.
            if (runasHandle == IntPtr.Zero)
            {
                {
                    return "Runas.exe is not running.";
                }
            }

            // Make Calculator the foreground application and send it 
            // a set of calculations.
            SetForegroundWindow(runasHandle);
            SendKeys.SendWait(password + "{ENTER}");
            //wait for telegram to open
            Thread.Sleep(5000);
            return "";
        }
    }
}
