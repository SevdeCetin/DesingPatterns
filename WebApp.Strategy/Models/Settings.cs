using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Strategy.Models
{
    public class Settings
    {
        public static string claimDatabaseType = "databaseType";
        public EDatabaseType DatabaseType;
        //default olarak sqlserver olarak belirliyoruz
        public EDatabaseType GetDefaultDatabaseType => EDatabaseType.SqlServer;
    }
}
