# GD Solutions API 

A GD Solutions desenvolve soluções voltadas para modernizar a gestão de pessoas e apoiar empresas na transição para o Futuro do Trabalho, um cenário marcado por digitalização, trabalho híbrido e uso intensivo de dados para tomada de decisão.

Esta API tem como objetivo oferecer uma base estruturada para o gerenciamento de funcionários e departamentos, permitindo que sistemas corporativos realizem operações de forma organizada, segura e escalável. Ela segue boas práticas REST, utiliza versionamento de API e emprega tecnologias modernas para garantir flexibilidade na evolução do sistema.

A GD Solutions API foi projetada para ser um ponto central de integração entre diferentes aplicações internas, como ferramentas de RH, dashboards de desempenho, plataformas de People Analytics e módulos administrativos. Com uma arquitetura limpa e orientada a serviços, a API facilita a automação de processos, melhora a qualidade dos dados e apoia estratégias de transformação digital.

## O que a API entrega
* Cadastro, consulta e gerenciamento completo de Funcionários
* Administração estruturada de Departamentos
* Versionamento inteligente (v1 e v2) para evoluções futuras
* Paginação, filtros e atualizações parciais (PATCH)
* Documentação completa via Swagger/OpenAPI
* Persistência usando Entity Framework Core + MySQL
* Arquitetura robusta com inversão de dependência e separação de responsabilidades

<br>
🔹 Extensível

Permitindo crescimento com novas versões da API sem quebrar integrações existentes.

🔹 Automatizado

Com migrations, validações e controle de entidades.

🔹 Adaptável

Pensado para integrações com sistemas de IA, rotinas de análise de desempenho, gestão de habilidades, entre outros módulos corporativos.

--- 
## Início Rápido

