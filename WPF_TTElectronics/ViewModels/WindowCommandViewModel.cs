using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using WPF_TTElectronics.Models;

namespace WPF_TTElectronics.ViewModels
{

    public class WindowCommandViewModel
    {

        public WindowCommandModel _model { get; set; }
        public DispatcherTimer _timer { get; set; }


        public WindowCommandViewModel()
        {
            _model = (_model != null) ? _model : new WindowCommandModel();
            _timer = (_timer != null) ? _timer : new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();

            _timer.Tick += (s, e) => {
                _model.Time = string.Format("{0:hh:mm:ss tt}", DateTime.Now);
                _model.DateNow = string.Format("{0:D}", DateTime.Now);
            };
        }

    }
}
