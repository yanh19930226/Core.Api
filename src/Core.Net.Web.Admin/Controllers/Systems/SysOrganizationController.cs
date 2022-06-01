﻿using Core.Net.Configuration;
using Core.Net.Entity.Dtos;
using Core.Net.Entity.Model.Expression;
using Core.Net.Entity.Model.Systems;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.Systems;
using Core.Net.Util.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Net.Web.Admin.Controllers.Systems
{
    /// <summary>
    /// 组织机构表
    ///</summary>
    [Description("组织机构表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SysOrganizationController : Controller
    {
        private readonly ISysOrganizationServices _sysOrganizationServices;
        private readonly ISysDictionaryServices _sysDictionaryServices;
        private readonly ISysDictionaryDataServices _sysDictionaryDataServices;
        /// <summary>
        ///  构造函数
        /// </summary>
        public SysOrganizationController(ISysOrganizationServices sysOrganizationServices, ISysDictionaryServices sysDictionaryServices, ISysDictionaryDataServices sysDictionaryDataServices)
        {
            _sysOrganizationServices = sysOrganizationServices;
            _sysDictionaryServices = sysDictionaryServices;
            _sysDictionaryDataServices = sysDictionaryDataServices;
        }
        #region 首页数据============================================================

        // POST: Api/SysRole/GetIndex
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

        // POST: Api/SysOrganization/GetPageList
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<JsonResult> GetPageList()
        {
            var jm = new AdminUiCallBack();

            //获取数据
            var list = await _sysOrganizationServices.QueryAsync();
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.Count;
            jm.msg = "数据调用成功!";
            return Json(jm);
        }

        #endregion

        #region 创建数据============================================================
        // POST: Api/SysOrganization/GetCreate
        /// <summary>
        /// 创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public async Task<JsonResult> GetCreate()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };

            var dict = await _sysDictionaryServices.QueryByClauseAsync(p => p.dictCode == "organization_type");
            var dictData = new List<SysDictionaryData>();
            if (dict != null)
            {
                dictData = await _sysDictionaryDataServices.QueryListByClauseAsync(p => p.dictId == dict.id);
            }
            jm.data = new
            {
                dictData
            };

            return new JsonResult(jm);
        }
        #endregion

        #region 创建提交============================================================
        // POST: Api/SysOrganization/DoCreate
        /// <summary>
        /// 创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<JsonResult> DoCreate([FromBody] SysOrganization entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await _sysOrganizationServices.InsertAsync(entity) > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = (bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure);

            return new JsonResult(jm);
        }
        #endregion

        #region 编辑数据============================================================
        // POST: Api/SysOrganization/GetEdit
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

            var model = await _sysOrganizationServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }

            var dict = await _sysDictionaryServices.QueryByClauseAsync(p => p.dictCode == "organization_type");
            var dictData = new List<SysDictionaryData>();
            if (dict != null)
            {
                dictData = await _sysDictionaryDataServices.QueryListByClauseAsync(p => p.dictId == dict.id);
            }
            jm.code = 0;
            jm.data = new
            {
                model,
                dictData
            };

            return new JsonResult(jm);
        }
        #endregion

        #region 编辑提交============================================================
        // POST: Api/SysOrganization/Edit
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<JsonResult> DoEdit([FromBody] SysOrganization entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _sysOrganizationServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            //事物处理过程开始
            //oldModel.id = entity.id;
            oldModel.parentId = entity.parentId;
            oldModel.organizationName = entity.organizationName;
            oldModel.organizationFullName = entity.organizationFullName;
            oldModel.organizationType = entity.organizationType;
            //oldModel.leaderId = entity.leaderId;
            oldModel.sortNumber = entity.sortNumber;
            oldModel.comments = entity.comments;
            oldModel.deleted = entity.deleted;
            //oldModel.createTime = entity.createTime;
            oldModel.updateTime = DateTime.Now;

            //事物处理过程结束
            var bl = await _sysOrganizationServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return new JsonResult(jm);
        }
        #endregion

        #region 删除数据============================================================
        // POST: Api/SysOrganization/DoDelete/10
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

            var model = await _sysOrganizationServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return new JsonResult(jm);
            }
            var bl = await _sysOrganizationServices.DeleteByIdAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return new JsonResult(jm);

        }
        #endregion

        #region 设置leader============================================================
        // POST: Api/SysOrganization/DoSetSysOrganizationLeader/10
        /// <summary>
        /// 设置leader
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置leader")]
        public async Task<JsonResult> DoSetSysOrganizationLeader([FromBody] FMDoSetSysOrganizationLeaderPost entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _sysOrganizationServices.QueryByIdAsync(entity.organizationId);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            oldModel.leaderId = entity.leaderId;
            var bl = await _sysOrganizationServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.SetDataSuccess : GlobalConstVars.SetDataFailure;

            return new JsonResult(jm);
        }
        #endregion
    }
}
