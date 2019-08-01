using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RabbitQueue;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Gateway.Services
{
  public class SandwichmakerListenerHostedService : IHostedService
  {
    private readonly IConfiguration _config;
    private readonly Queue _queue;

    public SandwichmakerListenerHostedService(IConfiguration config)
    {
      _config = config;
      _queue = new Queue(_config["rabbitmq:url"], "customer");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      _queue.StartListening((ea, message) =>
      {
        if (WorkInProgress.WipList.TryGetValue(ea.BasicProperties.CorrelationId, out WipItem wipItem))
        {
          wipItem.Response = new Messages.SandwichResponse();
          var response = JsonConvert.DeserializeObject<Messages.SandwichResponse>(message);
          wipItem.Response.Success = response.Success;
          wipItem.Response.Description = $"SUCCESS: {response.Description}";
          wipItem.Response.Error = $"FAILED: {response.Error}";
          wipItem.Lock.Set();
        }
        else
        {
          // log that we got an orphan message
          System.Diagnostics.Debug.Fail("got orphan message from queue");
        }
      });

      return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      _queue.Dispose();

      return Task.CompletedTask;
    }
  }
}
