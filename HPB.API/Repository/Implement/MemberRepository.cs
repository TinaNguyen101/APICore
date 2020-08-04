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
    internal class MemberRepository : RepositoryBase, IMemberRepository
    {

        public MemberRepository()
        {

        }

        public async Task<int> DeleteMemberAsync(int id)
        {
            var sql = " DELETE [dbo].[Member] " +
                     " WHERE  Id = @Id";


            var x = await Connection.ExecuteAsync(sql, new
            {
                Id = id
            }, Transaction);
            return x;
        }



        #region ProjectMember

        public async Task<int> CheckprojectExitMemberPosition(long projectId, long empId, long positionId)
        {
            var sql = "SELECT count(*) " +
                      " FROM Member " +
                      " where ProjectId = @projectId AND EmpId = @empId AND ProjectPositionId = @positionId";
            return await SqlMapper.ExecuteScalarAsync<int>(Connection, sql, new { projectId = projectId, empId = empId, positionId = positionId }, transaction: Transaction);
        }
        public async Task<IEnumerable<ProjectMemberDto>> GetProjectMemberAsync(int projectId,string pathFolderImage)
        {
            var sql = "SELECT Member.Id, ProjectId,EmpId,EmpName,EmpMobile1,EmpEmail1,'"+ pathFolderImage + "'+ EmpImage as EmpImage,EmpGender,ProjectPositionId,ProjectPositionName,StyleCss " +
                      " FROM Member " +
                      " LEFT JOIN  MstProjectPosition on Member.ProjectPositionId = MstProjectPosition.Id " +
                      " LEFT JOIN  Employee on Member.EmpId = Employee.Id " +
                      " where ProjectId = @ProjectId" +
                      " order by ProjectPositionId ";
            return await SqlMapper.QueryAsync<ProjectMemberDto>(Connection, sql, new { ProjectId = projectId }, transaction: Transaction);
        }

        public async Task<ProjectMemberDto> GetProjectMemberByIdAsync(int id)
        { 
            var sql = "SELECT Member.Id, ProjectId,EmpId,EmpName,EmpMobile1,EmpEmail1,EmpImage,EmpGender,ProjectPositionId,ProjectPositionName,StyleCss " +
                      " FROM Member " +
                      " LEFT JOIN  MstProjectPosition on Member.ProjectPositionId = MstProjectPosition.Id " +
                      " LEFT JOIN  Employee on Member.EmpId = Employee.Id " +
                      " where Member.Id = @Id";
            return await SqlMapper.QueryFirstOrDefaultAsync<ProjectMemberDto>(Connection, sql, new { Id = id }, transaction: Transaction);
        }
        public async Task<int> InsertMultiProjectMemberAsync(List<ProjectMemberInsertDto> dtolst)
        {
            var sql = " INSERT INTO [dbo].[Member] " +
                    " ([ProjectId] " +
                    " ,[EmpId] " +
                    " ,[ProjectPositionId]) " +
                     " VALUES " +
                    " (@ProjectId " +
                    " ,@EmpId" +
                    " ,@ProjectPositionId )";

            int count = await Connection.ExecuteAsync(sql, dtolst, Transaction);
            return count;
        }

        public async Task<int> InsertProjectMemberAsync(ProjectMemberDto dto)
        {
            var sql = " INSERT INTO [dbo].[Member] " +
                    " ([ProjectId] " +
                    " ,[EmpId] " +
                    " ,[ProjectPositionId]) " +
                     " VALUES " +
                    " (@ProjectId " +
                    " ,@EmpId" +
                    " ,@ProjectPositionId )";

            int count = await Connection.ExecuteAsync(sql, new
            {
                ProjectId = dto.ProjectId,
                EmpId = dto.EmpId,
                ProjectPositionId = dto.ProjectPositionId
            }, Transaction);
            return count;
        }


        public async Task<int> UpdateProjectMemberAsync(ProjectMemberDto dto)
        {
            var sql = " UPDATE [dbo].[Member] " +
                     "  SET [EmpId] = @EmpId " +
                     "     ,[ProjectPositionId] = @ProjectPositionId " +
                     "  WHERE  Member.Id = @Id";

            var x = await Connection.ExecuteAsync(sql, new
            {
                Id = dto.Id,
                EmpId = dto.EmpId,
                ProjectPositionId = dto.ProjectPositionId,
            }, Transaction);
            return x;
        }
        public async Task<int> DeleteProjectMemberByProjectIDAsync(int projectID)
        {
            var sql = " DELETE [dbo].[Member] " +
                     " WHERE  ProjectID = @ProjectID";

            var x = await Connection.ExecuteAsync(sql, new
            {
                ProjectID = projectID,
            }, Transaction);
            return x;
        }
        #endregion

        #region ProjectMaintenanceMember
        public async Task<IEnumerable<ProjectMaintenanceMemberDto>> GetProjectMaintenanceMemberAsync(int projectId,string pathFolderImage)
        {
            var sql = "SELECT Member.Id, ProjectMaintenanceId,EmpId,'" + pathFolderImage + "'+EmpImage as EmpImage,EmpMobile1,EmpEmail1,EmpName,EmpGender,ProjectPositionId,ProjectPositionName,StyleCss " +
                      " FROM Member " +
                      " LEFT JOIN  MstProjectPosition on Member.ProjectPositionId = MstProjectPosition.Id " +
                      " LEFT JOIN  Employee on Member.EmpId = Employee.Id " +
                      " where ProjectMaintenanceId = @ProjectId" +
                      " order by ProjectPositionId";
            return await SqlMapper.QueryAsync<ProjectMaintenanceMemberDto>(Connection, sql, new { ProjectId = projectId }, transaction: Transaction);
        }

        public async Task<ProjectMaintenanceMemberDto> GetProjectMaintenanceMemberByIdAsync(int id)
        {
            var sql = "SELECT Member.Id, ProjectMaintenanceId,EmpId,EmpName,EmpMobile1,EmpEmail1,EmpImage,EmpGender,ProjectPositionId,ProjectPositionName,StyleCss " +
                      " FROM Member " +
                      " LEFT JOIN  MstProjectPosition on Member.ProjectPositionId = MstProjectPosition.Id " +
                      " LEFT JOIN  Employee on Member.EmpId = Employee.Id " +
                      " where Member.Id = @Id";
            return await SqlMapper.QueryFirstOrDefaultAsync<ProjectMaintenanceMemberDto>(Connection, sql, new { Id = id }, transaction: Transaction);
        }

        public async Task<int> CheckprojectMaintenanceExitMemberPosition(long projectMaintenanceId, long empId, long positionId)
        {
            var sql = "SELECT count(*) " +
                      " FROM Member " +
                      " where ProjectMaintenanceId = @projectMaintenanceId AND EmpId = @empId AND ProjectPositionId = @positionId";
            return await SqlMapper.ExecuteScalarAsync<int>(Connection, sql, new { projectMaintenanceId = projectMaintenanceId, empId = empId, positionId= positionId }, transaction: Transaction);
        }
        public async Task<int> InsertMultiProjectMaintenanceMemberAsync(List<ProjectMaintenanceMemberInsertDto> dtolst)
        {
            var sql = " INSERT INTO [dbo].[Member] " +
                    " ([ProjectMaintenanceId] " +
                    " ,[EmpId] " +
                    " ,[ProjectPositionId]) " +
                     " VALUES " +
                    " (@ProjectMaintenanceId " +
                    " ,@EmpId" +
                    " ,@ProjectPositionId )";

            int count = await Connection.ExecuteAsync(sql, dtolst, Transaction);
            return count;
        }

        public async Task<int> InsertProjectMaintenanceMemberAsync(ProjectMaintenanceMemberDto dto)
        {
            var sql = " INSERT INTO [dbo].[Member] " +
                    " ([ProjectMaintenanceId] " +
                    " ,[EmpId] " +
                    " ,[ProjectPositionId]) " +
                     " VALUES " +
                    " (@ProjectMaintenanceId " +
                    " ,@EmpId" +
                    " ,@ProjectPositionId )";

            int count = await Connection.ExecuteAsync(sql, new
            {
                ProjectMaintenanceId = dto.ProjectMaintenanceId,
                EmpId = dto.EmpId,
                ProjectPositionId = dto.ProjectPositionId
            }, Transaction);
            return count;
        }

        public async Task<int> UpdateProjectMaintenanceMemberAsync(ProjectMaintenanceMemberDto dto)
        {
            var sql = " UPDATE [dbo].[Member] " +
                     "  SET [EmpId] = @EmpId " +
                     "     ,[ProjectPositionId] = @ProjectPositionId " +
                     "  WHERE  Member.Id = @Id";

            var x = await Connection.ExecuteAsync(sql, new
            {
                Id = dto.Id,
                EmpId = dto.EmpId,
                ProjectPositionId = dto.ProjectPositionId,
            }, Transaction);
            return x;
        }

        public async Task<int> DeleteProjectMaintenanceMemberByProjectMaintenanceIDAsync(int projectMaintenanceID)
        {
            var sql = " DELETE [dbo].[Member] " +
                     " WHERE  ProjectMaintenanceId = @ProjectMaintenanceId";

            var x = await Connection.ExecuteAsync(sql, new
            {
                ProjectMaintenanceId = projectMaintenanceID,
            }, Transaction);
            return x;
        }
        #endregion









    }
}