
//////此代码有CNetCodeGen生成， 作者：cts 生成时间：2025-01-27 15:35:33
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using CNet.Model;
    using CNet.Model.Main;
    using CNet.BLL.Main;

    namespace  CNet.Web.Api.Controllers
    {
        [Produces("application/json")]
        [Route("api/Pub_Role")]
        [Authorize]
        public class Pub_RoleController : BaseController
        {
            Pub_RoleBLL bll = new Pub_RoleBLL();
            /// <summary>
            /// 获取所有
            ///</summary>
            [Route("GetList")]
            [HttpPost]
            public DataRes<List<Pub_Role>> GetList()
            {
                var roles = bll.GetList("");

                return new DataRes<List<Pub_Role>> { data= roles} ;
            }

              /// <summary>
            /// 获取详细
            /// </summary>
            /// <returns></returns>
            [Route("Get/{id}")]
            [HttpPost]
            public DataRes<Pub_Role> Get(long id)
            {
                DataRes<Pub_Role> res = new DataRes<Pub_Role>() { code = ResCode.Success, data = null };

                var model = bll.Get(id);
                res.data = model;

                return res;
            }

            /// <summary>
            /// 获取分页
            /// </summary>
            /// <returns></returns>
            [Route("GetPage")]
            [HttpPost]
            public PageDateRes<Pub_Role> GetPage([FromBody]PageDataReq pageReq)
            {
                var whereStr = GetWhereStr();
                if (whereStr == "-1")
                {
                    return new PageDateRes<Pub_Role>() { code = ResCode.Error, msg = "查询参数有误！", data = null };
                }
                var list = bll.GetPage(whereStr, (pageReq.field + " " + pageReq.order), pageReq.pageNum, pageReq.pageSize);

                return list;
            }

            private string GetWhereStr()
            {
                StringBuilder sb = new StringBuilder(" 1=1 ");
                sb.Append(" and StopFlag=0 ");
                var query = this.HttpContext.GetWhereStr();
                if (query == "-1")
                {
                    return query;
                }
                sb.AppendFormat(" and {0} ", query);

                return sb.ToString();
            }


            /// <summary>
            /// 添加角色
            /// </summary>
            /// <returns></returns>
            [Route("Add")]
            [HttpPost]
            public DataRes<bool> Add([FromBody]Pub_Role model)
            {
                DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

                var r = bll.Insert(model)>0;
                if (!r)
                {
                    res.code = ResCode.Error;
                    res.msg = "保存失败！";
                }

                return res;
            }

            /// <summary>
            /// 编辑
            /// </summary>
            /// <returns></returns>
            [Route("Edit")]
            [HttpPost]
            public DataRes<bool> Edit([FromBody]Pub_Role model)
            {
                DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

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
            public DataRes<bool> Delete(long id)
            {
                DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

                var r = bll.ChangeSotpStatus($"id={id}");
                if (!r)
                {
                    res.code = ResCode.Error;
                    res.data = false;
                    res.msg = "删除失败";
                }

                return res;
            }
         }
  }