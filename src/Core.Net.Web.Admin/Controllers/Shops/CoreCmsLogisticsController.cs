using Core.Net.Entity.Model.Expression;
using Core.Net.Entity.Model.Shops;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.Shops;
using Core.Net.Util.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Net.Web.Admin.Controllers.Shops
{

    /// <summary>
    ///     物流公司表
    /// </summary>
    [Description("物流公司表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CoreCmsLogisticsController : Controller
    {
        private readonly ICoreCmsLogisticsServices _coreCmsLogisticsServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <param name="coreCmsLogisticsServices"></param>
        public CoreCmsLogisticsController(IWebHostEnvironment webHostEnvironment,
            ICoreCmsLogisticsServices coreCmsLogisticsServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsLogisticsServices = coreCmsLogisticsServices;
        }
        #region 首页数据============================================================

        // POST: Api/CoreCmsLogistics/GetIndex
        /// <summary>
        ///     首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public JsonResult GetIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            return Json(jm);
        }

        #endregion

        #region 获取列表============================================================

        // POST: Api/CoreCmsLogistics/GetPageList
        /// <summary>
        ///     获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<JsonResult> GetPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsLogistics>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsLogistics, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "logiName":
                    orderEx = p => p.logiName;
                    break;
                case "logiCode":
                    orderEx = p => p.logiCode;
                    break;
                case "sort":
                    orderEx = p => p.sort;
                    break;
                case "isDelete":
                    orderEx = p => p.isDelete;
                    break;
                default:
                    orderEx = p => p.id;
                    break;
            }

            //设置排序方式
            var orderDirection = Request.Form["orderDirection"].FirstOrDefault();
            var orderBy = orderDirection switch
            {
                "asc" => OrderByType.Asc,
                "desc" => OrderByType.Desc,
                _ => OrderByType.Desc
            };
            //查询筛选

            //序列 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
            //物流公司名称 nvarchar
            var logiName = Request.Form["logiName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(logiName)) @where = @where.And(p => p.logiName.Contains(logiName));
            //物流公司编码 nvarchar
            var logiCode = Request.Form["logiCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(logiCode)) @where = @where.And(p => p.logiCode.Contains(logiCode));
            //排序 int
            var sort = Request.Form["sort"].FirstOrDefault().ObjectToInt(0);
            if (sort > 0) @where = @where.And(p => p.sort == sort);
            //是否删除 bit
            var isDelete = Request.Form["isDelete"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isDelete) && isDelete.ToLowerInvariant() == "true")
                @where = @where.And(p => p.isDelete);
            else if (!string.IsNullOrEmpty(isDelete) && isDelete.ToLowerInvariant() == "false")
                @where = @where.And(p => p.isDelete == false);
            //获取数据
            var list = await _coreCmsLogisticsServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return Json(jm);
        }

        #endregion

        #region 拉取数据更新============================================================

        // POST: Api/CoreCmsLogistics/DoDelete/10
        /// <summary>
        ///     拉取数据更新
        /// </summary>
        [HttpPost]
        [Description("单选删除")]
        public async Task<JsonResult> DoUpdateCompany()
        {
            var jm = await _coreCmsLogisticsServices.DoUpdateCompany();
            return Json(jm);

        }

        #endregion
    }
}
