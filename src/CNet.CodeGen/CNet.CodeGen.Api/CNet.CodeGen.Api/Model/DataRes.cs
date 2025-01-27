using System;
using System.Collections.Generic;
using System.Text;

namespace CNet.Model
{
    public class DataRes<T>
    {
        public DataRes()
        {
            this.code = ResCode.Success;
            this.msg = "ok";
        }
        public ResCode code { get; set; }

        public string msg { get; set; }

        public T data { get; set; }


        public static DataRes<T> Success(T data, string msg = "成功", ResCode code = ResCode.Success)
        {
            return new DataRes<T> { code = code, msg = msg, data = data };
        }
        public static DataRes<T> Error(T data = default(T), string msg = "失败", ResCode code = ResCode.Error)
        {
            return new DataRes<T> { code = code, msg = msg, data = data };
        }
        public static DataRes<T> NoValidate(T data = default(T), string msg = "请求数据验证失败", ResCode code = ResCode.NoValidate)
        {
            return new DataRes<T> { code = code, msg = msg, data = data };
        }
    }
    public enum ResCode
    {
        /// <summary>
        /// 错误
        /// </summary>
        Error = -1,
        /// <summary>
        /// 验证未通过
        /// </summary>
        NoValidate = 0,
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,
        /// <summary>
        /// 授权失败
        /// </summary>
        Unauthorized = 401
    }
}
