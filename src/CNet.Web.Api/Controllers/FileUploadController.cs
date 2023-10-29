
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using System.Threading.Tasks;
using CNet.Web.Api.Controllers;
using CNet.Model;

namespace GemmyMobile.Web.Api.Controllers
{
    /// <summary>
    /// 对象存储
    /// </summary>
    [Authorize]
    [Produces("application/json")]
    [Route("api/ObjectStorage")]
    public class FileUploadController : BaseController
    {


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
                var folderUrl = AppContext.BaseDirectory + @"\TempFileUpload\" + name.Split('.')[0];
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
        public async Task<DataRes<string>> MergeFile([FromForm] MergFileOption fileOption)
        {
            DataRes<string> res = new() { code = ResCode.Error, msg = "" };
            try
            {
                string fileDir = fileOption.fileDirName;
                if (string.IsNullOrEmpty(fileDir))
                {
                    res.code = ResCode.Error;
                    res.msg = "fileDirName参数不能为空!";
                    return res;
                }

                //临时保存分块的目录
                var dir = Path.Combine(AppContext.BaseDirectory + @"TempFileUpload", fileOption.fileName.Split('.')[0]);
                var files = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);

                var newFilePath = AppContext.BaseDirectory + $"\\TempFileUpload\\{fileOption.fileName}";
                var newfileStream = new FileStream(newFilePath, FileMode.OpenOrCreate);
                foreach (var item in files)
                {
                    var tempfile = Path.Combine(dir, item);
                    using (var fileStream = new FileStream(tempfile, FileMode.Open))
                    {
                        await fileStream.CopyToAsync(newfileStream);
                    }
                }
                newfileStream.Close();

                var fi = new FileInfo(newFilePath);
                var fileSize = fi.Length;

                var ossDir = System.DateTime.Now.ToString("yyyyMMdd"); //获取当前日期，就可以每天都建立一个文件夹进行管理
                var filePathName = Guid.NewGuid().ToString("N") + "." + fileOption.fileSuffix;//重命名

                Tuple<bool, string> result = new(false, "");

                if (fileSize > (1 * 1024 * 1024))//1M
                {
                    //切片上传Oss
                    // result = PutObjectMulti(fileDir + "/" + ossDir + "/" + filePathName, newFilePath);
                }
                else
                {
                    //普通上传Oss
                    using var stremf = System.IO.File.OpenRead(newFilePath);
                    //result = PutObject(fileDir + "/" + ossDir + "/" + filePathName, stremf);
                }

                res.msg = "上传成功";
                res.code = ResCode.Success;
                //res.data = pub_ObjectStorageFile.AliyunPath;
                return res;

            }
            catch (Exception e)
            {
                res.code = ResCode.Error;
                res.msg = "上传失败" + e.Message;
                res.data = "";
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
            public int fileName { get; set; }
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
