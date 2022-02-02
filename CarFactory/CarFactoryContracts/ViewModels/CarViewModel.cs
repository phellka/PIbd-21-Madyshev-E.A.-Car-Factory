using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace CarFactoryContracts.ViewModels
{
    public class CarViewModel
    {
        public int Id { get; set; }
        [DisplayName("Название машины")]
        public string CarName { get; set; }
        [DisplayName("Цена")]
        public decimal Price { get; set; }
        public Dictionary<int, (string, int)> CarComponents { get; set; }
    }
}
