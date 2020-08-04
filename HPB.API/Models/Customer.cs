using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Project = new HashSet<Project>();
        }

        public int Id { get; set; }
        public string CustName { get; set; }
        public string CustShortName { get; set; }
        public string CustEngName { get; set; }
        public string CustContactName { get; set; }
        public string CustContactEmail { get; set; }
        public string CustContactPhone { get; set; }
        public string CustContactFax { get; set; }
        public string CustContactSkype { get; set; }
        public string CustAddress { get; set; }
        public string CustWebsite { get; set; }
        public string CustPostCode { get; set; }
        public string CustComment { get; set; }
        public bool? CustIsDelete { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CustStyleCss { get; set; }

        public virtual ICollection<Project> Project { get; set; }
    }
}
