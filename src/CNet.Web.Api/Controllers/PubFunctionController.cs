using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CNet.Main.BLL;
using CNet.Main.Model;
using CNet.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CNet.Web.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/PubFunction")]
    public class PubFunctionController : BaseController
    {
        Pub_FunctionBLL bll = new Pub_FunctionBLL();
        V_Pubfunction_ParentBLL functionParentBLL = new V_Pubfunction_ParentBLL();
        [HttpPost]
        [Route("GetList")]
        public DataRes<List<Pub_Function>> GetList()
        {
            var depts = bll.GetList("StopFlag=0", " sortidx asc");

            return new DataRes<List<Pub_Function>>() { data = depts };
        }

        /// <summary>
        /// 获取子权限列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetChildList")]
        [Route("GetChildList/{code}")]
        public DataRes<List<V_Pubfunction_Parent>> GetChildList(string code = "FC001")
        {

            // var depts = functionParentBLL.GetList(string.Format(" StopFlag=0 And FunctionCode IN (Select FunctionCode From f_SearchChildFunction('{0}'))", code), " FunctionCode ");
            var depts = bll.GetChildFunction(code);
            return new DataRes<List<V_Pubfunction_Parent>>() { data = depts };
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [Route("Add")]
        [HttpPost]
        public DataRes<bool> Add([FromBody]Pub_Function model)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            model.FunctionCode = bll.GetCode(model.ParentCode);
            model.editdate = DateTime.Now;
            var user = User.GetCNetUser();
            model.editor = string.Format("{0}-{1}", user.UserCode, user.UserName);
            model.StopFlag = false;
            var r = bll.Insert(model);
            return res;
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        [Route("Edit")]
        [HttpPost]
        public DataRes<bool> Edit([FromBody]Pub_Function model)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            model.editdate = DateTime.Now;
            var user = User.GetCNetUser();
            model.editor = string.Format("{0}-{1}", user.UserCode, user.UserName);
            var r = bll.Update(model);
            if (!r)
            {
                res.code = ResCode.Error;
                res.data = false;
                res.msg = "保存失败";
            }

            return res;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [Route("Delete/{id}")]
        [HttpPost]
        public DataRes<bool> Delete(string id)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var r = bll.ChangeSotpStatus("FunctionCode='" + id + "'");
            if (!r)
            {
                res.code = ResCode.Error;
                res.data = false;
                res.msg = "删除失败";
            }

            return res;
        }

        /// <summary>
        /// 获取左侧菜单
        /// </summary>
        /// <returns></returns>
        [Route("GetMenu")]
        [HttpPost]
        //[AllowAnonymous]//添加这个属性后获取不到User
        public DataRes<List<Menu>> GetMenu()
        {
            List<Menu> menus = new List<Menu>();

            DataRes<List<Menu>> res = new DataRes<List<Menu>>() { code = ResCode.Success, data = menus };
            List<Pub_Function> functions = bll.GetMenu(User.GetCNetUser().UserCode);
            List<Pub_Function> functionsRoot = functions.Where(p=>p.FunctionCode.Length==5).ToList();
            foreach (var item in functionsRoot)
            {
                Menu menu = GetMenuItem(item);
                var children=  GetMenuChild(functions, item.FunctionCode);
                if (children != null && children.Count > 0)
                {
                    menu.children = children;
                }
                menus.Add(menu);
            }

            return res;
        }

        /// <summary>
        /// 菜单明细项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>

        private Menu GetMenuItem(Pub_Function item) 
        {
            Menu menu = new Menu();
            menu.path = item.RouterPath;
            menu.name = item.FunctionEnglish;
            menu.meta = new Menu.Meta()
            {
                icon = item.MenuIcon,
                notCache = !item.IsCache.Value,
                title = item.FunctionChina,
                access=new List<string>(){item.FunctionCode}
            };
            menu.component = item.URLString;
            return menu;
        }

        /// <summary>
        /// 菜单子项
        /// </summary>
        /// <param name="functions"></param>
        /// <param name="parentCode"></param>
        /// <returns></returns>
        private List<Menu> GetMenuChild(List<Pub_Function> functions, string parentCode) 
        {
            List<Menu> menus = new List<Menu>();
            var childrenFun = functions.Where(p => p.ParentCode == parentCode);
            if (childrenFun == null || childrenFun.Count() <= 0)
            {
                return menus;
            }

            foreach (var item in childrenFun)
            {
                Menu menu = GetMenuItem(item);
                var children = GetMenuChild(functions, item.FunctionCode);
                if (children!=null&&children.Count>0)
                {
                    menu.children = children;
                }

                menus.Add(menu);
            }

            return menus;

        }

        public class Menu
        {
            /// <summary>
            /// 路由路径
            /// </summary>
            public string path { get; set; }

            /// <summary>
            /// 名称
            /// </summary>
            public string name { get; set; }

            public Meta meta { get; set; }

            /// <summary>
            /// 子组件
            /// </summary>
            public List<Menu> children { get; set; }

            public class Meta
            {
                /// <summary>
                /// 图标
                /// </summary>
                public string icon { get; set; }

                /// <summary>
                /// 不缓存 
                /// </summary>
                public bool? notCache { get; set; }

                /// <summary>
                /// 标题
                /// </summary>
                public string title { get; set; }

                /// <summary>
                /// 权限列表
                /// </summary>
                public List<string> access { get; set; }
            }

            public string component { get; set; }

        }

    }
}