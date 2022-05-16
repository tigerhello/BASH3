using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading.Tasks;
using System.Xml;


namespace Demo.repository.BPfile
{
    public class cXMLConfig
    {
        private string sNAME;
        private XmlDocument xDOC;

        public string FileName
        {
            get { return sNAME; }
            set { sNAME = value; }
        }

        public bool OpenXml()
        {
            try { xDOC.Load(sNAME); return true; }
            catch (System.IO.FileNotFoundException)
            {
                XmlTextWriter x_Writer = new XmlTextWriter(sNAME, System.Text.Encoding.UTF8);
                x_Writer.Formatting = Formatting.Indented;
                x_Writer.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                x_Writer.WriteStartElement("System_Root");
                x_Writer.Close();
                xDOC.Load(sNAME);
                return false;
            }
        }

        public cXMLConfig(string sFileName)
        {
            FileName = sFileName;
            xDOC = new XmlDocument();
            this.OpenXml();
        }

        ~cXMLConfig()
        {
            SaveXml();
        }

        public void SaveXml()
        {
            xDOC.Save(sNAME);
        }

        public string GetXmlValue(string Name)
        {
            XmlNode x_Node = xDOC.SelectSingleNode("System_Root").SelectSingleNode(Name);
            if (x_Node == null) { return ""; }
            else { return x_Node.InnerText; }
        }

        public void SetXmlValue(string Name, string Value)
        {
            XmlNode x_Root = xDOC.SelectSingleNode("System_Root");
            XmlNode x_Node = x_Root.SelectSingleNode(Name);
            if (x_Node != null) { x_Node.InnerText = Value; }
            else
            {
                XmlElement x_Elem = xDOC.CreateElement(Name);
                x_Elem.InnerText = Value;
                x_Root.AppendChild(x_Elem);
            }

            SaveXml();
        }
    }

    public class IniFile
    {
        public string Path;            //文件INI名称 
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        //类的构造函数，传递INI文件名 
        public IniFile(string inipath)
        {
            Path = inipath;
        }

        public IniFile()
        {
          
        }

        //写INI文件 
        public void WriteObjToIni(string Section, string Key, object  Value, string filePath)
        {
            WritePrivateProfileString(Section, Key, Value.ToString(), filePath);
        }

        //读取INI文件指定 
        public long ReadObjFromIni(string Section, string Key, string filePath)
        {
            try
            {
                StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, filePath);
            return Convert.ToInt64(temp.ToString());
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public double ReadDBFromIni(string Section, string Key, string filePath)
        {
            try
            {
                StringBuilder temp = new StringBuilder(255);
                int i = GetPrivateProfileString(Section, Key, "", temp, 255, filePath);
                return Convert.ToDouble(temp.ToString());
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        public string ReadStringFromIni(string Section, string Key, string filePath)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, filePath);
            return temp.ToString();
        }


        // 验证文件是否存在，返回布尔值
        public bool ExistINIFile()
        {
            return File.Exists(this.Path);
        }
    }

}