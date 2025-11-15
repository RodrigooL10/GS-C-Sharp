# 🚀 FuturoDoTrabalho.Api - REST API em C#

API REST profissional para gerenciamento de trabalhadores com versionamento, integração MySQL e Entity Framework Core.

## 📋 Índice Rápido

1. [Início Rápido](#-início-rápido---5-minutos)
2. [Instalação Completa](#-instalação-completa)
3. [Documentação da API](#-documentação-da-api)
4. [Exemplos de Uso](#-exemplos-de-uso)
5. [Arquitetura](#-arquitetura)
6. [Perguntas Frequentes](#-perguntas-frequentes)

---

## ⚡ Início Rápido - 5 Minutos

### Pré-requisitos
- .NET 9.0+ (`dotnet --version`)
- MySQL 8.0+ (`mysql --version`)

### Passos

```bash
# 1. Criar banco de dados
mysql -u root -p
# Digite senha: admin12@

CREATE DATABASE futuro_trabalho CHARACTER SET utf8mb4;
CREATE DATABASE futuro_trabalho_dev CHARACTER SET utf8mb4;
EXIT;

# 2. Aplicar migrations
dotnet ef database update

# 3. Executar
dotnet run

# 4. Abrir Swagger
# http://localhost:5000
```

**Pronto!** 🎉 Sua API está rodando.

---

## 📥 Instalação Completa

### Windows

#### 1. Instalar .NET 9.0
```bash
# Download: https://dotnet.microsoft.com/download/dotnet/9.0
dotnet --version  # Confirmar instalação
```

#### 2. Instalar MySQL 8.0
```bash
# Download: https://dev.mysql.com/downloads/mysql/
# Executar instalador
# Configurar porta: 3306
# Root password: admin12@
```

#### 3. Verificar Instalação
```bash
# Testar conexão
mysql -u root -p
# Digite: admin12@
EXIT;
```

#### 4. Preparar Projeto
```bash
cd c:\Users\apblr\Downloads\C#\FuturoDoTrabalho.Api
dotnet restore
```

#### 5. Criar Banco de Dados
```bash
mysql -u root -p -e "CREATE DATABASE futuro_trabalho CHARACTER SET utf8mb4;"
mysql -u root -p -e "CREATE DATABASE futuro_trabalho_dev CHARACTER SET utf8mb4;"
```

#### 6. Aplicar Migrations
```bash
dotnet ef database update
```

#### 7. Executar
```bash
dotnet run --environment Development
# Acesse: http://localhost:5000
```

### macOS/Linux

```bash
# Instalar .NET
brew install dotnet

# Instalar MySQL
brew install mysql
mysql.server start

# Resto igual ao Windows
```

---

## 📊 Documentação da API

### API v1 - Endpoints Básicos

**Base URL:** `/api/v1/trabalhador`

| Método | Endpoint | Descrição | Status |
|--------|----------|-----------|--------|
| GET | `/` | Listar todos | 200 |
| GET | `/{id}` | Obter um | 200, 404 |
| POST | `/` | Criar novo | 201, 400 |
| PUT | `/{id}` | Atualizar | 200, 404, 400 |
| DELETE | `/{id}` | Deletar | 204, 404 |

**Filtros disponíveis:**
```
GET /api/v1/trabalhador?ativo=true    # Apenas ativos
GET /api/v1/trabalhador?ativo=false   # Apenas inativos
```

### API v2 - Endpoints Avançados com Paginação

**Base URL:** `/api/v2/trabalhador`

| Método | Endpoint | Descrição | Status |
|--------|----------|-----------|--------|
| GET | `/?pageNumber=1&pageSize=10` | Listar com paginação | 200 |
| GET | `/{id}` | Obter um | 200, 404 |
| POST | `/` | Criar novo | 201, 400 |
| PUT | `/{id}` | Atualizar completo | 200, 404, 400 |
| **PATCH** | `/{id}` | Atualizar parcial | 200, 404, 400 |
| DELETE | `/{id}` | Deletar | 204, 404 |

**Parâmetros de paginação:**
- `pageNumber` (padrão: 1) - Número da página
- `pageSize` (padrão: 10, máx: 100) - Itens por página
- `ativo` (opcional: true/false) - Filtrar por status

### Campos de Trabalhador

```json
{
  "id": 1,
  "nome": "João Silva",           // Obrigatório, 3-150 chars
  "cargo": "Desenvolvedor",        // Obrigatório, máx 100 chars
  "departamento": "TI",            // Opcional, máx 255 chars
  "salario": 5000.00,              // Decimal(10,2), positivo
  "dataAdmissao": "2024-01-15",    // Obrigatório
  "cpf": "12345678901",            // Opcional, único, máx 11 chars
  "telefone": "11999999999",       // Opcional, máx 20 chars
  "email": "joao@example.com",     // Opcional, válido, máx 150 chars
  "endereco": "Rua A, 123",        // Opcional, máx 500 chars
  "ativo": true,                   // Boolean (padrão: true)
  "dataCriacao": "2024-11-14T...", // Auto-gerado
  "dataAtualizacao": null          // Auto-atualizado
}
```

---

## 📝 Exemplos de Uso

### Com Swagger (Interface Gráfica) - RECOMENDADO

1. Execute a API: `dotnet run`
2. Abra navegador: `http://localhost:5000`
3. Clique em um endpoint
4. Clique em **"Try it out"**
5. Preencha os dados
6. Clique em **"Execute"**

### Com cURL

#### Criar Trabalhador
```bash
curl -X POST http://localhost:5000/api/v1/trabalhador \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "João Silva",
    "cargo": "Desenvolvedor",
    "departamento": "TI",
    "salario": 5000.00,
    "dataAdmissao": "2024-01-15",
    "cpf": "12345678901",
    "telefone": "11999999999",
    "email": "joao@example.com",
    "endereco": "Rua A, 123"
  }'
```

**Resposta (201 Created):**
```json
{
  "id": 1,
  "nome": "João Silva",
  "cargo": "Desenvolvedor",
  ...
}
```

#### Listar Todos (v1)
```bash
curl http://localhost:5000/api/v1/trabalhador
```

#### Listar com Paginação (v2)
```bash
curl "http://localhost:5000/api/v2/trabalhador?pageNumber=1&pageSize=10&ativo=true"
```

**Resposta:**
```json
{
  "data": [...],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 50,
  "totalPages": 5
}
```

#### Obter Um
```bash
curl http://localhost:5000/api/v1/trabalhador/1
```

#### Atualizar Completo (PUT)
```bash
curl -X PUT http://localhost:5000/api/v1/trabalhador/1 \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "João Silva Atualizado",
    "salario": 6000.00
  }'
```

#### Atualizar Parcial (PATCH - v2 apenas)
```bash
curl -X PATCH http://localhost:5000/api/v2/trabalhador/1 \
  -H "Content-Type: application/json" \
  -d '{
    "salario": 7000.00
  }'
```

#### Deletar
```bash
curl -X DELETE http://localhost:5000/api/v1/trabalhador/1
```

**Resposta (204 No Content)** - Sem corpo

### Com VS Code REST Client

Arquivo `requests.http` incluído com 50+ exemplos. Instale a extensão "REST Client" e clique em "Send Request".

---

## 🏗️ Arquitetura

### Padrão de Camadas

```
Controllers (v1, v2)
    ↓
Services (lógica de negócio)
    ↓
Repositories (acesso a dados)
    ↓
DbContext (Entity Framework)
    ↓
MySQL Database
```

### Estrutura de Pastas

```
FuturoDoTrabalho.Api/
├── Controllers/
│   ├── v1/TrabalhadorController.cs
│   └── v2/TrabalhadorController.cs
├── Services/
│   ├── ITrabalhadorService.cs
│   └── TrabalhadorService.cs
├── Repositories/
│   ├── ITrabalhadorRepository.cs
│   └── TrabalhadorRepository.cs
├── Models/
│   └── Trabalhador.cs
├── DTOs/
│   ├── TrabalhadorCreateDto.cs
│   ├── TrabalhadorUpdateDto.cs
│   └── TrabalhadorReadDto.cs
├── Data/
│   └── AppDbContext.cs
├── Migrations/
│   ├── 20241114000000_InitialCreate.cs
│   └── AppDbContextModelSnapshot.cs
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
└── README.md
```

### Padrões de Design

- **Repository Pattern** - Abstração de dados
- **Service Pattern** - Lógica de negócio
- **DTO Pattern** - Transferência segura de dados
- **Dependency Injection** - Loose coupling
- **API Versioning** - Múltiplas versões simultâneas

---

## 🔐 Status Codes e Respostas

### Respostas de Sucesso

| Código | Situação | Exemplo |
|--------|----------|---------|
| **200** | OK | GET, PUT bem-sucedido |
| **201** | Created | POST criou recurso |
| **204** | No Content | DELETE bem-sucedido |

### Respostas de Erro

| Código | Situação | Causa |
|--------|----------|-------|
| **400** | Bad Request | Dados inválidos, validação falhou |
| **404** | Not Found | Recurso não existe |
| **500** | Server Error | Erro interno do servidor |

### Formato de Erro

```json
{
  "message": "Descrição do erro",
  "error": "Detalhes (apenas em desenvolvimento)"
}
```

---

## 🛠️ Comandos Úteis

```bash
# Desenvolvimento
dotnet run                                    # Executar
dotnet watch run                              # Hot reload
dotnet build                                  # Compilar

# Banco de Dados
dotnet ef migrations add NomeMigracao        # Nova migration
dotnet ef database update                     # Aplicar
dotnet ef database update 0                   # Reverter todas
dotnet ef migrations remove                   # Remove última

# Produção
dotnet publish -c Release                     # Build release
dotnet publish -c Release -o ./publish        # Com output
```

---

## ❓ Perguntas Frequentes

### Como mudo a senha do MySQL?
Edite `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=futuro_trabalho;User=root;Password=NOVA_SENHA;"
}
```

### Posso usar SQL Server ao invés de MySQL?
Sim! Instale o provider:
```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

Depois altere em `Program.cs`:
```csharp
options.UseSqlServer(connectionString)
```

### Como implanto em produção?
```bash
# Build
dotnet publish -c Release

# Mude para produção em appsettings.json
# Configure HTTPS
# Adicione autenticação
# Deploy no seu servidor (Azure, AWS, etc.)
```

### Como adiciono autenticação JWT?
Instale:
```bash
dotnet add package System.IdentityModel.Tokens.Jwt
```

Adicione em `Program.cs`:
```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { /* configurar */ });

app.UseAuthentication();
```

### Qual é a senha padrão do MySQL?
A senha configurada é: `admin12@`

Você pode alterá-la com:
```sql
ALTER USER 'root'@'localhost' IDENTIFIED BY 'nova_senha';
FLUSH PRIVILEGES;
```

### Como deleto um banco e recrio?
```bash
# Reverter todas as migrations
dotnet ef database update 0

# Ou deletar manualmente
mysql -u root -p -e "DROP DATABASE futuro_trabalho;"
mysql -u root -p -e "CREATE DATABASE futuro_trabalho CHARACTER SET utf8mb4;"

# Reaplicar
dotnet ef database update
```

### Como adiciono um novo campo?
1. Edite `Models/Trabalhador.cs`:
```csharp
[StringLength(100)]
public string Novocampo { get; set; }
```

2. Crie migration:
```bash
dotnet ef migrations add AdicionarNovocamp
```

3. Aplique:
```bash
dotnet ef database update
```

### Erro: "Access denied for user 'root'"
Confirme a senha em `appsettings.json` é `admin12@` e MySQL está rodando.

### Erro: "Cannot find DbContext"
Verifique se `AppDbContext.cs` está em `Data/` e registrado em `Program.cs`.

### Swagger não aparece
Acesse `http://localhost:5000` (sem `/swagger` na URL).

### Como faço um PATCH?
PATCH está disponível apenas na **v2**:
```bash
curl -X PATCH http://localhost:5000/api/v2/trabalhador/1 \
  -H "Content-Type: application/json" \
  -d '{"salario": 8000}'
```

PATCH atualiza apenas os campos enviados (atualização parcial).
PUT atualiza o recurso inteiro.

### Posso usar Docker?
Sim! Crie um `Dockerfile`:
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY bin/Release/net9.0/publish .
ENTRYPOINT ["dotnet", "FuturoDoTrabalho.Api.dll"]
```

Build e execute:
```bash
docker build -t futuro-api .
docker run -p 5000:5000 futuro-api
```

### Como executo testes?
Crie um projeto de testes:
```bash
dotnet new xunit -n FuturoDoTrabalho.Api.Tests
dotnet add FuturoDoTrabalho.Api.Tests reference FuturoDoTrabalho.Api
dotnet test
```

---

## 📦 Tecnologias

| Tecnologia | Versão | Uso |
|-----------|--------|-----|
| .NET | 9.0 | Runtime |
| ASP.NET Core | 9.0 | Framework |
| Entity Framework Core | 9.0 | ORM |
| MySQL | 8.0+ | Banco de dados |
| Pomelo MySQL Driver | 9.0.0 | Acesso MySQL |
| Swagger/OpenAPI | 10.0.1 | Documentação |
| API Versioning | 5.1.0 | Versionamento |

---

## 🎯 Próximos Passos

### Agora
1. ✅ Execute a API
2. ✅ Teste via Swagger
3. ✅ Crie alguns registros

### Em Seguida
1. Implemente autenticação JWT
2. Adicione mais entidades
3. Crie testes unitários
4. Configure CI/CD

### Futuro
1. Deploy em produção
2. Monitoring e alertas
3. Caching (Redis)
4. Escalabilidade

---

## 📚 Recursos Externos

- [ASP.NET Core Docs](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [MySQL Documentation](https://dev.mysql.com/doc)
- [REST API Best Practices](https://restfulapi.net)
- [HTTP Status Codes](https://httpwg.org)

---

## 📝 Notas Importantes

### Segurança
- ⚠️ Altere as credenciais padrão em produção
- ⚠️ Use HTTPS em produção
- ⚠️ Implemente autenticação
- ⚠️ Use variáveis de ambiente para secrets

### Performance
- ✅ Índices criados no banco
- ✅ Paginação na v2
- ✅ Connection pooling automático
- 💡 Considere cache para futuro

### Manutenção
- ✅ Bem documentado
- ✅ Fácil de estender
- ✅ Padrões claros
- 💡 Adicione testes

---

## 📞 Suporte

1. **Swagger** - Teste interativo em `http://localhost:5000`
2. **Este README** - Consulte para referência
3. **Arquivo requests.http** - Exemplos prontos
4. **Stack Overflow** - Para dúvidas técnicas

---

## ✨ Resumo

Você tem uma **API REST profissional** que:
- ✅ Segue padrões REST
- ✅ Tem versionamento
- ✅ Integra com MySQL
- ✅ Usa Entity Framework
- ✅ Tem documentação Swagger
- ✅ É extensível e mantível

**Comece agora:**
```bash
dotnet run
# Abra http://localhost:5000
```

---

**Desenvolvido com ❤️ em C# .NET 9.0**

*Última atualização: Novembro 2024*
