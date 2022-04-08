using CNet.DAL;
using CNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CNet.BLL
{
   public partial class Pub_FunctionBLL
    {
       Pub_FunctionDAL dal = new Pub_FunctionDAL();
        /// <summary>
        /// 获取编号
        /// </summary>
        /// <param name="pCode"></param>
        /// <returns></returns>
        public string GetCode(string parentCode)
        {
            var code = "FC001";
            List<Pub_Function> list = GetList("ParentCode='" + parentCode + "'", " FunctionCode Desc ", 1);
            if (list.Count > 0)
            {
                var model = list.First();
                var lastNum = model.FunctionCode.Substring(model.FunctionCode.Length - 3, 3);
                code = parentCode + ((Convert.ToInt32(lastNum) + 1).ToString().PadLeft(3, '0'));
            }
            else
            {
                code = parentCode + "001";
            }

            return code;
        }

        /// <summary>
        /// 获取用户权限列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetUserAccess(string userCode)
        {
            return dal.GetUserAccess(userCode);
        }

       /// <summary>
       /// 获取用户菜单
       /// </summary>
       /// <returns></returns>
        public List<Pub_Function> GetMenu(string userCode) 
        {
            return dal.GetMenu(userCode);
        }
    }
}
