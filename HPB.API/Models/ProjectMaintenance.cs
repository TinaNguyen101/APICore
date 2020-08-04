using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class ProjectMaintenance
    {
        public ProjectMaintenance()
        {
            AttachmentFile = new HashSet<AttachmentFile>();
            Member = new HashSet<Member>();
            MonthlyOt = new HashSet<MonthlyOt>();
            Task = new HashSet<Task>();
        }

        public int Id { get; set; }
        public int ProjectId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string MaintenanceContent { get; set; }
        public string MaintenanceName { get; set; }
        public decimal? EstimateCost { get; set; }
        public int? EstimateCostCurrencyId { get; set; }
        public decimal? EstimateManDay { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? MaintenanceStatusId { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedId { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual MstCostCurrency EstimateCostCurrency { get; set; }
        public virtual MstProjectStatus MaintenanceStatus { get; set; }
        public virtual Project Project { get; set; }
        public virtual ICollection<AttachmentFile> AttachmentFile { get; set; }
        public virtual ICollection<Member> Member { get; set; }
        public virtual ICollection<MonthlyOt> MonthlyOt { get; set; }
        public virtual ICollection<Task> Task { get; set; }
    }
}
