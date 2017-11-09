namespace SolutionName.DataLayer.Migrations
{
    using Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SolutionName.DataLayer.SalesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SolutionName.DataLayer.SalesContext context)
        {
            context.SalesOrders.AddOrUpdate(
                so => so.CustomerName,
                new SalesOrder { CustomerName = "Adam", PONumber = "76576", SalesOrderItems
                    =
                    {
                        new SalesOrderItem {ProductCode="ABC123", Quantity=10, UnitPrice=1.23m },
                        new SalesOrderItem {ProductCode="ZYZ987", Quantity=7, UnitPrice=41.23m },
                        new SalesOrderItem {ProductCode="FDEA44", Quantity=3, UnitPrice=15.00m }
                    }
                },
                new SalesOrder { CustomerName = "Michael" },
                new SalesOrder { CustomerName = "David", PONumber = "Acme 9" }
                );
        }
    }
}
