using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Magic.Net.Plugins.GameCenter
{
    public class GCWebClient : WebClient
    {
        public string UserAgent { get; private set; }
        private WebClient wc = new WebClient();
        public CookieContainer CookieContainer { get; set; }
      /*  protected override WebRequest GetWebRequest(Uri address)
        {

            WebRequest request = base.GetWebRequest(address);
            
            if (request is HttpWebRequest)
            {
                (request as HttpWebRequest).CookieContainer = this.CookieContainer;
            }
            HttpWebRequest httpRequest = (HttpWebRequest)request;
            httpRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            return httpRequest;
        }*/

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)request;
            httpRequest.AllowAutoRedirect = false;
            return httpRequest.GetResponse();
        }
        public GCWebClient() : this("Downloader/12200")
        {
          
        }
        public GCWebClient(string userAgent)
        {
            UserAgent = userAgent;
            ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
        }

        public WebHeaderCollection GetResponseHeaders()
        {
            return ResponseHeaders;
        }
        private void ResetHeaders()
        {
            Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            Headers[HttpRequestHeader.UserAgent] = UserAgent;
            Headers[HttpRequestHeader.Accept] = "*/*";
        }

        public string UploadString(string address)
        {
            ResetHeaders();
            return DownloadString(GetUri(address));
        }

        /*public WebHeaderCollection GetResponseHeaders()
        {
          return wc.ResponseHeaders;
        }*/

        public string UploadString(string address, string data)
        {
            ResetHeaders();
            return UploadString(GetUri(address), data);
        }

        public string UploadString(string address, Dictionary<string, string> data)
        {
            ResetHeaders();

            var sb = new StringBuilder();

            var p = new List<string>();


            foreach (KeyValuePair<string, string> pair in data)
            {
                sb.Clear();
                sb.Append(pair.Key).Append("=").Append(pair.Value);
                p.Add(sb.ToString());
            }

            var pp = string.Join("&", p);

            return UploadString(address, pp);
        }

        private static Uri GetUri(string str)
        {
            var u = new Uri(str);
            var servicePoint = ServicePointManager.FindServicePoint(u);
            servicePoint.Expect100Continue = false;
            return u;
        }
    }
}
