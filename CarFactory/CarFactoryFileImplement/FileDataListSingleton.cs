using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarFactoryFileImplement.Models;
using CarFactoryContracts.Enums;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CarFactoryFileImplement
{
    public class FileDataListSingleton
    {
        private static FileDataListSingleton instance;
        private readonly string ComponentFileName = "Component.xml";
        private readonly string OrderFileName = "Order.xml";
        private readonly string CarFileName = "Car.xml";
        private readonly string WarehouseFileName = "Warehouse.xml";
        public List<Component> Components { get; set; }
        public List<Order> Orders { get; set; }
        public List<Car> Cars { get; set; }
        public List<Warehouse> Warehouses { get; set; }

        private FileDataListSingleton()
        {
            Components = LoadComponents();
            Orders = LoadOrders();
            Cars = LoadCars();
            Warehouses = LoadWarehouses();
        }
        public static FileDataListSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new FileDataListSingleton();
            }
            return instance;
        }
        public void Save()
        {
            SaveOrders();
            SaveComponents();
            SaveCars();
            SaveWarehouses();
        }
        private List<Component> LoadComponents()
        {
            var list = new List<Component>();
            if (File.Exists(ComponentFileName))
            {
                var xDocument = XDocument.Load(ComponentFileName);
                var xElements = xDocument.Root.Elements("Component").ToList();
                foreach (var elem in xElements)
                {
                    list.Add(new Component
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        ComponentName = elem.Element("ComponentName").Value
                    });
                }
            }
            return list;
        }
        private List<Order> LoadOrders()
        {
            var list = new List<Order>();
            if (File.Exists(OrderFileName))
            {
                var xDocument = XDocument.Load(OrderFileName);
                var xElements = xDocument.Root.Elements("Order").ToList();
                OrderStatus status;
                DateTime? dateImplement;
                foreach (var elem in xElements)
                {
                    Enum.TryParse<OrderStatus>(elem.Element("Status").Value, out status);
                    dateImplement = null;
                    if (elem.Element("DateImplement").Value != "")
                    {
                        dateImplement = DateTime.Parse(elem.Element("DateImplement").Value);
                    }
                    list.Add(new Order
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        CarId = Convert.ToInt32(elem.Element("CarId").Value),
                        Count = Convert.ToInt32(elem.Element("Count").Value),
                        Sum = Convert.ToDecimal(elem.Element("Sum").Value),
                        Status = status,
                        DateCreate = DateTime.Parse(elem.Element("DateCreate").Value),
                        DateImplement = dateImplement
                    });
                }
            }
            return list;
        }
        private List<Car> LoadCars()
        {
            var list = new List<Car>();
            if (File.Exists(CarFileName))
            {
                var xDocument = XDocument.Load(CarFileName);
                var xElements = xDocument.Root.Elements("Car").ToList();
                foreach (var elem in xElements)
                {
                    var carComp = new Dictionary<int, int>();
                    foreach (var component in
                        elem.Element("CarComponents").Elements("CarComponent").ToList())
                    {
                        carComp.Add(Convert.ToInt32(component.Element("Key").Value),
                            Convert.ToInt32(component.Element("Value").Value));
                    }
                    list.Add(new Car
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        CarName = elem.Element("CarName").Value,
                        Price = Convert.ToDecimal(elem.Element("Price").Value),
                        CarComponents = carComp
                    });
                }
            }
            return list;
        }
        private List<Warehouse> LoadWarehouses()
        {
            var list = new List<Warehouse>();
            if (File.Exists(WarehouseFileName))
            {
                var xDocument = XDocument.Load(WarehouseFileName);
                var xElements = xDocument.Root.Elements("Warehouse").ToList();
                foreach (var elem in xElements)
                {
                    var warComp = new Dictionary<int, int>();
                    foreach (var component in
                        elem.Element("WarehouseComponents").Elements("WarehouseComponent").ToList())
                    {
                        warComp.Add(Convert.ToInt32(component.Element("Key").Value),
                            Convert.ToInt32(component.Element("Value").Value));
                    }
                    list.Add(new Warehouse
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        WarehouseName = elem.Element("WarehouseName").Value,
                        Responsible = elem.Element("Responsible").Value,
                        DateCreate = DateTime.Parse(elem.Element("DateCreate").Value),
                        WarehouseComponents = warComp
                    });
                }
            }
            return list;
        }
        private void SaveComponents()
        {
            if (Components != null)
            {
                var xElement = new XElement("Components");
                foreach (var component in Components)
                {
                    xElement.Add(new XElement("Component",
                        new XAttribute("Id", component.Id),
                        new XElement("ComponentName", component.ComponentName)));
                }
                var xDocument = new XDocument(xElement);
                xDocument.Save(ComponentFileName);
            }
        }
        private void SaveOrders()
        {
            if (Orders != null)
            {
                var xElement = new XElement("Orders");
                foreach (var order in Orders)
                {
                    xElement.Add(new XElement("Order",
                        new XAttribute("Id", order.Id),
                        new XElement("CarId", order.CarId),
                        new XElement("Count", order.Count),
                        new XElement("Sum", order.Sum),
                        new XElement("Status", order.Status),
                        new XElement("DateCreate", order.DateCreate),
                        new XElement("DateImplement", order.DateImplement)));
                }
                var xDocument = new XDocument(xElement);
                xDocument.Save(OrderFileName);
            }
        }
        private void SaveCars()
        {
            if (Cars != null)
            {
                var xElement = new XElement("Cars");
                foreach (var car in Cars)
                {
                    var compElement = new XElement("CarComponents");
                    foreach (var component in car.CarComponents)
                    {
                        compElement.Add(new XElement("CarComponent",
                            new XElement("Key", component.Key),
                            new XElement("Value", component.Value)));
                    }
                    xElement.Add(new XElement("Car",
                        new XAttribute("Id", car.Id),
                        new XElement("CarName", car.CarName),
                        new XElement("Price", car.Price),
                        compElement));
                }
                var xDocument = new XDocument(xElement);
                xDocument.Save(CarFileName);
            }
        }
        private void SaveWarehouses()
        {
            if (Warehouses != null)
            {
                var xElement = new XElement("Warehouses");
                foreach (var warehouse in Warehouses)
                {
                    var compElement = new XElement("WarehouseComponents");
                    foreach (var component in warehouse.WarehouseComponents)
                    {
                        compElement.Add(new XElement("WarehouseComponent",
                            new XElement("Key", component.Key),
                            new XElement("Value", component.Value)));
                    }
                    xElement.Add(new XElement("Warehouse",
                        new XAttribute("Id", warehouse.Id),
                        new XElement("WarehouseName", warehouse.WarehouseName),
                        new XElement("Responsible", warehouse.Responsible),
                        new XElement("DateCreate", warehouse.DateCreate),
                        compElement));
                }
                var xDocument = new XDocument(xElement);
                xDocument.Save(WarehouseFileName);
            }
        }
    }
}
