using CNet.Model.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNet.BLL.Main
{
    public partial class Wiki_MainBLL
    {
        /// <summary>
        /// 获取编号
        /// </summary>
        /// <param name="pCode"></param>
        /// <returns></returns>
        public string GetSortCode()
        {
            var code = "WS000001";
            List<Wiki_Sort> Sorts = new Wiki_SortBLL().GetList("", " SortCode Desc ", 1);
            if (Sorts.Count > 0)
            {
                var sort = Sorts.First();
                code = "WS" + (Convert.ToInt32(sort.SortCode.Remove(0, 2)) + 1).ToString().PadLeft(6, '0');
            }

            return code;
        }

    }
}
