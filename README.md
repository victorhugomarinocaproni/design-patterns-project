# MyShop - Design Patterns Project

## Descrição
O **MyShop** é um sistema back-end em .NET 8 que simula um aplicativo de compras, desenvolvido com foco estrito em **Clean Architecture**, **Clean Code** e na implementação de 4 Design Patterns obrigatórios: **Observer**, **Strategy**, **Command** e **Proxy**.

O sistema gerencia carrinho de compras, processamento de pedidos, pagamentos e histórico, simulando um ambiente real de e-commerce.

## Tecnologias Utilizadas
- **.NET 8**
- **Entity Framework Core 8** (ORM)
- **SQLite** (Banco de dados)
- **xUnit** (Testes)
- **Swagger/OpenAPI** (Documentação da API)

## Arquitetura (Clean Architecture)
O projeto está organizado em camadas concêntricas:

1.  **MyShop.Domain**: Núcleo do sistema. Contém Entidades, Value Objects, Interfaces e Regras de Negócio. Não depende de nada.
2.  **MyShop.Application**: Casos de uso e orquestração. Depende do Domain.
3.  **MyShop.Infrastructure**: Implementação de interfaces (Repositórios, Serviços Externos). Depende do Domain e Application.
4.  **MyShop.Api**: Entrada da aplicação (Controllers). Depende de Application e Infrastructure (para DI).

## Design Patterns Implementados

### 1. Observer Pattern
*Gerenciamento de notificações do carrinho.*
- **Localização**: `src/MyShop.Domain/Entities/Cart.cs` (Subject), `src/MyShop.Infrastructure/Observers/` (Observers).
- **Implementação**: `Cart` notifica `ICartObserver` quando itens são adicionados/removidos.
- **Observers**: `CartInventoryObserver` (Reserva estoque), `CartAnalyticsObserver` (Logs de análise).

### 2. Strategy Pattern
*Seleção dinâmica de método de pagamento.*
- **Localização**: `src/MyShop.Domain/Interfaces/IPaymentStrategy.cs` (Interface), `src/MyShop.Infrastructure/Payment/Strategies/` (Concretas).
- **Implementação**: `PaymentStrategyFactory` seleciona a estratégia (Crédito, Débito, Pix, Boleto) em tempo de execução.

### 3. Command Pattern
*Encapsulamento de pedidos e histórico.*
- **Localização**: `src/MyShop.Application/Commands/` (Comandos), `src/MyShop.Application/Services/CommandInvoker.cs` (Invoker).
- **Implementação**: Operações como `CreateOrder`, `ProcessPayment`, `CancelOrder` são objetos que podem ser executados e desfeitos (Undo). Histórico persistido em `CommandLogs`.

### 4. Proxy Pattern
*Controle de acesso ao serviço de pagamento.*
- **Localização**: `src/MyShop.Infrastructure/Payment/PaymentGatewayProxy.cs`.
- **Implementação**: Intercepta chamadas ao `ExternalPaymentSimulator` para adicionar Logging, Retry Logic e Caching.

## Instruções de Execução

### Pré-requisitos
- .NET 8 SDK instalado.

### Passos
1.  **Restaurar pacotes**:
    ```bash
    dotnet restore
    ```

2.  **Executar a API**:
    ```bash
    cd MyShop.Api
    dotnet run
    ```
    O banco de dados `myshop.db` será criado automaticamente com dados de teste (Seed).

3.  **Acessar Swagger**:
    Abra o navegador em `http://localhost:5000/swagger` (ou a porta indicada no terminal).

### Testes
Para rodar os testes unitários:
```bash
dotnet test
```

## Endpoints Principais

### Cart
- `POST /api/cart/items`: Adicionar item (Header `X-User-Id` necessário).
- `GET /api/cart`: Ver carrinho.

### Order
- `POST /api/orders/checkout`: Finalizar pedido (Cria Order via Command).
- `GET /api/orders`: Histórico.
- `DELETE /api/orders/{id}`: Cancelar pedido (Undo).

### Payment
- `POST /api/payment/method`: Definir método (Strategy).
- `POST /api/payment/process`: Processar pagamento (Proxy + Strategy).
