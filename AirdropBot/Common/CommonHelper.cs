using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Management;
using System.Security.Cryptography;

namespace Common
{
    public class CommonHelper
    {
        public static string UsersFile = CommonHelper.AssemblyDirectory + "\\users.csv";

        // Return a random integer between a min and max value.
        public static int RandomInteger(int min, int max)
        {
            var Rand = new RNGCryptoServiceProvider();
            uint scale = uint.MaxValue;
            while (scale == uint.MaxValue)
            {
                // Get four random bytes.
                byte[] four_bytes = new byte[4];
                Rand.GetBytes(four_bytes);

                // Convert that into an uint.
                scale = BitConverter.ToUInt32(four_bytes, 0);
            }

            // Add min to the scaled difference between max and min.
            return (int)(min + (max - min) *
                (scale / (double)uint.MaxValue));
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

        // Get a handle to an application window.
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName,
                                               string lpWindowName);


        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);


        public static void CloseProcessAllInstances(string processName)
        {
            try
            {
                //close all instances of telegram first
                foreach (var p in Process.GetProcessesByName(processName))
                {
                    p.Kill();
                }
            }
            catch
            {
            }
        }



        public static Dictionary<string, int> GetProcessUsers(string processName)
        {
            var result = new Dictionary<string, int>();
            ObjectQuery x = new ObjectQuery("Select * From Win32_Process where Name='" + processName + "'");

            ManagementObjectSearcher mos = new ManagementObjectSearcher(x);

            foreach (ManagementObject mo in mos.Get())
            {

                string[] s = new string[2];

                mo.InvokeMethod("GetOwner", (object[])s);
                if (!result.ContainsKey(s[0]))
                {
                    result.Add(s[0], int.Parse(mo["PROCESSID"].ToString()));
                }

            }
            return result;
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);


        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);


        public static Point CalculateAbsolut(Rect location, int percX, int percY)
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
                startInfo.RedirectStandardError = true;
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
                while (!process.StandardError.EndOfStream)
                {
                    res.Add(process.StandardError.ReadLine());
                }


                result = string.Join("\r\n", res);
            }
            return result;
        }

    }
}

