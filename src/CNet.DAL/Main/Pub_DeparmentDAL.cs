using CNet.Main.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CNet.Main.DAL
{
    public partial class Pub_DepartmentDAL
	{
		public List<V_Pubdept_Parent> SearchChildDept(string code = "D000001")
		{
			////sqlserver 将WITH recursive f 修改为 WITH f
			string sql = $@"with recursive f as 
	                        (
	                        select * FROM Pub_Department AS pd where DeptCode=@code
	                        union all
	                        select a.* from Pub_Department as a inner join f as b on a.ParentCode=b.DeptCode
	                        )

	                        SELECT f.*,a.deptName as parentName FROM  f  LEFT JOIN Pub_Department as a on  f.ParentCode=a.DeptCode
	                        ";

			return DBHelperFactory.GetInstance_Main().Query<V_Pubdept_Parent>(sql, new { code });
			//return DBHelperFactory.GetInstance_Main().Query<V_Pubdept_Parent>("p_SearchChildDept", new { deptCodeIn = code }, commandType: System.Data.CommandType.StoredProcedure);
		}
	}
}
