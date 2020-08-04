using System;
using System.Collections.Generic;

namespace HPB.API.DTO
{
    public partial class ProjectMaintenanceAttachmentFileDto
    {
        public int Id { get; set; }
        public int? ProjectMaintenanceId { get; set; }
        public string AttachmentFileName { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}
