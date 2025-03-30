
//////此代码由CNetCodeGen生成， 作者：cts 生成时间：2025-03-30 20:48:13
using System;
using Dapper.Contrib.Extensions;
namespace CNet.Model.Main
{
     /// <summary>
    ///  
    ///</summary>
    [Table("gen_log")]
    public partial class gen_log
    {

        /// <summary>
        /// 创建时间
        ///</summary>
        public DateTime CreateTime { get; set; }
    
        /// <summary>
        /// 
        ///</summary>
        public string GenInfo { get; set; }

        public string TableName { get; set; }
        /// <summary>
        /// 
        ///</summary>

        [Key]
                public long Id { get; set; }
    
        /// <summary>
        /// 
        ///</summary>
        public int Status { get; set; }
    
   }
}