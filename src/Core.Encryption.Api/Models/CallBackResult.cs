using System;
using System.ComponentModel;

namespace Core.Encryption.Api.Models
{
    /// <summary>
    /// 响应实体
    /// </summary>
    public class CallBackResult
    {
        /// <summary>
        /// 响应码
        /// </summary>
        public CallBackResultCode Code { get; set; }
        /// <summary>
        /// 响应信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 成功
        /// </summary>
        public bool IsSuccess => Code == CallBackResultCode.SUCCESS;
        /// <summary>
        /// 时间戳(毫秒)
        /// </summary>
        public long Timestamp { get; } = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
        /// <summary>
        /// 响应成功
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public void Success(string message = "")
        {
            Message = message;
            Code = CallBackResultCode.SUCCESS;
        }
        /// <summary>
        /// 响应失败
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public void Failed(CallBackResultCode callBackResultCode, string message = "")
        {
            Message = message;
            Code = callBackResultCode;
        }
    }
    /// <summary>
    /// 响应实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CallBackResult<T> : CallBackResult
    {
        /// <summary>
        /// 返回结果
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 响应成功
        /// </summary>
        /// <param name="result"></param>
        /// <param name="message"></param>
        public void Success(T result = default(T), string message = "")
        {
            Message = message;
            Code = CallBackResultCode.SUCCESS;
            Data = result;
        }
    }
    
    /// <summary>
    /// 响应状态码
    /// </summary>
    public enum CallBackResultCode
    {

        #region 响应成功
        /// <summary>
        /// SUCCESS
        /// </summary>
        [Description("SUCCESS")]
        SUCCESS = 000000,
        #endregion

        #region 验证失败
        /// <summary>
        /// SIGNERROR
        /// </summary>
        [Description("SIGN_ERROR")]
        SIGN_ERROR = 100001,
        /// <summary>
        /// APP_AUTH_ERROR
        /// </summary>
        [Description("APP_AUTH_ERROR")]
        APP_AUTH_ERROR = 100002,
        /// <summary>
        /// USER_AUTH_ERROR
        /// </summary>
        [Description("USER_AUTH_ERROR")]
        USER_AUTH_ERROR = 100003,
        /// <summary>
        /// VALID_ERROR
        /// </summary>
        [Description("VALID_ERROR")]
        VALID_ERROR = 100004,
        #endregion

        #region 系统错误
        /// <summary>
        /// SYSTEM_ERROR
        /// </summary>
        [Description("SYSTEM_ERROR")]
        SYSTEM_ERROR = 90000, 
        #endregion
    }
}
