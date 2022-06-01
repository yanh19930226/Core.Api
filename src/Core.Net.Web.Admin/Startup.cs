using Autofac;
using Core.Net.Auth;
using Core.Net.AutoFac;
using Core.Net.Config;
using Core.Net.Configuration;
using Core.Net.Filter;
using Core.Net.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Net.Web.Admin
{
    public class Startup
    {
        /// <summary>
        ///     ���캯��
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }
        /// <summary>
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// </summary>
        public IWebHostEnvironment Env { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            //��ӱ���·����ȡ֧��
            services.AddSingleton(new AppSettingsHelper(Env.ContentRootPath));
            //�ڴ滺��
            services.AddMemoryCacheSetup();
            //����Redis
            services.AddRedisSetup();
            //Redis��Ϣ����
            services.AddRedisMessageQueueSetup();
            //jwt��Ȩ֧��ע��
            services.AddAuthorizationSetupForAdmin();
            //ע��mvc��ע��razor������ͼ
            services.AddMvc(options =>
            {
                //ʵ����֤
                options.Filters.Add<RequiredErrorForAdmin>();
                ////�쳣����
                options.Filters.Add<GlobalExceptionsFilterForAdmin>();
                ////Swagger�޳�����Ҫ����apiչʾ���б�
                //options.Conventions.Add(new ApiExplorerIgnores());
                //options.EnableEndpointRouting = false;
            }).AddNewtonsoftJson(p =>
                {
                    //���ݸ�ʽ����ĸСд ��ʹ���շ�
                    //p.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    //��ʹ���շ���ʽ��key
                    //p.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    //����ѭ������
                    //p.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    //����ʱ���ʽ
                    p.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                });
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
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            // ��¼�����뷵������ (ע�⿪��Ȩ�ޣ���Ȼ�����޷�д��)
            app.UseReuestResponseLog();
            // ��¼ip���� (ע�⿪��Ȩ�ޣ���Ȼ�����޷�д��)
            app.UseIpLogMildd();
            // �û����ʼ�¼(����ŵ���㣬��Ȼ��������쳣���ᱨ����Ϊ���ܷ�����)(ע�⿪��Ȩ�ޣ���Ȼ�����޷�д��)
            //app.UseRecordAccessLogsMildd();
            // signalr
            //app.UseSignalRSendMildd();

            app.UseStaticFiles();
            app.UseRouting();
            // �ȿ�����֤
            app.UseAuthentication();
            // Ȼ������Ȩ�м��
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "areas",
                    "{area:exists}/{controller=Default}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });

            //����Ĭ����ʼҳ index.html
            //�˴���·���������wwwroot�ļ��е����·��
            var defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(defaultFilesOptions);
            app.UseStaticFiles();
          
        }
    }
}
