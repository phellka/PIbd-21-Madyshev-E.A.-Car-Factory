using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarFactoryContracts.BindingModels;
using CarFactoryContracts.StoragesContracts;
using CarFactoryContracts.ViewModels;
using CarFactoryDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace CarFactoryDatabaseImplement.Implements
{
    public class OrderStorage : IOrderStorage
    {
        public List<OrderViewModel> GetFullList()
        {
            using var context = new CarFactoryDatabase();
            return context.Orders.Include(rec => rec.Car).Select(rec => new OrderViewModel {
                Id = rec.Id,
                CarId = rec.CarId,
                CarName = rec.Car.CarName,
                Count = rec.Count,
                Sum = rec.Sum,
                Status = rec.Status,
                DateCreate = rec.DateCreate,
                DateImplement = rec.DateImplement,
                ClientId = rec.ClientId,
                ClientFCs = rec.Client.ClientFCs
            }).ToList();
        }
        public List<OrderViewModel> GetFilteredList(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new CarFactoryDatabase();
            return context.Orders.Include(rec => rec.Car).Include(rec => rec.Client).Where(rec => rec.CarId == model.CarId ||
                (model.DateFrom.HasValue && model.DateTo.HasValue &&  rec.DateCreate >= model.DateFrom && rec.DateCreate <= model.DateTo) ||
                model.ClientId.HasValue && rec.ClientId == model.ClientId).Select(rec => new OrderViewModel
            {
                Id = rec.Id,
                CarId = rec.CarId,
                CarName = rec.Car.CarName,
                Count = rec.Count,
                Sum = rec.Sum,
                Status = rec.Status,
                DateCreate = rec.DateCreate,
                DateImplement = rec.DateImplement,
                ClientId = rec.ClientId,
                ClientFCs = rec.Client.ClientFCs
            }).ToList();
        }
        public OrderViewModel GetElement(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new CarFactoryDatabase();
            var order = context.Orders.FirstOrDefault(rec => rec.Id == model.Id);
            return order != null ? CreateModel(order, context) : null;
        }
        public void Insert(OrderBindingModel model)
        {
            using var context = new CarFactoryDatabase();
            context.Orders.Add(CreateModel(model, new Order()));
            context.SaveChanges();
        }
        public void Update(OrderBindingModel model)
        {
            using var context = new CarFactoryDatabase();
            var element = context.Orders.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, element);
            context.SaveChanges();
        }
        public void Delete(OrderBindingModel model)
        {
            using var context = new CarFactoryDatabase();
            Order element = context.Orders.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Orders.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
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
            return order;
        }
        public OrderViewModel CreateModel(Order order, CarFactoryDatabase context)
        {
            return new OrderViewModel
            {
                Id = order.Id,
                CarId = order.CarId,
                CarName = context.Cars.FirstOrDefault(rec => rec.Id == order.CarId)?.CarName,
                Count = order.Count,
                Sum = order.Sum,
                Status = order.Status,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement,
                ClientId = order.ClientId,
                ClientFCs = order.Client.ClientFCs
            };
        }
    }
}
