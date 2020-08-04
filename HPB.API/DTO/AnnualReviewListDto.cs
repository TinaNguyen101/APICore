using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
   
    public class AnnualReviewListDto
    {
        public int? EmpId { get; set; }
        public int? Year { get; set; }

        public int? ReviewId { get; set; }


        public string ReviewHeading { get; set; }
        public string ReviewContent { get; set; }

    }

}
