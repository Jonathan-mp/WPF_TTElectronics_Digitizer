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
    public class SearchModel : Observable
    {

        private string _tempFolder;
        public string TempFolder
        {
            get
            {
                var x = new Helpers.HelperPaths();
                var y = string.Empty;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    y = x.GetTemporaryFolderNameFromXML($@"{Environment.CurrentDirectory}\Settings.xml", "TemporaryFolder");
                    y = string.Format("{0}{1}", Path.GetTempPath(), y);
                });
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

                    var result = dirInfo.GetFiles("*.pdf", SearchOption.AllDirectories).Where(x => x.Name.Contains($@"{ModelToSearch}_{CodeDateToSearch}")).Select(x => new cFileInfo { FullName = x.Name.Split('.')[0], Model = x.Name.Split('_')[0], DateCode = x.Name.Split('_')[1].Split('.')[0], Family = x.Directory.Name, FullPathWithExtension = x.FullName, TimeCreation = x.CreationTime, TimeLastAccess = x.LastAccessTime, TimeLastWrite = x.LastWriteTime }).FirstOrDefault();

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







        //private cHojaDeRuta _folderContainer;
        //public cHojaDeRuta FolderContainer
        //{
        //    get
        //    {
        //        var x = new Helpers.HelperPaths();
        //        var y = new cHojaDeRuta();
        //        Application.Current.Dispatcher.Invoke(() =>
        //        {
        //            y = x.GetOneElementFromXML($@"{Environment.CurrentDirectory}\..\..\Helpers\Settings.xml", "ContainerFolder");
        //        });
        //        return y;
        //    }
        //    set
        //    {
        //        _folderContainer = value;
        //        NotifyPropertyChanged();


        //    }
        //}


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

        private string _header;
        public string Header
        {
            get { return _header; }
            set { _header = value;
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
                Application.Current.Dispatcher.Invoke(() =>
                {
                    y = x.GetAllElementsFromXML($@"{Environment.CurrentDirectory}\Settings.xml", "FolderPath");
                });
                return y;
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
                _modelToSearch = (value.All(char.IsDigit)) ? value : _modelToSearch;
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
                _codeDateToSearch = (value.All(char.IsDigit)) ? value : _codeDateToSearch;
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
            }
        }




      




    }
}
