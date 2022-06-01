using Autofac.Extensions.DependencyInjection;
using Core.Net.Loging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Net.Web.Admin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            try
            {
                //ȷ��NLog.config�������ַ�����appsettings.json��ͬ��
                NLogUtil.EnsureNlogConfig("NLog.config");
                //throw new Exception("�����쳣");//for test
                //������Ŀ����ʱ��Ҫ��������
                NLogUtil.WriteAll(NLog.LogLevel.Trace, LogType.Web, "��վ����", "��վ�����ɹ�");
                host.Run();
            }
            catch (Exception ex)
            {
                //ʹ��nlogд��������־�ļ�����һ���ݿ�û����/���ӳɹ���
                NLogUtil.WriteFileLog(NLog.LogLevel.Error, LogType.Web, "��վ����", "��ʼ�������쳣", ex);
                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
             .ConfigureLogging(logging =>
             {
                 //logging.ClearProviders(); //�Ƴ��Ѿ�ע���������־�������
                 logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace); //������С����־����
             }).UseNLog() //NLog: Setup NLog for Dependency injection
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
