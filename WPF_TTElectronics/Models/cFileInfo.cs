using System;


namespace WPF_TTElectronics.Models
{
    public class cFileInfo
    {
        public string FullName { get; set; }

        public string Model { get; set; }
        public string DateCode { get; set; }
        public string Family { get; set; }
        public string FullPathWithExtension { get; set; }
        public DateTime TimeCreation { get; set; }
        public DateTime TimeLastAccess { get; set; }
        public DateTime TimeLastWrite { get; set; }

        public bool Check2Add { get; set; }


    }
}
