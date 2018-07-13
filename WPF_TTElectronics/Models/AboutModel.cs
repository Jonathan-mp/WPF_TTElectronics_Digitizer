using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WPF_TTElectronics.Models
{
    public class AboutModel : Observable
    {
        private string _version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public string Version
        {
            get { return $"Version {_version}"; }
            set
            {
                _version = value;
                NotifyPropertyChanged();
            }
        }




        private string _aboutInfo;
        public string AboutInfo
        {
            get { return _aboutInfo = $"Digitizer software designed to TT Electronics.\nDigitizing scanned files to PDF.\nPrincipal functions: Create, View and Save files.\nSoftware {Version}."; ; }
            set
            {
                _aboutInfo = value;
                NotifyPropertyChanged();
            }
        }





        private string _developerCareer = "Information Techonology Engineer";

        public string DeveloperCareer
        {
            get { return _developerCareer; }
            set
            {
                _developerCareer = value;
                NotifyPropertyChanged();
            }
        }

        private string _developerEmail = "Jonathan_m.p@hotmail.com";

        public string DeveloperEmail
        {
            get { return _developerEmail; }
            set
            {
                _developerEmail = value;
                NotifyPropertyChanged();

            }
        }

        private int _secondsToClose = 8;
        public int SecondsToClose
        {
            get { return _secondsToClose; }
            set
            {
                _secondsToClose = value;
                NotifyPropertyChanged();

            }
        }



    }
}
