# SOSLocaliza

O SOSLocaliza é um projeto que visa promover a segurança da população em situação de risco climático. A aplicação desenvolvida em C# gerencia, como foco principal, o CRUD (Create, Read, Update, Delete) de usuários.

## 📊 Status do Projeto

```
✅ Unit Tests: 28/28 passed (100%)
✅ Integration Tests: 18/18 passed (100%)
✅ Total: 46/46 tests passed
```

### 🚀 Quick Start

**Run the application:**
```bash
cd Sprint1.API
dotnet run
```

**Check health:**
```bash
curl http://localhost:5132/health
```

**Run all tests:**
```bash
dotnet test
```

**Run specific test categories:**
```bash
# Unit tests only
dotnet test --filter Category=Unit

# Integration tests only
dotnet test --filter Category=Integration
```

## Sobre o Projeto

Este projeto é composto por duas aplicações principais:

1. **Sprint1.API**: API RESTful para gerenciamento de usuários
2. **Sprint1.Web**: Interface web MVC para gerenciamento de usuários

## Instalação e Configuração

### Pré-requisitos

- .NET SDK 8.0
- Oracle Database ou acesso a um servidor Oracle
- Visual Studio ou Rider

 **Executar a Aplicação**

   **API:**
   ```bash
   cd Sprint1.API
   dotnet run
   ```

   **Interface Web:**
   ```bash
   cd Sprint1.Web
   dotnet run
   ```

## API de Usuários

A API gerencia o CRUD completo de usuários e foi implementada seguindo os princípios da Clean Architecture.

### Escopo

O escopo deste projeto está focado na criação de uma API RESTful e interface web para manipular os dados dos usuários. As funcionalidades principais incluem:

* Criação de novos usuários
* Listagem de todos os usuários cadastrados
* Busca de um usuário específico por seu ID
* Atualização de informações específicas do usuário (email e senha)
* Remoção lógica de usuários (soft delete)
* Verificação de conexão com o banco de dados

## Funcionalidades Implementadas

### CRUD Completo de Usuários
- ✅ Criar usuário (API + Interface Web)
- ✅ Buscar usuário por ID (API + Interface Web)
- ✅ Listar todos os usuários (API + Interface Web)
- ✅ Alterar email específico (API + Interface Web)
- ✅ Alterar senha (API + Interface Web)
- ✅ Remover usuário (soft delete) (API + Interface Web)

### Monitoramento e Observabilidade

#### Health Checks (15 pts)
- ✅ Implemented 3 health check endpoints:
  - `/health` - Overall application health (API + Database)
  - `/health/ready` - Readiness probe for database connectivity
  - `/health/live` - Liveness probe for API status
- ✅ Database connectivity check using Entity Framework Core
- ✅ JSON formatted responses with detailed status information

#### Structured Logging (10 pts)
- ✅ Configured Serilog with multiple sinks (Console + File)
- ✅ Implemented log levels: Information, Warning, Error
- ✅ Request correlation with detailed context
- ✅ Rolling file logs with 30-day retention
- ✅ Added logging to UsuarioUseCase for all operations

#### Tracing and Metrics (15 pts)
- ✅ Implemented OpenTelemetry distributed tracing
- ✅ AspNetCore instrumentation for HTTP requests
- ✅ EntityFrameworkCore instrumentation for database queries
- ✅ Console exporter for trace visualization
- ✅ Performance metrics tracking (response times, error rates)

### Testes Automatizados - AAA Pattern

#### Unit Testing (20 pts)
- ✅ Created 28 unit tests following AAA pattern
- ✅ Domain Layer: 14 tests for Usuario entity
  - Constructor validation, email/password changes, activation/deactivation
- ✅ Application Layer: 14 tests for UsuarioUseCase
  - CRUD operations, validation scenarios, error handling
- ✅ Used xUnit, Moq, and FluentAssertions
- ✅ 100% pass rate

#### Integration Testing (15 pts)
- ✅ Created 18 integration tests using WebApplicationFactory
- ✅ Full HTTP request flow validation
- ✅ Tests for all CRUD endpoints
- ✅ Authentication and error handling scenarios
- ✅ Health check endpoint validation
- ✅ In-memory database for test isolation
- ✅ 100% pass rate

#### Coverage and Organization (15 pts)
- ✅ Organized test projects by layer (Unit, Integration)
- ✅ Consistent naming: `TestedMethod_Scenario_ExpectedResult`
- ✅ Custom WebApplicationFactory fixture for integration tests
- ✅ Proper test isolation with unique database instances
- ✅ Clear test structure and documentation

### Interface Web (Sprint1.Web)

A aplicação web MVC oferece:

- ✅ Interface responsiva com Bootstrap 5
- ✅ Views com validação de formulários
- ✅ ViewModels para transferência de dados entre camadas
- ✅ Rotas personalizadas e amigáveis
- ✅ Layout moderno com navegação intuitiva
- ✅ Feedback visual com mensagens de sucesso/erro
- ✅ Animações suaves em elementos interativos

#### ViewModels Implementados

