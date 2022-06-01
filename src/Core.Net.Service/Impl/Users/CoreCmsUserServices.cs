﻿using Core.Net.Configuration;
using Core.Net.Data;
using Core.Net.Data.Impl;
using Core.Net.Entity.Dtos;
using Core.Net.Entity.Model.Shops;
using Core.Net.Entity.Model.Users;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.Users;
using Core.Net.Util.Helper;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.Service.Impl.Users
{
    public class CoreCmsUserServices : BaseServices<CoreCmsUser>, ICoreCmsUserServices
    {
        private readonly IBaseRepository<CoreCmsUser> _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlSugarClient _sqlSugarClient;
        private readonly ICoreCmsUserBalanceServices _userBalanceServices;
        private readonly ICoreCmsUserPointLogServices _userPointLogServices;
        public CoreCmsUserServices(IBaseRepository<CoreCmsUser> dal, IUnitOfWork unitOfWork, SqlSugarClient sqlSugarClient, ICoreCmsUserBalanceServices userBalanceServices, ICoreCmsUserPointLogServices userPointLogServices)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _sqlSugarClient = sqlSugarClient;
            _userBalanceServices = userBalanceServices;
            _userPointLogServices = userPointLogServices;

        }

        public async Task<AdminUiCallBack> UpdateBalance(int id, decimal money)
        {
            var jm = new AdminUiCallBack();

            var model = await _dal.QueryByIdAsync(id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            var newMoney = model.balance + money;
            var up = await _dal.UpdateAsync(p => new CoreCmsUser() { balance = newMoney }, p => p.id == id);
            if (up)
            {
                var balance = new CoreCmsUserBalance();
                balance.type = (int)GlobalEnumVars.UserBalanceSourceTypes.Admin;
                balance.userId = model.id;
                balance.balance = model.balance;
                balance.createTime = DateTime.Now;
                balance.memo = UserHelper.GetMemo(balance.type, money);
                balance.money = money;
                balance.sourceId = GlobalEnumVars.UserBalanceSourceTypes.Admin.ToString();

                jm.code = await _userBalanceServices.InsertAsync(balance) > 0 ? 0 : 1;
            }

            jm.msg = jm.code == 0 ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        public async Task<AdminUiCallBack> UpdatePiont(FMUpdateUserPoint entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _dal.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            var newPoint = model.point + entity.point;
            var up = await _dal.UpdateAsync(p => new CoreCmsUser() { point = newPoint }, p => p.id == entity.id);
            if (up)
            {
                var point = new CoreCmsUserPointLog();
                point.userId = model.id;
                point.type = (int)GlobalEnumVars.UserPointSourceTypes.PointTypeAdminEdit;
                point.num = entity.point;
                point.balance = newPoint;
                point.remarks = entity.memo;
                point.createTime = DateTime.Now;

                jm.code = await _userPointLogServices.InsertAsync(point) > 0 ? 0 : 1;
            }

            jm.msg = jm.code == 0 ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;


            return jm;
        }
    }
}
