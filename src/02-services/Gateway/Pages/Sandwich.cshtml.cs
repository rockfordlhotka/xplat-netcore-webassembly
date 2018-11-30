using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using RabbitQueue;

namespace Gateway.Pages
{
  public class SandwichModel : PageModel
  {
    private readonly RabbitQueue.Queue _queue = new RabbitQueue.Queue("40.117.117.72", "customer");

    [BindProperty]
    public string TheMeat { get; set; }
    [BindProperty]
    public string TheBread { get; set; }

    [BindProperty]
    public string TheCheese { get; set; }

    [BindProperty]
    public bool TheLettuce { get; set; }


    [BindProperty]
    public string ReplyText { get; set; }

    public void OnGet()
    {

    }

    public async Task OnPost()
    {
      _queue.StartListening((ea, message) =>
      {
        var response = JsonConvert.DeserializeObject<Messages.SandwichReponse>(message);
        if (response.Success)
          ReplyText = response.Description;
        else
          ReplyText = response.Error;
      });

      var request = new Messages.SandwichRequest
      {
        Meat = TheMeat,
        Bread = TheBread,
        Cheese = TheCheese,
        Lettuce = TheLettuce
      };
      _queue.SendMessage("sandwichmaker", Guid.NewGuid().ToString(), request);

      var task = new AsyncManualResetEvent().WaitAsync();
      if (await Task.WhenAny(task, Task.Delay(1000)) != task)
        ReplyText = "The cook didn't get back to us in time, no sandwich";
    }
  }
}