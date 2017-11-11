using SolutionName.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SolutionName.Web.ViewModel
{
    public class SalesOrderViewModel : IObjectWithState
    {
        public SalesOrderViewModel()
        {
            SalesOrderItems = new List<SalesOrderItemViewModel>();
            SalesOrderItemsToDelete = new List<int>();
        }
        public int SalesOrderId { get; set; }
        [Required (ErrorMessage ="Server: You cannot create a sales order unless you supply the custoemr's name")]
        [StringLength(30, ErrorMessage ="Server: Customer name sut be 30 character or shoreter.")]
        public string CustomerName { get; set; }
        [StringLength(10, ErrorMessage = "Server: PO Number must be 10 characters or shorter.")]
        public string PONumber { get; set; }

        public string MessageToClient { get; set; }

        public ObjectState ObjectState { get; set; }

        public List<SalesOrderItemViewModel> SalesOrderItems { get; set; }

        public List<int> SalesOrderItemsToDelete { get; set; }

        public byte[] RowVersion { get; set; }
    }
}