- `UsuarioViewModel` - Criação de usuário
- `UsuarioListViewModel` - Listagem de usuários
- `UsuarioDetailsViewModel` - Detalhes do usuário
- `AlterarEmailViewModel` - Alteração de email
- `AlterarSenhaViewModel` - Alteração de senha (com confirmação)
- `TestConnectionViewModel` - Teste de conexão com banco

#### Rotas Personalizadas

| Rota | Descrição |
|------|-----------|
| `/usuarios/detalhes/{id}` | Visualizar detalhes do usuário |
| `/usuarios/criar` | Criar novo usuário |
| `/usuarios/{id}/alterar-email` | Alterar email do usuário |
| `/usuarios/{id}/alterar-senha` | Alterar senha do usuário |
| `/usuarios/testar-conexao` | Testar conexão com banco de dados |

### Requisitos Funcionais e Não Funcionais

#### Requisitos Funcionais:

* **RF01:** O sistema deve permitir o cadastro de novos usuários, fornecendo dados como nome, email, senha, data de nascimento e CPF.
* **RF02:** O sistema deve permitir a listagem de todos os usuários cadastrados.
* **RF03:** O sistema deve permitir a busca de um usuário específico através de seu ID.
* **RF04:** O sistema deve permitir a atualização do email de um usuário existente.
* **RF05:** O sistema deve permitir a atualização da senha de um usuário existente.
* **RF06:** O sistema deve permitir a desativação de um usuário (soft delete), mantendo o registro no banco de dados, mas marcando-o como inativo.
* **RF07:** O sistema deve disponibilizar um endpoint/página para testar a conexão com o banco de dados.

#### Requisitos Não Funcionais:
* **RNF01 (Segurança):** A comunicação com a API deve ser feita através do protocolo HTTPS.
* **RNF02 (Desempenho):** As respostas da API para as requisições devem ter um tempo médio inferior a 500ms.
* **RNF03 (Disponibilidade):** O serviço deve possuir uma disponibilidade de 99.5%.
* **RNF04 (Manutenibilidade):** O código deve seguir os princípios da Clean Architecture para garantir o desacoplamento e a facilidade de manutenção.
* **RNF05 (Confiabilidade):** A API deve implementar tratamento de erros para retornar mensagens claras e status HTTP adequados.
* **RNF06 (Usabilidade):** A interface web deve ser responsiva e intuitiva, utilizando Bootstrap para garantir compatibilidade em diferentes dispositivos.

## Endpoints da API

### Usuários
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `GET` | `/api/usuario/test-connection` | Testa conexão com banco |
| `POST` | `/api/usuario` | Criar novo usuário |
| `GET` | `/api/usuario` | Listar todos os usuários |
| `GET` | `/api/usuario/{id}` | Buscar usuário por ID |
| `PATCH` | `/api/usuario/{id}/email` | Alterar email do usuário |
| `PATCH` | `/api/usuario/{id}/senha` | Alterar senha do usuário |
| `DELETE` | `/api/usuario/{id}` | Remover usuário (soft delete) |

### Health Checks
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `GET` | `/health` | Status geral da aplicação (API + Database) |
| `GET` | `/health/ready` | Verifica se a aplicação está pronta (readiness probe) |
| `GET` | `/health/live` | Verifica se a aplicação está ativa (liveness probe) |

#### Exemplo de Resposta Health Check
```json
{
  "status": "Healthy",
  "totalDuration": "00:00:00.0123456",
  "entries": {
    "database": {
      "status": "Healthy",
      "description": "Database is accessible",
      "duration": "00:00:00.0098765"
    },
    "api": {
      "status": "Healthy",
      "description": "API is running",
      "duration": "00:00:00.0001234"
    }
  }
}
```

## Tecnologias Utilizadas

### Backend
- **ASP.NET Core 8.0** - Framework web
- **Entity Framework Core 8.0** - ORM para acesso a dados
- **Oracle Database** - Banco de dados
- **Clean Architecture** - Arquitetura do projeto

### Frontend (Interface Web)
- **Bootstrap 5** - Framework CSS
- **Bootstrap Icons** - Ícones
- **jQuery Validation** - Validação de formulários
- **Razor Pages** - Motor de views

### Monitoramento e Observabilidade
- **Serilog** - Logging estruturado com suporte a console e arquivo
- **OpenTelemetry** - Distributed tracing e métricas
- **Health Checks** - Monitoramento de saúde da aplicação
- **Health Checks UI Client** - Formatação de respostas de health checks

### Testes
- **xUnit** - Framework de testes
- **Moq** - Biblioteca para mocking
- **FluentAssertions** - Assertions legíveis e expressivas
- **WebApplicationFactory** - Testes de integração para ASP.NET Core

### Bibliotecas Adicionais
- **AutoMapper** - Mapeamento de objetos
- **FluentValidation** - Validações avançadas
- **Swashbuckle (Swagger)** - Documentação da API

## Validações Implementadas

- Email único no sistema
- CPF único no sistema
- Validação de formato de email
- Validação de CPF (11 dígitos numéricos)
- Campos obrigatórios
- Validação de senha mínima (6 caracteres)
- Soft delete (usuários são marcados como inativos)
- Validação de confirmação de senha

