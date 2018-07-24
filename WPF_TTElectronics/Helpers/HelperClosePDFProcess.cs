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

                return;
            }

            
        }

    }
}
