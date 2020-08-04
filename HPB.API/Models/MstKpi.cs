using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class MstKpi
    {
        public MstKpi()
        {
            AnnualKpiresult = new HashSet<AnnualKpiresult>();
            MstKpidetail = new HashSet<MstKpidetail>();
        }

        public int Id { get; set; }
        public string Kpiheading { get; set; }

        public virtual ICollection<AnnualKpiresult> AnnualKpiresult { get; set; }
        public virtual ICollection<MstKpidetail> MstKpidetail { get; set; }
    }
}
