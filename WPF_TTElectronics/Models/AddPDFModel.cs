using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_TTElectronics.Models
{
    public class AddPDFModel : Observable
    {
        private ObservableCollection<cHojaDeRuta> _comboItems;
        public ObservableCollection<cHojaDeRuta> ComboItems
        {
            get
            {
                var x = new Helpers.HelperPaths();
                var y = new ObservableCollection<cHojaDeRuta>();

#if(DEBUG)
                Application.Current.Dispatcher.Invoke(() =>
                {
                    y = x.GetAllElementsFromXML($@"{Environment.CurrentDirectory}\..\..\Helpers\Settings.xml", "FolderPath");
                });
#else

                 Application.Current.Dispatcher.Invoke(() =>
                {
                    y = x.GetAllElementsFromXML($@"{Environment.CurrentDirectory}\Settings.xml", "FolderPath");
                });

#endif
                return y;
            }
            set
            {
                _comboItems = value;
                NotifyPropertyChanged();
            }
        }


        private string _tempFolder;
        public string TempFolder
        {
            get
            {
                var x = new Helpers.HelperPaths();
                var y = string.Empty;

#if(DEBUG)
                Application.Current.Dispatcher.Invoke(() =>
                {
                    y = x.GetTemporaryFolderNameFromXML($@"{Environment.CurrentDirectory}\..\..\Helpers\Settings.xml", "TemporaryFolder");
                    y = string.Format("{0}{1}", Path.GetTempPath(), y);
                });
#else
  Application.Current.Dispatcher.Invoke(() =>
                {
                    y = x.GetTemporaryFolderNameFromXML($@"{Environment.CurrentDirectory}\Settings.xml", "TemporaryFolder");
                    y = string.Format("{0}{1}", Path.GetTempPath(), y);
                });
#endif
                return y;
            }
            set
            {
                _tempFolder = value;
                NotifyPropertyChanged();
            }
        }

        private cFileInfo _destinationFile;
        public cFileInfo  DestinationFile
        {
            get { return _destinationFile; }
            set { _destinationFile = value;
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
            get { return (DestinationFile != null) ? true : false; }
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
            get { return (DestinationFile == null) ? " Select a destination file" : $" File selected: {DestinationFile.FullName}"; }
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
            get { return (PDF2Add != null) ? $" {PDF2Add.Where(x => x.Check2Add == true).Count()} PDF files selected" : " Select files to add" ; }
            set
            {
                _selectFiles2Add = value;
                NotifyPropertyChanged();
            }
        }


        private cFileInfo _fileSelected;
        public cFileInfo FileSelected
        {
            get { return _fileSelected; }
            set
            {
                _fileSelected = value;
                NotifyPropertyChanged();
            }
        }



    }
}
