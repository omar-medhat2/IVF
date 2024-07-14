using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind_EF.VMs
{
    public class RegionVM
    {
        public int RegionID { get; set; }
        public string RegionDescription { get; set; }

        public List<TerritoryVM> Territories { get; set; }
    }
}
