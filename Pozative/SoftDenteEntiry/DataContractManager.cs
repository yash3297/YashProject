using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Pozative
{

    public class DataContractManager
    {
        public static T DeserializeXMLString<T>(string xmlString) where T : class
        {
            try
            {
                xmlString = xmlString.Replace("\0", "");
                xmlString = xmlString.Replace("<CountryCode></CountryCode>", "");
                xmlString = xmlString.Replace("<CountryCode> </CountryCode>", "");
                xmlString = xmlString.Replace("<CountryCode>1</CountryCode>", "");
                xmlString = xmlString.Replace("<CountryCode> 1 </CountryCode>", "");
                //xmlString = xmlString.Replace("<CountryCode>", "");
                xmlString = DataContractManager.ReplaceNonPrintingChars(xmlString);
                
                //MessageBox.Show("DeserializeXMLString 1");
                //File.WriteAllText(Application.StartupPath.ToString() + "\\appointment.txt", xmlString.ToString());
                return new XmlSerializer(typeof(T)).Deserialize((Stream)new MemoryStream(DataContractManager.ByteArrayFromString(xmlString))) as T;
                
            }
            catch (Exception)
            {
                //LogManager.GetLogger("DataContractManager").LogError((object)ex);
               // LogManager.GetLogger("DataContractManager").LogError((object)xmlString);
                //MessageBox.Show("DeserializeXMLString_ err" + ex.Message.ToString());
                throw;
            }
        }


        public static T DeserializeXML<T>(IntPtr ptr) where T : class
        {
            string stringOrNull = InteropHelper.PtrToStringOrNull(ptr);
            //MessageBox.Show("DeserializeXML Function");
            if (stringOrNull == null)
                return default(T);
            //MessageBox.Show("Call to Desrialize");
            return DataContractManager.DeserializeXMLString<T>(stringOrNull);
        }

        public static string SerializeObject(object obj)
        {
            if (obj == null)
                return (string)null;
            StringBuilder output = new StringBuilder();
            using (XmlWriter xmlWriter = XmlWriter.Create(output))
                new XmlSerializer(obj.GetType()).Serialize(xmlWriter, obj);
            return output.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
        }

        public static void SerializeObjectAndSaveToFile(object obj)
        {
            DataContractManager.SerializeObjectAndSaveToFile(obj, "C:\\object.xml");
        }

        public static void SerializeObjectAndSaveToFile(object obj, string path)
        {
            string str = DataContractManager.SerializeObject(obj);
            TextWriter textWriter = (TextWriter)new StreamWriter(path);
            textWriter.WriteLine(str);
            textWriter.Close();
        }

        private static byte[] ByteArrayFromString(string inputString)
        {
            return new UTF8Encoding().GetBytes(inputString);
        }

        private static string ReplaceNonPrintingChars(string text)
        {
            foreach (char nonPrintingChar in DataContractManager.GetNonPrintingChars())
            {
                char[] chArray = new char[1] { nonPrintingChar };
                text = text.Replace(new string(chArray), "");
            }
            return text;
        }

        private static char[] GetNonPrintingChars()
        {
            return new char[31]
      {
        char.MinValue,
        '\x0001',
        '\x0002',
        '\x0003',
        '\x0004',
        '\x0005',
        '\x0006',
        '\a',
        '\b',
        '\t',
        '\v',
        '\f',
        '\r',
        '\x000E',
        '\x000F',
        '\x0010',
        '\x0011',
        '\x0012',
        '\x0013',
        '\x0014',
        '\x0015',
        '\x0016',
        '\x0017',
        '\x0018',
        '\x0019',
        '\x001A',
        '\x001B',
        '\x001C',
        '\x001D',
        '\x001E',
        '\x001F'
      };
        }
    }

}
