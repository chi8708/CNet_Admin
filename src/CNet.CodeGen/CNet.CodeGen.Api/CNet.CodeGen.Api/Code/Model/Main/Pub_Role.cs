
//////此代码由CNetCodeGen生成， 作者：cts 生成时间：2025-01-27 13:48:01
using System;
using Dapper.Contrib.Extensions;
namespace CNet.Model.Main
{
     /// <summary>
    ///  
    ///</summary>
    [Table("Pub_Role")]
    public partial class Pub_Role
    {

        /// <summary>
        /// 
        ///</summary>

        [Key]
                public int Id { get; set; }
    
        /// <summary>
        /// 
        ///</summary>
        public string RoleCode { get; set; }
    
        /// <summary>
        /// 
        ///</summary>
        public string RoleName { get; set; }
    
        /// <summary>
        /// 
        ///</summary>
        public string Remark { get; set; }
    
        /// <summary>
        /// 
        ///</summary>
        public bool StopFlag { get; set; }
    
        /// <summary>
        /// 
        ///</summary>
        public string Crid { get; set; }
    
        /// <summary>
        /// 
        ///</summary>
        public DateTime Crdt { get; set; }
    
        /// <summary>
        /// 
        ///</summary>
        public string Lmid { get; set; }
    
        /// <summary>
        /// 
        ///</summary>
        public DateTime Lmdt { get; set; }
    
   }
}