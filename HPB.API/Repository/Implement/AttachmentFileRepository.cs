using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HPB.API.Models;
using HPB.API.Repository.Interface;
using HPB.API.DTO;

namespace HPB.API.Repositories.Implement
{
    internal class AttachmentFileRepository : RepositoryBase, IAttachmentFileRepository
    {
        public AttachmentFileRepository()
        {

        }

        public async Task<ProjectAttachmentFileDto> GetAttachmentFileByIdAsync(long id)
        {
            var sql = " SELECT  Id , ProjectId,ProjectMaintenanceId,EmpId, AttachmentFileName , CreatedId , CreatedDate  " +
                      " FROM AttachmentFile  " +
                      " where Id = @Id" +
                      " order by CreatedDate desc ";
            return await SqlMapper.QueryFirstOrDefaultAsync<ProjectAttachmentFileDto>(Connection, sql, new { Id = id }, transaction: Transaction);
        }
       

        public async Task<int> DeleteAttachmentFileAsync(int id)
        {
            var sql = " DELETE [dbo].[AttachmentFile] " +
                     " WHERE  Id = @Id";

            var x = await Connection.ExecuteAsync(sql, new
            {
                Id = id
            }, Transaction);
            return x;
        }

        #region project attach
        public async Task<IEnumerable<ProjectAttachmentFileDto>> GetProjectAttachmentFileAsync(long projectId)
        {
            var sql = " SELECT  Id , ProjectId, AttachmentFileName , CreatedId , CreatedDate  " +
                      " FROM AttachmentFile  " +
                      " where ProjectId = @ProjectId" +
                      "  order by CreatedDate desc ";
            return await SqlMapper.QueryAsync<ProjectAttachmentFileDto>(Connection, sql, new { ProjectId = projectId }, transaction: Transaction);
        }

        public async Task<IEnumerable<ProjectAttachmentFileDto>> GetProjectAttachmentFileNameAsync(ProjectAttachmentFileDto dto)
        {
            var sql = " SELECT  Id , ProjectId, AttachmentFileName , CreatedId , CreatedDate  " +
                      " FROM AttachmentFile  " +
                      " where AttachmentFileName = @fileName" +
                      " and ProjectId = @ProjectId" +
                      "  order by CreatedDate desc ";
            return await SqlMapper.QueryAsync<ProjectAttachmentFileDto>(Connection, sql, new { fileName = dto.AttachmentFileName , ProjectId = dto.ProjectId }, transaction: Transaction);
        }

       


        public async Task<int> InsertProjectAttachmentFileAsync(ProjectAttachmentFileDto dto)
        {
            var sql = " DECLARE @ID int;" +
                 " INSERT INTO [dbo].[AttachmentFile] " +
                    " ([ProjectId] " +
                    " ,[AttachmentFileName] " +
                    " ,[CreatedId] " +
                    " ,[CreatedDate]) " +
                     " VALUES " +
                    " (@ProjectId " +
                    " ,@AttachmentFileName " +
                    " ,@CreatedId " +
                    " ,GETDATE() )" +
              " SET @ID = SCOPE_IDENTITY(); " +
              " SELECT @ID";

            var id = await Connection.QuerySingleAsync<int>(sql, new
            {
                ProjectId = dto.ProjectId,
                AttachmentFileName = dto.AttachmentFileName,
                CreatedId = dto.CreatedId,
            }, Transaction);
            return id;
        }

        public async Task<int> DeleteProjectAttachmentFileAsync(long projectId)
        {
            var sql = " DELETE [dbo].[AttachmentFile] " +
                     " WHERE  ProjectId = @ProjectId";

            var x = await Connection.ExecuteAsync(sql, new
            {
                ProjectId = projectId
            }, Transaction);
            return x;
        }

        #endregion

        #region project Maintenance attach
        public async Task<IEnumerable<ProjectMaintenanceAttachmentFileDto>> GetProjectMaintenanceAttachmentFileNameAsync(ProjectMaintenanceAttachmentFileDto dto)
        {
            var sql = " SELECT  Id , ProjectId, AttachmentFileName , CreatedId , CreatedDate  " +
                      " FROM AttachmentFile  " +
                      " where AttachmentFileName = @fileName" +
                      " and ProjectMaintenanceId = @ProjectMaintenanceId" +
                      "  order by CreatedDate desc ";
            return await SqlMapper.QueryAsync<ProjectMaintenanceAttachmentFileDto>(Connection, sql, new { fileName = dto.AttachmentFileName, ProjectMaintenanceId = dto.ProjectMaintenanceId }, transaction: Transaction);
        }
        public async Task<IEnumerable<ProjectMaintenanceAttachmentFileDto>> GetProjectMaintenanceAttachmentFileAsync(long ProjectMaintenanceId)
        {
            var sql = " SELECT  Id , ProjectMaintenanceId, AttachmentFileName , CreatedId , CreatedDate  " +
                      " FROM AttachmentFile  " +
                      " where ProjectMaintenanceId = @ProjectMaintenanceId" +
                      "  order by CreatedDate desc ";
            return await SqlMapper.QueryAsync<ProjectMaintenanceAttachmentFileDto>(Connection, sql, new { ProjectMaintenanceId = ProjectMaintenanceId }, transaction: Transaction);
        }

