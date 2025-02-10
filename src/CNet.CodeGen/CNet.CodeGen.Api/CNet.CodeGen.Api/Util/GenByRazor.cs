using CNet.BLL.Main;
using CNet.CodeGen.Api.Template;
using CNet.Model.Main;
using RazorLight;
using System.Collections.Generic;
using System.Globalization;

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
                    .DisableEncoding() // 禁用编码
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


        /// <summary>
        /// 生成UI代码
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="templatePath"></param>
        /// <param name="saveDir"></param>
        /// <returns></returns>
        public static (bool, string) CompileAdminUI(string tableName)
        {
            var functionCode = CompileUI_InsertFunction(tableName).Item2;
            //根据模板生成代码
            CompileAdminUI_access(tableName, functionCode);
            CompileAdminUI_api(tableName, functionCode);
            CompileAdminUI_view(tableName, functionCode);

            return (true, functionCode);

        }

        /// <summary>
        /// 生成UI代码_access
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="templatePath"></param>
        /// <param name="saveDir"></param>
        /// <returns></returns>
        public static (bool, string) CompileAdminUI_access(string tableName,string functionCode , string saveDir = "", string templatePath = "./Code/AdminUI/1access.cshtml")
        {

            var dbHeplper = GetDbHelper();
            var model = new TableModel
            {
                TableName = tableName,
                Columns = null,
                RootFunction = functionCode
            };
            string rootPath = Directory.GetCurrentDirectory();
            var savePath = Path.Combine(rootPath, $"Code/AdminUI/access/{tableName}.js");
            return Compile(templatePath, savePath, model).Result;
        }

        /// <summary>
        /// 生成UI代码_api
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="templatePath"></param>
        /// <param name="saveDir"></param>
        /// <returns></returns>
        public static (bool, string) CompileAdminUI_api(string tableName, string functionCode, string saveDir = "", string templatePath = "./Code/AdminUI/1api.cshtml")
        {

            var dbHeplper = GetDbHelper();
            var model = new TableModel
            {
                TableName = tableName,
                Columns = null,
                RootFunction = functionCode
            };
            string rootPath = Directory.GetCurrentDirectory();
            var savePath = Path.Combine(rootPath, $"Code/AdminUI/api/{tableName}.js");
            return Compile(templatePath, savePath, model).Result;
        }


        /// <summary>
        /// 生成UI代码_view
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="templatePath"></param>
        /// <param name="saveDir"></param>
        /// <returns></returns>
        public static (bool, string) CompileAdminUI_view(string tableName, string functionCode)
        {

            var resultList= CompileAdminUI_view_List(tableName, functionCode);
            var resultEdit = CompileAdminUI_view_Edit(tableName, functionCode);

            return resultList;
        }


        /// <summary>
        /// 生成UI代码_view_List
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="templatePath"></param>
        /// <param name="saveDir"></param>
        /// <returns></returns>
        public static (bool, string) CompileAdminUI_view_List(string tableName, string functionCode, string saveDir = "", string templatePath = "./Code/AdminUI/1viewList.cshtml")
        {

            var dbHeplper = GetDbHelper();
            var columns = dbHeplper.GetDbColumns(tableName);
            var model = new TableModel
            {
                TableName = tableName,
                Columns = columns,
                RootFunction = functionCode
            };
            string rootPath = Directory.GetCurrentDirectory();
            var saveDirPath = Path.Combine(rootPath, $"Code/AdminUI/view/{tableName}");
            if (!Directory.Exists(saveDirPath))
            {
                Directory.CreateDirectory(saveDirPath);
            }
            var savePath = Path.Combine(rootPath, $"{saveDirPath}/List.vue");
            return Compile(templatePath, savePath, model).Result;
        }

        /// <summary>
        /// 生成UI代码_view_List
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="templatePath"></param>
        /// <param name="saveDir"></param>
        /// <returns></returns>
        public static (bool, string) CompileAdminUI_view_Edit(string tableName, string functionCode, string saveDir = "", string templatePath = "./Code/AdminUI/1viewEdit.cshtml")
        {

            var dbHeplper = GetDbHelper();
            var columns = dbHeplper.GetDbColumns(tableName);
            var model = new TableModel
            {
                TableName = tableName,
                Columns = columns,
                RootFunction = functionCode
            };
            string rootPath = Directory.GetCurrentDirectory();
            var saveDirPath = Path.Combine(rootPath, $"Code/AdminUI/view/{tableName}");
            if (!Directory.Exists(saveDirPath))
            {
                Directory.CreateDirectory(saveDirPath);
            }
            var savePath = Path.Combine(rootPath, $"{saveDirPath}/Edit.vue");
            return Compile(templatePath, savePath, model).Result;
        }

        /// <summary>
        /// 插入权限表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static (bool, string) CompileUI_InsertFunction(string tableName)
        {
            Pub_FunctionBLL bll = new Pub_FunctionBLL();

            var list= bll.GetList($"FunctionEnglish='{tableName}'");
            if (list.Count>0)
            {
                return (false, list[0].FunctionCode);
            }
            //根权限
            Pub_Function rootModel = new Pub_Function();
            rootModel.ParentCode = "FC002";
            rootModel.FunctionCode = bll.GetCode(rootModel.ParentCode);
            rootModel.FunctionChina = tableName;
            rootModel.FunctionEnglish  = tableName;
            rootModel.editdate = DateTime.Now;
            rootModel.editor = string.Format("000000-系统自动");
            rootModel.StopFlag = false;
            rootModel.MenuIcon = "ios - people";
            rootModel.sortidx = 99;
            rootModel.MenuFlag = true;
            rootModel.IsCache = false;
            rootModel.URLString = $"view/{tableName}/List.vue";
            rootModel.RouterPath = tableName;


            //查询
            Pub_Function listModel = new Pub_Function();
            listModel.ParentCode = rootModel.FunctionCode;
            listModel.FunctionCode = rootModel.FunctionCode+"001";
            listModel.FunctionChina ="查询";
            listModel.FunctionEnglish = tableName+"_List";
            listModel.editdate = DateTime.Now;
            listModel.editor = string.Format("000000-系统自动");
            listModel.StopFlag = false;
            listModel.sortidx = 99;
            listModel.MenuFlag = false;

            //新增
            Pub_Function addModel = new Pub_Function();
            addModel.ParentCode = rootModel.FunctionCode;
            addModel.FunctionCode = rootModel.FunctionCode + "002";
            addModel.FunctionChina = "新增";
            addModel.FunctionEnglish = tableName + "_Add";
            addModel.editdate = DateTime.Now;
            addModel.editor = string.Format("000000-系统自动");
            rootModel.IsCache = false;
            addModel.StopFlag = false;
            addModel.sortidx = 99;
            addModel.MenuFlag = false;

            //编辑
            Pub_Function editModel = new Pub_Function();
            editModel.ParentCode = rootModel.FunctionCode;
            editModel.FunctionCode = rootModel.FunctionCode + "003";
            editModel.FunctionChina = "编辑";
            editModel.FunctionEnglish = tableName + "_Edit";
            editModel.editdate = DateTime.Now;
            editModel.editor = string.Format("000000-系统自动");
            rootModel.IsCache = false;
            editModel.StopFlag = false;
            editModel.sortidx = 99;
            editModel.MenuFlag = false;

            //停用
            Pub_Function removeModel = new Pub_Function();
            removeModel.ParentCode = rootModel.FunctionCode;
            removeModel.FunctionCode = rootModel.FunctionCode + "004";
            removeModel.FunctionChina = "删除";
            removeModel.FunctionEnglish = tableName + "_Remove";
            removeModel.editdate = DateTime.Now;
            removeModel.editor = string.Format("000000-系统自动");
            rootModel.IsCache = false;
            removeModel.StopFlag = false;
            removeModel.sortidx = 99;
            removeModel.MenuFlag = false;

            var r= bll.InsertBatch(new List<Pub_Function>() {rootModel,listModel,addModel,editModel,removeModel });

            return (r, rootModel.FunctionCode);


        }


        private static BaseDbHelper GetDbHelper()
        {
            var dbType = Config.DbType;
            var db = DbFactory.CreatDb(dbType);
            return db;
        }

        /// <summary>
        /// 转为小驼峰命名 HELLO_WORLD转为helloWorld
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public static string ToLowerPascalCase(string input)
        {
            return input.First().ToString().ToLower() + input.Substring(1);
            //if (string.IsNullOrEmpty(input))
            //{
            //    return input;
            //}

            //// 将字符串转换为小写并分割单词
            //string[] words = input.ToLower().Split(new[] { '_', ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);

            //// 将第一个单词的首字母保持小写
            //string result = words[0];

            //// 将后续单词的首字母大写
            //for (int i = 1; i < words.Length; i++)
            //{
            //    result += CultureInfo.CurrentCulture.TextInfo.ToTitleCase(words[i]);
            //}

            //return result;
        }
    }
}
