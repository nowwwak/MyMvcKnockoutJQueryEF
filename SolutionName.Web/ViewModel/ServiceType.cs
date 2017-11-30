using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolutionName.Web.ViewModel
{
    public enum ServiceTypeEnum : byte
    {
        Standard = 1,
        Extended = 2,
        Premium = 3
    }

    public class ServiceType
    {
        public ServiceTypeEnum ServiceTypeId { get; set; }
        public string ServiceTypeName { get { return Enum.GetName(typeof(ServiceTypeEnum), ServiceTypeId); } }
    }
}