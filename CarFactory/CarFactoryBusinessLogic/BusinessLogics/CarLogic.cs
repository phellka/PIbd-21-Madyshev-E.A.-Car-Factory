using System;
using System.Collections.Generic;
using System.Text;
using CarFactoryContracts.BindingModels;
using CarFactoryContracts.BusinessLogicsContracts;
using CarFactoryContracts.StoragesContracts;
using CarFactoryContracts.ViewModels;

namespace CarFactoryBusinessLogic.BusinessLogics
{
    public class CarLogic : ICarLogic
    {
        private readonly ICarStorage carStorage;
        public CarLogic(ICarStorage carStorage)
        {
            this.carStorage = carStorage;
        }
        public List<CarViewModel> Read(CarBindingModel model)
        {
            if (model == null)
            {
                return carStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<CarViewModel> { carStorage.GetElement(model) };
            }
            return carStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(CarBindingModel model)
        {
            var element = carStorage.GetElement(new CarBindingModel { CarName = model.CarName });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть такая машина с таким названием");
            }
            if (model.Id.HasValue)
            {
                carStorage.Update(model);
            }
            else
            {
                carStorage.Insert(model);
            }
        }
        public void Delete(CarBindingModel model)
        {
            var element = carStorage.GetElement(new CarBindingModel { Id = model.Id });
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            carStorage.Delete(model);
        }
    }
}
