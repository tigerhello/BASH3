using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Data;

namespace Demo.driver.cam
{
    public class Tools
    {
        public static float levenshtein(string str1, string str2)
        {
            //计算两个字符串的长度。  
            int len1 = str1.Length;
            int len2 = str2.Length;
            //建立上面说的数组，比字符长度大一个空间  
            int[,] dif = new int[len1 + 1, len2 + 1];
            //赋初值，步骤B。  
            for (int a = 0; a <= len1; a++)
            {
                dif[a, 0] = a;
            }
            for (int a = 0; a <= len2; a++)
            {
                dif[0, a] = a;
            }
            //计算两个字符是否一样，计算左上的值  
            int temp;
            for (int i = 1; i <= len1; i++)
            {
                for (int j = 1; j <= len2; j++)
                {
                    if (str1[i - 1] == str2[j - 1])
                    {
                        temp = 0;
                    }
                    else
                    {
                        temp = 1;
                    }
                    //取三个值中最小的  
                    dif[i, j] = Math.Min(Math.Min(dif[i - 1, j - 1] + temp, dif[i, j - 1] + 1), dif[i - 1, j] + 1);
                }
            }
            //Console.WriteLine("字符串\"" + str1 + "\"与\"" + str2 + "\"的比较");

            ////取数组右下角的值，同样不同位置代表不同字符串的比较  
            //Console.WriteLine("差异步骤：" + dif[len1, len2]);
            //计算相似度  
            float similarity = 1 - (float)dif[len1, len2] / Math.Max(str1.Length, str2.Length);
            //Console.WriteLine("相似度：" + similarity);
            return similarity;
        }



