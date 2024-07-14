using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind_EF.VMs
{
    public class CustomerDemographicVM
    {
        public string CustomerTypeID { get; set; }
        public string CustomerDesc { get; set; }

        public List<CustomerVM> Customers { get; set; }

    }
}
