using CNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNet
{
    public class DapperHelperFactory
    {
        private static IDapperHelper iDapperHelper;
        public static IDapperHelper GetInstance()
        {
            if (iDapperHelper!=null)
            {
                return iDapperHelper;
            }

            var defatultDBType = DBTypeConfig.DefatultDBType;
            switch (defatultDBType)
            {
                case DBType.SqlServer:
                    iDapperHelper = new DapperHelper();
                    break;
                case DBType.MySql:
                    iDapperHelper = new DapperHelperMySql();
                    break;
                default:
                    break;
            }
            return iDapperHelper;
        }
    }
}
