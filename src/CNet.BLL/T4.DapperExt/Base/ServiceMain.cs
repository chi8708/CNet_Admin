using CNet.DAL;
using CNet.DAL.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNet.BLL.Main
{
    public partial class ServiceMain<T> : RepositoryMain<T> where T : class, new()
    {
    }
}
