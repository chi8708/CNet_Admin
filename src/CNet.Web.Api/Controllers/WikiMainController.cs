using CNet.Main.BLL;
using CNet.Main.Model;
using CNet.Model;
using CNet.Web.Api.Model.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.IO;
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
        [AllowAnonymous]
        public PageDateRes<V_Wiki_Main_Sort> GetPage([FromBody] PageDataReq pageReq)
        {
            var whereStr = GetWhereStr(pageReq.query);
            if (whereStr == "-1")
            {
                return new PageDateRes<V_Wiki_Main_Sort>() { code = ResCode.Error, msg = "查询参数有误！", data = null };
            }
            var users = new V_Wiki_Main_SortBLL().GetPage(whereStr, (pageReq.field + " " + pageReq.order), pageReq.pageNum, pageReq.pageSize);

            return users;
        }

        private string GetWhereStr(Dictionary<string,object> queryObj)
        {
            StringBuilder sb = new StringBuilder(" 1=1 ");
            sb.Append(" and StopFlag=0 ");
            var query = this.HttpContext.GetWhereStr();
            if (query == "-1")
            {
                return query;
            }
            sb.AppendFormat(" and {0} ", query);

            if (queryObj.ContainsKey("S_Keyword")&& !string.IsNullOrWhiteSpace(queryObj["S_Keyword"]?.ToString()))
            {
                sb.AppendFormat(" and (title like '%{0}%' or keyword like '%{0}%')",QueryHelper.InjectionFilter((queryObj["S_Keyword"]?.ToString())));
            }

            if (!string.IsNullOrWhiteSpace(queryObj["S_SortCode"]?.ToString()))
            {
                sb.AppendFormat(" and sortCode in ({0}) ", string.Format(@"with f as
                            (
                            select* FROM Wiki_Sort AS pd where SortCode = '{0}'

                            union all

                            select a.* from Wiki_Sort as a inner join f as b on a.ParentCode = b.SortCode
                            )
                            SELECT f.SortCode FROM  f LEFT JOIN Wiki_Sort as a on f.ParentCode = a.SortCode"
                            , queryObj["S_SortCode"]?.ToString()));
            }


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

        static string currentDir = Directory.GetCurrentDirectory();
        /// <summary>
        /// 获取word流文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [Route("GetDocxFile")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetDocxFile(string path)
        {
            var filePath = currentDir+ path;
            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "example.docx");
        }


    }



}
