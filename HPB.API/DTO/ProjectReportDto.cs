using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class ProjectReportDto
    {

        public int ProjectId { get; set; }

        public string CustName { get; set; }

        public string ProjectName { get; set; }

        public string ProjectDecription { get; set; }

        public DateTime? ProjectStartDate { get; set; }
        public DateTime? ProjectEndDate { get; set; }

        public decimal? ProjectEstimateCost { get; set; }
        public decimal? ProjectEstimateCostVND { get; set; }

        public string ProjectCurrencySymboy { get; set; }

        public DateTime? ProjectDeliveryDate { get; set; }
        public DateTime? ProjectPaymentDate { get; set; }
        public string ProjectStatusId { get; set; }
        public string ProjectStatus { get; set; }
        public string ProjectStatusStyle { get; set; }

        public string ProjectTotalEstimateCost { get; set; }
        public string ProjectLeader { get; set; }
        public string ProjectCoder { get; set; }
        public string ProjectTester { get; set; }


        public virtual ICollection<ProjectMaintenanceReportDto> ProjectMaintenanceReports { get; set; }


    }

    public class ProjectMaintenanceReportDto
    {
        public int MaintenanceId { get; set; }
        public string MaintenanceName { get; set; }
        public string MaintenanceContent { get; set; }
        public int MaintenanceProjectId { get; set; }
        public DateTime? MaintenanceStartDate { get; set; }
        public DateTime? MaintenanceEndDate { get; set; }

        public decimal? MaintenanceEstimateCost { get; set; }
        public decimal? MaintenanceEstimateCostVND { get; set; }
        public string MaintenanceCurrencySymboy { get; set; }

        public DateTime? MaintenanceDeliveryDate { get; set; }
        public DateTime? MaintenancePaymentDate { get; set; }

        public string MaintenanceStatus { get; set; }
        public string MaintenanceStatusStyle { get; set; }

        public string MaintenanceLeader { get; set; }
        public string MaintenanceCoder { get; set; }
        public string MaintenanceTester { get; set; }
    }
}
