//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//	   生成时间 2023-04-01 14:37:47
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失
//     作者：Harbour CTS
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using Dapper.Contrib.Extensions;

namespace CNet.Model.Main
{	
   
   [Table("Pub_Department")]
    public partial class Pub_Department
    {

	   /// <summary>
     	/// 部门编号
     	/// </summary>
		[ExplicitKey]
		public string DeptCode { get; set; }

		/// <summary>
     	/// 部门名称
     	/// </summary>
		public string DeptName { get; set; }

		/// <summary>
     	/// 备注
     	/// </summary>
		public string Remark { get; set; }

		/// <summary>
     	/// 上级部门编号
     	/// </summary>
		public string ParentCode { get; set; }

		/// <summary>
     	/// 部门级别
     	/// </summary>
		public int DeptLevel { get; set; }

		/// <summary>
     	/// 最后编辑人
     	/// </summary>
		public string Lmid { get; set; }

		/// <summary>
     	/// 最后编辑时间
     	/// </summary>
		public DateTime? Lmdt { get; set; }

		/// <summary>
     	/// 停用状态 默认0 未停用 1 停用
     	/// </summary>
		public bool? StopFlag { get; set; }

		   
    }
}

