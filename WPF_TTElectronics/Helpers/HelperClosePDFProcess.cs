using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_TTElectronics.Helpers
{
    public class HelperClosePDFProcess
    {


        public void AcrobatProcess()
        {

            try
            {
                var process = Process.GetProcesses(Environment.MachineName.ToString()).Where(p => p.ProcessName.StartsWith("Acro")).Select(pr => pr).FirstOrDefault();


                process.Kill();
               
            }
            catch
            {

               
            }
           
            

            //foreach (Process proc in Process.GetProcesses())
            //{
            //    if (proc.ProcessName.StartsWith("Acro"))
            //    {
            //        string proname = proc.ProcessName.ToString();
            //        if (proc.HasExited == false)
            //        {
            //            proc.WaitForExit(10000);
            //            string title = proc.MainWindowTitle.ToString();
            //            if (title == "Adobe Reader" && proname == "AcroRd32")
            //            {
            //                proc.Kill();
            //                break;
            //            }
            //            else
            //            {
            //                string title2 = proc.MainWindowTitle.ToString();
            //                if (title2 == "Adobe Reader" && proname == "AcroRd32")
            //                {
            //                    proc.Kill();
            //                    break;
            //                }
            //            }

            //        }
            //        else
            //        {
            //            try
            //            {
            //                proc.Kill();
            //                break;
            //            }
            //            catch
            //            {
            //                break;
            //            }
            //        }
            //    }
            //}




        }

    }
}
