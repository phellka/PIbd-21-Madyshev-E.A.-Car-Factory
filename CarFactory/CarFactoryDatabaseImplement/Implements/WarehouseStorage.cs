using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarFactoryDatabaseImplement.Models;
using CarFactoryContracts.BindingModels;
using CarFactoryContracts.StoragesContracts;
using CarFactoryContracts.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CarFactoryDatabaseImplement.Implements
{
    public class WarehouseStorage : IWarehouseStorage
    {
        public List<WarehouseViewModel> GetFullList()
        {
            using var context = new CarFactoryDatabase();
            return context.Warehouses.Include(rec => rec.WarehouseComponents).ThenInclude(rec => rec.Component).ToList().Select(CreateModel).ToList();
        }
        public List<WarehouseViewModel> GetFilteredList(WarehouseBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new CarFactoryDatabase();
            return context.Warehouses.Include(rec => rec.WarehouseComponents).ThenInclude(rec => rec.Component).Where(rec => rec.WarehouseName.Contains(model.WarehouseName)).ToList().Select(CreateModel).ToList();
        }
        public WarehouseViewModel GetElement(WarehouseBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new CarFactoryDatabase();
            var warehouse = context.Warehouses.Include(rec => rec.WarehouseComponents).ThenInclude(rec => rec.Component).FirstOrDefault(rec => rec.WarehouseName == model.WarehouseName || rec.Id == model.Id);
            return warehouse != null ? CreateModel(warehouse) : null;
        }
        public void Insert(WarehouseBindingModel model)
        {
            using var context = new CarFactoryDatabase();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                Warehouse warehouse = new Warehouse()
                {
                    WarehouseName = model.WarehouseName,
                    Responsible = model.Responsible,
                    DateCreate = model.DateCreate
                };
                context.Warehouses.Add(warehouse);
                context.SaveChanges();
                CreateModel(model, warehouse, context);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        public void Update(WarehouseBindingModel model)
        {
            using var context = new CarFactoryDatabase();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var element = context.Warehouses.FirstOrDefault(rec => rec.Id == model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, element, context);
                context.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        public void Delete(WarehouseBindingModel model)
        {
            using var context = new CarFactoryDatabase();
            Warehouse element = context.Warehouses.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Warehouses.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
        private static Warehouse CreateModel(WarehouseBindingModel model, Warehouse warehouse, CarFactoryDatabase context)
        {
            warehouse.WarehouseName = model.WarehouseName;
            warehouse.Responsible = model.Responsible;
            warehouse.DateCreate = model.DateCreate;
            if (model.Id.HasValue)
            {
                var warehouseComponents = context.WarehouseComponents.Where(rec => rec.WarehouseId == model.Id.Value).ToList();
                context.WarehouseComponents.RemoveRange(warehouseComponents.Where(rec => !model.WarehouseComponents.ContainsKey(rec.ComponentId)).ToList());
                context.SaveChanges();
                foreach (var updateComponent in warehouseComponents)
                {
                    updateComponent.Count = model.WarehouseComponents[updateComponent.ComponentId].Item2;
                    model.WarehouseComponents.Remove(updateComponent.ComponentId);
                }
                context.SaveChanges();
            }
            foreach (var pc in model.WarehouseComponents)
            {
                context.WarehouseComponents.Add(new WarehouseComponent
                {
                    WarehouseId = warehouse.Id,
                    ComponentId = pc.Key,
                    Count = pc.Value.Item2
                });
                context.SaveChanges();
            }
            return warehouse;
        }
        private static WarehouseViewModel CreateModel(Warehouse warehouse)
        {
            return new WarehouseViewModel
            {
                Id = warehouse.Id,
                WarehouseName = warehouse.WarehouseName,
                Responsible = warehouse.Responsible,
                DateCreate = warehouse.DateCreate,
                WarehouseComponents = warehouse.WarehouseComponents.ToDictionary(recPC => recPC.ComponentId, recPC =>(recPC.Component?.ComponentName, recPC.Count))
            };
        }
        public bool CheckBalance(Dictionary<int, int> components)
        {
            Dictionary<int, int> stockComponents = new Dictionary<int, int>();
            using var context = new CarFactoryDatabase();
            foreach (var warehouse in context.Warehouses)
            {
                foreach (var comp in warehouse.WarehouseComponents)
                {
                    if (stockComponents.ContainsKey(comp.ComponentId))
                    {
                        stockComponents[comp.ComponentId] += comp.Count;
                    }
                    else
                    {
                        stockComponents.Add(comp.ComponentId, comp.Count);
                    }
                }
            }
            foreach (var comp in components)
            {
                if (!stockComponents.ContainsKey(comp.Key) || stockComponents[comp.Key] < comp.Value)
                {
                    return false;
                }
            }
            return true;
        }

        public bool WriteOffBalance(Dictionary<int, int> components)
        {
            using var context = new CarFactoryDatabase();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                Dictionary<int, int> remainComponents = components.ToDictionary(comp => comp.Key, comp => comp.Value);
                foreach (WarehouseComponent warehouseComponent in context.WarehouseComponents)
                {
                    if (remainComponents.ContainsKey(warehouseComponent.ComponentId))
                    {
                        int decommission = Math.Min(remainComponents[warehouseComponent.ComponentId], warehouseComponent.Count);
                        remainComponents[warehouseComponent.ComponentId] -= decommission;
                        warehouseComponent.Count -= decommission;
                        if (remainComponents[warehouseComponent.ComponentId] == 0)
                        {
                            remainComponents.Remove(warehouseComponent.ComponentId);
                        }
                    }
                }
                if (remainComponents.Count != 0)
                {
                    throw new Exception("На складах недостаточно компонентов");
                }
                context.SaveChanges();
                context.WarehouseComponents.RemoveRange(context.WarehouseComponents.Where(rec => rec.Count == 0).ToList());
                context.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            return true;
        }
    }
}
