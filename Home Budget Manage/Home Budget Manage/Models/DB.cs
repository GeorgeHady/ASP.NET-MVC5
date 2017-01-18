using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Home_Budget_Manage.Models
{
    public class DB: DbContext
    {
        public DB(): base("MyConnection") { }
        public DbSet<Users> table_users { get; set; }
        public DbSet<FinancialAccounts> table_FinancialAccounts { get; set; }
        public DbSet<Permissions> table_Permissions { get; set; }
        public DbSet<AccountingEntries> table_AccountingEntries { get; set; }

        //public System.Data.Entity.DbSet<Home_Budget_Manage.Models.AccountsSummary> AccountsSummaries { get; set; }
    }
}