using System;
using System.Collections.Generic;
using System.Text;
using CarFactoryContracts.ViewModels;
using CarFactoryContracts.BindingModels;

namespace CarFactoryContracts.BusinessLogicsContracts
{
    public interface IComponentLogic
    {
        List<ComponentViewModel> Read(ComponentBindingModel model);
        void CreateOrUpdate(ComponentBindingModel model);
        void Delete(ComponentBindingModel model);
    }
}
