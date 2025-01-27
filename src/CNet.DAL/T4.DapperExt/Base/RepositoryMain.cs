﻿using CNet.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNet.DAL.Main
{
	public class RepositoryMain<T> : BaseDataDapperContribMySql<T> where T : class, new()
	{
        public RepositoryMain() : base(Connection.MainStr)
		{

		}
		public string ConnStr => Connection.MainStr;

	}
}
