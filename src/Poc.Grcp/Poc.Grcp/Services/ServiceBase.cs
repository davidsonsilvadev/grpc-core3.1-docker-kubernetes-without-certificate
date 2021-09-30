//using System;
//using System.Threading.Tasks;
//using Grpc.Core;

//namespace Poc.Grcp.Services
//{
//    public static class ServiceBase
//    {
//        public static async Task<tt2> FromResultAsync<tt2>(Func<Task<tt2>> fc) where tt2: new()
//        {
//            try
//            {
//                return await fc();
//            }
//            catch (RpcException e) when (e.StatusCode == StatusCode.Unknown)
//            {
//                return new tt2()
//                {
//                    Response = new Response()
//                    {
//                        ErrorMessage = e.Message,
//                        Status = (int)e.StatusCode
//                    }
//                };
//            }
//            catch (RpcException e)
//            {
//                result = new HelloReply
//                {
//                    Response = new Response()
//                    {
//                        ErrorMessage = e.Message,
//                        Status = (int)e.StatusCode
//                    }
//                };
//            }
//            catch (Exception e)
//            {
//                result = new HelloReply
//                {
//                    Response = new Response()
//                    {
//                        ErrorMessage = e.Message,
//                        Status = (int)StatusCode.Unknown
//                    }
//                };
//            }
//        }
//    }
//}
