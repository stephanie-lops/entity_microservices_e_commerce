# Microservices E-Commerce — Inventory, Sales e API Gateway

This project implements a microservices architecture for an e-commerce platform, with two independent services (Inventory and Sales) and a centralized API Gateway.

The architecture was built using:

- .NET 9  
- ASP.NET Web API  
- Entity Framework Core (SQLite)  
- Ocelot API Gateway  
- Communication via HttpClient
- Database per microservice (pattern: Database-per-service)

---

# Architecture

```
Microservices_E_Commerce/
│
├── ApiGateway/
│   ├── ocelot.json
│   └── Program.cs
│
├── Inventory.API/
│   ├── Controllers/
│   ├── Models/
│   ├── Data/
│   └── inventory.db
│
└── Sales.API/
    ├── Controllers/
    ├── Models/
    ├── Data/
    └── sales.db
```

Each microservice has its own database and is fully independent.
---

# Microservices

## 1. Inventory.API — Inventory Management

- Create products
- List products
- Get by ID
- Update stock
- Reserve stock for orders

### Endpoints

```
GET    /api/products
GET    /api/products/{id}
POST   /api/products
POST   /api/products/reserve
PUT    /api/products/{id}/stock
```

---

## 2. Sales.API — Sales Management

- Create orders
- List orders
- Validate stock by calling Inventory
- Persist sales data in the database

### Endpoints

```
GET    /api/orders
GET    /api/orders/{id}
POST   /api/orders
```

Sales calls Inventory to validate stock:

```
POST http://localhost:5212/api/products/reserve
```

---

# 3. API Gateway (Ocelot)

The Gateway routes:

### Inventory
```
GET/POST  /inventory/products       →  Inventory.API
```

### Sales
```
GET/POST  /sales/orders             →  Sales.API
```

The Gateway centralizes and secures access to the microservices.

---

# How to Run

### 1. Run Inventory.API
```
cd Inventory.API
dotnet run
```

### 2. Run Sales.API
```
cd Sales.API
dotnet run
```

### 3. Run API Gateway
```
cd ApiGateway
dotnet run
```

Gateway available at:
```
http://localhost:5000
```

---

# Testing via Gateway

### Create Product
```
POST http://localhost:5000/inventory/products
```
Body:
```json
{
  "name": "Monitor",
  "description": "Full HD",
  "price": 900,
  "quantity": 5
}
```

### List Products
```
GET http://localhost:5000/inventory/products
```

### Create Order
```
POST http://localhost:5000/sales/orders
```
Body:
```json
{
  "productId": 1,
  "quantity": 1
}
```

### List Orders
```
GET http://localhost:5000/sales/orders
```

---

# Database

Each service has its own SQLite database:

- `inventory.db` (inventory)
- `sales.db` (orders)

Ensuring domain isolation.

---

# Architecture Flow

1. Client sends request to Gateway
2. Gateway routes to Sales or Inventory 
3. Sales calls Inventory to reserve stock
4. Inventory confirms or rejects
5. Sales records the order
6. Gateway returns the response to the client

---

# Technologies

| Component              | Technology         |
|------------------------|--------------------|
| API Gateway            | Ocelot             |
| Microservices          | ASP.NET Web API    |
| Database               | SQLite + EF Core   |
| Internal Communication | HttpClientFactory  |
| Migrations             | EF Core Migrations |

---

# ✔ Requirements Met

- Two independent microservices
- Inventory + Sales
- Inter-service communication
- Stock update when creating orders
- Functional API Gateway
- Separate database per service
- Full CRUD in both services
- Order flow with real validation

---

# Future Enhancements

- RabbitMQ for asynchronous communication
- JWT Authentication 
- Docker Compose to run everything with a single command
- Logging and HealthCheck dashboard
- Unified Swagger via Gateway

---

# Conclusion

This project demonstrates a modern microservices architecture with a gateway, service isolation, inter-service communication, and independent persistence — all essential requirements for distributed and scalable systems.

----------------------------------------------------------
[Portuguese]

# Microservices E-Commerce — Inventory, Sales e API Gateway

Este projeto implementa uma arquitetura de **microserviços** para uma plataforma de e-commerce, com dois serviços independentes (**Estoque** e **Vendas**) e um **API Gateway** centralizando o acesso.

