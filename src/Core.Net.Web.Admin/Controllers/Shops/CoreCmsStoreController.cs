using Core.Net.Configuration;
using Core.Net.Entity.Dtos;
using Core.Net.Entity.Model.Expression;
using Core.Net.Entity.Model.Shops;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.Shops;
using Core.Net.Service.Users;
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
    ///     门店表
    /// </summary>
    [Description("门店表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CoreCmsStoreController : Controller
    {
        private readonly ICoreCmsStoreServices _coreCmsStoreServices;
        private readonly ICoreCmsClerkServices _coreCmsClerkServices;
        private readonly ICoreCmsUserServices _coreCmsUserServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        public CoreCmsStoreController(IWebHostEnvironment webHostEnvironment,
            ICoreCmsStoreServices coreCmsStoreServices, ICoreCmsClerkServices coreCmsClerkServices,
            ICoreCmsUserServices coreCmsUserServices
           )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsStoreServices = coreCmsStoreServices;
            _coreCmsClerkServices = coreCmsClerkServices;
            _coreCmsUserServices = coreCmsUserServices;
        }
        #region 首页数据============================================================

        // POST: Api/CoreCmsStore/GetIndex
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

        // POST: Api/CoreCmsStore/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsStore>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsStore, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "storeName":
                    orderEx = p => p.storeName;
                    break;
                case "mobile":
                    orderEx = p => p.mobile;
                    break;
                case "linkMan":
                    orderEx = p => p.linkMan;
                    break;
                case "logoImage":
                    orderEx = p => p.logoImage;
                    break;
                case "areaId":
                    orderEx = p => p.areaId;
                    break;
                case "address":
                    orderEx = p => p.address;
                    break;
                case "coordinate":
                    orderEx = p => p.coordinate;
                    break;
                case "latitude":
                    orderEx = p => p.latitude;
                    break;
                case "longitude":
                    orderEx = p => p.longitude;
                    break;
                case "createTime":
                    orderEx = p => p.createTime;
                    break;
                case "updateTime":
                    orderEx = p => p.updateTime;
                    break;
                default:
                    orderEx = p => p.isDefault;
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
            //门店名称 nvarchar
            var storeName = Request.Form["storeName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(storeName)) @where = @where.And(p => p.storeName.Contains(storeName));
            //门店电话/手机号 nvarchar
            var mobile = Request.Form["mobile"].FirstOrDefault();
            if (!string.IsNullOrEmpty(mobile)) @where = @where.And(p => p.mobile.Contains(mobile));
            //门店联系人 nvarchar
            var linkMan = Request.Form["linkMan"].FirstOrDefault();
            if (!string.IsNullOrEmpty(linkMan)) @where = @where.And(p => p.linkMan.Contains(linkMan));
            //门店logo nvarchar
            var logoImage = Request.Form["logoImage"].FirstOrDefault();
            if (!string.IsNullOrEmpty(logoImage)) @where = @where.And(p => p.logoImage.Contains(logoImage));
            //门店地区id int
            var areaId = Request.Form["areaId"].FirstOrDefault().ObjectToInt(0);
            if (areaId > 0) @where = @where.And(p => p.areaId == areaId);
            //门店详细地址 nvarchar
            var address = Request.Form["address"].FirstOrDefault();
            if (!string.IsNullOrEmpty(address)) @where = @where.And(p => p.address.Contains(address));
            //坐标位置 nvarchar
            var coordinate = Request.Form["coordinate"].FirstOrDefault();
            if (!string.IsNullOrEmpty(coordinate)) @where = @where.And(p => p.coordinate.Contains(coordinate));
            //纬度 nvarchar
            var latitude = Request.Form["latitude"].FirstOrDefault();
            if (!string.IsNullOrEmpty(latitude)) @where = @where.And(p => p.latitude.Contains(latitude));
            //经度 nvarchar
            var longitude = Request.Form["longitude"].FirstOrDefault();
            if (!string.IsNullOrEmpty(longitude)) @where = @where.And(p => p.longitude.Contains(longitude));
            //创建时间 datetime
            var createTime = Request.Form["createTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(createTime))
            {
                if (createTime.Contains("到"))
                {
                    var dts = createTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.createTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.createTime < dtEnd);
                }
                else
                {
                    var dt = createTime.ObjectToDate();
                    where = where.And(p => p.createTime > dt);
                }
            }

            //更新时间 datetime
            var updateTime = Request.Form["updateTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(updateTime))
            {
                if (updateTime.Contains("到"))
                {
                    var dts = updateTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.updateTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.updateTime < dtEnd);
                }
                else
                {
                    var dt = updateTime.ObjectToDate();
                    where = where.And(p => p.updateTime > dt);
                }
            }

            //获取数据
            var list = await _coreCmsStoreServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return Json(jm);
        }

        #endregion

        #region 创建数据============================================================

        // POST: Api/CoreCmsStore/GetCreate
        /// <summary>
        ///     创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public JsonResult GetCreate()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            return Json(jm);
        }

        #endregion

        #region 创建提交============================================================

        // POST: Api/CoreCmsStore/DoCreate
        /// <summary>
        ///     创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<JsonResult> DoCreate([FromBody] CoreCmsStore entity)
        {
            var jm = new AdminUiCallBack();

            var isDefaultObj = _coreCmsStoreServices.QueryByClauseAsync(p => p.isDefault == true).Result;
            if (isDefaultObj!=null)
            {
              entity.isDefault = false;
            }
            else
            {
                entity.isDefault = true;
            }
            entity.createTime = DateTime.Now;
            entity.updateTime = DateTime.Now;
            entity.distance = 0;
            if (entity.coordinate.Contains(","))
            {
                var latlong = entity.coordinate.Split(",");
                entity.latitude = latlong[0];
                entity.longitude = latlong[1];
            }
            var id = await _coreCmsStoreServices.InsertAsync(entity);
            var bl = id > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;
            return new JsonResult(jm);
        }

        #endregion

        #region 编辑数据============================================================

        // POST: Api/CoreCmsStore/GetEdit
        /// <summary>
        ///     编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑数据")]
        public async Task<JsonResult> GetEdit([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsStoreServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return Json(jm);
            }

            jm.code = 0;
            jm.data = model;

            return Json(jm);
        }

        #endregion

        #region 编辑提交============================================================

        // POST: Admins/CoreCmsStore/Edit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<JsonResult> DoEdit([FromBody] CoreCmsStore entity)
        {
            entity.updateTime = DateTime.Now;

           var  jm = await _coreCmsStoreServices.UpdateAsync(entity);

            return Json(jm);
        }

        #endregion

        #region 删除数据============================================================

        // POST: Api/CoreCmsStore/DoDelete/10
        /// <summary>
        ///     单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<JsonResult> DoDelete([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsStoreServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return Json(jm);
            }

            var bl = await _coreCmsStoreServices.DeleteByIdAsync(entity.id);
            if (bl)
            {
                await _coreCmsClerkServices.DeleteAsync(p => p.storeId == model.id);
            }

            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return Json(jm);

        }

        #endregion

        //店员设置

        #region 店员首页数据============================================================

        // POST: Api/CoreCmsStore/GetClerkIndex
        /// <summary>
        ///     店员首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("店员首页数据")]
        public JsonResult GetClerkIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            return Json(jm);
        }

        #endregion

        #region 获取列表============================================================

        // POST: Api/CoreCmsStore/GetClerkPageList
        /// <summary>
        ///     获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<JsonResult> GetClerkPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<StoreClerkDto>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<StoreClerkDto, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "storeId":
                    orderEx = p => p.storeId;
                    break;
                case "userId":
                    orderEx = p => p.userId;
                    break;
                case "isDel":
                    orderEx = p => p.isDel;
                    break;
                case "createTime":
                    orderEx = p => p.createTime;
                    break;
                case "updateTime":
                    orderEx = p => p.updateTime;
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
            //店铺ID int
            var storeId = Request.Form["storeId"].FirstOrDefault().ObjectToInt(0);
            if (storeId > 0) @where = @where.And(p => p.storeId == storeId);
            //用户ID int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0) @where = @where.And(p => p.userId == userId);
            //是否删除 bit
            var isDel = Request.Form["isDel"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isDel) && isDel.ToLowerInvariant() == "true")
                @where = @where.And(p => p.isDel);
            else if (!string.IsNullOrEmpty(isDel) && isDel.ToLowerInvariant() == "false")
                @where = @where.And(p => p.isDel == false);
            //创建时间 datetime
            var createTime = Request.Form["createTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(createTime))
            {
                if (createTime.Contains("到"))
                {
                    var dts = createTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.createTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.createTime < dtEnd);
                }
                else
                {
                    var dt = createTime.ObjectToDate();
                    where = where.And(p => p.createTime > dt);
                }
            }

            //删除时间 datetime
            var updateTime = Request.Form["updateTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(updateTime))
            {
                if (updateTime.Contains("到"))
                {
                    var dts = updateTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.updateTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.updateTime < dtEnd);
                }
                else
                {
                    var dt = updateTime.ObjectToDate();
                    where = where.And(p => p.updateTime > dt);
                }
            }

            //获取数据
            var list = await _coreCmsClerkServices.QueryStoreClerkDtoPageAsync(where, orderEx, orderBy, pageCurrent, pageSize, true);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return Json(jm);
        }

        #endregion

        #region 创建店员数据============================================================

        // POST: Api/CoreCmsStore/GetClerkCreate
        /// <summary>
        ///     创建店员数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建店员数据")]
        public JsonResult GetClerkCreate()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            return Json(jm);
        }

        #endregion

        #region 创建店员交============================================================

        // POST: Api/CoreCmsStore/DoClerkCreate
        /// <summary>
        ///     创建店员提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建店员提交")]
        public async Task<JsonResult> DoClerkCreate([FromBody] FMStoreClerkCURDPost entity)
        {
            var jm = new AdminUiCallBack();

            if (string.IsNullOrEmpty(entity.phone))
            {
                jm.msg = "请输入合法的手机号码";
                return Json(jm);
            }

            if (entity.storeId == 0)
            {
                jm.msg = "请选择要添加的门店";
                return Json(jm);
            }

            var haveStore = await _coreCmsStoreServices.ExistsAsync(p => p.id == entity.storeId);
            if (!haveStore)
            {
                jm.msg = "门店信息不存在";
                return Json(jm);
            }

            //判断是否存在当前的门店内
            var user = await _coreCmsUserServices.QueryByClauseAsync(p => p.mobile == entity.phone);
            if (user == null)
            {
                jm.msg = "不存在此手机注册用户";
                return Json(jm);
            }

            var haveClerk =
                await _coreCmsClerkServices.ExistsAsync(p => p.userId == user.id && p.storeId == entity.storeId);
            if (haveClerk)
            {
                jm.msg = "用户已经是当前门店店员";
                return Json(jm);
            }

            var model = new CoreCmsClerk
            {
                storeId = entity.storeId,
                userId = user.id,
                isDel = false,
                createTime = DateTime.Now
            };

            var bl = await _coreCmsClerkServices.InsertAsync(model) > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;


            return Json(jm);
        }

        #endregion

        #region 编辑店员数据============================================================

        // POST: Api/CoreCmsStore/GetClerkEdit
        /// <summary>
        ///     编辑店员数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑店员数据")]
        public async Task<JsonResult> GetClerkEdit([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsClerkServices.QueryByClauseAsync(p => p.id == entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return Json(jm);
            }

            jm.code = 0;
            jm.data = model;

            return Json(jm);
        }

        #endregion

        #region 删除店员数据============================================================

        // POST: Api/CoreCmsStore/DoClerkDelete/10
        /// <summary>
        ///     单选删除店员
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除店员")]
        public async Task<JsonResult> DoClerkDelete([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsClerkServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return Json(jm);
            }

            var bl = await _coreCmsClerkServices.DeleteByIdAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return Json(jm);

        }

        #endregion

        #region 批量删除店员============================================================

        // POST: Api/CoreCmsStore/DoClerkBatchDelete/10,11,20
        /// <summary>
        ///     批量删除店员
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量删除店员")]
        public async Task<JsonResult> DoClerkBatchDelete([FromBody] FMArrayIntIds entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await _coreCmsClerkServices.DeleteByIdsAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return Json(jm);
        }

        #endregion
    }
}
