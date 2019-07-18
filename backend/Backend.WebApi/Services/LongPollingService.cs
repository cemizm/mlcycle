using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.WebApi.Services
{
    public class LongPollingService
    {
        public static readonly string StepsCreated = "steps.created";

        private Dictionary<string, List<TaskCompletionSource<object>>> handlers;

        public LongPollingService() 
        {
            handlers = new Dictionary<string, List<TaskCompletionSource<object>>>();
        }

        private TaskCompletionSource<object> CreateTask(string channel)
        {
            var t = new TaskCompletionSource<object>();
            lock(handlers)
            {
                List<TaskCompletionSource<object>> items;
                if(!handlers.TryGetValue(channel, out items))
                {
                    items = new List<TaskCompletionSource<object>>();
                    handlers.Add(channel, items);
                }

                items.Add(t);
            }
            return t;
        }

        public async Task<bool> Wait(string channel) 
        {
            var t = CreateTask(channel);

            Task res = await Task.WhenAny(t.Task , Task.Delay(3 * 60 * 1000));

            return res == t.Task;
        }

        public void Notify(string channel)
        {
            lock(handlers)
            {
                List<TaskCompletionSource<object>> items;
                if(!handlers.TryGetValue(channel, out items))
                    return;

                foreach(var item in items.ToArray())
                {
                    item.SetResult(null);
                    items.Remove(item);
                }
            }
        }
    }
}