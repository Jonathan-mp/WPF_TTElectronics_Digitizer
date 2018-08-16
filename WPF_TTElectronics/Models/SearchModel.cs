using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_TTElectronics.Models
{
    public class SearchModel : Observable
    {
        readonly Regex inputForlat = new Regex(@"^[a-zA-Z-\d]{1,15}$");

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


        private cFileInfo _selectedFile;
        public cFileInfo SelectedFile
        {
            get
            {

                try
                {

                    var dirInfo = new DirectoryInfo($@"{FolderToSearch.FolderPath}");

                    var result = dirInfo.GetFiles("*.pdf", SearchOption.AllDirectories).Where(x => x.Name.Split('.')[0] == $@"{ModelToSearch}_{CodeDateToSearch}").Select(x => new cFileInfo { FullName = x.Name.Split('.')[0], Model = x.Name.Split('_')[0], DateCode = x.Name.Split('_')[1].Split('.')[0], Family = x.Directory.Name, FullPathWithExtension = x.FullName, TimeCreation =  x.CreationTime.ToString("d"), TimeLastAccess = x.LastAccessTime.ToString("d"), TimeLastWrite = x.LastWriteTime.ToString("d") }).FirstOrDefault();

                    _selectedFile = result;
                }
                catch (Exception ex)
                {
                    var a = ex.Message;
                    _selectedFile = new cFileInfo();
                }


                return _selectedFile;

            }
            set
            {
                _selectedFile = value;
                NotifyPropertyChanged();

            }
        }

        private cHojaDeRuta _folderToSearch;
        public cHojaDeRuta FolderToSearch
        {
            get { return _folderToSearch; }
            set
            {
                _folderToSearch = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("FullPathToSearch");
             

            }
        }

        //private string _header;
        //public string Header
        //{
        //    get { return _header; }
        //    set { _header = value;
        //        NotifyPropertyChanged();
        //    }
        //}

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


        private string _modelToSearch;
        public string ModelToSearch
        {
            get { return _modelToSearch; }
            set {
                if (value == string.Empty)
                    _modelToSearch = null;
                else
                _modelToSearch = (inputForlat.IsMatch(value)) ? value.ToUpper() : _modelToSearch;
                NotifyPropertyChanged();
                NotifyPropertyChanged("FullPathToSearch");
                NotifyPropertyChanged("FileNameAdded");
                NotifyPropertyChanged("AllFileNames");
            }
        }

        private string _codeDateToSearch;
        public string CodeDateToSearch
        {
            get { return _codeDateToSearch; }
            set
            {
                if (value == string.Empty)
                    _codeDateToSearch = null;
                else
                    _codeDateToSearch = (inputForlat.IsMatch(value)) ? value.ToUpper() : _codeDateToSearch;
                NotifyPropertyChanged();
                NotifyPropertyChanged("FullPathToSearch");
                NotifyPropertyChanged("FileNameAdded");
            }
        }

        private string _fileNameAdded;
        public string FileNameAdded
        {
            get {
                try
                {
                  return (ModelToSearch != string.Empty & CodeDateToSearch != string.Empty)?   $"{ModelToSearch}_{CodeDateToSearch}" :  null;
                }
                catch 
                {
                    return null;
                }

            }
            set { _fileNameAdded = value;
                NotifyPropertyChanged();
               
            }
        }





        private string _fullPathToSearch;
        public string FullPathToSearch
        {
            get {
                try
                {


                   return (FolderToSearch.FolderPath != null && FileNameAdded != null) ?
                         $"{FolderToSearch.FolderPath}{FileNameAdded}" : null;
                  
                }
                catch
                {
                    return null;
                }

            }
            set {
                _fullPathToSearch = value;
                NotifyPropertyChanged();
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




        private Visibility _searchStackVisibility;
        public Visibility SearchStackVisibility
        {
            get { return _searchStackVisibility; }
            set
            {
                _searchStackVisibility = value;
                NotifyPropertyChanged();
            }
        }


        private Visibility _addPageStackVisibility;
        public Visibility AddPageStackVisibility
        {
            get { return _addPageStackVisibility;  }
            set { _addPageStackVisibility = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("SelectedFile");
            }
        }

        private Visibility _visibilityView;
        public Visibility VisibilityView
        {
            get { return (IsMsgVisible==true)? Visibility.Hidden : Visibility.Visible; }
            set { _visibilityView = value;
                NotifyPropertyChanged();
            }
        }



        private bool _isMsgVisible = false;
        public bool IsMsgVisible
        {
            get { return _isMsgVisible; }
            set { _isMsgVisible = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("VisibilityView");
            }
        }




        private bool _autoAdd;
        public bool AutoAdd
        {
            get { return _autoAdd; }
            set { _autoAdd = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("AutoAddIcon");
            }
        }

        private string _autoAddIcon;
        public string AutoAddIcon
        {
            get { return (_autoAdd)? "Check" : string.Empty; }
            set { _autoAddIcon = value;
                NotifyPropertyChanged();
            }
        }

        private bool _colorSetting;
        public bool ColorSetting
        {
            get { return _colorSetting; }
            set
            {
                _colorSetting = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("ColorSettingTitle");
                NotifyPropertyChanged("ColorSettingIcon");



            }
        }

        private string _colorSettingTitle;
        public string ColorSettingTitle
        {
            get { return (_colorSetting) ? "Color" : "Black & White"; }
            set
            {
                _colorSettingTitle = value;
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

    }
}
