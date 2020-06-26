using Middleware.Models;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json;
using System;

namespace Middleware.Services.DecryptService
{
    public class RequestHttp
    {
        public void sendJson(DecryptMsg message)
        {
            WebRequest request = WebRequest.Create("http://192.168.20.10:8080/webservice/resources/cipher");
            request.Method = "POST";
            request.ContentType = "application/json";

            string json = JsonConvert.SerializeObject(message, Formatting.Indented);
            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                Console.WriteLine(responseFromServer);
            }
            response.Close();
        }
    }
}
