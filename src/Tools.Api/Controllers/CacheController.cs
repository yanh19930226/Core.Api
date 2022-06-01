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
    public class CacheController : ControllerBase
    {
        private readonly IEasyCachingProvider _cache;
        private readonly IUserService _service;
        public CacheController(IEasyCachingProvider provider, IUserService service)
        {
            this._cache = provider;
            _service = service;
        }
        [HttpGet]
        [Route("add")]
        public async Task<IActionResult> Add()
        {
            IList<User> users = _service.getAll();
            _cache.Set("users", users, TimeSpan.FromMinutes(2));
            await _cache.SetAsync("users2", users, TimeSpan.FromMinutes(3));
            return await Task.FromResult(new JsonResult(new { message = "添加成功！" }));
        }

        [HttpGet]
        [Route("remove")]
        public async Task<IActionResult> Remove()
        {
            _cache.Remove("users");
            await _cache.RemoveAsync("users2");
            return await Task.FromResult(new JsonResult(new { message = "删除成功！" }));
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> Get()
        {
            var users = _cache.Get<List<User>>("users");
            var users2 = await _cache.GetAsync<List<User>>("users2");
            return await Task.FromResult(new JsonResult(new { users1 = users.Value, users2 = users2.Value }));
        }
    }
}
