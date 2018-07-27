using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                NotifyPropertyChanged("CorrectFormat");
                NotifyPropertyChanged("VisibilityInfo");
                NotifyPropertyChanged("VisibilityPDF");

            }
        }

        private bool _correctFormat;
        public bool CorrectFormat
        {
            get { return (FileDestination != null) ? true : false; }
            set
            {
                _correctFormat = value;
                NotifyPropertyChanged();
               


            }
        }

        private Visibility _visibilityInfo;
        public Visibility VisibilityInfo
        {
            get { return (CorrectFormat) ? Visibility.Visible : Visibility.Hidden; }
            set
            {
                _visibilityInfo = value;
                NotifyPropertyChanged();
               

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

        private Visibility _visibilityPDF;
        public Visibility VisibilityPDF
        {
            get { return (!IsMsgVisible && CorrectFormat) ? Visibility.Visible : Visibility.Hidden; }
            set
            {
                _visibilityPDF = value;
                NotifyPropertyChanged();
            }
        }



        private ObservableCollection<cFileInfo> _pdf2Add;
        public ObservableCollection<cFileInfo> PDF2Add
        {
            get { return _pdf2Add; }
            set { _pdf2Add = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("SelectFiles2Add");
              
            }
        }

        private string _selectFiles2Add;
        public string SelectFiles2Add
        {
            get { return (PDF2Add != null) ? $" {PDF2Add.Where(x => x.Check2Add == true).Count()} PDF files selected" : " Select files to add..." ; }
            set
            {
                _selectFiles2Add = value;
                NotifyPropertyChanged();

            }
        }


        


    }
}
