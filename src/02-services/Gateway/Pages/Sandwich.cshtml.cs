using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitQueue;

namespace Gateway.Pages
{
  public class SandwichModel : PageModel
  {
    readonly IConfiguration _config;

    public SandwichModel(IConfiguration config)
    {
      _config = config;
    }

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
      var request = new Messages.SandwichRequest
      {
        Meat = TheMeat,
        Bread = TheBread,
        Cheese = TheCheese,
        Lettuce = TheLettuce
      };
      var result = await Controllers.SandwichController.RequestSandwich(request, _config["rabbitmq:url"]);

      if (result.Success)
        ReplyText = result.Description;
      else
        ReplyText = result.Error;
    }
  }
}