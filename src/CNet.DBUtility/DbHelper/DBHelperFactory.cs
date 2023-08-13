using CNet.Common;
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
			return new DapperHelperSqlServer(Connection.MainStr);
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
