using Facade.API.Classes;
using Facade.API.Hubs;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace Facade.API
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddRequestResponseBus(this IServiceCollection services)
        {
            IBusControl bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                IRabbitMqHost host = cfg.Host("localhost", "/", config =>
                {
                    config.Username("guest");
                    config.Password("guest");
                });
            });

            services.AddSingleton<IPublishEndpoint>(bus);
            services.AddSingleton<ISendEndpointProvider>(bus);
            services.AddSingleton<IBus>(bus);

            TimeSpan timeout = TimeSpan.FromSeconds(10);
            Uri serviceAddress = new Uri("rabbitmq://localhost/requestService");

            services.AddScoped<IRequestClient<RequestMessage, RequestResponse>>(x =>
                new MessageRequestClient<RequestMessage, RequestResponse>(x.GetRequiredService<IBus>(), serviceAddress, timeout, timeout));

            bus.Start();

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Facade.API",
                    Description = ".Net Core 2.2, MassTransit(RabbitMq), Mediatr, Swagger, EntityFramework(PostgresSql)",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "Dmitriy Makarov", Email = "makarovbryansk@yandex.re" }
                });
            });
            return services;
        }

        public static IApplicationBuilder UseCorsExtensions(this IApplicationBuilder app)
        {
            app.UseCors(builder => builder.SetIsOriginAllowed((host) => true)
                                          .AllowAnyMethod()
                                          .AllowCredentials()
                                          .AllowAnyHeader());
            return app;
        }

        public static IApplicationBuilder UseSignalRExtensions(this IApplicationBuilder app)
        {
            app.UseSignalR(routes =>
            {
                routes.MapHub<ResponseHub>("/connect");
            });
            return app;
        }

        public static IApplicationBuilder UseSwaggerExtensions(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

            });
            return app;
        }
    }
}
