using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using Microsoft.Owin.Security.WsFederation;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Data;

namespace rpa_mazar.Common
{
    public static class Functions
    {
        public static void ADFSSignout()
        {
            if (HttpContext.Current.GetOwinContext().Authentication.User.Identity.IsAuthenticated == true)
            {
                HttpContext.Current.GetOwinContext().Authentication.SignOut(
                new AuthenticationProperties { RedirectUri = ConfigurationManager.AppSettings["ida:Wtrealm"] },
                WsFederationAuthenticationDefaults.AuthenticationType,
                CookieAuthenticationDefaults.AuthenticationType);
            }
        }
        public static DataTable RemoveEmptyRow(DataTable dt)
        {
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                if (dt.Rows[i][1] == DBNull.Value)
                {
                    dt.Rows[i].Delete();
                }
            }
            dt.AcceptChanges();
            return dt;
        }
        public static void EmailNotification(string subjectline, string htmlbody, string tomail,string ProcessFlag)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com",587);
                smtp.UseDefaultCredentials = false;
                //Code added to use diffrent from email address for CL and SO
                if (ProcessFlag == "CL")
                {

                    message.From = new System.Net.Mail.MailAddress("cl.bot@dalmiabharat.com");
                    smtp.Credentials = new NetworkCredential("cl.bot@dalmiabharat.com", "Dalmia@2022");

                }
                else if (ProcessFlag == "SO")
                {
                    message.From = new System.Net.Mail.MailAddress("rpa.admin2@dalmiabharat.com");
                    smtp.Credentials = new NetworkCredential("rpa.admin2@dalmiabharat.com", "Dalmia@2022");


                }
                else if (ProcessFlag == "CC")
                {
                    message.From = new System.Net.Mail.MailAddress("cc.bot@dalmiabharat.com");
                    smtp.Credentials = new NetworkCredential("cc.bot@dalmiabharat.com", "Dalmia@2022");

                }

                else if(ProcessFlag == "Freight")
                {
                    message.From = new System.Net.Mail.MailAddress("Logistics.bot@dalmiabharat.com");
                    smtp.Credentials = new NetworkCredential("Logistics.bot@dalmiabharat.com", "Dalmia@2022");
                }
                else if(ProcessFlag == "Route")
                {
                    message.From = new System.Net.Mail.MailAddress("Logistics.bot@dalmiabharat.com");
                    smtp.Credentials = new NetworkCredential("Logistics.bot@dalmiabharat.com", "Dalmia@2022");
                }
                //message.From = new MailAddress(ConfigurationManager.AppSettings["FromMail"].ToString());
                //message.To.Add(new MailAddress("eashwar.rao@mazars.in"));
                message.To.Add(new MailAddress(tomail));
                message.Subject = subjectline;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = htmlbody;
                smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["MailPort"]);
                smtp.Host = ConfigurationManager.AppSettings["MailHost"].ToString(); //for gmail host  
                smtp.EnableSsl = true;
                
                //code changed to accomodate different passwords for SO and CL email addresses

                //smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["FromMail"].ToString(), ConfigurationManager.AppSettings["MailPwd"].ToString());
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);

                using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                {
                    Logger.Log("Mail Sent Successfully to : " + tomail, w);
                }

            }
            catch (Exception ex)
            {
                using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                {
                    Logger.Log(ex.Message + " \n " + ex.StackTrace, w);
                }
            }
        }
    }

}