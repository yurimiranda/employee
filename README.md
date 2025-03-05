Para o funcionamento da API, é necessário iniciar um DB Postgres com as seguintes configurações:
```
Host: localhost
Port: 5432
Database: postgres
User: postgres
Password: 123456 (Pode ser alterado se desejar. Porém, é necessário alterar no appsettings também.)
```

Para rodar as migrations basta apenas executar este comando no diretório raiz do projeto:
```
dotnet ef database update -p .\src\Employee.Infra.EFCore\ -s .\src\Employee.Host\ -v
```

Após rodar as migrations é possível fazer um login de teste. Para isso, basta fazer uma requisição POST para o endpoint **/api/signin** com o seguinte payload:
```
{
	"email": "john.doe@example.com",
	"password": "Teste@1234"
}
```