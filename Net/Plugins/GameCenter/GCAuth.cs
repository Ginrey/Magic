using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Xml;

namespace Magic.Net.Plugins.GameCenter
{
    public static class GCAuth
    {
        private const string ProjectID = "61";
        private const string SubProjectId = "0";
        private const string ShardId = "0";
        private const string UserId = "1";
        private const string UserId2 = "2";

        public static GCAuthData Login(string email, string password)
        {
            if (email.IndexOf("@mail.ru") != -1) return Login_MailRU(email, password);
            if (email.IndexOf("@inbox.ru") != -1) return Login_MailRU(email, password);
            if (email.IndexOf("@list.ru") != -1) return Login_MailRU(email, password);
            if (email.IndexOf("@bk.ru") != -1) return Login_MailRU(email, password);
            if (email.IndexOf("@mail.ua") != -1) return Login_MailRU(email, password);

            try
            {
                var rData = new GCAuthData();
                var wc = new GCWebClient();

                var post = XML("Auth", email, password);
                var response = wc.UploadString("https://authdl.mail.ru/sz.php?hint=Auth", post);
                var data = response.ToString();

                var d = new XmlDocument();
                d.LoadXml(data);
                rData.BaseAccount = d["Auth"].Attributes["PersId"].Value;

                var post2 = XML("AutoLogin", email, password);
                var response2 = wc.UploadString("https://authdl.mail.ru/sz.php?hint=AutoLogin", post2);
                var data2 = response2.ToString();

                d.LoadXml(data2);
                rData.Token = d["AutoLogin"].Attributes["Key"].Value;

                var post3 = XML("PersList", email, password);
                var response3 = wc.UploadString("https://authdl.mail.ru/sz.php?hint=PersList", post3);
                var data3 = response3.ToString();

                d.LoadXml(data3);
                var accounts = d["PersList"];

                var count = accounts.ChildNodes.Count;

                rData.Accounts = new PWAccount[count];
                var i = 0;
                foreach (XmlNode account in accounts)
                {
                    rData.Accounts[i] = new PWAccount();
                    rData.Accounts[i].Title = account.Attributes["Title"].Value;
                    rData.Accounts[i].UserId = account.Attributes["Id"].Value;
                    i++;
                }

                return rData;
            }
            catch
            {
                return null;
            }
        }

        private static string JsonRead(this string line, string key)
        {
            var index1 = line.IndexOf(key);
            if (index1 == -1)
            {
                return string.Empty;
            }
            index1 += key.Length + 3;
            var index2 = line.IndexOf('\"', index1);

            if(index2 == -1)
            {
                return string.Empty;
            }

            return line.Substring(index1, index2 - index1);
        }

          static WebHeaderCollection GET(string Url)
        {
           WebRequest req = WebRequest.Create(Url);
           WebResponse resp = req.GetResponse();
           return resp.Headers;
        }
        public static GCAuthData Login_MailRU(string email, string password)
        {
            try
            {
                var rData = new GCAuthData();
                var wc = new GCWebClient();

                var response = wc.UploadString("https://o2.mail.ru/token", new Dictionary<string, string>
                {
                    {"client_id", "gamecenter.mail.ru"},
                    {"grant_type", "password"},
                    {"username", email},
                    {"password", password},
                });

                var str = response.ToString();
                var token = str.JsonRead("access_token").ToString();

                var response4 = wc.UploadString("https://authdl.mail.ru/ec.php?hint=MrPage2", string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?><MrPage2 SessionKey=\"{0}\" Page=\"http://dl.mail.ru/robots.txt\"/>", token));
                str = response4.ToString();
                
                var d = new XmlDocument();
                d.LoadXml(str);
                var location = d["MrPage2"].Attributes["Location"].Value;
                var response1 = wc.UploadString(location);
                var headers = wc.GetResponseHeaders();
                
                var cookies = headers.Get("Set-Cookie").Split(';',',');
                var cookie = string.Empty;
                foreach(var cook in cookies)
                {
                    if (cook.StartsWith("Mpop"))
                    {
                        cookie = cook.Substring(5);
                        break;
                    }
                }
                
                wc.UploadString("https://authdl.mail.ru/ec.php?hint=Auth", string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?><Auth Cookie=\"{0}\" ChannelId=\"0\"/>", cookie));
                
                var r2 = wc.UploadString("https://authdl.mail.ru/sz.php?hint=AutoLogin", XML_MPOP("AutoLogin", cookie));
                var data2 = r2.ToString();

                d.LoadXml(data2);
                rData.Token = d["AutoLogin"].Attributes["Key"].Value;
                rData.BaseAccount = d["AutoLogin"].Attributes["PersId"].Value;

                var post3 = XML_MPOP("PersList", cookie);
                var r3 = wc.UploadString("https://authdl.mail.ru/sz.php?hint=PersList", post3);
                var data3 = r3.ToString();
                
                d.LoadXml(data3);
                var accounts = d["PersList"];

                var count = accounts.ChildNodes.Count;

                rData.Accounts = new PWAccount[count];
                var i = 0;
                foreach (XmlNode account in accounts)
                {
                    rData.Accounts[i] = new PWAccount();
                    rData.Accounts[i].Title = account.Attributes["Title"].Value;
                    rData.Accounts[i].UserId = account.Attributes["Id"].Value;
                    i++;
                }

                return rData;
            }
            catch
            {
                return null;
            }
        }

        private static string XML(string name, string username, string password)
        {
            return
              string.Format(
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?><{0} ProjectId=\"{1}\" SubProjectId=\"{2}\" ShardId=\"{3}\" UserId=\"{4}\" UserId2=\"{5}\" Username=\"{6}\" Password=\"{7}\"/>",
                name,
                ProjectID,
                SubProjectId,
                ShardId,
                UserId,
                UserId2, username, password);
        }

        private static string XML_MPOP(string name, string MPop)
        {
            return
              string.Format(
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?><{0} ProjectId=\"{1}\" SubProjectId=\"{2}\" ShardId=\"{3}\" UserId=\"{4}\" UserId2=\"{5}\" Mpop=\"{6}\"/>",
                name,
                ProjectID,
                SubProjectId,
                ShardId,
                UserId,
                UserId2, MPop);
        }

    }
}
