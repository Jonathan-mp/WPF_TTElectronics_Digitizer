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

            var xmlStr = File.ReadAllText($@"{xmlFullPath}");
            var xmlReader = XmlReader.Create(new StringReader(xmlStr));
            while (xmlReader.Read())
            {
                if (xmlReader.Name.Equals(elementName) && (xmlReader.NodeType == XmlNodeType.Element))
                {
                    var title = xmlReader.GetAttribute("Title");
                    var path = xmlReader.GetAttribute("FolderPath");

                    tmp.Add(new cHojaDeRuta() { Title = title, FolderPath = path });
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
