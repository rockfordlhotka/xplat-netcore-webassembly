using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using RabbitQueue;

namespace SandwichMaker
{
	 class SandwichMaker
	 {
    private static Queue _queue = new Queue("40.117.117.72", "sandwichmaker");
    private static readonly Dictionary<string, SandwichInProgress> _workInProgress =
      new Dictionary<string, SandwichInProgress>();

    static async Task Main(string[] args)
    {
      Console.WriteLine("SandwichMaker starting to listen");
      _queue.StartListening(HandleMessage);

      // wait forever - we run until the container is stopped
      await new AsyncManualResetEvent().WaitAsync();
    }

    private static void HandleMessage(BasicDeliverEventArgs ea, string message)
    {
      switch (ea.BasicProperties.Type)
      {
        case "SandwichRequest":
          RequestIngredients(ea, message);
          break;
        case "MeatBinResponse":
          HandleMeatBinResponse(ea, message);
          break;
        case "BreadBinResponse":
          HandleBreadBinResponse(ea, message);
          break;
        case "CheeseBinResponse":
          HandleCheeseBinResponse(ea, message);
          break;
        case "LettuceBinResponse":
          HandleLettuceBinResponse(ea, message);
          break;
        default:
          Console.WriteLine($"Unknown message type '{ea.BasicProperties.Type}' from {ea.BasicProperties.ReplyTo}");
          break;
      }
    }

    private static void HandleCheeseBinResponse(BasicDeliverEventArgs ea, string message)
    {
      if (_workInProgress.TryGetValue(ea.BasicProperties.CorrelationId, out SandwichInProgress wip))
      {
        var response = JsonConvert.DeserializeObject<Messages.CheeseBinResponse>(message);
        wip.GotCheese = response.Success;
        SeeIfSandwichIsComplete(wip);
      }
      else
      {
        // got Cheese we apparently don't need, so return it
        Console.WriteLine("Returning unneeded Cheese");
        _queue.SendReply("Cheesebin", null, new Messages.CheeseBinRequest { Returning = true });
      }
    }

    private static void HandleLettuceBinResponse(BasicDeliverEventArgs ea, string message)
    {
      if (_workInProgress.TryGetValue(ea.BasicProperties.CorrelationId, out SandwichInProgress wip))
      {
        var response = JsonConvert.DeserializeObject<Messages.LettuceBinResponse>(message);
        wip.GotLettuce = response.Success;
        SeeIfSandwichIsComplete(wip);
      }
      else
      {
        // got lettuce we apparently don't need, so return it
        Console.WriteLine("Returning unneeded lettuce");
        _queue.SendReply("lettucebin", null, new Messages.LettuceBinRequest { Returning = true });
      }
    }

    private static void HandleBreadBinResponse(BasicDeliverEventArgs ea, string message)
    {
      if (_workInProgress.TryGetValue(ea.BasicProperties.CorrelationId, out SandwichInProgress wip))
      {
        var response = JsonConvert.DeserializeObject<Messages.BreadBinResponse>(message);
        wip.GotBread = response.Success;
        SeeIfSandwichIsComplete(wip);
      }
      else
      {
        // got Bread we apparently don't need, so return it
        Console.WriteLine("Returning unneeded Bread");
        _queue.SendReply("Breadbin", null, new Messages.BreadBinRequest { Returning = true });
      }
    }

    private static void HandleMeatBinResponse(BasicDeliverEventArgs ea, string message)
    {
      if (_workInProgress.TryGetValue(ea.BasicProperties.CorrelationId, out SandwichInProgress wip))
      {
        var response = JsonConvert.DeserializeObject<Messages.MeatBinResponse>(message);
        wip.GotMeat = response.Success;
        SeeIfSandwichIsComplete(wip);
      }
      else
      {
        // got Meat we apparently don't need, so return it
        Console.WriteLine("Returning unneeded Meat");
        _queue.SendReply("Meatbin", null, new Messages.MeatBinRequest { Returning = true });
      }
    }

    private static void SeeIfSandwichIsComplete(SandwichInProgress wip)
    {
      if (wip.IsComplete)
      {
        _queue.SendReply(wip.ReplyTo, wip.CorrelationId, new Messages.SandwichReponse
        {
          Description = wip.GetDescription(),
          Success = wip.Failed,
          Error = wip.GetFailureReason()
        });
      }
    }
    
    private static void RequestIngredients(BasicDeliverEventArgs ea, string message)
    {
      var request = JsonConvert.DeserializeObject<Messages.SandwichRequest>(message);
      _workInProgress.Add(ea.BasicProperties.CorrelationId, new SandwichInProgress
      {
        ReplyTo = ea.BasicProperties.ReplyTo,
        CorrelationId = ea.BasicProperties.CorrelationId,
        Request = request
      });
      Console.WriteLine($"Processing request for {request.Meat} on {request.Bread}{Environment.NewLine}  from {ea.BasicProperties.CorrelationId} at {DateTime.Now}");

      if (!string.IsNullOrEmpty(request.Meat))
      {
        _queue.SendMessage("MeatBin", ea.BasicProperties.CorrelationId,
          new Messages.MeatBinRequest { Meat = request.Meat });
      }
      if (!string.IsNullOrEmpty(request.Bread))
      {
        _queue.SendMessage("BreadBin", ea.BasicProperties.CorrelationId,
          new Messages.BreadBinRequest { Bread = request.Bread });
      }
      if (!string.IsNullOrEmpty(request.Cheese))
      {
        _queue.SendMessage("CheeseBin", ea.BasicProperties.CorrelationId,
          new Messages.CheeseBinRequest { Cheese = request.Cheese });
      }
      if (request.Lettuce)
      {
        _queue.SendMessage("LettuceBin", ea.BasicProperties.CorrelationId,
          new Messages.LettuceBinRequest());
      }
    }
  }
}
