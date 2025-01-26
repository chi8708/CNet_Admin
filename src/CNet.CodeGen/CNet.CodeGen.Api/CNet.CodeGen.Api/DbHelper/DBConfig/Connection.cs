using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNet.CodeGen.DB
{
	public class Connection
	{
		public static string MainStr => AppConfigurtaionServices.Configuration.GetConnectionString("connStr_Main");

		public static string ShopStr => AppConfigurtaionServices.Configuration.GetConnectionString("connStr_Shop");
	}
}
