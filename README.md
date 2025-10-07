# SOSLocaliza

### Objetivo do Projeto

O SOSLocaliza é um aplicativo inovador que visa promover a segurança da população em situação de risco climático, combinando geolocalização, aprendizado de máquina e mapas interativos. O principal objetivo é fornecer uma ferramenta eficaz para que os usuários possam identificar se estão em uma área de risco, enviar alertas emergenciais de forma rápida e acessar orientações preventivas para agir em eventos extremos, como alagamentos e tempestades.

## API de Usuários
Esta aplicação gerencia o CRUD (Create, Read, Update, Delete) de usuários e foi implementada seguindo os princípios da Clean Architecture.

## Funcionalidades Implementadas

### CRUD Completo de Usuários
- ✅ Criar usuário
- ✅ Buscar usuário por ID
- ✅ Listar todos os usuários
- ✅ Atualizar usuário
- ✅ Alterar email específico
- ✅ Remover usuário (soft delete)

### Arquitetura
- **Domain Layer**: Entidades e interfaces de repositório
- **Infrastructure Layer**: Implementação do repositório com Entity Framework
- **Application Layer**: Use Cases para cada operação
- **Presentation Layer**: Controllers com DTOs

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

## Exemplo de Uso

### Criar Usuário
```json
POST /api/usuario
{
  "nomeCompleto": "João Silva",
  "email": "joao@email.com",
  "dataNascimento": "1990-01-01",
  "cpf": "12345678901"
}
```

### Atualizar Usuário
```json
PUT /api/usuario/{id}
{
  "nomeCompleto": "João Silva Santos",
  "email": "joao.santos@email.com",
  "dataNascimento": "1990-01-01",
  "cpf": "12345678901"
}
```

## Validações Implementadas
- Email único
- CPF único
- Validação de formato de email
- Validação de CPF (11 dígitos)
- Campos obrigatórios
- Soft delete (usuários são marcados como inativos)

### Configurar Credenciais do Banco de Dados

⚠️ **IMPORTANTE**: As credenciais do banco de dados **NÃO** estão incluídas no repositório por motivos de segurança.

Crie o arquivo `Sprint1.API/appsettings.Development.json` com o seguinte conteúdo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(DESCRIPTION=(RETRY_COUNT=20)(RETRY_DELAY=3)(ADDRESS=(PROTOCOL=TCPS)(PORT=1522)(HOST=seu-host.oraclecloud.com))(CONNECT_DATA=(SERVICE_NAME=seu_service_name.oraclecloud.com))(SECURITY=(SSL_SERVER_DN_MATCH=yes)));User Id=SEU_USUARIO;Password=SUA_SENHA;Wallet_Location=/caminho/para/seu/Wallet"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

**Substitua:**
- `seu-host.oraclecloud.com` - Host do seu Oracle Cloud
- `seu_service_name.oraclecloud.com` - Nome do serviço do banco
- `SEU_USUARIO` - Usuário do banco de dados
- `SUA_SENHA` - Senha do banco de dados
- `/caminho/para/seu/Wallet` - Caminho completo para a pasta da Oracle Wallet

### 3. Configurar Oracle Wallet

A Oracle Wallet contém os certificados necessários para conexão segura:

1. Acesse o [Oracle Cloud Console](https://cloud.oracle.com/)
2. Navegue até seu Autonomous Database
3. Clique em **"DB Connection"**
4. Baixe a Wallet (arquivo .zip)
5. Extraia em um local seguro (ex: `~/Documents/Wallet_NomeDoSeuBanco`)
6. Use esse caminho no `appsettings.Development.json`

