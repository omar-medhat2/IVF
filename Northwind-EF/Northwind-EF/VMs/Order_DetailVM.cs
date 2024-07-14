using Northwind_EF.VMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind_EF.VMs
{
    public class Order_DetailVM
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }
        public OrderVM Order { get; set; }
        public ProductVM Product { get; set; }
    }
}
