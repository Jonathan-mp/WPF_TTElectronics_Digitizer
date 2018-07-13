using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_TTElectronics.Models
{
    public  class WindowCommandModel : Observable
    {
        private string _time;
        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
                NotifyPropertyChanged();
            }
        }

        private string _dateNow;

        public string DateNow
        {
            get { return _dateNow; }
            set
            {
                _dateNow = value;
                NotifyPropertyChanged();

            }
        }



    }
}
