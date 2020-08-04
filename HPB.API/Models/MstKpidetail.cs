using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class MstKpidetail
    {
        public MstKpidetail()
        {
            AnnualKpiresult = new HashSet<AnnualKpiresult>();
        }

        public int Id { get; set; }
        public string Kpicontent { get; set; }
        public int Kpiid { get; set; }
        public int? KpidetailNo { get; set; }
        public int? Kpirate { get; set; }

        public virtual MstKpi Kpi { get; set; }
        public virtual ICollection<AnnualKpiresult> AnnualKpiresult { get; set; }
    }
}
