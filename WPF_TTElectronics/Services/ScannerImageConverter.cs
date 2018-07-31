using System;
using System.Windows.Media.Imaging;
using WIA;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Collections.ObjectModel;
using PdfSharp.Pdf.IO;
using MahApps.Metro.Controls.Dialogs;
using WPF_TTElectronics.Models;
using System.Threading.Tasks;

namespace WPF_TTElectronics.Services
{
    public class ScannerImageConverter
    {
        public string _container { get; set; }
        public string _containerNname { get; set; }


        public ScannerImageConverter(string container)
        {
            _container = $"{container}";
            _containerNname = $"{container}Preview";
        }



        // this could be in the ScannerService, but then that service
        // takes a dependency on WPF, which I didn't want. Better to have
        // the dependencies wrapped into this service instead. Requires
        // FileIOPermission
        public BitmapSource ConvertScannedImage(ImageFile imageFile)
        {
            if (imageFile == null)
                return null;

            // save the image out to a temp file
            var fileName = Path.GetTempFileName();
             
            // this is pretty hokey, but since SaveFile won't overwrite, we 
            // need to do something to both guarantee a unique name and
            // also allow SaveFile to write the file
            File.Delete(fileName);

            // now save using the same filename
            imageFile.SaveFile(fileName);

            BitmapFrame img;

            // load the file back in to a WPF type, this is just 
            // to get around size issues with large scans
            using (FileStream stream = File.OpenRead(fileName))
            {
                img = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                stream.Close();
            }

            // clean up
            File.Delete(fileName);

            return img;
          
        }









      
        public bool SaveOnPDF(BitmapFrame img)
        {
            try
            {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(img));
                    using (FileStream stream = new FileStream($@"{_containerNname}.png", FileMode.Create))
                    encoder.Save(stream);
                    ConvertImageToPDF();
                if(File.Exists($"{_containerNname}.png"))
                    File.Delete($"{_containerNname}.png");

                    return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }





    
        public void ConvertImageToPDF()
        {
            try
            {
                    var doc = new PdfDocument();
                    doc.Pages.Add(new PdfPage());
                    var xgr = XGraphics.FromPdfPage(doc.Pages[0]);
                    var _img = XImage.FromFile($"{_containerNname}.png");
                    xgr.DrawImage(_img, 0, 0);
                    doc.Save($"{_containerNname}.pdf");
                    
                    doc.Close();
                    xgr.Dispose();
                    _img.Dispose();
                    doc.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
               
            }
        }


        public void SavePDFsOn(ObservableCollection<ImageFile> imgs)
        {

            try
            {
                var bitmapsource = new ObservableCollection<BitmapSource>();
                var outPdf = new PdfDocument();

                foreach (ImageFile item in imgs)
                    bitmapsource.Add(ConvertScannedImage(item));

                foreach (BitmapSource item in bitmapsource)
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(item as BitmapFrame));

                    using (FileStream stream = new FileStream($@"{_containerNname}.png", FileMode.Create))
                        encoder.Save(stream);
                    ConvertImageToPDF();
                    var inputPdf = PdfReader.Open($"{_containerNname}.pdf", PdfDocumentOpenMode.Import);
                    outPdf = CopyPages(inputPdf, outPdf);
                    if (File.Exists($"{_containerNname}.pdf"))
                        File.Delete($"{_containerNname}.pdf");
                    if (File.Exists($"{_containerNname}.png"))
                        File.Delete($"{_containerNname}.png");
                }

                outPdf.Save($"{_containerNname}.pdf");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }


        }


        public void AddToExistingPDF(string pathFrom, string pathTo)
        {
            var inputPdf = PdfReader.Open($@"{pathFrom}", PdfDocumentOpenMode.Import);
            var outPdf = PdfReader.Open($@"{pathTo}", PdfDocumentOpenMode.Modify);
             outPdf = CopyPages(inputPdf, outPdf);

            outPdf.Save(pathTo);
            outPdf.Close();
            outPdf.Dispose();
         
            inputPdf.Dispose();
        }

        public PdfDocument CopyPages(PdfDocument from, PdfDocument to)
        {
            for (int i = 0; i < from.PageCount; i++)
                to.AddPage(from.Pages[i]);

            return to;
        }



        public  void AddToExistingPDF(string pathFrom, string pathTo, ProgressDialogController ctrl)
        {
            var _in = new FileInfo(pathFrom);
            var _out = new FileInfo(pathTo);

            var inputPdf = PdfReader.Open($@"{_in.FullName}", PdfDocumentOpenMode.Import);
            var outPdf = PdfReader.Open($@"{_out.FullName}", PdfDocumentOpenMode.Modify);
            outPdf =  CopyPages(inputPdf, outPdf, ctrl, _in, _out);
      
            outPdf.Save(pathTo);
            outPdf.Close();
            outPdf.Dispose();

            inputPdf.Dispose();
        }

        public PdfDocument CopyPages(PdfDocument from, PdfDocument to, ProgressDialogController ctrl, FileInfo _in, FileInfo _out)
        {
            for (int i = 0; i < from.PageCount; i++)
            {
                ctrl.SetTitle($"Adding pages to {_out.Name}");
                ctrl.SetMessage($"{i+1}/{from.PageCount} pages from {_in.Name}");
                to.AddPage(from.Pages[i]);
                var progress = (((((i + 1f) * 100f)) / ((from.PageCount) * (100f))));
                ctrl.SetProgress(progress);
                
            }

            return to;
        }










        public BitmapSource InMemoryConvertScannedImage(ImageFile imageFile)
        {
            if (imageFile == null)
                return null;

            try
            {
                WIA.Vector vector = imageFile.FileData;

                if (vector != null)
                {
                    byte[] bytes = vector.get_BinaryData() as byte[];

                    if (bytes != null)
                    {
                        var ms = new MemoryStream(bytes);
                        return BitmapFrame.Create(ms);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return null;
        }




     
    }
}
