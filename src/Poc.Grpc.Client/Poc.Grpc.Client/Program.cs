using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Poc.Grpc.Client.Services;

namespace Poc.Grpc.Client
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).RunConsoleAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseConsoleLifetime()
                .ConfigureServices((hostContext, services) =>
                {

                    IConfiguration configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", true, true)
                        .Build();

                    AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                    AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

                    var httpHandler = new HttpClientHandler();
                    httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

                    var channel = GrpcChannel.ForAddress(configuration["urlApi"], new GrpcChannelOptions
                    {
                        HttpHandler = httpHandler
                    });

                    services.AddGrpcClient<Greeter.GreeterClient>(o =>
                    {
                        o.Address = new Uri(configuration["urlApi"]);
                    }).ConfigureChannel(c =>
                    {
                        c.HttpHandler = httpHandler;
                    });
                    services.AddSingleton<IGreetService, GreetService>();

                    services.AddHostedService<GrpcClientWorker>();
                });


        static async Task Main2(string[] args)
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
                    httpHandler.ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;


                    //Controle de Retentativas
                    var defaultMethodConfig = new MethodConfig
                    {
                        Names = { MethodName.Default },
                        RetryPolicy = new RetryPolicy
                        {
                            MaxAttempts = 5,
                            InitialBackoff = TimeSpan.FromSeconds(1),
                            MaxBackoff = TimeSpan.FromSeconds(10),
                            BackoffMultiplier = 1.5,
                            RetryableStatusCodes = { StatusCode.Unknown }
                        }
                    };

                    // The port number(5001) must match the port of the gRPC server.
                    var channel = GrpcChannel.ForAddress(urlApi, new GrpcChannelOptions
                    {
                        HttpHandler = httpHandler,
                        ServiceConfig = new ServiceConfig { MethodConfigs = { defaultMethodConfig } }
                    });

                    var client = new Greeter.GreeterClient(channel);
                    sh.Stop();

                    var reply = await client.SayHelloAsync(
                        new HelloRequest { Name = "Davidson!!" },
                        deadline: DateTime.UtcNow.AddSeconds(10));

                    Console.WriteLine("Fim Consulta GRPC Server! | " + "durou: (" + sh.Elapsed.TotalMilliseconds + "ms)");
                    Console.WriteLine("Message: " + reply.Message);
                    Console.WriteLine("Aguardando 10 segundos para próximo processo!");
                    await Task.Delay(TimeSpan.FromSeconds(10));
                } while (!stop);
            }
            catch (RpcException rpcEx) when (rpcEx.StatusCode == StatusCode.DeadlineExceeded)
            {
                Console.WriteLine("Greeting timeout.");
            }
            catch (RpcException rpcEx)
            {
                Console.WriteLine($"Greeting timeout.{rpcEx}");
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
