using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using RabbitQueue;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CheeseService
{
  class CheeseBin
  {
    private static Queue _queue = new Queue("40.117.117.72", "cheesebin");

    static async Task Main(string[] args)
    {
      Console.WriteLine("Cheese bin service starting to listen");
      _queue.StartListening(HandleMessage);

      // wait forever - we run until the container is stopped
      await new AsyncManualResetEvent().WaitAsync();
    }

    private volatile static int _inventory = 10;

    private static void HandleMessage(BasicDeliverEventArgs ea, string message)
    {
      var request = JsonConvert.DeserializeObject<Messages.CheeseBinRequest>(message);
      var response = new Messages.CheeseBinResponse();
      lock (_queue)
      {
        if (request.Returning)
        {
          _inventory++;
        }
        else if (_inventory > 0)
        {
          _inventory--;
          response.Success = true;
          _queue.SendReply(ea.BasicProperties.ReplyTo, ea.BasicProperties.CorrelationId, response);
        }
        else
        {
          response.Success = false;
          _queue.SendReply(ea.BasicProperties.ReplyTo, ea.BasicProperties.CorrelationId, response);
        }
      }
    }
  }
}
