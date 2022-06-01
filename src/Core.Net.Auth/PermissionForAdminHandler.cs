﻿using Core.Net.Service.Systems;
using Core.Net.Util.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Net.Auth
{
    /// <summary>
    /// 权限授权处理器
    /// </summary>
    public class PermissionForAdminHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// 验证方案提供对象
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }
        private readonly ISysRoleMenuServices _sysRoleMenuServices;
        private readonly IHttpContextAccessor _accessor;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="schemes"></param>
        /// <param name="navigationRepository"></param>
        /// <param name="accessor"></param>
        public PermissionForAdminHandler(IAuthenticationSchemeProvider schemes
            , ISysRoleMenuServices sysRoleMenuServices
            , IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            Schemes = schemes;
            _sysRoleMenuServices = sysRoleMenuServices;
        }

        // 重写异步处理程序
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {

            var httpContext = _accessor.HttpContext;

            if (!requirement.Permissions.Any())
            {
                var data = await _sysRoleMenuServices.RoleModuleMaps();
                var list = new List<PermissionItem>();

              
                    list = (from item in data
                            orderby item.id
                            select new PermissionItem
                            {
                                Url = item.menu?.component,
                                RouteUrl = item.menu?.path,
                                Authority = item.menu?.authority,
                                Role = item.role?.roleCode,
                            }).ToList();
              
                requirement.Permissions = list;
            }

            //请求Url
            if (httpContext != null)
            {
                //
                var questUrl = httpContext.Request.Path.Value.ToLower();
                //判断请求是否停止
                var handlers = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
                foreach (var scheme in await Schemes.GetRequestHandlerSchemesAsync())
                {
                    if (await handlers.GetHandlerAsync(httpContext, scheme.Name) is IAuthenticationRequestHandler handler && await handler.HandleRequestAsync())
                    {
                        context.Fail();
                        return;
                    }
                }
                //判断请求是否拥有凭据，即有没有登录
                var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
                if (defaultAuthenticate != null)
                {
                    var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                    //result?.Principal不为空即登录成功
                    if (result?.Principal != null)
                    {
                        httpContext.User = result.Principal;

                        // 获取当前用户的角色信息
                        var currentUserRoles = new List<string>();

                       
                            // jwt
                            currentUserRoles = (from item in httpContext.User.Claims
                                                where item.Type == requirement.ClaimType
                                                select item.Value).ToList();

                        var isMatchRole = false;

                        var permisssionRoles = requirement.Permissions.Where(w => currentUserRoles.Contains(w.Role)).ToList();

                        foreach (var item in permisssionRoles)
                        {
                            try
                            {
                                //权限中是否存在请求的url
                                if (Regex.Match(questUrl, item.Url.ObjectToString().ToLower()).Value == questUrl)
                                {
                                    isMatchRole = true;
                                    break;
                                }
                            }
                            catch (Exception)
                            {
                                // ignored
                            }
                        }

                        //验证权限
                        if (currentUserRoles.Count <= 0 || !isMatchRole)
                        {
                            context.Fail();
                            return;
                        }
                        var isExp = false;

                      
                            // jwt
                            isExp = (httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) != null && DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) >= DateTime.Now;

                        if (isExp)
                        {
                            context.Succeed(requirement);
                        }
                        else
                        {
                            context.Fail();
                            return;
                        }
                        return;
                    }
                }
                //判断没有登录时，是否访问登录的url,并且是Post请求，并且是form表单提交类型，否则为失败
                if (!questUrl.Equals(requirement.LoginPath.ToLower(), StringComparison.Ordinal) && (!httpContext.Request.Method.Equals("POST") || !httpContext.Request.HasFormContentType))
                {
                    context.Fail();
                    return;
                }
            }

            context.Succeed(requirement);
        }
    }
}
