using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Data;
using System.Net;
using System.Drawing.Imaging;


namespace Demo.driver.cam
{
    public class JsonToolsNet
    {
        // 从一个对象信息生成Json串
        public static string ObjectToJson(object obj)
        {
            
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            return Encoding.UTF8.GetString(dataBytes);
        }
        // 从一个Json串生成对象信息
        public static object JsonToObject(string jsonString, object obj)
        {
             string jsonArrayText="[{\"id\":1,\"name\":\"推荐菜式\",\"en_name\":\"Hit\",\"thumbnail\":\"http://img1.imgtn.bdimg.com/it/u=3969229294,3492115897&fm=26&gp=0.jpg\"},{\"id\":2,\"name\":\"海鲜类\",\"en_name\":\"Seafood\",\"thumbnail\":\"http://img3.imgtn.bdimg.com/it/u=2949365023,2449816104&fm=26&gp=0.jpg\"},{\"id\":3,\"name\":\"肉类\",\"en_name\":\"Meat\",\"thumbnail\":\"http://img2.imgtn.bdimg.com/it/u=1458900293,456042393&fm=200&gp=0.jpg\"},{\"id\":4,\"name\":\"蔬菜类\",\"en_name\":\"Vegetables\",\"thumbnail\":\"http://img1.imgtn.bdimg.com/it/u=3525638744,1814493687&fm=200&gp=0.jpg\"},{\"id\":5,\"name\":\"小吃类\",\"en_name\":\"Snack\",\"thumbnail\":\"http://img4.imgtn.bdimg.com/it/u=1165950757,1132142937&fm=26&gp=0.jpg\"},{\"id\":6,\"name\":\"饮料类\",\"en_name\":\"Drink\",\"thumbnail\":\"https://ss0.bdstatic.com/70cFuHSh_Q1YnxGkpoWK1HF6hhy/it/u=1051452898,1589162946&fm=26&gp=0.jpg\"}]";
            JArray jArray = (JArray)JsonConvert.DeserializeObject(jsonArrayText);
            var mJObj1 = JArray.Parse(jsonArrayText);

            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                return serializer.ReadObject(mStream);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static  List<T> ReceivFrJ<T>(string jsonString) where T : new()
        {
            try
            {
                T a =new T();
                           

                JArray jArray = (JArray)JsonConvert.DeserializeObject(jsonString);
          
                List<T> Temp_arr = new List<T>();

                if (jArray.Count > 0)
                {
                    for (int i = 0; i < jArray.Count; i++)
                    {
                        T p = JsonHelper.JsonDeserialize<T>(jArray[i].ToString()); 

            
                        
                        Temp_arr.Add(p);
                    }
                }

                return Temp_arr;
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        public static T ReceivObj<T>(string jsonString)
        {
            try
            {
                T jArray = (T)JsonConvert.DeserializeObject(jsonString, typeof(T));            
            
                return jArray;
            }
            catch (Exception ex)
            {
                return default(T);
            }

        }


        public static string GetJsonElement(string JsonStr, string  Ele)
        {
            JavaScriptSerializer Jss = new JavaScriptSerializer();
            Dictionary<string, object> DicText = (Dictionary<string, object>)Jss.DeserializeObject(JsonStr);
            if (!DicText.ContainsKey(Ele))
                return "";
            return DicText[Ele].ToString();
        }


        public static object GetJsonElement0(string JsonStr, string Ele)
        {
            JavaScriptSerializer Jss = new JavaScriptSerializer();
            Dictionary<string, object> DicText = (Dictionary<string, object>)Jss.DeserializeObject(JsonStr.TrimStart('[').TrimEnd(']'));
            if (!DicText.ContainsKey(Ele))
                return "";
            return DicText[Ele];
        }


     
        public class JsonHelper
        {
            /// <summary> 
            /// Json序列化 
            /// </summary> 
            public static string JsonSerializer<T>(T obj)
            {
                string jsonString = string.Empty;
                try
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

                    using (MemoryStream ms = new MemoryStream())
                    {
                        serializer.WriteObject(ms, obj);
                        jsonString = Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
                catch
                {
                    jsonString = string.Empty;
                }
                return jsonString;
            }

            /// <summary> 
            /// Json反序列化
            /// </summary> 
            public static T JsonDeserialize<T>(string jsonString)
            {
                T obj = Activator.CreateInstance<T>();
                try
                {
                    using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
                    {
                        DataContractJsonSerializer ser = new DataContractJsonSerializer(obj.GetType());//typeof(T)
                        T jsonObject = (T)ser.ReadObject(ms);
                        ms.Close();

                        return jsonObject;
                    }
                }
                catch
                {
                    return default(T);
                }
            }

            // 将 DataTable 序列化成 json 字符串
            public static string DataTableToJson(DataTable dt)
            {
                if (dt == null || dt.Rows.Count == 0)
                {
                    return "\"\"";
                }
                JavaScriptSerializer myJson = new JavaScriptSerializer();

                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                foreach (DataRow dr in dt.Rows)
                {
                    Dictionary<string, object> result = new Dictionary<string, object>();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        result.Add(dc.ColumnName, dr[dc].ToString());
                    }
                    list.Add(result);
                }
                return myJson.Serialize(list);
            }

            // 将对象序列化成 json 字符串
            public static string ObjectToJson(object obj)
            {
                if (obj == null)
                {
                    return string.Empty;
                }
                JavaScriptSerializer myJson = new JavaScriptSerializer();

                return myJson.Serialize(obj);
            }

            // 将 json 字符串反序列化成对象
            public static object JsonToObject(string json)
            {
                if (string.IsNullOrEmpty(json))
                {
                    return null;
                }
                JavaScriptSerializer myJson = new JavaScriptSerializer();

                return myJson.DeserializeObject(json);
            }

            // 将 json 字符串反序列化成对象
            public static T JsonToObject<T>(string json)
            {
                if (string.IsNullOrEmpty(json))
                {
                    return default(T);
                }
                JavaScriptSerializer myJson = new JavaScriptSerializer();

                return myJson.Deserialize<T>(json);
            }
        }
    }
}
