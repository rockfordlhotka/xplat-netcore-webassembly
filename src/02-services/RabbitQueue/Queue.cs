using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitQueue
{
  /// <summary>
  /// Helper methods to simplify use of RabbitMQ
  /// </summary>
  public class Queue
  {
    private IConnection connection;
    private readonly string serviceName;

    public Queue(string hostName, string serviceName)
    {
      this.serviceName = serviceName;
      var factory = new ConnectionFactory() { HostName = hostName };
      connection = factory.CreateConnection();
    }

    public void SendMessage(string destination, string correlationId, object request)
    {
      SendMessage(serviceName, destination, correlationId, request);
    }

    public void SendReply(string destination, string correlationId, object request)
    {
      SendMessage(null, destination, correlationId, request);
    }

    private void SendMessage(string sender, string destination, string correlationId, object request)
    {
      var message = JsonConvert.SerializeObject(request);
      using (var channel = connection.CreateModel())
      {
        channel.QueueDeclare(queue: destination, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var body = Encoding.UTF8.GetBytes(message);

        var props = channel.CreateBasicProperties();
        if (!string.IsNullOrWhiteSpace(sender))
          props.ReplyTo = sender;
        props.CorrelationId = correlationId;
        props.Type = request.GetType().Name;
        channel.BasicPublish(exchange: "", routingKey: destination, basicProperties: props, body: body);
      }
    }

    public void StartListening(Action<BasicDeliverEventArgs> handleMessage)
    {
      using (var channel = connection.CreateModel())
      {
        channel.QueueDeclare(queue: serviceName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) => { handleMessage(ea); };
        channel.BasicConsume(queue: serviceName, autoAck: true, consumer: consumer);
      }
    }
  }
}
