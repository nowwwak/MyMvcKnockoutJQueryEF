using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionName.Model
{
    public class SalesOrderItem : IObjectWithState
    {
        [Key]
        public int SalesOrderItemId { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public int SalesOrderId { get; set; }
        public SalesOrder SalesOrder { get; set; }
        public ObjectState ObjectState { get; set; }
        public byte[] RowVersion { get; set; }

        public bool ExtendWarranty { get; set; }
        public byte? ServiceTypeId {get;set;}
    }
}
