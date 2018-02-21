
using LoSa.Land.Service;
using LoSa.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace LoSa.Land.Tables
{
    [Serializable()]
    [DataContract]
    public class SettingsLand
    {
        [XmlElementAttribute("Setting")]
        public List<SettingLand> Setting { get; set; }

        public SettingsLand()
        {
            this.Setting = new List<SettingLand>();
        }

        public SettingLand FindKey(string key)
        {
            return Setting.Find
                (
                    delegate(SettingLand landSetting)
                    {
                        return landSetting.Key == key;
                    }
                );
        }

        public SettingLand FindName(string name)
        {
            return Setting.Find
                (
                    delegate(SettingLand landSetting)
                    {
                        return landSetting.Name == name;
                    }
                );
        }

        public static SettingsLand Default
        {
            get
            {
                SettingsLand setsLandDefault = new SettingsLand();
                setsLandDefault.Setting.Add(new SettingLand("key", "name", "description"));

                return setsLandDefault;
            }
        }

        public static void TextToXml(string pathText, string pathXml)
        {
            StreamReader streamReader = new StreamReader(pathText, Encoding.GetEncoding(1251));
            List<SettingLand> list = new List<SettingLand>();
            string strBuff;
            while ( ( strBuff = streamReader.ReadLine() ) != null )
            {
                SettingLand settingLand = new SettingLand();

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
            ServiceXml.WriteXml<List<SettingLand>>(list, pathXml);
        }
    }

    [Serializable()]
    public class SettingLand
    {
        [XmlAttributeAttribute()]
        public string Key { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public SettingLand()
        {

        }

        public SettingLand(string key, string name, string description)
        {
            this.Key = key;
            this.Name = name;
            this.Description = description;
        }

        public static SettingLand Default 
        {
            get
            {
                SettingLand setsDefault = new SettingLand();
                setsDefault.Key = "key";
                setsDefault.Name = "name";
                setsDefault.Description = "description";
                return setsDefault;
            }
        }
    } 
}