using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Net.Sockets;
using System.Net;
using Demo.driver.cam;
using Demo.ui.view.page;
using Demo.ui.view.card;
using System.Windows;
using System.Text.RegularExpressions;
using Demo.ui;

namespace Demo.driver.accesarry
{
  public  class SocketComm
    {

        Thread threadClient = null; // 创建用于接收服务端消息的 线程；
        public Socket sockClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // 定义socket客户端（本机为客户端）
        IPEndPoint endPoint = null;
        IPAddress address = null;


        object ob3 = new object();
        public string open_Com(string Address, string PortName, int i = 0)
        {
            lock (ob3)
            {
                try
                {
                    if (sockClient != null)
                        sockClient.Close();

                    sockClient = null;

                    if (threadClient != null)
                        threadClient.Abort();

                    address = IPAddress.Parse(Address);

                    endPoint = new IPEndPoint(address, int.Parse(PortName));

                    sockClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    sockClient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 1500);
                    sockClient.Connect(endPoint);

                    threadClient = new Thread(RecMsg);
                    threadClient.IsBackground = true;
                    threadClient.Start();

                    return "0";

                }
                catch (SocketException ex)
                {
                    return "-1";
                }
            }

        }

        object obj = new object();
        byte[] Para1 = new byte[] { };
        driver.cam.Tools Tools1 = new driver.cam.Tools();
        public void SendData000(string str)
        {
             lock (obj)
            {
                
                byte[] Para = System.Text.Encoding.Default.GetBytes(str);// Tools1.StringToByteArray1(str);

                //MainFrame.Log("产品告知下位机中间： " + str + "     "  + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), MainFrame.logpath, "JUDGE");

                //WriteCOM(Para);
                sockClient.Send(Para); // 发送消息； 

                // str = "";
                //Para = null;

                Thread.Sleep(30);

            }

       
        }

        public string Connection_Status = null;
        object ob2 = new object();

        public void WriteCOM(byte[] byteBuffer)
        {
            //lock (ob2)
            {
                try
                {
                    sockClient.Send(byteBuffer); // 发送消息； 
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString());
                }
            }
        }

        public byte[] RecDataBuffer;
        StringBuilder sb = new StringBuilder(); //这个是用来保存：接收到了的，但是还没有结束的消息
        string terminateString = "\r\n";
        List<byte> DB = new List<byte>();
        public event MessageCallBack _CallBack;
        public delegate void MessageCallBack(string Msg);//声明

        public event NETBreackCallBack NetBreak;
        public delegate void NETBreackCallBack(string Msg);//声明


        AutoResetEvent tmr = new AutoResetEvent(false);

        public void DelivTCPInfo(string Msg)
        {
            _CallBack(Msg);
        }

        private void RecMsg()//接收数据
        {
            //address = IPAddress.Parse("192.168.1.62");


            //IPEndPoint endPoint2 = new IPEndPoint(address, int.Parse("5001"));
            while (true)
            {
                //lock (ob)
                {
                    byte[] RecDataBuffer0 = new byte[1024 * 5];
                    int length = -1;
                    String strMsg = "";
                    try
                    {
                        if (sockClient.Connected == false)
                        {
                            this.Close_Com();

                            string RemoteR = this.open_Com(address.ToString(), endPoint.Port.ToString());

                            if (RemoteR != "0")
                            {
                                DelivTCPInfo("Error:  " + endPoint.ToString() + "   " + "运动主机已经断开联机。\r\n");
                            }
                            else
                            {
                                DelivTCPInfo("Succed:  " + endPoint.ToString() + "   " + "运动主机重新联机成功。\r\n");
                            }

                            tmr.WaitOne(100);
                        }

                        length = sockClient.Receive(RecDataBuffer0); // 接收数据，并返回数据的长度；

                        if (length > 0)
                        {

                            RecDataBuffer = new byte[length];
                            Array.Copy(RecDataBuffer0, RecDataBuffer, length);


                                strMsg = System.Text.Encoding.Default.GetString(RecDataBuffer).Replace("\0", "");


                                string[] MsgArr = strMsg.Split(new string[] { "]T" }, StringSplitOptions.RemoveEmptyEntries);

                                int h = MsgArr.Length;

                                for (int g = 0; g < h; g++)
                                {
                                    string strMsg_Temp = "";
                                    //var r = Regex.Matches(MsgArr[g], @"\[(.+?)\]");
                                    var r = Regex.Matches(MsgArr[g] + "]", @"\[(.*?)\]");
                                    foreach (Match x in r)
                                    {
                                        strMsg_Temp += x.Groups[1].Value.TrimEnd(';') + ",";
                                    }

                                    DelivTCPInfo(strMsg_Temp.TrimEnd(','));
                                }
                     
                        }
                        else
                        {
                            NetBreak("");
                        }

                        tmr.WaitOne(30);

                    }
                    catch (Exception ex)
                    {

                        if (RecDataBuffer0 != null && RecDataBuffer0.Length > 0)
                            Array.Clear(RecDataBuffer0, 0, RecDataBuffer0.Length);

                        if (RecDataBuffer != null && RecDataBuffer.Length > 0)
                            Array.Clear(RecDataBuffer, 0, RecDataBuffer.Length);
                        return;
                    }
                }
            }
        }

        public void Close_Com()
        {
            try
            {
                if (sockClient != null)
                {
                    sockClient.Close();
                }

                if (threadClient != null)
                    threadClient.Abort();


                //if (threadHeartBeat != null)
                //    threadHeartBeat.Abort();


                //return ErrorCode.TM_SUCCESS;
            }
            catch (System.Exception)
            {
                //return ErrorCode.TM_EXCEPTION;
            }
        }

    }
}
