using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class MonthlyOTListDto
    {

        public int Id { get; set; }
        public int ProID { get; set; }
        public string ProName { get; set; }
        public DateTime? Otdate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public decimal? Wage { get; set; }
        public int? EmpId { get; set; }

        public string EmpName { get; set; }

    }
}
