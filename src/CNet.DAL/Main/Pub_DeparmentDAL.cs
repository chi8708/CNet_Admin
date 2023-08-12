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
			return DapperHelperFactory.GetInstance_Main().Query<V_Pubdept_Parent>("p_SearchChildDept", new { deptCodeIn = code }, commandType: System.Data.CommandType.StoredProcedure);
		}
	}
}
