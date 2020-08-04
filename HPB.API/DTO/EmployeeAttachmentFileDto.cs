using System;
using System.Collections.Generic;

namespace HPB.API.DTO
{
    public partial class EmployeeAttachmentFileDto
    {
        public int Id { get; set; }
        public int? EmpId { get; set; }
        public string AttachmentFileName { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}
