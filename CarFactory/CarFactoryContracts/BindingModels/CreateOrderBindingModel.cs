using System;
using System.Collections.Generic;
using System.Text;

namespace CarFactoryContracts.BindingModels
{
    public class CreateOrderBindingModel
    {
        public int CarId { get; set; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
    }
}
