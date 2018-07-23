﻿using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using WPF_TTElectronics.UserControls;

namespace WPF_TTElectronics.Models
{
    public class MainBaseModel : Observable
    {
        private bool _about = true;
        public bool About
        {
            get { return _about; }
            set
            {
                _about = value;
                NotifyPropertyChanged();
            }
        }
       


        private string _version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public string Version
        {
            get { return $"Version {_version}"; }
            set
            {
                _version = value;
                NotifyPropertyChanged();
            }
        }

        //private cHojaDeRuta _foldersContainer;
        //public cHojaDeRuta FoldersContainer
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
        //        _foldersContainer = value;
        //        NotifyPropertyChanged();


        //    }
        //}


        


        private string _tempFolder;
        public string TempFolder
        {
            get
            {
                var x = new Helpers.HelperPaths();
                var y = string.Empty;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    y = x.GetTemporaryFolderNameFromXML($@"{Environment.CurrentDirectory}\..\..\Helpers\Settings.xml", "TemporaryFolder");
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



        private ScanView _v_Scan;
        public ScanView V_Scan
        {
            get { return new ScanView(); }
            set { _v_Scan = value; }
        }






    }
}
