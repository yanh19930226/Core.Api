using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Core.Net.AutoFac;
using Core.Net.Config;
using Core.Net.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }
        /// <summary>
        /// web����
        /// </summary>
        public IWebHostEnvironment Env { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //��ӱ���·����ȡ֧��
            services.AddSingleton(new AppSettingsHelper(Env.ContentRootPath));
          
            //�ڴ滺��
            services.AddMemoryCacheSetup();
            //����Redis
            services.AddRedisSetup();
            //Redis��Ϣ����
            services.AddRedisMessageQueueSetup();
            //���ÿ���CORS��
            services.AddCorsSetup();
            //Swagger�ӿ��ĵ�ע��
            services.AddClientSwaggerSetup();
          
        }
        /// <summary>
        /// Autofac
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {

            builder.RegisterModule(new AutofacModuleRegister());

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCoreSwagger();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
