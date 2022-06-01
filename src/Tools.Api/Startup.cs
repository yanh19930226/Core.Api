using AutoMapper;
using Core;
using Core.Logger;
using Core.Swagger;
using EasyCaching.Core.Configurations;
using EasyCaching.Interceptor.AspectCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools.Api.Models;
using Tools.Api.Services;

namespace Tools.Api
{
    public class Startup : CommonStartup
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void CommonServices(IServiceCollection services)
        {
            // AutoMapper Settings
            services.AddAutoMapper(typeof(MappingConfigs));
            services.AddSingleton<IBookService, BookService>();
            // MongoDB Settings
            services.Configure<BookStoreDatabaseSettings>(
                Configuration.GetSection(nameof(BookStoreDatabaseSettings)));
            services.AddSingleton<IBookStoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<BookStoreDatabaseSettings>>().Value);

            services.AddCoreSeriLog()
                             .AddCoreSwagger();

            services.AddSingleton<IUserService, UserService>();

            services.AddEasyCaching(option =>
            {
                       //使用redis
               option.UseRedis(config =>
                 {
                                    config.DBConfig.Endpoints.Add(new ServerEndPoint("116.62.214.239", 6379));
                               }, "localhostRedis").WithJson();
            });


            #region InMemory
            // 添加EasyCaching
            //services.AddEasyCaching(option =>
            //{
            //    // 使用InMemory最简单的配置
            //    option.UseInMemory("default");

            //    //// 使用InMemory自定义的配置
            //    //option.UseInMemory(options => 
            //    //{
            //    //     // DBConfig这个是每种Provider的特有配置
            //    //     options.DBConfig = new InMemoryCachingOptions
            //    //     {
            //    //         // InMemory的过期扫描频率，默认值是60秒
            //    //         ExpirationScanFrequency = 60, 
            //    //         // InMemory的最大缓存数量, 默认值是10000
            //    //         SizeLimit = 100 
            //    //     };
            //    //     // 预防缓存在同一时间全部失效，可以为每个key的过期时间添加一个随机的秒数，默认值是120秒
            //    //     options.MaxRdSecond = 120;
            //    //     // 是否开启日志，默认值是false
            //    //     options.EnableLogging = false;
            //    //     // 互斥锁的存活时间, 默认值是5000毫秒
            //    //     options.LockMs = 5000;
            //    //     // 没有获取到互斥锁时的休眠时间，默认值是300毫秒
            //    //     options.SleepMs = 300;
            //    // }, "m2");         

            //    //// 读取配置文件
            //    //option.UseInMemory(Configuration, "m3");
            //}); 
            #endregion

        }

        public override void CommonConfigure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCoreSwagger();
        }
    }
}
