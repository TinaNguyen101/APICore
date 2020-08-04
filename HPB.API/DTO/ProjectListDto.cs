using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class ProjectListDto
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? PaymentDate { get; set; }

        public int? CustId { get; set; }
        public string CustName { get; set; }
        public string CustShortName { get; set; }

        
        public decimal? EstimateCost { get; set; }
        public string CostCurrencySymboy { get; set; }
        public int? EstimateCostCurrencyId { get; set; }


        public int? ProjectStatusId { get; set; }
        public string ProjectStatusName { get; set; }
        public string StyleCss { get; set; }
    }
}
