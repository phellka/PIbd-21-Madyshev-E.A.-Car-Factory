using System;
using System.Collections.Generic;
using System.Text;
using CarFactoryContracts.BindingModels;
using CarFactoryContracts.BusinessLogicsContracts;
using CarFactoryContracts.StoragesContracts;
using CarFactoryContracts.ViewModels;
using CarFactoryContracts.Enums;
using CarFactoryBusinessLogic.MailWorker;

namespace CarFactoryBusinessLogic.BusinessLogics
{
    public class OrderLogic : IOrderLogic
    {
        private readonly IOrderStorage orderStorage;
        private readonly AbstractMailWorker mailWorker;
        private readonly IClientStorage clientStorage;
        public OrderLogic(IOrderStorage orderStorage, AbstractMailWorker mailWorker, IClientStorage clientStorage)
        {
            this.orderStorage = orderStorage;
            this.mailWorker = mailWorker;
            this.clientStorage = clientStorage;
        }
        public List<OrderViewModel> Read(OrderBindingModel model)
        {
            if (model == null)
            {
                return orderStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<OrderViewModel> { orderStorage.GetElement(model) };
            }
            return orderStorage.GetFilteredList(model);
        }
        public void CreateOrder(CreateOrderBindingModel model)
        {
            OrderBindingModel tempOrder = new OrderBindingModel { 
                CarId = model.CarId, Count = model.Count, Sum = model.Sum,
                Status = OrderStatus.Принят, DateCreate = DateTime.Now,
                ClientId = model.ClientId
            };
            orderStorage.Insert(tempOrder);
            mailWorker.MailSendAsync(new MailSendInfoBindingModel { 
                MailAddress = clientStorage.GetElement(new ClientBindingModel { Id = model.ClientId})?.Login,
                Subject = "Создан новый заказ",
                Text = $"Дата заказа: {DateTime.Now}, сумма заказа: {model.Sum}"
            });
        }
        public void TakeOrder(ChangeStatusBindingModel model)
        {
            OrderViewModel tempOrder = orderStorage.GetElement(new OrderBindingModel 
                { Id = model.OrderId });
            if (tempOrder == null)
            {
                throw new Exception("Не найден заказ");
            }
            if (tempOrder.Status != OrderStatus.Принят)
            {
                throw new Exception("Статус заказа отличен от \"Принят\"");
            }
            tempOrder.Status = OrderStatus.Выполняется;
            tempOrder.DateImplement = DateTime.Now;
            orderStorage.Update(new OrderBindingModel
            {
                Id = tempOrder.Id,
                CarId = tempOrder.CarId,
                Count = tempOrder.Count,
                Sum = tempOrder.Sum,
                DateCreate = tempOrder.DateCreate,
                DateImplement = tempOrder.DateImplement,
                Status = tempOrder.Status,
                ClientId = tempOrder.ClientId,
                ImplementerId = model.ImplementerId
            });
            mailWorker.MailSendAsync(new MailSendInfoBindingModel
            {
                MailAddress = clientStorage.GetElement(new ClientBindingModel { Id = tempOrder.ClientId })?.Login,
                Subject = $"Смена статуса заказа№ {tempOrder.Id}",
                Text = $"Статус изменен на: {OrderStatus.Выполняется}"
            });
        }
        public void FinishOrder(ChangeStatusBindingModel model)
        {
            OrderViewModel tempOrder = orderStorage.GetElement(new OrderBindingModel
            { Id = model.OrderId });
            if (tempOrder == null)
            {
                throw new Exception("Не найден заказ");
            }
            if (tempOrder.Status != OrderStatus.Выполняется)
            {
                throw new Exception("Статус заказа отличен от \"Выполняется\"");
            }
            tempOrder.Status = OrderStatus.Готов;
            orderStorage.Update(new OrderBindingModel
            {
                Id = tempOrder.Id,
                CarId = tempOrder.CarId,
                Count = tempOrder.Count,
                Sum = tempOrder.Sum,
                DateCreate = tempOrder.DateCreate,
                DateImplement = tempOrder.DateImplement,
                Status = tempOrder.Status,
                ClientId = tempOrder.ClientId,
                ImplementerId = model.ImplementerId
            });
            mailWorker.MailSendAsync(new MailSendInfoBindingModel
            {
                MailAddress = clientStorage.GetElement(new ClientBindingModel { Id = tempOrder.ClientId })?.Login,
                Subject = $"Смена статуса заказа№ {tempOrder.Id}",
                Text = $"Статус изменен на: {OrderStatus.Готов}"
            });
        }
        public void DeliveryOrder(ChangeStatusBindingModel model)
        {
            OrderViewModel tempOrder = orderStorage.GetElement(new OrderBindingModel
            { Id = model.OrderId });
            if (tempOrder == null)
            {
                throw new Exception("Не найден заказ");
            }
            if (tempOrder.Status != OrderStatus.Готов)
            {
                throw new Exception("Статус заказа отличен от \"Готов\"");
            }
            tempOrder.Status = OrderStatus.Выдан;
            orderStorage.Update(new OrderBindingModel
            {
                Id = tempOrder.Id,
                CarId = tempOrder.CarId,
                Count = tempOrder.Count,
                Sum = tempOrder.Sum,
                DateCreate = tempOrder.DateCreate,
                DateImplement = tempOrder.DateImplement,
                Status = tempOrder.Status,
                ClientId = tempOrder.ClientId,
                ImplementerId = tempOrder.ImplementerId
            });
            mailWorker.MailSendAsync(new MailSendInfoBindingModel
            {
                MailAddress = clientStorage.GetElement(new ClientBindingModel { Id = tempOrder.ClientId })?.Login,
                Subject = $"Смена статуса заказа№ {tempOrder.Id}",
                Text = $"Статус изменен на: {OrderStatus.Выдан}"
            });
        }    
    }
}