        public static void SaveFileToCSV(System.Windows.Controls.DataGrid dg, string fPath, string Sig = "")
        {
            try
            {
                if (!File.Exists(fPath))
                {
                    File.Create(fPath).Close();
                }

                using (StreamWriter sw = new StreamWriter(fPath, false, Encoding.Default))
                {

                    //for (int i = 1; i < dg.Columns.Count; i++)
                    //{
                    //    string Str = dg.Columns[i].Header.ToString();
                    //    sw.Write(Str + ",");
                    //}
                    //sw.WriteLine();


                    for (int i = 0; i < dg.Items.Count; i++)
                    {

                        for (int j = 1; j < dg.Columns.Count; j++)
                        {
                            System.Windows.Controls.CheckBox check = dg.Columns[0].GetCellContent(dg.Items[i]) as System.Windows.Controls.CheckBox;
                            if (check.IsChecked == true)
                            {
                                //if (dg.Items[i] != null)//填充可见列数据  
                                //{
                                //    sw.Write(dg.Columns[j].GetCellContent(dg.Items[i]).ToString() + ",");
                                //}

                                if (dg.Items[i] != null && (dg.Columns[j].GetCellContent(dg.Items[i]) as System.Windows.Controls.TextBlock) != null)//填充可见列数据  
                                {
                                    string str = (dg.Columns[j].GetCellContent(dg.Items[i]) as System.Windows.Controls.TextBlock).Text.ToString();
                                    sw.Write(str + ",");
                                }
                                sw.WriteLine();
                            }


                        }

                    }


                    string[] Signature = Sig.Split(';');

                    if (Signature.Length > 0)
                    {
                        for (int k = 0; k < Signature.Length; k++)
                        {
                            sw.Write(Signature[k]);
                            sw.WriteLine();

                        }

                    }

                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }
        public static List<string> ReadTxt(string path)
        {
            StreamReader sr = new StreamReader(path, Encoding.UTF8);
            string line;
            List<string> config = new List<string>();
            while ((line = sr.ReadLine()) != null)
            {
                config.Add(line);
            }
            System.Threading.Thread.Sleep(50);
            return config;
        }

        public System.Drawing.Image DrawStr(System.Drawing.Image tableChartImage, string signature)
        {

            System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(tableChartImage);

            graph.DrawImage(tableChartImage, 0, 0);

            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Yellow);
            graph.DrawString(signature, new System.Drawing.Font("Arial", 48), drawBrush, 0, 0);
            drawBrush.Dispose();

            graph.Dispose();
            GC.Collect();
            return tableChartImage;
        }
        public byte[] StringToByteArray1(string s)
        {
            try
            {
                //List<byte> LS = new List<byte>();
                //foreach (char a in s)
                //{
                //    LS.Add((byte)int.Parse(a.ToString()));
                //}

                //return LS.ToArray();

                return System.Text.Encoding.Default.GetBytes(s);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DirDEL(string args,int DayKeep)
        {
            DirectoryInfo d = new DirectoryInfo(args);
            DirectoryInfo[] dics = d.GetDirectories();//获取文件夹

            foreach (DirectoryInfo file in dics)//遍历文件夹
            {
                if (file.CreationTime < DateTime.Now.AddDays(-1 * DayKeep))
                    file.Delete(true);
            }
        }
        public static void ExportDataGridToCSV(DataTable dt, string Path)
        {
            try
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(Path, false, Encoding.Default);

                //Tabel header       
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sw.Write(dt.Columns[i].ColumnName);
                    sw.Write(",");
                }

                sw.WriteLine();        //Table body     
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // System.Threading.Thread.Sleep(50);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        sw.Write(dt.Rows[i][j].ToString());
                        sw.Write(",");
                    }
                    sw.WriteLine();
                }
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void WriteCSVLine(string Path, string Content)
        {
            try
            {
                FileStream fs = new FileStream(Path, FileMode.Append, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(Content);
                sw.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Chr_utf8(int utf8Code)
        {
            try
            {
                if (utf8Code >= 0 && utf8Code <= 255)
                {
                    System.Text.UTF8Encoding asciiEncoding = new System.Text.UTF8Encoding();
                    byte[] byteArray = new byte[] { (byte)utf8Code };
                    string strCharacter = asciiEncoding.GetString(byteArray);
                    return (strCharacter);
                }
                else
                {
                    throw new Exception("utf8 Code is not valid.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public static string Listtosstr(List<byte> Lres)
        {
            try
            {
                string resultst = "";

                foreach (byte bb in Lres)
                {
                    resultst += bb.ToString("X2");
                }
                return resultst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static byte[] HexStringToByteArray(string s)
        {
            try
            {
                if (s.Length == 0)
                    throw new Exception("将16进制字符串转换成字节数组时出错，错误信息：被转换的字符串长度为0。");
                s = s.Replace(" ", "").Replace("\r\n", "");

                byte[] buffer = new byte[s.Length / 2];
                int i = 0;
                int j = 0;
                for (; i < s.Length; i += 2, j += 1)
                    buffer[j] = Convert.ToByte(s.Substring(i, 2), 16);
                return buffer;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static Bitmap BytesToBitmap(byte[] Bytes)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(Bytes);
                return new Bitmap((Image)new Bitmap(stream));
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            finally
            {
                stream.Close();
            }
        }


        public static byte[] BitmapToBytes(Bitmap Bitmap0)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                Bitmap0.Save(ms, ImageFormat.Bmp);
                byte[] byteImage = new Byte[ms.Length];
                byteImage = ms.ToArray();
                return byteImage;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            finally
            {
                ms.Close();
            }
        }

        public static byte[] BitmapToBytes1(Bitmap Bitmap0)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                Bitmap0.Save(ms, ImageFormat.Jpeg);
                byte[] byteImage = new Byte[ms.Length];
                byteImage = ms.ToArray();
                return byteImage;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            finally
            {
                ms.Close();
            }
        }

        public static Bitmap ReadImageFile(string path)
        {
            FileStream fs = File.OpenRead(path); //OpenRead
            int filelength = 0;
            filelength = (int)fs.Length; //获得文件长度 
            Byte[] image = new Byte[filelength]; //建立一个字节数组 
            fs.Read(image, 0, filelength); //按字节流读取 
            System.Drawing.Image result = System.Drawing.Image.FromStream(fs);
            fs.Close();
            Bitmap bit = new Bitmap(result);
            return bit;
        }

        public static Bitmap DeepClone(Bitmap bitmap)
        {
            Bitmap dstBitmap = null;
            using (MemoryStream mStream = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(mStream, bitmap);
                mStream.Seek(0, SeekOrigin.Begin);
                dstBitmap = (Bitmap)bf.Deserialize(mStream);
                mStream.Close();
            }
            return dstBitmap;
        }
        public static BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            BitmapImage bitmapImage = new BitmapImage();
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Bmp);
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            return bitmapImage;
        }
        public static byte[] IsValid(IntPtr hDevDropEvent)
        {
            UInt32 byLen = 0;
            byte[] bufHandle = new byte[IntPtr.Size];
            GCHandle gch = GCHandle.Alloc(bufHandle, GCHandleType.Pinned);    //固定托管内存
            Marshal.WriteIntPtr(Marshal.UnsafeAddrOfPinnedArrayElement(bufHandle, 0), hDevDropEvent);
            gch.Free();
            return bufHandle;
        }


        public  Bitmap RGB2Gray(Bitmap srcBitmap)

        {

            int wide = srcBitmap.Width;

            int height = srcBitmap.Height;

            Rectangle rect = new Rectangle(0, 0, wide, height);

            //将Bitmap锁定到系统内存中,获得BitmapData

            BitmapData srcBmData = srcBitmap.LockBits(rect,

                      ImageLockMode.ReadWrite, srcBitmap.PixelFormat);

            //创建Bitmap

            Bitmap dstBitmap =new Bitmap(wide, height, PixelFormat.Format16bppGrayScale); // CreateGrayscaleImage(wide, height);//这个函数在后面有定义

            BitmapData dstBmData = dstBitmap.LockBits(rect,

                      ImageLockMode.ReadWrite, PixelFormat.Format16bppGrayScale);

            //位图中第一个像素数据的地址。它也可以看成是位图中的第一个扫描行

            System.IntPtr srcPtr = srcBmData.Scan0;

            System.IntPtr dstPtr = dstBmData.Scan0;

            //将Bitmap对象的信息存放到byte数组中

            int src_bytes = srcBmData.Stride * height;

            byte[] srcValues = new byte[src_bytes];

            int dst_bytes = dstBmData.Stride * height;

            byte[] dstValues = new byte[dst_bytes];

            //复制GRB信息到byte数组

            System.Runtime.InteropServices.Marshal.Copy(srcPtr, srcValues, 0, src_bytes);

            System.Runtime.InteropServices.Marshal.Copy(dstPtr, dstValues, 0, dst_bytes);

            //根据Y=0.299*R+0.114*G+0.587B,Y为亮度

            for (int i = 0; i < height; i++)

                for (int j = 0; j < wide; j++)

                {

                    //只处理每行中图像像素数据,舍弃未用空间

                    //注意位图结构中RGB按BGR的顺序存储

                    int k = 3 * j;

                    byte temp = (byte)(srcValues[i * srcBmData.Stride + k + 2] *0.299

                         + srcValues[i * srcBmData.Stride + k + 1] * 0.587

                         + srcValues[i * srcBmData.Stride + k] * 0.114);

                    dstValues[i * dstBmData.Stride + j] = temp;

                }

            System.Runtime.InteropServices.Marshal.Copy(dstValues, 0, dstPtr, dst_bytes);

            //解锁位图

            srcBitmap.UnlockBits(srcBmData);

            dstBitmap.UnlockBits(dstBmData);

            return dstBitmap;

        }

        /// <summary>
        /// 连接远程共享文件夹
        /// </summary>
        /// <param name="path">远程共享文件夹的路径</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <returns></returns>
        public static bool connectState(string path, string userName, string passWord)
        {
            bool Flag = false;
            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                string dosLine = "net use " + path + " /User:" + userName + " " + passWord;
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                while (!proc.HasExited)
                {
                    proc.WaitForExit(1000);
                }
                string errormsg = proc.StandardError.ReadToEnd();
                proc.StandardError.Close();
                if (string.IsNullOrEmpty(errormsg))
                {
                    Flag = true;
                }
                else
                {
                    throw new Exception(errormsg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            return Flag;
        }

        /// <summary>
        /// 获取文件夹所有文件
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="path"></param>
        public static void GetFiles(string dir, List<string> path)
        {
            DirectoryInfo d = new DirectoryInfo(dir);
            FileSystemInfo[] fsinfos = d.GetFileSystemInfos();
            foreach (FileSystemInfo fsinfo in fsinfos)
            {
                if (fsinfo is DirectoryInfo)     //判断是否为文件夹
                {
                    GetFiles(fsinfo.FullName, path);//递归调用
                }
                else
                {
                    path.Add(fsinfo.FullName);
                }
            }
        }
    }


}

