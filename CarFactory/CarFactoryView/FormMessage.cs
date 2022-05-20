using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CarFactoryBusinessLogic.MailWorker;
using CarFactoryContracts.BindingModels;
using CarFactoryContracts.BusinessLogicsContracts;
using CarFactoryContracts.ViewModels;

namespace CarFactoryView
{
    public partial class FormMessage : Form
    {
        public string MessageId
        {
            set { messageId = value; }
        }
        private readonly IMessageInfoLogic messageLogic;

        private readonly IClientLogic clientLogic;

        private readonly AbstractMailWorker mailWorker;
        private string messageId;
        public FormMessage(IMessageInfoLogic messageLogic, IClientLogic clientLogic, AbstractMailWorker mailWorker)
        {
            InitializeComponent();
            this.messageLogic = messageLogic;
            this.clientLogic = clientLogic;
            this.mailWorker = mailWorker;
        }

        private void buttonReply_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxReplyText.Text))
            {
                MessageBox.Show("Введите текст ответа", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                mailWorker.MailSendAsync(new MailSendInfoBindingModel
                {
                    MailAddress = labelSender.Text,
                    Subject = "Re: " + labelSubject.Text,
                    Text = textBoxReplyText.Text
                });

                messageLogic.CreateOrUpdate(new MessageInfoBindingModel
                {
                    ClientId = clientLogic.Read(new ClientBindingModel { Login = labelSender.Text })?[0].Id,
                    MessageId = messageId,
                    FromMailAddress = labelSender.Text,
                    Subject = labelSubject.Text,
                    Body = labelBody.Text,
                    DateDelivery = DateTime.Parse(labelDateDelivery.Text),
                    Viewed = true,
                    ReplyText = textBoxReplyText.Text
                });
                MessageBox.Show("Ответ отправлен", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormMessage_Load(object sender, EventArgs e)
        {
            if (messageId != null)
            {
                try
                {
                    MessageInfoViewModel view = messageLogic.Read(new MessageInfoBindingModel { MessageId = messageId })?[0];
                    if (view != null)
                    {
                        if (!view.Viewed)
                        {
                            messageLogic.CreateOrUpdate(new MessageInfoBindingModel
                            {
                                ClientId = clientLogic.Read(new ClientBindingModel { Login = view.SenderName })?[0].Id,
                                MessageId = messageId,
                                FromMailAddress = view.SenderName,
                                Subject = view.Subject,
                                Body = view.Body,
                                DateDelivery = view.DateDelivery,
                                Viewed = true,
                                ReplyText = view.ReplyText
                            });
                        }
                        labelBody.Text = view.Body;
                        labelSender.Text = view.SenderName;
                        labelSubject.Text = view.Subject;
                        labelDateDelivery.Text = view.DateDelivery.ToString();
                        textBoxReplyText.Text = view.ReplyText;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
