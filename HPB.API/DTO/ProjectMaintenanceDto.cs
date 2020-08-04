using System;
using System.Collections.Generic;

namespace HPB.API.DTO
{
    public partial class ProjectMaintenanceDto
    {

        public int Id { get; set; }
        public int ProjectId { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string MaintenanceName { get; set; }
        public string MaintenanceContent { get; set; }
        public decimal? EstimateCost { get; set; }
        public int? EstimateCostCurrencyId { get; set; }
        public string CostCurrency { get; set; }
        public string CostCurrencySymboy { get; set; }

        public decimal? EstimateManDay { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? MaintenanceStatusId { get; set; }
        public string ProjectStatusName { get; set; }
        public string StyleCss { get; set; }

        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedId { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
