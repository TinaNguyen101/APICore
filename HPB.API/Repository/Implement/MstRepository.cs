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
    internal class MstRepository : RepositoryBase, IMstRepository
    {
        public MstRepository()
        {

        }

        public async Task<dynamic> GetAllMstAsync()
        {
            var sql1 = " SELECT * FROM MstCostCurrency ";
            var sql2 = " SELECT * FROM MstEmplomentStatus";
            var sql3 = " SELECT * FROM MstPosition";
            var sql4 = " SELECT * FROM MstProjectPosition";
            var sql5 = " SELECT * FROM MstProjectStatus";
            var multi = await SqlMapper.QueryMultipleAsync(Connection, sql1 + ";" + sql2 + ";" + sql3 + ";" + sql4 + ";" + sql5 ,  transaction: Transaction);
            var _MstCostCurrency = multi.Read<MstCostCurrency>().ToList();
            var _MstEmplomentStatus = multi.Read<MstEmplomentStatus>().ToList();
            var _MstPosition = multi.Read<MstPosition>().ToList();
            var _MstProjectPosition = multi.Read<MstProjectPosition>().ToList();
            var _MstProjectStatus = multi.Read<MstProjectStatus>().ToList();
            return new {
                MstCostCurrency = _MstCostCurrency,
                MstEmplomentStatus = _MstEmplomentStatus,
                MstPosition = _MstPosition,
                MstProjectPosition = _MstProjectPosition,
                MstProjectStatus = _MstProjectStatus
            };
        }

        public async Task<dynamic> GetMstByNameAsync(string mstName)
        {
            var sql1 = " SELECT * FROM " + mstName;
         
            var data = await SqlMapper.QueryAsync(Connection, sql1, transaction: Transaction);
            return new
            {
                mst = data.ToList()
            };
        }

    }
}