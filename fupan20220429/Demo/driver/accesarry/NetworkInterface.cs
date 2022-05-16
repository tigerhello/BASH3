using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net.Http.Headers;

namespace Demo.driver.cam
{
    public class NetworkInterface
    {
        public  string url = @"http://127.0.0.1:5000/aoi";
        public  string urlcut = @"http://127.0.0.1:5000/aoi/cut";
        public  string urlpredict = @"http://127.0.0.1:5000/aoi/predict";
        public  string urlvisualize = @"http://127.0.0.1:5000/aoi/visualize";
        public  string url_new = @"http://192.168.1.128:5000/aoi";
        public string urlocr = @"http://127.0.0.1:5000/ocr/ocr";

        public  RestClient client = new RestClient();

        public  string HttpPostCheckCut(string station, byte[] vein)
        {
            try
            {
                client = new RestClient(urlcut);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");

                string featureStr = Convert.ToBase64String(vein);
                var values = new Dictionary<string, object>
                    {
                       {"station", station },
                        {"image", featureStr},
                     };


                string stringPayload = JsonConvert.SerializeObject(values);

                request.AddParameter("application/json", stringPayload, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                string resultStr = response.Content;
                return resultStr;
            }
            catch (Exception exception)
            {
                throw new Exception(vein.Length.ToString ()+"   " +exception.ToString());
            }
        }

        public string HttpPostCheckOcr(byte[] vein)
        {
            try
            {
                client.BaseUrl=urlocr;
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");

                string featureStr = Convert.ToBase64String(vein);
                var values = new Dictionary<string, object>
                    {
                
                        {"image", featureStr},
                     };


                string stringPayload = JsonConvert.SerializeObject(values);

                request.AddParameter("application/json", stringPayload, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                string resultStr = response.Content;
                return resultStr;
            }
            catch (Exception exception)
            {
                //System.Windows.Forms.MessageBox.Show(exception.ToString());
                return "";
                //throw new Exception(vein.Length.ToString() + "   " + exception.ToString());
            }
        }

        public  string HttpPostCheckPreDi(string station, string THresh, byte[] vein)
        {
            try
            {
                client = new RestClient(urlpredict);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");

                string featureStr = Convert.ToBase64String(vein);
                double th = Convert.ToDouble(THresh);
                //th = th <=0 ? 0.1 : th;

                var values = new Dictionary<string, object>
                    {
                        {"station", station },
                        {"threshold",th},
                        {"iou_threshold",0.3},
                        {"image", featureStr},
                     };


                string stringPayload = JsonConvert.SerializeObject(values);

                request.AddParameter("application/json", stringPayload, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                string resultStr = response.Content;
                return resultStr;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }



        public  string HttpPostCheckVis(object ResultVis, byte[] vein)
        {
            try
            {
                client = new RestClient(urlvisualize);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");

                string featureStr = Convert.ToBase64String(vein);
                var values = new Dictionary<string, object>
                    {
                        {"result", ResultVis },
                        {"image", featureStr},
                     };

                string stringPayload = JsonConvert.SerializeObject(values);

                request.AddParameter("application/json", stringPayload, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                string resultStr = response.Content;
                return resultStr;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

    }
}
