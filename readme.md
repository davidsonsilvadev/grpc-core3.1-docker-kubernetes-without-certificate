# gRPC | C# | Docker

Neste case subo 2 pods 1 com um pequeno metodo exposto com protocolo grpc usando o google protobuffer
outro que consome este protobuffer atraves do serviço, de forma direta. 
Notem que nao estou usando certifica (lembrando que por padrão gRPC trabalha com TLS), pois trata-se 
de uma comunicação interna entre PODs pelo ClusterIP. O objetivo era exatamente este, trazer um exmplo, simples,
de app dockerizada em utilização do TLS.

## Dependencias
 - [Core 3.1](https://dotnet.microsoft.com/download/dotnet/3.1)
 - [Docker+Kubernetes](https://www.docker.com/products/docker-desktop)
 - [Visual Studio 2019](https://visualstudio.microsoft.com/pt-br/vs/)

## Utilização

Após baixar o repositório, crie as imagens no seu docker local.

Obs: Nos exemplos abaixo, os comandos são rodados dentro da pasta "src"

#### Docker Build - Server
```bash
docker build -t testegrpcserver:v1 .\Poc.Grcp\
```
#### Docker Build - Client
```bash
docker build -t testegrpcclient:v1 .\Poc.Grpc.Client\
```
#### Aplicar k8s no Kubernetes

```bash
kubectl apply -f k8s.yml
```

## 🛠 Skills
.NET,C#,Docker,Kubernetes...

## Contributing

Contributions are always welcome!

😎🐧🐧🐧🐧🐧🐧🐧🐧🐧🐧🐧🐧🐧🐧🐧🐧🐧🐧🐧🐧🐧

## License

[MIT](https://choosealicense.com/licenses/mit/)
