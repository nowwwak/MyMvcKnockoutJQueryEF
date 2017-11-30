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
            context.Cities.AddOrUpdate(
                so => so.Name,
                new City { Name = "London" },
                new City { Name = "New York" },
                new City { Name = "Warsaw" }
                );

            context.SaveChanges();

            City london = context.Cities.First(c => c.Name == "London");
            context.SalesOrders.AddOrUpdate(
                so => so.CustomerName,
                new SalesOrder { CustomerName = "Adam", PONumber = "76576", CityId=london.CityId, City=london, SalesOrderItems
                    =
                    {
                        new SalesOrderItem {ProductCode="ABC123", Quantity=10, UnitPrice=1.23m, ExtendWarranty=true, ServiceTypeId=1 },
                        new SalesOrderItem {ProductCode="ZYZ987", Quantity=7, UnitPrice=41.23m, ExtendWarranty=true, ServiceTypeId=1 },
                        new SalesOrderItem {ProductCode="FDEA44", Quantity=3, UnitPrice=15.00m, ExtendWarranty=true, ServiceTypeId=1 }
                    }
                },
                new SalesOrder { CustomerName = "Michael", CityId = london.CityId, City = london, },
                new SalesOrder { CustomerName = "David", PONumber = "Acme 9", CityId = london.CityId, City = london, }
                );
        }
    }
}
