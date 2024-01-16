ProductApi

Execução do código -> No prompt de comando dentra da pasta do projeto ProductApi.WebApi execute "dotnet run"

ProductApi.Domain -> Inclui entidades e objetos de valores. Interage com a aplicação de infraestrutra através de abstrações e dependencias(interfaces e contratos)
ProductApi.Infrastructure -> Implementa o acesso aos dados.
ProductApi.Application -> Implementar  os serviços da aplicação, DTOs e mappers.
ProductApi.WebApi -> Implementa a API.
ProductApi.Tests -> Implementa os testes unitários dos projetos da solution.

Foi utilizado Entity Framework Code First, com banco de dados InMemory e seedData que popula a data inicial, que é uma boa pratica usando code-first.