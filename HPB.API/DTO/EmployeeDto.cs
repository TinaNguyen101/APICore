using System;
using System.Collections.Generic;

namespace HPB.API.DTO
{
    public partial class EmployeeDto
    {
        public int Id { get; set; }
        public int? PosId { get; set; }
        public string PositionName { get; set; }
        public string EmpName { get; set; }
        public DateTime? EmpBirthday { get; set; }
        public string EmpIdentityNo { get; set; }
        public DateTime? EmpIdentityDate { get; set; }
        public string EmpIdentityPlace { get; set; }
        public string EmpPassportNo { get; set; }
        public DateTime? EmpPassportDate { get; set; }
        public int? EmpPassportExpiryDate { get; set; }
        public string EmpAddress { get; set; }
        public string EmpAddressBirth { get; set; }
        public string EmpMobile1 { get; set; }
        public string EmpMobile2 { get; set; }
        public string EmpEmail1 { get; set; }
        public string EmpEmail2 { get; set; }
        public string EmpImage { get; set; }
        public DateTime? EmpStartDate { get; set; }
        public DateTime? EmpEndDate { get; set; }
        public int? EmpGender { get; set; }
        public string EmpComment { get; set; }
        public int? EmpStatusId { get; set; }
        public string EmpStatus { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? DeptId { get; set; }
        public string DepartmentName { get; set; }
        public string LicensePlate { get; set; }
        public int? VehicleTypeId { get; set; }
        public string VehicleType { get; set; }
        public string VehicleComment { get; set; }

        public DateTime? RegularDate { get; set; }
    }
}
