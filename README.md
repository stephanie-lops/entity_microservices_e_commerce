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

# 🧱 Arquitetura

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

# 🚀 Microserviços

## 1️⃣ Inventory.API — Gestão de Estoque

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

## 2️⃣ Sales.API — Gestão de Vendas

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

# 🧩 API Gateway (Ocelot)

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

# ⚙️ Como Executar

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

# Próximas Extensões (Não Obrigatórias)

- RabbitMQ para comunicação assíncrona  
- JWT Authentication  
- Docker Compose para subir tudo em 1 comando  
- Dashboard de Logs e HealthCheck  
- Swagger unificado no Gateway  

---

# Conclusão

Este projeto demonstra arquitetura moderna de microserviços com gateway, isolamento de serviços, comunicação entre APIs e persistência independente — todos requisitos essenciais para sistemas distribuídos e escaláveis.

