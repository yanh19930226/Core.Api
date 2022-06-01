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
    public class CacheAopController : Controller
    {
        private readonly IEasyCachingProvider _cache;
        private readonly IUserService _service;
        public CacheAopController(IEasyCachingProvider cache, IUserService service)
        {
            this._cache = cache;
            this._service = service;
        }

        [HttpGet]
        [Route("get/{id}")]
        public User GetUserById(int id)
        {
            return _service.getById(id);
        }
    }
}
