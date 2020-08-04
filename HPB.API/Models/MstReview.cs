using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class MstReview
    {
        public MstReview()
        {
            AnnualReview = new HashSet<AnnualReview>();
        }

        public int Id { get; set; }
        public string ReviewHeading { get; set; }

        public virtual ICollection<AnnualReview> AnnualReview { get; set; }
    }
}
