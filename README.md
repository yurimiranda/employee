Para o funcionamento da API, é necessário iniciar um DB Postgres com as seguintes configurações:
```
Host: localhost
Port: 5432
Database: postgres
User: postgres
Password: 123456 (Pode ser alterado se desejar. Porém, é necessário alterar no appsettings também.)
```

Para rodar as migrations basta apenas executar este comando na raiz do diretório do projeto:
```
dotnet ef database update -p .\src\Employee.Infra.EFCore\ -s .\src\Employee.Host\ -v
```
