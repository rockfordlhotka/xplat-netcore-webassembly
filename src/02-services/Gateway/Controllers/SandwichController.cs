using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitQueue;

namespace Gateway.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SandwichController : ControllerBase
  {
    [HttpGet]
    public string OnGet()
    {
      return "I am running";
    }

    [HttpPut]
    public async Task<Messages.SandwichReponse> OnPut(Messages.SandwichRequest request)
    {
      var result = new Messages.SandwichReponse();
      using (var _queue = new Queue("dinky-wallaby-rabbitmq.default.svc.cluster.local", "customer"))
      {
        var reset = new AsyncManualResetEvent();
        _queue.StartListening((ea, message) =>
        {
          var response = JsonConvert.DeserializeObject<Messages.SandwichReponse>(message);
          result.Success = response.Success;
          result.Description = $"SUCCESS: {response.Description}";
          result.Error = $"FAILED: {response.Error}";
          reset.Set();
        });

        var requestToCook = new Messages.SandwichRequest
        {
          Meat = request.Meat,
          Bread = request.Bread,
          Cheese = request.Cheese,
          Lettuce = request.Lettuce
        };
        _queue.SendMessage("sandwichmaker", Guid.NewGuid().ToString(), requestToCook);

        var task = reset.WaitAsync();
        if (await Task.WhenAny(task, Task.Delay(10000)) != task)
          result.Error = "The cook didn't get back to us in time, no sandwich";

        return result;
      }
    }
  }
}