using Core.Net.Configuration;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.Shops;
using Core.Net.Util.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Net.Web.Admin.Controllers.Shops
{
    /// <summary>
    /// 平台设置表
    ///</summary>
    [Description("平台设置表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CoreCmsSettingController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsSettingServices _coreCmsSettingServices;

        /// <summary>
        /// 构造函数
        ///</summary>
        ///  <param name="webHostEnvironment"></param>
        ///<param name="CoreCmsSettingServices"></param>
        public CoreCmsSettingController(IWebHostEnvironment webHostEnvironment, ICoreCmsSettingServices CoreCmsSettingServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsSettingServices = CoreCmsSettingServices;
        }

        #region 首页数据============================================================
        // POST: Api/CoreCmsSetting/GetIndex
        /// <summary>
        /// 首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public async Task<JsonResult> GetIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            var configs = await _coreCmsSettingServices.GetConfigDictionaries();
            var filesStorageOptionsType = EnumHelper.EnumToList<GlobalEnumVars.FilesStorageOptionsType>();

            jm.data = new
            {
                configs,
                filesStorageOptionsType
            };

            return new JsonResult(jm);
        }
        #endregion

        #region 保存提交============================================================
        //// POST: Api/CoreCmsSetting/DoSave
        ///// <summary>
        ///// 保存提交
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Description("保存提交")]
        //public async Task<JsonResult> DoSave([FromBody] FMCoreCmsSettingDoSaveModel model)
        //{
        //    var jm = await _coreCmsSettingServices.UpdateAsync(model);
        //    return new JsonResult(jm);
        //}
        #endregion
    }
}
