using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class MstKpiclassification
    {
        public int Id { get; set; }
        public string Classification { get; set; }
        public int? StartScoreRange { get; set; }
        public int? EndScoreRange { get; set; }
    }
}
