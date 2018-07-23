using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using WPF_TTElectronics.Models;

namespace WPF_TTElectronics.Helpers
{
    public class HelperPaths
    {
        public ObservableCollection<cHojaDeRuta> GetAllElementsFromXML(string xmlFullPath, string elementName)
        {
            var tmp = new ObservableCollection<cHojaDeRuta>();

            //Task.Factory.StartNew(delegate {
            //    var xmlStr = File.ReadAllText($@"{xmlFullPath}");
            //    var xmlReader = XmlReader.Create(new StringReader(xmlStr));
            //    while (xmlReader.Read())
            //    {
            //        if (xmlReader.Name.Equals(elementName) && (xmlReader.NodeType == XmlNodeType.Element))
            //        {
            //            string title = xmlReader.GetAttribute("Title");
            //            string path = xmlReader.GetAttribute("Path");
            //            tmp.Add(new HojaDeRuta() { Title = title, FolderPath = path });
            //        }
            //    }

            //});


            var xmlStr = File.ReadAllText($@"{xmlFullPath}");
            var xmlReader = XmlReader.Create(new StringReader(xmlStr));
            while (xmlReader.Read())
            {
                if (xmlReader.Name.Equals(elementName) && (xmlReader.NodeType == XmlNodeType.Element))
                {
                    var title = xmlReader.GetAttribute("Title");
                    var code = xmlReader.GetAttribute("Code");
                    var path = xmlReader.GetAttribute("FolderPath");
                    var title2display = xmlReader.GetAttribute("Title2Display");

                    tmp.Add(new cHojaDeRuta() { Title = title, FolderPath = path, Code = code, Title2Display = title2display });
                }
            }
          
            
            
            return tmp;
        }


        public cHojaDeRuta GetOneElementFromXML(string xmlFullPath, string elementName)
        {
            var tmp = new cHojaDeRuta();


            var xmlStr = File.ReadAllText($@"{xmlFullPath}");
            var xmlReader = XmlReader.Create(new StringReader(xmlStr));
            while (xmlReader.Read())
            {
                if (xmlReader.Name.Equals(elementName) && (xmlReader.NodeType == XmlNodeType.Element))
                {
                    tmp.Title = xmlReader.GetAttribute("Title");
                    tmp.Code = xmlReader.GetAttribute("Code");
                    tmp.FolderPath = xmlReader.GetAttribute("FolderPath");
                    tmp.Title2Display = xmlReader.GetAttribute("Title2Display");

                }
            }
        
             

            return tmp;
        }


        public string GetTemporaryFolderNameFromXML(string xmlFullPath, string elementName)
        {
            var tmp = string.Empty;


            var xmlStr = File.ReadAllText($@"{xmlFullPath}");
            var xmlReader = XmlReader.Create(new StringReader(xmlStr));
            while (xmlReader.Read())
            {
                if (xmlReader.Name.Equals(elementName) && (xmlReader.NodeType == XmlNodeType.Element))
                {
                    tmp = xmlReader.GetAttribute("Name");
                   

                }
            }



            return tmp;
        }

    }
}
