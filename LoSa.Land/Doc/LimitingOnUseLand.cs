using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using LoSa.Xml;
using LoSa.Utility;

namespace LoSa.Land.Doc
{
    public class LimitingOnUseLand
    {
        [XmlAttributeAttribute()]
        public string Code { get; set; }

        public string Name { get; set; }

        public static List<LimitingOnUseLand> GetLimitingOnUseLandFromText()
        {
            string filePath = new LocalPath("LoSa_Land").FindLocalPathFromXml("PathLimitingOnUseLand").Name
                                                                        .Replace(".xml", ".txt");
            StreamReader streamReader = new StreamReader(filePath, Encoding.GetEncoding(1251));

            List<LimitingOnUseLand> list = new List<LimitingOnUseLand>();
            string strBuff;
            while ((strBuff = streamReader.ReadLine()) != null)
            {
                string[] mas = strBuff.Split('-');

                LimitingOnUseLand limitingOnUseLand = new LimitingOnUseLand();
                limitingOnUseLand.Code = strBuff.Split('-')[0]; 
                limitingOnUseLand.Name = strBuff.Split('-')[1];
                list.Add(limitingOnUseLand);
            }
            return list;
        }

        public static List<LimitingOnUseLand> GetLimitingOnUseLandFromXml()
        {
            return ServiceXml.ReadXml<List<LimitingOnUseLand>>
                (new LocalPath("LoSa_Land").FindLocalPathFromXml("PathLimitingOnUseLand").Name);
        }
       
    }
}

