using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HPB.API.Repositories
{
    public abstract class RepositoryBase
    {
        public IDbTransaction Transaction { get;  set; }
        protected IDbConnection Connection { get { return Transaction.Connection; } }


        public RepositoryBase()
        {
        }
             
    }
}