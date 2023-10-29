using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CNet.Main.Model;
using CNet.Main.BLL;
using CNet.Model;

namespace CNet.Web.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/WikiSort")]
    public class WikiSortController : Controller
    {
        Wiki_SortBLL bll = new Wiki_SortBLL();
        Wiki_MainBLL mainBLL = new Wiki_MainBLL();
        [HttpPost]
        [Route("GetList")]
        public DataRes<List<V_Wiki_Sort_Parent>> GetList()
        {
            var Sorts = new V_Wiki_Sort_ParentBLL().GetList("StopFlag=0");

            return new DataRes<List<V_Wiki_Sort_Parent>>() { data = Sorts };
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [Route("Add")]
        [HttpPost]
        public DataRes<bool> Add([FromBody]Wiki_Sort model)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            model.SortCode = mainBLL.GetSortCode();
            model.ParentCode = model.ParentCode ?? "";
            model.Lmdt = model.Lmdt = DateTime.Now;
            var user = CNetFactory.GetCNetUser(User);
            model.Lmid = $"{user.UserCode}-{user.UserName}";
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
        public DataRes<bool> Edit([FromBody]Wiki_Sort model)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            model.Lmdt = DateTime.Now;
            var user = CNetFactory.GetCNetUser(User);
            model.Lmid = $"{user.UserCode}-{user.UserName}";
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

            var r = bll.ChangeSotpStatus($"SortCode='{id}'");
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