
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using ThridLibray;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

namespace Demo.driver.cam
{
    public enum CameraStatus
    {
        InitFailed = -2,
        Destroyed = -1,
        Uninit = 0,
        OK = 1,
        Grabing = 2,
    }

    public enum Image_Pixel_Format//像素类型
    {
        Mono8 = 1,//黑色
        Rgb = 3,//彩色
    }

    public delegate void UpdatePicBox(byte[] bmp, int Count);
    public delegate void StreamCallBack(IGrabbedRawData e, int Count);
    public delegate void MessageCallBack(string Msg);//声明
    public delegate void ServerConnectedCallBack();
    public delegate void ServerLossCallBack();
    public class DahuaG
    {
        public event StreamCallBack _CallBack;
        private IDevice m_dev;
        private CameraStatus m_Status = CameraStatus.Destroyed;

        private System.Threading.Mutex m_mutex = new System.Threading.Mutex();
        public string m_CameraName = "";
        private bool m_RGBChannel = false;
        private int m_CameraIndex = -1;
        private int m_frameCount = 0;
        private bool m_Trigger = true;

        const int DEFAULT_INTERVAL = 40;
        Stopwatch m_stopWatch = new Stopwatch();    /* 时间统计器 */

        #region 日志
        private static string path = "CameraLog\\";
        private static string name = "CCD_Dahua_";

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="message">log 信息</param>
        /// <param name="path">log 路径 </param>
        /// <param name="name">log日志名称</param>
        public static void Log(string message, string path, string name, int CameraIndex)
        {
            return;

            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string __stringFileName = path + name + CameraIndex.ToString() + ".log";
                using (TextWriter logFile = TextWriter.Synchronized(File.AppendText(__stringFileName)))
                {
                    logFile.WriteLine(DateTime.Now + "  Message[" + message + "];");
                    logFile.Flush();
                    logFile.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public DahuaG(int cameraindex, Image_Pixel_Format format, bool TriggerMode = true)
        {
            int nums = Enumerator.EnumerateDevices().Count;
            if (cameraindex < 0 || cameraindex > nums - 1)
            {
                m_Status = CameraStatus.Destroyed;
                string str = "DahuaG相机[" + m_CameraIndex.ToString() + "]索引越界，请检查";
                Log(str, path, name, cameraindex);
            }
            else
            {
                m_CameraIndex = cameraindex;
                m_Trigger = TriggerMode;
                if (format == Image_Pixel_Format.Mono8) m_RGBChannel = false;
                else m_RGBChannel = true;

                try
                {
                    m_dev = Enumerator.GetDeviceByIndex(cameraindex);
                    m_CameraName = m_dev.DeviceKey.ToString();
                    m_dev.CameraOpened += OnCameraOpen;
                    m_dev.ConnectionLost += OnConnectLoss;
                    m_dev.CameraClosed += OnCameraClose;
                }
                catch (Exception)
                {
                    string str1 = "DahuaG相机[" + m_CameraIndex.ToString() + "]实例化失败";
                    Log(str1, path, name, m_CameraIndex);
                }

                m_Status = CameraStatus.Uninit;
                string trige = TriggerMode ? "硬触发" : "软触发";
                string chan = m_RGBChannel ? "3" : "1";
                string str = "DahuaG相机[" + m_CameraIndex.ToString() + "]实例化成功！参数信息[索引值:" + cameraindex.ToString() + ",触发模式:" + trige + ",通道数:" + chan + "]";
                Log(str, path, name, m_CameraIndex);
            }
        }

        public DahuaG(string cameraname, Image_Pixel_Format format, bool TriggerMode = true)
        {
            List<IDeviceInfo> infolists = Enumerator.EnumerateDevices();
            int count = 0;
            int index = -1;
            for (int i = 0; i < infolists.Count; i++)
            {
                if (cameraname == infolists[i].Key)
                {
                    count++;
                    index = i;
                    break;
                }
            }

            if (count != 1)
            {
                m_Status = CameraStatus.Destroyed;
                string str = "DahuaG相机找不到名字为[" + cameraname + "]的相机，请检查原因。";
                Log(str, path, name, index);
            }
            else
            {
                m_CameraIndex = index;
                m_Trigger = TriggerMode;
                if (format == Image_Pixel_Format.Mono8) m_RGBChannel = false;
                else m_RGBChannel = true;

                try
                {
                    m_dev = Enumerator.GetDeviceByIndex(index);
                    m_CameraName = m_dev.DeviceKey.ToString();
                    m_dev.CameraOpened += OnCameraOpen;
                    m_dev.ConnectionLost += OnConnectLoss;
                    m_dev.CameraClosed += OnCameraClose;
                }
                catch (Exception)
                {
                    string str1 = "DahuaG相机[" + m_CameraIndex.ToString() + "]实例化失败";
                    Log(str1, path, name, m_CameraIndex);
                }

                m_Status = CameraStatus.Uninit;
                string trige = TriggerMode ? "硬触发" : "软触发";
                string chan = m_RGBChannel ? "3" : "1";
                string str = "DahuaG相机[" + m_CameraIndex.ToString() + "]实例化成功！参数信息[索引值:" + index.ToString() + ",触发模式:" + trige + ",通道数:" + chan + "]";
                Log(str, path, name, m_CameraIndex);
            }
        }

        ~DahuaG()
        {

        }

        public bool OpenDahua()
        {
            if (m_dev == null) return false;
            if (m_Status == CameraStatus.Destroyed || m_Status == CameraStatus.OK || m_Status == CameraStatus.Grabing) return false;

            bool flag = false;
            int times = 0;
            try
            {
                do
                {
                    times++;
                    if (m_dev.Open())
                    {
                        break;
                    }
                    else
                    {
                        flag = false;
                        m_Status = CameraStatus.InitFailed;
                        string str = "DahuaG相机[" + m_CameraIndex.ToString() + "]打开失败,第" + times.ToString() + "次，正在尝试重连...";
                        Log(str, path, name, m_CameraIndex);
                    }
                    Thread.Sleep(100);
                }
                while (times <= 3);
                if (times > 3)
                {
                    m_Status = CameraStatus.InitFailed;
                    string str = "DahuaG相机[" + m_CameraIndex.ToString() + "]打开失败3次，请检查原因";
                    Log(str, path, name, m_CameraIndex);
                    return false;
                }
                else
                {
                    // m_dev.UserSet.Current = UserSetType.userSet1;
                    //if (m_Trigger)
                    //{
                    //    m_dev.TriggerSet.Open(TriggerSourceEnum.Line1);
                    //}
                    //else
                    //{
                    //    m_dev.TriggerSet.Open(TriggerSourceEnum.Software);
                    //}


                    /* 打开Trigger(Line1) */
                    //m_dev.TriggerSet.Open(TriggerSourceEnum.Line1);



                    ///* 设置图像格式 */
                    //using (IEnumParameter p = m_dev.ParameterCollection[ParametrizeNameSet.ImagePixelFormat])
                    //{
                    //    p.SetValue("Mono8");
                    //}

                    /* 设置曝光 */
                    using (IFloatParameter p = m_dev.ParameterCollection[ParametrizeNameSet.ExposureTime])
                    {
                        p.SetValue(1000);
                    }

                    /* 设置增益 */
                    using (IFloatParameter p = m_dev.ParameterCollection[ParametrizeNameSet.GainRaw])
                    {
                        p.SetValue(1.0);
                    }

                    ///* 设置缓存个数为8（默认值为16） */
                    //m_dev.StreamGrabber.SetBufferCount(8);

                    m_dev.StreamGrabber.ImageGrabbed += OnImageGrabbed;

                    /////* 开启码流 */
                    //if (m_dev.DeviceInfo.Key.Contains("39") )
                    //{
                    //    m_dev.GrabUsingGrabLoopThread();
                    //}


                    m_Status = CameraStatus.OK;
                    flag = true;

                    string str = "DahuaG相机[" + m_CameraIndex.ToString() + "]打开成功";
                    Log(str, path, name, m_CameraIndex);
                }
            }
            catch
            {
                m_Status = CameraStatus.InitFailed;
                string str = "DahuaG相机[" + m_CameraIndex.ToString() + "]打开失败，请检查原因";
                Log(str, path, name, m_CameraIndex);
                return false;
            }
            return flag;
        }

        public int SetCfg(string strFullFileName)
        {
            List<string> oErrPropertyList = new List<string>();

            return m_dev.LoadDeviceCfg(strFullFileName, ref oErrPropertyList);
        }
        public bool StartGrab()
        {
            if (m_dev == null) return false;
            if (m_Status != CameraStatus.OK) return false;
            try
            {
                m_dev.GrabUsingGrabLoopThread();
            }
            catch (Exception)
            {
                string str1 = "DahuaG相机[" + m_CameraIndex.ToString() + "]开启码流失败";
                Log(str1, path, name, m_CameraIndex);
                return false;
            }
            m_Status = CameraStatus.Grabing;
            string str = "DahuaG相机[" + m_CameraIndex.ToString() + "]开始取图";
            Log(str, path, name, m_CameraIndex);
            return true;
        }

        public bool StopGrab()
        {
            if (m_dev == null) return false;
            if (m_Status != CameraStatus.Grabing) return false;
            try
            {
                m_dev.ShutdownGrab();
            }
            catch (Exception)
            {
                string str1 = "DahuaG相机[" + m_CameraIndex.ToString() + "]停止码流失败";
                Log(str1, path, name, m_CameraIndex);
                return false;
            }
            m_Status = CameraStatus.OK;
            string str = "DahuaG相机[" + m_CameraIndex.ToString() + "]停止取图";
            Log(str, path, name, m_CameraIndex);
            return true;
        }

        public bool CloseDahua()
        {
            if (m_dev == null) return false;
            if (m_Status == CameraStatus.Destroyed || m_Status == CameraStatus.InitFailed || m_Status == CameraStatus.Uninit) return false;
            try
            {
                m_dev.ShutdownGrab();
                m_dev.StreamGrabber.ImageGrabbed -= OnImageGrabbed;
            }
            catch (Exception)
            {
                string str1 = "DahuaG相机[" + m_CameraIndex.ToString() + "]停止码流失败";
                Log(str1, path, name, m_CameraIndex);
                return false;
            }
            Thread.Sleep(100);
            try
            {
                m_dev.Close();
            }
            catch (Exception)
            {
                string str1 = "DahuaG相机[" + m_CameraIndex.ToString() + "]关闭失败";
                Log(str1, path, name, m_CameraIndex);
                return false;
            }
            string str = "DahuaG相机[" + m_CameraIndex.ToString() + "]关闭成功";
            Log(str, path, name, m_CameraIndex);
            return true;
        }

        public bool Dispose()
        {
            CloseDahua();
            Thread.Sleep(10);

            try
            {
                if (m_dev != null)
                {
                    m_dev.Dispose();
                    m_Status = CameraStatus.Destroyed;
                    m_dev = null;
                    string str = "DahuaG相机[" + m_CameraIndex.ToString() + "]释放成功";
                    Log(str, path, name, m_CameraIndex);
                }
            }
            catch (Exception)
            {
                string str = "DahuaG相机[" + m_CameraIndex.ToString() + "]释放失败";
                Log(str, path, name, m_CameraIndex);
                return false;
            }
            return true;
        }

        private bool isTimeToDisplay()
        {
            m_stopWatch.Stop();
            long m_lDisplayInterval = m_stopWatch.ElapsedMilliseconds;
            if (m_lDisplayInterval <= DEFAULT_INTERVAL)
            {
                m_stopWatch.Start();
                return false;
            }
            else
            {
                m_stopWatch.Reset();
                m_stopWatch.Start();
                return true;
            }
        }

        public void GrabSoftware()
        {
            if (m_dev == null)
            {
                string str = "DahuaG相机[" + m_CameraIndex.ToString() + "]未初始化";
                Log(str, path, name, m_CameraIndex);
                return;
            }
            if (m_Status != CameraStatus.Grabing)
            {
                string str = "DahuaG相机[" + m_CameraIndex.ToString() + "]未开启循环拍照";
                Log(str, path, name, m_CameraIndex);
                return;
            }
            try
            {
                m_dev.ExecuteSoftwareTrigger();
            }
            catch (Exception)
            {
                string str = "DahuaG相机[" + m_CameraIndex.ToString() + "]软触发异常";
                Log(str, path, name, m_CameraIndex);
                return;
            }
        }

        /* 相机打开回调 */
        private void OnCameraOpen(object sender, EventArgs e)
        {
            m_Status = CameraStatus.OK;
            string str = "DahuaG相机[" + m_CameraIndex.ToString() + "]打开成功(OnCameraOpen回调)";
            Log(str, path, name, m_CameraIndex);
        }

        /* 相机关闭回调 */
        private void OnCameraClose(object sender, EventArgs e)
        {
            m_Status = CameraStatus.Uninit;
            string str = "DahuaG相机[" + m_CameraIndex.ToString() + "]关闭成功(OnCameraClose回调)";
            Log(str, path, name, m_CameraIndex);
        }

        /* 相机丢失回调 */
        private void OnConnectLoss(object sender, EventArgs e)
        {
            m_Status = CameraStatus.Destroyed;
            m_dev.ShutdownGrab();
            m_dev.Dispose();
            m_dev = null;
            string str = "DahuaG相机[" + m_CameraIndex.ToString() + "]丢失，请检查原因(OnCameraLoss回调)";
            Log(str, path, name, m_CameraIndex);
        }

        private void OnImageGrabbed(Object sender, GrabbedEventArgs e)
        {
            _CallBack(e.GrabResult.Clone(), m_frameCount++);
        }

        public CameraStatus GetDahuaStatus()
        {
            return m_Status;
        }

        public int GetDahuaCameraList(ref List<string> NameLists)
        {
            List<IDeviceInfo> lists = Enumerator.EnumerateDevices();
            int nums = lists.Count;
            if (nums == 0)
            {
                NameLists = new List<string>();
            }
            else if (nums > 0)
            {
                for (int i = 0; i < nums; i++)
                {
                    string str = lists[i].Key.ToString();
                    NameLists.Add(str);
                }
            }
            return nums;
        }

        //false 软触发，true 硬触发 默认接线line1
        public void SetTriggerMode(bool LineTri)
        {
            /* 打开Software Trigger */
            if (m_dev == null || m_Status != CameraStatus.OK) return;
            if (!LineTri)
            {
                m_dev.TriggerSet.Open(TriggerSourceEnum.Software);//软触发
                string str = "DahuaG相机[" + m_CameraIndex.ToString() + "]修改为软触发";
                Log(str, path, name, m_CameraIndex);
            }
            else
            {
                m_dev.TriggerSet.Open(TriggerSourceEnum.Line1);//硬触发，选择线
                string str = "DahuaG相机[" + m_CameraIndex.ToString() + "]修改为硬触发";
                Log(str, path, name, m_CameraIndex);
            }
        }

        public void SetExpose(int expo)
        {
            /* 设置曝光 */
            if (m_dev == null || m_Status != CameraStatus.OK) return;
            using (IFloatParameter p = m_dev.ParameterCollection[ParametrizeNameSet.ExposureTime])
            {
                p.SetValue((double)expo);
                string str = "DahuaG相机[" + m_CameraIndex.ToString() + "]修改曝光时间为" + expo.ToString();
                Log(str, path, name, m_CameraIndex);
            }
        }

        public void SetGain(int gain)
        {
            /* 增益 */
            if (m_dev == null || m_Status != CameraStatus.OK) return;
            using (IFloatParameter p = m_dev.ParameterCollection[ParametrizeNameSet.GainRaw])
            {
                p.SetValue((double)gain);
                string str = "DahuaG相机[" + m_CameraIndex.ToString() + "]修改增益为" + gain.ToString();
                Log(str, path, name, m_CameraIndex);
            }
        }
        //获取曝光
        public int GetExpose()
        {
            if (m_dev == null) return -1;
            using (IFloatParameter p = m_dev.ParameterCollection[ParametrizeNameSet.ExposureTime])
            {
                return (int)p.GetValue();
            }
        }
        //获取增益
        public int GetGain()
        {
            if (m_dev == null) return -1;
            using (IFloatParameter p = m_dev.ParameterCollection[ParametrizeNameSet.GainRaw])
            {
                return (int)p.GetValue();
            }
        }

        public void GetMinMaxExpo(ref int min, ref int max)
        {
            if (m_dev == null) return;
            using (IFloatParameter p = m_dev.ParameterCollection[ParametrizeNameSet.ExposureTime])
            {
                min = (int)p.GetMinimum();
                max = (int)p.GetMaximum();
            }
        }

        public void GetMinMaxGain(ref int min, ref int max)
        {
            if (m_dev == null) return;
            using (IFloatParameter p = m_dev.ParameterCollection[ParametrizeNameSet.GainRaw])
            {
                min = (int)p.GetMinimum();
                max = (int)p.GetMaximum();
            }
        }

        public string GetCameraName()
        {
            if (m_dev == null) return "";
            else return m_CameraName;
        }

    }

    public struct CameraInfo_SN
    {
        public string _sn;
        public double _x;
        public double _y;
        public double _angle;

        public static byte[] GetBGRValues(Bitmap bmp, out int stride)
        {
            var rect = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);
            var bmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);
            stride = bmpData.Stride;
            //int channel = bmpData.Stride / bmp.Width; 
            var rowBytes = bmpData.Width * System.Drawing.Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
            var imgBytes = bmp.Height * rowBytes;
            byte[] rgbValues = new byte[imgBytes];
            IntPtr ptr = bmpData.Scan0;
            for (var i = 0; i < bmp.Height; i++)
            {
                Marshal.Copy(ptr, rgbValues, i * rowBytes, rowBytes);   //对齐
                ptr += bmpData.Stride; // next row
            }
            bmp.UnlockBits(bmpData);
            return rgbValues;
        }

