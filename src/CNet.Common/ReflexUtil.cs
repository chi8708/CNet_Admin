using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNet.Common
{
    public class ReflexUtil
    {

        /// <summary>
        /// 比较字段是否修改
        /// </summary>
        /// <param name="oldClient">原Model</param>
        /// <param name="client">修改后的Model</param>
        /// <param name="ignoreProperties">忽略的属性名</param>
        public static bool IsChange<T>(T oldModel, T newModel, List<string> includeProperties = null, List<string> ignoreProperties = null)
            where T : class,new()
         {
            var newProperties = oldModel.GetType().GetProperties();
            if (includeProperties != null)
            {
                newProperties = newProperties.Where(p => includeProperties.Contains(p.Name)).ToArray();
            }
            else if (ignoreProperties != null)
            {
                newProperties = newProperties.Where(p => !ignoreProperties.Contains(p.Name)).ToArray();
            }

            foreach (var item in newProperties)
            {
                var oldValue = item.GetValue(oldModel) ?? "";
                var newValue = item.GetValue(newModel) ?? "";
                if (!oldValue.Equals(newValue))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
