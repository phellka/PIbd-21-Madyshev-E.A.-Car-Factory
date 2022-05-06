using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarFactoryContracts.StoragesContracts;
using CarFactoryContracts.ViewModels;
using CarFactoryContracts.BindingModels;
using CarFactoryListImplement.Models;

namespace CarFactoryListImplement.Implemets
{
    public class MessageInfoStorage : IMessageInfoStorage
    {

        private readonly DataListSingleton source;
        public MessageInfoStorage()
        {
            source = DataListSingleton.GetInstance();
        }
        public List<MessageInfoViewModel> GetFullList()
        {
            var result = new List<MessageInfoViewModel>();
            foreach (var message in source.Messages)
            {
                result.Add(CreateModel(message));
            }
            return result;
        }
        public List<MessageInfoViewModel> GetFilteredList(MessageInfoBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            int toSkip = model.ToSkip ?? 0;
            int toTake = model.ToTake ?? source.Messages.Count;
            var result = new List<MessageInfoViewModel>();
            if (model.ToSkip.HasValue && model.ToTake.HasValue && !model.ClientId.HasValue)
            {
                foreach (var msg in source.Messages)
                {
                    if (toSkip > 0) { toSkip--; continue; }
                    if (toTake > 0)
                    {
                        result.Add(CreateModel(msg));
                        toTake--;
                    }
                }
                return result;
            }
            foreach (var message in source.Messages)
            {
                if ((model.ClientId.HasValue && message.ClientId == model.ClientId) ||
                    (!model.ClientId.HasValue && message.DateDelivery.Date == model.DateDelivery.Date))
                {
                    if (toSkip > 0) { toSkip--; continue; }
                    if (toTake > 0)
                    {
                        result.Add(CreateModel(message));
                        toTake--;
                    }
                }
            }
            return result;
        }
        public MessageInfoViewModel GetElement(MessageInfoBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            foreach (var message in source.Messages)
            {
                if (message.MessageId == model.MessageId)
                {
                    return CreateModel(message);
                }
            }
            return null;
        }
        public void Insert(MessageInfoBindingModel model)
        {
            source.Messages.Add(CreateModel(model, new MessageInfo()));
        }
        public void Update(MessageInfoBindingModel model)
        {
            MessageInfo tempMessage = null;
            foreach (var message in source.Messages)
            {
                if (message.MessageId == model.MessageId)
                {
                    tempMessage = message;
                    break;
                }
            }
            if (tempMessage == null)
            {
                throw new Exception("Element is not found");
            }
            CreateModel(model, tempMessage);
        }
        private MessageInfo CreateModel(MessageInfoBindingModel model,
            MessageInfo message)
        {
            string clientName = string.Empty;
            foreach(var client in source.Clients)
            {
                if (client.Id == model.ClientId)
                {
                    clientName = client.FCs;
                    break;
                }
            }
            message.MessageId = model.MessageId;
            message.SenderName = clientName;
            message.Body = model.Body;
            message.ClientId = model.ClientId;
            message.DateDelivery = model.DateDelivery;
            message.Subject = model.Subject;
            message.ReplyText = model.ReplyText;
            message.Viewed = model.Viewed;
            return message;
        }
        private MessageInfoViewModel CreateModel(MessageInfo message)
        {
            return new MessageInfoViewModel
            {
                MessageId = message.MessageId,
                Body = message.Body,
                DateDelivery = message.DateDelivery,
                SenderName = message.SenderName,
                Subject = message.Subject,
                Viewed = message.Viewed,
                ReplyText = message.ReplyText
            };
        }
    }
}
