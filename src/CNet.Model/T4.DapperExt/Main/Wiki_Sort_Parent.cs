//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//	   生成时间 2023-10-29 11:13:59
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失
//     作者：Harbour CTS
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using Dapper.Contrib.Extensions;

namespace CNet.Model.Main
{	
   
   [Table("V_Wiki_Sort_Parent")]
    public partial class V_Wiki_Sort_Parent
    {

	   /// <summary>
     	/// 
     	/// </summary>
		public string SortCode { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string SortName { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string Remark { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string ParentCode { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public int? SortLevel { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string Lmid { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public DateTime? Lmdt { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public bool? StopFlag { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string parentName { get; set; }

		   
    }
}

