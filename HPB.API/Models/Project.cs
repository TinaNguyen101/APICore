using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class Project
    {
        public Project()
        {
            AttachmentFile = new HashSet<AttachmentFile>();
            Member = new HashSet<Member>();
            MonthlyOt = new HashSet<MonthlyOt>();
            ProjectMaintenance = new HashSet<ProjectMaintenance>();
            Task = new HashSet<Task>();
        }

        public int Id { get; set; }
        public int? CustId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDecription { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? EstimateCost { get; set; }
        public int? EstimateCostCurrencyId { get; set; }
        public decimal? EstimateManDay { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? ProjectStatusId { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedId { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Customer Cust { get; set; }
        public virtual MstCostCurrency EstimateCostCurrency { get; set; }
        public virtual MstProjectStatus ProjectStatus { get; set; }
        public virtual ICollection<AttachmentFile> AttachmentFile { get; set; }
        public virtual ICollection<Member> Member { get; set; }
        public virtual ICollection<MonthlyOt> MonthlyOt { get; set; }
        public virtual ICollection<ProjectMaintenance> ProjectMaintenance { get; set; }
        public virtual ICollection<Task> Task { get; set; }
    }
}
