
//////此代码由CNetCodeGen生成， 作者：cts 生成时间：2025-06-11 10:16:12
using System;
using Dapper.Contrib.Extensions;
namespace CNet.Model.Main
{
     /// <summary>
    ///  
    ///</summary>
    [Table("test_table")]
    public partial class test_table
    {

        /// <summary>
        /// 
        ///</summary>

        [Key]
            public int Id { get; set; }
    
        /// <summary>
        /// 
        ///</summary>
        public string Name { get; set; }
    
   }
}