using Facade.API.Classes;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Persistence.Classes
{
    public class SubscribeSevice : BackgroundService
    {
        private readonly IBusControl _bus;
        public SubscribeSevice(AddContext contextDb)
        {

            _bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                IRabbitMqHost host = cfg.Host("localhost", "/", config =>
                {
                    config.Username("guest");
                    config.Password("guest");
                });

                cfg.ReceiveEndpoint(host, "requestService", e =>
                {

                    e.Handler<RequestMessage>(context =>
                    {
                        bool resultTransaction;
                        using (var transaction = contextDb.Database.BeginTransaction())
                        {
                            try
                            {
                                contextDb.Contracts.Add(context.Message);
                                contextDb.SaveChanges();
                                transaction.Commit();
                                resultTransaction = true;
                            }
                            catch
                            {
                                transaction.Rollback();
                                resultTransaction = false;
                            }
                        }
                        return context.RespondAsync<RequestResponse>(
                            new RequestResponse
                            {
                                Data = resultTransaction
                            });
                    });
                });
            });
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _bus.StartAsync();
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.WhenAll(base.StopAsync(cancellationToken), _bus.StopAsync());
        }
    }
}
