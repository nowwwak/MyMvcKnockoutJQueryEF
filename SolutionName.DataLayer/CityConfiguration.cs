using SolutionName.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionName.DataLayer
{
    public class CityConfiguration : EntityTypeConfiguration<City>
    {
        public CityConfiguration()
        {
            Property(so => so.Name).HasMaxLength(50).IsRequired();
        }
    }
}
