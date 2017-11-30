using SolutionName.Model;
using SolutionName.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolutionName.Web
{
    public static class Helpers
    {
        private static List<ServiceType> GetServiceTypes()
        {
            List<ServiceType> serviceTypes = new List<ServiceType>();

            foreach(var st in Enum.GetValues(typeof(ServiceTypeEnum)))
            {
                serviceTypes.Add(new ServiceType() { ServiceTypeId =(ServiceTypeEnum) st });
            }

            return serviceTypes;
        }

        public static SalesOrderViewModel CreateEmptySalesOrderViewModel(List<City> cities)
        {
            List<ServiceType> serviceTypes = GetServiceTypes();
            SalesOrderViewModel salesOrderViewModel = new SalesOrderViewModel();
            salesOrderViewModel.ObjectState = ObjectState.Added;
            if (cities != null)
                cities.ForEach(c => salesOrderViewModel.Cities.Add(new CityViewModel() { CityId = c.CityId, Name = c.Name }));

            salesOrderViewModel.ServiceTypes = GetServiceTypes();

            return salesOrderViewModel;
        }

        public static SalesOrderViewModel CreateSalesOrderViewModelFromSalesOrder(SalesOrder salesOrder, List<City> cities)
        {
            List<ServiceType> serviceTypes = GetServiceTypes();
            SalesOrderViewModel salesOrderViewModel = new SalesOrderViewModel();
            salesOrderViewModel.SalesOrderId = salesOrder.SalesOrderId;
            salesOrderViewModel.CustomerName = salesOrder.CustomerName;
            salesOrderViewModel.PONumber = salesOrder.PONumber;
            salesOrderViewModel.RowVersion = salesOrder.RowVersion;
            salesOrderViewModel.CityId = salesOrder.CityId;
            salesOrderViewModel.CityName = salesOrder.City.Name;
            salesOrderViewModel.ServiceTypes = GetServiceTypes();
            foreach (SalesOrderItem salesOrderItem in salesOrder.SalesOrderItems)
            {
                SalesOrderItemViewModel salesOrderItemViewModel = new SalesOrderItemViewModel();
                salesOrderItemViewModel.SalesOrderItemId = salesOrderItem.SalesOrderItemId;
                salesOrderItemViewModel.ProductCode = salesOrderItem.ProductCode;
                salesOrderItemViewModel.Quantity = salesOrderItem.Quantity;
                salesOrderItemViewModel.UnitPrice = salesOrderItem.UnitPrice;
                salesOrderItemViewModel.RowVersion = salesOrderItem.RowVersion;
                salesOrderItemViewModel.ExtendWarranty = salesOrderItem.ExtendWarranty;
                salesOrderItemViewModel.ServiceTypeId = salesOrderItem.ServiceTypeId;
                salesOrderItemViewModel.ServiceTypeName = Enum.GetName(typeof(ServiceTypeEnum), salesOrderItem.ServiceTypeId);
                salesOrderItemViewModel.ServiceTypes = serviceTypes;

                salesOrderItemViewModel.ObjectState = ObjectState.Unchanged;

                salesOrderItemViewModel.SalesOrderId = salesOrder.SalesOrderId;


                salesOrderViewModel.SalesOrderItems.Add(salesOrderItemViewModel);
            }

            if(cities!=null)
                cities.ForEach(c => salesOrderViewModel.Cities.Add(new CityViewModel() { CityId=c.CityId, Name = c.Name }));

            return salesOrderViewModel;
        }

        public static SalesOrder CreateSalesOrderFromSalesOrderViewModel(SalesOrderViewModel salesOrderViewModel)
        {
            SalesOrder salesOrder = new SalesOrder();
            salesOrder.SalesOrderId = salesOrderViewModel.SalesOrderId;
            salesOrder.CustomerName = salesOrderViewModel.CustomerName;
            salesOrder.PONumber = salesOrderViewModel.PONumber;            
            salesOrder.ObjectState = salesOrderViewModel.ObjectState;
            salesOrder.RowVersion = salesOrderViewModel.RowVersion;
            salesOrder.CityId = salesOrderViewModel.CityId;
            int temporarySalesOrderItemId = -1;
            foreach (SalesOrderItemViewModel salesOrderItemViewModel in salesOrderViewModel.SalesOrderItems)
            {
                SalesOrderItem salesOrderItem = new SalesOrderItem();
                salesOrderItem.ProductCode = salesOrderItemViewModel.ProductCode;
                salesOrderItem.Quantity = salesOrderItemViewModel.Quantity;
                salesOrderItem.UnitPrice = salesOrderItemViewModel.UnitPrice;
                salesOrderItem.ObjectState = salesOrderItemViewModel.ObjectState;
                salesOrderItem.RowVersion = salesOrderItemViewModel.RowVersion;
                salesOrderItem.ExtendWarranty = salesOrderItemViewModel.ExtendWarranty;
                salesOrderItem.ServiceTypeId = salesOrderItemViewModel.ServiceTypeId;

                if (salesOrderItemViewModel.ObjectState != ObjectState.Added)
                    salesOrderItem.SalesOrderItemId = salesOrderItemViewModel.SalesOrderItemId;
                else
                {
                    salesOrderItem.SalesOrderItemId = temporarySalesOrderItemId;
                    temporarySalesOrderItemId--;
                }

                salesOrderItem.SalesOrderId = salesOrderViewModel.SalesOrderId;

                salesOrder.SalesOrderItems.Add(salesOrderItem);
            }

            return salesOrder;
        }


        public static string GetMessageToClient(ObjectState objectState, string customerName)
        {
            string messageToClient = string.Empty;

            switch (objectState)
            {
                case ObjectState.Added:
                    messageToClient = string.Format("A sales order for {0} has been added to the database.", customerName);
                    break;

                case ObjectState.Modified:
                    messageToClient = string.Format("The customer name for this sales order has been updated to {0} in the database.", customerName);
                    break;
            }

            return messageToClient;
        }
    }
}