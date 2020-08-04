using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class MstWorkDayMonth
    {
        public int Id { get; set; }
        public int WorkDayMonth { get; set; }
        public DateTime? ApproveDate { get; set; }
    }
}
