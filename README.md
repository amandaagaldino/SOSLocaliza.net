# SOSLocaliza
O SOSLocaliza é um aplicativo inovador que visa promover a segurança da população em situação de risco climático, combinando geolocalização, aprendizado de máquina e mapas interativos. O principal objetivo é fornecer uma ferramenta eficaz para que os usuários possam identificar se estão em uma área de risco, enviar alertas emergenciais de forma rápida e acessar orientações preventivas para agir em eventos extremos, como alagamentos e tempestades.

## API de Usuários
Esta aplicação gerencia o CRUD (Create, Read, Update, Delete) de usuários e foi implementada seguindo os princípios da Clean Architecture.

### Escopo

O escopo deste projeto está focado exclusivamente na criação de uma API RESTful para manipular os dados dos usuários. As funcionalidades principais incluem:

* Criação de novos usuários.
* Listagem de todos os usuários cadastrados.
* Busca de um usuário específico por seu ID.
* Atualização de informações específicas do usuário (email e senha).
* Remoção lógica de usuários (soft delete).
* Verificação de conexão com o banco de dados.

## Funcionalidades Implementadas

### CRUD Completo de Usuários
- ✅ Criar usuário
- ✅ Buscar usuário por ID
- ✅ Listar todos os usuários
- ✅ Atualizar usuário
- ✅ Alterar email específico
- ✅ Remover usuário (soft delete)

### Requisitos Funcionais e Não Funcionais

#### Requisitos Funcionais:

* **RF01:** O sistema deve permitir o cadastro de novos usuários, fornecendo dados como nome, email e senha.
* **RF02:** O sistema deve permitir a listagem de todos os usuários cadastrados.
* **RF03:** O sistema deve permitir a busca de um usuário específico através de seu ID.
* **RF04:** O sistema deve permitir a atualização do email de um usuário existente.
* **RF05:** O sistema deve permitir a atualização da senha de um usuário existente.
* **RF06:** O sistema deve permitir a desativação de um usuário (soft delete), mantendo o registro no banco de dados, mas marcando-o como inativo.
* **RF07:** O sistema deve disponibilizar um endpoint para testar a conexão com o banco de dados.

#### Requisitos Não Funcionais:
* **RNF01 (Segurança):** A comunicação com a API deve ser feita através do protocolo HTTPS.
* **RNF02 (Desempenho):** As respostas da API para as requisições devem ter um tempo médio inferior a 500ms.
* **RNF03 (Disponibilidade):** O serviço deve possuir uma disponibilidade de 99.5%.
* **RNF04 (Manutenibilidade):** O código deve seguir os princípios da Clean Architecture para garantir o desacoplamento e a facilidade de manutenção.
* **RNF05 (Confiabilidade):** A API deve implementar tratamento de erros para retornar mensagens claras e status HTTP adequados.


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
