using CNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNet.DAL
{
    public class BaseDataDapperContribFactory
    {
        public static IBaseDataDapperContrib<T> GetInstance<T>() where T:  class,new()
        {
            return new BaseDataDapperContribMySql<T>();
            //ICustomeContainer container = new CustomeContainer();//创建容器 
            //container.RegisterType<IBaseDataDapperContrib<T>, BaseDataDapperContrib<T>>();
            //IBaseDataDapperContrib<T> baseDataDapperContrib = container.Resolve<IBaseDataDapperContrib<T>>();
            //return baseDataDapperContrib;
        }
    }
}
