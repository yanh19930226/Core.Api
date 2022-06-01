using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools.Api.Dtos;
using Tools.Api.Models.Mongos;
using Tools.Api.Services;

namespace Tools.Api.Controllers
{
    /// <summary>
    /// Mongo
    /// </summary>
    [ApiController]
    [Route("Api/Mongo")]
    public class MongoController : Controller
    {
        /// <summary>
        /// GetData
        /// </summary>
        /// <param name="mongoSearchDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FetData")]
        [ProducesResponseType(typeof(AdvertiseFee), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AdvertiseFee), StatusCodes.Status404NotFound)]
        public IActionResult FetData([FromBody]MongoSearchDto mongoSearchDto)
        {

            mongoSearchDto.StartTime = DateTime.Now.ToString();
            mongoSearchDto.EndTime = DateTime.Now.ToString();
            var service = new MongoService();
            var res = service.FetData(mongoSearchDto);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        /// <summary>
        /// FetDataPage
        /// </summary>
        /// <param name="mongoSearchDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FetDataPage")]
        [ProducesResponseType(typeof(AdvertiseFee), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AdvertiseFee), StatusCodes.Status404NotFound)]
        public IActionResult FetDataPage([FromBody] SearchPage SearchPage)
        {
            SearchPage.StartTime = DateTime.Now.ToString();
            SearchPage.EndTime = DateTime.Now.ToString();
            SearchPage.PageIndex = 1;
            SearchPage.PageSize = 10;
            var service = new MongoService();
            var res = service.FetDataPage(SearchPage);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }
    }
}
