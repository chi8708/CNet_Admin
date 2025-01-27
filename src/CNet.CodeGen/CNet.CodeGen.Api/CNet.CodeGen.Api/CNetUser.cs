using CNet.BLL.Main;
using CNet.Model.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Text;


namespace CNet.Web.Api
{
    public static class CNetFactory
    {
        public static CNetUser GetCNetUser(this IPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
            {
                return null;
            }
            return new CNetUser(user);
        }
    }

    public class CNetUser
    {
        public CNetUser(IPrincipal user)
        {
            this.User = user;
            this.Identity= (ClaimsIdentity)user.Identity;
        }

        public IPrincipal User { get; set; }
        public ClaimsIdentity Identity { get; set; }

        public int Id 
        {
            get
            {

                var id = Identity.Claims.FirstOrDefault(p => p.Type == "Id").Value;
                return Convert.ToInt32(id);
            }
            
        }
        public string UserCode
        {
            get
            {

                var userCode = Identity.Claims.FirstOrDefault(p => p.Type == "UserCode").Value;
                return userCode;               
            }
        }

        public  string UserName
        {
            get
            {
                var userName = Identity.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name).Value;
                return userName;   
            }
        }

        public  string DeptCode
        {
            get
            {
                var deptCode = Identity.Claims.FirstOrDefault(p => p.Type == "DeptCode").Value;
                return deptCode; 
            }
        }


        public string Tel
        {
            get
            {
                var mobile = Identity.Claims.FirstOrDefault(p => p.Type == "Tel").Value;
                return mobile;
            }
        }

        public  List<string> Access
        {
            get
            {
               
                //var functions = new Pub_FunctionBLL().GetUserAccess(this.UserCode);

                return null;
            }
        }


        public static void UpdateToken(dynamic user)
        {
            //写入或更新redis
            //TimeSpan ts = TimeSpan.FromHours(Convert.ToInt32(Constant.TOKEN_EXPRIE_HOURS));
            //Constant.redis7.StringSet(Constant.TOKEN_PRE + user.UserCode, user.Token, ts);
        }


    }
   
}
