using System;
using System.Collections.Generic;
using System.Text;
using CarFactoryContracts.ViewModels;
using CarFactoryContracts.BindingModels;

namespace CarFactoryContracts.BusinessLogicsContracts
{
    public interface IOrderLogic
    {
        List<OrderViewModel> Read(OrderBindingModel model);
        void CreateOrder(CreateOrderBindingModel model);
        void TakeOrder(ChangeStatusBindingModel model);
        void FinishOrder(ChangeStatusBindingModel model);
        void DeliveryOrder(ChangeStatusBindingModel model);
    }
}