        public static byte[] getByteStreamFromBitmap(int width, int height, int channel, Bitmap img)
        {
            byte[] bytes = new byte[width * height * channel];
            BitmapData im = img.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, img.PixelFormat);
            int stride = im.Stride;
            int offset = stride - width * channel;
            int length = stride * height;
            byte[] temp = new byte[stride * height];
            Marshal.Copy(im.Scan0, temp, 0, temp.Length);
            img.UnlockBits(im);

            int posreal = 0;
            int posscan = 0;
            for (int c = 0; c < height; c++)
            {
                for (int d = 0; d < width * channel; d++)
                {
                    bytes[posreal++] = temp[posscan++];
                }
                posscan += offset;
            }

            return bytes;
        }

        public static Bitmap getBitmapFromByteStream(byte[] imgByte, int imgH, int imgW, int channel)
        {
            //申请目标位图的变量，并将其内存区域锁定
            Bitmap bitmap = null;
            if (channel == 1)
            {
                bitmap = new Bitmap(imgW, imgH, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            }
            else if (channel == 3)
            {
                bitmap = new Bitmap(imgW, imgH, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            }
            // Bitmap bitmap = new Bitmap(imgW, imgH, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            BitmapData bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, imgW, imgH), ImageLockMode.WriteOnly, bitmap.PixelFormat);

            //获取图像参数
            int stride = bitmapData.Stride; //扫描线的宽度
            int offset = stride - imgW * channel;        //显示宽度与扫描线宽度的问题
            IntPtr iptr = bitmapData.Scan0;    //获取bitmapdata的内存起始位置
            int scanbytes = stride * imgH;   //用stride宽度，表示这是内存区域的大小

            int posScan = 0, posReal = 0;
            byte[] pixelvalues = new byte[scanbytes];

            if (0 != offset)
            {
                for (int x = 0; x < imgH; x++)
                {
                    for (int y = 0; y < imgW * channel; y++)
                    {
                        pixelvalues[posScan++] = imgByte[posReal++];
                    }
                    posScan += offset;
                }
                Marshal.Copy(pixelvalues, 0, iptr, scanbytes);
            }
            else
            {
                Marshal.Copy(imgByte, 0, iptr, scanbytes);
            }

            bitmap.UnlockBits(bitmapData);

            ////从伪彩改为灰度
            if (channel == 1)
            {
                var pal = bitmap.Palette;
                for (int j = 0; j < 256; j++)
                    pal.Entries[j] = System.Drawing.Color.FromArgb(j, j, j);
                bitmap.Palette = pal;
            }

            return bitmap;
        }
    }

    public struct SNPath
    {
        public string _SN { get; set; }
        public string _SNDir { get; set; }
        public SNPath deepCopy()
        {
            SNPath t = new SNPath();
            t._SN = this._SN;
            t._SNDir = this._SNDir;
            return t;
        }
    }

    public class InputInfo
    {
        public IGrabbedRawData _bmp { get; set; }

        public IGrabbedRawData _bmp2 { get; set; }

        public string _SNDir { get; set; }
        public UInt32 _SN { get; set; }

        public byte[] _imge { get; set; }

        public byte[] _imge2 { get; set; }

        public int _CameraNo { get; set; }

        public string _CameraType { get; set; }

        public int _width { get; set; }

        public int _height { get; set; }

        public string _StationCode { get; set; }

        public string _JResult { get; set; }
        public int _Chanel { get; set; }

        public int Num { get; set; }

        public string _Res { get; set; }

        public Bitmap BP4 { get; set; }

        public Bitmap BP3 { get; set; }
        public InputInfo deepcopy()
        {
            InputInfo t = new InputInfo();

            t._StationCode = this._StationCode;
            t._SNDir = this._SNDir;
            t._SN = this._SN;
            t._Res = this._Res;
            t._CameraNo = this._CameraNo;
            t._width = this._width;
            t._height = this._height;
            t._CameraType = this._CameraType;
            t.Num = this.Num;

            if (this._imge != null && this._imge.Length > 0)
            {
                t._imge = new byte[this._imge.Length];

                for (int ir = 0; ir < this._imge.Length; ir++)
                {
                    t._imge[ir] = this._imge[ir];
                }
            }

            if (this._imge2 != null && this._imge2.Length > 0)
            {
                t._imge2 = new byte[this._imge2.Length];

                for (int ir = 0; ir < this._imge2.Length; ir++)
                {
                    t._imge2[ir] = this._imge2[ir];
                }
            }
            return t;
        }

        public InputInfo()
        {
            _Res = "";
            _SN = 0;
            _SNDir = "";
            _width = 0;
            _height = 0;
            _CameraType = "";
            _StationCode = "";
        }

    }
}
