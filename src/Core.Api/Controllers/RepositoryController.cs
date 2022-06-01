using Core.Net.Data;
using Core.Net.Entity.Model;
using Core.Net.Service;
using Core.Net.Service.Message;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Controllers
{
    /// <summary>
    ///  Repository
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RepositoryController : ControllerBase
    {
        private readonly ICoreCmsMessageServices _service;
        public RepositoryController(ICoreCmsMessageServices service)
        {
            _service = service;
        }
       
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //var res = await _dal.QueryAsync();

            var res = await _service.QueryByIdAsync(1);

            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            CoreCmsMessage coreCmsMessage = new CoreCmsMessage();
            coreCmsMessage.userId = 1;
            coreCmsMessage.code ="11";

            coreCmsMessage.parameters = "11";

            coreCmsMessage.contentBody = "11";

            coreCmsMessage.status = true;

            return Ok();
        }
    }
}
