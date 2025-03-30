
//////此代码由CNetCodeGen生成， 作者：cts 生成时间：2025-03-30 22:08:28
using System;
using Dapper.Contrib.Extensions;
namespace CNet.Model.Main
{
     /// <summary>
    ///  
    ///</summary>
    [Table("testgen")]
    public partial class testgen
    {

        /// <summary>
        /// 创建时间
        ///</summary>
        public DateTime Crdt { get; set; }
    
        /// <summary>
        /// 创建人
        ///</summary>
        public string Crid { get; set; }
    
        /// <summary>
        /// 
        ///</summary>

        [Key]
                public int Id { get; set; }
    
        /// <summary>
        /// 最后更新时间
        ///</summary>
        public DateTime Lmdt { get; set; }
    
        /// <summary>
        /// 最后更新人
        ///</summary>
        public string Lmid { get; set; }
    
        /// <summary>
        /// 备注
        ///</summary>
        public string Remark { get; set; }
    
        /// <summary>
        /// 角色编号
        ///</summary>
        public string RoleCode { get; set; }
    
        /// <summary>
        /// 角色名称
        ///</summary>
        public string RoleName { get; set; }
    
        /// <summary>
        /// 停用状态 默认0  未停用 1 停用
        ///</summary>
        public bool StopFlag { get; set; }
    
   }
}