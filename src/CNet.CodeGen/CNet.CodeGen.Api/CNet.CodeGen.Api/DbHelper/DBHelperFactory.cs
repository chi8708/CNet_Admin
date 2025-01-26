using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNet.CodeGen.DB
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

    }
}
