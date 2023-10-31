using CNet.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System;
using CNet.Main.Model;
using System.Threading.Tasks;
using System.Net;
using ZstdSharp.Unsafe;

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
        //AppContext.BaseDirectory
        static string  currentDir=Directory.GetCurrentDirectory();
        
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



        /// <summary>
        /// 切片上传文件
        /// </summary>
        /// <param name="fileOption">字段标识符</param>
        /// <returns></returns>
        [Route("SaveFile")]
        [HttpPost]
        public async Task<DataRes<string>> SaveFile([FromForm] FileSectionOption fileOption)
        {
            DataRes<string> res = new() { code = ResCode.Error, msg = "" };

            try
            {
                if (fileOption == null)
                {
                    res.code = ResCode.Error;
                    res.msg = "未选择上传文件";
                }
                StreamReader reader = new StreamReader(fileOption.file.OpenReadStream());
                string content = reader.ReadToEnd();
                string name = fileOption.file.FileName;
                string suffix = name.Split('.')[1];
                //if (!fileFormatArray.Contains(suffix))
                //{
                //    return Ok("只能上传指定格式文件");
                //}
                var folderUrl = currentDir + @"\TempFileUpload\" +User.GetCNetUser().UserCode+"\\"+ name.Split('.')[0];
                if (int.Parse(fileOption.fileNum) < 10)
                {
                    fileOption.fileNum = "0" + fileOption.fileNum;
                }
                var fileNameNew = name.Split('.')[0] + "_" + fileOption.fileNum;

                if (!Directory.Exists(folderUrl))
                {
                    Directory.CreateDirectory(folderUrl);
                }
                string filePath = string.Format(folderUrl + "/" + fileNameNew);
                using var fs = System.IO.File.Create(filePath);
                await fileOption.file.CopyToAsync(fs);
                fs.Flush();
                fs.Close();

                res.code = ResCode.Success;
                res.msg = "上传保存成功";
                return res;
            }
            catch (Exception e)
            {
                res.code = ResCode.Error;
                res.msg = "上传保存失败：" + e.Message;
                return res;
            }

        }

        /// <summary>
        /// 合并切片，上传Oss
        /// </summary>
        /// <param name="fileOption"></param>
        /// <returns></returns>
        [Route("MergeFile")]
        [HttpPost]
        public async Task<DataRes<dynamic>> MergeFile([FromForm] MergFileOption fileOption)
        {
            DataRes<dynamic> res = new() { code = ResCode.Error, msg = "" };
            try
            {

                //临时保存分块的目录
                var dir = Path.Combine(currentDir + @"\\TempFileUpload\\"+ User.GetCNetUser().UserCode + "\\", fileOption.fileName.Split('.')[0]);
                var files = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);

                //重新保存目录；
                var newFileUrl = $"FileUpload\\{fileOption.fileDirName}\\{DateTime.Now.ToString("yyyyMMdd")}";
                var newFolderUrl= currentDir +"\\"+newFileUrl;
                if (!Directory.Exists(newFolderUrl))
                {
                    Directory.CreateDirectory(newFolderUrl);
                }
                var newFileName = $"{DateTime.Now.ToString("yyyyMMddHHmmsss")}_{fileOption.fileName}";
                var newFilePath = $"{newFolderUrl}\\{newFileName}";

                using var newfileStream = new FileStream(newFilePath, FileMode.OpenOrCreate);
                foreach (var item in files)
                {
                    var tempfile = Path.Combine(dir, item);
                    using (var fileStream = new FileStream(tempfile, FileMode.Open))
                    {
                        await fileStream.CopyToAsync(newfileStream);
                    }
                }
                newfileStream.Close();
                newfileStream.Dispose();

                var fi = new FileInfo(newFilePath);
                var fileSize = fi.Length;

                var ossDir = System.DateTime.Now.ToString("yyyyMMdd"); //获取当前日期，就可以每天都建立一个文件夹进行管理
                var filePathName = Guid.NewGuid().ToString("N") + "." + fileOption.fileSuffix;//重命名

                Tuple<bool, string> result = new(true, "");

                if (fileSize > (1 * 1024 * 1024))//1M
                {
                    //切片上传Oss
                   // result = PutObjectMulti(fileDir + "/" + ossDir + "/" + filePathName, newFilePath);
                }
                else
                {
                    //普通上传Oss
                    //using var stremf = System.IO.File.OpenRead(newFilePath);
                   // result = PutObject(fileDir + "/" + ossDir + "/" + filePathName, stremf);
                }
                //删除上传服务器的文件
                // var delFilePath = currentDir + "TempFileUpload";
                var delFilePath = dir;
                DeleteSpecifiedPathAllFile(delFilePath);
                res.msg = "上传成功";
                res.code = ResCode.Success;
                res.data = new {newFileUrl,newFileName,fileName= fileOption.fileName };
                return res;
            }
            catch (Exception e)
            {
                res.code = ResCode.Error;
                res.msg = "上传失败" + e.Message;
                res.data = new { };
                return res;
            }

        }


        /// <summary>
        /// 删除指定路径下的所有文件
        /// </summary>
        /// <param name="filepath">指定路径</param>
        /// <returns></returns>
        private bool DeleteSpecifiedPathAllFile(string filepath)
        {
            try
            {
                DirectoryInfo info = new(filepath)
                {
                    // 去除文件夹的只读属性
                    Attributes = FileAttributes.Normal & FileAttributes.Directory
                };
                // 去除文件的只读属性
                System.IO.File.SetAttributes(filepath, FileAttributes.Normal);
                if (Directory.Exists(filepath))
                {
                    foreach (var file in Directory.GetFileSystemEntries(filepath))
                    {
                        if (System.IO.File.Exists(file))
                        {
                            // 如果有子文件则删除子文件的所有文件
                            System.IO.File.Delete(file);
                        }
                        else
                        {
                            // 递归删除子文件夹
                            DeleteSpecifiedPathAllFile(file);
                        }
                    }
                    // 删除已空文件夹(此步骤会删除指定目录的最底层文件夹)
                    Directory.Delete(filepath, true);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public class FileSectionOption
        {
            public IFormFile file { get; set; }
            public int fileName { get; set; }//文件名需要前端处理，不要重名
            public string fileType { get; set; }
            public string fileNum { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class MergFileOption
        {
            public string fileName { get; set; }
            public string fileTotalNum { get; set; }
            public string fileSuffix { get; set; }
            public string fileDirName { get; set; }
        }

    }
}
