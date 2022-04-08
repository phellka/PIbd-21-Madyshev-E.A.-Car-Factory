using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarFactoryContracts.BindingModels;
using CarFactoryContracts.BusinessLogicsContracts;
using CarFactoryContracts.StoragesContracts;
using CarFactoryContracts.ViewModels;
using CarFactoryContracts.Enums;

namespace CarFactoryBusinessLogic.BusinessLogics
{
    public class OrderLogic : IOrderLogic
    {
        private readonly IOrderStorage orderStorage;
        private readonly IWarehouseStorage warehouseStorage;
        private readonly ICarStorage carStorage;
        public OrderLogic(IOrderStorage orderStorage, IWarehouseStorage warehouseStorage, ICarStorage carStorage)
        {
            this.orderStorage = orderStorage;
            this.warehouseStorage = warehouseStorage;
            this.carStorage = carStorage;
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
            CarViewModel tempCar = carStorage.GetElement(new CarBindingModel
            { Id = tempOrder.CarId });
            try
            {
                if (!warehouseStorage.WriteOffBalance(tempCar.CarComponents.ToDictionary(car => car.Key, car => car.Value.Item2 * tempOrder.Count)))
                {
                    throw new Exception("На складах недостаточно компонентов");
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
        }    
    }
}
