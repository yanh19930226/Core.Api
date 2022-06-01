using EasyCaching.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools.Api.Models;
using Tools.Api.Services;

namespace Tools.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Cache2Controller : ControllerBase
    {
        private readonly IEasyCachingProviderFactory _cacheFactory;
        private readonly IUserService _userService;
        public Cache2Controller(IEasyCachingProviderFactory cacheFactory, IUserService userService)
        {
            this._cacheFactory = cacheFactory;
            this._userService = userService;
        }

        [HttpGet]
        [Route("add")]
        public async Task<IActionResult> Add()
        {
            var _cache1 = _cacheFactory.GetCachingProvider("m1");   //获取名字为m1的provider
            var _cache2 = _cacheFactory.GetCachingProvider("localhostRedis");   //获取名字为localhostRedis的provider
            IList<User> users = _userService.getAll();
            IList<string> loginNames = users.Select(s => s.username).ToList();
            _cache1.Set("loginNames", loginNames, TimeSpan.FromMinutes(2));
            await _cache2.SetAsync("users", users, TimeSpan.FromMinutes(2));
            return await Task.FromResult(new JsonResult(new { message = "添加成功！" }));
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> Get()
        {
            var _cache1 = _cacheFactory.GetCachingProvider("m1");   //获取名字为m1的provider
            var _cache2 = _cacheFactory.GetCachingProvider("localhostRedis");   //获取名字为localhostRedis的provider
            IList<string> loginNames = _cache1.Get<List<string>>("loginNames").Value;
            IList<User> users = (await _cache2.GetAsync<List<User>>("users")).Value;
            return await Task.FromResult(new JsonResult(new { loginNames = loginNames, users = users }));
        }
    }
}
