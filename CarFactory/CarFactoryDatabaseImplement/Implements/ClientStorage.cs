using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarFactoryContracts.BindingModels;
using CarFactoryContracts.StoragesContracts;
using CarFactoryContracts.ViewModels;
using CarFactoryDatabaseImplement.Models;

namespace CarFactoryDatabaseImplement.Implements
{
    public class ClientStorage : IClientStorage
    {
        public List<ClientViewModel> GetFullList()
        {
            using var context = new CarFactoryDatabase();
            return context.Clients.Select(CreateModel).ToList();
        }
        public List<ClientViewModel> GetFilteredList(ClientBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new CarFactoryDatabase();
            return context.Clients.Where(rec => rec.Login == model.Login && rec.Password == model.Password).Select(CreateModel).ToList();
        }
        public ClientViewModel GetElement(ClientBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new CarFactoryDatabase();
            var client = context.Clients.FirstOrDefault(rec => rec.Login == model.Login || rec.Id == model.Id);
            return client != null ? CreateModel(client) : null;
        }
        public void Insert(ClientBindingModel model)
        {
            using var context = new CarFactoryDatabase();
            context.Clients.Add(CreateModel(model, new Client()));
            context.SaveChanges();
        }
        public void Update(ClientBindingModel model)
        {
            using var context = new CarFactoryDatabase();
            var client = context.Clients.FirstOrDefault(rec => rec.Id == model.Id);
            if (client == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, client);
            context.SaveChanges();
        }
        public void Delete(ClientBindingModel model)
        {
            using var context = new CarFactoryDatabase();
            Client client = context.Clients.FirstOrDefault(rec => rec.Id == model.Id);
            if (client != null)
            {
                context.Clients.Remove(client);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
        private static Client CreateModel(ClientBindingModel model, Client client)
        {
            client.ClientFCs = model.FCs;
            client.Login = model.Login;
            client.Password = model.Password;
            return client;
        }
        private static ClientViewModel CreateModel(Client client)
        {
            return new ClientViewModel
            {
                Id = client.Id,
                FCs = client.ClientFCs,
                Login = client.Login,
                Password = client.Password
            };
        }
    }
}
