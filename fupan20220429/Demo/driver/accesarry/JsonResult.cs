using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Demo.driver.cam
{
    public struct MyDefect
    {
        public int _NGType { get; set; }
        public int _StartPointX { get; set; }
        public int _StartPointY { get; set; }
        public int _Width { get; set; }
        public int _Height { get; set; }
        public MyDefect(int NGType, int StartPointX, int StartPointY, int Width, int Height)
        {
            _NGType = NGType;
            _StartPointX = StartPointX;
            _StartPointY = StartPointY;
            _Width = Width;
            _Height = Height;
        }
        public MyDefect clone()
        {
            MyDefect t = new MyDefect();
            t._NGType = this._NGType;
            t._StartPointX = this._StartPointX;
            t._StartPointY = this._StartPointY;
            t._Width = this._Width;
            t._Height = this._Height;
            return t;
        }
    }
    public class JsonResult
    {
        public Dictionary<int, string> _DefectTypes { get; set; }
        public bool _DefectResult { get; set; }
        public int _DefectNum { get; set; }
        public List<MyDefect> _DefectLists { get; set; }
        public JsonResult(int DefectNum, bool DefectResult, List<MyDefect> DefectLists, Dictionary<int, string> DefectTypes)
        {
            _DefectNum = DefectNum;
            _DefectResult = DefectResult;
            _DefectTypes = DefectTypes;
            _DefectLists = new List<MyDefect>();
            foreach (MyDefect mydefect in DefectLists)
            {
                MyDefect temp = mydefect.clone();
                _DefectLists.Add(temp);
            }
        }

        public bool SaveResultJson(string jsonfile)
        {
            StreamWriter sw = new StreamWriter(jsonfile);
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            //构建Json.net的写入流
            JsonWriter writer = new JsonTextWriter(sw);
            //把模型数据序列化并写入Json.net的JsonWriter流中 
            writer.WriteStartObject();
            writer.WritePropertyName("_defeResult");
            string Result0 = _DefectResult ? "OK" : "NG";
            writer.WriteValue(Result0);   //检测结果
            writer.WritePropertyName("_checkResult");
            writer.WriteValue("NULL");   //复判结果
            writer.WritePropertyName("_defeNumber");
            writer.WriteValue(_DefectNum);   //缺陷个数
            writer.WritePropertyName("Labels");
            if (_DefectNum > 0)
            {
                writer.WriteStartArray();
                for (int i = 0; i < _DefectLists.Count(); i++)
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("_NGName");
                    string NGname = "";
                    if (_DefectTypes.ContainsKey(_DefectLists[i]._NGType))
                    {
                        _DefectTypes.TryGetValue(_DefectLists[i]._NGType, out NGname);
                    }
                    else
                    {
                        NGname = "Unknown";
                    }
                    writer.WriteValue(NGname);
                    writer.WritePropertyName("_StartPointX");
                    writer.WriteValue(_DefectLists[i]._StartPointX);
                    writer.WritePropertyName("_StartPointY");
                    writer.WriteValue(_DefectLists[i]._StartPointY);
                    writer.WritePropertyName("_Width");
                    writer.WriteValue(_DefectLists[i]._Width);
                    writer.WritePropertyName("_Height");
                    writer.WriteValue(_DefectLists[i]._Height);
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }
            else
            {
                writer.WriteValue("");
            }
            writer.WriteEndObject();
            writer.Close();
            sw.Close();
            return true;
        }

        public static void Writejson(string jsonfile, string jsonExpr)
        {
            using (StreamWriter sw = new StreamWriter(jsonfile))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    writer.WriteRaw(jsonExpr);
                }
            }

             //StreamWriter sw = new StreamWriter(jsonfile);
             //JsonWriter writer = new JsonTextWriter(sw);
             //writer.WriteStartObject();
             //writer.WriteValue(jsonExpr);
             //writer.WriteEndObject();
            //writer.Close();
            //sw.Close();
            //System.IO.File.WriteAllText(jsonfile, jsonExpr);

        }


        public static string Readjson(string jsonfile, string key)
        {

            using (System.IO.StreamReader file = System.IO.File.OpenText(jsonfile))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject o = (JObject)JToken.ReadFrom(reader);
                    var value = o[key].ToString();
                    return value;
                }
            }
        }

        public static string Readjson(string jsonfile)
        {

            using (System.IO.StreamReader file = System.IO.File.OpenText(jsonfile))
            {
                string jsonWordTemplate = file.ReadToEnd();
                return jsonWordTemplate;
            }
        }

        public static string Parsejson(string jsonST, string key)
        {

                    JObject o = (JObject)JToken.Parse(jsonST);
                    var value = o[key].ToString();
                    return value;

        }
    }
}
