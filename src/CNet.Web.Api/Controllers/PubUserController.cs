using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CNet.Web.Api.Model.Request;
using CNet.Model.Main;
using CNet.BLL.Main;
using CNet.Model;

namespace CNet.Web.Api.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("api/PubUser")]
    public class PubUserController : BaseController
    {
        private Pub_UserBLL bll = new Pub_UserBLL();
        private V_Pubuser_DeptBLL userDeptBLL = new V_Pubuser_DeptBLL();
        private Pub_UserroleBLL userRoleBLL = new Pub_UserroleBLL();
         Pub_UserfunctionBLL userFunctionBLL = new  Pub_UserfunctionBLL();

        [HttpGet,Route("GetAccess")]
        public dynamic GetAccess()
        {
            // var userCode = User.Identity.Name;

            //var c = (a:1,b:2);
            //return (
            //    access: new List<string>() { "super_admin", "admin" },
            //    avatar: "https://file.iviewui.com/dist/a0e88e83800f138b94d2414621bd9704.png",
            //    name: "super_admin",
            //    user_id:"1"
            //    );
            var user = new CNetUser(User);
            var userName = user.UserName;
            var userCode = user.UserCode;
            var access = user.Access;
            return new
            {
                access = access,
                avatar = "https://file.iviewui.com/dist/a0e88e83800f138b94d2414621bd9704.png",
                name = userName,
                user_id = userCode
            };
        }

        /// <summary>
        /// 获取用户分页
        /// </summary>
        /// <returns></returns>
        [Route("GetPage")]
        [HttpPost]
        public PageDateRes<V_Pubuser_Dept> GetPage([FromBody]PageDataReq pageReq)
        {
            var whereStr = GetWhereStr();
            if (whereStr=="-1")
            {
                return new PageDateRes<V_Pubuser_Dept>() {code=ResCode.Error,msg="查询参数有误！",data=null };
            }
            var users = userDeptBLL.GetPage(whereStr, (pageReq.field + " " + pageReq.order), pageReq.pageNum, pageReq.pageSize);

            //  PageDateRes<V_Pubuser_DeptExt> users = usersPage.MapTo<PageDateRes <V_Pubuser_Dept>,PageDateRes <V_Pubuser_DeptExt>>();
            var userCodes = string.Join("','", users.data.Select(p => p.UserCode));
            List<Pub_Userrole> userRoles = userRoleBLL.GetList($"userCode in ('{userCodes}')");
            users.data.ForEach(p =>
            {
                p.RoleCodes = userRoles.Where(c => c.UserCode == p.UserCode).Select(d => d.RoleCode);
            });

            return users;
        }

        private string GetWhereStr()
        {
            StringBuilder sb = new StringBuilder(" 1=1 ");
            sb.Append(" and StopFlag=0 ");
            var query = this.HttpContext.GetWhereStr();
            if (query=="-1")
            {
                return query;
            }
            sb.AppendFormat(" and {0} ", query);

            return sb.ToString();
        }


        /// <summary>
        /// 添加用户
        /// </summary>
        /// <returns></returns>
        [Route("Add")]
        [HttpPost]
        public DataRes<bool> Add([FromBody]V_Pubuser_Dept model)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var oldUser = bll.GetUserByUserName(model.UserName);
            if (oldUser!= null) 
            {
                //res.code = ResCode.NoValidate;
                //res.data = false;
                //res.msg ="用户名已存在，请修改！";
                //return res;
                return DataRes<bool>.NoValidate(false, "用户名已存在，请修改!");
            }

            model.Crdt = model.Lmdt = DateTime.Now;
            var user = new CNetUser(User);
            model.Crid = model.Lmid = $"{user.UserCode}-{user.UserName}";
            var r = bll.Add(model);
            if (!r.Item1)
            {
                //res.code = ResCode.Error;
                //res.data = false;
                //res.msg = r.Item2;
                return DataRes<bool>.Error();
            }

            return res;
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <returns></returns>
        [Route("Edit")]
        [HttpPost]
        public DataRes<bool> Edit([FromBody]V_Pubuser_Dept model)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var oldUser = bll.GetUserByUserName(model.UserName);
            if (oldUser != null&&oldUser.Id!=model.Id)
            {
                res.code = ResCode.NoValidate;
                res.data = false;
                res.msg = "用户名已存在，请修改！";
                return res;
            }

            model.Lmdt = DateTime.Now;
            var user = new CNetUser(User);
            model.Lmid = $"{user.UserCode}-{user.UserName}";
            var r = bll.Edit(model);
            if (!r.Item1)
            {
                res.code = ResCode.Error;
                res.data = false;
                res.msg = r.Item2;
            }

            return res;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        [Route("Delete/{id}")]
        [HttpPost]
        public DataRes<bool> Delete(long id)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var r = bll.ChangeSotpStatus($"id={id}", null);
            if (!r)
            {
                res.code = ResCode.Error;
                res.data = false;
                res.msg = "删除失败";
            }

            return res;
        }


        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <returns></returns>
        [Route("GetFunctions/{code}")]
        [HttpPost]
        public DataRes<IEnumerable<string>> GetFunctions(string code)
        {
            DataRes<IEnumerable<string>> res = new DataRes<IEnumerable<string>>() { code = ResCode.Success };

            var list = userFunctionBLL.GetList($"userCode='{code}'");
            res.data = list.Select(p=>p.FunctionCode);

            return res;
        }

        /// <summary>
        /// 保存用户权限
        /// </summary>
        /// <returns></returns>
        [Route("SaveFunctions/{code}")]
        [HttpPost]
        public DataRes<bool> SaveFunctions(string code, [FromBody]List<string> functions)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            List< Pub_Userfunction> list = new List< Pub_Userfunction>();
            functions.ForEach(p => { list.Add(new  Pub_Userfunction() { FunctionCode = p,UserCode = code }); });
            var r = bll.SaveFunctions(code, list);
            if (!r.Item1)
            {
                res.code = ResCode.Error;
                res.data = false;
                res.msg = "保存失败";
            }

            return res;
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>

        [HttpPost, Route("Logout")]
        public dynamic Logout()
        {
            //User = null;
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            return res;
        }


        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <returns></returns>
        [Route("EditPassword")]
        [HttpPost]
        public DataRes<bool> EditPassword([FromBody] EditPasswordReq model)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };
            var user = ThisUser;

            if (user == null)
            {
                res.code = ResCode.NoValidate;
                res.data = false;
                res.msg = "用户未登陆";

                return res;
            }
            var oldModel = bll.Get(user.Id);

            if (oldModel.UserPwd != model.OldPassword)
            {
                res.code = ResCode.NoValidate;
                res.data = false;
                res.msg = "原密码不正确";

                return res;
            }

            bll.EditPassword(user.Id, model.Password, user.UserCode + "-" + user.UserName);
            if (user == null)
            {
                res.code = ResCode.Error;
                res.data = false;
                res.msg = "保存失败";
                return res;
            }


            return res;
        }


    }
}