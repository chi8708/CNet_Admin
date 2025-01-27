﻿using CNet.DAL.Main;
using CNet.Model.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNet.BLL.Main
{

    public partial class Pub_RoleBLL
    {
        Pub_RolefunctionBLL roleFunctionBLL = new Pub_RolefunctionBLL();
        /// <summary>
        /// 获取部门编号
        /// </summary>
        /// <param name="pCode"></param>
        /// <returns></returns>
        public string GetCode()
        {
            var code = "RC000001";
            List<Pub_Role> roles = GetList("", " Id Desc ", 1);
            if (roles.Count > 0)
            {
                var model = roles.First();
                code ="RC"+ (Convert.ToInt32(model.RoleCode.Remove(0, 2)) + 1).ToString().PadLeft(6, '0');
            }

            return code;
        }

        public  Tuple<bool, string> SaveFunctions( string code, List<Pub_Rolefunction> functions)
        {
            var r= roleFunctionBLL.DeleteByWhere("RoleCode='"+code+"'");
                r= roleFunctionBLL.InsertBatch(functions);

            return  Tuple.Create(r, r?"保存成功":"保存失败");

        }
    }
}
