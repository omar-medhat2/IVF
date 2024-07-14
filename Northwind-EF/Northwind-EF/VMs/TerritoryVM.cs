using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind_EF.VMs
{
    public class TerritoryVM
    {

        public string TerritoryID { get; set; }
        public string TerritoryDescription { get; set; }
        public int RegionID { get; set; }
        public RegionVM Region { get; set; }
        public List<EmployeeVM> Employees { get; set; }



    }
}
