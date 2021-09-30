using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Poc.Grpc.Client.Services
{
    public class GreetService : IGreetService
    {
        private readonly Greeter.GreeterClient _client;

        public GreetService(Greeter.GreeterClient client)
        {
            _client = client;
        }

        public async Task<string> SayHellos(HelloRequest request)
        {
            var resp = await _client.SayHelloAsync(request);
            return await Task.FromResult(resp.Message);
        }
    }

    public interface IGreetService
    {
        public Task<string> SayHellos(HelloRequest request);
    }
}
