using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Entity.Response
{
    public class APIResponse
    {
        /// <summary>
        /// 错误码
        /// 1表示成功
        /// 100***：系统级基础错误  
        /// 100000: NULL
        /// 100001: SIGN_ERROR
        /// 100002: APP_AUTH_ERROR
        /// 100003: USER_AUTH_ERROR
        /// 100004: SYSTEM_ERROR
        /// 100005: 参数验证失败
        /// 100006：服务器操作失败
        /// 
        /// 200***：用户相关错误
        /// 200001：登录名或密码错误
        /// 200002: 您的账户已经被禁用，请联系管理员
        /// 200003: 用户授权失败
        /// 200004: 文件不合法
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 子错误码
        /// </summary>
        public string SubCode { get; set; }

        /// <summary>
        /// 子错误信息
        /// </summary>
        public string SubMessage { get; set; }

        /// <summary>
        /// 禁止访问字段
        /// </summary>
        public string ForbiddenFields { get; set; }

        /// <summary>
        /// 响应原始内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// HTTP GET请求的URL
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 响应结果是否错误
        /// </summary>
        public bool IsError
        {
            get
            {
                return !string.IsNullOrEmpty(this.Code) || !string.IsNullOrEmpty(this.SubCode);
            }
        }
    }
}
