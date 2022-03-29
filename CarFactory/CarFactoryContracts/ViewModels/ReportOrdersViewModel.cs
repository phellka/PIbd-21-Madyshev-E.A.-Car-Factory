using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFactoryContracts.ViewModels
{
    public class ReportOrdersViewModel
    {
        public int Count { get; set; }
        public DateTime DateCreate { get; set; }
        public string ClientFCs { get; set; }
        public string CarName { get; set; }
        public string Status { get; set; }
        public decimal Sum { get; set; }
    }
}
