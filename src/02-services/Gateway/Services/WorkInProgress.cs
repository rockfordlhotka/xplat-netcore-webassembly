using RabbitQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Services
{
  public static class WorkInProgress
  {
    public static readonly Dictionary<string, WipItem> WipList = 
      new Dictionary<string, WipItem>();
  }

  public class WipItem
  {
    public AsyncManualResetEvent Lock { get; set; }
    public Messages.SandwichResponse Response { get; set; }
  }
}
