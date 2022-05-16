using System;
using System.Windows;
using System.ComponentModel;
using log4net;
using Demo.ui.view.page;
using System.Threading;
using System.Windows.Threading;
using System.Threading.Tasks;
using Demo.utilities;
using System.Diagnostics;
using ThridLibray;
using Demo.driver.cam;
using Demo.repository.BPfile;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using Demo.ui.model;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using Newtonsoft.Json;
using System.Data.SQLite;
using Demo.ui.view.snippet;
using Demo.ui.view.card;
using System.Text.RegularExpressions;
using Demo.driver.accesarry;
using Tools = Demo.driver.cam.Tools;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Amib.Threading;

namespace Demo.ui
{
    public partial class MainFrame : Window
    {
        Server server = null;
        Server serverOCRCam = null;
        string TestFixtureNumber = "";
        string TestHeadNumber = "";
        string Project = "";
        string QPOSITION = "";
        public int MES_Count = 1;

        public MainFrame()
        {
            InitializeComponent();

            //  string strMsg_Temp = "1325074453";

            //string  TopGun = recordManager.Get_ALL_Record("TestResult", strMsg_Temp);

            //  //for (int kk = 0; kk <= 10000; kk++)
            //  //{
            //  //    Task.Factory.StartNew(() =>
            //  //    {
            //  //        recordManager.InsertRecord820v4("TestResult", kk.ToString(), "MES", "OK");

            //  //    });

            //  //    Thread.Sleep(30);

            //  //}

            //string addStr = recordManager.Get_ALLOCR_Record("TestResult", "F1621210WGQPM06B3", 0);

            //string[] aryLine = addStr.TrimEnd(',').Split(',');
            


            try
            {
                List<string> configMES = ReadTxt(@"data\MES.txt");
                TestFixtureNumber = configMES[0];
                Project = configMES[1];
                QPOSITION = configMES[2];

                //bool DllR = ThreadPool.SetMaxThreads(10, 50);

                App.Current.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                ShowM += new ChildThreadExceptionHandler_Record(RunStatus);
                //tubeNavigatorCard.IniImageRecord+= new view.card.TubeNavigatorCard.IniImage_Record(InitialCamCash);
                List<string> config = ReadTxt(@"data\config0.txt");
                config_Temp = config;

                for (int i = 0; i < 1; i++)
                {
                    server = new Server(config[i].Split(',')[0], config[i].Split(',')[1]);
                    server._CallBack += new MessageCallBack(ServerCallBack0);
                    //Server.dictConn.Add(config[i], null);
                }

                for (int i = 1; i < 4; i++)
                {
                    server.dictConn.Add(config[i], null);
                }


                serverOCRCam = new Server(config[4].Split(',')[0], config[4].Split(',')[1]);
                serverOCRCam._CallBack += new MessageCallBack(ServerCallBack1);

                //Server.dictConn.Add(config[i], null);


                SK = new SocketComm();

                //string RemoteR = SK.open_Com(config[5].Split(',')[0], config[5].Split(',')[1]);
                //SK._CallBack += new SocketComm.MessageCallBack(ServerCallBack2);
                //SK.NetBreak += new SocketComm.NETBreackCallBack(PerfoCl);

                Task.Factory.StartNew(() =>
                {
                    

                    //if (SK.sockClient.Connected == false)
                    //{
                    //}

                    //while (true) 
                    //{
                        if (SK.sockClient.Connected == false)
                        {
                            SK.Close_Com();

                            string RemoteR0 = SK.open_Com(config_Temp[5].Split(',')[0], config_Temp[5].Split(',')[1]);

                            SK._CallBack += new SocketComm.MessageCallBack(ServerCallBack2);
                            SK.NetBreak += new SocketComm.NETBreackCallBack(PerfoCl);


                            if (RemoteR0 == "0")
                            {
                                ListShowInfo(config_Temp[5] + "   " + "运动主机联机成功。\r\n");
                            }
                            else
                            {
                                ListShowInfo(config_Temp[5] + "   " + "运动主机联机失败。\r\n");
                            }
                        }

                        Thread.Sleep(1000);

                    //}

                });


            //if (RemoteR == "0")
            //{
            //    ListShowInfo(config[5] + "   " + "运动主机联机成功。\r\n");
            //}
            //else
            //{
            //    ListShowInfo(config[5] + "   " + "运动主机联机失败。\r\n");
            //}

            SaveFolder1 = config[6];

                Tools.DirDEL(SaveFolder1, 7);
            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show(ee.Message);
            }
        }

        public static List<string> config_Temp = new List<string>();
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) { }

        private void CurrentDomain_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //AutoRunPage.Log(e.ToString(), AutoRunPage.logpath, "ERROR");

