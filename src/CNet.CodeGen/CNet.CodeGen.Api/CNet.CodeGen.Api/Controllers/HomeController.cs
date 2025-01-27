using CNet.CodeGen.Api.Template;
using Microsoft.AspNetCore.Mvc;

namespace CNet.CodeGen.Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = GetData("pub_role");
            return View("./Template/Model/Entity.cshtml", model);
        }

        /// <summary>
        /// 生成实体Model层代码
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="templatePath"></param>
        /// <param name="saveDir"></param>
        /// <returns></returns>
        public TableModel GetData(string tableName)
        {
            var dbType = Config.DbType;
            var dbHeplper = DbFactory.CreatDb(dbType);
            var columns = dbHeplper.GetDbColumns(tableName);
            var model = new TableModel
            {
                TableName = tableName,
                Columns = columns
            };
            return model;
        }
    }
}
