Para executar a aplicação basta apenas executar os seguintes comandos:
```
# Construir a imagem Docker
docker-compose build

# Executar os containers
docker-compose up
```

Após subir a aplicação é possível fazer um login de teste. Para isso, basta fazer uma requisição POST para o endpoint /api/signin com o seguinte payload:
```
{
	"email": "john.doe@example.com",
	"password": "Teste@1234"
}
```