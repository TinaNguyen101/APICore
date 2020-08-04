using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class MstOvertimeRate
    {
        public int Id { get; set; }
        public string RateOtname { get; set; }
        public decimal? RateOt { get; set; }
        public DateTime? ApproveDate { get; set; }
        public int? RateOttype { get; set; }
    }
}
