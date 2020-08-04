using System;
using System.Collections.Generic;

namespace HPB.API.DTO
{
    public partial class EmployeeListDto
    {

        public int Id { get; set; }
        public string PositionName { get; set; }
        public string EmpName { get; set; }
        public DateTime? EmpBirthday { get; set; }
        public string EmpIdentityNo { get; set; }
        public string EmpPassportNo { get; set; }
        public string EmpAddress { get; set; }
        public string EmpMobile1 { get; set; }
        public string EmpEmail1 { get; set; }
        public DateTime? EmpStartDate { get; set; }
        public int? EmpGender { get; set; }
        public string EmpStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string DepartmentName { get; set; }
        public int? EmpStatusId { get; set; }
        public int? DeptId { get; set; }


    }
}
