using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Microsoft.Win32;

namespace Pozative
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
         {

            try
            {
                int InstanceCount = 1;
                //try
                //{
                //    string parm = "";
                //    if (args != null && args.Length == 1)
                //    {
                //        parm = args[0].ToString();
                //        if (parm.ToString().ToLower() == "true".ToString().ToLower())
                //        {
                //            InstanceCount = 2;
                //        }                       
                //    }                   
                //}
                //catch (Exception)
                //{
                //    InstanceCount = 1;
                //}
                int intExitInstanceCount = 0;
                foreach (Process Prc in System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)))
                {
                    try
                    {
                        if (Prc.HasExited)
                        {
                            intExitInstanceCount += 1;
                            InstanceCount += 1;
                        }
                    }
                    catch (Exception x)
                    {

                    }
                }
                if (intExitInstanceCount > 0)
                {
                    UTL.Utility.WriteToErrorLogFromAll(intExitInstanceCount.ToString() + " Disabled Instance found so Allowed Instance is now " + InstanceCount.ToString());//###
                }

                if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() <= InstanceCount)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    try
                    {
                        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\UseSingleCPU");                        
                        if (key != null )
                        {
                            if (bool.Parse(key.GetValue("Value").ToString()))
                            {
                                try
                                {
                                    System.Diagnostics.Process.GetCurrentProcess().ProcessorAffinity = (System.IntPtr)1;
                                }
                                catch (Exception)
                                {

                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                       
                    }
                   
                    //Application.Run(new frmPozative());
                    Application.Run(new frmSplashScreen());
                    // Application.Run(new frmNotification());
                }
                else
                {
                    //    MessageBox.Show("Application is already running.");
                    //add code for 3rd instance try..
                }

                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                registryKey.SetValue("Pozative", Application.ExecutablePath);
            }
            catch (Exception ex)
            {
                UTL.Utility.WriteToErrorLogFromAll(ex.Message);
            }
            finally
            {

            }
        }
    }
}
