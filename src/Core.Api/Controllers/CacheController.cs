using Core.Net.Cache;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Controllers
{
    /// <summary>
    ///  Cache
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CacheController : ControllerBase
    {
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Set")]
        public IActionResult Set()
        {
            CacheManagerFactory.Instance.Set("CacheKey", "CacheVal",1);
            return Ok();
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var res=CacheManagerFactory.Instance.Get<string>("CacheKey");
            return Ok(res);
        }
    }
}
