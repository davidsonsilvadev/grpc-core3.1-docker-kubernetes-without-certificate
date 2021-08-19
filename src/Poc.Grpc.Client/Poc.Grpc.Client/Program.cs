using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace Poc.Grpc.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                bool stop = false;

                Console.WriteLine("Buscando configucações!");
                IConfiguration configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
                Console.WriteLine("Configucações ajustadas!");

                string urlApi = configuration["urlApi"];

                Console.WriteLine("Url utilizada será " + urlApi);

                do
                {
                    Console.WriteLine("Novo ciclo iniciado!");
                    Console.WriteLine("Inicio Consulta GRPC Server!");
                    Stopwatch sh = new Stopwatch();
                    sh.Start();

                    // Aqui permitimos a comunicação não segura | Lembrando que isso não é recomendado para acesso publico
                    AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                    AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

                    var httpHandler = new HttpClientHandler();
                    httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;


                    // The port number(5001) must match the port of the gRPC server.
                    var channel = GrpcChannel.ForAddress(urlApi, new GrpcChannelOptions { HttpHandler = httpHandler });

                    var client = new Greeter.GreeterClient(channel);
                    sh.Stop();
                    var reply = await client.SayHelloAsync(
                                      new HelloRequest { Name = "Davidson!!" });
                    Console.WriteLine("Fim Consulta GRPC Server! | " + "durou: (" + sh.Elapsed.TotalMilliseconds + "ms)");
                    Console.WriteLine("Message: " + reply.Message);
                    Console.WriteLine("Aguardando 10 segundos para próximo processo!");
                    await Task.Delay(TimeSpan.FromSeconds(10));
                } while (!stop);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Falha no processamento da aplicação");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
