HeatWatch API

  

📖 Descrição do Projeto

HeatWatch API é uma aplicação RESTful desenvolvida com ASP.NET Core (.NET 8) para monitoramento de regiões, eventos de calor, registros de temperatura e alertas. O serviço utiliza Oracle como banco de dados, JWT para autenticação, e oferece:

Versionamento de API via URL

Tratamento de erros padronizado (RFC7807)

Cache e controle de ETag

Rate limiting por IP

Documentação automática com Swagger/OpenAPI

🛠 Tecnologias Utilizadas

Camada

Framework / Biblioteca

Backend

ASP.NET Core 8

ORM

Entity Framework Core (Oracle)

Banco de Dados

Oracle Managed Data Access/Core

Autenticação

JWT (Microsoft.AspNetCore.Authentication.JwtBearer)

Versionamento

Microsoft.AspNetCore.Mvc.Versioning

Rate Limiting

AspNetCoreRateLimit

Erros

Hellang.Middleware.ProblemDetails

Documentação

Swashbuckle.AspNetCore (Swagger)

Testes Unitários

xUnit, Moq

Testes de Integração

Microsoft.AspNetCore.Mvc.Testing

🚀 Como Executar

Pré-requisitos:

.NET 8 SDK

Instância Oracle acessível (string de conexão válida)

Configuração:
Rename appsettings.json.example para appsettings.json e ajuste DefaultConnection:

{
  "ConnectionStrings": {
    "DefaultConnection": "User Id=...;Password=...;Data Source=..."
  }
}

Build & Run:

cd HeatWatch.API
dotnet build
dotnet run

Acesso:

Swagger UI: https://localhost:{PORT}/ (RoutePrefix vazio)

Endpoints: https://localhost:{PORT}/api/v1/[recurso]

📡 Endpoints

Base Url: /api/v{version:apiVersion} (p.ex. /api/v1)

Recurso

Rota completa

Regiões

/api/v1/regioes

Eventos de Calor

/api/v1/eventos-calor

Registros de Temperatura

/api/v1/registros-temperatura

Alertas

/api/v1/alertas

Formato de Rota e Exemplos

Regiões

GET /api/v1/regioes?page=1&size=20&sort=Nome&filter=...

GET /api/v1/regioes/{id}

POST /api/v1/regioesExemplo body:

{
  "nome": "Região Central",
  "latitude": -23.5505,
  "longitude": -46.6333,
  "descricao": "Área central",
  "area": 1521.11
}

PUT /api/v1/regioes/{id}Body igual ao POST

DELETE /api/v1/regioes/{id}

Respostas Comuns: 200 OK, 201 Created, 204 No Content, 400 Bad Request, 404 Not Found, 429 Too Many Requests

Eventos de Calor

GET /api/v1/eventos-calor

GET /api/v1/eventos-calor/{id}

POST /api/v1/eventos-calorBody:

{
  "nome": "Ondas de Calor",
  "dataInicio": "2025-06-01T00:00:00Z",
  "dataFim": "2025-06-05T00:00:00Z",
  "intensidade": 7,
  "regiaoId": 1
}

PUT /api/v1/eventos-calor/{id}

DELETE /api/v1/eventos-calor/{id}

Registros de Temperatura

GET /api/v1/registros-temperatura

GET /api/v1/registros-temperatura/{id}

POST /api/v1/registros-temperaturaBody:

{
  "regiaoId": 2,
  "dataRegistro": "2025-06-08T10:00:00Z",
  "temperaturaCelsius": 36.6
}

PUT /api/v1/registros-temperatura/{id}

DELETE /api/v1/registros-temperatura/{id}

Alertas

GET /api/v1/alertas

GET /api/v1/alertas/{id}

POST /api/v1/alertasBody:

{
  "mensagem": "Alerta de calor crítico",
  "dataEmissao": "2025-06-08T12:00:00Z",
  "severidade": "Alta",
  "eventoCalorId": 1
}

PUT /api/v1/alertas/{id}

DELETE /api/v1/alertas/{id}

⚙️ Instruções de Testes

Testes Unitários

Local: HeatWatch.API.Tests/Unit

dotnet test --filter Category=Unit

Testes de Integração

Local: HeatWatch.API.Tests/Integration

dotnet test --filter Category=Integration

Este README foi gerado seguindo boas práticas de documentação, garantindo clareza e facilidade de uso.

