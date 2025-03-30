
//////此代码由CNetCodeGen生成， 作者：cts 生成时间：2025-03-31 00:31:48
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
        /// 
        ///</summary>

        [Key]
            public long Id { get; set; }
    
        /// <summary>
        /// 创建时间
        ///</summary>
        public DateTime CreateTime { get; set; }
    
        /// <summary>
        /// 
        ///</summary>
        public string GenInfo { get; set; }
    
        /// <summary>
        /// 
        ///</summary>
        public int Status { get; set; }
    
        /// <summary>
        /// 
        ///</summary>
        public string TableName { get; set; }
    
   }
}