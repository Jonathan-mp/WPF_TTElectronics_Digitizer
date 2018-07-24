﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_TTElectronics.Models
{
    public class TestModel : Observable
    {
        private ObservableCollection<cFileInfo> _allFilesNames;
        public ObservableCollection<cFileInfo> AllFileNames
        {
            get {
                try
                {
                   
                    var dirInfo = new DirectoryInfo($@"{FolderToSearch.FolderPath}");

                    var listResult = dirInfo.GetFiles("*.pdf", SearchOption.AllDirectories).Where(x => x.Name.StartsWith(ModelToSearch) && x.Name.Split('_')[1].StartsWith($@"{CodeDateToSearch}")).Select(x => new cFileInfo { FullName = x.Name, Model = x.Name.Split('_')[0], DateCode = x.Name.Split('_')[1].Split('.')[0], Family = x.Directory.Name, FullPathWithExtension = x.FullName, TimeCreation = x.CreationTime, TimeLastAccess = x.LastAccessTime, TimeLastWrite = x.LastWriteTime }).ToList();

                    _allFilesNames = new ObservableCollection<cFileInfo>(listResult);
                }
                catch(Exception ex)
                {
                    var a = ex.Message;
                    _allFilesNames = new ObservableCollection<cFileInfo>();
                }

                return _allFilesNames;

            }
            set { _allFilesNames = value;
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
                NotifyPropertyChanged("AllFileNames");

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
                    y = x.GetAllElementsFromXML($@"{Environment.CurrentDirectory}\..\..\Helpers\Settings.xml", "FolderPath");
                });
                return y;
            }
            set
            {
                _comboItems = value;
                NotifyPropertyChanged();

            }
        }



        private string _modelToSearch = string.Empty;
        public string ModelToSearch
        {
            get { return _modelToSearch; }
            set
            {
                _modelToSearch = value.Replace(" ", string.Empty);
                NotifyPropertyChanged();
                NotifyPropertyChanged("FullPathToSearch");
                NotifyPropertyChanged("FileNameAdded");
                NotifyPropertyChanged("AllFileNames");
            }
        }

        private string _codeDateToSearch = string.Empty;
        public string CodeDateToSearch
        {
            get { return _codeDateToSearch; }
            set
            {
                _codeDateToSearch = value.Replace(" ", string.Empty);
                NotifyPropertyChanged();
                NotifyPropertyChanged("FullPathToSearch");
                NotifyPropertyChanged("FileNameAdded");
                NotifyPropertyChanged("AllFileNames");
            }
        }



        private cFileInfo _fileNameSelected;
        public cFileInfo FileNameSelected
        {
            get
            {
                return (_fileNameSelected != null) ? _fileNameSelected : null;
            }
            set
            {
                _fileNameSelected = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("VisibilityHeader");
            }
        }


        private Visibility _visibilityHeader;
        public Visibility VisibilityHeader
        {
            get {
                try
                {
                    return (string.IsNullOrWhiteSpace(FileNameSelected.FullName)) ? Visibility.Hidden : Visibility.Visible;
                }
                catch
                {

                    return Visibility.Hidden;
                }
            }
            set { _visibilityHeader = value;
                NotifyPropertyChanged();
            }
        }




    }


  
   
}
