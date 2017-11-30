using SolutionName.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SolutionName.Web.ViewModel
{
    public class SalesOrderItemViewModel : IObjectWithState
    {
        public int SalesOrderItemId { get; set; }
        [Required(ErrorMessage ="Server: You cannot create a sales order item unless you suply the product code.")]
        [StringLength(15, ErrorMessage ="Server: Product codes must be 15 characters or shorter.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage ="Server: Product code consists of letters only.")]
        public string ProductCode { get; set; }
        [Required(ErrorMessage ="Server: You cannot create a sales order item unless you supply the quanity.")]
        [Range(1, 1000000, ErrorMessage ="Server: Quantity must be between 1 and 1,000,000.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage ="Server: Unit price is a required field.")]
        [Range(0, 100000, ErrorMessage ="Server: Unit price must be between zero and 100,000.")]
        public decimal UnitPrice { get; set; }

        public int SalesOrderId { get; set; }
        public ObjectState ObjectState { get; set; }
        public byte[] RowVersion { get; set; }

        public bool ExtendWarranty { get; set; }
        public byte? ServiceTypeId { get; set; }
        public string ServiceTypeName { get; internal set; }
        public List<ServiceType> ServiceTypes { get; internal set; }
    }
}