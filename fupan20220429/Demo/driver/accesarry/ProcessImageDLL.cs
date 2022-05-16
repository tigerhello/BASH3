#define Test
#undef Test

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThridLibray;
using Demo.ui;
using Demo.ui.model;
using Demo.ui.view.page;
using System.Windows;

namespace Demo.driver.cam
{
    public struct BohrImage
    {
        public byte[] image_data;
        public int width;  // 图像宽度
        public int height; // 图像高度
        public int channel;
    }

    public class ProcessImageDLL
    {
        [DllImport(@"GetCalibrationPoint.dll", EntryPoint = "GetASN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetASN(byte[] srcImg, int width, int height, int channel, bool isDetect, StringBuilder savepath, StringBuilder jsonpath);

        [DllImport(@"GetCalibrationPoint.dll", EntryPoint = "GetLiftAPoint", CharSet = CharSet.Ansi)]
        public static extern string GetLiftAPoint(byte[] srcImg, int width, int height, int channel, bool isDetect, StringBuilder savepath, StringBuilder jsonpath);

        [DllImport(@"GetCalibrationPoint.dll", EntryPoint = "GetLiftDPoint", CharSet = CharSet.Ansi)]
        public static extern string GetLiftDPoint(byte[] srcImg, int width, int height, int channel, bool isDetect, StringBuilder savepath, StringBuilder jsonpath);

        [DllImport(@"GetCalibrationPoint.dll", EntryPoint = "GetRightAPoint", CharSet = CharSet.Ansi)]
        public static extern string GetRightAPoint(byte[] srcImg, int width, int height, int channel, bool isDetect, StringBuilder savepath, StringBuilder jsonpath);

        [DllImport(@"GetCalibrationPoint.dll", EntryPoint = "GetRightDPoint", CharSet = CharSet.Ansi)]
        public static extern string GetRightDPoint(byte[] srcImg, int width, int height, int channel, bool isDetect, StringBuilder savepath, StringBuilder jsonpath);


        public delegate void ProcessImageCallBack(InputInfo info);//声明
        public ProcessImageCallBack ProcessImage_CallBack;




        List<TestResModel> FiltDefault(List<TestResModel> RawDefault, double MinV, double MaxV, int TypeV,ref List<TestResModel> TrueDefault, double MinVBeckupH = 0, double MaxVBeckupH = 0)
        {
            double InMin = (double)MinV;
            double InMax = (double)MaxV;

            if (RawDefault == null || RawDefault.Count <= 0) return new List<TestResModel>();

            if (InMin > InMax) return new List<TestResModel>();

            List<TestResModel> ListTr = new List<TestResModel>();

            foreach (TestResModel tr in RawDefault)
            {
                switch (TypeV)
                {
                    case 0:
                        if (tr.area >= MaxV && Math.Abs(tr.mean_background - tr.mean_foreground)>= MinVBeckupH)
                        {
                            TrueDefault.Add(tr);
                          
                        }

                       else  if(tr.area< MaxV && tr.area> MinV)
                        {
                            ListTr.Add(tr);
                        }
                        break;
                    case 1:
                        if (tr.height < InMax && tr.height > InMin)
                        {
                            ListTr.Add(tr);
                        }
                        break;
                    case 2:
                        if (tr.width < InMax && tr.width > InMin)
                        {
                            ListTr.Add(tr);
                        }
                        break;
                    case 3:
                        if (tr.category_name.ToLower().Contains("hair") || tr.category_name.ToLower().Contains("wool"))
                        {
                            double ErrorrWool = Math.Abs(tr.mean_background - tr.mean_foreground);
                            if ((tr.area >= 5000 && ErrorrWool >= 15) || (tr.area >= 1000 && ErrorrWool >= 30))
                            {
                                TrueDefault.Add(tr);
                            }
                        }

                        break;
                    case 4:
                        double Errorr = Math.Abs(tr.mean_background - tr.mean_foreground);

                        if (Math.Log10(Errorr* tr.area)>=3.9)
                        {
                            TrueDefault.Add(tr);
                        }
                        break;
                    case 5:
                        if (tr.bbox[2] < InMax && tr.bbox[2] > InMin)
                        {
                            ListTr.Add(tr);
                        }
                        break;
                    case 6:
                        if (tr.bbox[3] < InMax && tr.bbox[3] > InMin)
                        {
                            ListTr.Add(tr);
                        }
                        break;
                    case 7:
                        if (tr.category_name.ToLower().Contains("black spot") || tr.category_name.ToLower().Contains("black_spot"))
                        {
                            ListTr.Add(tr);
                        }
                        break;
                    case 8:
                        if (tr.category_name.ToLower().Contains("scratch")|| tr.category_name.ToLower().Contains("black spot") || tr.category_name.ToLower().Contains("black_spot"))
                        {
                            ListTr.Add(tr);
                        }
                        break;
                    case 9:
                        if (tr.category_name.ToLower().Contains("dirt"))
                        {
                            ListTr.Add(tr);
                        }
                        break;
                    case 10:
                        if (tr.category_name.ToLower().Contains("gap") && tr.area>3000)
                        {
                            ListTr.Add(tr);
                        }
                        break;
                    case 11:
                        double Comp = 0;
                        if (tr.bbox[3] <= tr.bbox[2])
                        {
                             InMin = (double)MinV;
                             InMax = (double)MaxV;
                             Comp = tr.bbox[2];
                        }
                        else
                        {
                            InMin = (double)MinVBeckupH;
                            InMax = (double)MaxVBeckupH;
                            Comp = tr.bbox[3];
                        }

                        if(InMin <= Comp && InMax >=Comp)
                        {
                            TrueDefault.Add(tr);
                        }
                        break;
                    case 12:
                        double Errorr22 = Math.Abs(tr.mean_background - tr.mean_foreground);

                        if (Errorr22 >=6)
                        {
                            ListTr.Add(tr);
                        }
                        break;
                    case 16:
                        if (tr.category_name.ToLower().Contains("color_error"))
                        {
                            TrueDefault.Add(tr);
                        }
                        break;
                }
            }

            return ListTr;
        }


        //public Dictionary<string, List<string>> DiatanceCountUnite = new Dictionary<string, List<string>>();

        Dictionary<string, List<TestResModel>> DistanceCC(List<TestResModel> RawData, double StdArea, double StdDis ,double mmPerPixel, double StdAreaBackupMax)
        {
            Dictionary<string, List<TestResModel>> DiatanceCountUnite = new Dictionary<string, List<TestResModel>>();

            for (int i = 0; i < RawData.Count; i++)
            {
                if (RawData[i].area< StdArea || RawData[i].area > StdAreaBackupMax || (!RawData[i].category_name.ToLower().Contains("black spot") && !RawData[i].category_name.ToLower().Contains("black_spot")))
                {
                    continue;
                }

                if (!DiatanceCountUnite.ContainsKey(i.ToString()))
                    DiatanceCountUnite.Add(i.ToString(), new List<TestResModel>());

                for (int ij = 0; ij <RawData.Count; ij++)
                {
                    if (RawData[ij].area < StdArea || RawData[ij].area > StdAreaBackupMax || (!RawData[ij].category_name.ToLower().Contains("black spot") && !RawData[ij].category_name.ToLower().Contains("black_spot")))
                    {
                        continue;
                    }

                    double temp = (Math.Pow(RawData[ij].bbox[0] - RawData[i].bbox[0], 2) + Math.Pow(RawData[ij].bbox[1] - RawData[i].bbox[1], 2));

                    if(temp <= Math.Pow(StdDis / mmPerPixel, 2))
                    {
                        DiatanceCountUnite[i.ToString()].Add(RawData[ij]);
                    }                  
                }


                if (DiatanceCountUnite[i.ToString()].Count <= 1) DiatanceCountUnite.Remove(i.ToString());
            }
        
            return DiatanceCountUnite;
        }

        Dictionary<string, List<TestResModel>> DistanceTinyP(List<TestResModel> RawData, double StdArea, double StdDis, double mmPerPixel, double StdAreaBackupMax)
        {
            Dictionary<string, List<TestResModel>> DiatanceCountUnite = new Dictionary<string, List<TestResModel>>();

            for (int i = 0; i < RawData.Count; i++)
            {
                if (RawData[i].area < StdArea || RawData[i].area > StdAreaBackupMax || (!RawData[i].category_name.ToLower().Contains("black spot") && !RawData[i].category_name.ToLower().Contains("black_spot")))
                {
                    continue;
                }

                if (!DiatanceCountUnite.ContainsKey(i.ToString()))
                    DiatanceCountUnite.Add(i.ToString(), new List<TestResModel>());

                for (int ij = 0; ij < RawData.Count; ij++)
                {
                    if (RawData[ij].area < StdArea || RawData[ij].area > StdAreaBackupMax || (!RawData[ij].category_name.ToLower().Contains("black spot") && !RawData[ij].category_name.ToLower().Contains("black_spot")))
                    {
                        continue;
                    }

                    double temp = (Math.Pow(RawData[ij].bbox[0] - RawData[i].bbox[0], 2) + Math.Pow(RawData[ij].bbox[1] - RawData[i].bbox[1], 2));

                    if (temp <= Math.Pow(StdDis / mmPerPixel, 2))
                    {
                        DiatanceCountUnite[i.ToString()].Add(RawData[ij]);
                    }
                }


                if (DiatanceCountUnite[i.ToString()].Count <= 2) DiatanceCountUnite.Remove(i.ToString());
            }

            return DiatanceCountUnite;
        }
        public void SaveSrcBitmap(string path, IGrabbedRawData src, int fl = 1)
        {
            Task.Factory.StartNew(() => {

                Bitmap bitmap = null;

                if (fl == 1)
                    bitmap = src.ToBitmap(false);
                if (fl == 3)
                    bitmap = src.ToBitmap(true);
                if(null!=bitmap) //2021-0427 增加保存条件
                {
                    bitmap.Save(path, ImageFormat.Bmp);
                }
            });
        }


        public static object OBBJ = new object();


        public void SaveImgINfo0(object u)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                /*Task.Factory.StartNew(() =>*/
                {
                    sw.Restart();
               
                    InputInfo info1 = (InputInfo)u;

                    string ImageDir = info1._SNDir + @"\Image\";
                    string ImagePath = ImageDir + info1._StationCode + "-" + info1.Num + "-" + info1._CameraNo + ".bmp";

                    if (System.IO.Directory.Exists(ImageDir) == false)//如果不存在就创建file文件夹
                    {
                        Directory.CreateDirectory(ImageDir); //新建文件夹  
                    }

                    FileStream fsBe0 = new FileStream(ImagePath, FileMode.Create);
                    fsBe0.Write(info1._imge, 0, info1._imge.Length);
                    fsBe0.Dispose();
                    sw.Stop();

                    //MemoryStream ms = new MemoryStream();
                    //FileStream file = new FileStream("file.bin", FileMode.Create, FileAccess.Write);
                    //ms.WriteTo(file);
                    //file.Close();
                    //ms.Close();

                    //MessageBox.Show("存图："+sw.ElapsedMilliseconds.ToString());

                }//);
            }
            catch (Exception ex)
            {
             
                MessageBox.Show(ex.ToString());
            
            }


        
        }
    }
}
