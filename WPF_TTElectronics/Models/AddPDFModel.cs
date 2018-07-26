using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                NotifyPropertyChanged("SelectADestinationFile");
            }
        }

        private string _selectADestinationFile;
        public string SelectADestinationFile
        {
            get { return (FileDestination == null) ? " Select a destination file..." : $" FILE SELECTED: {FileDestination.FullName}"; }
            set { _selectADestinationFile = value;
                NotifyPropertyChanged();

            }
        }




        private bool _isMsgVisible;
        public bool IsMsgVisible
        {
            get { return _isMsgVisible; }
            set { _isMsgVisible = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("VisibilityPDF");

            }
        }


        private bool _correctFormat;
        public bool CorrectFormat
        {
            get { return _correctFormat; }
            set { _correctFormat = value;
                NotifyPropertyChanged();
               
            }
        }

        private Visibility _visibilityPDF;
        public Visibility VisibilityPDF
        {
            get { return (IsMsgVisible) ? Visibility.Hidden : Visibility.Visible; }
            set { _visibilityPDF = value;
                NotifyPropertyChanged();
            }
        }

        


    }
}
