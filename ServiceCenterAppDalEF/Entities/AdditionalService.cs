using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterAppDalEF.Entities
{
    public class AdditionalService
    {
        public int ServiceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal? Price { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }

}
