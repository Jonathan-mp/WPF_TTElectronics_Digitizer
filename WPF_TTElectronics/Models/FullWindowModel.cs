using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_TTElectronics.Models
{
    public class FullWindowModel : Observable
    {
       
        private string _titleWindow;
        public string TitleWindow
        {
            get { return _titleWindow; }
            set { _titleWindow = value;
                NotifyPropertyChanged();
            }
        }



    }
}
