using CarFactoryContracts.BindingModels;
using CarFactoryContracts.BusinessLogicsContracts;
using MailKit.Net.Pop3;
using MailKit.Security;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CarFactoryBusinessLogic.MailWorker
{
    public class MailKitWorker : AbstractMailWorker
    {
        public MailKitWorker(IMessageInfoLogic messageInfoLogic) : base(messageInfoLogic) { }
        protected override async Task SendMailAsync(MailSendInfoBindingModel info)
        {
            using var objMailMessage = new MailMessage();
            using var objSmtpClient = new SmtpClient(smtpClientHost, smtpClientPort);
            try
            {
                objMailMessage.From = new MailAddress(mailLogin);
                objMailMessage.To.Add(new MailAddress(info.MailAddress));
                objMailMessage.Subject = info.Subject;
                objMailMessage.Body = info.Text;
                objMailMessage.SubjectEncoding = Encoding.UTF8;
                objMailMessage.BodyEncoding = Encoding.UTF8;
                objSmtpClient.UseDefaultCredentials = false;
                objSmtpClient.EnableSsl = true;
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                objSmtpClient.Credentials = new NetworkCredential(mailLogin, mailPassword);
                await Task.Run(() => objSmtpClient.Send(objMailMessage));
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected override async Task<List<MessageInfoBindingModel>> ReceiveMailAsync()
        {
            var list = new List<MessageInfoBindingModel>();
            using var client = new Pop3Client();
            await Task.Run(() =>
            {
                try
                {
                    client.Connect(popHost, popPort, SecureSocketOptions.SslOnConnect);
                    client.Authenticate(mailLogin, mailPassword);
                    for (int i = 0; i < client.Count; i++)
                    {
                        var message = client.GetMessage(i);
                        foreach (var mail in message.From.Mailboxes)
                        {
                            list.Add(new MessageInfoBindingModel
                            {
                                DateDelivery = message.Date.DateTime,
                                MessageId = message.MessageId,
                                FromMailAddress = mail.Address,
                                Subject = message.Subject,
                                Body = message.TextBody,
                                Viewed = false
                            });
                        }
                    }
                }
                finally
                {
                    client.Disconnect(true);
                }
            });
            return list;
        }
    }
}
