using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNet.Model
{
    public class PageDateRes<T> where T : class, new()
    {
        public ResCode code { get; set; }

        public string msg { get; set; }

        public int count { get; set; }

        public int totalPage { get; set; }

        public List<T> data { get; set; }

        public int pageNum { get; set; }

        public int pageSize { get; set; }
    }
}
