using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace Demo.utilities
{
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    public struct RecStruct
    {
        public UInt32 FrameStart;
        public UInt32 PartNum;
        public byte WorkNo;
        public byte CapNo;
        public UInt16 CMD;
        public UInt32 FrameEnd;
    }

    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    public struct SendStruct
    {
        public UInt32 FrameStart;
        public UInt32 PartNum;
        public byte WorkNo;
        public byte CapNo;
        public byte Res;
        public UInt32 FrameEnd;
    }
    public class BPTools
    {
        public static bool GetBitValue(int value, int index)
        {
            if (index > 31) throw new ArgumentOutOfRangeException("index");
            var val = 1 << index;
            return (value & val) == val;
        }

        public static int SetBitValue(int value, int index, bool bitValue)
        {
            if (index > 31) throw new ArgumentOutOfRangeException("index");
            var val = 1 << index;
            return bitValue ? (value | val) : (value & ~val);
        }

        public static char HexChar(char c)
        {
            if ((c >= '0') && (c <= '9'))
                return Convert.ToChar(c - 0x30);
            else if ((c >= 'A') && (c <= 'F'))
                return Convert.ToChar(c - 'A' + 10);
            else if ((c >= 'a') && (c <= 'f'))
                return Convert.ToChar(c - 'a' + 10);
            else
                return Convert.ToChar(0x10);
        }

        public static bool IsArrayZero(byte[] bytes)
        {
            try
            {
                if (bytes == null) return true;
                if (bytes.Length == 0) return true;

                for (int i = 0; i < bytes.Length; ++i)
                {
                    if (bytes[i] != 0)
                    {
                        return false;
                    }
                }
                return true;

            }
            catch (Exception)
            {
                return true;
            }

        }

        public static int Str2Hex(string str, char[] data)
        {
            int t, t1;
            int rlen = 0, len = str.Length;
            for (int i = 0; i < len;)
            {
                char l, h = str[i];
                if (h == ' ')
                {
                    i++;
                    continue;
                }
                i++;
                if (i >= len) break;
                l = str[i];
                t = HexChar(h);
                t1 = HexChar(l);
                if ((t == 16) || (t1 == 16))
                    break;
                else
                    t = t * 16 + t1;
                i++;
                data[rlen] = (char)t;

                rlen++;
            }
            return rlen;
        }

        public static int Str2byteArray(string str, ref byte[] data)
        {
            string[] StArr = str.Split(' ');

            for (int i = 0; i < StArr.Length; i++)
            {
                data[i] = Convert.ToByte(StArr[i], 16);
            }


            return StArr.Length;
        }

        public static int char2int(char ch)
        {
            switch (ch)
            {
                case '1':
                    {
                        return 1;

                    }
                case '2':
                    {
                        return 2;

                    }
                case '3':
                    {
                        return 3;

                    }
                case '4':
                    {
                        return 4;

                    }
                case '5':
                    {
                        return 5;

                    }
                case '6':
                    {
                        return 6;

                    }
                case '7':
                    {
                        return 7;

                    }
                case '8':
                    {
                        return 8;

                    }
                case '9':
                    {
                        return 9;

                    }
                case 'A':
                    {
                        return 10;

                    }
                case 'B':
                    {
                        return 11;

                    }
                case 'C':
                    {
                        return 12;

                    }
                case 'D':
                    {
                        return 13;

                    }
                case 'E':
                    {
                        return 14;

                    }
                case 'F':
                    {
                        return 15;

                    }
                case '0':
                    {
                        return 0;

                    }
            }
            return 0;
        }


        public static int hexCharToValue(char ch)
        {

            int result = 0;
            if (ch >= '0' && ch <= '9')
            {
                result = (int)(ch - '0');
            }
            else if (ch >= 'a' && ch <= 'f')
            {
                result = (int)(ch - 'a') + 10;
            }
            else if (ch >= 'A' && ch <= 'F')
            {
                result = (int)(ch - 'A') + 10;
            }
            else
            {
                result = -1;
            }
            return result;
        }

        public static int Hex2int(string str)
        {
            try
            {
                char[] cha = str.ToCharArray();
                int nPos1, nPos2, nPos;

                nPos1 = char2int(cha[6]) * 16 * 16 * 16 + char2int(cha[7]) * 16 * 16 + char2int(cha[8]) * 16 + char2int(cha[9]);    //低8位的值
                nPos2 = char2int(cha[10]) * 16 * 16 * 16 * 16 * 16 * 16 * 16 + char2int(cha[11]) * 16 * 16 * 16 * 16 * 16 * 16 + char2int(cha[12]) * 16 * 16 * 16 * 16 * 16 + char2int(cha[13]) * 16 * 16 * 16 * 16;   //高8位的值
                nPos = nPos1 + nPos2;
                return nPos;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static T Byte2Struct<T>(byte[] arr)
        {
            int structSize = Marshal.SizeOf(typeof(T));
            IntPtr ptemp = Marshal.AllocHGlobal(structSize);
            Marshal.Copy(arr, 0, ptemp, structSize);
            T rs = (T)Marshal.PtrToStructure(ptemp, typeof(T));
            Marshal.FreeHGlobal(ptemp);
            return rs;
        }

        public static byte[] Struct2Byte<T>(T s)
        {
            int structSize = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[structSize];
            //分配结构体大小的内存空间 
            IntPtr structPtr = Marshal.AllocHGlobal(structSize);
            //将结构体拷到分配好的内存空间 
            Marshal.StructureToPtr(s, structPtr, false);
            //从内存空间拷到byte数组 
            Marshal.Copy(structPtr, buffer, 0, structSize);
            //释放内存空间 
            Marshal.FreeHGlobal(structPtr);
            return buffer;
        }

        public static string filename = "";
        public static void WriteInfo(byte[] bt, string fpath)
        {
            if (File.Exists(fpath))
            {
                File.Delete(fpath);
                return;
            }

            FileStream fs = new FileStream(fpath, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bt);
            bw.Flush();

            bw.Close();
            fs.Close();
        }

        public static byte[] ReadInfo(string file)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                BinaryReader br = new BinaryReader(fs);

                byte[] bt = br.ReadBytes(144);
                br.Close();
                fs.Close();

                return bt;
            }
            catch (Exception ex)
            {
                return new byte[144];
            }
        }


        public static byte[] StringToByteArray1(string s)
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

        public static string ByteToHexString(byte s)
        {
            try
            {
                return Convert.ToString(s, 16).ToUpper();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static long IpToInt(string ip)
        {
            try
            {
                string[] items = ip.Split('.');
                //这里|可以换成+ 因为转化二进制 后面的位数都是0 所以能用 |
                return long.Parse(items[0]) << 24
                        | long.Parse(items[1]) << 16
                        | long.Parse(items[2]) << 8
                        | long.Parse(items[3]);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        static ulong[] Crc32Table;
        private static Object thisLock1 = new Object();

        /// <summary>
        /// 获取字符串的CRC32校验值
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static uint StmCalcCrc(byte[] buf, int offset, int len)
        {
            lock (thisLock1)
            {
                uint nReg = 0xFFFFFFFF;
                uint nTemp = 0;
                int i, n;

                for (n = 0; n < len; n++)
                {
                    nReg ^= (uint)buf[offset + n];
                    for (i = 0; i < 4; i++)
                    {
                        nTemp = crc32table[(nReg >> 24) & 0xff]; //取一个字节，查表
                        nReg <<= 8;                        //丢掉计算过的头一个BYTE
                        nReg ^= nTemp;                //与前一个BYTE的计算结果异或 
                    }
                }
                return nReg;
            }
        }
        /// <summary>
        /// CRC表
        /// </summary>
        static readonly uint[] crc32table =
        {
            0x00000000,0x04C11DB7,0x09823B6E,0x0D4326D9,0x130476DC,0x17C56B6B,0x1A864DB2,0x1E475005,
            0x2608EDB8,0x22C9F00F,0x2F8AD6D6,0x2B4BCB61,0x350C9B64,0x31CD86D3,0x3C8EA00A,0x384FBDBD,
            0x4C11DB70,0x48D0C6C7,0x4593E01E,0x4152FDA9,0x5F15ADAC,0x5BD4B01B,0x569796C2,0x52568B75,
            0x6A1936C8,0x6ED82B7F,0x639B0DA6,0x675A1011,0x791D4014,0x7DDC5DA3,0x709F7B7A,0x745E66CD,
            0x9823B6E0,0x9CE2AB57,0x91A18D8E,0x95609039,0x8B27C03C,0x8FE6DD8B,0x82A5FB52,0x8664E6E5,
            0xBE2B5B58,0xBAEA46EF,0xB7A96036,0xB3687D81,0xAD2F2D84,0xA9EE3033,0xA4AD16EA,0xA06C0B5D,
            0xD4326D90,0xD0F37027,0xDDB056FE,0xD9714B49,0xC7361B4C,0xC3F706FB,0xCEB42022,0xCA753D95,
            0xF23A8028,0xF6FB9D9F,0xFBB8BB46,0xFF79A6F1,0xE13EF6F4,0xE5FFEB43,0xE8BCCD9A,0xEC7DD02D,
            0x34867077,0x30476DC0,0x3D044B19,0x39C556AE,0x278206AB,0x23431B1C,0x2E003DC5,0x2AC12072,
            0x128E9DCF,0x164F8078,0x1B0CA6A1,0x1FCDBB16,0x018AEB13,0x054BF6A4,0x0808D07D,0x0CC9CDCA,
            0x7897AB07,0x7C56B6B0,0x71159069,0x75D48DDE,0x6B93DDDB,0x6F52C06C,0x6211E6B5,0x66D0FB02,
            0x5E9F46BF,0x5A5E5B08,0x571D7DD1,0x53DC6066,0x4D9B3063,0x495A2DD4,0x44190B0D,0x40D816BA,
            0xACA5C697,0xA864DB20,0xA527FDF9,0xA1E6E04E,0xBFA1B04B,0xBB60ADFC,0xB6238B25,0xB2E29692,
            0x8AAD2B2F,0x8E6C3698,0x832F1041,0x87EE0DF6,0x99A95DF3,0x9D684044,0x902B669D,0x94EA7B2A,
            0xE0B41DE7,0xE4750050,0xE9362689,0xEDF73B3E,0xF3B06B3B,0xF771768C,0xFA325055,0xFEF34DE2,
            0xC6BCF05F,0xC27DEDE8,0xCF3ECB31,0xCBFFD686,0xD5B88683,0xD1799B34,0xDC3ABDED,0xD8FBA05A,
            0x690CE0EE,0x6DCDFD59,0x608EDB80,0x644FC637,0x7A089632,0x7EC98B85,0x738AAD5C,0x774BB0EB,
            0x4F040D56,0x4BC510E1,0x46863638,0x42472B8F,0x5C007B8A,0x58C1663D,0x558240E4,0x51435D53,
            0x251D3B9E,0x21DC2629,0x2C9F00F0,0x285E1D47,0x36194D42,0x32D850F5,0x3F9B762C,0x3B5A6B9B,
            0x0315D626,0x07D4CB91,0x0A97ED48,0x0E56F0FF,0x1011A0FA,0x14D0BD4D,0x19939B94,0x1D528623,
            0xF12F560E,0xF5EE4BB9,0xF8AD6D60,0xFC6C70D7,0xE22B20D2,0xE6EA3D65,0xEBA91BBC,0xEF68060B,
            0xD727BBB6,0xD3E6A601,0xDEA580D8,0xDA649D6F,0xC423CD6A,0xC0E2D0DD,0xCDA1F604,0xC960EBB3,
            0xBD3E8D7E,0xB9FF90C9,0xB4BCB610,0xB07DABA7,0xAE3AFBA2,0xAAFBE615,0xA7B8C0CC,0xA379DD7B,
            0x9B3660C6,0x9FF77D71,0x92B45BA8,0x9675461F,0x8832161A,0x8CF30BAD,0x81B02D74,0x857130C3,
            0x5D8A9099,0x594B8D2E,0x5408ABF7,0x50C9B640,0x4E8EE645,0x4A4FFBF2,0x470CDD2B,0x43CDC09C,
            0x7B827D21,0x7F436096,0x7200464F,0x76C15BF8,0x68860BFD,0x6C47164A,0x61043093,0x65C52D24,
            0x119B4BE9,0x155A565E,0x18197087,0x1CD86D30,0x029F3D35,0x065E2082,0x0B1D065B,0x0FDC1BEC,
            0x3793A651,0x3352BBE6,0x3E119D3F,0x3AD08088,0x2497D08D,0x2056CD3A,0x2D15EBE3,0x29D4F654,
            0xC5A92679,0xC1683BCE,0xCC2B1D17,0xC8EA00A0,0xD6AD50A5,0xD26C4D12,0xDF2F6BCB,0xDBEE767C,
            0xE3A1CBC1,0xE760D676,0xEA23F0AF,0xEEE2ED18,0xF0A5BD1D,0xF464A0AA,0xF9278673,0xFDE69BC4,
            0x89B8FD09,0x8D79E0BE,0x803AC667,0x84FBDBD0,0x9ABC8BD5,0x9E7D9662,0x933EB0BB,0x97FFAD0C,
            0xAFB010B1,0xAB710D06,0xA6322BDF,0xA2F33668,0xBCB4666D,0xB8757BDA,0xB5365D03,0xB1F740B4
        };

        public static void FileLog(string fileName,string ContentF)
        {
            try
            {
                System.IO.FileStream fileStream = null;


                //string fileFullName = AppDomain.CurrentDomain.BaseDirectory + fileName;
               
                if (!System.IO.File.Exists(fileName))
                {
                    fileStream = File.Create(fileName);
                    fileStream.Close();
                }


                try
                {
                    using (StreamWriter sw = new StreamWriter(fileName, false, Encoding.Default))
                    {
                        sw.Write(ContentF);

                        sw.WriteLine();
                    }
                }
                catch (System.IO.IOException)
                {
                
                }
                catch (System.Exception)
                {
        
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
              
            }
        }

      
    
       
      
            public static byte[] StructureToByte<T>(T structure)
            {
                int size = Marshal.SizeOf(typeof(T));
                byte[] buffer = new byte[size];
                IntPtr bufferIntPtr = Marshal.AllocHGlobal(size);
                try
                {
                    Marshal.StructureToPtr(structure, bufferIntPtr, true);
                    Marshal.Copy(bufferIntPtr, buffer, 0, size);
                }
                finally
                {
                    Marshal.FreeHGlobal(bufferIntPtr);
                }
                return buffer;
            }

            public static T ByteToStructure<T>(byte[] dataBuffer)
            {
                object structure = null;
                int size = Marshal.SizeOf(typeof(T));
                IntPtr allocIntPtr = Marshal.AllocHGlobal(size);
                try
                {
                    Marshal.Copy(dataBuffer, 0, allocIntPtr, size);
                    structure = Marshal.PtrToStructure(allocIntPtr, typeof(T));
                }
                finally
                {
                    Marshal.FreeHGlobal(allocIntPtr);
                }
                return (T)structure;
            }
       
    }
}


        


    

