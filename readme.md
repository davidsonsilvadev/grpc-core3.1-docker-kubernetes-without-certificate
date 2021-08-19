Neste case subo 2 pods

1 com um pequeno metodo exposto com protocolo grpc usando o google protobuffer

outro que consome este protobuffer atraves do serviço exporto


comandos
1 - Rodar yaml
	kubectl apply -f davidson-47aso-atividadems-k8s-.yaml

2 - ver logs
	kubectl logs -f -l app=poc-grpc-job
