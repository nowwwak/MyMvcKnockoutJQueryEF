using SolutionName.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionName.DataLayer
{
    public class SalesContext : DbContext
    {
        public SalesContext() : base("DefaultConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
         modelBuilder.Configurations.Add(new SalesOrderConfiguration());
        }

        public DbSet<SalesOrder> SalesOrders { get; set; }
    }
}
