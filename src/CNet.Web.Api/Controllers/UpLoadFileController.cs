using CNet.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System;
using CNet.Main.Model;

namespace CNet.Web.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [Produces("application/json")]
    [Route("api/UpLoadFile")]
    public class UpLoadFileController : BaseController
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="SourceType">字段标识符</param>
        /// <returns></returns>
        [Route("UploadInterface/{SourceType}")]
        [HttpPost]
        public DataRes<List<File_Upload>> UploadInterface(string SourceType)
        {
            List<File_Upload> list = new List<File_Upload>();
            DataRes<List<File_Upload>> res = new DataRes<List<File_Upload>>() { code = ResCode.Error, data = list, msg = "" };
            try
            {
                File_Upload model = null;
                if (string.IsNullOrWhiteSpace(SourceType))
                {
                    res.msg = "SourceType参数不能为空!";
                    return res;
                }
                IFormFileCollection files = HttpContext.Request.Form.Files;
                var basePath = System.IO.Directory.GetCurrentDirectory();
                string fileHead = basePath + "\\FileUpload\\" + SourceType + "\\";//需要在appsettings中配置物理路径
                //判断是否有文件上传
                if (files.Count == 0)
                {
                    res.msg = "没有找到上传文件";
                    return res;
                }
                for (int i = 0; i < files.Count; i++)
                {
                    Random rnd = new Random();
                    int rndNum = rnd.Next(100, 999);
                    //设置文件上传路径
                    string filetype = files[i].FileName.Split('.')[files[i].FileName.Split('.').Length - 1];//取后缀
                    string filename = DateTime.Now.ToString("yyyyMMddhhmmss-" + rndNum) + "." + filetype;
                    string fullFileName = fileHead + filename;//完整字段
                    //检查上传的物理路径是否存在，不存在则创建
                    string path = Path.GetDirectoryName(fullFileName);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //将流写入文件
                    using (Stream stream = files[i].OpenReadStream())
                    {
                        // 把 Stream 转换成 byte[]
                        byte[] bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, bytes.Length);
                        // 设置当前流的位置为流的开始
                        stream.Seek(0, SeekOrigin.Begin);
                        // 把 byte[] 写入文件
                        FileStream fs = new FileStream(fullFileName, FileMode.Create);
                        BinaryWriter bw = new BinaryWriter(fs);
                        bw.Write(bytes);
                        bw.Close();
                        fs.Close();
                    }
                    var user = CNetFactory.GetCNetUser(User);
                    model = new File_Upload();
                    model.FileUrl = SourceType + "/" + filename;
                    model.FileName = filename;
                    model.SourceType = SourceType;
                    model.AppType = 1;
                    model.CreateTime = DateTime.Now;
                    model.CreateUser = user.UserCode + "-" + user.UserName;
                    list.Add(model);
                }
                res.msg = "上传成功";
                res.code = ResCode.Success;
                res.data = list;
                return res;
            }
            catch (Exception e)
            {
                res.msg = e.Message;
                return res;
            }
        }
    }
}