## Exemplo de Uso

### API - Criar Usuário
```json
POST /api/usuario
{
  "nomeCompleto": "João Silva",
  "email": "joao@email.com",
  "senha": "senha123",
  "dataNascimento": "1990-01-01",
  "cpf": "12345678901"
}
```

### API - Alterar Email
```json
PATCH /api/usuario/{id}/email
{
  "email": "novo.email@email.com"
}
```

### API - Alterar Senha
```json
PATCH /api/usuario/{id}/senha
{
  "senhaAtual": "senha123",
  "novaSenha": "novaSenha456"
}
```

## Monitoramento da Aplicação

### Logs Estruturados

A aplicação utiliza Serilog para logging estruturado com os seguintes níveis:

- **Information**: Operações normais da aplicação
- **Warning**: Situações que requerem atenção (ex: email duplicado)
- **Error**: Erros que impedem operações específicas

Os logs são salvos em:
- **Console**: Para desenvolvimento e debugging
- **Arquivo**: `logs/soslocaliza-YYYYMMDD.log` (rotação diária, retenção de 30 dias)

#### Exemplo de Log
```
[22:30:45 INF] Attempting to create user with email: joao@email.com
[22:30:45 INF] User created successfully with ID: 1
[22:30:46 WRN] Failed to create user. Email already exists: joao@email.com
```

### Distributed Tracing

OpenTelemetry rastreia requisições através das camadas da aplicação:

- **AspNetCore Instrumentation**: Rastreamento de requisições HTTP
- **EntityFrameworkCore Instrumentation**: Rastreamento de queries ao banco
- **Console Exporter**: Exportação de traces para o console

### Métricas de Performance

A aplicação expõe métricas sobre:
- Tempo de resposta das requisições
- Taxa de sucesso/erro
- Performance de queries ao banco de dados

## Testes Automatizados

### Estrutura de Testes

```
Sprint1.UnitTests/
├── Domain/
│   └── Entities/
│       └── UsuarioTests.cs (14 testes)
└── Application/
    └── UseCase/
        └── UsuarioUseCaseTests.cs (14 testes)

Sprint1.IntegrationTests/
├── Fixtures/
│   └── CustomWebApplicationFactory.cs
└── Controllers/
    └── UsuarioControllerTests.cs (18 testes)
```

### Executar Testes

```bash
# Executar todos os testes
dotnet test

# Executar apenas testes unitários
dotnet test --filter Category=Unit

# Executar apenas testes de integração
dotnet test --filter Category=Integration

# Executar testes com detalhes
dotnet test --verbosity detailed

# Executar testes com cobertura (requer coverlet)
dotnet test /p:CollectCoverage=true
```

### Cobertura de Testes

**Testes Unitários (28 testes)**:
- ✅ Domain Layer: Testes para entidade Usuario
  - Construtor e validações
  - Métodos de alteração (email, senha, nome)
  - Ativação/desativação de usuários
  
- ✅ Application Layer: Testes para UsuarioUseCase
  - Criação de usuários (sucesso e falhas)
  - Busca por ID e listagem
  - Alteração de email e senha
  - Remoção de usuários

**Testes de Integração (18 testes)**:
- ✅ Endpoints de CRUD completo
- ✅ Validações de duplicidade
- ✅ Tratamento de erros (404, 400)
- ✅ Health checks

### Padrão AAA (Arrange-Act-Assert)

Todos os testes seguem o padrão AAA:

```csharp
[Fact]
public async Task CreateUsuarioAsync_ValidData_ReturnsUsuarioResponseDto()
{
    // Arrange - Preparar dados e mocks
    var dto = new CreateUsuarioDto { /* ... */ };
    _repositoryMock.Setup(/* ... */);

    // Act - Executar a ação
    var result = await _useCase.CreateUsuarioAsync(dto);

    // Assert - Verificar resultados
    result.Should().NotBeNull();
    result.Email.Should().Be(dto.Email);
}
```

### Nomenclatura de Testes

Seguimos o padrão: `TestedMethod_Scenario_ExpectedResult`

Exemplos:
- `CreateUsuarioAsync_ValidData_ReturnsUsuarioResponseDto`
- `AlterarEmail_InvalidEmail_ThrowsArgumentException`
- `POST_CreateUsuario_DuplicateEmail_ReturnsBadRequest`

## Interface Web

A interface web oferece uma experiência completa de gerenciamento de usuários:

- **Home**: Página inicial com cards de navegação rápida
- **Lista de Usuários**: Tabela responsiva com todas as informações
- **Detalhes do Usuário**: Visualização completa dos dados
- **Criar Usuário**: Formulário com validações em tempo real
- **Alterar Email/Senha**: Formulários específicos para cada operação
- **Testar Conexão**: Verificação do status do banco de dados

## Desenvolvido por

- **Amanda Galdino** (RM560066)
- **Bruno Cantacini** (RM560242)
- **Gustavo** (RM560716)

## Challenge Oracle - FIAP

Este projeto foi desenvolvido como parte do Challenge Oracle da FIAP.
