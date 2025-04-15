﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;


namespace YT1.Models.Common
{
    public class SendMail
    {
        private static string password = ConfigurationManager.AppSettings["PasswordEmail"];
        private static string Email = ConfigurationManager.AppSettings["Email"];

        public static bool SendMailClient(string name, string subject, string content, string toMail)
        {
            bool rs = false;
            try
            {
                MailMessage message = new MailMessage();
                var smtp = new System.Net.Mail.SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    //smtp.Credentials = new NetworkCredential(Email, password);
                    //smtp.Timeout = 15000;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential()
                    {
                        UserName = Email,
                        Password = password,
                    };

                }
                MailAddress fromAddress = new MailAddress(Email, name);
                message.From = fromAddress;
                message.To.Add(toMail);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = content;
                smtp.Send(message);
                rs = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("SendMail Error: " + ex.Message);
                rs = false;
            }
            return rs;
        }
    }
}