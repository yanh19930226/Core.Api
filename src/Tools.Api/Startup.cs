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
                       //ʹ��redis
               option.UseRedis(config =>
                 {
                                    config.DBConfig.Endpoints.Add(new ServerEndPoint("116.62.214.239", 6379));
                               }, "localhostRedis").WithJson();
            });


            #region InMemory
            // ���EasyCaching
            //services.AddEasyCaching(option =>
            //{
            //    // ʹ��InMemory��򵥵�����
            //    option.UseInMemory("default");

            //    //// ʹ��InMemory�Զ��������
            //    //option.UseInMemory(options => 
            //    //{
            //    //     // DBConfig�����ÿ��Provider����������
            //    //     options.DBConfig = new InMemoryCachingOptions
            //    //     {
            //    //         // InMemory�Ĺ���ɨ��Ƶ�ʣ�Ĭ��ֵ��60��
            //    //         ExpirationScanFrequency = 60, 
            //    //         // InMemory����󻺴�����, Ĭ��ֵ��10000
            //    //         SizeLimit = 100 
            //    //     };
            //    //     // Ԥ��������ͬһʱ��ȫ��ʧЧ������Ϊÿ��key�Ĺ���ʱ�����һ�������������Ĭ��ֵ��120��
            //    //     options.MaxRdSecond = 120;
            //    //     // �Ƿ�����־��Ĭ��ֵ��false
            //    //     options.EnableLogging = false;
            //    //     // �������Ĵ��ʱ��, Ĭ��ֵ��5000����
            //    //     options.LockMs = 5000;
            //    //     // û�л�ȡ��������ʱ������ʱ�䣬Ĭ��ֵ��300����
            //    //     options.SleepMs = 300;
            //    // }, "m2");         

            //    //// ��ȡ�����ļ�
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
