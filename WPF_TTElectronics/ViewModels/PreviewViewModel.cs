using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPF_TTElectronics.ViewModels
{
     public class PreviewViewModel
    {
        public string pathToOpen { get; set; }

        public PreviewViewModel(string fullPath)
        {

            if (fullPath != null)
                pathToOpen = fullPath;
           
        }


       


    }
}
