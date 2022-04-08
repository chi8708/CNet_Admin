using CNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNet.DBUtility;

namespace CNet.DAL
{
    public partial class  Pub_FunctionDAL
    {
        /// <summary>
        /// 获取用户菜单
        /// </summary>
        /// <returns></returns>
        public List<Pub_Function> GetMenu(string userCode)
        {
            return DapperHelper.Query<Pub_Function>("P_GetMenu", new { userCode = userCode }, commandType: System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// 获取用户权限列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetUserAccess(string userCode)
        {
            return DapperHelper.Query<string>("P_GetUserAccess", new { userCode = userCode }, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
