﻿using System;
using WIA;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using MahApps.Metro.Controls.Dialogs;

namespace WPF_TTElectronics.Services
{
    public class ScannerException : ApplicationException
    {
        public ScannerException()
            : base()
        { }

        public ScannerException(string message)
            : base(message)
        { }

        public ScannerException(string message, Exception innerException)
            : base(message, innerException)
        { }

    }

    public class ScannerNotFoundException : ScannerException
    {
        public ScannerNotFoundException()
              : base("Error retrieving a list of scanners. Is your scanner or multi-function printer turned on?")
        {
        }
    }

    public class ScannerService
    {
        

        public ImageFile Scan(ProgressDialogController ctrl)
        {

         
            try
            {
                
                var image = new ImageFile();
                var dialog = new CommonDialog();
                //dialog.ShowAcquireImage(
                //     WiaDeviceType.UnspecifiedDeviceType,
                //     WiaImageIntent.ColorIntent,
                //     WiaImageBias.MaximizeQuality,
                //     "{B96B3CB1-0728-11D3-9D7B-0000F81EF32E}",
                //     false,
                //     false,false
                //    );

                ctrl.SetTitle("STARTING SCANNER");
                ctrl.SetMessage("This can take a few seconds...");
                var x = dialog.ShowSelectDevice(WiaDeviceType.ScannerDeviceType, false, false);
                var item = x.Items[1];


                image = dialog.ShowTransfer(item, "{B96B3CB1-0728-11D3-9D7B-0000F81EF32E}", true);


                if (image != null)
                {
                    
                    ctrl.SetTitle("SCANNING FINISHED!");
                    ctrl.SetMessage($"File Scanned Successfully...");
                   

                }
                return image;
            }
            catch (COMException ex)
            {
                if (ex.ErrorCode == -2145320939)
                {
                    throw new ScannerNotFoundException();
                }
                else
                {
                    throw new ScannerException(ex.Message, ex);
                }
            }

        }






        public ObservableCollection<ImageFile> ScanAll(ProgressDialogController ctrl)
        {
            try
            {
                var image = new ImageFile();
                var dialog = new CommonDialog();
                var images = new ObservableCollection<ImageFile>();
               
                ctrl.SetTitle("STARTING SCANNER");
                ctrl.SetMessage("This can take a few seconds...");


                var x = dialog.ShowSelectDevice(WiaDeviceType.ScannerDeviceType, false, true);
                foreach (Item item in x.Items)
                {
                    while (true)
                    {
                        try
                        {

                            image = dialog.ShowTransfer(item, "{B96B3CB1-0728-11D3-9D7B-0000F81EF32E}", true);
                            images.Add(image);
                            ctrl.SetTitle("SCANNING");
                            ctrl.SetMessage($"{images.Count} {(images.Count>1? "Files" : "File")} Scanned Successfully...");
                            
                        }
                        catch
                        {
                            ctrl.SetTitle("SCANNING FINISHED!");
                            ctrl.SetMessage($"{images.Count} {(images.Count > 1 ? "Files" : "File")} Scanned Successfully...");
                            break;
                        }

                    }
                }
               


             

                return images;
            }
            catch (COMException ex)
            {
                if (ex.ErrorCode == -2145320939)
                {
                   
                    throw new ScannerNotFoundException();
                }
                else
                {
                   
                    throw new ScannerException(ex.Message, ex);
                }
            }
        }




      
    }

    
}
