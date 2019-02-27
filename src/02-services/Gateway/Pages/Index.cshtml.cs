using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RabbitQueue;

namespace Gateway.Pages
{
  public class IndexModel : PageModel
  {
    private static IConnection connection;

    [BindProperty]
    public string UserInput { get; set; }

    [BindProperty]
    public string ReplyText { get; set; }

    public void OnGet()
    {
      if (connection == null)
      {
        var factory = new ConnectionFactory() { HostName = "40.117.117.72" };
        connection = factory.CreateConnection();
      }
    }

    public async Task OnPost()
    {
      SendMessage(UserInput);
      await WaitForReply();
    }

    private async Task WaitForReply()
    {
      var resetEvent = new AsyncManualResetEvent();
      using (var channel = connection.CreateModel())
      {
        channel.QueueDeclare(queue: "gateway", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
          var message = Encoding.UTF8.GetString(ea.Body);
          ReplyText = message;
          resetEvent.Set();
        };
        channel.BasicConsume(queue: "gateway", autoAck: true, consumer: consumer);
        await resetEvent.WaitAsync();
      }
    }

    private void SendMessage(string message)
    {
      using (var channel = connection.CreateModel())
      {
        channel.QueueDeclare(
          queue: "greeter", 
          durable: false, 
          exclusive: false, 
          autoDelete: false, 
          arguments: null);

        var body = Encoding.UTF8.GetBytes(message);

        var props = channel.CreateBasicProperties();
        props.ReplyTo = "gateway";
        props.CorrelationId = Guid.NewGuid().ToString();
        channel.BasicPublish(
          exchange: "", 
          routingKey: "greeter", 
          basicProperties: props, 
          body: body);
      }
    }
  }
}
