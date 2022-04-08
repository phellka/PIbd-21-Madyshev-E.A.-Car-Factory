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
    public class OrderStorage : IOrderStorage
    {
        private readonly DataListSingleton source;
        public OrderStorage()
        {
            source = DataListSingleton.GetInstance();
        }
        public List<OrderViewModel> GetFullList()
        {
            var result = new List<OrderViewModel>();
            foreach(var order in source.Orders)
            {
                result.Add(CreateModel(order));
            }
            return result;
        }
        public List<OrderViewModel> GetFilteredList(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            var result = new List<OrderViewModel>();
            foreach(var order in source.Orders)
            {
                if (order.CarId == model.CarId || (model.DateFrom.HasValue && model.DateTo.HasValue && 
                    order.DateCreate >= model.DateFrom && order.DateCreate <= model.DateTo)
                    || (model.ClientId.HasValue && order.ClientId == model.ClientId.Value)
                    || (model.SearchStatus.HasValue && model.SearchStatus.Value == order.Status)
                    || (model.ImplementerId.HasValue && order.ImplementerId == model.ImplementerId && model.Status == order.Status))
                {
                    result.Add(CreateModel(order));
                }
            }
            return result;
        }
        public OrderViewModel GetElement(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            foreach (var order in source.Orders)
            {
                if (model.Id == order.Id)
                {
                    return CreateModel(order);
                }
            }
            return null;
        }
        public void Insert(OrderBindingModel model)
        {
            var tempOrder = new Order { Id = 1 };
            foreach (var order in source.Orders)
            {
                if (order.Id >= tempOrder.Id)
                {
                    tempOrder.Id = order.Id + 1;
                }
            }
            source.Orders.Add(CreateModel(model, tempOrder));
        }
        public void Update(OrderBindingModel model)
        {
            Order tempOrder = null;
            foreach (var order in source.Orders)
            {
                if (model.Id == order.Id)
                {
                    tempOrder = order;
                }
            }
            if (tempOrder == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, tempOrder);
        }
        public void Delete(OrderBindingModel model)
        {
            for (int i = 0; i < source.Orders.Count; ++i)
            {
                if (source.Orders[i].Id == model.Id)
                {
                    source.Orders.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
        public static Order CreateModel(OrderBindingModel model,
            Order order)
        {
            order.CarId = model.CarId;
            order.Count = model.Count;
            order.Sum = model.Sum;
            order.Status = model.Status;
            order.DateCreate = model.DateCreate;
            order.DateImplement = model.DateImplement;
            order.ClientId = model.ClientId.Value;
            order.ImplementerId = model.ImplementerId;
            return order;
        }
        public OrderViewModel CreateModel(Order order)
        {
            string carName = string.Empty;
            foreach(var car in source.Cars)
            {
                if(car.Id == order.CarId)
                {
                    carName = car.CarName;
                    break;
                }
            }
            string clientFCs = string.Empty;
            foreach(var client in source.Clients)
            {
                if(client.Id == order.ClientId)
                {
                    clientFCs = client.FCs;
                    break;
                }
            }
            string implementerFCs = string.Empty;
            foreach(var implementer in source.Implementers)
            {
                if (implementer.Id == order.ImplementerId)
                {
                    implementerFCs = implementer.ImplementerFCs;
                    break;
                }
            }
            return new OrderViewModel { 
                Id = order.Id,
                CarId = order.CarId,
                CarName = carName,
                Count = order.Count,
                Sum = order.Sum,
                Status = order.Status,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement,
                ClientId = order.ClientId,
                ClientFCs = clientFCs,
                ImplementerId = order.ImplementerId,
                ImplementerFCs = implementerFCs
            };
        }
    }
}
