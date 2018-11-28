// Code from Stephen Toub @ Microsoft
// https://blogs.msdn.microsoft.com/pfxteam/2012/02/11/building-async-coordination-primitives-part-1-asyncmanualresetevent/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gateway.Pages
{
  public class AsyncManualResetEvent
  {
    private volatile TaskCompletionSource<bool> m_tcs = new TaskCompletionSource<bool>();

    public Task WaitAsync() { return m_tcs.Task; }

    public void Set() { m_tcs.TrySetResult(true); }
    public void Reset()
    {
      while (true)
      {
        var tcs = m_tcs;
        if (!tcs.Task.IsCompleted ||
            Interlocked.CompareExchange(ref m_tcs, new TaskCompletionSource<bool>(), tcs) == tcs)
          return;
      }
    }
  }
}
