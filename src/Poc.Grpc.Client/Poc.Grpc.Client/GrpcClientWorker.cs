using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Poc.Grpc.Client.Services;

namespace Poc.Grpc.Client
{
    public class GrpcClientWorker : IHostedService, IDisposable
    {
        private IGreetService _greetclient;

        public GrpcClientWorker(IGreetService greetclient)
        {
            _greetclient = greetclient;
        }

        public static long i = 0;
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Stopwatch el = new Stopwatch();
            while (true)
            {
                el.Start();
                Console.WriteLine("faz algo");
                var resp = await _greetclient.SayHellos(new HelloRequest() { Name = "Clovis" });
                el.Stop();
                Console.WriteLine(resp + i++ +" | durou: (" + el.Elapsed.TotalMilliseconds + "ms)\n");
                el.Reset();
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("terminou de fazer algo");
        }

        public void Dispose()
        {
        }
    }
}
