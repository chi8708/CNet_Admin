using CNet.Main.BLL;
using CNet.Main.Model;
using CNet.Model;
using CNet.Web.Api.Model.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace CNet.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WikiMainController : ControllerBase
    {
        Wiki_MainBLL bll = new Wiki_MainBLL();

        /// <summary>
        /// 获取内容分页
        /// </summary>
        /// <returns></returns>
        [Route("GetPage")]
        [HttpPost]
        public PageDateRes<V_Wiki_Main_Sort> GetPage([FromBody] PageDataReq pageReq)
        {
            var whereStr = GetWhereStr();
            if (whereStr == "-1")
            {
                return new PageDateRes<V_Wiki_Main_Sort>() { code = ResCode.Error, msg = "查询参数有误！", data = null };
            }
            var users = new V_Wiki_Main_SortBLL().GetPage(whereStr, (pageReq.field + " " + pageReq.order), pageReq.pageNum, pageReq.pageSize);

            return users;
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
        /// 添加内容
        /// </summary>
        /// <returns></returns>
        [Route("Add")]
        [HttpPost]
        public DataRes<bool> Add([FromBody] Wiki_Main model)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            model.EditTime = model.CreateTime = DateTime.Now;
            var user = new CNetUser(User);
            model.EditUser = model.CreateUser = $"{user.UserCode}-{user.UserName}";
            model.StopFlag = false;
            var r = bll.Insert(model);
            if (r <= 0)
            {

                return DataRes<bool>.Error();
            }

            return res;
        }

        /// <summary>
        /// 编辑内容
        /// </summary>
        /// <returns></returns>
        [Route("Edit")]
        [HttpPost]
        public DataRes<bool> Edit([FromBody] Wiki_Main model)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            model.EditTime = DateTime.Now;
            var user = new CNetUser(User);
            model.EditUser = $"{user.UserCode}-{user.UserName}";
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
        /// 删除内容
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
