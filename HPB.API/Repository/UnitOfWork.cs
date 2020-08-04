using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data.Common;
using HPB.API.Repository.Interface;
using Microsoft.Extensions.Configuration;

namespace HPB.API.Repositories
{
    public interface IUnitOfWork
    {
        //Repository
        IKPIRepository KPIRepository { get; }
        IAnnualBonusRepository AnnualBonusRepository { get; }
        IAnnualLeavePaidRepository AnnualLeavePaidRepository { get; }
        ISalaryRepository SalaryRepository { get; }
        ITaskRepository TaskRepository { get; }
        IProjectRepository ProjectRepository { get; }
        IMemberRepository MemberRepository { get; }
        IProjectMaintenanceRepository ProjectMaintenanceRepository { get; }
        IAttachmentFileRepository AttachmentFileRepository { get; }
        IUserRepository UserRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
         ICustomerRepository CustomerRepository { get; }
        IMstRepository MstRepository { get; }
        IDbTransaction Transaction { get; set; }
        IDbConnection GetConnection { get; }
        IDbTransaction Begin();
        void Commit();
        void Dispose();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly IConfiguration _config;
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private bool _disposed;

        //Repository
        private IKPIRepository _kPIRepository;
        private IAnnualBonusRepository _annualBonusRepository;
        private IAnnualLeavePaidRepository _annualLeavePaidRepository;
        private ISalaryRepository _salaryRepository;
        private ITaskRepository _taskRepository;
        private IProjectRepository _projectRepository;
        private IMemberRepository _MemberRepository;
        private IProjectMaintenanceRepository _projectMaintenanceRepository;
        private IAttachmentFileRepository _attachmentFileRepository;
        private IUserRepository _UserRepository;
        private IEmployeeRepository _EmployeeRepository;
        private ICustomerRepository _CustomerRepository;
        private IMstRepository _MstRepository;


        public UnitOfWork(IConfiguration config
            , IProjectRepository projectRepository
            , IMemberRepository memberRepository
            , IProjectMaintenanceRepository projectMaintenanceRepository
            , IAttachmentFileRepository attachmentFileRepository
            , IUserRepository userRepository
            , IMstRepository mstRepository
            , IEmployeeRepository employeeRepository
            , ICustomerRepository customerRepository
            , ITaskRepository taskRepository
            , ISalaryRepository salaryRepository
            , IAnnualLeavePaidRepository annualLeavePaidRepository
            , IAnnualBonusRepository annualBonusRepository
            , IKPIRepository kPIRepository
            )
        {
            _config = config;
            _projectRepository = projectRepository;
            _MemberRepository = memberRepository;
            _projectMaintenanceRepository = projectMaintenanceRepository;
            _attachmentFileRepository = attachmentFileRepository;
            _UserRepository = userRepository;
            _MstRepository = mstRepository;
            _EmployeeRepository = employeeRepository;
            _CustomerRepository = customerRepository;
            _taskRepository = taskRepository;
            _salaryRepository = salaryRepository;
            _annualLeavePaidRepository = annualLeavePaidRepository;
            _annualBonusRepository = annualBonusRepository;
            _kPIRepository = kPIRepository;
        }

        #region Repository property
        public IKPIRepository KPIRepository
        {
            get
            {
                _kPIRepository.Transaction = _transaction;
                return _kPIRepository;
            }
        }
        public IAnnualBonusRepository AnnualBonusRepository
        {
            get
            {
                _annualBonusRepository.Transaction = _transaction;
                return _annualBonusRepository;
            }
        }
        public IAnnualLeavePaidRepository AnnualLeavePaidRepository
        {
            get
            {
                _annualLeavePaidRepository.Transaction = _transaction;
                return _annualLeavePaidRepository;
            }
        }
        public ISalaryRepository SalaryRepository
        {
            get
            {
                _salaryRepository.Transaction = _transaction;
                return _salaryRepository;
            }
        }

        public ITaskRepository TaskRepository
        {
            get
            {
                _taskRepository.Transaction = _transaction;
                return _taskRepository;
            }
        }

        public IProjectRepository ProjectRepository
        {
            get
            {
                _projectRepository.Transaction = _transaction;
                return _projectRepository;
            }
        }
        public IMemberRepository MemberRepository
        {
            get
            {
                _MemberRepository.Transaction = _transaction;
                return _MemberRepository;
            }
        }

        public IProjectMaintenanceRepository ProjectMaintenanceRepository
        {
            get
            {
                _projectMaintenanceRepository.Transaction = _transaction;
                return _projectMaintenanceRepository;
            }
        }

        public IAttachmentFileRepository AttachmentFileRepository
        {
            get
            {
                _attachmentFileRepository.Transaction = _transaction;
                return _attachmentFileRepository;
            }
        }
        public IUserRepository UserRepository
        {
            get
            {
                _UserRepository.Transaction = _transaction;
                return _UserRepository;
            }
        }

        public IEmployeeRepository EmployeeRepository
        {
            get
            {
                _EmployeeRepository.Transaction = _transaction;
                return _EmployeeRepository;
            }
        }

        public ICustomerRepository CustomerRepository
        {
            get
            {
                _CustomerRepository.Transaction = _transaction;
                return _CustomerRepository;
            }
        }
        public IMstRepository MstRepository
        {
            get
            {
                _MstRepository.Transaction = _transaction;
                return _MstRepository;
            }
        }

        private void resetRepositories()
        {
            //Repository
            _projectRepository = null;
            _UserRepository = null;
            _MemberRepository = null;
            _projectMaintenanceRepository = null;
            _attachmentFileRepository = null;
            _MstRepository = null;
            _EmployeeRepository = null;
            _CustomerRepository = null;
        }

        #endregion
        public IDbTransaction Transaction
        {
            get
            {
                return _transaction;
            }
            set
            {
                _transaction = value;
            }
        }

        public IDbConnection GetConnection
        {
            get
            {
                _connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
                _connection.Open();
                return _connection;
            }
        }

        public IDbTransaction Begin()
        {
            return GetConnection.BeginTransaction();
        }
        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
            }
        }

        

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            dispose(false);
        }
    }
}