using Facade.API.Classes;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Facade.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient<IRequestMediatorService, RequestMediatorService>();
            services.AddRequestResponseBus();
            services.AddCors();
            services.AddSignalR();
            services.AddSwagger();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCorsExtensions();
            app.UseSignalRExtensions();
            app.UseSwaggerExtensions();
            app.UseMvc();
        }


    }
}
