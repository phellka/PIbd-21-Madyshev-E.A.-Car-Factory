using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarFactoryContracts.ViewModels;

namespace CarFactoryBusinessLogic.OfficePackage.HelperModels
{
    public class WordInfo
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public List<CarViewModel> Cars { get; set; }
        public List<WarehouseViewModel> Warehouses { get; set; }
    }
}
