using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class AnnualReview
    {
        public int Id { get; set; }
        public int? EmpId { get; set; }
        public int? Year { get; set; }
        public int? ReviewId { get; set; }
        public string ReviewContent { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedId { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Employee Emp { get; set; }
        public virtual MstReview Review { get; set; }
    }
}
