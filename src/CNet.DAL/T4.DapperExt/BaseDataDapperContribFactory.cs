using CNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNet.DAL
{
    public class BaseDataDapperContribFactory<T> where T : class, new()
    {
        private static IBaseDataDapperContrib<T> iBaseDataDapperContrib;
        public static IBaseDataDapperContrib<T> GetInstance() 
        {
            if (iBaseDataDapperContrib!=null)
            {
                return iBaseDataDapperContrib;
            }
            var defatultDBType = DBTypeConfig.DefatultDBType;
            switch (defatultDBType)
            {
                case DBType.SqlServer:
                    iBaseDataDapperContrib = new BaseDataDapperContrib<T>();
                    break;
                case DBType.MySql:
                    iBaseDataDapperContrib = new BaseDataDapperContribMySql<T>();
                    break;
                default:
                    break;
            }

            return iBaseDataDapperContrib;

        }
    }
}
