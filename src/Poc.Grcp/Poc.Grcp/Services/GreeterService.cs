using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Poc.Grcp
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {

            HelloReply result = null;
            try
            {
                //throw new Exception();
                result = new HelloReply
                {
                    Message = "Hello " + request.Name + " | hora: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
                    Response = new Response()
                    {
                        Status = (int)StatusCode.OK
                    }
                };
            }
            catch (RpcException e) when (e.StatusCode == StatusCode.Unauthenticated)
            {
                result = new HelloReply
                {
                    Response = new Response()
                    {
                        ErrorMessage = e.Message,
                        Status = (int)e.StatusCode
                    }
                };
            }
            catch (RpcException e)
            {
                result = new HelloReply
                {
                    Response = new Response()
                    {
                        ErrorMessage = e.Message,
                        Status = (int)e.StatusCode
                    }
                };
            }
            catch (Exception e)
            {
                result = new HelloReply
                {
                    Response = new Response()
                    {
                        ErrorMessage = e.Message,
                        Status = (int)StatusCode.Unknown
                    }
                };
            }

            return await Task.FromResult(result);


            //return await FromResultAsync<HelloReply>(delegate
            //{
            //    return Task.FromResult(new HelloReply
            //    {
            //        Message = "Hello " + request.Name + " | hora: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")
            //    });
            //});
        }
    }
}
