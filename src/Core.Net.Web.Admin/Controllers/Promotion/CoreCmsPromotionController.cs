using Core.Net.Configuration;
using Core.Net.Entity.Dtos;
using Core.Net.Entity.Model.Expression;
using Core.Net.Entity.Model.Promotion;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.Promotion;
using Core.Net.Util.Extensions;
using Core.Net.Util.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Net.Web.Admin.Controllers.Promotion
{
    /// <summary>
    /// 促销表
    ///</summary>
    [Description("促销表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CoreCmsPromotionController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsPromotionServices _coreCmsPromotionServices;
        private readonly ICoreCmsPromotionConditionServices _promotionConditionServices;
        private readonly ICoreCmsPromotionResultServices _promotionResultServices;
        private readonly ICoreCmsCouponServices _coreCmsCouponServices;



        ///  <summary>
        ///  构造函数
        /// </summary>
        public CoreCmsPromotionController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsPromotionServices coreCmsPromotionServices
            , ICoreCmsPromotionConditionServices promotionConditionServices
            , ICoreCmsPromotionResultServices promotionResultServices
            , ICoreCmsCouponServices coreCmsCouponServices
            )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsPromotionServices = coreCmsPromotionServices;
            _promotionConditionServices = promotionConditionServices;
            _promotionResultServices = promotionResultServices;
            _coreCmsCouponServices = coreCmsCouponServices;
        }

        #region 获取列表============================================================
        // POST: Api/CoreCmsPromotion/GetPageList
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<JsonResult> GetPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsPromotion>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsPromotion, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "name":
                    orderEx = p => p.name;
                    break;
                case "type":
                    orderEx = p => p.type;
                    break;
                case "sort":
                    orderEx = p => p.sort;
                    break;
                case "parameters":
                    orderEx = p => p.parameters;
                    break;
                case "startTime":
                    orderEx = p => p.startTime;
                    break;
                case "endTime":
                    orderEx = p => p.endTime;
                    break;
                case "isExclusive":
                    orderEx = p => p.isExclusive;
                    break;
                case "isAutoReceive":
                    orderEx = p => p.isAutoReceive;
                    break;
                case "isEnable":
                    orderEx = p => p.isEnable;
                    break;
                case "isDel":
                    orderEx = p => p.isDel;
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
            var types = Request.Form["types"].FirstOrDefault().ObjectToInt(0);
            if (types == 1)
            {
                where = where.And(p => p.type == (int)GlobalEnumVars.PromotionType.Promotion);
            }
            else if (types == 2)
            {
                where = where.And(p => p.type == (int)GlobalEnumVars.PromotionType.Coupon);
            }
            else if (types == 3)
            {
                where = where.And(p => p.type == (int)GlobalEnumVars.PromotionType.Group || p.type == (int)GlobalEnumVars.PromotionType.Seckill);
            }
            //促销名称 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name))
            {
                where = where.And(p => p.name.Contains(name));
            }
            //排序 int
            var sort = Request.Form["sort"].FirstOrDefault().ObjectToInt(0);
            if (sort > 0)
            {
                where = where.And(p => p.sort == sort);
            }
            //其它参数 nvarchar
            var parameters = Request.Form["parameters"].FirstOrDefault();
            if (!string.IsNullOrEmpty(parameters))
            {
                where = where.And(p => p.parameters.Contains(parameters));
            }
            //开始时间 datetime
            var startTime = Request.Form["startTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(startTime))
            {
                if (startTime.Contains("到"))
                {
                    var dts = startTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.startTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.startTime < dtEnd);
                }
                else
                {
                    var dt = startTime.ObjectToDate();
                    where = where.And(p => p.startTime > dt);
                }
            }
            //结束时间 datetime
            var endTime = Request.Form["endTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(endTime))
            {
                if (endTime.Contains("到"))
                {
                    var dts = endTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.endTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.endTime < dtEnd);
                }
                else
                {
                    var dt = endTime.ObjectToDate();
                    where = where.And(p => p.endTime > dt);
                }
            }
            //是否排他 bit
            var isExclusive = Request.Form["isExclusive"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isExclusive) && isExclusive.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.isExclusive == true);
            }
            else if (!string.IsNullOrEmpty(isExclusive) && isExclusive.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.isExclusive == false);
            }

            //是否开启 bit
            var isEnable = Request.Form["isEnable"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isEnable) && isEnable.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.isEnable == true);
            }
            else if (!string.IsNullOrEmpty(isEnable) && isEnable.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.isEnable == false);
            }
            where = where.And(p => p.isDel == false);
            //获取数据
            var list = await _coreCmsPromotionServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);

            if (list.Any() && types == 3)
            {

            }


            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return new JsonResult(jm);
        }
        #endregion

        #region 首页数据============================================================
        // POST: Api/CoreCmsPromotion/GetIndex
        /// <summary>
        /// 首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public JsonResult GetIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            var promotionType = EnumHelper.EnumToList<GlobalEnumVars.PromotionType>();
            jm.data = new
            {
                promotionType
            };
            return new JsonResult(jm);
        }
        #endregion

        #region 创建数据============================================================
        // POST: Api/CoreCmsPromotion/GetCreate
        /// <summary>
        /// 创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public JsonResult GetCreate()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            return new JsonResult(jm);
        }
        #endregion

        #region 创建提交============================================================
        // POST: Api/CoreCmsPromotion/DoCreate
        /// <summary>
        /// 创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<JsonResult> DoCreate([FromBody] CoreCmsPromotion entity)
        {
            var jm = new AdminUiCallBack();

            if (entity.startTime >= entity.endTime)
            {
                jm.msg = "开始时间必须小于结束时间";
                return new JsonResult(jm);
            }

            var bl = await _coreCmsPromotionServices.InsertAsync(entity) > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = (bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure);

            return new JsonResult(jm);
        }
        #endregion

        #region 编辑数据============================================================
        // POST: Api/CoreCmsPromotion/GetEdit
        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑数据")]
        public async Task<JsonResult> GetEdit([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsPromotionServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            jm.code = 0;

            var conditions = await _promotionConditionServices.QueryListByClauseAsync(p => p.promotionId == model.id);
            var results = await _promotionResultServices.QueryListByClauseAsync(p => p.promotionId == model.id);

            //获取促销添加参数类型字典
            var promotionConditionTypes = SystemSettingDictionary.GetPromotionConditionType();

            //获取促销添加结果类型字典
            var promotionResultTypes = SystemSettingDictionary.GetPromotionResultType();

            jm.data = new
            {
                model,
                conditions,
                results,
                promotionConditionTypes,
                promotionResultTypes
            };

            return new JsonResult(jm);
        }
        #endregion

        #region 编辑提交============================================================
        // POST: Admins/CoreCmsPromotion/Edit
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<JsonResult> DoEdit([FromBody] CoreCmsPromotion entity)
        {
            var jm = new AdminUiCallBack();

            if (entity.startTime >= entity.endTime)
            {
                jm.msg = "开始时间必须小于结束时间";
                return new JsonResult(jm);
            }

            var oldModel = await _coreCmsPromotionServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            //事物处理过程开始
            oldModel.name = entity.name;
            oldModel.type = entity.type;
            oldModel.sort = entity.sort;
            oldModel.parameters = entity.parameters;
            oldModel.startTime = entity.startTime;
            oldModel.endTime = entity.endTime;

            oldModel.maxNums = entity.maxNums > 0 ? entity.maxNums : 0;
            oldModel.maxGoodsNums = entity.maxGoodsNums > 0 ? entity.maxGoodsNums : 0;
            oldModel.maxRecevieNums = entity.maxRecevieNums > 0 ? entity.maxRecevieNums : 0;

            oldModel.effectiveDays = entity.effectiveDays;
            oldModel.effectiveHours = entity.effectiveHours;

            if (entity.type == (int)GlobalEnumVars.PromotionType.Promotion)
            {
                oldModel.isExclusive = entity.isExclusive;
            }

            if (entity.type == (int)GlobalEnumVars.PromotionType.Coupon)
            {
                oldModel.isAutoReceive = entity.isAutoReceive;

                if (oldModel.effectiveDays == 0 && oldModel.effectiveHours == 0)
                {
                    jm.msg = "优惠券有效时间不能为0";
                    return new JsonResult(jm);
                }
            }

            oldModel.isEnable = entity.isEnable;

            if (oldModel.type == (int)GlobalEnumVars.PromotionType.Group || oldModel.type == (int)GlobalEnumVars.PromotionType.Seckill)
            {
                await _promotionConditionServices.DeleteAsync(p => p.promotionId == oldModel.id && p.code == "GOODS_IDS");
                var coreCmsPromotionResult = new CoreCmsPromotionCondition();
                coreCmsPromotionResult.promotionId = oldModel.id;
                coreCmsPromotionResult.code = "GOODS_IDS";
                coreCmsPromotionResult.parameters = entity.parameters;
                await _promotionConditionServices.InsertAsync(coreCmsPromotionResult);
            }

            //事物处理过程结束
            var bl = await _coreCmsPromotionServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return new JsonResult(jm);
        }
        #endregion

        #region 删除数据============================================================
        // POST: Api/CoreCmsPromotion/DoDelete/10
        /// <summary>
        /// 单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<JsonResult> DoDelete([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsPromotionServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return new JsonResult(jm);
            }
            model.isDel = true;
            var bl = await _coreCmsPromotionServices.UpdateAsync(model);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return new JsonResult(jm);

        }
        #endregion


        #region 设置是否排他============================================================
        // POST: Api/CoreCmsPromotion/DoSetisExclusive/10
        /// <summary>
        /// 设置是否排他
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否排他")]
        public async Task<JsonResult> DoSetisExclusive([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsPromotionServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            oldModel.isExclusive = (bool)entity.data;

            var bl = await _coreCmsPromotionServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return new JsonResult(jm);
        }
        #endregion

        #region 设置是否开启============================================================
        // POST: Api/CoreCmsPromotion/DoSetisEnable/10
        /// <summary>
        /// 设置是否开启
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否开启")]
        public async Task<JsonResult> DoSetisEnable([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsPromotionServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            oldModel.isEnable = (bool)entity.data;

            var bl = await _coreCmsPromotionServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return new JsonResult(jm);
        }
        #endregion

        //促销条件===============================================================================



        //促销结果===============================================================================



        //优惠券码===============================================================================
        #region 获取优惠券码列表============================================================
        // POST: Api/CoreCmsPromotion/GetCouponPageList
        /// <summary>
        /// 获取优惠券码列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取优惠券码列表")]
        public async Task<JsonResult> GetCouponPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsCoupon>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsCoupon, object>> orderEx;
            switch (orderField)
            {
                case "couponCode":
                    orderEx = p => p.couponCode;
                    break;
                case "promotionId":
                    orderEx = p => p.promotionId;
                    break;
                case "isUsed":
                    orderEx = p => p.isUsed;
                    break;
                case "userId":
                    orderEx = p => p.userId;
                    break;
                case "usedId":
                    orderEx = p => p.usedId;
                    break;
                case "createTime":
                    orderEx = p => p.createTime;
                    break;
                case "updateTime":
                    orderEx = p => p.updateTime;
                    break;
                default:
                    orderEx = p => p.createTime;
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
            //优惠券编码 nvarchar
            var couponCode = Request.Form["couponCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(couponCode))
            {
                where = where.And(p => p.couponCode.Contains(couponCode));
            }
            //优惠券序列 int
            var promotionId = Request.Form["promotionId"].FirstOrDefault().ObjectToInt(0);
            if (promotionId > 0)
            {
                where = where.And(p => p.promotionId == promotionId);
            }
            //是否使用 bit
            var isUsed = Request.Form["isUsed"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isUsed) && isUsed.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.isUsed == true);
            }
            else if (!string.IsNullOrEmpty(isUsed) && isUsed.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.isUsed == false);
            }
            //领取者 int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0)
            {
                where = where.And(p => p.userId == userId);
            }
            //使用者 int
            var usedId = Request.Form["usedId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(usedId))
            {
                where = where.And(p => p.usedId.Contains(usedId));
            }
            //生成时间 datetime
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
            var list = await _coreCmsCouponServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return new JsonResult(jm);
        }
        #endregion

        #region 优惠券码首页数据============================================================
        // POST: Api/CoreCmsPromotion/GetCouponIndex
        /// <summary>
        /// 优惠券码首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("优惠券码首页数据")]
        public JsonResult GetCouponIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            return new JsonResult(jm);
        }
        #endregion

    }
}
