using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using CarFactoryContracts.Attributes;

namespace CarFactoryContracts.ViewModels
{
    public class ImplementerViewModel
    {
        public int Id { get; set; }
        [Column(title: "ФИО исполнителя", gridViewAutoSize: GridViewAutoSize.Fill)]
        public string ImplementerFCs { get; set; }
        [Column(title: "Время работы исполнителя", width: 150)]
        public int WorkingTime { get; set; }
        [Column(title: "Время отдыха исполнителя", width: 150)]
        public int PauseTime { get; set; }
    }
}
