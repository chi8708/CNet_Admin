using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CNet.Web.Api
{

    /// <summary>
    /// 搜索条件
    /// </summary>
    public static class QueryHelper
    {

        /// <summary>
        /// 获取搜索query
        /// </summary>
        public static string GetWhereStr(this HttpContext context)
        {

            var request = context.Request;
            var body = request.Body;
            var postData = "";
            var query = new Dictionary<string, object>();
            var r = GetQuery();
            if (!r)
            {
                return "-1";
            }
            bool GetQuery()
            {
                try
                {

                    if (request.ContentType.IndexOf("application/json") >= 0)
                    {
                        //request.Body.Position = 0;
                        //using var requestReader = new StreamReader(request.Body);
                        //var requestContent = requestReader.ReadToEnd();
                        //request.Body.Position = 0;

                        body.Position = 0;//body.Position = 0;才能读到数据
                        var bytes = new byte[body.Length];
                        body.Read(bytes, 0, bytes.Length);
                        postData = System.Text.Encoding.UTF8.GetString(bytes);
                    }
                    if (string.IsNullOrEmpty(postData))
                    {
                        return false;
                    }

                    var postDataJson = JsonConvert.DeserializeObject<dynamic>(postData);
                    query = JsonConvert.DeserializeObject<Dictionary<string, object>>(postDataJson.query.ToString());
                    if (query == null || query.Keys.Count <= 0)
                    {
                        return true;
                    }
                    return true;
                }
                catch (Exception ex)
                {

                    return false;
                }
            }

            return context.GetWhereStr(query);
        }

        public static string GetWhereStr(this HttpContext context, Dictionary<string, object> query)
        {
            string strWhere = " 1=1 ";
            if (query == null)
            {
                return strWhere;
            }
            var keys = query.Select(p => p.Key);
            var parms = keys.Where(p => (p.Contains("SL_")
                || p.Contains("SEB_")) || p.Contains("SES_") || p.Contains("SEGT_") || p.Contains("SELT_") || p.Contains("SEI_") || p.Contains("SENE_") || p.Contains("SLL_") || p.Contains("SLR_"));
            foreach (var parm in parms)
            {
                var name = parm.Split('_');
                var keyPosition = name[0].Length + 1;
                var fieldName = parm.Substring(keyPosition, parm.Length - keyPosition);

                var value = query[parm].ToString().Trim();
                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                value = SqlFilter(value);

                switch (name[0])
                {
                    case "SL": strWhere += string.Format(" And {0} like '%{1}%' ", fieldName, value); break;
                    case "SLL": strWhere += string.Format(" And {0} like '%{1}' ", fieldName, value); break;
                    case "SLR": strWhere += string.Format(" And {0} like '{1}%' ", fieldName, value); break;
                    case "SEB": strWhere += string.Format(" And {0}={1} ", fieldName, value); break;
                    case "SEI": strWhere += string.Format(" And {0} in({1})", fieldName, value); break;
                    case "SES": strWhere += string.Format(" And {0}='{1}'", fieldName, value); break;
                    case "SEGT": strWhere += string.Format(" And {0}>='{1}' ", fieldName, value); break;
                    case "SELT": strWhere += string.Format(" And {0}<='{1}' ", fieldName, value); break;
                    case "SENE": strWhere += string.Format(" And {0}<>'{1}' ", fieldName, value); break;
                    default:
                        break;
                }
            }

            return strWhere;
        }
        /// <summary>
        /// 过滤危险的字符（SQL注入）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SqlFilter(this string str)
        {
            var ext = new[] { "and ", "exec ", "insert ", "select ", "delete ", "update ", "chr ", "mid ", "master ", "or ", "truncate ", "char ", "declare ", "join ", "\r", "\n", "'" };

            if (str.Contains("'"))
            {
                str = str.Replace("'", "''");
            }
            else
            {
                if (!string.IsNullOrEmpty(str) && str.Length >= 3)
                {
                    foreach (var e in ext.Where(e => str.ToLower().IndexOf(e, StringComparison.Ordinal) != -1))
                    {
                        str = Regex.Replace(str, e, "", RegexOptions.IgnoreCase);
                    }
                }

                if (str.Length >= 128)
                    str = "";
            }


            return str;
        }



        #region 字符串过滤 sql注入、跨站脚本注入等
        ///<summary>
        /// 过滤危险字符
        ///</summary>
        ///<param name="inputString">需要过滤字符串</param>
        ///<returns>过滤后的字符串</returns>
        public static string InjectionFilter(string inputString)
        {
            if (!string.IsNullOrEmpty(inputString))
            {
                string xss = XSSFilter(inputString);
                string flash = FlashFilter(xss);
                string sql = SqlFilter(flash);
                return sql;
            }
            else
            {
                return string.Empty;
            }
        }

        ///<summary>
        /// 过滤字符串中的注入跨站脚本(先进行UrlDecode再过滤脚本关键字)
        /// 过滤脚本如:<script>window.alert("test");</script>输出window.alert("test");
        /// 如<a href = "javascript:onclick='fun1();'">输出<a href=" onXXX='fun1();'">
        /// 过滤掉javascript和 onXXX
        ///</summary>
        ///<param name="source">需要过滤的字符串</param>
        ///<returns>过滤后的字符串</returns>
        private static string XSSFilter(string source)
        {
            if (string.IsNullOrEmpty(source)) return source;

            string result = HttpUtility.UrlDecode(source);

            string replaceEventStr = " onXXX =";
            string tmpStr = "";

            string patternGeneral = @"<[^<>]*>";                              //例如 <abcd>
            string patternEvent = @"([\s]|[:])+[o]{1}[n]{1}\w*\s*={1}";     // 空白字符或: on事件=
            string patternScriptBegin = @"\s*((javascript)|(vbscript))\s*[:]?";  // javascript或vbscript:
            string patternScriptEnd = @"<([\s/])*script.*>";                       // </script>
            string patternTag = @"<([\s/])*\S.+>";                       // 例如</textarea>

            Regex regexGeneral = new Regex(patternGeneral, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Regex regexEvent = new Regex(patternEvent, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Regex regexScriptEnd = new Regex(patternScriptEnd, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex regexScriptBegin = new Regex(patternScriptBegin, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex regexTag = new Regex(patternTag, RegexOptions.Compiled | RegexOptions.IgnoreCase);


            Match matchGeneral, matchEvent, matchScriptEnd, matchScriptBegin, matchTag;

            //符合类似 <abcd> 条件的
            #region 符合类似 <abcd> 条件的
            //过滤字符串中的 </script>   
            for (matchGeneral = regexGeneral.Match(result); matchGeneral.Success; matchGeneral = matchGeneral.NextMatch())
            {
                tmpStr = matchGeneral.Groups[0].Value;
                matchScriptEnd = regexScriptEnd.Match(tmpStr);
                if (matchScriptEnd.Success)
                {
                    tmpStr = regexScriptEnd.Replace(tmpStr, "");
                    result = result.Replace(matchGeneral.Groups[0].Value, tmpStr);
                }
            }

            //过滤字符串中的脚本事件
            for (matchGeneral = regexGeneral.Match(result); matchGeneral.Success; matchGeneral = matchGeneral.NextMatch())
            {
                tmpStr = matchGeneral.Groups[0].Value;
                matchEvent = regexEvent.Match(tmpStr);
                if (matchEvent.Success)
                {
                    tmpStr = regexEvent.Replace(tmpStr, replaceEventStr);
                    result = result.Replace(matchGeneral.Groups[0].Value, tmpStr);
                }
            }

            //过滤字符串中的 javascript或vbscript:
            for (matchGeneral = regexGeneral.Match(result); matchGeneral.Success; matchGeneral = matchGeneral.NextMatch())
            {
                tmpStr = matchGeneral.Groups[0].Value;
                matchScriptBegin = regexScriptBegin.Match(tmpStr);
                if (matchScriptBegin.Success)
                {
                    tmpStr = regexScriptBegin.Replace(tmpStr, "");
                    result = result.Replace(matchGeneral.Groups[0].Value, tmpStr);
                }
            }

            #endregion

            //过滤字符串中的事件 例如 onclick --> onXXX
            for (matchEvent = regexEvent.Match(result); matchEvent.Success; matchEvent = matchEvent.NextMatch())
            {
                tmpStr = matchEvent.Groups[0].Value;
                tmpStr = regexEvent.Replace(tmpStr, replaceEventStr);
                result = result.Replace(matchEvent.Groups[0].Value, tmpStr);
            }

            //过滤掉html标签，类似</textarea>
            for (matchTag = regexTag.Match(result); matchTag.Success; matchTag = matchTag.NextMatch())
            {
                tmpStr = matchTag.Groups[0].Value;
                tmpStr = regexTag.Replace(tmpStr, "");
                result = result.Replace(matchTag.Groups[0].Value, tmpStr);
            }


            return result;
        }

        ///<summary>
        /// 过滤字符串中注入Flash代码
        ///</summary>
        ///<param name="htmlCode">输入字符串</param>
        ///<returns>过滤后的字符串</returns>
        private static string FlashFilter(string htmlCode)
        {
            if (string.IsNullOrEmpty(htmlCode)) return htmlCode;

            string pattern = @"\w*<OBJECT\s+.*(macromedia)[\s*|\S*]{1,1300}</OBJECT>";

            return Regex.Replace(htmlCode, pattern, "", RegexOptions.Multiline);
        }


        #endregion

    }
}
