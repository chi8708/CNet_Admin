using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CNet
{
    public enum DBType
    {
        SqlServer,
        MySql,
        SQLite
    }

    public class DBTypeConfig
    {
       private static string main=  AppConfigurtaionServices.Configuration.GetConnectionString("db_Main");
        public static DBType Main => (DBType)Enum.Parse(typeof(DBType), main, true);
    }

}
