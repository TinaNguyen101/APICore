using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class Employee
    {
        public Employee()
        {
            AnnualBonus = new HashSet<AnnualBonus>();
            AnnualEvaluationResultEmp = new HashSet<AnnualEvaluationResult>();
            AnnualEvaluationResultEvaluator = new HashSet<AnnualEvaluationResult>();
            AnnualKpiresultEmp = new HashSet<AnnualKpiresult>();
            AnnualKpiresultEvaluator = new HashSet<AnnualKpiresult>();
            AnnualLeavePaid = new HashSet<AnnualLeavePaid>();
            AnnualRatingFactor = new HashSet<AnnualRatingFactor>();
            AnnualReview = new HashSet<AnnualReview>();
            AttachmentFile = new HashSet<AttachmentFile>();
            DayOff = new HashSet<DayOff>();
            Evaluator = new HashSet<Evaluator>();
            Member = new HashSet<Member>();
            MonthlyBonus = new HashSet<MonthlyBonus>();
            MonthlyOt = new HashSet<MonthlyOt>();
            MonthlySalary = new HashSet<MonthlySalary>();
            Salary = new HashSet<Salary>();
            Task = new HashSet<Task>();
        }

        public int Id { get; set; }
        public int? PosId { get; set; }
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
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? DeptId { get; set; }
        public string LicensePlate { get; set; }
        public int? VehicleTypeId { get; set; }
        public string VehicleComment { get; set; }
        public DateTime? RegularDate { get; set; }
        public int? IsDelete { get; set; }

        public virtual MstDepartment Dept { get; set; }
        public virtual MstEmplomentStatus EmpStatus { get; set; }
        public virtual MstPosition Pos { get; set; }
        public virtual MstVehicleType VehicleType { get; set; }
        public virtual ICollection<AnnualBonus> AnnualBonus { get; set; }
        public virtual ICollection<AnnualEvaluationResult> AnnualEvaluationResultEmp { get; set; }
        public virtual ICollection<AnnualEvaluationResult> AnnualEvaluationResultEvaluator { get; set; }
        public virtual ICollection<AnnualKpiresult> AnnualKpiresultEmp { get; set; }
        public virtual ICollection<AnnualKpiresult> AnnualKpiresultEvaluator { get; set; }
        public virtual ICollection<AnnualLeavePaid> AnnualLeavePaid { get; set; }
        public virtual ICollection<AnnualRatingFactor> AnnualRatingFactor { get; set; }
        public virtual ICollection<AnnualReview> AnnualReview { get; set; }
        public virtual ICollection<AttachmentFile> AttachmentFile { get; set; }
        public virtual ICollection<DayOff> DayOff { get; set; }
        public virtual ICollection<Evaluator> Evaluator { get; set; }
        public virtual ICollection<Member> Member { get; set; }
        public virtual ICollection<MonthlyBonus> MonthlyBonus { get; set; }
        public virtual ICollection<MonthlyOt> MonthlyOt { get; set; }
        public virtual ICollection<MonthlySalary> MonthlySalary { get; set; }
        public virtual ICollection<Salary> Salary { get; set; }
        public virtual ICollection<Task> Task { get; set; }
    }
}
