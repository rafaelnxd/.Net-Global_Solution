# HeatWatch API

**HeatWatch API** é uma aplicação RESTful desenvolvida em **ASP.NET Core 8** para monitoramento de regiões, eventos de calor, registros de temperatura e alertas.

---

## 📖 Descrição do Projeto

O serviço oferece:

- Versionamento de API via URL (v1, v2, …)  
- Tratamento de erros padronizado (RFC7807)  
- Cache e controle de ETag  
- Rate limiting por IP  
- Autenticação JWT  
- Documentação automática com Swagger/OpenAPI  

---

## Vídeo:
https://www.youtube.com/watch?v=uOzpU0tkYqs

## 🛠 Tecnologias Utilizadas

| Camada           | Tecnologia / Biblioteca                                       |
| ---------------- | ------------------------------------------------------------- |
| **Backend**      | ASP.NET Core 8                                                |
| **ORM**          | Entity Framework Core (Oracle)                                |
| **Banco de Dados** | Oracle Managed Data Access/Core                             |
| **Autenticação** | Microsoft.AspNetCore.Authentication.JwtBearer                |
| **Versionamento**| Microsoft.AspNetCore.Mvc.Versioning                           |
| **Rate Limiting**| AspNetCoreRateLimit                                           |
| **Tratamento Erros** | Hellang.Middleware.ProblemDetails                        |
| **Documentação** | Swashbuckle.AspNetCore (Swagger)                              |
| **Testes Unitários** | xUnit, Moq                                               |
| **Integração**   | Microsoft.AspNetCore.Mvc.Testing                              |

---

## 🚀 Como Executar

### 1. Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)  
- Instância Oracle acessível  

### 2. Configuração

1. Renomeie `appsettings.json.example` para `appsettings.json`.  
2. Ajuste a connection string:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "User Id=<usuário>;Password=<senha>;Data Source=<host>:<porta>/<serviço>"
     }
   }
3. Build & Run
bash
Copiar
Editar
cd HeatWatch.API
dotnet build
dotnet run
4. Acesso à API
Swagger UI: https://localhost:{PORT}/

Base URL: https://localhost:{PORT}/api/v1

📡 Endpoints
Base URL: /api/v{version} (ex.: /api/v1)

Regiões
Método	Rota	Descrição
GET	/api/v1/regioes	Lista regiões (paginação)
GET	/api/v1/regioes/{id}	Recupera região por ID
POST	/api/v1/regioes	Cria nova região
PUT	/api/v1/regioes/{id}	Atualiza região existente
DELETE	/api/v1/regioes/{id}	Remove região

Eventos de Calor
Método	Rota	Descrição
GET	/api/v1/eventos-calor	Lista eventos de calor
GET	/api/v1/eventos-calor/{id}	Recupera evento por ID
POST	/api/v1/eventos-calor	Cria novo evento
PUT	/api/v1/eventos-calor/{id}	Atualiza evento existente
DELETE	/api/v1/eventos-calor/{id}	Remove evento

Registros de Temperatura
Método	Rota	Descrição
GET	/api/v1/registros-temperatura	Lista registros
GET	/api/v1/registros-temperatura/{id}	Recupera registro por ID
POST	/api/v1/registros-temperatura	Cria novo registro
PUT	/api/v1/registros-temperatura/{id}	Atualiza registro existente
DELETE	/api/v1/registros-temperatura/{id}	Remove registro

Alertas
Método	Rota	Descrição
GET	/api/v1/alertas	Lista alertas
GET	/api/v1/alertas/{id}	Recupera alerta por ID
POST	/api/v1/alertas	Cria nova alerta
PUT	/api/v1/alertas/{id}	Atualiza alerta existente
DELETE	/api/v1/alertas/{id}	Remove alerta

📄 Exemplos de Body (JSON)
POST /api/v1/regioes
json
Copiar
Editar
{
  "nome": "Região Central",
  "latitude": -23.5505,
  "longitude": -46.6333,
  "descricao": "Área central da cidade",
  "area": 1521.11
}
POST /api/v1/eventos-calor
json
Copiar
Editar
{
  "nome": "Ondas de Calor",
  "dataInicio": "2025-06-01T00:00:00Z",
  "dataFim": "2025-06-05T00:00:00Z",
  "intensidade": 7,
  "regiaoId": 1
}
POST /api/v1/registros-temperatura
json
Copiar
Editar
{
  "regiaoId": 2,
  "dataRegistro": "2025-06-08T10:00:00Z",
  "temperaturaCelsius": 36.6
}
POST /api/v1/alertas
json
Copiar
Editar
{
  "mensagem": "Alerta de calor crítico",
  "dataEmissao": "2025-06-08T12:00:00Z",
  "severidade": "Alta",
  "eventoCalorId": 1
}
⚙️ Instruções de Testes
Testes Unitários
bash
Copiar
Editar
cd HeatWatch.API.Tests
dotnet test --filter Category=Unit
Testes de Integração
bash
Copiar
Editar
cd HeatWatch.API.Tests
dotnet test --filter Category=Integration
📌 Observações
A organização e formatação deste README influenciam diretamente a avaliação e a forma como outros desenvolvedores entenderão e utilizarão o projeto.

Siga as boas práticas de Markdown, mantenha seções claras e exemplos precisos.

Copiar
Editar
