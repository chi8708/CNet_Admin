using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using CNet.Main.BLL;
using CNet.Common;
using CNet.Main.Model;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using T4;
using Microsoft.Extensions.Caching.Memory;
using System.Drawing;
using System.Security;
using Microsoft.Extensions.ObjectPool;
using CNet.Web.Api.Model;

namespace CNet.Web.Api.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    public class ValuesController : Controller
    {
        //public ILogger<ValuesController> _logger;
        //public ValuesController(ILogger<ValuesController> logger)
        //{
        //    _logger = logger;

        //}
        // GET api/values
        //Pub_UserBLL bll = null;
        //public ValuesController(Pub_UserBLL pubbll)
        //{
            
        //    bll = pubbll;
        //}
        private IMemoryCache _memoryCache;
        private Pub_UserBLL _userBLL;
         public ValuesController(IMemoryCache cache, Pub_UserBLL user) 
        {
          this._memoryCache = cache;
          this._userBLL = user;
        }
        [HttpGet]
        [ResponseCache(Duration =20)]
        public  string Get()
        {
            //    T4.MySqlDbHelper tt = new MySqlDbHelper();
            //    tt.GetDbTables();
            //    tt.GetDbColumns("Pub_User");

            //t4 SQLiteDbHelper 实例
            T4.SQLiteDbHelper tt = new SQLiteDbHelper();
			tt.GetDbTables();
			tt.GetDbColumns("Pub_User");

            //log4 net 实例
            //var log= LogFactory.GetLogger(Request.Path);
            //log.Info("info");
            //log.Warning("ok");
            //log.Error("error");
            //_logger.LogError("ssss");
            //LogHelper.WrtieRequestLog(Common.LogLevel.Info, "1111");
            //int a = 0;
            //int b = 1 / a;
            // var users = bll.GetList(" 1=1 ");

            // RedisHelper 实例
            //RedisHelper rd = new RedisHelper();
            //rd.HashSet("key1", "keyData1",new { id=1,value="2"});
            //rd.HashSet("key1", "keyData2", new { id = 2, value = "2" });


            return "1111";
        }

        [HttpGet]
        [Route("cacheTest")]
        public async Task<string> CacheTest()
        {

            // 内存缓存 实例
            string data = await _memoryCache.GetOrCreateAsync<string>("test", (e) => {
                e.AbsoluteExpiration =  DateTime.Now.AddSeconds(20);//绝对过期时间
                //e.AbsoluteExpirationRelativeToNow=TimeSpan.FromSeconds(20);//滑动过期时间
                return Task.FromResult("1233333");
            });

           // _memoryCache.Remove("test");//删除缓存
            return data;
        }

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DITest/{a}/{b}")]
        public string DITest([FromServices]DI_Test test, int a,int b)
        {
            var result= test.Add(a, b);
            var userList = _userBLL.GetList("1=1");
            return result.ToString();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            LogHelper.WrtieRequestLog(Common.LogLevel.Info, "222");
              int a = 0;
            int b = 1 / a;
            return "value";
        }

        // POST api/values
        [HttpPost]
        // application/x-www-form-urlencoded
        //FromBody 后才能读到 application/json; charset=utf-8的值 ，但不能拿接收application/x-www-form-urlencoded类型
        //[Consumes("application/json;charset=utf-8")]
        public void Post([FromBody]Pub_User value)
        {
            //  Request.EnableRewind();
            if (Request.ContentLength>0)
            {
                Request.Body.Position = 0;
                var bytes = new byte[Request.Body.Length];
                Request.Body.Read(bytes, 0, bytes.Length);
                var data = System.Text.Encoding.UTF8.GetString(bytes);
            }
          
            var a = 0;
            var b = 1 / a;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
