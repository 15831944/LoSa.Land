using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace LoSa.Xml
{
    public class ServiceXml
    {
        public static String GetStringXml<T>(T obj)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);
                return writer.ToString();
            }
        }

        public static void WriteXml<T>(T obj, String path)
        {
            XmlSerializer writer = new XmlSerializer(typeof(T));
            try
            {
                FileStream file = File.Create(path);
                writer.Serialize(file, obj);
                file.Close();
            }
            catch (FileNotFoundException fnfExc)
            {
                MessageBox.Show(fnfExc.Message,
                                "WriteXml -> FileNotFoundException",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message,
                                "WriteXml -> Exception",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error); ;
            }
        }

        public static T ReadXml<T>(String path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            try
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                XmlReader reader = XmlReader.Create(fs);
                T obj = (T)serializer.Deserialize(reader);
                fs.Close();
                return obj;
            }
            catch (FileNotFoundException fnfExc)
            {
                MessageBox.Show(fnfExc.Message,
                                "ReadXml -> FileNotFoundException",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error); 
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message,
                                "ReadXml -> Exception",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error); ;
            }
            return default(T);
        }
    }
}
