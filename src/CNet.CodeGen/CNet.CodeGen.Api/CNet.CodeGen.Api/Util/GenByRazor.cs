using CNet.CodeGen.Api.Template;
using RazorLight;

namespace CNet.CodeGen.Api.Util
{
    public class GenByRazor
    {
        public static async Task<(bool, string)> Compile(string templatePath,string savePath, dynamic model) 
        {
            try
            {

               //"./Template/Model/Entity.cshtml";

                // 读取模板内容
                string templateContent = System.IO.File.ReadAllText(templatePath);

                // 创建 RazorLight 引擎
                var engine = new RazorLightEngineBuilder()
                    .UseMemoryCachingProvider() // 使用内存缓存
                    .Build();

                // 解析模板并生成代码
                string generatedCode = await engine.CompileRenderStringAsync(DateTime.Now.GetHashCode().ToString(), templateContent, model);

                // 保存生成的代码到文件
                await System.IO.File.WriteAllTextAsync(savePath, generatedCode);

                return (true, savePath);
            }
            catch (Exception ex)
            {
                return (false, ex.StackTrace);
            }
        
        }

        /// <summary>
        /// 生成Model层代码
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="templatePath"></param>
        /// <param name="saveDir"></param>
        /// <returns></returns>
        public static (bool, string) CompileModel(string tableName, string saveDir="", string templatePath= "./Code/Model/1Entity.cshtml")
        {
            var dbHeplper= GetDbHelper();
            var columns = dbHeplper.GetDbColumns(tableName);
            var model = new TableModel
            {
                TableName = tableName,
                Columns = columns
            };
            string rootPath = Directory.GetCurrentDirectory();
            var savePath = Path.Combine(rootPath, $"Code/Model/{Config.Namespace2}/{tableName}.cs");
            return Compile(templatePath, savePath, model).Result;

        }

        /// <summary>
        /// 生成BLL层代码
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="templatePath"></param>
        /// <param name="saveDir"></param>
        /// <returns></returns>
        public static (bool, string) CompileBLL(string tableName, string saveDir = "", string templatePath = "./Code/BLL/1Service.cshtml")
        {
            var dbHeplper = GetDbHelper();
            var model = new TableModel
            {
                TableName = tableName,
                Columns = null
            };
            string rootPath = Directory.GetCurrentDirectory();
            var savePath = Path.Combine(rootPath, $"Code/BLL/{Config.Namespace2}/{tableName}.cs");
            return Compile(templatePath, savePath, model).Result;

        }

        /// <summary>
        /// 生成Controllers层代码
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="templatePath"></param>
        /// <param name="saveDir"></param>
        /// <returns></returns>
        public static (bool, string) CompileAdminController(string tableName, string saveDir = "", string templatePath = "./Code/AdminController/1Controller.cshtml")
        {
            var dbHeplper = GetDbHelper();
            var model = new TableModel
            {
                TableName = tableName,
                Columns = null
            };
            string rootPath = Directory.GetCurrentDirectory();
            var savePath = Path.Combine(rootPath, $"Code/AdminController/{tableName}Controller.cs");
            return Compile(templatePath, savePath, model).Result;

        }

        private static BaseDbHelper GetDbHelper()
        {
            var dbType = Config.DbType;
            var db = DbFactory.CreatDb(dbType);
            return db;
        }
    }
}
