using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Runtime.InteropServices;

namespace Demo.utilities
{
    class Properties
    {
        public static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static object locker = new object();

        private Dictionary<String, String> list;
        private String filename;

        public Properties(String file)
        {
            reload(file);
        }
        //获得ip名称，以名字来做键
        public String get(String field, String defValue)
        {
            return (get(field) == null) ? (defValue) : (get(field));
        }
        public String get(String field)
        {
            return (list.ContainsKey(field)) ? (list[field]) : (null);
        }

        public void set(String field, Object value)
        {
            if (!list.ContainsKey(field))
            {
                if (value != null)
                {
                    list.Add(field, value.ToString());
                }
            }
            else
            {
                if (value != null)
                {
                    list[field] = value.ToString();
                }
            }

        }

        public void Save()
        {
            try
            {
                Save(this.filename);
            }
            catch (Exception e) {
                log.Error("save file error");
            }
        }

        public void Save(String filename)
        {
            this.filename = filename;

            lock (locker)
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
                foreach (String prop in list.Keys.ToArray())
                {
                    if (!String.IsNullOrWhiteSpace(list[prop]))
                    {
                        string strValue = list[prop];
                        strValue = strValue.Replace("\n", "mk$10$");
                        file.WriteLine(prop + "=" + strValue);
                    }

                }

                file.Close();
            }
                
        }

        public void reload()
        {
            reload(this.filename);
        }

        public void reload(String filename)
        {
            this.filename = filename;
            list = new Dictionary<String, String>();

            if (System.IO.File.Exists(filename))
                loadFromFile(filename);
            else
            {
                System.IO.FileStream file = System.IO.File.Create(filename);
                file.Close();
            }
        }
        //加载窗体文件
        private void loadFromFile(String file)
        {
            lock (locker)
            {
                foreach (String line in System.IO.File.ReadAllLines(file))
                {
                    if ((!String.IsNullOrEmpty(line)) &&
                        (!line.StartsWith(";")) &&
                        (!line.StartsWith("#")) &&
                        (!line.StartsWith("'")) &&
                        (line.Contains('=')))
                    {
                        int index = line.IndexOf('=');
                        String key = line.Substring(0, index).Trim();
                        String value = line.Substring(index + 1).Trim();

                        if ((value.StartsWith("\"") && value.EndsWith("\"")) ||
                            (value.StartsWith("'") && value.EndsWith("'")))
                        {
                            value = value.Substring(1, value.Length - 2);
                        }

                        try
                        {
                            value = value.Replace("mk$10$", "\n");
                            list.Add(key, value);
                       
                        }
                        catch { }
                    }
                }
            }

           
        }

        //gc
            [DllImport("kernel32.dll")]

            public static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max);
            /// <summary>
                    /// 释放内存
                    /// </summary>
            public static void Flush()
            {

                GC.Collect();

                GC.WaitForPendingFinalizers();

                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                }

            }
 
 
    }
}
