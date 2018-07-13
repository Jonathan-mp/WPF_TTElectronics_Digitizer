using System.Windows;
using System.Linq;
using System.Reflection;
using System.Windows.Threading;
using System;
using WPF_TTElectronics.Models;

namespace WPF_TTElectronics.ViewModels
{
    public class AboutViewModel
    {

        MahApps.Metro.Controls.MetroWindow activeWindow = Application.Current.Windows.OfType<Views.AboutView>().FirstOrDefault();
        public DispatcherTimer _timer { get; set; }
        public AboutModel _model { get; set; }

        /// <summary>
        /// Make a timer only to show this windows for 8 seconds then it will be closed.
        /// </summary>
        public AboutViewModel()
        {
            _model = (_model != null) ? _model : new AboutModel();
            _timer = (_timer != null) ? _timer : new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
            _timer.Tick += (s, e) =>
            {
                if (_model.SecondsToClose <= 1)
                    activeWindow.Close();
                else
                    _model.SecondsToClose--;
            };

            
        }
   

       


    }
}
