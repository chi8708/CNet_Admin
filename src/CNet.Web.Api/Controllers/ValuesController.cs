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
        [HttpGet]
        public string Get()
        {
        //    T4.MySqlDbHelper tt = new MySqlDbHelper();
        //    tt.GetDbTables();
        //    tt.GetDbColumns("Pub_User");

            T4.SQLiteDbHelper tt = new SQLiteDbHelper();
			tt.GetDbTables();
			tt.GetDbColumns("pub_user");
			//var log= LogFactory.GetLogger(Request.Path);
			//log.Info("info");
			//log.Warning("ok");
			//log.Error("error");
			//_logger.LogError("ssss");
			//LogHelper.WrtieRequestLog(Common.LogLevel.Info, "1111");
			//int a = 0;
			//int b = 1 / a;
			// var users = bll.GetList(" 1=1 ");
			//RedisHelper rd = new RedisHelper();
			//rd.HashSet("key1", "keyData1",new { id=1,value="2"});
			//rd.HashSet("key1", "keyData2", new { id = 2, value = "2" });
			return "1111";
            //return new Pub_UserBLL().GetList("");
            //return new Pub_UserBLL().GetList("");
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
