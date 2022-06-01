using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Core.Encryption.Api
{
    /// <summary>
    /// AuthorizationFilter
    /// </summary>
    public class AuthorizationFilter : IAuthorizationFilter
    {
        /// <summary>
        /// 参数校验
        /// </summary>
        private void CheckParams(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.ContainsKey("timestamp"))
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new JsonResult("缺少timestamp参数");
                return;
            }
        }

        /// <summary>
        /// 构建机密字符串
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string BuildParams(AuthorizationFilterContext context)
        {
            try
            {
                string buildString = string.Empty;
                HttpRequest request = context.HttpContext.Request;
                switch (request.Method)
                {
                    case "POST":
                        #region Form请求数据

                        var reqForms = context.HttpContext.Request.Form.OrderBy(kv => kv.Key);

                        var segments = reqForms.Select(kv => kv.Key + "=" + kv.Value);

                        buildString = string.Join("&", segments);//用&符号拼接起来 

                        #endregion

                        break;

                    case "GET":
                        //第一步：取出所有get参数
                        NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(context.HttpContext.Request.QueryString.ToString());
                        IDictionary<string, string> parameters = new Dictionary<string, string>();
                        for (int f = 0; f < nameValueCollection.Count; f++)
                        {
                            string key = nameValueCollection.Keys[f];
                            parameters.Add(key, nameValueCollection[key]);
                        }
                        //添加应用秘钥构建加密字符串
                        parameters.Add("appsecret", "AppSecret");
                        // 第二步：把字典按Key的字母顺序排序
                        IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
                        IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

                        // 第三步：把所有参数名和参数值串在一起
                        StringBuilder query = new StringBuilder();
                        while (dem.MoveNext())
                        {
                            string key = dem.Current.Key;
                            string value = dem.Current.Value;
                            if (!string.IsNullOrEmpty(key))
                            {
                                query.Append(key).Append("=").Append(value).Append("&");
                            }
                        }
                        buildString = query.ToString().TrimEnd('&');
                        break;
                    default:
                        buildString = string.Empty;
                        break;
                }
                return buildString;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// OnAuthorization
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            
        }
    }
}
