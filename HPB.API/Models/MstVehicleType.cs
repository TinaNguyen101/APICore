using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class MstVehicleType
    {
        public MstVehicleType()
        {
            Employee = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string VehicleType { get; set; }
        public decimal? ParkingFee { get; set; }

        public virtual ICollection<Employee> Employee { get; set; }
    }
}
