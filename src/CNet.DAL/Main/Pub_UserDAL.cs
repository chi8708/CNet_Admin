using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNet.Main.DAL
{
    public partial class Pub_UserDAL
    {
        /// <summary>
        /// 修改删除状态
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool ChangeSotpStatus(string where, object pms)
        {
            string sql = "UPDATE Pub_User SET StopFlag =1 ";
            if (string.IsNullOrWhiteSpace(where))
            {
                return false;
            }

            sql += " where " + where;

            return DBHelperFactory.GetInstance_Main().Excute(sql, pms) > 0;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool EditPassWord(string userCode,string pwd)
        {
            string sql = "UPDATE Pub_User SET UserPwd =@UserPwd ";
            var pms = new { UserPwd = pwd };

            return DBHelperFactory.GetInstance_Main().Excute(sql,pms) > 0;
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool EditPassword(int id, string password, string editor)
        {
            string sql = "UPDATE Pub_User SET UserPwd =@UserPwd, Lmid =@Lmid,Lmdt =GETDATE()  WHERE Id=@Id ";
            var pms = new
            {
                UserPwd = password,
                Id = id,
                Lmid = editor
            };

            return DBHelperFactory.GetInstance_Main().Excute(sql, pms) > 0;
        }

    }
}
