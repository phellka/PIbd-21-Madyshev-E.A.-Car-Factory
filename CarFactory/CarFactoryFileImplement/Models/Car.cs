using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFactoryFileImplement.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string CarName { get; set; }
        public decimal Price { get; set; }
        public Dictionary<int, int> CarComponents { get; set; }
    }
}
