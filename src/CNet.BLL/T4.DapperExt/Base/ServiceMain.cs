using CNet.BLL;
using CNet.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNet.Main.BLL
{
    public partial class ServiceMain<T> : BaseServiceDapperContrib<T> where T : class, new()
    {
        public ServiceMain() : base(BaseDataDapperContribFactory<T>.GetInstance())
        {
        }
    }
}
