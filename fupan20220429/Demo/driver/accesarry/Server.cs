using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using Demo.utilities;

using ThridLibray;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.InteropServices;
using Demo.ui.view.page;
using Demo.ui;

namespace Demo.driver.cam
{

    public class Server
    {

        public event MessageCallBack _CallBack;


        TcpListener server = null;

        public string m_ip = "";
        public string m_port = "";


        private Thread LogThread = null;
        private Mutex Log_mutex = new Mutex();
        private bool Logflag = true;
        private List<string> LogMsgList = new List<string>();

        #region TCP日志
        public static string path = "TCPLog\\";
        public static string name = "TCP_";

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="message">log 信息</param>
        /// <param name="path">log 路径 </param>
        /// <param name="name">log日志名称</param>
        public static void Log(string message, string path, string name)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string __stringFileName = path + name + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                using (TextWriter logFile = TextWriter.Synchronized(File.AppendText(__stringFileName)))
                {
                    logFile.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff") + "  Message[" + message + "];");
                    logFile.Flush();
                    logFile.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void ProcessLogThread()
        {
            while (true)
            {
                if (Logflag)
                {
                    if (LogMsgList.Count > 0)
                    {
                        string Msg = LogMsgList[0];
                        Log_mutex.WaitOne();
                        LogMsgList.RemoveAt(0);
                        Log_mutex.ReleaseMutex();

                        Log(Msg, path, name);
                    }
                }
                Thread.Sleep(100);
            }
        }
        #endregion

        public Server(string IP, string Port)
        {
            m_ip = IP;
            m_port = Port;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    int port = Convert.ToInt32(Port);
                    IPEndPoint listenPort = new IPEndPoint(IPAddress.Parse(IP), port);
                    server = new TcpListener(listenPort);//初始化TcpListener的新实例
                    server.Start();//开始监听客户端的请求

                    while (true)//循环监听
                    {
                        TcpClient client = server.AcceptTcpClient();
                        bool IsCont = false;

                        for (int y=1;y<4;y++)
                        {
                            string bb = client.Client.RemoteEndPoint.ToString();
                            if (client.Client.RemoteEndPoint.ToString().Contains(MainFrame.config_Temp[y]))
                            {
                                IsCont = true;
                                ConnectionClient connection = new ConnectionClient(client, this);
                                connection.KeyIndex = IP + "," + Port;

                                dictConn[MainFrame.config_Temp[y]] = connection;
                                string str = "TCP客户端 [IP:" + client.Client.RemoteEndPoint.ToString() + "]链接成功";

                                if ("192.168.1.62" == IP)
                                {
                                    MainFrame.count = 1200;
                                }

                                DelivTCPInfo(str);
                                break;
                            }
                            //AutoRunPage.ServerMutex.ReleaseMutex();
                        }

                        if(!IsCont)
                        {
                            ConnectionClient connection = new ConnectionClient(client, this);

                            if(dictConn.ContainsKey(client.Client.RemoteEndPoint.ToString()))
                            {
                                dictConn.Add(client.Client.RemoteEndPoint.ToString(), null);
                            }

                            dictConn[client.Client.RemoteEndPoint.ToString()] = connection;
                        }


                    }
                }
                catch (Exception e)
                {

                }
                finally
                {
                    server.Stop();
                }

            });
        }

        ~Server()
        {
            CloseTCP();
            string str = "TCP服务器 [IP:" + m_ip.ToString() + " ,Port:" + m_port.ToString() + "]自动析构";
            //Log(str, path, name);
        }

        private void SafeClose(TcpListener sok)
        {
            if (sok == null) return;
            //if (socket != null) socket.Close();
            try
            {
                sok.Stop();
            }
            catch (Exception)
            {
            }
        }
        public void CloseTCP()
        {
            if (LogThread != null)
            {
                if (Logflag) Logflag = false;
                Thread.Sleep(50);
                LogThread.Abort();
                LogThread = null;
            }
            SafeClose(server);
            string str = "TCP客户端 [IP:" + m_ip.ToString() + " ,Port:" + m_port.ToString() + "]关闭成功";
            Log_mutex.WaitOne();
            LogMsgList.Add(str);
            Log_mutex.ReleaseMutex();
        }


        public  Dictionary<string, ConnectionClient> dictConn = new Dictionary<string, ConnectionClient>();




        public void DelivTCPInfo(string Msg)
        {
            _CallBack(Msg);
        }

        public bool GetStatus()
        {
            return true;
            return (server != null);
        }

        public void LogWrite(string str)
        {
            Log_mutex.WaitOne();
            LogMsgList.Add(str);
            Log_mutex.ReleaseMutex();
        }


    }

    public class ConnectionClient
    {
        TcpClient sokMsg;
        Thread threadMsg;
        bool isRec = true;
        Server FatherServer = null;
        public UInt32[] ControlBoolArr = new UInt32[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        public string KeyIndex = "";
        NetworkStream stream = null;
        #region 构造函数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sokMsg">通信套接字</param>
        /// <param name="dgShowMsg">向主窗体文本框显示消息的方法委托</param>
        public ConnectionClient(TcpClient CL, Server SVFath)
        {
            sokMsg = CL;
            stream = sokMsg.GetStream();
            sokMsg.SendTimeout = 1200;
            sokMsg.Client.SendTimeout = 1200;
            //KeyIndex = sokMsg.Client.RemoteEndPoint.ToString().Split(':')[0]+","+sokMsg.Client.RemoteEndPoint.ToString().Split(':')[1];
            FatherServer = SVFath;
            this.threadMsg = new Thread(Receive);
            this.threadMsg.IsBackground = true;
            this.threadMsg.Start();
        }
        #endregion


        StringBuilder sb = new StringBuilder(); //这个是用来保存：接收到了的，但是还没有结束的消息
        string terminateString = "\r\n";

        private void Receive()
        {

            while (true)
            {
                try
                {

                    if (sokMsg.Connected)
                    {
                        byte[] msgArr = new byte[1024 * 5];//接收到的消息的缓冲区
                        int length = 0;
                        //接收服务端发送来的消息数据
                        //try
                        //{

                        length = stream.Read(msgArr, 0, msgArr.Length);

                        //}
                        //catch (Exception ex)
                        //{
                        //    FatherServer.LogWrite(ex.ToString());

                        //    MessageBox.Show(ex.ToString());
                        //}

                        if (length > 0)
                        {
                            try
                            {
                                String strMsg = "";

                                //strMsg = System.Text.Encoding.Default.GetString(msgArr).Replace("\0", "");
                                strMsg = System.Text.Encoding.Default.GetString(msgArr).Replace("\0", "");

                                if (FatherServer.m_ip == "192.168.0.204")
                                {

                                    FatherServer.DelivTCPInfo(strMsg);

                                }
                                else
                                {
                                    int rnFixLength = terminateString.Length; //这个是指消息结束符的长度，此处为\r\n
                                    for (int i = 0; i < strMsg.Length;) //遍历接收到的整个buffer文本
                                    {
                                        if (i <= strMsg.Length - rnFixLength)
                                        {
                                            if (strMsg.Substring(i, rnFixLength) != terminateString) //非消息结束符，则加入sb
                                            {
                                                sb.Append(strMsg[i]);
                                                i++;
                                            }
                                            else
                                            {
                                                if (FatherServer.m_port == "5001" && FatherServer.m_ip == "192.168.1.62")
                                                {
                                                    MainFrame.ToFupan = sb.ToString();
                                                }
                                                else
                                                {
                                                    FatherServer.DelivTCPInfo(sb.ToString());

                                                    //MainFrame.Log("收到原始信号： "+ sb + "     " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), MainFrame.logpath, "SUANFA");
                                                }

                                                i += rnFixLength;
                                                sb.Clear();
                                            }
                                        }
                                        else
                                        {
                                            sb.Append(strMsg[i]);
                                            i++;
                                        }
                                    }


                                }

                                Array.Clear(msgArr, 0, msgArr.Length);


                                Thread.Sleep(10);


                            }
                            catch (Exception ex)
                            {
                                //MessageBox.Show(ex.ToString());
                            }

                        }
                    }

                }
                catch (Exception ex)
                {
                    FatherServer.LogWrite(ex.ToString());

                    //MessageBox.Show(ex.ToString());
                }

                Thread.Sleep(10);

            }
        }


        object obj = new object();
        byte[] Para1 = new byte[] { };
        driver.cam.Tools Tools1 = new driver.cam.Tools();

        public string SendData000(string str)
        {
            string ss = "";
            SocketError errorCode = SocketError.Success;

            lock (obj)
            {
                byte[] Para = System.Text.Encoding.Default.GetBytes(str);

                for (int i = 0; i < 3; i++)
                {
                    sokMsg.Client.Send(Para, 0, Para.Length, 0, out errorCode);

                    if (errorCode.ToString().Contains("Success"))
                    {
                        break;
                    }
                    else
                    {
                        Thread.Sleep(80);
                    }
                }

                ss = errorCode.ToString();

                //MainFrame.Log(str + "    Socket发送结果：  " + ss +"    " +DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), MainFrame.logpath, "JUDGETCP");

                Thread.Sleep(30);
            }
            return ss;
        }

        public string Connection_Status = null;
        object ob2 = new object();

        public string WriteCOM(byte[] byteBuffer)
        {
            //lock (ob2)
            {
                try
                {
                    try
                    {
                        stream.Write(byteBuffer, 0, byteBuffer.Length);

                        return "OK";
                    }
                    catch (System.ObjectDisposedException ex)
                    {
                        return ex.ToString();
                    }
                }
                catch (System.IO.IOException ex)
                {
                    return ex.ToString();
                }
            }
        }

        #region 03向客户端发送消息
        /// <summary>
        /// 向客户端发送消息
        /// </summary>
        /// <param name="strMsg"></param>
        public void Send(string strMsg)
        {
            try
            {
                byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes(strMsg);
          
                stream.Write(arrMsg, 0, arrMsg.Length);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }

        }
        #endregion

        object oj = new object();
  


        public void SendInt(UInt32 PAN, byte WKN, byte CAMN, UInt16 re = 1, Int32 Xpos = 0, Int32 Ypos = 0, Int32 Zpos = 0)
        {
            //lock (oj)
            {

                try
                {
                    if (sokMsg == null) return;
                    SendStruct send = new SendStruct();
                    send.WorkNo = (byte)WKN;
                    send.PartNum = PAN;
                    send.CapNo = (byte)CAMN;
                    send.Res = (UInt16)re;
                    send.Xpos = Xpos;
                    send.Ypos = Ypos;
                    send.Zpos = Zpos;
                    send.FrameStart = 0x4E504E50;
                    send.FrameEnd = 0x24242424;
                    byte[] arrMsg = Telc.StructureToByte<SendStruct>(send);

                    stream.Write(arrMsg, 0, arrMsg.Length);


                    string str = "TCP服务器端 [IP:" + FatherServer.m_ip.ToString() + " ,Port:" + FatherServer.m_port.ToString() + "] 发送数据:[" + PAN.ToString() + "," + WKN.ToString() + "," + CAMN.ToString() + "," + "]成功";
                    FatherServer.LogWrite(str);

                    Thread.Sleep(50);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        public string Analyze_Res(RecStruct INData)
        {
            try
            {
                if (INData.FrameStart != 0x4E504E50 || INData.FrameEnd != 0x24242424) return "error";
                return "ok";
            }
            catch (Exception ex)
            {
                return "error";
            }
        }

    }
}