        public async Task<int> InsertProjectMaintenanceAttachmentFileAsync(ProjectMaintenanceAttachmentFileDto dto)
        {
            var sql = " DECLARE @ID int;" +
                 " INSERT INTO [dbo].[AttachmentFile] " +
                    " ([ProjectMaintenanceId] " +
                    " ,[AttachmentFileName] " +
                    " ,[CreatedId] " +
                    " ,[CreatedDate]) " +
                     " VALUES " +
                    " (@ProjectMaintenanceId " +
                    " ,@AttachmentFileName " +
                    " ,@CreatedId " +
                    " ,GETDATE() )" +
              " SET @ID = SCOPE_IDENTITY(); " +
              " SELECT @ID";

            var id = await Connection.QuerySingleAsync<int>(sql, new
            {
                ProjectMaintenanceId = dto.ProjectMaintenanceId,
                AttachmentFileName = dto.AttachmentFileName,
                CreatedId = dto.CreatedId,
            }, Transaction);
            return id;
        }

        public async Task<int> DeleteProjectMaintenanceAttachmentFileAsync(long projectMaintenanceId)
        {
            var sql = " DELETE [dbo].[AttachmentFile] " +
                     " WHERE  ProjectMaintenanceId = @ProjectMaintenanceId";

            var x = await Connection.ExecuteAsync(sql, new
            {
                ProjectMaintenanceId = projectMaintenanceId
            }, Transaction);
            return x;
        }
        #endregion

        #region Employee attach
        public async Task<IEnumerable<EmployeeAttachmentFileDto>> GetEmployeeAttachmentFileNameAsync(EmployeeAttachmentFileDto dto)
        {
            var sql = " SELECT  Id , ProjectId, AttachmentFileName , CreatedId , CreatedDate  " +
                      " FROM AttachmentFile  " +
                      " where AttachmentFileName = @fileName" +
                      " and EmpId = @EmpId" +
                      "  order by CreatedDate desc ";
            return await SqlMapper.QueryAsync<EmployeeAttachmentFileDto>(Connection, sql, new { fileName = dto.AttachmentFileName, EmpId = dto.EmpId }, transaction: Transaction);
        }
        public async Task<IEnumerable<EmployeeAttachmentFileDto>> GetEmployeeAttachmentFileAsync(long EmpId)
        {
            var sql = " SELECT  Id , EmpId, AttachmentFileName , CreatedId , CreatedDate  " +
                      " FROM AttachmentFile  " +
                      "  where EmpId = @EmpId" +
                      "  order by CreatedDate desc ";
            return await SqlMapper.QueryAsync<EmployeeAttachmentFileDto>(Connection, sql, new { EmpId = EmpId }, transaction: Transaction);
        }

        public async Task<int> InsertEmployeeAttachmentFileAsync(EmployeeAttachmentFileDto dto)
        {
            var sql = " DECLARE @ID int;" +
                 " INSERT INTO [dbo].[AttachmentFile] " +
                    " ([EmpId] " +
                    " ,[AttachmentFileName] " +
                    " ,[CreatedId] " +
                    " ,[CreatedDate]) " +
                     " VALUES " +
                    " (@EmpId " +
                    " ,@AttachmentFileName " +
                    " ,@CreatedId " +
                    " ,GETDATE() )" +
              " SET @ID = SCOPE_IDENTITY(); " +
              " SELECT @ID";

            var id = await Connection.QuerySingleAsync<int>(sql, new
            {
                EmpId = dto.EmpId,
                AttachmentFileName = dto.AttachmentFileName,
                CreatedId = dto.CreatedId,
            }, Transaction);
            return id;
        }
        public async Task<int> DeleteEmployeeAttachmentFileAsync(long empId)
        {
            var sql = " DELETE [dbo].[AttachmentFile] " +
                     " WHERE  EmpId = @EmpId";

            var x = await Connection.ExecuteAsync(sql, new
            {
                EmpId = empId
            }, Transaction);
            return x;
        }
        #endregion
    }
}