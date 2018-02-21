using System;
using System.Xml.Serialization;

namespace LoSa.Land.Parcel
{
    [Serializable()]
    public class LandInfo
    {
        [XmlElementAttribute("Код")]
        public string Key { get; set; }

        [XmlElementAttribute("Значення")]
        public string Value { get; set; }

        public LandInfo() 
        {
        
        }

        public LandInfo(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}
