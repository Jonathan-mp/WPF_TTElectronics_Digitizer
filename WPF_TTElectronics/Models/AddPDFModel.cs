using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_TTElectronics.Models
{
    public class AddPDFModel : Observable
    {

        private cFileInfo _fileDestination;
        public cFileInfo  FileDestination
        {
            get { return _fileDestination; }
            set { _fileDestination = value;
                NotifyPropertyChanged();
            }
        }



        private bool _isMsgVisible;
        public bool IsMsgVisible
        {
            get { return _isMsgVisible; }
            set { _isMsgVisible = value;
                NotifyPropertyChanged();

            }
        }

        

    }
}
