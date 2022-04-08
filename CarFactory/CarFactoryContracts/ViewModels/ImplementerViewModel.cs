using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;

namespace CarFactoryContracts.ViewModels
{
    public class ImplementerViewModel
    {
        public int Id { get; set; }
        [DisplayName("ФИО исполнителя")]
        public string ImplementerFCs { get; set; }
        [DisplayName("Время работы исполнителя")]
        public int WorkingTime { get; set; }
        [DisplayName("Время отдыха исполнителя")]
        public int PauseTime { get; set; }
    }
}
