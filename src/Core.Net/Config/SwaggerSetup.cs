using Core.Net.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Core.Net.Config
{
    public static class SwaggerSetup
    {
        public static IServiceCollection AddAdminSwaggerSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (!AppSettingsConstVars.SwaggerEnabled)
            {
                return services;
            }
            var apiName = AppSettingsConstVars.ApiName;
            var version = AppSettingsConstVars.ApiVersion;
            services.AddSwaggerGen((s) =>
            {
                s.SwaggerDoc(version, new OpenApiInfo
                {
                    Version = version,
                    Title = $"{apiName} 接口文档",
                    Description = $"{apiName} HTTP API " + version,
                    Contact = new OpenApiContact { Name = apiName, Email = "yanh@163.com", Url = new Uri("http://yande.buzz") },
                });
                s.OrderActionsBy(o => o.RelativePath);

                try
                {
                    //生成API XML文档
                    var basePath = AppContext.BaseDirectory;
                    var xmlPath = Path.Combine(basePath, "doc.xml");
                    s.IncludeXmlComments(xmlPath);
                }
                catch (Exception ex)
                {
                    //NLogUtil.WriteFileLog(NLog.LogLevel.Error, LogType.Swagger, "Swagger", "Swagger生成失败，Doc.xml丢失，请检查并拷贝。", ex);
                }

                //// 开启加权小锁
                //s.OperationFilter<AddResponseHeadersFilter>();
                //s.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                //// 在header中添加token，传递到后台
                //s.OperationFilter<SecurityRequirementsOperationFilter>();

                //// 必须是 oauth2
                //s.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                //{
                //    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                //    Name = "Authorization",//jwt默认的参数名称
                //    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                //    Type = SecuritySchemeType.ApiKey
                //});

            });
            return services;
        }
        public static IServiceCollection AddClientSwaggerSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (!AppSettingsConstVars.SwaggerEnabled)
            {
                return services;
            }

            var apiName = AppSettingsConstVars.ApiName;
            var version = AppSettingsConstVars.ApiVersion;
            services.AddSwaggerGen((s) =>
            {
                s.SwaggerDoc(apiName, new OpenApiInfo
                {
                    Version = version,
                    Title = $"{apiName} 接口文档",
                    Description = $"{apiName} HTTP API " + version,
                    Contact = new OpenApiContact { Name = apiName, Email = "yanh@163.com", Url = new Uri("http://yande.buzz") },
                });
                s.OrderActionsBy(o => o.RelativePath);
                try
                {
                    //生成API XML文档
                    var basePath = AppContext.BaseDirectory;
                    var xmlPath = Path.Combine(basePath, "doc.xml");
                    s.IncludeXmlComments(xmlPath);
                }
                catch (Exception ex)
                {
                    //NLogUtil.WriteFileLog(NLog.LogLevel.Error, LogType.Swagger, "Swagger", "Swagger生成失败，Doc.xml丢失，请检查并拷贝。", ex);
                }
               
                //// 开启加权小锁
                //s.OperationFilter<AddResponseHeadersFilter>();
                //s.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                //// 在header中添加token，传递到后台
                //s.OperationFilter<SecurityRequirementsOperationFilter>();

                //// 必须是 oauth2
                //s.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                //{
                //    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                //    Name = "Authorization",//jwt默认的参数名称
                //    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                //    Type = SecuritySchemeType.ApiKey
                //});
            });
            return services;
        }

        public static IApplicationBuilder UseCoreSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "{documentName}/swagger.json";
            }).UseSwaggerUI(options =>
            {
                var apiName = AppSettingsConstVars.ApiName;
                var version = AppSettingsConstVars.ApiVersion;
                options.SwaggerEndpoint($"/{apiName}/swagger.json", $"{apiName} V{version}");

                options.DefaultModelsExpandDepth(-1); //设置为 - 1 可不显示models
                options.DocExpansion(DocExpansion.List);//设置为none可折叠所有方法
            });

            return app;
        }
    }
}
