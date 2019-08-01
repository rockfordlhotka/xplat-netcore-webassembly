using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitQueue;

namespace Gateway.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SandwichController : ControllerBase
  {
    readonly IConfiguration _config;

    public SandwichController(IConfiguration config)
    {
      _config = config;
    }

    [HttpGet]
    public string OnGet()
    {
      return "I am running; use PUT to make a sandwich";
    }

    [HttpPut]
    public async Task<Messages.SandwichResponse> OnPut(Messages.SandwichRequest request)
    {
      return await RequestSandwich(request, _config["rabbitmq:url"]);
    }

    public static async Task<Messages.SandwichResponse> RequestSandwich(Messages.SandwichRequest request, string queueUrl)
    {
      var result = new Messages.SandwichResponse();
      var requestToCook = new Messages.SandwichRequest
      {
        Meat = request.Meat,
        Bread = request.Bread,
        Cheese = request.Cheese,
        Lettuce = request.Lettuce
      };
      var correlationId = Guid.NewGuid().ToString();
      var wipItem = new Services.WipItem { Lock = new AsyncManualResetEvent() };
      Services.WorkInProgress.WipList.Add(correlationId, wipItem);
      try
      {
        using (var _queue = new Queue(queueUrl, "customer"))
        {
          _queue.SendMessage("sandwichmaker", correlationId, requestToCook);
        }
        var messageArrived = wipItem.Lock.WaitAsync();
        if (await Task.WhenAny(messageArrived, Task.Delay(10000)) == messageArrived)
        {
          result = wipItem.Response;
        }
        else
        {
          result.Error = "The cook didn't get back to us in time, no sandwich";
          result.Success = false;
        }
      }
      finally
      {
        Services.WorkInProgress.WipList.Remove(correlationId);
      }

      return result;
    }
  }
}