﻿using Core.Net.Configuration;
using Core.Net.Util.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.Auth
{
    /// <summary>
    /// Db 启动服务
    /// </summary>
    public static class AuthorizationSetup
    {
        /// <summary>
        /// 后台管理员jwt初始化设置
        /// </summary>
        /// <param name="services"></param>
        public static void AddAuthorizationSetupForAdmin(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));


            //读取配置文件
            var symmetricKeyAsBase64 = AppSettingsConstVars.JwtConfigSecretKey;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var issuer = AppSettingsConstVars.JwtConfigIssuer;
            var audience = AppSettingsConstVars.JwtConfigAudience;

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);


            // 如果要数据库动态绑定，这里先留个空，后边处理器里动态赋值
            var permission = new List<PermissionItem>();

            // 角色与接口的权限要求参数
            var permissionRequirement = new PermissionRequirement(
                "/api/denied",// 拒绝授权的跳转地址（目前无用）
                permission,
                ClaimTypes.Role,//基于角色的授权
                issuer,//发行人
                audience,//听众
                signingCredentials,//签名凭据
                expiration: TimeSpan.FromSeconds(60 * 60 * 1)//接口的过期时间
                );

            // 复杂的策略授权
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Permissions.Name,
                         policy => policy.Requirements.Add(permissionRequirement));
            });

            // 令牌验证参数
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,   //是否验证SecurityKey
                IssuerSigningKey = signingKey,  //拿到SecurityKey
                ValidateIssuer = true, //是否验证Issuer
                ValidIssuer = issuer,//发行人 //Issuer，这两项和前面签发jwt的设置一致
                ValidateAudience = true, //是否验证Audience
                ValidAudience = audience,//订阅人
                ValidateLifetime = true,//是否验证失效时间
                ClockSkew = TimeSpan.FromSeconds(60),
                RequireExpirationTime = true,
            };

            // core自带官方JWT认证，开启Bearer认证
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = nameof(ApiResponseForAdminHandler);
                o.DefaultForbidScheme = nameof(ApiResponseForAdminHandler);
            })
             // 添加JwtBearer服务
             .AddJwtBearer(o =>
             {
                 o.TokenValidationParameters = tokenValidationParameters;
                 o.Events = new JwtBearerEvents
                 {
                     OnChallenge = context =>
                     {
                         context.Response.Headers.Add("Token-Error", context.ErrorDescription);
                         return Task.CompletedTask;
                     },
                     OnAuthenticationFailed = context =>
                     {
                         var token = context.Request.Headers["Authorization"].ObjectToString().Replace("Bearer ", "");
                         var jwtToken = (new JwtSecurityTokenHandler()).ReadJwtToken(token);

                         if (jwtToken.Issuer != issuer)
                         {
                             context.Response.Headers.Add("Token-Error-Iss", "issuer is wrong!");
                         }

                         if (jwtToken.Audiences.FirstOrDefault() != audience)
                         {
                             context.Response.Headers.Add("Token-Error-Aud", "Audience is wrong!");
                         }

                         // 如果过期，则把<是否过期>添加到，返回头信息中
                         if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                         {
                             context.Response.Headers.Add("Token-Expired", "true");
                         }
                         return Task.CompletedTask;
                     }
                 };
             })
             .AddScheme<AuthenticationSchemeOptions, ApiResponseForAdminHandler>(nameof(ApiResponseForAdminHandler), o => { });

            services.AddScoped<IAuthorizationHandler, PermissionForAdminHandler>();
            services.AddSingleton(permissionRequirement);
        }
    }
}
