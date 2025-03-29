using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNet
{
    public class DBHelperFactory
    {
        private static IDapperHelper iDapperHelper;
        public static IDapperHelper GetInstance_Main()
        {
            return new DapperHelperMySql(Connection.MainStr);
            //return new DapperHelperSQLite(Connection.MainStr);
			if (iDapperHelper!=null)
            {
                return iDapperHelper;
            }

            var defatultDBType = DBTypeConfig.Main;
            switch (defatultDBType)
            {
                case DBType.SqlServer:
                    iDapperHelper = new DapperHelperSqlServer(Connection.MainStr);
                    break;
                case DBType.MySql:
                    iDapperHelper = new DapperHelperMySql(Connection.MainStr);
                    break;
                default:
                    break;
            }
            return iDapperHelper;
        }

        public static List<dynamic> GetTables_Main()
        {
            string sql = string.Format(@"SELECT a.TABLE_SCHEMA schemname,a.TABLE_NAME tablename,a.TABLE_ROWS `rows`,CAST(!ISNULL(b.HasPrimaryKey) AS SIGNED) HasPrimaryKey
                        FROM information_schema.`TABLES` AS a
                        LEFT JOIN (
	                        SELECT TABLE_NAME, COUNT(COLUMN_NAME) AS HasPrimaryKey
	                        FROM information_schema.key_column_usage
	                        WHERE CONSTRAINT_NAME = 'PRIMARY' AND TABLE_SCHEMA = '{0}' GROUP BY TABLE_NAME
                        ) AS b ON a.TABLE_NAME = b.TABLE_NAME
                        WHERE a.TABLE_SCHEMA = '{0}'", "cnet");


            return GetInstance_Main().Query<dynamic>(sql);


        }

    }
}