### Pré-requisitos
- .NET 9.0+ ([Baixar](https://dotnet.microsoft.com/download/dotnet/9.0))
- MySQL 8.0+ ([Baixar](https://dev.mysql.com/downloads/mysql/))
- Git

### Instalação Completa (Desde o Zero)

#### 1. Clonar o Repositório
```bash
git clone https://github.com/RodrigooL10/GS-C-Sharp.git
cd GS-C-Sharp
```

#### 2. Acessar a Pasta da API
```bash
cd FuturoDoTrabalho.Api
```

#### 3. Restaurar Dependências
```bash
dotnet restore
```

#### 4. Criar Banco de Dados
```bash
mysql -u root -padmin12@ -e "CREATE DATABASE futuro_trabalho_dev CHARACTER SET utf8mb4;"
```

> **Nota:** Se sua senha do MySQL é diferente de `admin12@`, troque esse trecho `-padmin12` por `-p'SuaSenha'`. Troque também no arquivo `appsettings.json` na seção `ConnectionStrings` antes de continuar.

#### 5. Aplicar Migrations
```bash
dotnet ef database update
```

#### 6. Executar a API
```bash
dotnet run
```

#### 7. Acesse o Swagger UI no navegador através da URL:
```bash
http://localhost:5015
```

---

## Estrutura do Projeto

```
FuturoDoTrabalho.Api/
├── Controllers/           # Endpoints da API
│   ├── v1/
│   │   ├── FuncionarioController.cs
│   │   └── DepartamentoController.cs
│   └── v2/
│       ├── FuncionarioController.cs
│       └── DepartamentoController.cs
│
├── Services/              # Lógica de negócio
│   ├── IFuncionarioService.cs
│   ├── FuncionarioService.cs
│   ├── IDepartamentoService.cs
│   └── DepartamentoService.cs
│
├── Repositories/          # Acesso a dados
│   ├── IGenericRepository.cs
│   ├── GenericRepository.cs
│   ├── IFuncionarioRepository.cs
│   ├── FuncionarioRepository.cs
│   ├── IDepartamentoRepository.cs
│   └── DepartamentoRepository.cs
│
├── Models/                # Entidades
│   ├── Funcionario.cs
│   └── Departamento.cs
│
├── DTOs/                  # Objetos de transferência
│   ├── Funcionario/
│   │   ├── FuncionarioCreateDto.cs
│   │   ├── FuncionarioUpdateDto.cs
│   │   ├── FuncionarioPatchDto.cs
│   │   └── FuncionarioReadDto.cs
│   └── Departamento/
│       ├── DepartamentoCreateDto.cs
│       ├── DepartamentoUpdateDto.cs
│       ├── DepartamentoPatchDto.cs
│       └── DepartamentoReadDto.cs
│
├── Data/
│   └── AppDbContext.cs    # Configuração do banco
│
├── Mappings/
│   └── MappingProfile.cs  # AutoMapper
│
├── Migrations/            # Histórico do banco
│
├── Program.cs             # Configuração inicial
├── appsettings.json       # Config produção
├── appsettings.Development.json
└── README.md
```

---

## Versões da API

### v1 - Básica

**Base:** `/api/v1/funcionario` e `/api/v1/departamento`

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/` | Listar todos |
| GET | `/{id}` | Obter um |
| POST | `/` | Criar |
| PUT | `/{id}` | Atualizar completo |
| DELETE | `/{id}` | Deletar |


### v2 - Avançada

**Base:** `/api/v2/funcionario` e `/api/v2/departamento`

Inclui tudo da v1 mais:

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| **PATCH** | `/{id}` | Atualizar parcial |
| GET | `/?pageNumber=1&pageSize=10` | Paginação |

**Paginação:**
```
GET /api/v2/funcionario?pageNumber=1&pageSize=10&ativo=true
GET /api/v2/departamento?pageNumber=1&pageSize=10
```

**Resposta paginada:**
```json
{
  "data": [...],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 50,
  "totalPages": 5
}
```

---

## Exemplos de Uso

### Listar Funcionários (v1)

```bash
curl http://localhost:5015/api/v1/funcionario
```

Resposta:
```json
[
  {
    "id": 1,
    "nome": "João Silva",
    "cargo": "Desenvolvedor",
    "email": "joao@mail.com",
    "cpf": "123.456.789-00",
    "departamentoId": 1,
    "salario": 5000.00,
    "dataAdmissao": "2024-01-15",
    "ativo": true,
    "dataCriacao": "2025-11-18T10:30:00",
    "dataAtualizacao": null
  }
]
```

### Criar Funcionário

```bash
curl -X POST http://localhost:5015/api/v1/funcionario \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Maria Santos",
    "cargo": "Gerente",
    "email": "maria@mail.com",
    "cpf": "987.654.321-00",
    "departamentoId": 1,
    "salario": 8000,
    "dataAdmissao": "2024-06-10"
  }'
```

### Listar com Paginação (v2)

```bash
curl "http://localhost:5015/api/v2/funcionario?pageNumber=1&pageSize=5&ativo=true"
```

### Obter um Funcionário

```bash
curl http://localhost:5015/api/v1/funcionario/1
```

### Atualizar Salário (PATCH v2)

```bash
curl -X PATCH http://localhost:5015/api/v2/funcionario/1 \
  -H "Content-Type: application/json" \
  -d '{"salario": 9000}'
```

### Deletar

```bash
curl -X DELETE http://localhost:5015/api/v1/funcionario/1
```

---

## Swagger

Documentação interativa disponível em:
```
http://localhost:5015
```

Teste endpoints diretamente na interface.

---

## Banco de Dados
(Adapte conforme seu ambiente em appsettings.json)

**Banco:** `futuro_trabalho_dev`


### Tabelas

**funcionarios**
- id (PK)
- nome (obrigatório)
- cargo (obrigatório)
- email (único)
- cpf (único)
- telefone
- endereco
- departamentoId (FK)
- salario (decimal 10,2)
- nivelSenioridade (1-5)
- ativo (bool)
- dataCriacao (timestamp)
- dataAtualizacao (timestamp)

**departamentos**
- id (PK)
- nome (único)
- descricao
- lider (obrigatório)
- ativo (bool)
- dataCriacao (timestamp)
- dataAtualizacao (timestamp)

---

## Comandos Úteis

```bash
# Executar em desenvolvimento
dotnet run

# Compilar
dotnet build

# Criar migration
dotnet ef migrations add NomeMigracao

# Aplicar migrations
dotnet ef database update

# Reverter migrations
dotnet ef database update 0

# Remover última migration
dotnet ef migrations remove
```

---

## Arquitetura

### Tecnologias Usadas

| Tecnologia | Versão | Uso |
|-----------|--------|-----|
| .NET | 9.0 | Runtime |
| ASP.NET Core | 9.0 | Framework |
| Entity Framework Core | 9.0 | ORM |
| Pomelo.MySql | 9.0.0 | Driver MySQL |
| AutoMapper | 12.0.1 | Mapping |
| API Versioning | 5.1.0 | Versionamento |
| Swashbuckle | 6.6.2 | Swagger/OpenAPI |

### Padrões Usados

- **Repository Pattern** - Organizando o acesso ao banco de dados, deixando a API independente da forma como os dados são armazenados.
- **Service Pattern** - Centralizando as regras de negócio para manter os controllers mais limpos.
- **DTO Pattern** - Definindo exatamente o que a API recebe e devolve, evitando expor modelos internos.
- **Dependency Injection** - Permite usar serviços e repositórios sem acoplamento direto, facilitando manutenção e testes.
- **API Versioning** - Permite manter várias versões da API (ex.: v1 e v2) sem quebrar integrações já existentes.

### Fluxo de Requisição

```
Request HTTP
    ↓
Controller (v1 ou v2)
    ↓
Service (validação, lógica)
    ↓
Repository (acesso ao banco)
    ↓
DbContext (Entity Framework)
    ↓
MySQL Database
    ↓
Response JSON
```

### Status Codes

| Código | Significado |
|--------|------------|
| 200 | OK |
| 201 | Created |
| 204 | No Content |
| 400 | Bad Request |
| 404 | Not Found |
| 500 | Server Error |

---

## Integrantes
- Adriano Lopes - RM98574
- Henrique de Brito - RM98831
- Rodrigo Lima - RM98326
