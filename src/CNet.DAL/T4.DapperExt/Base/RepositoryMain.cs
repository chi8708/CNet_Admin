using CNet.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNet.Main.DAL
{
	public class RepositoryMain<T> : BaseDataDapperContribSqlServer<T> where T : class, new()
	{
        public RepositoryMain() : base(Connection.MainStr)
		{

		}
		public string ConnStr => Connection.MainStr;

	}
}
