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
            return DapperHelperFactory.GetInstance().Query<Pub_Function>("P_GetMenu", new { userCode = userCode }, commandType: System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// 获取用户权限列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetUserAccess(string userCode)
        {
            return DapperHelperFactory.GetInstance().Query<string>("P_GetUserAccess", new { userCode = userCode }, commandType: System.Data.CommandType.StoredProcedure);
        }


        /// <summary>
        /// 获取自己或子级
        /// </summary>
        /// <returns></returns>
        public List<V_PubFunction_Parent> GetChildFunction(string code)
        {
            return DapperHelperFactory.GetInstance().Query<V_PubFunction_Parent>("p_SearchChildFunction", new { functionCodeIn = code }, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
