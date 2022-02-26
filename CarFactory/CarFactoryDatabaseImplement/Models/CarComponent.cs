using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CarFactoryDatabaseImplement.Models
{
    public class CarComponent
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public int ComponentId { get; set; }
        [Required]
        public int Count { get; set; }
        public virtual Component Component { get; set; }
        public virtual Car Car { get; set; }
    }
}
