# SOSLocaliza

O SOSLocaliza é um projeto que visa promover a segurança da população em situação de risco climático. A aplicação desenvolvida em C# gerencia, como foco principal, o CRUD (Create, Read, Update, Delete) de usuários.

## Sobre o Projeto

Este projeto é composto por duas aplicações principais:

1. **Sprint1.API**: API RESTful para gerenciamento de usuários
2. **Sprint1.Web**: Interface web MVC para gerenciamento de usuários

## Instalação e Configuração

### Pré-requisitos

- .NET SDK 8.0 ou superior
- Oracle Database ou acesso a um servidor Oracle
- Visual Studio ou Rider

### Configuração

1. **Configurar Connection String**

   Atualize a connection string nos arquivos `appsettings.json` de ambos os projetos:
   
   ```json
   {
     "ConnectionStrings": {
       "OracleDb": "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=HOST:PORTA/SERVICO;"
     }
   }
   ```

2. **Executar Migrations**

   ```bash
   cd Sprint1.API
   dotnet ef database update
   ```

3. **Executar a Aplicação**

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

### Arquitetura

- **Domain Layer**: Entidades e interfaces de repositório
- **Infrastructure Layer**: Implementação do repositório com Entity Framework
- **Application Layer**: Use Cases para cada operação
- **Presentation Layer**: 
  - Controllers com DTOs (API)
  - Controllers com ViewModels (Web)
  - Views Razor com validações

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
- **Madjer Finamor** (RM560716)

## Challenge Oracle - FIAP

Este projeto foi desenvolvido como parte do Challenge Oracle da FIAP.
