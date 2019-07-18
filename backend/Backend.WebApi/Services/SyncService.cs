using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.WebApi.Services
{
    public class SyncService
    {
        public static readonly string Job = nameof(Job);

        private Dictionary<string, SemaphoreSlim> locks;

        public SyncService()
        {
            locks = new Dictionary<string, SemaphoreSlim>();
        }

        public async Task<Sema> GetLockObject(string topic)
        {
            lock(locks)
            {
                if(!locks.ContainsKey(topic))
                    locks.Add(topic, new SemaphoreSlim(1, 1));
            }

            var sema = new Sema(locks[topic]);

            await sema.Wait();

            return sema;
        }

        public class Sema : IDisposable
        {
            private SemaphoreSlim sema;
            internal Sema(SemaphoreSlim sema)
            {
                this.sema = sema;
            }

            public async Task Wait()
            {
                await sema.WaitAsync();
            }

            public void Dispose()
            {
                sema.Release();
            }
        }
    }
}