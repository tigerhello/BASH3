using System.Windows;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Threading;

namespace Demo.ui.model
{
    public class TestResModel 
    {
        public int category_id { get; set; }//0 表示毛刺  1 表示其他缺陷
        public double area { get; set; }//求面积
        public double score { get; set; }// 求分数

        public double width { get; set; }//求面积
        public double height { get; set; }// 求分数

        public double mean_background { get; set; }//求面积
        public double mean_foreground { get; set; }// 求分数

        public double mean { get; set; }//求面积


        public List<double> bbox = new List<double>();//缺陷框

        public string category_name { get; set; }//0 表示毛刺  1 表示其他缺陷

        public string Name { get; set; }

    }

    [Serializable]
    public class ListFlow
    {
        public  double[] DBMinArea = new double[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public  double[] DBMaxArea = new double[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public  double[] DBMinHeight = new double[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public  double[] DBMaxHeight = new double[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public  double[] DBMinWidth = new double[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public  double[] DBMaxWidth = new double[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public  double[] DBThreshhold = new double[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public double[] DBMinCompare = new double[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public double[] DBMaxCompare = new double[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static void Serialize(string strFile, bool bSave, ref ListFlow p)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ListFlow));
            if (bSave || System.IO.File.Exists(strFile) == false)
            {
                Stream stream = new FileStream(strFile, FileMode.Create, FileAccess.Write, FileShare.Read);
                xs.Serialize(stream, p);
                stream.Close();
            }

            else
            {
                try
                {
                    if (System.IO.File.Exists(strFile))
                    {
                        Stream stream = new FileStream(strFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                        p = (ListFlow)xs.Deserialize(stream);
                        stream.Close();
                    }

                }
                catch (FileNotFoundException)
                {

                }
            }
        }
    }
    }
