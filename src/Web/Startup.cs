using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Web.Attributes;
using Web.Services.Implement;
using Web.Services.Interface;

namespace Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // ����k Runtime �ɳQĲ�o�A�ϥΦ���k�[�J�����A�Ȧܮe��
        public void ConfigureServices(IServiceCollection services)
        {
            // �`�J�һ� Services
            services.AddTransient<ICustomService, CustomService>();
            services.AddTransient<IOtherService, OtherService>();

            services.AddControllers();

            // �]�w�ʺA�N�z
            // https://github.com/dotnetcore/AspectCore-Framework
            services.ConfigureDynamicProxy(config => { config.Interceptors.AddTyped<ServiceAopAttribute>(Predicates.ForMethod("Execute*")); });
        }

        // ����k Runtime �ɳQĲ�o�A�ϥΦ���k�]�m HTTP �n�D�޹D(HTTP request pipeline)
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
