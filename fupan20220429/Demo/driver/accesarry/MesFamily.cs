using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Demo.driver.cam
{
   public class MesFamily
    {
        public string CreateWriteXmlStr(string StatusCode, string SerialNumber, string TimeStamp, string TestFixtureNumber,string TestHeadNumber)

        {

            XmlDocument doc = new XmlDocument();//实例化文档对象

            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);//设置声明 

            doc.AppendChild(dec);

            XmlElement root = doc.CreateElement("vCheckTester");//加入根节点 

            root.SetAttribute("xmlns", "Valor.vCheckTester.xsd");

            root.SetAttribute("Version", "4.0");

            root.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");

            root.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");

            doc.AppendChild(root);


            XmlElement student = doc.CreateElement("Unit");//插入一个student节点 

            student.SetAttribute("StatusCode", StatusCode);

            student.SetAttribute("SerialNumber", SerialNumber);

            student.SetAttribute("TimeStamp", TimeStamp);


            doc.DocumentElement.AppendChild(student);//将student节点连接在根节点上 


            if (StatusCode == "FAIL")
            {
                XmlElement studentFail = doc.CreateElement("Symptom");//插入一个student节点 

                studentFail.SetAttribute("Name", "A0");

                studentFail.SetAttribute("Type", "Unknown");

                student.AppendChild(studentFail);
            }


            XmlElement student2 = doc.CreateElement("Header");//插入一个student节点 

            student2.SetAttribute("TestFixtureNumber", TestFixtureNumber);

            student2.SetAttribute("TestHeadNumber", TestHeadNumber);



            student.AppendChild(student2);

            return doc.OuterXml.ToString();

        }
    }
}
