using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class ProjectReportStatisticsDto
    {

        public string CustName { get; set; }
        public string CustStyleCss { get; set; }
        public decimal TotalCost { get; set; }

        public int TotalProject { get; set; }


    }

    public class ProjectReportStatisticsByCustDto
    {
        public string StatisticsMonth { get; set; }
        public int CustId { get; set; }
        public string CustStyleCss { get; set; }
        public string CustName { get; set; }
        public decimal MonthTotalCost { get; set; }
        public int MonthTotalProject { get; set; }

    }
}
