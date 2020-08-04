using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HPB.API.Models
{
    public partial class HPBContext : DbContext
    {
        public HPBContext()
        {
        }

        public HPBContext(DbContextOptions<HPBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AnnualBonus> AnnualBonus { get; set; }
        public virtual DbSet<AnnualEvaluationResult> AnnualEvaluationResult { get; set; }
        public virtual DbSet<AnnualKpiresult> AnnualKpiresult { get; set; }
        public virtual DbSet<AnnualLeavePaid> AnnualLeavePaid { get; set; }
        public virtual DbSet<AnnualRatingFactor> AnnualRatingFactor { get; set; }
        public virtual DbSet<AnnualReview> AnnualReview { get; set; }
        public virtual DbSet<AttachmentFile> AttachmentFile { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<DayOff> DayOff { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Evaluator> Evaluator { get; set; }
        public virtual DbSet<Holiday> Holiday { get; set; }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<MonthlyBonus> MonthlyBonus { get; set; }
        public virtual DbSet<MonthlyOt> MonthlyOt { get; set; }
        public virtual DbSet<MonthlySalary> MonthlySalary { get; set; }
        public virtual DbSet<MstCostCurrency> MstCostCurrency { get; set; }
        public virtual DbSet<MstDepartment> MstDepartment { get; set; }
        public virtual DbSet<MstEmplomentStatus> MstEmplomentStatus { get; set; }
        public virtual DbSet<MstEvaluation> MstEvaluation { get; set; }
        public virtual DbSet<MstImageBasicMonth> MstImageBasicMonth { get; set; }
        public virtual DbSet<MstImageUnitPrice> MstImageUnitPrice { get; set; }
        public virtual DbSet<MstKpi> MstKpi { get; set; }
        public virtual DbSet<MstKpiclassification> MstKpiclassification { get; set; }
        public virtual DbSet<MstKpidetail> MstKpidetail { get; set; }
        public virtual DbSet<MstOvertimeRate> MstOvertimeRate { get; set; }
        public virtual DbSet<MstPosition> MstPosition { get; set; }
        public virtual DbSet<MstProjectPosition> MstProjectPosition { get; set; }
        public virtual DbSet<MstProjectStatus> MstProjectStatus { get; set; }
        public virtual DbSet<MstReview> MstReview { get; set; }
        public virtual DbSet<MstVehicleType> MstVehicleType { get; set; }
        public virtual DbSet<MstWorkDayMonth> MstWorkDayMonth { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectMaintenance> ProjectMaintenance { get; set; }
        public virtual DbSet<Salary> Salary { get; set; }
        public virtual DbSet<Task> Task { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=192.168.1.5;Initial Catalog=HPB;User ID=sa;Password=hpbvn123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<AnnualBonus>(entity =>
            {
                entity.Property(e => e.Bonus).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.AnnualBonus)
                    .HasForeignKey(d => d.EmpId)
                    .HasConstraintName("FK_AnnualBonus_Employee");
            });

            modelBuilder.Entity<AnnualEvaluationResult>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EvaluationContent).HasMaxLength(1000);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.AnnualEvaluationResultEmp)
                    .HasForeignKey(d => d.EmpId)
                    .HasConstraintName("FK_AnnualEvaluationResult_Employee");

                entity.HasOne(d => d.Evaluation)
                    .WithMany(p => p.AnnualEvaluationResult)
                    .HasForeignKey(d => d.EvaluationId)
                    .HasConstraintName("FK_AnnualEvaluationResult_MstEvaluation");

                entity.HasOne(d => d.Evaluator)
                    .WithMany(p => p.AnnualEvaluationResultEvaluator)
                    .HasForeignKey(d => d.EvaluatorId)
                    .HasConstraintName("FK_AnnualEvaluationResult_Employee1");

                entity.HasOne(d => d.EvaluatorNavigation)
                    .WithMany(p => p.AnnualEvaluationResult)
                    .HasForeignKey(d => d.EvaluatorId)
                    .HasConstraintName("FK_AnnualEvaluationResult_Evaluator");
            });

            modelBuilder.Entity<AnnualKpiresult>(entity =>
            {
                entity.ToTable("AnnualKPIResult");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EvaluationContent).HasMaxLength(500);

                entity.Property(e => e.KpidetailId).HasColumnName("KPIDetailId");

                entity.Property(e => e.Kpiid).HasColumnName("KPIId");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.AnnualKpiresultEmp)
                    .HasForeignKey(d => d.EmpId)
                    .HasConstraintName("FK_AnnualKPIResult_Employee");

                entity.HasOne(d => d.Evaluator)
                    .WithMany(p => p.AnnualKpiresultEvaluator)
                    .HasForeignKey(d => d.EvaluatorId)
                    .HasConstraintName("FK_AnnualKPIResult_Employee1");

                entity.HasOne(d => d.EvaluatorNavigation)
                    .WithMany(p => p.AnnualKpiresult)
                    .HasForeignKey(d => d.EvaluatorId)
                    .HasConstraintName("FK_AnnualKPIResult_Evaluator");

                entity.HasOne(d => d.Kpidetail)
                    .WithMany(p => p.AnnualKpiresult)
                    .HasForeignKey(d => d.KpidetailId)
                    .HasConstraintName("FK_AnnualKPIResult_MstKPIDetail");

                entity.HasOne(d => d.Kpi)
                    .WithMany(p => p.AnnualKpiresult)
                    .HasForeignKey(d => d.Kpiid)
                    .HasConstraintName("FK_AnnualKPIResult_MstKPI");
            });

            modelBuilder.Entity<AnnualLeavePaid>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.Year).HasColumnName("year");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.AnnualLeavePaid)
                    .HasForeignKey(d => d.EmpId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnnualLeavePaid_Employee");
            });

            modelBuilder.Entity<AnnualRatingFactor>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.RatingFactorLeader).HasColumnType("decimal(3, 2)");

                entity.Property(e => e.RatingFactorMember).HasColumnType("decimal(3, 2)");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.AnnualRatingFactor)
                    .HasForeignKey(d => d.EmpId)
                    .HasConstraintName("FK_AnnualRatingFactor_Employee");
            });

            modelBuilder.Entity<AnnualReview>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ReviewContent).HasMaxLength(1000);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.AnnualReview)
                    .HasForeignKey(d => d.EmpId)
                    .HasConstraintName("FK_AnnualReview_Employee");

                entity.HasOne(d => d.Review)
                    .WithMany(p => p.AnnualReview)
                    .HasForeignKey(d => d.ReviewId)
                    .HasConstraintName("FK_AnnualReview_MstReview");
            });

            modelBuilder.Entity<AttachmentFile>(entity =>
            {
                entity.Property(e => e.AttachmentFileName).HasMaxLength(1000);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.AttachmentFile)
                    .HasForeignKey(d => d.EmpId)
                    .HasConstraintName("FK_AttachmentFile_Employee");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.AttachmentFile)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_AttachmentFile_Project1");

                entity.HasOne(d => d.ProjectMaintenance)
                    .WithMany(p => p.AttachmentFile)
                    .HasForeignKey(d => d.ProjectMaintenanceId)
                    .HasConstraintName("FK_AttachmentFile_ProjectMaintenance");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CustAddress).HasMaxLength(500);

                entity.Property(e => e.CustContactEmail).HasMaxLength(500);

                entity.Property(e => e.CustContactFax).HasMaxLength(30);

                entity.Property(e => e.CustContactName).HasMaxLength(500);

                entity.Property(e => e.CustContactPhone).HasMaxLength(20);

                entity.Property(e => e.CustContactSkype).HasMaxLength(50);

                entity.Property(e => e.CustEngName).HasMaxLength(500);

                entity.Property(e => e.CustIsDelete).HasDefaultValueSql("((0))");

                entity.Property(e => e.CustName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.CustPostCode).HasMaxLength(20);

                entity.Property(e => e.CustShortName).HasMaxLength(200);

                entity.Property(e => e.CustStyleCss).HasMaxLength(500);

                entity.Property(e => e.CustWebsite).HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<DayOff>(entity =>
            {
                entity.Property(e => e.ApprovedDate).HasColumnType("date");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DayOff1)
                    .HasColumnName("DayOff")
                    .HasColumnType("date");

                entity.Property(e => e.Reason).HasMaxLength(500);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.DayOff)
                    .HasForeignKey(d => d.EmpId)
                    .HasConstraintName("FK_DayOff_Employee");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EmpAddress)
                    .HasMaxLength(500)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.EmpAddressBirth)
                    .HasMaxLength(500)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.EmpBirthday)
                    .HasColumnType("date")
                    .HasDefaultValueSql("('1900-01-01T00:00:00.000')");

                entity.Property(e => e.EmpEmail1).HasMaxLength(100);

                entity.Property(e => e.EmpEmail2).HasMaxLength(100);

                entity.Property(e => e.EmpEndDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("('1900-01-01T00:00:00.000')");

                entity.Property(e => e.EmpGender).HasDefaultValueSql("((0))");

                entity.Property(e => e.EmpIdentityDate).HasColumnType("date");

                entity.Property(e => e.EmpIdentityNo).HasMaxLength(15);

                entity.Property(e => e.EmpIdentityPlace).HasMaxLength(250);

                entity.Property(e => e.EmpImage).HasMaxLength(50);

                entity.Property(e => e.EmpMobile1).HasMaxLength(15);

                entity.Property(e => e.EmpMobile2).HasMaxLength(15);

                entity.Property(e => e.EmpName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.EmpPassportDate).HasColumnType("date");

                entity.Property(e => e.EmpPassportNo).HasMaxLength(15);

                entity.Property(e => e.EmpStartDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("('1900-01-01T00:00:00.000')");

                entity.Property(e => e.LicensePlate).HasMaxLength(20);

                entity.Property(e => e.RegularDate).HasColumnType("date");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.VehicleComment).HasMaxLength(500);

                entity.HasOne(d => d.Dept)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.DeptId)
                    .HasConstraintName("FK_Employee_MstDepartment");

                entity.HasOne(d => d.EmpStatus)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.EmpStatusId)
                    .HasConstraintName("FK_Employee_MstEmplomentStatus");

                entity.HasOne(d => d.Pos)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.PosId)
                    .HasConstraintName("FK_Employee_MstPosition");

                entity.HasOne(d => d.VehicleType)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.VehicleTypeId)
                    .HasConstraintName("FK_Employee_MstVehicleType");
            });

            modelBuilder.Entity<Evaluator>(entity =>
            {
                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.Evaluator)
                    .HasForeignKey(d => d.EmpId)
                    .HasConstraintName("FK_Evaluator_Employee");
            });

            modelBuilder.Entity<Holiday>(entity =>
            {
                entity.Property(e => e.Holiday1)
                    .HasColumnName("Holiday")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.Member)
                    .HasForeignKey(d => d.EmpId)
                    .HasConstraintName("FK_ProjectMember_Employee");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Member)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ProjectMember_Project");

                entity.HasOne(d => d.ProjectMaintenance)
                    .WithMany(p => p.Member)
                    .HasForeignKey(d => d.ProjectMaintenanceId)
                    .HasConstraintName("FK_Member_ProjectMaintenance");

                entity.HasOne(d => d.ProjectPosition)
                    .WithMany(p => p.Member)
                    .HasForeignKey(d => d.ProjectPositionId)
                    .HasConstraintName("FK_ProjectMember_MstProjectPosition");
            });

            modelBuilder.Entity<MonthlyBonus>(entity =>
            {
                entity.Property(e => e.Comment).HasMaxLength(500);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.Wage).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.MonthlyBonus)
                    .HasForeignKey(d => d.EmpId)
                    .HasConstraintName("FK_MonthlyBonus_Employee");

                entity.HasOne(d => d.ImageUnitPrice)
                    .WithMany(p => p.MonthlyBonus)
                    .HasForeignKey(d => d.ImageUnitPriceId)
                    .HasConstraintName("FK_MonthlyBonus_MstImageUnitPrice");
            });

            modelBuilder.Entity<MonthlyOt>(entity =>
            {
                entity.ToTable("MonthlyOT");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Otdate)
                    .HasColumnName("OTDate")
                    .HasColumnType("date");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.Wage).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.MonthlyOt)
                    .HasForeignKey(d => d.EmpId)
                    .HasConstraintName("FK_MonthlyOT_Employee");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.MonthlyOt)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_MonthlyOT_Project");

                entity.HasOne(d => d.ProjectMaintenance)
                    .WithMany(p => p.MonthlyOt)
                    .HasForeignKey(d => d.ProjectMaintenanceId)
                    .HasConstraintName("FK_MonthlyOT_ProjectMaintenance");
            });

            modelBuilder.Entity<MonthlySalary>(entity =>
            {
                entity.Property(e => e.Allowance).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.BonusOt)
                    .HasColumnName("BonusOT")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Deduction).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.MonthlySalary)
                    .HasForeignKey(d => d.EmpId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MonthlySalary_Employee");

                entity.HasOne(d => d.Salary)
                    .WithMany(p => p.MonthlySalary)
                    .HasForeignKey(d => d.SalaryId)
                    .HasConstraintName("FK_MonthlySalary_Salary");
            });

            modelBuilder.Entity<MstCostCurrency>(entity =>
            {
                entity.Property(e => e.CostCurrency).HasMaxLength(50);

                entity.Property(e => e.CostCurrencySymboy).HasMaxLength(50);
            });

            modelBuilder.Entity<MstDepartment>(entity =>
            {
                entity.Property(e => e.DepartmentName)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<MstEmplomentStatus>(entity =>
            {
                entity.Property(e => e.EmpStatus).HasMaxLength(100);
            });

            modelBuilder.Entity<MstEvaluation>(entity =>
            {
                entity.Property(e => e.EvaluationHeading)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<MstImageBasicMonth>(entity =>
            {
                entity.Property(e => e.ApproveDate).HasColumnType("date");
            });

            modelBuilder.Entity<MstImageUnitPrice>(entity =>
            {
                entity.Property(e => e.ApproveDate).HasColumnType("date");

                entity.Property(e => e.ImageType).HasMaxLength(50);

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<MstKpi>(entity =>
            {
                entity.ToTable("MstKPI");

                entity.Property(e => e.Kpiheading)
                    .IsRequired()
                    .HasColumnName("KPIHeading")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<MstKpiclassification>(entity =>
            {
                entity.ToTable("MstKPIClassification");

                entity.Property(e => e.Classification)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<MstKpidetail>(entity =>
            {
                entity.ToTable("MstKPIDetail");

                entity.Property(e => e.Kpicontent)
                    .IsRequired()
                    .HasColumnName("KPIContent")
                    .HasMaxLength(1000);

                entity.Property(e => e.KpidetailNo).HasColumnName("KPIDetailNo");

                entity.Property(e => e.Kpiid).HasColumnName("KPIId");

                entity.Property(e => e.Kpirate).HasColumnName("KPIRate");

                entity.HasOne(d => d.Kpi)
                    .WithMany(p => p.MstKpidetail)
                    .HasForeignKey(d => d.Kpiid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MstKPIDetail_MstKPI");
            });

            modelBuilder.Entity<MstOvertimeRate>(entity =>
            {
                entity.Property(e => e.ApproveDate).HasColumnType("date");

                entity.Property(e => e.RateOt)
                    .HasColumnName("RateOT")
                    .HasColumnType("decimal(2, 1)");

                entity.Property(e => e.RateOtname)
                    .HasColumnName("RateOTName")
                    .HasMaxLength(50);

                entity.Property(e => e.RateOttype).HasColumnName("RateOTType");
            });

            modelBuilder.Entity<MstPosition>(entity =>
            {
                entity.Property(e => e.PositionName)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<MstProjectPosition>(entity =>
            {
                entity.Property(e => e.ProjectPositionName).HasMaxLength(200);

                entity.Property(e => e.StyleCss).HasMaxLength(1000);
            });

            modelBuilder.Entity<MstProjectStatus>(entity =>
            {
                entity.Property(e => e.ProjectStatusName).HasMaxLength(200);

                entity.Property(e => e.StyleCss).HasMaxLength(1000);
            });

            modelBuilder.Entity<MstReview>(entity =>
            {
                entity.Property(e => e.ReviewHeading)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<MstVehicleType>(entity =>
            {
                entity.Property(e => e.ParkingFee).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.VehicleType)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MstWorkDayMonth>(entity =>
            {
                entity.Property(e => e.ApproveDate).HasColumnType("date");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeliveryDate).HasColumnType("date");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.EstimateCost).HasColumnType("decimal(12, 0)");

                entity.Property(e => e.EstimateManDay).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.PaymentDate).HasColumnType("date");

                entity.Property(e => e.ProjectName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Cust)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.CustId)
                    .HasConstraintName("FK_Project_Customer");

                entity.HasOne(d => d.EstimateCostCurrency)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.EstimateCostCurrencyId)
                    .HasConstraintName("FK_Project_MstPriceCurrency");

                entity.HasOne(d => d.ProjectStatus)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.ProjectStatusId)
                    .HasConstraintName("FK_Project_MstProjectStatus");
            });

            modelBuilder.Entity<ProjectMaintenance>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeliveryDate).HasColumnType("date");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.EstimateCost).HasColumnType("decimal(12, 0)");

                entity.Property(e => e.EstimateManDay).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.MaintenanceName).HasMaxLength(200);

                entity.Property(e => e.PaymentDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.EstimateCostCurrency)
                    .WithMany(p => p.ProjectMaintenance)
                    .HasForeignKey(d => d.EstimateCostCurrencyId)
                    .HasConstraintName("FK_ProjectMaintenance_MstCostCurrency");

                entity.HasOne(d => d.MaintenanceStatus)
                    .WithMany(p => p.ProjectMaintenance)
                    .HasForeignKey(d => d.MaintenanceStatusId)
                    .HasConstraintName("FK_ProjectMaintenance_MstProjectStatus");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectMaintenance)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_ProjectMaintenance_Project");
            });

            modelBuilder.Entity<Salary>(entity =>
            {
                entity.Property(e => e.ApprovalDate).HasColumnType("date");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Salary1)
                    .HasColumnName("Salary")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.Salary)
                    .HasForeignKey(d => d.EmpId)
                    .HasConstraintName("FK_Salary_Employee");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.TaskName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.Task)
                    .HasForeignKey(d => d.EmpId)
                    .HasConstraintName("FK_Task_Employee");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Task)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_Task_Project");

                entity.HasOne(d => d.ProjectMaintenance)
                    .WithMany(p => p.Task)
                    .HasForeignKey(d => d.ProjectMaintenanceId)
                    .HasConstraintName("FK_Task_ProjectMaintenance");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(100)
                    .ValueGeneratedNever();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}
