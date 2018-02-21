using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

using LoSa.Xml;

namespace LoSa.Utility
{
    [Serializable()]
    [DataContract]
    public class Settings
    {
        [XmlElementAttribute("Setting")]
        public List<Setting> Setting { get; set; }

        public Settings()
        {
            this.Setting = new List<Setting>();
        }

        public Setting FindKey(string key)
        {
            return Setting.Find
                (
                    delegate(Setting landSetting)
                    {
                        return landSetting.Key == key;
                    }
                );
        }

        public Setting FindName(string name)
        {
            return Setting.Find
                (
                    delegate(Setting setting)
                    {
                        return setting.Name == name;
                    }
                );
        }

        public static Settings Default
        {
            get
            {
                Settings setsLandDefault = new Settings();
                setsLandDefault.Setting.Add(new Setting("key", "name", "description"));

                return setsLandDefault;
            }
        }

        public static void TextToXml(string pathText, string pathXml)
        {
            StreamReader streamReader = new StreamReader(pathText, Encoding.GetEncoding(1251));
            List<Setting> list = new List<Setting>();
            string strBuff;
            while ( ( strBuff = streamReader.ReadLine() ) != null )
            {
                Setting settingLand = new Setting();

                string[] aBuff = strBuff.Split(new char[] { '-' }, 3);
                if (aBuff.Length < 2)
                {
                    settingLand.Key = aBuff[0];
                    settingLand.Name = "*";
                    settingLand.Description = "*";
                }
                else if (aBuff.Length < 3)
                {
                    settingLand.Key = aBuff[0];
                    settingLand.Name = aBuff[1];
                    settingLand.Description = "*";
                }
                else
                {
                    settingLand.Key = aBuff[0];
                    settingLand.Name = aBuff[1];
                    settingLand.Description = aBuff[2];
                }
                list.Add(settingLand);
            }
            ServiceXml.WriteXml<List<Setting>>(list, pathXml);
        }
    }

    [Serializable()]
    public class Setting
    {
        [XmlAttributeAttribute()]
        public string Key { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Setting()
        {

        }

        public Setting(string key, string name, string description)
        {
            this.Key = key;
            this.Name = name;
            this.Description = description;
        }

        public static Setting Default 
        {
            get
            {
                Setting setsDefault = new Setting();
                setsDefault.Key = "key";
                setsDefault.Name = "name";
                setsDefault.Description = "description";
                return setsDefault;
            }
        }
    } 
}