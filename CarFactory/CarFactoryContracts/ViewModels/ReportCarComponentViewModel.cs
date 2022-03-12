using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFactoryContracts.ViewModels
{
    public class ReportCarComponentViewModel
    {
        public string ComponentName { get; set; }
        public int TotalCount { get; set; }
        public List<Tuple<string, int>> Cars { get; set; }
    }
}
