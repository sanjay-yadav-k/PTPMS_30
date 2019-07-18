using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Datalayer.BusinessLogic
{
   public static class sendmail
    {
        public static string sendmailtostudent(string tomail, string from,string Date,string body)
        {
            try
            {
                using (MailMessage mm = new MailMessage(tomail, from))
                {
                    mm.Subject = "Start classas soon as";
                    mm.Body = body;
                    mm.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("karmickmail4test@gmail.com", "karmick@123");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                }
                return "true";
            }
            catch(Exception ex)
            {
                return "false";
            }
          
        }
        public static string sensms(string tomail, string from, string Dat)
        {
            string URL = "https://smsapi.engineeringtgr.com/send/?Mobile=9800422946&Password=16121991&Message=text&To=8240408108&Key=skysa0yDq8TnZ3uMWRFztcNw";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.ContentLength = 0;// DATA.Length;

            try
            {
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();
                Console.Out.WriteLine(response);
                responseReader.Close();
            }
            catch (Exception xe)
            {
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(xe.Message);
            }
            return null;
        }
    }
    
}
