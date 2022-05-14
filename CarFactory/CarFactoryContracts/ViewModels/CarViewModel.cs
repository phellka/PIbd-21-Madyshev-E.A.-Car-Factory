using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using CarFactoryContracts.Attributes;

namespace CarFactoryContracts.ViewModels
{
    public class CarViewModel
    {
        public int Id { get; set; }
        [Column(title: "Название машины", gridViewAutoSize: GridViewAutoSize.Fill)]
        public string CarName { get; set; }
        [Column(title: "Цена", width: 100)]
        public decimal Price { get; set; }
        public Dictionary<int, (string, int)> CarComponents { get; set; }
    }
}
