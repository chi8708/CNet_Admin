using CNet.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNet.Main.DAL
{
	public class RepositoryMain<T>:BaseDataDapperContribMySql<T> where T : class,new()
	{
	}
}
