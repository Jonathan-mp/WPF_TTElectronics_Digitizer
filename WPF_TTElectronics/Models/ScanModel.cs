using PdfSharp.Pdf;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WPF_TTElectronics.Models
{
    public class ScanModel : Observable
    {
        readonly Regex inputForlat = new Regex(@"^[a-zA-Z-\d]{1,15}$");


        private bool _colorSetting;
        public bool ColorSetting
        {
            get { return _colorSetting; }
            set { _colorSetting = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("ColorSettingTitle");
                NotifyPropertyChanged("ColorSettingIcon");

            }
        }

        private string _colorSettingTitle;
        public string ColorSettingTitle
        {
            get { return (_colorSetting)? "Color" : "Black & White"; }
            set { _colorSettingTitle = value;
                NotifyPropertyChanged();
            }
        }

        private string _colorSettingdIcon;
        public string ColorSettingIcon
        {
            get { return (_colorSetting) ? "Check" : string.Empty; }
            set
            {
                _colorSettingdIcon = value;
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
                return new ObservableCollection<cHojaDeRuta>(y.OrderBy(i => i.Title));
            }
            set
            {
                _comboItems = value;
                NotifyPropertyChanged();
            }
        }


        private cHojaDeRuta _folderToSave;
        public cHojaDeRuta FolderToSave
        {
            get { return _folderToSave; }
            set { _folderToSave = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("FullPathToSave");
                NotifyPropertyChanged("SaveEnable");



            }
        }


        private string _modelToSave;
        public string ModelToSave
        {
            get { return _modelToSave; }
            set {
                if (value == string.Empty)
                    _modelToSave = null;
                else
                    _modelToSave = (inputForlat.IsMatch(value)) ? value.ToUpper() : _modelToSave;

                NotifyPropertyChanged();
                NotifyPropertyChanged("FileNameToSave");
                NotifyPropertyChanged("SaveEnable");

            }
        }

        private string _codeDateToSave;

        public string CodeDateToSave
        {
            get { return _codeDateToSave; }
            set {
                if (value == string.Empty)
                    _codeDateToSave = null;
                else
                    _codeDateToSave = (inputForlat.IsMatch(value)) ? value.ToUpper() : _codeDateToSave;

                NotifyPropertyChanged();
                NotifyPropertyChanged("FileNameToSave");
                NotifyPropertyChanged("SaveEnable");
            }
        }




        private string _fileNameToSave;
        public string FileNameToSave
        {
            get
            {
                try
                {
                    return (!string.IsNullOrWhiteSpace(ModelToSave) && !string.IsNullOrWhiteSpace(CodeDateToSave)) ? $"{ModelToSave}_{CodeDateToSave}" : null;
                }
                catch
                {
                    return null;
                }
            }
                set { _fileNameToSave = value.Replace(" ", "_");
                NotifyPropertyChanged();
                NotifyPropertyChanged("FullPathToSave");
               


            }
        }


        private string _fullPathToSave;

        public string FullPathToSave
        {
            get { return $"{FolderToSave.FolderPath}{FileNameToSave}"; }
            set { _fullPathToSave = value;
                NotifyPropertyChanged();

            }
        }


        private string _openedFileFolder;

        public string OpenedFileFolder
        {
            get { return _openedFileFolder; }
            set { _openedFileFolder = value;
                NotifyPropertyChanged();

            }
        }


        private string _openedFileName;

        public string OpenedFileName
        {
            get { return _openedFileName; }
            set { _openedFileName = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("VisibilityHeader");

            }
        }


        private string _fullPathOpenedFile;

        public string FullPathOpenedFile
        {
            get { return _fullPathOpenedFile; }
            set { _fullPathOpenedFile = value;
                NotifyPropertyChanged();

            }
        }


        private Visibility _visibilityHeader = Visibility.Hidden;
        public Visibility VisibilityHeader
        {
            get { return _visibilityHeader; }
            set { _visibilityHeader = value;
                NotifyPropertyChanged();
                
                    

            }
        }


        private string _fullPathFile;
        public string FullPathFile
        {
            get { return _fullPathFile;  }
            set { _fullPathFile = value;
                NotifyPropertyChanged();
            }
        }


        private BitmapSource _scannedImage;
        public BitmapSource ScannedImage
        {
            get { return _scannedImage; }
            set
            {
                _scannedImage = value;
                NotifyPropertyChanged("ScannedImage");
            }
        }




        private bool _saveAsEnable = false;
        public bool SaveAsEnable
        {
            get { return _saveAsEnable; }
            set
            {
                _saveAsEnable = value;
                NotifyPropertyChanged();
            }
        }

        private bool _saveEnable = false;
        public bool SaveEnable
        {
            get { return (string.IsNullOrWhiteSpace(FileNameToSave) || FolderToSave is null) ? false : true; }
            set
            {
                _saveEnable = value;
                NotifyPropertyChanged();
                

            }
        }




       



        private HojaDeRuta _selectedPath;
        public HojaDeRuta SelectedPath
        {
            get { return _selectedPath; }
            set { _selectedPath = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("SavePath");
            }
        }

    }





    public class HojaDeRuta : Observable
    {

        private string _title;
        public string Title 
        {
            get { return _title; }
            set { _title = value;
                NotifyPropertyChanged();
            }
        }

        private string _folderPath;
        public string FolderPath
        {
            get { return _folderPath; }
            set { _folderPath = value;
                NotifyPropertyChanged();
            }
        }



    }
}
