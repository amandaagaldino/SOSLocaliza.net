# 🌍 SOSLocaliza - Sistema de Gerenciamento de Usuários para Situações de Risco Climático

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Tests](https://img.shields.io/badge/tests-46%2F46%20passing-success)](https://github.com)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

> Sistema completo de gerenciamento de usuários desenvolvido com Clean Architecture, implementando API RESTful com autenticação JWT, auditoria MongoDB e observabilidade completa.

---

## 📋 Índice

- [Visão Geral](#-visão-geral)
- [Arquitetura](#-arquitetura)
- [Funcionalidades](#-funcionalidades)
- [Tecnologias](#-tecnologias)
- [Instalação](#-instalação)
- [Endpoints da API](#-endpoints-da-api)
- [Autenticação JWT](#-autenticação-jwt)
- [Testes](#-testes)
- [Monitoramento](#-monitoramento)
- [Integrantes](#-integrantes)

---

## 🎯 Visão Geral

O **SOSLocaliza** é uma solução completa para gerenciamento de usuários em situações de risco climático, desenvolvida como parte do Challenge Oracle da FIAP. O sistema oferece:

- ✅ **API RESTful** completa com paginação, filtros, ordenação e HATEOAS
- ✅ **Autenticação JWT** com autorização baseada em roles
- ✅ **Auditoria MongoDB** para logs de login
- ✅ **Observabilidade** com health checks, logging estruturado e tracing
- ✅ **Testes automatizados** com 100% de aprovação (46 testes)
- ✅ **Clean Architecture** com separação clara de responsabilidades
- ✅ **Interface Web MVC** responsiva com Bootstrap 5

### 📊 Status do Projeto

```
✅ Unit Tests:        28/28 passed (100%)
✅ Integration Tests: 18/18 passed (100%)
✅ Total Coverage:    Domain ~95%, Application ~90%
✅ Build Status:      Success (0 errors)
```

---

## 🏗️ Arquitetura

### Diagrama de Arquitetura da Solução

```
┌─────────────────────────────────────────────────────────────────────────┐
│                          PRESENTATION LAYER                              │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                           │
│  ┌──────────────────┐              ┌──────────────────────────────┐    │
│  │   Swagger UI     │              │      MVC Web App             │    │
│  │  (API Docs)      │              │   (Sprint1.Web)              │    │
│  └────────┬─────────┘              └──────────┬───────────────────┘    │
│           │                                    │                         │
│           └────────────────┬───────────────────┘                         │
│                            │                                             │
│                   ┌────────▼─────────┐                                  │
│                   │  API Controllers  │                                  │
│                   │  - UsuarioController                                 │
│                   │  - AuthController                                    │
│                   └────────┬─────────┘                                  │
│                            │                                             │
└────────────────────────────┼─────────────────────────────────────────────┘
                             │
┌────────────────────────────┼─────────────────────────────────────────────┐
│                   APPLICATION LAYER                                      │
├────────────────────────────┼─────────────────────────────────────────────┤
│                            │                                             │
│                   ┌────────▼─────────┐                                  │
│                   │    Use Cases      │                                  │
│                   │  - UsuarioUseCase │                                  │
│                   │  - AuthUseCase    │                                  │
│                   └────────┬─────────┘                                  │
│                            │                                             │
│                   ┌────────▼─────────┐                                  │
│                   │    Services       │                                  │
│                   │  - TokenService   │                                  │
│                   │  - HateoasHelper  │                                  │
│                   └────────┬─────────┘                                  │
│                            │                                             │
│                   ┌────────▼─────────┐                                  │
│                   │      DTOs         │                                  │
│                   │  - Request DTOs   │                                  │
│                   │  - Response DTOs  │                                  │
│                   │  - Resource DTOs  │                                  │
│                   └───────────────────┘                                  │
│                                                                           │
└────────────────────────────┼─────────────────────────────────────────────┘
                             │
┌────────────────────────────┼─────────────────────────────────────────────┐
│                      DOMAIN LAYER                                        │
├────────────────────────────┼─────────────────────────────────────────────┤
│                            │                                             │
│                   ┌────────▼─────────┐                                  │
│                   │    Entities       │                                  │
│                   │  - Usuario        │                                  │
│                   │  - LoginAudit     │                                  │
│                   └────────┬─────────┘                                  │
│                            │                                             │
│                   ┌────────▼─────────┐                                  │
│                   │   Exceptions      │                                  │
│                   │  - UsuarioNotFoundException                          │
│                   │  - EmailDuplicadoException                           │
│                   │  - CpfDuplicadoException                             │
│                   └───────────────────┘                                  │
│                                                                           │
└────────────────────────────┼─────────────────────────────────────────────┘
                             │
┌────────────────────────────┼─────────────────────────────────────────────┐
│                  INFRASTRUCTURE LAYER                                    │
├────────────────────────────┼─────────────────────────────────────────────┤
│                            │                                             │
│                   ┌────────▼─────────┐                                  │
│                   │   Repositories    │                                  │
│                   │  - UsuarioRepository                                 │
│                   │  - LoginAuditRepository                              │
│                   └────────┬─────────┘                                  │
│                            │                                             │
│              ┌─────────────┴─────────────┐                              │
│              │                           │                              │
│     ┌────────▼─────────┐       ┌────────▼─────────┐                   │
│     │  Oracle Database  │       │  MongoDB Atlas    │                   │
│     │  (Relational)     │       │  (NoSQL)          │                   │
│     │                   │       │                   │                   │
│     │  - Usuarios       │       │  - LoginAudits    │                   │
│     │  - Migrations     │       │  - Audit Logs     │                   │
│     └───────────────────┘       └───────────────────┘                   │
│                                                                           │
└───────────────────────────────────────────────────────────────────────────┘

┌───────────────────────────────────────────────────────────────────────────┐
│                      CROSS-CUTTING CONCERNS                               │
├───────────────────────────────────────────────────────────────────────────┤
│                                                                           │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  ┌─────────────┐ │
│  │   Logging    │  │   Tracing    │  │ Health Checks│  │  Exception  │ │
│  │  (Serilog)   │  │(OpenTelemetry│  │              │  │  Handling   │ │
│  └──────────────┘  └──────────────┘  └──────────────┘  └─────────────┘ │
│                                                                           │
└───────────────────────────────────────────────────────────────────────────┘
```

### Princípios da Clean Architecture

1. **Independência de Frameworks**: O domínio não depende de frameworks externos
2. **Testabilidade**: Todas as camadas são testáveis independentemente
3. **Independência de UI**: A lógica de negócio não conhece a UI
4. **Independência de Banco de Dados**: Uso de abstrações (repositories)
5. **Regra de Dependência**: Dependências apontam sempre para dentro

### Estrutura de Pastas

```
SOSLocaliza.net/
├── Sprint1.API/                    # API RESTful
│   ├── Controllers/                # Endpoints HTTP
│   ├── Application/                # Casos de uso e DTOs
│   │   ├── DTOs/                   # Data Transfer Objects
│   │   ├── UseCase/                # Lógica de aplicação
│   │   └── Services/               # Serviços (JWT, etc)
│   ├── Domain/                     # Entidades e regras de negócio
│   │   ├── Entities/               # Entidades do domínio
│   │   └── Exceptions/             # Exceções customizadas
│   ├── Infrastructure/             # Acesso a dados
│   │   ├── Repositories/           # Implementação de repositórios
│   │   ├── Mappings/               # Configuração EF Core
│   │   └── Migrations/             # Migrações do banco
│   ├── Middleware/                 # Middlewares customizados
│   └── Utils/                      # Utilitários (Swagger, HATEOAS)
│
├── Sprint1.Web/                    # Interface Web MVC
│   ├── Controllers/                # Controllers MVC
│   ├── Views/                      # Views Razor
│   ├── Models/                     # ViewModels
│   └── wwwroot/                    # Arquivos estáticos
│
├── Sprint1.UnitTests/              # Testes unitários (28 testes)
│   ├── Domain/                     # Testes de entidades
│   └── Application/                # Testes de use cases
│
└── Sprint1.IntegrationTests/       # Testes de integração (18 testes)
    ├── Controllers/                # Testes de endpoints
    └── Fixtures/                   # Configuração de testes
```

---

## ✨ Funcionalidades

### 🔐 Autenticação e Autorização
- ✅ Login com JWT (JSON Web Tokens)
- ✅ Autorização baseada em roles (User, Admin)
- ✅ Tokens com expiração configurável (60 minutos)
- ✅ Auditoria completa de tentativas de login no MongoDB

### 👥 Gerenciamento de Usuários
- ✅ CRUD completo de usuários
- ✅ Paginação com metadados (page, pageSize, totalCount, totalPages)
- ✅ Ordenação por múltiplos campos (nome, email, cpf, dataNascimento)
- ✅ Filtros avançados (nome, email, cpf, status ativo)
- ✅ HATEOAS com links de navegação
- ✅ Soft delete (inativação lógica)
- ✅ Validações robustas (email único, CPF único)

### 📊 Monitoramento e Observabilidade
- ✅ Health Checks (3 endpoints: /health, /health/ready, /health/live)
- ✅ Logging estruturado com Serilog (Console + File)
- ✅ Distributed Tracing com OpenTelemetry
- ✅ Métricas de performance
- ✅ Auditoria de login no MongoDB

### 🧪 Qualidade de Código
- ✅ 46 testes automatizados (100% aprovação)
- ✅ Padrão AAA (Arrange-Act-Assert)
- ✅ Cobertura: Domain ~95%, Application ~90%
- ✅ Testes unitários e de integração
- ✅ Clean Architecture

---

## 🛠️ Tecnologias

### Backend
| Tecnologia | Versão | Descrição |
|------------|--------|-----------|
| .NET | 8.0 | Framework principal |
| ASP.NET Core | 8.0 | Framework web |
| Entity Framework Core | 8.0 | ORM para Oracle |
| MongoDB.Driver | 2.28.0 | Driver para MongoDB |
| Serilog | 8.0.0 | Logging estruturado |
| OpenTelemetry | 1.9.0 | Observabilidade |
| xUnit | 2.5.3 | Framework de testes |
| FluentAssertions | 6.12.0 | Assertions expressivas |
| Moq | 4.20.70 | Mocking para testes |
| Swashbuckle | 6.5.0 | Documentação Swagger |
| AutoMapper | 12.0.1 | Mapeamento de objetos |
| FluentValidation | 11.9.0 | Validações |

### Bancos de Dados
- **Oracle Database**: Dados relacionais (usuários)
- **MongoDB Atlas**: Auditoria de login (NoSQL)

### Frontend (Interface Web)
- **Bootstrap 5**: Framework CSS
- **jQuery**: Manipulação DOM
- **Razor Pages**: Motor de views

---

## 📦 Instalação

### Pré-requisitos

- [.NET SDK 8.0](https://dotnet.microsoft.com/download)
- [Oracle Database](https://www.oracle.com/database/) ou acesso a servidor Oracle
- [MongoDB](https://www.mongodb.com/) (local ou Atlas)
- IDE: [Visual Studio](https://visualstudio.microsoft.com/) ou [Rider](https://www.jetbrains.com/rider/)

### Passo 1: Clonar o Repositório

```bash
git clone https://github.com/seu-usuario/soslocaliza.git
cd soslocaliza
```

### Passo 2: Configurar Banco de Dados Oracle

Edite `Sprint1.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "OracleDb": "User Id=seu_usuario;Password=sua_senha;Data Source=servidor:porta/servico;"
  }
}
```

### Passo 3: Configurar MongoDB

**Opção A: MongoDB Local (Docker)**
```bash
docker run -d -p 27017:27017 --name mongodb mongo:latest
```

**Opção B: MongoDB Atlas (Cloud)**
1. Crie uma conta em [MongoDB Atlas](https://www.mongodb.com/cloud/atlas)
2. Crie um cluster gratuito
3. Configure acesso de rede (IP Whitelist)
4. Obtenha a connection string

Edite `Sprint1.API/appsettings.json`:

```json
{
  "MongoDB": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "SOSLocalizaDB",
    "LoginAuditsCollection": "LoginAudits"
  }
}
```

### Passo 4: Aplicar Migrations

```bash
cd Sprint1.API
dotnet ef database update
```

### Passo 5: Executar a Aplicação

**API:**
```bash
cd Sprint1.API
dotnet run
```

Acesse: `http://localhost:5132` (Swagger UI)

**Interface Web:**
```bash
cd Sprint1.Web
dotnet run
```

Acesse: `http://localhost:5000`

### Passo 6: Executar Testes

```bash
# Todos os testes
dotnet test

# Apenas testes unitários
dotnet test Sprint1.UnitTests/

# Apenas testes de integração
dotnet test Sprint1.IntegrationTests/

# Com detalhes
dotnet test --verbosity detailed
```

---

## 🌐 Endpoints da API

### Documentação Interativa

Acesse a documentação Swagger em: `http://localhost:5132`

### Autenticação

#### POST /api/auth/login
Realizar login e obter token JWT.

**Request:**
```json
{
  "email": "usuario@email.com",
  "senha": "senha123"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2024-01-15T11:30:00Z",
  "tokenType": "Bearer",
  "userId": 1,
  "email": "usuario@email.com",
  "role": "User"
}
```

**Erros:**
- `400 Bad Request`: Credenciais inválidas
- `404 Not Found`: Usuário não encontrado

---

### Usuários

#### POST /api/usuario
Criar novo usuário (público, não requer autenticação).

**Request:**
```json
{
  "nomeCompleto": "João Silva",
  "email": "joao@email.com",
  "senha": "senha123",
  "dataNascimento": "1990-01-01",
  "cpf": "12345678901"
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "nomeCompleto": "João Silva",
  "email": "joao@email.com",
  "cpf": "12345678901",
  "dataNascimento": "1990-01-01",
  "ativo": true,
  "role": "User"
}
```

**Erros:**
- `400 Bad Request`: Email ou CPF duplicado, validação falhou

---

#### GET /api/usuario
Listar usuários com paginação, filtros e ordenação (requer autenticação).

**Query Parameters:**
- `page` (int): Número da página (padrão: 1)
- `pageSize` (int): Itens por página (padrão: 10, máx: 100)
- `sortBy` (string): Campo para ordenar (nome, email, cpf, dataNascimento)
- `sortOrder` (string): Direção (asc, desc)
- `nome` (string): Filtrar por nome (busca parcial)
- `email` (string): Filtrar por email (busca parcial)
- `cpf` (string): Filtrar por CPF (busca exata)
- `ativo` (bool): Filtrar por status

**Exemplo:**
```bash
GET /api/usuario?page=1&pageSize=10&sortBy=nome&sortOrder=asc&ativo=true
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
  "items": [
    {
      "id": 1,
      "nomeCompleto": "João Silva",
      "email": "joao@email.com",
      "cpf": "12345678901",
      "dataNascimento": "1990-01-01",
      "ativo": true,
      "role": "User",
      "links": [
        {
          "href": "/api/usuario/1",
          "rel": "self",
          "method": "GET"
        },
        {
          "href": "/api/usuario/1/email",
          "rel": "update-email",
          "method": "PATCH"
        },
        {
          "href": "/api/usuario/1/senha",
          "rel": "update-password",
          "method": "PATCH"
        },
        {
          "href": "/api/usuario/1",
          "rel": "delete",
          "method": "DELETE"
        }
      ]
    }
  ],
  "page": 1,
  "pageSize": 10,
  "totalCount": 25,
  "totalPages": 3,
  "hasPrevious": false,
  "hasNext": true
}
```

**Headers de Resposta:**
```
X-Pagination-Links: {
  "first": "/api/usuario?page=1&pageSize=10",
  "next": "/api/usuario?page=2&pageSize=10",
  "last": "/api/usuario?page=3&pageSize=10"
}
```

---

#### GET /api/usuario/{id}
Buscar usuário por ID com HATEOAS (requer autenticação).

**Response (200 OK):**
```json
{
  "id": 1,
  "nomeCompleto": "João Silva",
  "email": "joao@email.com",
  "cpf": "12345678901",
  "dataNascimento": "1990-01-01",
  "ativo": true,
  "role": "User",
  "links": [
    {
      "href": "/api/usuario/1",
      "rel": "self",
      "method": "GET"
    },
    {
      "href": "/api/usuario/1/email",
      "rel": "update-email",
      "method": "PATCH"
    },
    {
      "href": "/api/usuario/1/senha",
      "rel": "update-password",
      "method": "PATCH"
    },
    {
      "href": "/api/usuario/1",
      "rel": "delete",
      "method": "DELETE"
    },
    {
      "href": "/api/usuario",
      "rel": "all-users",
      "method": "GET"
    }
  ]
}
```

**Erros:**
- `404 Not Found`: Usuário não encontrado

---

#### PATCH /api/usuario/{id}/email
Alterar email do usuário (requer autenticação).

**Request:**
```json
{
  "email": "novo.email@email.com"
}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "nomeCompleto": "João Silva",
  "email": "novo.email@email.com",
  "cpf": "12345678901",
  "dataNascimento": "1990-01-01",
  "ativo": true,
  "role": "User"
}
```

**Erros:**
- `400 Bad Request`: Email já em uso
- `404 Not Found`: Usuário não encontrado

---

#### PATCH /api/usuario/{id}/senha
Alterar senha do usuário (requer autenticação).

**Request:**
```json
{
  "senhaAtual": "senha123",
  "novaSenha": "novaSenha456"
}
```

**Response (200 OK):**
```json
{
  "message": "Senha alterada com sucesso"
}
```

**Erros:**
- `400 Bad Request`: Senha atual incorreta
- `404 Not Found`: Usuário não encontrado

---

#### DELETE /api/usuario/{id}
Remover usuário - soft delete (requer autenticação Admin).

**Response (204 No Content)**

**Erros:**
- `401 Unauthorized`: Token inválido ou ausente
- `403 Forbidden`: Usuário não tem permissão (não é Admin)
- `404 Not Found`: Usuário não encontrado

---

### Health Checks

#### GET /health
Status geral da aplicação (API + Database).

**Response (200 OK):**
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

#### GET /health/ready
Readiness probe - verifica se a aplicação está pronta para receber tráfego.

#### GET /health/live
Liveness probe - verifica se a aplicação está viva.

---

## 🔐 Autenticação JWT

### Fluxo de Autenticação

```
┌─────────┐                ┌─────────┐                ┌──────────┐
│ Cliente │                │   API   │                │  Oracle  │
└────┬────┘                └────┬────┘                └────┬─────┘
     │                          │                          │
     │  POST /api/auth/login    │                          │
     │ ────────────────────────>│                          │
     │  {email, senha}          │                          │
     │                          │  Verificar credenciais   │
     │                          │ ────────────────────────>│
     │                          │                          │
     │                          │  Usuário válido          │
     │                          │ <────────────────────────│
     │                          │                          │
     │                          │  Gerar JWT Token         │
     │                          │ ─────────┐               │
     │                          │          │               │
     │                          │ <────────┘               │
     │                          │                          │
     │  200 OK + JWT Token      │                          │
     │ <────────────────────────│                          │
     │  {token, expiration}     │                          │
     │                          │                          │
     │  GET /api/usuario        │                          │
     │  Authorization: Bearer   │                          │
     │ ────────────────────────>│                          │
     │                          │  Validar Token           │
     │                          │ ─────────┐               │
     │                          │          │               │
     │                          │ <────────┘               │
     │                          │                          │
     │                          │  Buscar dados            │
     │                          │ ────────────────────────>│
     │                          │                          │
     │                          │  Retornar dados          │
     │                          │ <────────────────────────│
     │                          │                          │
     │  200 OK + Dados          │                          │
     │ <────────────────────────│                          │
     │                          │                          │
```

### Configuração JWT

**appsettings.json:**
```json
{
  "JwtSettings": {
    "SecretKey": "sua-chave-secreta-minimo-32-caracteres",
    "Issuer": "SOSLocaliza.API",
    "Audience": "SOSLocaliza.Client",
    "ExpirationMinutes": "60"
  }
}
```

### Roles e Permissões

| Role | Permissões |
|------|------------|
| **User** | Criar conta, visualizar usuários, alterar próprio email/senha |
| **Admin** | Todas as permissões de User + deletar usuários |

### Exemplo de Uso

```bash
# 1. Fazer login
TOKEN=$(curl -s -X POST http://localhost:5132/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email": "usuario@email.com", "senha": "senha123"}' \
  | jq -r '.token')

# 2. Usar token em requisições
curl -X GET http://localhost:5132/api/usuario \
  -H "Authorization: Bearer $TOKEN"

# 3. Swagger UI
# Clique em "Authorize" e insira: Bearer {seu-token}
```

---

## 🧪 Testes

### Estrutura de Testes

```
Sprint1.UnitTests/ (28 testes)
├── Domain/
│   └── Entities/
│       └── UsuarioTests.cs
│           ├── Constructor_ValidData_CreatesUsuario
│           ├── Constructor_NullNomeCompleto_ThrowsArgumentException
│           ├── AlterarEmail_ValidEmail_UpdatesEmail
│           ├── AlterarSenha_ValidPassword_UpdatesPassword
│           ├── VerificarSenha_CorrectPassword_ReturnsTrue
│           └── ... (10 testes)
│
└── Application/
    └── UseCase/
        └── UsuarioUseCaseTests.cs
            ├── CreateUsuarioAsync_ValidData_ReturnsUsuarioResponseDto
            ├── CreateUsuarioAsync_DuplicateEmail_ThrowsEmailDuplicadoException
            ├── GetUsuarioByIdAsync_ExistingId_ReturnsUsuarioResponseDto
            ├── AlterarEmailUsuarioAsync_ValidData_UpdatesEmail
            ├── GetUsuariosPagedAsync_ReturnsPagedResult
            └── ... (18 testes)

Sprint1.IntegrationTests/ (18 testes)
└── Controllers/
    └── UsuarioControllerTests.cs
        ├── POST_CreateUsuario_ReturnsCreated
        ├── POST_CreateUsuario_DuplicateEmail_ReturnsBadRequest
        ├── GET_GetUsuarioById_ReturnsOk
        ├── GET_GetAllUsuarios_ReturnsOkWithList
        ├── PATCH_AlterarEmail_ReturnsOk
        ├── DELETE_DeleteUsuario_ReturnsNoContent
        ├── GET_Health_ReturnsHealthy
        └── ... (18 testes)
```

### Padrão AAA (Arrange-Act-Assert)

Todos os testes seguem o padrão AAA para clareza e manutenibilidade:

```csharp
[Fact]
public async Task CreateUsuarioAsync_ValidData_ReturnsUsuarioResponseDto()
{
    // Arrange - Preparar dados e mocks
    var dto = new CreateUsuarioDto
    {
        NomeCompleto = "João Silva",
        Email = "joao@email.com",
        Senha = "senha123",
        DataNascimento = new DateTime(1990, 1, 1),
        Cpf = "12345678901"
    };
    
    _repositoryMock
        .Setup(r => r.EmailExistsAsync(dto.Email))
        .ReturnsAsync(false);

    // Act - Executar a ação
    var result = await _useCase.CreateUsuarioAsync(dto);

    // Assert - Verificar resultados
    result.Should().NotBeNull();
    result.Email.Should().Be(dto.Email);
    result.NomeCompleto.Should().Be(dto.NomeCompleto);
    result.Ativo.Should().BeTrue();
}
```

### Executar Testes

```bash
# Todos os testes
dotnet test

# Com detalhes
dotnet test --verbosity detailed

# Apenas unitários
dotnet test Sprint1.UnitTests/

# Apenas integração
dotnet test Sprint1.IntegrationTests/

# Com cobertura (requer coverlet)
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### Resultados Esperados

```
✅ Unit Tests: 28/28 passed (100%)
   - Domain Layer: 10 tests
   - Application Layer: 18 tests

✅ Integration Tests: 18/18 passed (100%)
   - CRUD Operations: 12 tests
   - Health Checks: 3 tests
   - Error Handling: 3 tests

✅ Total: 46/46 tests passed
⏱️ Duration: ~1.5 seconds
```

---

## 📊 Monitoramento

### Health Checks

A aplicação expõe 3 endpoints de health check:

| Endpoint | Descrição | Uso |
|----------|-----------|-----|
| `/health` | Status geral (API + DB) | Monitoramento geral |
| `/health/ready` | Readiness probe | Kubernetes readiness |
| `/health/live` | Liveness probe | Kubernetes liveness |

**Exemplo de uso com Kubernetes:**
```yaml
livenessProbe:
  httpGet:
    path: /health/live
    port: 5132
  initialDelaySeconds: 30
  periodSeconds: 10

readinessProbe:
  httpGet:
    path: /health/ready
    port: 5132
  initialDelaySeconds: 5
  periodSeconds: 5
```

### Logging Estruturado

**Serilog** configurado com:
- **Console Sink**: Logs em tempo real
- **File Sink**: Logs persistentes em `logs/soslocaliza-{Date}.log`
- **Rotação**: Diária
- **Retenção**: 30 dias

**Níveis de Log:**
- `Information`: Operações normais
- `Warning`: Situações que requerem atenção
- `Error`: Erros que impedem operações

**Exemplo de logs:**
```
[22:30:45 INF] Attempting to create user with email: joao@email.com
[22:30:45 INF] User created successfully with ID: 1
[22:30:46 WRN] Failed to create user. Email already exists: joao@email.com
[22:30:47 ERR] An unhandled exception occurred: Database connection failed
```

### Distributed Tracing

**OpenTelemetry** rastreia:
- Requisições HTTP (AspNetCore Instrumentation)
- Queries ao banco de dados (EF Core Instrumentation)
- Tempo de resposta
- Erros e exceções

**Exemplo de trace:**
```
Activity.TraceId: fae6d35fb5425313c3fb2fe4eba548f5
Activity.SpanId: f2667900fe2bc3c5
Activity.DisplayName: POST api/Usuario
Activity.Duration: 00:00:00.0105220
Activity.Tags:
    http.request.method: POST
    http.response.status_code: 201
    url.path: /api/usuario
```

### Auditoria MongoDB

Todas as tentativas de login são registradas no MongoDB:

```json
{
  "_id": "ObjectId",
  "email": "usuario@email.com",
  "ipAddress": "192.168.1.100",
  "success": true,
  "timestamp": "2024-01-15T10:30:00Z",
  "failureReason": null,
  "userId": 1,
  "userAgent": "Mozilla/5.0..."
}
```

**Consultas úteis:**
```javascript
// Últimas tentativas de login
db.LoginAudits.find().sort({timestamp: -1}).limit(10)

// Tentativas falhadas
db.LoginAudits.find({success: false})

// Logins de um usuário específico
db.LoginAudits.find({email: "usuario@email.com"})
```

---

## 👥 Integrantes

| Nome | RM | Email |
|------|-----|-------|
| **Amanda Galdino** | RM560066 | RM560066@fiap.com.br |
| **Bruno Cantacini** | RM560242 | RM560242@fiap.com.br |
| **Gustavo Gonçalves** | RM556823 | RM556823@fiap.com.br |

