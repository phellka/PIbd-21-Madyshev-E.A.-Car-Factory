using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using CarFactoryContracts.Attributes;

namespace CarFactoryContracts.ViewModels
{
    public class ComponentViewModel
    {
        public int Id { get; set; }
        [Column(title: "Название компонента", gridViewAutoSize: GridViewAutoSize.Fill)]
        public string ComponentName { get; set; }
    }
}
