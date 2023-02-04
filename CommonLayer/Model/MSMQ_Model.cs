using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using Experimental.System.Messaging;

namespace CommonLayer.Model
{
    public class MSMQ_Model
    {
        MessageQueue message = new MessageQueue();
        public void sendData2Queue(string token)
        {
            message.Path = @".\private$\token";
            if (!(MessageQueue.Exists(message.Path)))
            {
                MessageQueue.Create(message.Path);
            }

            message.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            message.ReceiveCompleted += Message_ReceiveCompleted;
            message.Send(token);
            message.BeginReceive();
            message.Close();
        }

        public void Message_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            var msg = message.EndReceive(e.AsyncResult);
            string token = msg.Body.ToString();
            string Subject = "Fundoo Notes Reset Token";
            string Body = token;
            var SMTP = new SmtpClient("Smtp.gmail.com")
            {
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("shravanthi27041996@gmail.com", "pjkebdccuiyggikn"),
                EnableSsl = true

            };
            SMTP.Send("shravanthi27041996@gmail.com", "shravanthi27041996@gmail.com", Subject, Body);
            message.BeginReceive();
        }
    }
}
