using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Core.Net.Configuration;
using Core.Net.Redis;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Core.Net.Configuration.GlobalConstVars;

namespace Core.Api.Controllers
{
    /// <summary>
    ///  Redis消息队列
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RedisController : ControllerBase
    {
        private readonly IRedisOperationRepository _redisOperationRepository;
        public RedisController(IRedisOperationRepository redisOperationRepository)
        {
            _redisOperationRepository = redisOperationRepository;
        }
        /// <summary>
        ///Redis基本Ge测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            var res= await _redisOperationRepository.Get("testKey");

            return Ok(res);
        }
        /// <summary>
        ///Redis基本Set测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Set")]
        public async Task<IActionResult> Set()
        {
            await _redisOperationRepository.Set("testKey", "testVal", TimeSpan.FromMinutes(10));

            return Ok();
        }
        /// <summary>
        /// Redis消息队列
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Send")]
        public async Task<IActionResult> Send()
        {
            TestMq testMq = new TestMq() { 
               Age=10,
               Name="22"
            };

            if (AppSettingsConstVars.RedisConfigUseRedisMessageQueue)
            {
                //结佣处理
                await _redisOperationRepository.ListLeftPushAsync(RedisMessageQueueKey.LogingQueue, JsonConvert.SerializeObject(testMq));
            }

            return Ok();
        }
    }
    /// <summary>
    /// 测试消息模型
    /// </summary>
    public  class TestMq
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
    }
}