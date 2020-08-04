using System;
using System.Collections.Generic;

namespace HPB.API.DTO
{
    public partial class EmployeeVehicleReportDto
    {
        public IEnumerable<EmployeeVehicleDetailReportDto> VehicleDetail { get; set; }
        public EmployeeVehicleTotalReportDto VehicleTotal { get; set; }

    }
        public partial class EmployeeVehicleDetailReportDto
    {

        public int No { get; set; }
        public string EmpName { get; set; }
        public string LicensePlate { get; set; }
        public string VehicleType1 { get; set; }
        public decimal ParkingFee1 { get; set; }
        public string VehicleType2 { get; set; }
        public decimal ParkingFee2 { get; set; }
        public string VehicleComment { get; set; }
        public int Id { get; set; }



    }

    public partial class EmployeeVehicleTotalReportDto
    {

        public int No { get; set; }
        public int CountVehicleType1 { get; set; }
        public decimal TotalParkingFee1 { get; set; }
        public decimal ParkingFee1 { get; set; }
        public int CountVehicleType2 { get; set; }
        public decimal TotalParkingFee2 { get; set; }
        public decimal ParkingFee2 { get; set; }
        public decimal totalFee { get; set; }



    }

    public partial class EmployeeReportDto
    {
        public IEnumerable<EmployeeRegularReportDto> EmployeeRegular { get; set; }
        public IEnumerable<EmployeeProbationReportDto> EmployeeProbation { get; set; }
        public IEnumerable<EmployeeLeaveReportDto> EmployeeLeave { get; set; }

    }

    public partial class EmployeeRegularReportDto
    {
        public int Id { get; set; }
        public int No { get; set; }
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
        public string EmpEmail1 { get; set; }
        public string EmpEmail2 { get; set; }
        public DateTime? EmpStartDate { get; set; }
        public string EmpComment { get; set; }
    }

    public partial class EmployeeProbationReportDto
    {
        public int Id { get; set; }
        public int No { get; set; }
        public string EmpName { get; set; }
        public DateTime? EmpBirthday { get; set; }
        public string EmpIdentityNo { get; set; }
        public DateTime? EmpIdentityDate { get; set; }
        public string EmpIdentityPlace { get; set; }
        public string EmpAddress { get; set; }
        public string EmpAddressBirth { get; set; }
        public string EmpMobile1 { get; set; }
        public string EmpEmail1 { get; set; }
        public string EmpEmail2 { get; set; }
        public DateTime? EmpStartDate { get; set; }
        public string EmpComment { get; set; }
    }

    public partial class EmployeeLeaveReportDto
    {
        public int Id { get; set; }
        public int No { get; set; }
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
        public string EmpEmail1 { get; set; }
        public string EmpEmail2 { get; set; }
        public DateTime? EmpStartDate { get; set; }
        public DateTime? EmpEndDate { get; set; }
        public string EmpComment { get; set; }
    }
}
