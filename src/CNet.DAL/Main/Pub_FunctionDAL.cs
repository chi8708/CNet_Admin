using CNet.Model.Main;
using NPOI.SS.Formula.Functions;
using NPOI.Util;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNet.DAL.Main
{
    public partial class  Pub_FunctionDAL
	{
        /// <summary>
        /// 获取用户菜单
        /// </summary>
        /// <returns></returns>
        public List<Pub_Function> GetMenu(string userCode)
        {
			//sqlserver 将WITH recursive f 修改为 WITH f 。mysql需要加recursive
			string sql = $@"WITH recursive f -- recursive 加了就可以，否则报错Table 'cnet.f' doesn't exist
	                        AS(

		                        SELECT pf.* FROM Pub_Function AS pf
		                        WHERE pf.StopFlag = 0 AND  EXISTS(SELECT prf.Id FROM  Pub_RoleFunction prf WHERE pf.FunctionCode = prf.FunctionCode AND
		                         prf.RoleCode IN(SELECT pur.RoleCode FROM Pub_UserRole AS pur WHERE pur.UserCode =@userCode))
		                        UNION
		                        SELECT pf.* FROM Pub_Function AS pf
		                        WHERE pf.StopFlag = 0 AND EXISTS(SELECT puf.Id FROM Pub_UserFunction AS puf WHERE pf.FunctionCode = puf.FunctionCode AND puf.UserCode = @userCode)
		                        UNION ALL

		                        SELECT  a.* from Pub_Function as a inner join f as b on b.parentCode = a.FunctionCode
			                        )/*递归 f前后不能加;*/
	                        SELECT DISTINCT  *FROM f WHERE MenuFlag = 1 ORDER BY FunctionCode;";

            return DBHelperFactory.GetInstance_Main().Query<Pub_Function>(sql, new { userCode });
            //return DapperHelperFactory.GetInstance_Main().Query<Pub_Function>("P_GetMenu", new { userCode = userCode }, commandType: System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// 获取用户权限列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetUserAccess(string userCode)
        {
            string sql = string.Format(@"select DISTINCT FunctionCode from (
		        SELECT pf.* FROM Pub_Function AS pf
		        WHERE pf.StopFlag=0 AND  EXISTS(SELECT prf.Id FROM  Pub_RoleFunction prf WHERE pf.FunctionCode= prf.FunctionCode AND
		         prf.RoleCode IN(SELECT pur.RoleCode FROM Pub_UserRole AS pur WHERE pur.UserCode=@userCode ) )
		        UNION 
		        SELECT pf.* FROM Pub_Function AS pf
		        WHERE pf.StopFlag=0 AND EXISTS(SELECT puf.Id FROM Pub_UserFunction AS puf WHERE pf.FunctionCode=puf.FunctionCode AND puf.UserCode=@userCode)
	          ) t where t.StopFlag=0;");

            return DBHelperFactory.GetInstance_Main().Query<string>(sql, new { userCode });
            //return DapperHelperFactory.GetInstance_Main().Query<string>("P_GetUserAccess", new { userCode = userCode }, commandType: System.Data.CommandType.StoredProcedure);
        }


        /// <summary>
        /// 获取自己或子级
        /// </summary>
        /// <returns></returns>
        public List<V_Pubfunction_Parent> GetChildFunction(string code)
        {
			//sqlserver 将WITH recursive f 修改为 WITH f。mysql需要加recursive
			string sql = $@"with recursive f as 
	                        (
	                        select * FROM Pub_Function AS pd where FunctionCode=@code
	                        union all
	                        select a.* from Pub_Function as a inner join f as b on a.ParentCode=b.FunctionCode
	                        )

	                        SELECT f.*,a.FunctionChina as parentName FROM  f  LEFT JOIN Pub_Function as a on  f.ParentCode=a.functionCode
	                        ";

            return DBHelperFactory.GetInstance_Main().Query<V_Pubfunction_Parent>(sql, new { code });
            //return DapperHelperFactory.GetInstance_Main().Query<V_Pubfunction_Parent>("p_SearchChildFunction", new { functionCodeIn = code }, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
