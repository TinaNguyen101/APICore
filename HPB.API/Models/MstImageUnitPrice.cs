using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class MstImageUnitPrice
    {
        public MstImageUnitPrice()
        {
            MonthlyBonus = new HashSet<MonthlyBonus>();
        }

        public int Id { get; set; }
        public decimal? UnitPrice { get; set; }
        public string ImageType { get; set; }
        public DateTime? ApproveDate { get; set; }

        public virtual ICollection<MonthlyBonus> MonthlyBonus { get; set; }
    }
}