A arquitetura foi desenvolvida com:

- .NET 9  
- ASP.NET Web API  
- Entity Framework Core (SQLite)  
- Ocelot API Gateway  
- Comunicação via HttpClient  
- Banco por microserviço (pattern: Database-per-service)

---

# Arquitetura

```
Microservices_E_Commerce/
│
├── ApiGateway/
│   ├── ocelot.json
│   └── Program.cs
│
├── Inventory.API/
│   ├── Controllers/
│   ├── Models/
│   ├── Data/
│   └── inventory.db
│
└── Sales.API/
    ├── Controllers/
    ├── Models/
    ├── Data/
    └── sales.db
```

Cada microserviço possui seu próprio banco e é totalmente independente.

---

# Microserviços

## 1. Inventory.API — Gestão de Estoque

- Cadastrar produtos  
- Listar produtos  
- Buscar por ID  
- Atualizar estoque  
- Reservar estoque para pedidos

### Endpoints

```
GET    /api/products
GET    /api/products/{id}
POST   /api/products
POST   /api/products/reserve
PUT    /api/products/{id}/stock
```

---

## 2. Sales.API — Gestão de Vendas

- Criar pedidos  
- Listar pedidos  
- Validar estoque chamando o Inventory  
- Registrar venda no banco  

### Endpoints

```
GET    /api/orders
GET    /api/orders/{id}
POST   /api/orders
```

O Sales chama o Inventory para validar estoque:

```
POST http://localhost:5212/api/products/reserve
```

---

# 3. API Gateway (Ocelot)

O Gateway direciona:

### Inventory
```
GET/POST  /inventory/products       →  Inventory.API
```

### Sales
```
GET/POST  /sales/orders             →  Sales.API
```

O Gateway centraliza e protege o acesso aos microserviços.

---

# Como Executar

### 1. Rodar Inventory.API
```
cd Inventory.API
dotnet run
```

### 2. Rodar Sales.API
```
cd Sales.API
dotnet run
```

### 3. Rodar API Gateway
```
cd ApiGateway
dotnet run
```

Gateway disponível em:
```
http://localhost:5000
```

---

# Testes pelo Gateway

### Criar Produto
```
POST http://localhost:5000/inventory/products
```
Body:
```json
{
  "name": "Monitor",
  "description": "Full HD",
  "price": 900,
  "quantity": 5
}
```

### Listar Produtos
```
GET http://localhost:5000/inventory/products
```

### Criar Pedido
```
POST http://localhost:5000/sales/orders
```
Body:
```json
{
  "productId": 1,
  "quantity": 1
}
```

### Listar Pedidos
```
GET http://localhost:5000/sales/orders
```

---

# Banco de Dados

Cada serviço tem seu próprio SQLite:

- `inventory.db` (estoque)
- `sales.db` (pedidos)

Garantindo isolamento entre domínios.

---

# Fluxo da Arquitetura

1. Cliente faz requisição ao Gateway  
2. Gateway roteia para Sales ou Inventory  
3. Sales chama Inventory para reservar estoque  
4. Inventory confirma ou rejeita  
5. Sales registra pedido  
6. Gateway retorna a resposta ao cliente  

---

# Tecnologias

| Componente          | Tecnologia |
|---------------------|------------|
| API Gateway         | Ocelot     |
| Microserviços       | ASP.NET Web API |
| Banco               | SQLite + EF Core |
| Comunicação interna | HttpClientFactory |
| Migrações           | EF Core Migrations |

---

# ✔ Requisitos Atendidos

- Dois microserviços independentes  
- Estoque + Vendas  
- Comunicação entre microserviços  
- Atualização de estoque ao criar pedido  
- API Gateway funcional  
- Banco separado por serviço  
- CRUD completo em ambos  
- Fluxo de pedido com validação real  

---

# Próximas Extensões

- RabbitMQ para comunicação assíncrona  
- JWT Authentication  
- Docker Compose para subir tudo em 1 comando  
- Dashboard de Logs e HealthCheck  
- Swagger unificado no Gateway  

---

# Conclusão

Este projeto demonstra arquitetura moderna de microserviços com gateway, isolamento de serviços, comunicação entre APIs e persistência independente — todos requisitos essenciais para sistemas distribuídos e escaláveis.