            System.Windows.Forms.MessageBox.Show(e.ToString());
        }

        public static string Path2 = AppDomain.CurrentDomain.BaseDirectory + @"\DATA\custominfo.CSV";


        public static string ToFupan = "";



        private Thread mUpdateAlarmThread;

        public static int count = 0;
        public void ProcessInfoThread()//trueman 192.168.0.203:7960
        {
            while (true)
            {
                try
                {
                    if (LoaderDoSnippet.CamNub != "")
                    {
                        string tEMP = LoaderDoSnippet.CamNub;
                        LoaderDoSnippet.CamNub = "";
                        string tEMPRes = "";

                        switch (tEMP)
                        {
                            case "A":
                                tEMPRes = "A";
                                break;
                            case "B":
                                tEMPRes = "A";
                                break;
                            case "C0":
                                tEMPRes = "CR1";
                                break;
                            case "C1":
                                tEMPRes = "CR1";
                                break;
                            case "C2":
                                tEMPRes = "CR3";
                                break;
                            case "C3":
                                tEMPRes = "CR3";
                                break;
                            case "D":
                                tEMPRes = "A";
                                break;
                            case "E":
                                tEMPRes = "A";
                                break;
                            case "F":
                                tEMPRes = "B";
                                break;
                            case "G":
                                tEMPRes = "B";
                                break;

                            case "H":
                                tEMPRes = "C1C2";
                                break;
                            case "I":
                                tEMPRes = "C1C2";
                                break;
                            case "P":
                                tEMPRes = "B";
                                break;
                        }
                        SentImageNV(tEMPRes);
                    }

                    if (ToFupan != "")
                    {
                        string[] TempSt = ToFupan.Split(',');

                        ToFupan = "";

                        if (TempSt[0].StartsWith("TestResult") && TempSt.Length == 3)
                        {
                            string contion1 = string.Format("Time between '{0}' and  '{1}'", TempSt[1], TempSt[2]);
                            DataTable dt = TestRecordManager.GetDataTable(conn, string.Format("select * from {0} where {1}", "TestResult", contion1));
                            string json = JsonConvert.SerializeObject(dt);
                            server.dictConn[config_Temp[1]].Send("TestResult," + json + "\r\n");
                        }

                        if (TempSt[0].StartsWith("ChangeRec") && TempSt.Length == 3)
                        {
                            string contion1 = string.Format("Time between '{0}' and  '{1}'", TempSt[1], TempSt[2]);
                            DataTable dt = TestRecordManager.GetDataTable(conn, string.Format("select * from {0} where {1}", "ChangeRec", contion1));
                            string json = JsonConvert.SerializeObject(dt);
                            server.dictConn[config_Temp[1]].Send("ChangeRec," + json + "\r\n");
                        }

                        if (TempSt[0].StartsWith("DRAWING") && TempSt.Length == 3)
                        {

                            string Draw = "DRAWING,";
                            int Hours = DateTime.Now.Hour;

                            for (int i = 0; i < 24; i += 1)
                            {
                                int TaltoNum = 0;
                                int OKNum = 0;
                                string str3 = "";

                                int j = i + 8;

                                if (j >= 24)
                                {
                                    j = j - 24;
                                }



                                if (Hours >= 8 && Hours < 21)
                                {
                                    if (j >= 8 && j < 21)
                                    {
                                        str3 = DateTime.Now.ToString("yyyy/MM/dd") + " ";
                                    }
                                    else
                                    {
                                        TaltoNum = 0;
                                        OKNum = 0;
                                        Draw += TaltoNum.ToString() + "+" + OKNum.ToString() + ",";
                                        continue;
                                    }
                                }


                                if (Hours >= 21)
                                {
                                    if (j >= 21)
                                    {
                                        str3 = DateTime.Now.ToString("yyyy/MM/dd") + " ";
                                    }
                                    else
                                    {
                                        TaltoNum = 0;
                                        OKNum = 0;
                                        Draw += TaltoNum.ToString() + "+" + OKNum.ToString() + ",";
                                        continue;
                                    }
                                }


                                if (Hours < 8)
                                {
                                    if (j >= 8 && j < 21)
                                    {
                                        TaltoNum = 0;
                                        OKNum = 0;
                                        Draw += TaltoNum.ToString() + "+" + OKNum.ToString() + ",";
                                        continue;
                                    }
                                    else
                                    {
                                        if (j >= 21)
                                        {

                                            str3 = string.Format("{0}/{1}/{2}", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), (DateTime.Now.Day - 1).ToString().PadLeft(2, '0')) + " ";
                                        }
                                        else
                                        {
                                            str3 = DateTime.Now.ToString("yyyy/MM/dd") + " ";
                                        }
                                    }
                                }



                                string str = string.Format("{0}:00", j).PadLeft(5, '0') + ":00";
                                string str2 = string.Format("{0}:00", j + 1).PadLeft(5, '0') + ":00";
                                if (j == 23)
                                {
                                    str2 = "23:59:59";
                                }


                                string contion1 = string.Format("Time between '{0}' and  '{1}'", str3 + str, str3 + str2);
                                string contion2 = string.Format("Time between '{0}' and  '{1}' and AOI like '%OK%'", str3 + str, str3 + str2);

                                TaltoNum = recordManager.GetCount("TestResult", contion1);
                                OKNum = recordManager.GetCount("TestResult", contion2);
                                Draw += TaltoNum.ToString() + "+" + OKNum.ToString() + ",";
                            }
                            Draw = Draw.TrimEnd(',') + "\r\n";

                            server.dictConn[config_Temp[1]].Send(Draw);
                        }
                    }

                    if (count >= 1200)
                    {
                        count = 0;
                        string Draw = "DRAWING,";
                        int Hours = DateTime.Now.Hour;

                        for (int i = 0; i < 24; i += 1)
                        {
                            int TaltoNum = 0;
                            int OKNum = 0;
                            string str3 = "";

                            int j = i + 8;

                            if (j >= 24)
                            {
                                j = j - 24;
                            }



                            if (Hours >= 8 && Hours < 21)
                            {
                                if (j >= 8 && j < 21)
                                {
                                    str3 = DateTime.Now.ToString("yyyy/MM/dd") + " ";
                                }
                                else
                                {
                                    TaltoNum = 0;
                                    OKNum = 0;
                                    Draw += TaltoNum.ToString() + "+" + OKNum.ToString() + ",";
                                    continue;
                                }
                            }


                            if (Hours >= 21)
                            {
                                if (j >= 21)
                                {
                                    str3 = DateTime.Now.ToString("yyyy/MM/dd") + " ";
                                }
                                else
                                {
                                    TaltoNum = 0;
                                    OKNum = 0;
                                    Draw += TaltoNum.ToString() + "+" + OKNum.ToString() + ",";
                                    continue;
                                }
                            }


                            if (Hours < 8)
                            {
                                if (j >= 8 && j < 21)
                                {
                                    TaltoNum = 0;
                                    OKNum = 0;
                                    Draw += TaltoNum.ToString() + "+" + OKNum.ToString() + ",";
                                    continue;
                                }
                                else
                                {
                                    if (j >= 21)
                                    {
                                        str3 = string.Format("{0}/{1}/{2}", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), (DateTime.Now.Day - 1).ToString().PadLeft(2, '0')) + " ";
                                    }
                                    else
                                    {
                                        str3 = DateTime.Now.ToString("yyyy/MM/dd") + " ";
                                    }
                                }
                            }



                            string str = string.Format("{0}:00", j).PadLeft(5, '0') + ":00";
                            string str2 = string.Format("{0}:00", j + 1).PadLeft(5, '0') + ":00";

                            if (j == 23)
                            {
                                str2 = "23:59:59";
                            }



                            string contion1 = string.Format("Time between '{0}' and  '{1}'", str3 + str, str3 + str2);
                            string contion2 = string.Format("Time between '{0}' and  '{1}' and OCR like '%OK%'", str3 + str, str3 + str2);

                            TaltoNum = recordManager.GetCount("TestResult", contion1);
                            OKNum = recordManager.GetCount("TestResult", contion2);
                            Draw += TaltoNum.ToString() + "+" + OKNum.ToString() + ",";
                        }
                        Draw = Draw.TrimEnd(',') + "\r\n";

                        if (server.dictConn[config_Temp[1]] != null)
                            server.dictConn[config_Temp[1]].Send(Draw);
                    }
                }
                catch
                {

                }
                count++;
                Thread.Sleep(100);
            }
        }


        object og100 = new object();
        void SentImageNV(string pNAME)
        {
            Task.Factory.StartNew(() =>
            {
                lock (og100)
                {
                    try
                    {
                        System.Windows.Point position = new System.Windows.Point();

                        foreach (string i in imageGr.Keys)
                        {
                            if (i == pNAME)
                            {
                                if (cONDICTION1 == "缺陷")
                                {
                                    position.X = 0;
                                    position.Y = -1 * Math.Round(imageGr[i].MoveToYNG * (mainPanel.gridofimage.ActualHeight / ImageTempNG.Height), 0);
                                }
                                else
                                {
                                    position.X = 0;
                                    position.Y = -1 * Math.Round(imageGr[i].MoveToY * (mainPanel.gridofimage.ActualHeight / ImageTempALL.Height), 0);

                                }

                                break;
                            }
                        }

                        this.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, (ThreadStart)delegate ()
                         {
                             System.Drawing.Image Temp;

                             if (cONDICTION1 == "缺陷")
                             {
                                 Temp = ImageTempNG;
                             }
                             else
                             {
                                 Temp = ImageTempALL;
                             }
                             if (Temp != null)
                                 this.mainPanel.Source = BitmapToBitmapImage(Temp);

                             mainPanel.DoImageMove0(this.mainPanel.mainPanel, position);
                         });


                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.ToString());
                    }
                }
            });
        }
        public static List<string> ReadTxt(string path)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            string line;
            List<string> config = new List<string>();
            while ((line = sr.ReadLine()) != null)
            {
                config.Add(line);
            }
            System.Threading.Thread.Sleep(50);
            return config;
        }

        private Thread LogThread = null;
        private bool LogFlag = true;
        private Mutex Log_mutex = new Mutex();
        private List<string> LogMsgList = new List<string>();
        public static string logpath = @"SystemLog\";
        private static string logname = "Inspector1";
        public bool IsOnline = true;

        private List<string> CameraNames = new List<string>();

        private List<string> serverMsgs = new List<string>();
        private Thread ServerThread = null;
        private Mutex ServerMutex = new Mutex();
        private bool ServerFlag = true;


        private string SaveFolder1 = "";


        private Thread ScanThread = null;
        private bool ScanFlag = false;
        private Mutex Scan_mutex = new Mutex();

        public delegate void OnExitWorkspace();
        public delegate void OnEnterWorkspace(int workspaceIndex);

        TestRecordManager recordManager = new TestRecordManager(@"Data Source=D:\result1\db.db3");
        SQLiteConnection conn = new SQLiteConnection(@"Data Source=D:\result1\db.db3");
        public class ImageSourceModel
        {
            public ImageSource ImageSource { get; set; }
        }



        private void SmallImgShow_listBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            ItemsControl control = (ItemsControl)sender;
            ScrollViewer viewer = FindVisualChild<ScrollViewer>(control);
            if (viewer != null)
            {
                int delta = e.Delta;
                if (delta < 0)
                {
                    viewer.LineRight();
                }
                if (delta > 0)
                {
                    viewer.LineLeft();
                }
                viewer.ScrollToTop();
            }
        }


        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            if (obj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if ((child != null) && (child is T))
                    {
                        return (T)child;
                    }
                    T local = FindVisualChild<T>(child);
                    if (local != null)
                    {
                        return local;
                    }
                }
            }
            return default(T);
        }

        private void SmallImgShow_listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if ((Enumerable.Count<string>(this.saveCameraIndex) > 0) && (this.SmallImgShow_listBox.SelectedIndex != -1))
            //{
            //    try
            //    {
            //        this.mainPanel.Source = new BitmapImage(new Uri(this.saveCameraIndex[this.SmallImgShow_listBox.SelectedIndex]));
            //    }
            //    catch (Exception exception)
            //    {
            //        Console.WriteLine(exception.ToString());
            //    }
            //}
        }



        public void InitialCamCash()
        {
            Startl = true;
            dahualist_0.Clear();
        }

        //F1614651PS1PM06BJ-R728209166
        DataTable dt = new DataTable();
        private void Button_Search_Click(object sender, RoutedEventArgs e)
        {
         
            SNin.Text = "";
            RESA.Text = "";
            dt.Clear();
            ProductLi.Items.Clear();


            string contion1 = "";
            if (Timee.IsChecked == true)
            {
                contion1 = string.Format(" Time between '{0}' and  '{1}'",
                    this.TimeU.Value.ToString("yyyy/MM/dd HH:mm:ss"),
                    this.TimeD.Value.ToString("yyyy/MM/dd HH:mm:ss"));
            }

            string contion3 = "";
            if (Today.IsChecked == true)
            {
                string str = DateTime.Now.ToString("yyyy/MM/dd") + " 00:00:00";
                string str1 = DateTime.Now.ToString("yyyy/MM/dd") + " 23:59:59";

                contion3 = string.Format(" Time between '{0}' and  '{1}'",
                    str,
                    str1);
            }

            string contion4 = "";
            if (La15.IsChecked == true)
            {
                ProductLi.Items.Clear();

                string str = DateTime.Now.AddMinutes(-1 * 15).ToString("yyyy/MM/dd HH:mm:ss");// DateTime.Now.ToString("yyyy/MM/dd") + string.Format(" {0}:{1}:{2}", DateTime.Now.Hour, (DateTime.Now.Minute - 15), DateTime.Now.Second);
                string str1 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                contion4 = string.Format(" Time between '{0}' and  '{1}'",
                    str,
                    str1);
            }


            string contion2 = "";

            if (SNCode.IsChecked == true)
            {
                contion2 = string.Format(" OCR = '{0}'", PCODE.Text);
            }

            dt = recordManager.GetDataTable(string.Format("select * from {0} where {1}", "TestResult", contion1 + contion2 + contion3+ contion4));

            string temp = "";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                temp = "";

                temp = dt.Rows[i][2].ToString();

                if (temp != "")
                    ProductLi.Items.Add(temp);

                //for (int j = 0; j < dt.Columns.Count; j++)
                //{
                //    temp += dt.Rows[i][j].ToString()+ ",";
                //}

                //ProductLi.Items.Add(temp);
            }

            //if (contion2 != "" && temp != "" && dt.Rows.Count == 1)
            //{
            //    string[] tempArr = temp.TrimEnd(',').Split(',');

            //    if(tempArr.Length==5)                   
            //    {
            //        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            //        {
            //            SNin.Text = tempArr[1];
            //            RESA.Text = tempArr[4];
            //        });


            //        string DateD = (Convert.ToDateTime(tempArr[3])).ToString("yyyy-MM-dd");
            //        //CheckAndShow(tempArr[1], DateD, tempArr[2]);
            //        CheckAndShow(tempArr[1], DateD);
            //    }    
            //}

            //if (contion2 == "" && temp != "" && dt.Rows.Count > 0)
            //{

            //}
        }

        ConnectionClient CONCL = null;
        private static int DahuaCount = 1; //大华相机数量 
        public DahuaG[] dahua = new DahuaG[14] { null, null, null, null, null, null, null, null, null, null, null, null, null, null }; //大华类实例

        public delegate void ChildThreadExceptionHandler_Record(string message);
        public event ChildThreadExceptionHandler_Record ShowM;

        public delegate void UPdate_Record(int output, int ngCount);
        public event UPdate_Record UpRecord;

        //public Dictionary<string, ConnectionClient> dictConn = new Dictionary<string, ConnectionClient>();

        public void ListShowInfo(string args)
        {
            if (ShowM != null)
            {
                ShowM(args);
            }

        }



        public SocketComm SK = new SocketComm();
        Dictionary<string, List<double>> SizeFamily = new Dictionary<string, List<double>>();
        public bool Init0()
        {
            SizeFamily.Add("A1", new List<double>());
            SizeFamily["A1"].Add(2002);
            SizeFamily["A1"].Add(1259);

            SizeFamily.Add("A2", new List<double>());
            SizeFamily["A2"].Add(2002);
            SizeFamily["A2"].Add(1259);

            SizeFamily.Add("D1", new List<double>());
            SizeFamily["D1"].Add(1939);
            SizeFamily["D1"].Add(1252);

            SizeFamily.Add("D2", new List<double>());
            SizeFamily["D2"].Add(1939);
            SizeFamily["D2"].Add(1252);

            SizeFamily.Add("USB", new List<double>());
            SizeFamily["USB"].Add(1700);
            SizeFamily["USB"].Add(1450);

            SizeFamily.Add("USB1", new List<double>());
            SizeFamily["USB1"].Add(1700);
            SizeFamily["USB1"].Add(1450);

            SizeFamily.Add("B1_1", new List<double>());
            SizeFamily["B1_1"].Add(69);
            SizeFamily["B1_1"].Add(843);

            SizeFamily.Add("B1_2", new List<double>());
            SizeFamily["B1_2"].Add(69);
            SizeFamily["B1_2"].Add(843);

            SizeFamily.Add("B2", new List<double>());
            SizeFamily["B2"].Add(340);
            SizeFamily["B2"].Add(882);

            SizeFamily.Add("B3_1", new List<double>());
            SizeFamily["B3_1"].Add(69);
            SizeFamily["B3_1"].Add(843);

            SizeFamily.Add("B3_2", new List<double>());
            SizeFamily["B3_2"].Add(69);
            SizeFamily["B3_2"].Add(843);

            SizeFamily.Add("B4", new List<double>());
            SizeFamily["B4"].Add(340);
            SizeFamily["B4"].Add(882);

            SizeFamily.Add("B5", new List<double>());
            SizeFamily["B5"].Add(340);
            SizeFamily["B5"].Add(882);

            SizeFamily.Add("B6", new List<double>());
            SizeFamily["B6"].Add(340);
            SizeFamily["B6"].Add(882);

            SizeFamily.Add("C1", new List<double>());
            SizeFamily["C1"].Add(1463);
            SizeFamily["C1"].Add(870);

            SizeFamily.Add("C2", new List<double>());
            SizeFamily["C2"].Add(1487);
            SizeFamily["C2"].Add(528);

            SizeFamily.Add("C3", new List<double>());
            SizeFamily["C3"].Add(1463);
            SizeFamily["C3"].Add(870);

            SizeFamily.Add("C4", new List<double>());
            SizeFamily["C4"].Add(1487);
            SizeFamily["C4"].Add(528);

            SizeFamily.Add("C1_1", new List<double>());
            SizeFamily["C1_1"].Add(1330);
            SizeFamily["C1_1"].Add(1252);

            SizeFamily.Add("C2_1", new List<double>());
            SizeFamily["C2_1"].Add(1371);
            SizeFamily["C2_1"].Add(1205);

            SizeFamily.Add("C3_1", new List<double>());
            SizeFamily["C3_1"].Add(1330);
            SizeFamily["C3_1"].Add(1252);

            SizeFamily.Add("C4_1", new List<double>());
            SizeFamily["C4_1"].Add(1371);
            SizeFamily["C4_1"].Add(1205);

            SizeFamily.Add("C5_1", new List<double>());
            SizeFamily["C5_1"].Add(1318);
            SizeFamily["C5_1"].Add(769);

            SizeFamily.Add("C6_1", new List<double>());
            SizeFamily["C6_1"].Add(1313);
            SizeFamily["C6_1"].Add(1002);

            SizeFamily.Add("C7_1", new List<double>());
            SizeFamily["C7_1"].Add(1318);
            SizeFamily["C7_1"].Add(769);

            SizeFamily.Add("C8_1", new List<double>());
            SizeFamily["C8_1"].Add(1313);
            SizeFamily["C8_1"].Add(1002);

            SizeFamily.Add("CR1_1", new List<double>());
            SizeFamily["CR1_1"].Add(1480);
            SizeFamily["CR1_1"].Add(296);

            SizeFamily.Add("CR1_2", new List<double>());
            SizeFamily["CR1_2"].Add(1480);
            SizeFamily["CR1_2"].Add(296);

            SizeFamily.Add("CR1_3", new List<double>());
            SizeFamily["CR1_3"].Add(1480);
            SizeFamily["CR1_3"].Add(296);

            SizeFamily.Add("CR2_1", new List<double>());
            SizeFamily["CR2_1"].Add(1480);
            SizeFamily["CR2_1"].Add(296);

            SizeFamily.Add("CR2_2", new List<double>());
            SizeFamily["CR2_2"].Add(1480);
            SizeFamily["CR2_2"].Add(296);

            SizeFamily.Add("CR2_3", new List<double>());
            SizeFamily["CR2_3"].Add(1480);
            SizeFamily["CR2_3"].Add(296);

            SizeFamily.Add("CR3_1", new List<double>());
            SizeFamily["CR3_1"].Add(1480);
            SizeFamily["CR3_1"].Add(296);

            SizeFamily.Add("CR3_2", new List<double>());
            SizeFamily["CR3_2"].Add(1480);
            SizeFamily["CR3_2"].Add(296);

            SizeFamily.Add("CR3_3", new List<double>());
            SizeFamily["CR3_3"].Add(1480);
            SizeFamily["CR3_3"].Add(296);

            SizeFamily.Add("CR4_1", new List<double>());
            SizeFamily["CR4_1"].Add(1480);
            SizeFamily["CR4_1"].Add(296);

            SizeFamily.Add("CR4_2", new List<double>());
            SizeFamily["CR4_2"].Add(1480);
            SizeFamily["CR4_2"].Add(296);

            SizeFamily.Add("CR4_3", new List<double>());
            SizeFamily["CR4_3"].Add(1480);
            SizeFamily["CR4_3"].Add(296);

            smartThreadPool_0.MaxThreads = 40;

            int status = 0;
            int U = 0;

            mUpdateAlarmThread = new Thread(ProcessInfoThread);
            mUpdateAlarmThread.Start();


  
            ServerThread = new Thread(ProcessLogThread);
            ServerThread.Start();

            dahuaThread[0] = new Thread(DahuaListThread_0); dahuaThread[0].Name = "Dh0"; dahuaThread[0].IsBackground = true; dahuaThread[0].Start();//不管是否需要用到相机，都需要打开线程，可以从相机端

            return (status == 0);
        }

        public void ProcessLogThread()
        {
            AutoResetEvent tmr = new AutoResetEvent(false);
            while (true)
            {
                tmr.WaitOne(5000);
                if (LogFlag)
                {
                    Log_mutex.WaitOne();
                    string[] s = new string[LogMsgList.Count];
                    LogMsgList.CopyTo(s);
                    LogMsgList.Clear();
                    Log_mutex.ReleaseMutex();

                    List<string> QUEUEli = new List<string>();
                    List<string> MOTIONli = new List<string>();
                    List<string> ALGli = new List<string>();

                    foreach (string Msg in s)
                    {

                        if (Msg.StartsWith("QUEUE"))
                            QUEUEli.Add(Msg);

                        if (Msg.StartsWith("MOTION"))
                            MOTIONli.Add(Msg);

                        if (Msg.StartsWith("ALG"))
                            ALGli.Add(Msg);
                    }

                    if (QUEUEli.Count > 0)
                        Log(QUEUEli.ToArray(), logpath, "QUEUE");

                    if (MOTIONli.Count > 0)
                        Log(MOTIONli.ToArray(), logpath, "MOTION");

                    if (ALGli.Count > 0)
                        Log(ALGli.ToArray(), logpath, "ALG");

                }
            }
        }

        object LOAnswer= new object();
        AutoResetEvent Answertmr = new AutoResetEvent(false);
        int AnswerCount = 0;
        int AnOCRCount = 0;
        public void AnswerTatol(string Msg)
        {
            try
            {
                Log_mutex.WaitOne();
                LogMsgList.Add("MOTION收到产品结果查询信号： " + Msg + "     " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                Log_mutex.ReleaseMutex();


                string[] St_temp = Msg.TrimEnd(';').Split(',');

                string strMsg_Temp = "";

                //string OCRMsg_Temp = "";

                //string OCRINDB = "";

                string ISGood = "NG";

                if (St_temp.Length >= 3 && !string.IsNullOrEmpty(St_temp[1]))
                {
                    strMsg_Temp = St_temp[1];
                    //OCRMsg_Temp = St_temp[2];

                    int numOK = 0;
                    int numNG = 0;

                    //string addStr = recordManager.Get_ALLOCR_Record("TestResult", OCRMsg_Temp, 0);

                    //string[] aryLine = addStr.TrimEnd(',').Split(',');

                    //if(aryLine!=null && aryLine.Length>10)
                    //{
                    //    //OCRINDB = aryLine[2];

                    //    if (aryLine[1] != strMsg_Temp)
                    //    {
                    //        strMsg_Temp = aryLine[1];
                    //    }

                    //}

                    //if (Tools.levenshtein(OCRINDB, OCRMsg_Temp) < 0.8)
                    //{
                    //    AnOCRCount++;
                    //}


                    //string contion1 = string.Format(" SN = '{0}'", strMsg_Temp);

                    //string TaltoNum = recordManager.GetItemValue("TestResult", "Time", contion1);

                    //string DateD = (Convert.ToDateTime(TaltoNum)).ToString("yyyy-MM-dd");

                    string DateD = (DateTime.Now).ToString("yyyy-MM-dd");

                    string ImagePath1 = SaveFolder1 + @"\" + DateD + @"\" + strMsg_Temp + @"\Json";

                    DateTime dt0 = DateTime.Now;

                    int NUM = 0;

                    string OCR = "";

                    do
                    {
                        numOK = 0;
                        numNG = 0;

                        foreach (string f0 in Directory.EnumerateFiles(ImagePath1, "*.json", SearchOption.TopDirectoryOnly))
                        {
                            numOK++;

                            if (f0.Contains("000") && string.IsNullOrEmpty(OCR))
                            {
                                string[] RealN = f0.Split('\\');

                                string kk = RealN[RealN.Length - 1];

                                OCR = kk.TrimStart("000".ToCharArray()).TrimEnd(".json".ToCharArray());
                            }

                            if (f0.Contains("缺陷"))
                            {
                                numNG++;

                                break;
                            }
                        }

                        if (NUM > 0)
                            Answertmr.WaitOne(50);


                        NUM++;
                    }
                    while (numNG <= 0 && numOK < 39  && (DateTime.Now - dt0).TotalMilliseconds < 4000);

                    Log_mutex.WaitOne();
                    LogMsgList.Add("MOTION  "+(DateTime.Now - dt0).TotalMilliseconds + "   产品结果查询结束： " + strMsg_Temp + "     " + OCR + "    numNG:" + numNG + "   numOK:" + numOK + "    " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                    Log_mutex.ReleaseMutex();

                    if (numNG <= 0 && numOK >= 39 && !string.IsNullOrEmpty(OCR))
                    {
                        ISGood = "OK";
                    }

                    Log_mutex.WaitOne();
                    LogMsgList.Add("MOTION结束MES： " + strMsg_Temp + "     " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                    Log_mutex.ReleaseMutex();

                    if (AnswerCount > 15)
                    {
                        ISGood = "JUMP_MESBREAK_NG";

                        this.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, (ThreadStart)delegate ()
                        {
                            System.Windows.Forms.MessageBox.Show("MES 异常！");
                        });
                    }

                    string H17_TT_SIDE = string.Format("[JUDGE][{0}][{1};{2}]T", strMsg_Temp, OCR,ISGood.ToLower().Contains("ok") ? "OK" : "NG");

                    string H17_TT_SIDE2 = "H" + H17_TT_SIDE.Length + H17_TT_SIDE;

                    SK.SendData000(H17_TT_SIDE2);

                    Log_mutex.WaitOne();
                    LogMsgList.Add("MOTION产品结果告知下位机结束： " + strMsg_Temp + "     " + ISGood + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                    Log_mutex.ReleaseMutex();

                    if(cONDICTION_MES)
                    {
                        ISGood = "JUMP_OK";
                    }

                    if (!ISGood.Contains("JUMP") && !ISGood.Contains("NG"))
                    {
                        lock (LOAnswer)
                        {
                            ISGood = "NG";


                            bool MesBool = false;

                            string TimeStamp = "";

                            bool SnBool = false;

                            MentorAPI.cSelcompAPI MES = new MentorAPI.cSelcompAPI();

                            try
                            {

                                SnBool = MES.CheckRouteForSerialNumber(OCR);

                                if (!SnBool)
                                {
                                    AnswerCount += 1;

                                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                                    {
                                        MesRes.AppendText(OCR + "   " + MES.Message + "\r\n");
                                        MesRes.ScrollToEnd();
                                    });
                                }

                            }
                            catch
                            {
                                AnswerCount += 1;

                                this.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, (ThreadStart)delegate ()
                                {
                                    System.Windows.Forms.MessageBox.Show("MES 错误！");
                                });
                            }


                            TimeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");


                            if (SnBool)
                            {
                                MesFamily Mf = new MesFamily();

                                try
                                {

                                    MesBool = MES.SaveResult(Mf.CreateWriteXmlStr("PASS", OCR, TimeStamp, TestFixtureNumber, TestHeadNumber));

                                    if (!MesBool)
                                    {

                                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                                        {
                                            MesRes.AppendText(OCR + "   " + MES.Message + "\r\n");
                                            MesRes.ScrollToEnd();
                                            
                                        });                                      
                                    }
                               }
                                catch
                                {
                                    this.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, (ThreadStart)delegate ()
                                    {
                                        System.Windows.Forms.MessageBox.Show("MES 异常！");
                                    });
                                }

                                if (MesBool)
                                {
                                    AnswerCount = 0;
                                    ISGood = "OK";
                                    ListShowInfo(OCR + "上传MES成功\r\n");
                                }
                                else
                                {

                                    ListShowInfo(OCR + "上传MES失败\r\n");
                                }
                            }
                        }
                    }


                    List<string> ITEMs = new List<string>();
                    List<string> VALUEs = new List<string>();
                    ITEMs.Add("Time");
                    ITEMs.Add("MES");
                    ITEMs.Add("OCR");

                    VALUEs.Add(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    VALUEs.Add(ISGood);
                    VALUEs.Add(OCR);
                    recordManager.InsertRecordBlockData("TestResult", strMsg_Temp, ITEMs, VALUEs);



                    //string[] ArrayList = TopGun.Split('#')[1].TrimEnd("\r\n".ToCharArray()).Split(',');

                    //for (int btcon = 0; btcon < ArrayList.Length; btcon++)
                    //{
                    //    string[] tEMsT = ArrayList[btcon].Split(':');

                    //    if (tEMsT.Length == 2)
                    //    {
                    //        Log("收到注册信号： " + strMsg_Temp + "   " + tEMsT[0] + "  " + tEMsT[1] + "     " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), logpath, "JUDGEdab");

                    //        recordManager.InsertRecord820v4("TestResult", strMsg_Temp, tEMsT[0], tEMsT[1]);

                    //        Log("注册完成： " + strMsg_Temp + "   " + tEMsT[0] + "  " + tEMsT[1] + "     " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), logpath, "JUDGEdab");

                    //    }
                    //}

                    SelfFuncAIM_XFMR(strMsg_Temp);

                    //Log("产品记录MES结果到数据库： " + strMsg_Temp + "   "+OCR+"  " + ISGood + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), logpath, "JUDGEdab");
                }



            }
            catch
            {

            }



            //});
        }

        object LOMES= new object();

        object exCELlO = new object();
        public void SelfFuncAIM_XFMR(string ProductSN)
        {
            lock (exCELlO)
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "EXcelLog" + "\\";
                string name = DateTime.Now.ToString("yyyy-MM-dd") + "_AOI.csv";
                string fileName = path + name;

                try
                {
                    string addStr = recordManager.Get_ALL_Record("TestResult", ProductSN, 0);

                    string[] aryLine = addStr.TrimEnd(',').Split(',');

                    //DataRow dr = dt1.NewRow();

                    //for (int j = 0; j < aryLine.Length; j++)
                    //{
                    //    dr[j] = aryLine[j];
                    //}

                    //for (int i = 0; i < dt1.Rows.Count; i++)
                    //{
                    //    if (dt1.Rows[i][0].ToString() == dr[0].ToString())
                    //    {
                    //        for (int j = 15; j < dt1.Columns.Count; j++)
                    //        {
                    //            dt1.Rows[i][j] = dr[j];
                    //        }
                    //        break;
                    //    }

                    //    if (i == (dt1.Rows.Count - 1))
                    //    {
                    //        dt1.Rows.Add(dr);
                    //    }
                    //}


                    if (!File.Exists(fileName))
                    {

                        string hdStr = "ID,SN,OCR,Time,MES,A,USB_0,USB_1,C1C2_0,C1C2_1,C1C2_2,C1C2_3,C1C2_4,C1C2_5,C1C2_6,C1C2_7,B_0,B_1,B_2,B_3,B_4,B_5,B_6,B_7,C3_0,C3_1,C3_2,C3_3,C3_4,C3_5,C3_6,C3_7,C3_8,C3_9,C3_10,C3_11,C4_0,C4_1,C4_2,C4_3,D,REF";

                        driver.cam.Tools.WriteCSVLine(fileName, hdStr);
                    }


                    if (!string.IsNullOrEmpty(addStr))
                    {

                        driver.cam.Tools.WriteCSVLine(fileName, addStr);
                    }


                }

                catch
                {
                    //System.Windows.MessageBox.Show("导出\"" + fileName + "\"失败");
                }

            }
        }



        object ooooj = new object();

        private Mutex DictonnaryMutex = new Mutex();
        public static List<string> ChangUnite = new List<string>();
        public void ServerCallBack0(string Msg)
        {
            Thread t = new Thread(() =>
            {
                try
                {
                    if (Msg.StartsWith("CBA"))
                    {
                        string SN_Fl = "";

                        string[] ArrayList = Msg.TrimEnd("\r\n".ToCharArray()).Split('$');

                        if (ArrayList.Length > 1)
                        {
                            for (int i = 1; i < ArrayList.Length; i++)
                            {

                                string[] TempGun = ArrayList[i].Split('#');

                                SN_Fl += TempGun[0] + ",";

                                //if (TempGun.Length > 1)
                                //{
                                //    DictonnaryMutex.WaitOne();
                                //    LIST_info(ref ChangUnite, TempGun[0], TempGun[1].TrimEnd(',') + ",");
                                //    DictonnaryMutex.ReleaseMutex();
                                //}

                                if (TempGun.Length > 1)
                                {

                                    Task.Factory.StartNew(() =>
                                    {
                                        DictonnaryMutex.WaitOne();
                                        ADDLIST_info(ref ChangUnite, TempGun[0]);

                                        LIST_info(ref ChangUnite, TempGun[0], TempGun[1].TrimEnd(',') + ",");
                                        DictonnaryMutex.ReleaseMutex();


                                        string[] ItList = TempGun[1].TrimEnd(',').Split(',');
                                        List<string> ITEMs = new List<string>();
                                        List<string> VALUEs = new List<string>();

                                        for (int btcon = 0; btcon < ItList.Length; btcon++)
                                        {
                                            string[] tEMsT = ItList[btcon].Split(':');

                                            if (tEMsT.Length == 2)
                                            {
                                                ITEMs.Add(tEMsT[0]);
                                                VALUEs.Add(tEMsT[1]);
                                            }
                                        }
                                        recordManager.InsertRecordBlockData("TestResult", TempGun[0], ITEMs, VALUEs);

                                    });
                                }



                            }

                            if (ArrayList[0] == "CBA31")
                            {


                                if (server.dictConn["192.168.1.31"] != null)
                                {
                                    server.dictConn["192.168.1.31"].SendData000("ASK" + "," + SN_Fl.TrimEnd(',') + "," + "END" + "\r\n");
                                    //Log("192.168.1.31网络发送结束：   " + "  ASK  " + SN_Fl + "    END   " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), logpath, "JUDGETCP");
                                }
                            }


                            if (ArrayList[0] == "CBA21")
                            {
                                if (server.dictConn["192.168.1.21"] != null)
                                {
                                    server.dictConn["192.168.1.21"].SendData000("ASK" + "," + SN_Fl.TrimEnd(',') + "," + "END" + "\r\n");
                                    //Log("192.168.1.21网络发送结束：   " + "  ASK  " + SN_Fl + "    END   " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), logpath, "JUDGETCP");
                                }
                            }

                            if (ArrayList[0] == "CBA11")
                            {
                                if (server.dictConn["192.168.1.11"] != null)
                                {
                                    server.dictConn["192.168.1.11"].SendData000("ASK" + "," + SN_Fl.TrimEnd(',') + "," + "END" + "\r\n");
                                    //Log("192.168.1.11网络发送结束：   " + "  ASK  " + SN_Fl + "    END   " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), logpath, "JUDGETCP");
                                }
                            }
                        }
                        else
                        {
                            return;
                        }
                    }

                    if (Msg.StartsWith("REG"))
                    {
                        string[] ArrayList = Msg.TrimEnd("\r\n".ToCharArray()).TrimEnd(',').Split(',');
                        if (ArrayList.Length >= 4)
                        {
                            DictonnaryMutex.WaitOne();

                            ADDLIST_info(ref ChangUnite, ArrayList[1]);


                            if (ChangUnite.Count >80)
                            {
                                ChangUnite.RemoveAt(0);
                            }

                            DictonnaryMutex.ReleaseMutex();


                            //Log("收到注册信号： " + ArrayList[1] + "   " + ArrayList[2] + "  " + ArrayList[3] + "     " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), logpath, "SUANFA");

                            recordManager.InsertRecord820v4("TestResult", ArrayList[1], ArrayList[2], ArrayList[3]);



                            //Log("注册完成： " + ArrayList[1] + "   " + ArrayList[2] + "  " + ArrayList[3] + "     " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), logpath, "SUANFA");
                        }

                    }

                }
                catch
                {

                }
            });
            t.Start();



            Thread.Sleep(30);
        }

        public void ServerCallBack1(string Msg)
        {

            if (Msg.ToLower().StartsWith("ok"))
            {
                Product_PN_Auto = Msg.TrimEnd("\r\n".ToCharArray()).Split(';')[1];

                ActiveQueueEvent.Set();//打开开关

                Product_PN = Msg.TrimEnd("\r\n".ToCharArray()).Split(';')[1];
                SNRead = Msg.TrimEnd("\r\n".ToCharArray()).Split(';')[1];
            }
        }


        public void ServerCallBack2(string Msg)
        {

            Thread t = new Thread(() =>
               {
                   if (Msg.Contains("JUDGE"))
                   {
                       AnswerTatol(Msg);
                   }

                   if (Msg.Contains("FP_OK"))
                   {
                       SentMes(SNRead);
                   }
               });
            t.Start();

            Thread.Sleep(30);
        }


        Dictionary<string, ImageInfo1>  imageGr=new Dictionary<string, ImageInfo1>();
        NetworkInterface Nextfix = new NetworkInterface();
        string Product_PN = "";
        string Product_PN_Auto = "";
        ManualResetEvent ActiveQueueEvent = new ManualResetEvent(false);
        public void DahuaListThread_0()
        {
            InputInfo info = new InputInfo();
            info._SNDir = AppDomain.CurrentDomain.BaseDirectory;
            while (true)
            {
                try
                {
                    ActiveQueueEvent.WaitOne();//等待
                    string SN007 = Product_PN_Auto;
                    Product_PN_Auto = "";

                    imageGr = null;
                    ImageTempALL = null;
                    ImageTempNG = null;

                    imageGr_Path1.Clear();
                    GC.Collect();

                    imageGr = new Dictionary<string, ImageInfo1>();

                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        MesRes.Document.Blocks.Clear();
                        SNRead = "";
                        SNin.Text = "";
                        RESA.Text = "";
                        Button_NG1.Background = new SolidColorBrush(Colors.Red);
                        Button_OK1.Background = new SolidColorBrush(Colors.Gray);

                        this.mainPanel.Source = null;

                        ProductLi.Items.Add(SN007);
                        SNRead = SN007;
                        SNin.Text = SN007;
                        RESA.Foreground = new SolidColorBrush(Colors.Red);
                        RESA.Text = "NG";
                    });

                    string addStr = recordManager.Get_ALLOCR_Record("TestResult", SN007, 0);

                    string[] aryLine = addStr.TrimEnd(',').Split(',');

                    if(aryLine==null || aryLine.Length<2)
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            MesRes.AppendText("没有搜索到OCR号------请联系工程师或重新投递\r\n");
                            MesRes.ScrollToEnd();
                        });

                        for (int wo = 0; wo < 38; wo++)
                        {
                            Navigator.mViewModel.DeStatus[wo] = false;

                        }

                        Navigator.mViewModel.NotifyDeStatus();

                        continue;
                    }


                    string SN = aryLine[1];
                    if (SN == "")
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            MesRes.AppendText("没有搜索到OCR号------请联系工程师或重新投递\r\n");
                            MesRes.ScrollToEnd();
                        });

                        for (int wo = 0; wo < 38; wo++)
                        {
                            Navigator.mViewModel.DeStatus[wo] = false;

                        }

                        Navigator.mViewModel.NotifyDeStatus();

                        continue;
                    }
                    else
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            MesRes.AppendText(SN + "\r\n");
                            MesRes.ScrollToEnd();
                        });
                    }

                    string MES = "";
                    string TaltoNum = "";

                    if (aryLine == null || aryLine.Length >5)
                    {
                        MES = aryLine[4];
                        if (MES.ToLower().Contains("true"))
                        {
                            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                            {
                                MesRes.AppendText("该产品已经OK------请放行\r\n");
                                MesRes.ScrollToEnd();
                            });

                            for (int wo = 0; wo < 38; wo++)
                            {
                                Navigator.mViewModel.DeStatus[wo] = false;

                            }

                            Navigator.mViewModel.NotifyDeStatus();


                            continue;
                        }

                        TaltoNum = aryLine[3];

                     
                    }

                    if (TaltoNum == "")
                    {
                        TaltoNum = DateTime.Now.ToString("yyyy-MM-dd");
                    }

                    string DateD = (Convert.ToDateTime(TaltoNum)).ToString("yyyy-MM-dd");

                    CheckAndShow(SN, DateD);

                    CheckAndShow_WHOLE(SN, DateD);


                }
                catch
                {

                }
                finally
                {
                    ActiveQueueEvent.Reset();
                }
            }
        }

        string cONDICTION1 = "缺陷";

        object og = new object();
        Dictionary<string, ImageInfo1> imageGr_Path1 = new Dictionary<string, ImageInfo1>();
        //Dictionary<string, ImageInfo1> imageGr_Path2 = new Dictionary<string, ImageInfo1>();
        public static int BSides_count=0;
         
        SmartThreadPool smartThreadPool_0 = new SmartThreadPool();

        double WidthFactor = 1.3;
        double HeightFactor = 1.3;

        public void CheckAndShow(string SN, string Time)
        {
            lock (og)
            {

                try
                {
                    AutoResetEvent tmr = new AutoResetEvent(false);

                    for (int i = 0; i < 38; i++)
                    {
                        Navigator.mViewModel.DeStatus[i] = false;

                    }

                    Navigator.mViewModel.NotifyDeStatus();

                    int rightOK_count = 0;
                    int rightNG_count = 0;
                    BSides_count = 0;
                    List<System.Drawing.Image> imageList_Path1 = new List<System.Drawing.Image>();
                    List<System.Drawing.Image> imageList_Path2 = new List<System.Drawing.Image>();
                    ComPic CP_Path1 = new ComPic();

                    imageGr_Path1.Clear();
                    GC.Collect();


                    //string ImagePath1 = SaveFolder1 + @"\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\" + SN;// FindDirec0(SaveFolder1, Time, SN + "-" + PN);

                    string ImagePath1 = SaveFolder1 + @"\" + Time + @"\" + SN + @"\Save";

                    //string ImagePath1 = AppDomain.CurrentDomain.BaseDirectory + "1338250976" + @"\Save";

                    if (System.IO.Directory.Exists(ImagePath1) == false)//如果不存在就创建file文件夹
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            MesRes.AppendText("没有该产品图片------请检查网络连接状态或联系工程师\r\n");
                            MesRes.ScrollToEnd();
                        });
                        return;

                    }


                    //OCR_DaoChu = recordManager.GetItemValue("TestResult", "OCR", string.Format(" SN = '{0}'", SN));


                    Mutex FinishFlagMutex = new Mutex();
                    Mutex ImagPath1Mutex = new Mutex();
                    List<bool> FinishFlag = new List<bool>();
                    DateTime dt0kk = DateTime.Now;

                    foreach (string f0 in Directory.EnumerateFiles(ImagePath1, "*.jpg", SearchOption.TopDirectoryOnly))
                    {

                        string[] RealN = f0.Split('\\');

                        if (RealN == null || RealN.Length <= 0) continue;

                        string kk = RealN[RealN.Length - 1];



                        if (f0.Contains("缺陷"))
                        {
                            rightNG_count++;

                            FinishFlagMutex.WaitOne();
                            FinishFlag.Add(false);
                            FinishFlagMutex.ReleaseMutex();

                            smartThreadPool_0.QueueWorkItem(() =>
                            {

                                try
                                {
                                    string f_ORG = f0;
                                    string f = kk.TrimEnd("_缺陷.jpg".ToCharArray()).TrimEnd("_OK.jpg".ToCharArray());

                                    string Kind = "";
                                    int StationN = -1;

                                    if (f == "A1")
                                    {
                                        StationN = 0;
                                        Kind = "A";
                                    }

                                    if (f == "A2")
                                    {
                                        StationN = 1;
                                        Kind = "A";
                                    }

                                    if (f == "USB" || f == "USB1")
                                    {
                                        StationN = 2;
                                        Kind = "A";
                                    }

                                    if (f == "D1" || f == "D2")
                                    {
                                        StationN = 3;
                                        Kind = "A";
                                    }

                                    if (f == "C1_1" || f == "C2_1" || f == "C3_1" || f == "C4_1")
                                    {
                                        StationN = 4;
                                        Kind = "C1C2";
                                    }

                                    if (f == "C5_1" || f == "C6_1" || f == "C7_1" || f == "C8_1")
                                    {
                                        StationN = 5;
                                        Kind = "C1C2";
                                    }


                                    if (f == "B1_1" || f == "B1_2" || f == "B3_1" || f == "B3_2" || f == "B2" || f == "B4" || f == "B5" || f == "B6")
                                    {
                                        StationN = 6;
                                        Kind = "B";
                                    }

                                    if (f == "CR1_1" || f == "CR1_2" || f == "CR1_3")
                                    {
                                        StationN = 7;
                                        Kind = "CR1";
                                    }

                                    if (f == "CR2_1" || f == "CR2_2" || f == "CR2_3")
                                    {
                                        StationN = 8;
                                        Kind = "CR1";
                                    }

                                    if (f == "CR3_1" || f == "CR3_2" || f == "CR3_3")
                                    {
                                        StationN = 9;
                                        Kind = "CR3";
                                    }

                                    if (f == "CR4_1" || f == "CR4_2" || f == "CR4_3")
                                    {
                                        StationN = 10;
                                        Kind = "CR3";
                                    }

                                    if (f == "C1" || f == "C3")
                                    {
                                        StationN = 11;
                                        //Kind = "C4";
                                        Kind = "B";
                                        BSides_count++;
                                    }

                                    if (f == "C2" || f == "C4")
                                    {
                                        StationN = 12;
                                        //Kind = "C4";
                                        Kind = "B";
                                        BSides_count++;
                                    }

                                    System.Drawing.Image ImageTemp = System.Drawing.Bitmap.FromFile(f_ORG);

                                    int ISfirst = 0;
                                    ImagPath1Mutex.WaitOne();
                                    if (!imageGr_Path1.ContainsKey(Kind))
                                    {
                                        imageGr_Path1.Add(Kind, new ImageInfo1());
                                        ISfirst++;
                                    }
                                    ImagPath1Mutex.ReleaseMutex();

                                    if ((ISfirst != 0 && !kk.StartsWith("B")) || kk.StartsWith("B"))
                                    {
                                        ImageTemp = mainPanel.DrawStr(ImageTemp, f);
                                    }


                                    Navigator.mViewModel.DeStatus[StationN] = true;

                                    ImagPath1Mutex.WaitOne();
                                    imageGr_Path1[Kind].ListGroupAll.Add(ImageTemp);
                                    imageGr_Path1[Kind].WidthCollect.Add(ImageTemp.Width);
                                    imageGr_Path1[Kind].HeightCollect.Add(ImageTemp.Height);
                                    ImagPath1Mutex.ReleaseMutex();

                                }
                                catch
                                {

                                }
                                finally
                                {

                                }

                                FinishFlagMutex.WaitOne();
                                if (FinishFlag.Count > 0)
                                    FinishFlag.RemoveAt(0);
                                FinishFlagMutex.ReleaseMutex();

                            });


                        }

                        rightOK_count++;

                        tmr.WaitOne(5);

                    }


                    dt0kk = DateTime.Now;

                    while (FinishFlag.Count > 0 && (DateTime.Now - dt0kk).TotalMilliseconds < 5000)//tiger
                    {
                        tmr.WaitOne(10);
                    }



                    if (FinishFlag.Count > 0)
                    {
                        FinishFlagMutex.WaitOne();
                        FinishFlag.Clear();
                        FinishFlagMutex.ReleaseMutex();

                        smartThreadPool_0.Cancel(true);

                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            MesRes.AppendText("取图异常------请重新放置产品\r\n");
                            MesRes.ScrollToEnd();
                        });

                        return;
                    }


                    if (rightOK_count < 38)
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            MesRes.AppendText("因为少图------请重新投递\r\n");
                            MesRes.ScrollToEnd();
                        });

                        return;
                    }

                    if (rightNG_count <= 0)
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            MesRes.AppendText("本产品为OK产品------请直接上传MES为PASS！！！\r\n");
                            MesRes.ScrollToEnd();
                        });
                    }


                    object OBBJ = new object();
                    FinishFlag.Clear();

                    foreach (string i in imageGr_Path1.Keys)
                    {
                        FinishFlagMutex.WaitOne();
                        FinishFlag.Add(false);
                        FinishFlagMutex.ReleaseMutex();

                        smartThreadPool_0.QueueWorkItem(() =>
                        {
                            try
                            {

                                ComPic CP_Path1Temp = new ComPic();
                                double height = imageGr_Path1[i].HeightCollect.Max();
                                double width = imageGr_Path1[i].WidthCollect.Sum();

                                if (imageGr_Path1[i].ListGroupAll.Count > 0)
                                {
                                    System.Drawing.Image Iilist = CP_Path1Temp.JoinImageR_BACK(imageGr_Path1[i].ListGroupAll, (int)(width * WidthFactor), (int)(height * HeightFactor));

                                    lock (OBBJ)
                                    {
                                        imageList_Path1.Add(Iilist);

                                        if (!imageGr.ContainsKey(i))
                                            imageGr.Add(i, new ImageInfo1());
                                    }

                                    imageGr[i].MoveToXNG = imageGr_Path1[i].MoveToXNG;
                                    imageGr[i].MoveToYNG = imageGr_Path1[i].MoveToYNG;
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            finally
                            {

                            }


                            FinishFlagMutex.WaitOne();
                            if (FinishFlag.Count > 0)
                                FinishFlag.RemoveAt(0);
                            FinishFlagMutex.ReleaseMutex();

                        });
                    }

                    dt0kk = DateTime.Now;

                    while (FinishFlag.Count > 0 && (DateTime.Now - dt0kk).TotalMilliseconds < 5000)//tiger
                    {
                        tmr.WaitOne(10);
                    }


                    if (FinishFlag.Count > 0)
                    {
                        FinishFlagMutex.WaitOne();
                        FinishFlag.Clear();
                        FinishFlagMutex.ReleaseMutex();

                        smartThreadPool_0.Cancel(true);

                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            MesRes.AppendText("画图异常------请重新放置产品\r\n");
                            MesRes.ScrollToEnd();
                        });

                        return;
                    }


                    CP_Path1.CAL_NGpic_MoveTo(imageGr_Path1, 200);



                    ImageTempNG = CP_Path1.JoinImageC_BUILDING(imageList_Path1, (int)(CP_Path1.NGmaxX * 2), (int)(CP_Path1.NGmaxY * 2));


                    Navigator.mViewModel.NotifyDeStatus();

                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        System.Drawing.Image Temp = null;

                        if (cONDICTION1 == "缺陷")
                        {
                            Temp = ImageTempNG;
                        }


                        if (Temp != null)
                        {
                            this.mainPanel.Source = null;
                            GC.Collect();

                            this.mainPanel.Source = BitmapToBitmapImage(Temp);
                        }

                    });

                }
                catch
                {

                }
                finally
                {

                }
            }
        }

        //public void CheckAndShow(string SN, string Time)
        //{
        //    lock (og)
        //    {

        //        try
        //        {
        //            AutoResetEvent tmr = new AutoResetEvent(false);

        //            for (int i = 0; i < 38; i++)
        //            {
        //                Navigator.mViewModel.DeStatus[i] = false;

        //            }

        //            Navigator.mViewModel.NotifyDeStatus();

        //            int rightOK_count = 0;
        //            int rightNG_count = 0;
        //            BSides_count = 0;
        //            List<System.Drawing.Image> imageList_Path1 = new List<System.Drawing.Image>();
        //            List<System.Drawing.Image> imageList_Path2 = new List<System.Drawing.Image>();
        //            ComPic CP_Path1 = new ComPic();

        //            imageGr_Path1.Clear();
        //            GC.Collect();


        //            //string ImagePath1 = SaveFolder1 + @"\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\" + SN;// FindDirec0(SaveFolder1, Time, SN + "-" + PN);

        //            string ImagePath1 = SaveFolder1 + @"\" + Time + @"\" + SN + @"\Save";

        //            //string ImagePath1 = AppDomain.CurrentDomain.BaseDirectory + "1338250976" + @"\Save";

        //            if (System.IO.Directory.Exists(ImagePath1) == false)//如果不存在就创建file文件夹
        //            {
        //                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
        //                {
        //                    MesRes.AppendText("没有该产品图片------请检查网络连接状态或联系工程师\r\n");
        //                    MesRes.ScrollToEnd();
        //                });
        //                return;

        //            }


        //            //OCR_DaoChu = recordManager.GetItemValue("TestResult", "OCR", string.Format(" SN = '{0}'", SN));


        //            Mutex FinishFlagMutex = new Mutex();
        //            Mutex ImagPath1Mutex = new Mutex();
        //            List<bool> FinishFlag = new List<bool>();

        //            DateTime dt0kk = DateTime.Now;

        //            foreach (string f0 in Directory.EnumerateFiles(ImagePath1, "*.jpg", SearchOption.TopDirectoryOnly))
        //            {

        //                string[] RealN = f0.Split('\\');

        //                if (RealN == null || RealN.Length <= 0) continue;

        //                string kk = RealN[RealN.Length - 1];



        //                if ((f0.Contains("缺陷") && !kk.StartsWith("B")) || kk.StartsWith("B"))
        //                {
        //                    rightNG_count++;

        //                    FinishFlagMutex.WaitOne();
        //                    FinishFlag.Add(false);
        //                    FinishFlagMutex.ReleaseMutex();



        //                    smartThreadPool_0.QueueWorkItem(() =>
        //                    {

        //                        try
        //                        {
        //                            string f_ORG = f0;
        //                            string f = kk.TrimEnd("_缺陷.jpg".ToCharArray()).TrimEnd("_OK.jpg".ToCharArray());

        //                            string Kind = "";
        //                            int StationN = -1;

        //                            if (f == "A1")
        //                            {
        //                                StationN = 0;
        //                                Kind = "A";
        //                            }

        //                            if (f == "A2")
        //                            {
        //                                StationN = 1;
        //                                Kind = "A";
        //                            }

        //                            if (f == "USB" || f == "USB1")
        //                            {
        //                                StationN = 2;
        //                                Kind = "A";
        //                            }

        //                            if (f == "D1" || f == "D2")
        //                            {
        //                                StationN = 3;
        //                                Kind = "A";
        //                            }

        //                            if (f == "C1_1" || f == "C2_1" || f == "C3_1" || f == "C4_1")
        //                            {
        //                                StationN = 4;
        //                                Kind = "C1C2";
        //                            }

        //                            if (f == "C5_1" || f == "C6_1" || f == "C7_1" || f == "C8_1")
        //                            {
        //                                StationN = 5;
        //                                Kind = "C1C2";
        //                            }


        //                            if (f == "B1_1" || f == "B1_2" || f == "B3_1" || f == "B3_2" || f == "B2" || f == "B4" || f == "B5" || f == "B6")
        //                            {
        //                                StationN = 6;
        //                                Kind = "B";
        //                            }

        //                            if (f == "CR1_1" || f == "CR1_2" || f == "CR1_3")
        //                            {
        //                                StationN = 7;
        //                                Kind = "CR1";
        //                            }

        //                            if (f == "CR2_1" || f == "CR2_2" || f == "CR2_3")
        //                            {
        //                                StationN = 8;
        //                                Kind = "CR1";
        //                            }

        //                            if (f == "CR3_1" || f == "CR3_2" || f == "CR3_3")
        //                            {
        //                                StationN = 9;
        //                                Kind = "CR3";
        //                            }

        //                            if (f == "CR4_1" || f == "CR4_2" || f == "CR4_3")
        //                            {
        //                                StationN = 10;
        //                                Kind = "CR3";
        //                            }

        //                            if (f == "C1" || f == "C3")
        //                            {
        //                                StationN = 11;
        //                                //Kind = "C4";
        //                                Kind = "B";
        //                                BSides_count++;
        //                            }

        //                            if (f == "C2" || f == "C4")
        //                            {
        //                                StationN = 12;
        //                                //Kind = "C4";
        //                                Kind = "B";
        //                                BSides_count++;
        //                            }

        //                            System.Drawing.Image ImageTemp = System.Drawing.Bitmap.FromFile(f_ORG);

        //                            int ISfirst = 0;
        //                            ImagPath1Mutex.WaitOne();
        //                            if (!imageGr_Path1.ContainsKey(Kind))
        //                            {
        //                                imageGr_Path1.Add(Kind, new ImageInfo1());
        //                                ISfirst++;
        //                            }
        //                            ImagPath1Mutex.ReleaseMutex();

        //                            if (ISfirst != 0)
        //                            {
        //                                ImageTemp = mainPanel.DrawStr(ImageTemp, f);
        //                            }


        //                            Navigator.mViewModel.DeStatus[StationN] = true;

        //                            ImagPath1Mutex.WaitOne();
        //                            imageGr_Path1[Kind].ListGroupAll.Add(ImageTemp);
        //                            imageGr_Path1[Kind].WidthCollect.Add(ImageTemp.Width);
        //                            imageGr_Path1[Kind].HeightCollect.Add(ImageTemp.Height);
        //                            ImagPath1Mutex.ReleaseMutex();

        //                        }
        //                        catch
        //                        {

        //                        }
        //                        finally
        //                        {

        //                        }

        //                        FinishFlagMutex.WaitOne();
        //                        if (FinishFlag.Count > 0)
        //                            FinishFlag.RemoveAt(0);
        //                        FinishFlagMutex.ReleaseMutex();

        //                    });


        //                }

        //                rightOK_count++;

        //                tmr.WaitOne(5);

        //            }


        //            dt0kk = DateTime.Now;

        //            while (FinishFlag.Count > 0 && (DateTime.Now - dt0kk).TotalMilliseconds < 5000)//tiger
        //            {
        //                tmr.WaitOne(10);
        //            }


        //            if (FinishFlag.Count > 0)
        //            {
        //                FinishFlagMutex.WaitOne();
        //                FinishFlag.Clear();
        //                FinishFlagMutex.ReleaseMutex();

        //                smartThreadPool_0.Cancel(true);

        //                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
        //                {
        //                    MesRes.AppendText("取图异常------请重新放置产品\r\n");
        //                    MesRes.ScrollToEnd();
        //                });

        //                return;
        //            }


        //            if (rightOK_count < 38)
        //            {
        //                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
        //                {
        //                    MesRes.AppendText("因为少图------请重新投递\r\n");
        //                    MesRes.ScrollToEnd();
        //                });

        //                return;
        //            }

        //            if (rightNG_count <= 0)
        //            {
        //                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
        //                {
        //                    MesRes.AppendText("本产品为OK产品------请直接上传MES为PASS！！！\r\n");
        //                    MesRes.ScrollToEnd();
        //                });
        //            }


        //            object OBBJ = new object();
        //            FinishFlag.Clear();

        //            foreach (string i in imageGr_Path1.Keys)
        //            {
        //                FinishFlagMutex.WaitOne();
        //                FinishFlag.Add(false);
        //                FinishFlagMutex.ReleaseMutex();

        //                smartThreadPool_0.QueueWorkItem(() =>
        //                {
        //                    try
        //                    {

        //                        ComPic CP_Path1Temp = new ComPic();
        //                        double height = imageGr_Path1[i].HeightCollect.Max();
        //                        double width = imageGr_Path1[i].WidthCollect.Sum();

        //                        if (imageGr_Path1[i].ListGroupAll.Count > 0)
        //                        {
        //                            System.Drawing.Image Iilist = CP_Path1Temp.JoinImageR_BACK(imageGr_Path1[i].ListGroupAll, (int)(width * WidthFactor), (int)(height * HeightFactor));

        //                            lock (OBBJ)
        //                            {
        //                                imageList_Path1.Add(Iilist);

        //                                if (!imageGr.ContainsKey(i))
        //                                    imageGr.Add(i, new ImageInfo1());
        //                            }

        //                            imageGr[i].MoveToXNG = imageGr_Path1[i].MoveToXNG;
        //                            imageGr[i].MoveToYNG = imageGr_Path1[i].MoveToYNG;
        //                        }
        //                    }
        //                    catch(Exception ex)
        //                    {

        //                    }
        //                    finally
        //                    {

        //                    }


        //                    FinishFlagMutex.WaitOne();
        //                    if (FinishFlag.Count > 0)
        //                        FinishFlag.RemoveAt(0);
        //                    FinishFlagMutex.ReleaseMutex();

        //                });
        //            }

        //            dt0kk = DateTime.Now;

        //            while (FinishFlag.Count > 0 && (DateTime.Now - dt0kk).TotalMilliseconds < 5000)//tiger
        //            {
        //                tmr.WaitOne(10);
        //            }


        //            if (FinishFlag.Count > 0)
        //            {
        //                FinishFlagMutex.WaitOne();
        //                FinishFlag.Clear();
        //                FinishFlagMutex.ReleaseMutex();

        //                smartThreadPool_0.Cancel(true);

        //                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
        //                {
        //                    MesRes.AppendText("画图异常------请重新放置产品\r\n");
        //                    MesRes.ScrollToEnd();
        //                });

        //                return;
        //            }


        //            CP_Path1.CAL_NGpic_MoveTo(imageGr_Path1, 200);



        //            ImageTempNG = CP_Path1.JoinImageC_BUILDING(imageList_Path1, (int)(CP_Path1.NGmaxX * 2), (int)(CP_Path1.NGmaxY * 2));


        //            Navigator.mViewModel.NotifyDeStatus();

        //            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
        //            {
        //                System.Drawing.Image Temp = null;

        //                if (cONDICTION1 == "缺陷")
        //                {
        //                    Temp = ImageTempNG;
        //                }


        //                if (Temp != null)
        //                {
        //                    this.mainPanel.Source = null;
        //                    GC.Collect();

        //                    this.mainPanel.Source = BitmapToBitmapImage(Temp);
        //                }

        //            });

        //        }
        //        catch
        //        {

        //        }
        //        finally
        //        {

        //        }
        //    }
        //}

        object ogw = new object();

        public void CheckAndShow_WHOLE(string SN, string Time)
        {
            lock (ogw)
            {
                if (cONDICTION1 == "缺陷")
                    return;

                AutoResetEvent tmr = new AutoResetEvent(false);

                try
                {

                    List<System.Drawing.Image> imageList_Path1 = new List<System.Drawing.Image>();
                    List<System.Drawing.Image> imageList_Path2 = new List<System.Drawing.Image>();
                    ComPic CP_Path1 = new ComPic();


                    //string ImagePath1 = SaveFolder1 + @"\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\" + SN;// FindDirec0(SaveFolder1, Time, SN + "-" + PN);

                    //ImagePath1 = AppDomain.CurrentDomain.BaseDirectory + "1324482578" + @"\Save";

                    string ImagePath1 = SaveFolder1 + @"\" + Time + @"\" + SN + @"\Save";

                    //string ImagePath1 = AppDomain.CurrentDomain.BaseDirectory + "1338250976" + @"\Save";

                    if (System.IO.Directory.Exists(ImagePath1) == false)//如果不存在就创建file文件夹
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            MesRes.AppendText("没有该产品图片------请检查网络连接状态或联系工程师\r\n");
                            MesRes.ScrollToEnd();
                        });

                        return;

                    }

                    List<bool> FinishFlag = new List<bool>();

                    Mutex FinishFlagMutex = new Mutex();

                    Mutex WholeMutex = new Mutex();

                    Mutex ImagPath1Mutex = new Mutex();

                    DateTime dt0kk = DateTime.Now;

                    foreach (string f0 in Directory.EnumerateFiles(ImagePath1, "*.jpg", SearchOption.TopDirectoryOnly))
                    {

                        string[] RealN = f0.Split('\\');

                        if (RealN == null || RealN.Length <= 0) continue;

                        string kk = RealN[RealN.Length - 1];

                        FinishFlagMutex.WaitOne();
                        FinishFlag.Add(false);
                        FinishFlagMutex.ReleaseMutex();


                        smartThreadPool_0.QueueWorkItem(() =>
                        {
                            try
                            {
                                string f_ORG = f0;
                                string f = kk;
                                f = f.TrimEnd("_缺陷.jpg".ToCharArray()).TrimEnd("_OK.jpg".ToCharArray());

                                string Kind = "";


                                if (f == "A1" || f == "A2")
                                {

                                    Kind = "A";
                                }

                                if (f == "USB" || f == "USB1")
                                {

                                    Kind = "A";
                                }

                                if (f == "D1" || f == "D2")
                                {

                                    Kind = "A";
                                }

                                if (f == "C1_1" || f == "C2_1" || f == "C3_1" || f == "C4_1")
                                {

                                    Kind = "C1C2";
                                }

                                if (f == "C5_1" || f == "C6_1" || f == "C7_1" || f == "C8_1")
                                {

                                    Kind = "C1C2";
                                }


                                if (f == "B1_1" || f == "B1_2" || f == "B3_1" || f == "B3_2" || f == "B2" || f == "B4" || f == "B5" || f == "B6")
                                {

                                    Kind = "B";
                                }

                                if (f == "CR1_1" || f == "CR1_2" || f == "CR1_3" || f == "CR2_1" || f == "CR2_2" || f == "CR2_3")
                                {

                                    Kind = "CR1";
                                }

                                if (f == "CR3_1" || f == "CR3_2" || f == "CR3_3" || f == "CR4_1" || f == "CR4_2" || f == "CR4_3")
                                {

                                    Kind = "CR3";
                                }

                                if (f == "C1" || f == "C2" || f == "C3" || f == "C4")
                                {

                                    Kind = "B";
                                }


                                if (!f_ORG.Contains("缺陷"))
                                {

                                    Tools tool1 = new Tools();
                                    System.Drawing.Image ImageTemp00 = System.Drawing.Image.FromFile(f_ORG);


                                    int ISfirst = 0;
                                    ImagPath1Mutex.WaitOne();
                                    if (!imageGr_Path1.ContainsKey(Kind))
                                    {
                                        imageGr_Path1.Add(Kind, new ImageInfo1());
                                        ISfirst++;
                                    }
                                    ImagPath1Mutex.ReleaseMutex();

                                    if (ISfirst != 0)
                                    {
                                        ImageTemp00 = tool1.DrawStr(ImageTemp00, f);
                                    }

                                    ImagPath1Mutex.WaitOne();
                                    imageGr_Path1[Kind].ListGroupOK.Add(ImageTemp00);
                                    imageGr_Path1[Kind].WidthCollect.Add(ImageTemp00.Width);
                                    imageGr_Path1[Kind].HeightCollect.Add(ImageTemp00.Height);
                                    ImagPath1Mutex.ReleaseMutex();
                                }
                            }
                            catch
                            {

                            }
                            finally
                            {

                            }

                            FinishFlagMutex.WaitOne();
                            if (FinishFlag.Count > 0)
                                FinishFlag.RemoveAt(0);
                            FinishFlagMutex.ReleaseMutex();

                        });

                        tmr.WaitOne(5);

                    }

                    while (FinishFlag.Count > 0 && (DateTime.Now - dt0kk).TotalMilliseconds < 7000)//tiger
                    {
                        tmr.WaitOne(10);
                    }


                    if (FinishFlag.Count > 0)
                    {
                        FinishFlagMutex.WaitOne();
                        FinishFlag.Clear();
                        FinishFlagMutex.ReleaseMutex();

                        smartThreadPool_0.Cancel(true);

                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            MesRes.AppendText("取图异常------请重新放置产品\r\n");
                            MesRes.ScrollToEnd();
                        });

                        return;
                    }


                    object OBBJWh = new object();
                    FinishFlag.Clear();

                    foreach (string i in imageGr_Path1.Keys)
                    {
                        FinishFlagMutex.WaitOne();
                        FinishFlag.Add(false);
                        FinishFlagMutex.ReleaseMutex();

                        smartThreadPool_0.QueueWorkItem(() =>
                        {
                            try
                            {
                                ComPic CP_Path1Temp = new ComPic();

                                imageGr_Path1[i].ListGroupAll.AddRange(imageGr_Path1[i].ListGroupOK);

                                double height = imageGr_Path1[i].HeightCollect.Max();

                                if (imageGr_Path1[i].ListGroupAll.Count > 0)
                                {

                                    System.Drawing.Image III = CP_Path1Temp.JoinImageR_BACK(imageGr_Path1[i].ListGroupAll, (int)(CP_Path1.ALLmaxX), (int)((height) * HeightFactor));
                                    imageGr_Path1[i].ListGroupOK.Clear();
                                    imageGr_Path1[i].ListGroupAll.Clear();
                                    GC.Collect();

                                    lock (OBBJWh)
                                    {
                                        imageList_Path1.Add(III);
                                    }  
                                }
                            }
                            catch(Exception ex)
                            {

                            }
                            finally
                            {

                            }


                            FinishFlagMutex.WaitOne();
                            if (FinishFlag.Count > 0)
                                FinishFlag.RemoveAt(0);
                            FinishFlagMutex.ReleaseMutex();



                        });
                    }

                    dt0kk = DateTime.Now;

                    while (FinishFlag.Count > 0 && (DateTime.Now - dt0kk).TotalMilliseconds < 5000)//tiger
                    {
                        tmr.WaitOne(10);
                    }


                    if (FinishFlag.Count > 0)
                    {
                        FinishFlagMutex.WaitOne();
                        FinishFlag.Clear();
                        FinishFlagMutex.ReleaseMutex();

                        smartThreadPool_0.Cancel(true);

                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            MesRes.AppendText("画图异常------请重新放置产品\r\n");
                            MesRes.ScrollToEnd();
                        });

                        return;
                    }

                    CP_Path1.CAL_all_MoveTo(imageGr_Path1, 200);

                    ImageTempALL = CP_Path1.JoinImageC_BUILDING(imageList_Path1, (int)(CP_Path1.ALLmaxX), (int)(CP_Path1.ALLmaxY * 1.7));

                    imageList_Path1.Clear();
                    GC.Collect();

                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        System.Drawing.Image Temp = null;

                        if (cONDICTION1 != "缺陷")
                        {
                            Temp = ImageTempALL;
                        }

                        if (Temp != null)
                        {
                            this.mainPanel.Source = null;
                            GC.Collect();
                            this.mainPanel.Source = BitmapToBitmapImage(Temp);
                        }

                    });

                }
                catch(Exception ex)
                {

                }
                finally
                {

                }
            }



        }

        private static PixelFormat[] indexedPixelFormats = { PixelFormat.Undefined, PixelFormat.DontCare,
PixelFormat.Format16bppArgb1555, PixelFormat.Format1bppIndexed, PixelFormat.Format4bppIndexed,
PixelFormat.Format8bppIndexed
        };

        private static bool IsPixelFormatIndexed(PixelFormat imgPixelFormat)
        {
            foreach (PixelFormat pf in indexedPixelFormats)
            {
                if (pf.Equals(imgPixelFormat)) return true;
            }

            return false;
        }


        public static BitmapImage BitmapToBitmapImage(System.Drawing.Image bitmap)
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

        //Dictionary<string, ImageInfo1> imageGr = new Dictionary<string, ImageInfo1>();
        System.Drawing.Image ImageTempALL;
        System.Drawing.Image ImageTempNG;

        public static object OJ = new object();
        public static bool logForbid = false;
        //public static void Log(string message, string path, string name)
        //{
        //    Task.Factory.StartNew(() =>
        //    {
        //        if (logForbid)
        //            return;

        //        lock (OJ)
        //        {
        //            try
        //            {
        //                if (!Directory.Exists(path))
        //                    Directory.CreateDirectory(path);
        //                string __stringFileName = path + name + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
        //                using (TextWriter logFile = TextWriter.Synchronized(File.AppendText(__stringFileName)))
        //                {
        //                    logFile.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff") + "  Message[" + message + "];");
        //                    logFile.Flush();
        //                    logFile.Close();
        //                }
        //            }
        //            catch 
        //            {

        //            }

        //            Thread.Sleep(20);
        //        }
        //    });
        //}



        public static void Log(string[] message, string path, string name)
        {
            //Task.Factory.StartNew(() =>
            //{
            if (logForbid)
                return;

            lock (OJ)
            {
                try
                {
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    string __stringFileName = path + name + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                    using (TextWriter logFile = TextWriter.Synchronized(File.AppendText(__stringFileName)))
                    {

                        foreach (string Msg in message)
                        {

                            logFile.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff") + "  Message[" + Msg + "];");
                        }

                        logFile.Flush();
                        logFile.Close();
                    }
                }
                catch
                {

                }

            }
            //});
        }
        public string FindDirec(string path, string key)
        {
            var di = new DirectoryInfo(path);

            var last = di.EnumerateDirectories().OrderBy(d => d.CreationTime).Last();

            foreach (string dd in Directory.EnumerateDirectories(last.FullName, "*", SearchOption.AllDirectories))
            {
                if (dd.Contains(key)) return dd;
            }
            return "";
        }

        public string FindDirec0(string path, string TimeC, string key)
        {
            var di = new DirectoryInfo(path);

            //List<string> diList = (List<string>)di.EnumerateDirectories();

            //foreach (string dd in Directory.EnumerateDirectories(path, "*", SearchOption.TopDirectoryOnly))
            //{
            //    foreach (string dd1 in Directory.EnumerateDirectories(dd, string.Format("%{0}%", key), SearchOption.TopDirectoryOnly))
            //    {
            //        return dd1;
            //    }
            //}


            foreach (string dd in Directory.EnumerateDirectories(path, string.Format("*{0}*", TimeC), SearchOption.TopDirectoryOnly))
            {
                foreach (string dd1 in Directory.EnumerateDirectories(dd, string.Format("*{0}*", key), SearchOption.TopDirectoryOnly))
                {
                    return dd1;
                }
            }
            return "";
        }

        bool Startl = false;
        private Mutex[] dahuamutex = new Mutex[14] { new Mutex(), null, null, null, null, null, null, null, null, null, null, null, null, null }; //大华对象的互斥量
        public Thread[] dahuaThread = new Thread[14] { null, null, null, null, null, null, null, null, null, null, null, null, null, null }; //大华线程


        public List<IGrabbedRawData> dahualist_0 = new List<IGrabbedRawData>(); //大华相机0图片列表
        public void DahuaCallBack_0(IGrabbedRawData bmp, int count)
        {
            if (Startl  && dahualist_0.Count == 0)
            {
                dahuamutex[0].WaitOne();
                dahualist_0.Add(bmp);
                dahuamutex[0].ReleaseMutex();
            }
            else
            {
                bmp = null;
            }
        }
        public void RunStatus(string water)
        {

            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                //  lock (OBBJ)
                {
                    Status.Content = water;
                    Thread.Sleep(100);
                }
            });

        }


        List<ImageSourceModel> models = new List<ImageSourceModel>();

        private static List<Bitmap>  fname = new List<Bitmap>();
        private static AutoResetEvent autoEvent;
        private void Button_OpenFile_Click(object sender, RoutedEventArgs e)
        {

            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

            string path0 = AppContext.BaseDirectory;

            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                path0 = folderBrowserDialog1.SelectedPath;

            }


            int p = 0;
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             

            this.mainPanel.Source = null;

            Task.Factory.StartNew(() =>
            {
                foreach (string dd in Directory.EnumerateDirectories(path0, "Image", SearchOption.AllDirectories))
                {

                    foreach (string f in Directory.EnumerateFiles(dd, "*.bmp", SearchOption.AllDirectories))
                    {

                        bool fla = true;

                        if (f.Contains("缺陷"))
                        {
                            fla = false;
                        }
                

                    }
                }
            });

        }
        Bitmap[] mm = null;
       static object oj = new object();



        public static BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            BitmapImage bitmapImage = new BitmapImage();
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            return bitmapImage;
        }

        public void OnComplate(object uu)
        {
            int u = (int)uu;
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
            {

                pbImageLeft[u].Source = BitmapToBitmapImage(mm[u]);
                mm[u] = null;
            }));
        }

        string SNRead = "";

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            //return;

            //Task.Factory.StartNew(() =>
            //{
                SentMes(SNRead);
            //});
            

        }

        object og1 = new object();
        void SentMes(string SNN, bool ng = false)
        {
            //Task.Factory.StartNew(() =>
            //{
            lock (LOAnswer)
            {
                try
                {
                    if (SNN != "")
                    //if (SNN != "" && TestHeadNumber != "")
                    {
                        bool MesBool = false;

                        string TimeStamp = "";
                        bool SnBool = false;
                        MentorAPI.cSelcompAPI MES = new MentorAPI.cSelcompAPI();

                        try
                        {

                            SnBool = MES.CheckRouteForSerialNumber(SNN);
                            //Log("检查路径Mes： " + SNN + "     " + MES.Message + "   " + SnBool + "    " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), logpath, "JUDGEMES");
                            //if (!SnBool)
                            {

                                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                                {
                                    MesRes.AppendText(SNN + "   " + MES.Message + "\r\n");
                                    MesRes.ScrollToEnd();
                                });
                            }
                        }
                        catch
                        {
                            this.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, (ThreadStart)delegate ()
                            {
                                System.Windows.Forms.MessageBox.Show("MES 异常！");
                            });
                        }

                        TimeStamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzz:00");

                        if (SnBool)
                        {
                            MesFamily Mf = new MesFamily();
                            string STc = "PASS";

                            if (ng) STc = "FAIL";

                            try
                            {

                                MesBool = MES.SaveResult(Mf.CreateWriteXmlStr(STc, SNN, TimeStamp, TestFixtureNumber, TestHeadNumber));
                                //Log("检查路径Mes： " + SNN + "     " + MES.Message + "   " + MesBool + "    " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), logpath, "JUDGEMES");
                                //if (!MesBool)
                                {

                                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                                    {
                                        MesRes.AppendText(SNN + "   " + MES.Message + "\r\n");
                                        MesRes.ScrollToEnd();
                                    });
                                }
                            }
                            catch
                            {
                                this.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, (ThreadStart)delegate ()
                                {
                                    System.Windows.Forms.MessageBox.Show("MES 异常！");
                                });
                            }



                            if (MesBool)
                            {


                                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                                {
                                    RESA.Foreground = new SolidColorBrush(Colors.Green);
                                    RESA.Text = "OK";
                                    Button_NG1.Background = new SolidColorBrush(Colors.Gray);
                                    Button_OK1.Background = new SolidColorBrush(Colors.Green);
                                    MesRes.AppendText(SNN + "上传MES成功\r\n");
                                    MesRes.ScrollToEnd();
                                });
                            }
                            else
                            {
                                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                                {
                                    RESA.Foreground = new SolidColorBrush(Colors.Red);
                                    RESA.Text = "NG";
                                    Button_NG1.Background = new SolidColorBrush(Colors.Red);
                                    Button_OK1.Background = new SolidColorBrush(Colors.Gray);
                                    MesRes.AppendText(SNN + "上传MES失败\r\n");
                                    MesRes.ScrollToEnd();
                                });

                            }
                        }


                    }
                }

                catch
                {

                }
                finally
                {

                }

            }

            //});
        }

        private void Button_NG_Click(object sender, RoutedEventArgs e)
        {
            SentMes(SNRead, true);
        }

        //Canvas[] canvas = new Canvas[20];
        System.Windows.Controls.Image[] pbImageLeft = new System.Windows.Controls.Image[22];
        private void dd(object sender, RoutedEventArgs e)
        {
            InitialCamCash();

            if (Init0())
            {
                this.ScanFlag = true;
                //this.Button_Start.IsEnabled = true;
                //this.Button_Stop.IsEnabled = true;
                //this.Log_mutex.WaitOne();
                //this.LogMsgList.Add("开启检测成功");
                //this.Log_mutex.ReleaseMutex();
                Status.Content = "开启检测成功";
            }
            else
            {
                //this.Log_mutex.WaitOne();
                //this.LogMsgList.Add("开启检测失败");
                //this.Log_mutex.ReleaseMutex();
                Status.Content = "开启检测失败";
            }
        }


        Process p = new Process();
        void StartService0(string FilePath, int tySel)
        {
            Environment.SetEnvironmentVariable("CUDA_VISIBLE_DEVICES", "0");
            try
            {
                string fileName = AppDomain.CurrentDomain.BaseDirectory + @"python\" + @"ocr.py";



                if (tySel == 0)
                {
                    p.StartInfo = new ProcessStartInfo("cmd", @"/K " + @"python " + fileName + " & pause")
                    {
                        RedirectStandardInput = false,
                        RedirectStandardOutput = false,
                        UseShellExecute = true,
                        CreateNoWindow = false
                    };
                    p.Start();
                }
                else
                {
                    p.StartInfo = new ProcessStartInfo(FilePath.TrimEnd('\\') + @"\python.exe", fileName)
                    {
                        RedirectStandardInput = false,
                        RedirectStandardOutput = false,
                        UseShellExecute = true,
                        CreateNoWindow = false
                    };
                    p.Start();
                }

                string ss = p.ProcessName;
            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show(ee.Message);
            }
        }
        void StartService(string FilePath, int tySel)
        {
            //         set PYTHONHOME =% CD %\Miniconda3
            //         set PATH =% PYTHONHOME %;% PYTHONHOME %\Library\bin;% PYTHONHOME %\Scripts;% PATH %
            //       set PYTHONPATH =

            Environment.SetEnvironmentVariable("CUDA_VISIBLE_DEVICES", "0");
            try
            {
                string fileName = AppDomain.CurrentDomain.BaseDirectory + @"python\" + @"ocr.py";



                if (tySel == 0)
                {
                    p.StartInfo = new ProcessStartInfo("cmd", @"/K " + @"python " + fileName + " & pause")
                    {
                        WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory + @"python",
                        RedirectStandardInput = false,
                        RedirectStandardOutput = false,
                        UseShellExecute = true,
                        CreateNoWindow = false
                    };
                    p.Start();
                }
                else
                {
                    p.StartInfo = new ProcessStartInfo(FilePath.TrimEnd('\\') + @"\python.exe", fileName)
                    {
                        WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory + @"python",
                        RedirectStandardInput = false,
                        RedirectStandardOutput = false,
                        UseShellExecute = true,
                        CreateNoWindow = false
                    };
                    p.Start();
                }

                string ss = p.ProcessName;
            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show(ee.Message);
            }

            Environment.SetEnvironmentVariable("CUDA_VISIBLE_DEVICES", "1");
            try
            {
                string fileName = AppDomain.CurrentDomain.BaseDirectory + @"python\" + @"aoi_cpu.py";



                if (tySel == 0)
                {
                    p.StartInfo = new ProcessStartInfo("cmd", @"/K " + @"python " + fileName + " & pause")
                    {
                        WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory + @"python",
                        RedirectStandardInput = false,
                        RedirectStandardOutput = false,
                        UseShellExecute = true,
                        CreateNoWindow = false
                    };
                    p.Start();
                }
                else
                {
                    p.StartInfo = new ProcessStartInfo(FilePath.TrimEnd('\\') + @"\python.exe", fileName)
                    {
                        WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory + @"python",
                        RedirectStandardInput = false,
                        RedirectStandardOutput = false,
                        UseShellExecute = true,
                        CreateNoWindow = false
                    };
                    p.Start();
                }

                string ss = p.ProcessName;
            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show(ee.Message);
            }
        }
        void SetEnviranmentParam()
        {

            try
            {
                string fileDir = AppDomain.CurrentDomain.BaseDirectory + @"python\Miniconda3";

                string dd = Environment.GetEnvironmentVariable("PATH");

                var dllDirectory = string.Format(@"{0};{0}\Library\mingw-w64\bin;{0}\Library\usr\bin;{0}\Library\bin;{0}\Scripts;", fileDir);

                Environment.SetEnvironmentVariable("PYTHONHOME", fileDir);
                Environment.SetEnvironmentVariable("PATH", dllDirectory + dd);
                Environment.SetEnvironmentVariable("PYTHONPATH", AppDomain.CurrentDomain.BaseDirectory + @"python");
            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show(ee.Message);
            }
        }




        protected override void OnClosing(CancelEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Would you want to exit?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {

                StopProcess(@"python");
                StopProcess(@"cmd");
            }
            else
            {
                e.Cancel = true;
            }
        }



        private void KillThread()
        {

            try
            {

                    if (dahua[0] != null)
                    {
                        dahua[0].StopGrab();
                        dahua[0].CloseDahua();
                        dahua[0].Dispose();
                    }

            }
            catch (Exception Exc)
            {
                System.Windows.MessageBox.Show(Exc.Message);
            }
        }

        public static void StopProcess(string processName)
        {
            try
            {
                System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcessesByName(processName);
                foreach (System.Diagnostics.Process p in ps)
                {
                    p.Kill();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            try
            {

                KillThread();
                GC.Collect();
                System.Environment.Exit(0);
            }
            catch (Exception ex)
            {

            }

            base.OnClosed(e);
            System.Windows.Application.Current.Shutdown();
        }

        private void chk_Click(object sender, RoutedEventArgs e)
        {
            var checkBoxes = new[] { Today, La15, Timee , SNCode };
            var current = (System.Windows.Controls.CheckBox)sender;
            foreach (var checkBox in checkBoxes)
            {
                if (checkBox != current  && checkBox.IsChecked==true)
                {
                    checkBox.IsChecked = !current.IsChecked;
                }
            }

            //string contion4 = "";
            //if (La15.IsChecked == true)
            //{
            //    ProductLi.Items.Clear();

            //    string str = DateTime.Now.AddMinutes(-1 * 15).ToString("yyyy/MM/dd HH:mm:ss");// DateTime.Now.ToString("yyyy/MM/dd") + string.Format(" {0}:{1}:{2}", DateTime.Now.Hour, (DateTime.Now.Minute - 15), DateTime.Now.Second);
            //    string str1 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            //    contion4 = string.Format(" Time between '{0}' and  '{1}'",
            //        str,
            //        str1);

            //    dt = recordManager.GetDataTable(string.Format("select * from {0} where {1}", "TestResult", contion4));

            //    string temp = "";

            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        temp = dt.Rows[i][2].ToString();

            //        if(temp !="")
            //        ProductLi.Items.Add(temp);
            //    }
            //}

      
        }

        private void Button_FirstOK_Click(object sender, RoutedEventArgs e)
        {
            server.dictConn[config_Temp[1]].Send("FirstUnite,OK\r\n");
        }

        private void Button_FirstNG_Click(object sender, RoutedEventArgs e)
        {
            server.dictConn[config_Temp[1]].Send("FirstUnite,NG\r\n");
        }

        private void Button_NGRate_Click(object sender, RoutedEventArgs e)
        {
            server.dictConn[config_Temp[1]].Send("NGRate,OK\r\n");
        }

        private void DaoChu(object sender, RoutedEventArgs e)
        {
            if(dt!=null && dt.Rows.Count>0)
            {
                string FilePath = AppDomain.CurrentDomain.BaseDirectory + @"SA\";

                if (System.IO.Directory.Exists(FilePath) == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(FilePath); //新建文件夹  
                }

                FilePath = FilePath + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + @".CSV";

                ExportDataTableToCSV(dt, FilePath);

                dt.Clear();
            }
            else
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    MesRes.AppendText("导出内容为空！\r\n");
                    MesRes.ScrollToEnd();
                });
            }

 
        }

        public static void ExportDataTableToCSV(DataTable dt, string Path)
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
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }

        private void okshow_Click(object sender, RoutedEventArgs e)
        {
            if(!(bool)ImgeIS.IsChecked)
                cONDICTION1 = "缺陷";
            else
                cONDICTION1 = "-";
        }


        public void PerfoCl(string msg)
        {
            Button_Connect.Dispatcher.Invoke(new System.Action(delegate{
            Button_Connect.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
            }));
        }

        private void Button_Connect_Click(object sender, RoutedEventArgs e)
        {
            //CheckAndShow("", "");
            //CheckAndShow_WHOLE("", "");
            //return;
            //Timeout timeout = new Timeout();
            //timeout.Do = WriterMES;
            //timeout.PARAM = "";
            // string  ISGood = timeout.DoWithTimeout(new TimeSpan(0, 0, 0,0,800));


            //for (int kk = 0; kk <= 1000; kk++)
            //{
            //    Task.Factory.StartNew(() =>
            //    {
            //        recordManager.InsertRecord820v4("TestResult", kk.ToString(), "MES", "OK");

            //    });

            //    Thread.Sleep(5);

            //}
            //return;


            Task.Factory.StartNew(() =>
            {
                string RemoteR = "";

                //if (SK.sockClient.Connected == false)
                //{
                //}

                do
                {

                    SK.Close_Com();

                    RemoteR = SK.open_Com(config_Temp[5].Split(',')[0], config_Temp[5].Split(',')[1]);


                    if (RemoteR == "0")
                    {
                        ListShowInfo(config_Temp[5] + "   " + "运动主机联机成功。\r\n");
                    }
                    else
                    {
                        ListShowInfo(config_Temp[5] + "   " + "运动主机联机失败。\r\n");
                    }

                    Thread.Sleep(1000);

                } while (RemoteR != "0");

            });

        }

        private void Button_Log_Click(object sender, RoutedEventArgs e)
        {
            string vca = WorkO.Text;
            

            Task.Factory.StartNew(() =>
            {
                //Stopwatch m_stopWatch0 = new Stopwatch();

                //m_stopWatch0.Restart();

                MentorAPI.cSelcompAPI MES = new MentorAPI.cSelcompAPI();

                bool IsLOG = false;// MES.CheckOPTrainingQualifications(vca,  Project,  QPOSITION);

                try
                {

                    IsLOG = MES.CheckOPTrainingQualifications(vca, Project, QPOSITION);

                    if (!IsLOG)
                    {

                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            MesRes.AppendText(MES.Message + "\r\n");
                            MesRes.ScrollToEnd();
                        });
                    }
                }
                catch
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, (ThreadStart)delegate ()
                    {
                        System.Windows.Forms.MessageBox.Show("MES 异常！");
                    });
                }

                this.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, (ThreadStart)delegate ()
                {
                    if (IsLOG)
                    {
                        MESSHOW.Text = MES.OperatorName + "登录";
                        TestHeadNumber = string.Format("工号: {0} 姓名: {1} Program: Version: Type:", vca, MES.OperatorName);
                    }
                    else
                    {
                        MESSHOW.Text = "登录失败";
                        TestHeadNumber = "";
                    }
                });


            
               
            });

        }
        public static bool cONDICTION_MES = false;

        private void ForbitMES_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)MESIS.IsChecked)
                cONDICTION_MES = true;
            else
                cONDICTION_MES = false;
        }

        private void MenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                if (ProductLi.SelectedItem!=null && !string.IsNullOrEmpty(ProductLi.SelectedItem.ToString()))
                {
                    Product_PN_Auto = ProductLi.SelectedItem.ToString();

                    ActiveQueueEvent.Set();//打开开关

                    Product_PN = ProductLi.SelectedItem.ToString();
                }
            }
            catch
            {

            }                
        }


        string ImagePath_DaoChu = "";
        string ImagePath_DaoChuJason = "";
        string OCR_DaoChu = "";
        private void Button_Daochu_Click(object sender, RoutedEventArgs e)
        {
            if(Product_PN=="")
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    MesRes.AppendText("产品条码为空或不正确\r\n");
                    MesRes.ScrollToEnd();
                });
                return;
            }

            string contion1 = string.Format(" OCR = '{0}'", Product_PN);

            string TaltoNum = recordManager.GetItemValue("TestResult", "Time", contion1);

            if (TaltoNum == "")
            {
                TaltoNum = DateTime.Now.ToString("yyyy-MM-dd");
            }

            string SN = recordManager.GetItemValue("TestResult", "SN", contion1);

            if (SN == "")
            {
                if (System.IO.Directory.Exists(ImagePath_DaoChu) == false)//如果不存在就创建file文件夹
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        MesRes.AppendText("没有记录！\r\n");
                        MesRes.ScrollToEnd();
                    });
                    return;

                }
            }

            string DateD = (Convert.ToDateTime(TaltoNum)).ToString("yyyy-MM-dd");

            ImagePath_DaoChu = SaveFolder1 + @"\" + DateD + @"\" + SN + @"\Save";
            ImagePath_DaoChuJason = SaveFolder1 + @"\" + DateD + @"\" + SN + @"\Json";

            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();


            string path0 = "";

            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                path0 = folderBrowserDialog1.SelectedPath;

            }
            else
            {
                return;
            }

            OCR_DaoChu = Product_PN;

            string ImageDirOK = path0 + @"\" + OCR_DaoChu +  @"\OK\";

            if (System.IO.Directory.Exists(ImageDirOK) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(ImageDirOK); //新建文件夹  
            }

            string ImageDirNG = path0 + @"\" + OCR_DaoChu + @"\NG\";

            if (System.IO.Directory.Exists(ImageDirNG) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(ImageDirNG); //新建文件夹  
            }

            string ImageDirOKJson = path0 + @"\" + OCR_DaoChu + @"\JsonOK\";

            if (System.IO.Directory.Exists(ImageDirOKJson) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(ImageDirOKJson); //新建文件夹  
            }

            string ImageDirNGJson = path0 + @"\" + OCR_DaoChu + @"\JsonNG\";

            if (System.IO.Directory.Exists(ImageDirNGJson) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(ImageDirNGJson); //新建文件夹  
            }

            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (System.IO.Directory.Exists(ImagePath_DaoChu) == false)//如果不存在就创建file文件夹
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            MesRes.AppendText("没有该产品图片------请检查网络连接状态或联系工程师\r\n");
                            MesRes.ScrollToEnd();
                        });
                        return;

                    }


                    foreach (string f0 in Directory.EnumerateFiles(ImagePath_DaoChu, "*.jpg", SearchOption.TopDirectoryOnly))
                    {
                        string[] RealN = f0.Split('\\');

                        if (RealN == null || RealN.Length <= 0) continue;

                        string kk = RealN[RealN.Length - 1];

                        System.Drawing.Image ImageTemp = System.Drawing.Bitmap.FromFile(f0);

                        string ImageDir = "";

                        if (f0.Contains("缺陷"))
                        {
                            ImageDir = ImageDirNG+kk;
                        }
                        else
                        {
                            ImageDir = ImageDirOK + kk;
                        }

                        ImageTemp.Save(ImageDir);
                    }


                    foreach (string f0 in Directory.EnumerateFiles(ImagePath_DaoChuJason, "*.json", SearchOption.TopDirectoryOnly))
                    {
                        string[] RealN = f0.Split('\\');

                        if (RealN == null || RealN.Length <= 0) continue;

                        string kk = RealN[RealN.Length - 1];

                        string COntentTemp = System.IO.File.ReadAllText(f0);

                        string ImageDir = "";

                        if (!f0.Contains("缺陷"))
                        {
                            //ImageDir = ImageDirOKJson + kk.TrimEnd(".json".ToCharArray())+ "_OK.json";
                            ImageDir = ImageDirOKJson + kk.TrimEnd(".json".ToCharArray()) + ".json";
                        }
                        else
                        {
                            ImageDir = ImageDirNGJson + kk;
                        }

                        //ImageDir = ImageDirNGJson + kk;

                        BPTools.FileLog(ImageDir, COntentTemp);
                    }
                }
                catch (Exception ex)
                {
                    ListShowInfo(ex.ToString() + "\r\n");
                }

            });
        }

        private void Button_OpenPath_Click(object sender, RoutedEventArgs e)
        {
            //SelfFuncAIM_XFMR("1325074435");
            //return;

            string ImagePath1 = AppDomain.CurrentDomain.BaseDirectory + "EXcelLog";// + @"\";
            System.Diagnostics.Process.Start(ImagePath1);
        }

        private void Button_DoubleC_Click(object sender, RoutedEventArgs e)
        {
            TBc.SelectedIndex = 0;
        }

        private void Button_checkP_Click(object sender, RoutedEventArgs e)
        {
            TBc.SelectedIndex = 1;
        }

        private void Button_Save_DJ(object sender, RoutedEventArgs e)
        {
            String FilePath = AppDomain.CurrentDomain.BaseDirectory + @"check\";

            if (System.IO.Directory.Exists(FilePath) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(FilePath); //新建文件夹  
            }

            FilePath = FilePath + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + @".CSV";

            string sign = "操作员签名(签全名):" + OPName.Text + ";ME签名(签全名):" + MEName.Text;
            string sign1 = ";确认人(签全名)::" + OPName2.Text + ";确认人(签全名):" + MEName2.Text;

            driver.cam.Tools.SaveFileToCSV(RecordTable.alarmDataGrid, FilePath, sign + sign1);
        }

        public static void LIST_info(ref List<string> sender, string kke, string infomationss)
        {
            try
            {
                for (int i = 0; i < sender.Count; i++)
                {
                    if (sender[i].Contains(kke))
                    {
                        sender[i] += infomationss;
                        break;
                    }
                }
            }
            catch
            {

            }
        }

        public static string GetLIST_info(ref List<string> sender, string kke)
        {
            try
            {
                for (int i = 0; i < sender.Count; i++)
                {
                    if (sender[i].Contains(kke))
                    {
                        return sender[i];
                    }
                }

            }
            catch
            {

            }
            return "";
        }

        public  void ADDLIST_info(ref List<string> sender, string kke)
        {
            try
            {
                for (int i = 0; i < sender.Count; i++)
                {
                    if (sender[i].Contains(kke))
                    {
                        return;
                    }
                }

                sender.Add(kke + "#");
                //recordManager.InsertRecord820v4("TestResult", kke, "Time", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            }
            catch
            {

            }

        }

        public static void RemoveLIST_info(ref List<string> sender, string kke)
        {
            try
            {
                int ire = -1;
                for (int i = 0; i < sender.Count; i++)
                {
                    if (sender[i].Contains(kke))
                    {
                        return;
                    }
                }

                if (ire > 0)
                    sender.RemoveAt(ire);
            }
            catch
            {

            }

        }

        private void TextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                string contion1 = string.Format(" OCR = '{0}'", SNin.Text);

                string SN = recordManager.GetItemValue("TestResult", "SN", contion1);
                if (SN == "")
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        MesRes.AppendText("没有搜索到SN号------请联系工程师或重新投递\r\n");
                        MesRes.ScrollToEnd();
                    });
                }
                else
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        MesRes.AppendText(SN + "\r\n");
                        MesRes.ScrollToEnd();
                    });
                }
                e.Handled = true;
            }
        }
    }

    public delegate string DoHandler(string s);
    public class Timeout
    {
        private ManualResetEvent mTimeoutObject;
        //标记变量
        private bool mBoTimeout;
        public DoHandler Do;
        public string PARAM;
        public string CallBackV;
        public Timeout()
        {
            //  初始状态为 停止
            this.mTimeoutObject = new ManualResetEvent(true);
        }
        ///<summary>
        /// 指定超时时间 异步执行某个方法
        ///</summary>
        ///<returns>执行 是否超时</returns>
        public string DoWithTimeout(TimeSpan timeSpan)
        {
            CallBackV = "";
            if (this.Do == null)
            {
                return "未实例化对象";
            }
            this.mTimeoutObject.Reset();
            this.mBoTimeout = true; //标记
            this.Do.BeginInvoke(PARAM, DoAsyncCallBack, null);
            // 等待 信号Set
            if (!this.mTimeoutObject.WaitOne(timeSpan, false))
            {
                this.mBoTimeout = true;
            }
            if (this.mBoTimeout) return "执行超时";
            return CallBackV;
        }
        ///<summary>
        /// 异步委托 回调函数
        ///</summary>
        ///<param name="result"></param>
        private void DoAsyncCallBack(IAsyncResult result)
        {
            try
            {
                CallBackV = this.Do.EndInvoke(result);
                // 指示方法的执行未超时
                this.mBoTimeout = false;

            }
            catch (Exception ex)
            {

                this.mBoTimeout = true;

            }
            finally
            {
                this.mTimeoutObject.Set();

            }
        }
    }
}
