using System;
using System.Collections.Generic;
using System.Text;
using CarFactoryContracts.BindingModels;
using CarFactoryContracts.ViewModels;

namespace CarFactoryContracts.StoragesContracts
{
    public interface ICarStorage
    {
        List<CarViewModel> GetFullList();
        List<CarViewModel> GetFilteredList(CarBindingModel model);
        CarViewModel GetElement(CarBindingModel model);
        void Insert(CarBindingModel model);
        void Update(CarBindingModel model);
        void Delete(CarBindingModel model);
    }
}
