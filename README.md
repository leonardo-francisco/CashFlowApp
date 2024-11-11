# CashFlowApp

**CashFlowApp** é uma aplicação para gerenciar o fluxo de caixa diário de comerciantes, registrando lançamentos de créditos e débitos e gerando um relatório consolidado de saldo diário. Desenvolvida em .NET Core e MongoDB, a aplicação segue princípios de arquitetura limpa e boas práticas para garantir escalabilidade, segurança e resiliência.

## Funcionalidades

- **Gerenciamento de Transações**: Adiciona lançamentos de crédito e débito com informações como data, valor e descrição.
- **Consolidação Diária de Saldo**: Gera um relatório consolidado do saldo para cada dia, incluindo o total de débitos e créditos.
- **API REST**: Interface para acesso e gerenciamento de transações e relatórios diários.
- **Escalabilidade e Resiliência**: Arquitetura preparada para crescimento e recuperação de falhas.

## Estrutura do Projeto
CashFlowApp/ 
  ├── CashFlowApp.Domain/ # Camada de domínio (entidades, serviços, interfaces de domínio) 
  ├── CashFlowApp.Application/ # Camada de aplicação (serviços, DTOs) 
  ├── CashFlowApp.Infrastructure/ # Camada de infraestrutura (repositórios, configuração de banco MongoDB) 
  ├── CashFlowApp.Presentation/ # Camada de apresentação (API) 
  ├── CashFlowApp.Tests/ # Testes automatizados para cada camada 
  └── README.md # Documentação e instruções de uso

## Requisitos

- **.NET 8 SDK**
- **MongoDB** (executando localmente ou usando MongoDB Atlas)
- **Redis** (executando localmente ou usando Redis Cloud)

## Configuração

### 1. Clonar o Repositório

```bash
git clone https://github.com/leonardo-francisco/CashFlowApp.git
cd CashFlowApp
```

### 2. Clonar o MongoDB
- **Localmente: Inicie o MongoDB em sua máquina**
- **Docker: Caso prefira usar Docker, você pode iniciar o MongoDB com o comando:
```bash
docker run -d -p 27017:27017 --name CashFlowApp mongo
```

### 3. Configurar Redis Cloud
- **Acessar https://cloud.redis.io/ e criar uma conta no Redis Cloud**
- **Ao criar a conta deve criar um database e nas configurações do database deve pegar a senha do database

### 4. Configuração do Arquivo appsettings.json
- **No diretório CashFlowApp.Presentation, crie um arquivo appsettings.json e configure a string de conexão do MongoDB**
```bash
 "MongoSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "CashFlowApp"
  },
  "ConnectionStrings": {
    "RedisConnection": "redis-18557.c308.sa-east-1-1.ec2.redns.redis-cloud.com:18557,password={senha_db_redis_cloud},ssl=False,abortConnect=False"
  }
```

## Executando a Aplicação

### 1. Restaurar Dependências
- **No diretório raiz do projeto, execute:
```bash
dotnet restore
```

### 2. Rodar a Aplicação
- **Para iniciar a API, execute o seguinte comando:
```bash
dotnet run --project CashFlowApp.API
```
- **A aplicação será iniciada use ferramentas como Postman ou Swagger para testar os endpoints

### 3. Testando a Aplicação
- **Para rodar os testes:
```bash
dotnet test
```
- **Isso executará os testes em CashFlowApp.Tests e exibirá os resultados no terminal

## Endpoints Principais

### Transações
- **POST /api/Transaction**: Adiciona uma nova transação (crédito ou débito).
- **GET /api/Transaction/{date}**: Recupera uma transação pela data.
