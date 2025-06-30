using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterAppDalEF.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public int ClientId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int RepairTypeId { get; set; }
        public int? AdditionalServiceId { get; set; }

        public Client? Client { get; set; }
        public RepairType? RepairType { get; set; }
        public AdditionalService? AdditionalService { get; set; }
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }

}
