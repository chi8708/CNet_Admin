using CNet.DAL;
using CNet.Main.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNet.Main.BLL
{
    public partial class ServiceMain<T> : RepositoryMain<T> where T : class, new()
    {
    }
}
