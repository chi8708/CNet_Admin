using CNet.BLL.Main;
using CNet.CodeGen.Api.Util;
using CNet.Model;
using CNet.Model.Main;
using CNet.Web.Api;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNet.CodeGen.Api.Controllers
{
    /// <summary>
    /// 代码生成器控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CodeGenController : ControllerBase
    {
        private readonly string _codeBasePath = "Code";
        private readonly string baseDir = "E:\\Study\\DotNet\\CNet_Admin\\src\\";
        gen_logBLL genLogBLL = new gen_logBLL();

      /// <summary>
      /// 获取所有数据表
      /// </summary>
      /// <returns>数据表列表</returns>
      [HttpGet("tables")]
        public async Task<DataRes<List<string>>> GetTables()
        {
            try
            {
                // 这里实现获取数据库中所有表的逻辑
                // 示例数据，实际应用中应从数据库获取

                List<string> tables = new List<string>();
                DBHelperFactory.GetTables_Main().ForEach(p => { tables.Add(p.tablename); });

                return DataRes<List<string>>.Success(tables);
            }
            catch (Exception ex)
            {
                return DataRes<List<string>>.Fail($"获取数据表失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="request">生成代码请求</param>
        /// <returns>生成结果</returns>
        [HttpPost("generate")]
        public async Task<DataRes<List<GeneratedCodeResult>>> GenerateCode([FromBody] GenerateCodeRequest request)
        {
            try
            {
                if (request.Tables == null || !request.Tables.Any())
                {
                    return DataRes<List<GeneratedCodeResult>>.Fail("请至少选择一个数据表");
                }

                // 保存生成结果
                var results = new List<GeneratedCodeResult>();

 
                foreach (var table in request.Tables)
                {
                    //插入权限表
                    var functionCode = GenByRazor.Compile_InsertFunction(table).Item2;

                    // 调用代码生成逻辑
                    // 实际应用中，这里会根据options中的选项决定生成哪些代码
                    GenByRazor.CompileModel(table, request.Options.Backend.Model? @$"{baseDir}CNet.Model\T4.DapperExt\Main" : "");
                    GenByRazor.CompileBLL(table, request.Options.Backend.Bll ? @$"{baseDir}CNet.BLL\\T4.DapperExt\Main" : "");
                    GenByRazor.CompileAdminController(table, request.Options.Backend.Controllers ? @$"{baseDir}\CNet.Web.Api\Controllers" : "");
                    GenByRazor.CompileAdminUI_access(table,functionCode ,request.Options.Frontend.Access ? @$"{baseDir}\CNet.Web.Admin\src\access" : "");
                    GenByRazor.CompileAdminUI_api(table, functionCode, request.Options.Frontend.Api ? @$"{baseDir}\CNet.Web.Admin\src\api" : "");
                    var r= GenByRazor.CompileAdminUI_view(table, functionCode, request.Options.Frontend.Views ? @$"{baseDir}\CNet.Web.Admin\src\view" : "");

                    List<string> codeInfos = new List<string>() { };
                    if (request.Options.Backend.Model)
                    {
                        codeInfos.Add("Model");
                    }
                    if (request.Options.Backend.Bll)
                    {
                        codeInfos.Add("Bll");
                    }
                    if (request.Options.Backend.Controllers)
                    {
                        codeInfos.Add("Controllers");
                    }
                    if (request.Options.Frontend.Access)
                    {
                        codeInfos.Add("Access");
                    }
                    if (request.Options.Frontend.Api)
                    {
                        codeInfos.Add("Api");
                    }
                    if (request.Options.Frontend.Views)
                    {
                        codeInfos.Add("Views");
                    }
                    genLogBLL.Insert(new Model.Main.gen_log { 
                        TableName=table,
                        CreateTime = DateTime.Now,
                         GenInfo = $"{string.Join("、", codeInfos)}",
                         Status=r.Item1?1:-1
                    });

                    // 创建生成结果记录
                    var result = new GeneratedCodeResult
                    {
                        TableName = table,
                        Code = $"{string.Join("、", codeInfos)}",
                        GeneratedTime = DateTime.Now
                    };

                    results.Add(result);
                }

                // 在实际应用中，你可能需要将结果保存到数据库
                // 这里简化处理，直接返回结果
                return DataRes<List<GeneratedCodeResult>>.Success(results);
            }
            catch (Exception ex)
            {
                return DataRes<List<GeneratedCodeResult>>.Fail($"代码生成失败: {ex.Message}");
            }
        }

        [Route("GetPage")]
        [HttpPost]
        public PageDateRes<gen_log> GetPage([FromBody] PageDataReq pageReq)
        {
            var whereStr = GetWhereStr();
            if (whereStr == "-1")
            {
                return new PageDateRes<gen_log>() { code = ResCode.Error, msg = "查询参数有误！", data = null };
            }
            var list = genLogBLL.GetPage(whereStr, (pageReq.field + " " + pageReq.order), pageReq.pageNum, pageReq.pageSize);

            return list;
        }

        private string GetWhereStr()
        {
            StringBuilder sb = new StringBuilder(" 1=1 ");
            var query = this.HttpContext.GetWhereStr();
            if (query == "-1")
            {
                return query;
            }
            sb.AppendFormat(" and {0} ", query);

            return sb.ToString();
        }

        /// <summary>
        /// 查看指定数据表的生成代码
        /// </summary>
        /// <param name="tableName">数据表名称</param>
        /// <returns>生成的代码内容</returns>
        [HttpGet("view/{tableName}")]
        public async Task<DataRes<CodeContent>> ViewCode(string tableName)
        {
            try
            {
                // 获取代码内容
                var codeContent = new CodeContent
                {
                    // 实际应用中，这里应该读取生成的文件内容
                    access = await ReadCodeFileAsync($"AdminUI/access/{tableName}.js"),
                    api = await ReadCodeFileAsync($"AdminUI/api/{tableName}.js"),
                    view_list = await ReadCodeFileAsync($"AdminUI/view/{tableName}/List.vue"),
                    view_edit = await ReadCodeFileAsync($"AdminUI/view/{tableName}/Edit.vue"),
                    model = await ReadCodeFileAsync($"Model/Main/{tableName}.cs"),
                    bll = await ReadCodeFileAsync($"BLL/Main/{tableName}.cs"),
                    dal = await ReadCodeFileAsync($"DAL/Main/{tableName}.cs")
                };

                return DataRes<CodeContent>.Success(codeContent);
            }
            catch (Exception ex)
            {
                return DataRes<CodeContent>.Fail($"查看代码失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 下载指定数据表的生成代码
        /// </summary>
        /// <param name="tableName">数据表名称</param>
        /// <returns>代码压缩包文件</returns>
        [HttpGet("download/{tableName}")]
        public async Task<IActionResult> DownloadCode(string tableName)
        {
            try
            {
                // 实际应用中，这里应该将生成的代码打包为zip文件并返回
                // 这里简化为返回一个文本文件
                var codeContent = await ReadCodeFileAsync($"Model/Main/{tableName}.cs");
                
                // 如果找不到文件内容，返回一个示例代码
                if (string.IsNullOrEmpty(codeContent))
                {
                    codeContent = $"// 示例代码\npublic class {tableName} {{\n    public int Id {{ get; set; }}\n    public string Name {{ get; set; }}\n}}";
                }

                var bytes = System.Text.Encoding.UTF8.GetBytes(codeContent);
                return File(bytes, "application/octet-stream", $"{tableName}_Code.cs");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"下载代码失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 读取代码文件内容
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <returns>文件内容</returns>
        private async Task<string> ReadCodeFileAsync(string relativePath)
        {
            try
            {
                var fullPath = Path.Combine(_codeBasePath, relativePath);
                if (System.IO.File.Exists(fullPath))
                {
                    return await System.IO.File.ReadAllTextAsync(fullPath);
                }
                return $"// 文件不存在: {fullPath}";
            }
            catch (Exception ex)
            {
                return $"// 读取文件错误: {ex.Message}";
            }
        }
    }

    /// <summary>
    /// 通用数据响应类
    /// </summary>
    public class DataRes<T>
    {
        /// <summary>
        /// 状态码，200表示成功
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 创建成功响应
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="msg">消息</param>
        /// <returns>成功响应</returns>
        public static DataRes<T> Success(T data, string msg = "操作成功")
        {
            return new DataRes<T>
            {
                Code = 200,
                Msg = msg,
                Data = data
            };
        }

        /// <summary>
        /// 创建失败响应
        /// </summary>
        /// <param name="msg">错误消息</param>
        /// <param name="code">错误码</param>
        /// <returns>失败响应</returns>
        public static DataRes<T> Fail(string msg, int code = 500)
        {
            return new DataRes<T>
            {
                Code = code,
                Msg = msg,
                Data = default
            };
        }
    }

    /// <summary>
    /// 分页结果
    /// </summary>
    public class PageResult<T>
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 当前页数据
        /// </summary>
        public List<T> Items { get; set; }
    }

    /// <summary>
    /// 生成代码请求
    /// </summary>
    public class GenerateCodeRequest
    {
        /// <summary>
        /// 要生成代码的数据表列表
        /// </summary>
        public List<string> Tables { get; set; }

        /// <summary>
        /// 代码生成选项
        /// </summary>
        public CodeGenOptions Options { get; set; }
    }

    /// <summary>
    /// 代码生成选项
    /// </summary>
    public class CodeGenOptions
    {
        /// <summary>
        /// 前端代码生成选项
        /// </summary>
        public FrontendOptions Frontend { get; set; }

        /// <summary>
        /// 后端代码生成选项
        /// </summary>
        public BackendOptions Backend { get; set; }
    }

    /// <summary>
    /// 前端代码生成选项
    /// </summary>
    public class FrontendOptions
    {
        /// <summary>
        /// 是否生成Access
        /// </summary>
        public bool Access { get; set; }

        /// <summary>
        /// 是否生成API
        /// </summary>
        public bool Api { get; set; }

        /// <summary>
        /// 是否生成Views
        /// </summary>
        public bool Views { get; set; }
    }

    /// <summary>
    /// 后端代码生成选项
    /// </summary>
    public class BackendOptions
    {
        /// <summary>
        /// 是否生成Model
        /// </summary>
        public bool Model { get; set; }

        /// <summary>
        /// 是否生成BLL
        /// </summary>
        public bool Bll { get; set; }

        /// <summary>
        /// 是否生成Controllers
        /// </summary>
        public bool Controllers { get; set; }
    }

    /// <summary>
    /// 生成的代码结果
    /// </summary>
    public class GeneratedCodeResult
    {
        /// <summary>
        /// 数据表名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 代码内容预览
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime GeneratedTime { get; set; }
    }

    /// <summary>
    /// 代码内容
    /// </summary>
    public class CodeContent
    {
        /// <summary>
        /// Access代码
        /// </summary>
        public string access { get; set; }

        /// <summary>
        /// API代码
        /// </summary>
        public string api { get; set; }

        /// <summary>
        /// 列表视图代码
        /// </summary>
        public string view_list { get; set; }

        /// <summary>
        /// 编辑视图代码
        /// </summary>
        public string view_edit { get; set; }

        /// <summary>
        /// 模型代码
        /// </summary>
        public string model { get; set; }

        /// <summary>
        /// 业务逻辑层代码
        /// </summary>
        public string bll { get; set; }

        /// <summary>
        /// 数据访问层代码
        /// </summary>
        public string dal { get; set; }
    }
} 