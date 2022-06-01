namespace App1
{
    using EasyCaching.Core.Configurations;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEasyCaching(option =>
            {
                // local
                option.UseInMemory("m1");
                // distributed
                option.UseRedis(config =>
                {
                    config.DBConfig.Endpoints.Add(new ServerEndPoint("116.62.214.239", 6379));
                    config.DBConfig.Database = 5;
                }, "myredis");

                // combine local and distributed
                option.UseHybrid(config =>
                {
                    config.TopicName = "test-topic";
                    ////rabbitmq bus should use route key
                    //config.TopicName = "rmq.queue.undurable.easycaching.subscriber.*";

                    config.EnableLogging = true;

                    // specify the local cache provider name after v0.5.4
                    config.LocalCacheProviderName = "m1";
                    // specify the distributed cache provider name after v0.5.4
                    config.DistributedCacheProviderName = "myredis";
                }, "h1")
                // use redis bus
                 .WithRedisBus(busConf =>
                 {
                     busConf.Endpoints.Add(new ServerEndPoint("116.62.214.239", 6379));
                 });
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
