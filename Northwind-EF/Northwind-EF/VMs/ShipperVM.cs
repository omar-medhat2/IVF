using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind_EF.VMs
{
    public class ShipperVM
    {

        public int ShipperID { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public List<OrderVM> Orders { get; set; } = new List<OrderVM>();

       
    }
}
