using CNet.Main.BLL;
using CNet.Main.Model;
using CNet.Model;
using CNet.Web.Api.Model.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;

namespace CNet.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WikiMainController : ControllerBase
    {
        Wiki_MainBLL bll=new Wiki_MainBLL();

        /// <summary>
        /// 获取用户分页
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
    }
}
