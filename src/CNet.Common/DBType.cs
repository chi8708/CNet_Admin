using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNet.Common;
using Microsoft.Extensions.Configuration;

namespace CNet
{
    public enum DBType
    {
        SqlServer,
        MySql,
    }

    public class DBTypeConfig
    {
       private static string defaultDB=  AppConfigurtaionServices.Configuration.GetConnectionString("defaultDB");
        public static DBType DefatultDBType =>(DBType)Enum.Parse(typeof(DBType), defaultDB, true);
    }

}
