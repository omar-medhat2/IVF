using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind_EF
{
    public static class DbContextExtensions
    {
        public static string GetPrimaryKeyName<T>(this DbContext context) where T : class
        {
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            ObjectSet<T> set = objectContext.CreateObjectSet<T>();
            EntityType entityType = set.EntitySet.ElementType;
            string pkName = entityType.KeyMembers.Select(k => k.Name).FirstOrDefault();
            return pkName;
        }
    }
}
