using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterAppDalEF.Entities
{
    public class RepairType
    {
        public int RepairTypeId { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }

        public ICollection<Order> Orders { get; set; }
    }

}
