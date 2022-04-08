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
    }

    public enum ResCode
    {
        /// <summary>
        /// 错误
        /// </summary>
       Error=-1,
       /// <summary>
       /// 验证未通过
       /// </summary>
       NoValidate=0,
       /// <summary>
       /// 成功
       /// </summary>
       Success = 1
    }
}
