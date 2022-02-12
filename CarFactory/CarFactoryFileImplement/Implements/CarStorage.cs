using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarFactoryContracts.BindingModels;
using CarFactoryContracts.StoragesContracts;
using CarFactoryContracts.ViewModels;
using CarFactoryFileImplement.Models;
    
namespace CarFactoryFileImplement.Implements
{
    public class CarStorage : ICarStorage
    {
        private readonly FileDataListSingleton source;
        public CarStorage()
        {
            source = FileDataListSingleton.GetInstance();
        }
        public List<CarViewModel> GetFullList()
        {
            return source.Cars.Select(CreateModel).ToList();
        }
        public List<CarViewModel> GetFilteredList(CarBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return source.Cars.Where(rec => rec.CarName.Contains(model.CarName)).Select(CreateModel).ToList();
        }
        public CarViewModel GetElement(CarBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            var car = source.Cars.FirstOrDefault(rec => rec.CarName == model.CarName || 
                rec.Id == model.Id);
            return car != null ? CreateModel(car) : null;
        }
        public void Insert(CarBindingModel model)
        {
            int maxId = source.Cars.Count > 0 ? source.Components.Max(rec => rec.Id) : 0;
            var element = new Car
            {
                Id = maxId + 1,
                CarComponents = new Dictionary<int, int>()
            };
            source.Cars.Add(CreateModel(model, element));
        }
        public void Update(CarBindingModel model)
        {
            var element = source.Cars.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, element);
        }
        public void Delete(CarBindingModel model)
        {
            Car element = source.Cars.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                source.Cars.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
        private static Car CreateModel(CarBindingModel model, Car car)
        {
            car.CarName = model.CarName;
            car.Price = model.Price;
            foreach (var key in car.CarComponents.Keys.ToList())
            {
                if (!model.CarComponents.ContainsKey(key))
                {
                    car.CarComponents.Remove(key);
                }
            }
            foreach (var component in model.CarComponents)
            {
                if (car.CarComponents.ContainsKey(component.Key))
                {
                    car.CarComponents[component.Key] =
                        model.CarComponents[component.Key].Item2;
                }
                else
                {
                    car.CarComponents.Add(component.Key,
                        model.CarComponents[component.Key].Item2);
                }
            }
            return car;
        }
        private CarViewModel CreateModel(Car car)
        {
            return new CarViewModel
            {
                Id = car.Id,
                CarName = car.CarName,
                Price = car.Price,
                CarComponents = car.CarComponents
                    .ToDictionary(recPC => recPC.Key, recPC =>
                    (source.Components.FirstOrDefault(recC => recC.Id ==
                    recPC.Key)?.ComponentName, recPC.Value))
            };
        }
    }
}
