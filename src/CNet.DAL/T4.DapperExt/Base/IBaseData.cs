using CNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNet.DAL
{
   public  interface IBaseData<T> where T : class, new()
    {
        public long Insert(T model);
        public bool InsertBatch(List<T> models);
        public bool Update(T model);
        public bool UpdateBatch(List<T> models);
        public dynamic Delete(T model);
        public bool DeleteByWhere(string where, object param = null);
        public bool DeleteBatch(List<T> models);
        public T Get(object id);
        public T Get(object id, string keyName);

        public List<T> GetList(string where, string sort = null, int limits = -1, string fileds = " * ", string orderby = "");
        public PageDateRes<T> GetPage(string where, string sort, int page, int resultsPerPage, string fields = "*", Type result = null);
        public bool ChangeSotpStatus(string where);

    }
}
