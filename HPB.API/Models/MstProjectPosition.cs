using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class MstProjectPosition
    {
        public MstProjectPosition()
        {
            Member = new HashSet<Member>();
        }

        public int Id { get; set; }
        public string ProjectPositionName { get; set; }
        public string StyleCss { get; set; }

        public virtual ICollection<Member> Member { get; set; }
    }
}
