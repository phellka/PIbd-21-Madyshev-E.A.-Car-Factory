using System;
using System.Collections.Generic;
using System.Text;
using CarFactoryContracts.ViewModels;
using CarFactoryContracts.BindingModels;

namespace CarFactoryContracts.BusinessLogicsContracts
{
    public interface ICarLogic
    {
        List<CarViewModel> Read(CarBindingModel model);
        void CreateOrUpdate(CarBindingModel model);
        void Delete(CarBindingModel model);
    }
}
