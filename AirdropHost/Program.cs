using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AirdropHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var airdropExeLoc = ConfigurationManager.AppSettings["AirdropLocation"];
            var dir = Path.GetDirectoryName(airdropExeLoc);
            var commandTextFile = dir + "\\command.txt";
            StartProcess(airdropExeLoc, "");
            BringToFront("Airdrop");

            Console.WriteLine("Listening to the commands by airdrop");
            Console.WriteLine("Press any key to close airdrop host.... ");
            do
            {
                if (File.Exists(commandTextFile))
                {
                    CloseProcessAllInstances("AirdropBot");
                    var command = File.ReadAllText(commandTextFile);
                    StartProcess(airdropExeLoc, command);
                    BringToFront("Airdrop");
                    Thread.Sleep(3000);
                    File.Delete(commandTextFile);
                    Thread.Sleep(1000);
                }
            } while (!Console.KeyAvailable);

        }
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


        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(String lpClassName, String lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void BringToFront(string title)
        {
            // Get a handle to the Calculator application.
            IntPtr handle = FindWindow(null, title);

            // Verify that Calculator is a running process.
            if (handle == IntPtr.Zero)
            {
                return;
            }

            // Make Calculator the foreground application
            SetForegroundWindow(handle);
        }
    }
}
