# Resumo Técnico: Design Patterns no MyShop

## Seção 1: Estudo Teórico dos Padrões

### 1. Observer Pattern
- **Definição**: Define uma dependência um-para-muitos entre objetos, de modo que quando um objeto muda de estado, todos os seus dependentes são notificados e atualizados automaticamente.
- **Propósito**: Permitir que objetos reajam a eventos em outros objetos sem acoplamento rígido.
- **Estrutura**:
    - **Subject (Sujeito)**: Mantém lista de observers e notifica. (`Cart`)
    - **Observer**: Interface para receber atualizações. (`ICartObserver`)
    - **ConcreteObserver**: Implementação da reação. (`CartInventoryObserver`)
- **Quando Usar**: Quando uma mudança em um objeto requer mudanças em outros, e você não sabe quantos objetos precisam mudar.

### 2. Strategy Pattern
- **Definição**: Define uma família de algoritmos, encapsula cada um deles e os torna intercambiáveis.
- **Propósito**: Permitir que o algoritmo varie independentemente dos clientes que o utilizam.
- **Estrutura**:
    - **Context**: Mantém referência para uma Strategy.
    - **Strategy**: Interface comum. (`IPaymentStrategy`)
    - **ConcreteStrategy**: Implementação do algoritmo. (`PixPaymentStrategy`)
- **Quando Usar**: Quando você tem muitas classes que diferem apenas no comportamento de um método, ou precisa trocar variantes de um algoritmo em tempo de execução.

### 3. Command Pattern
- **Definição**: Encapsula uma solicitação como um objeto, permitindo parametrizar clientes com diferentes solicitações, enfileirar ou registrar solicitações e suportar operações que podem ser desfeitas.
- **Propósito**: Desacoplar o objeto que invoca a operação daquele que sabe como executá-la.
- **Estrutura**:
    - **Command**: Interface com `Execute`. (`ICommand`)
    - **ConcreteCommand**: Implementa `Execute` invocando o Receiver. (`CreateOrderCommand`)
    - **Invoker**: Solicita a execução. (`CommandInvoker`)
    - **Receiver**: Realiza o trabalho real. (`OrderRepository`)
- **Quando Usar**: Para implementar callbacks, filas de tarefas, histórico de operações e Undo/Redo.

### 4. Proxy Pattern
- **Definição**: Fornece um substituto ou marcador para outro objeto para controlar o acesso a ele.
- **Propósito**: Controlar o acesso a um objeto original, podendo adicionar funcionalidades como lazy loading, logging, controle de acesso, etc.
- **Estrutura**:
    - **Subject**: Interface comum. (`IPaymentGateway`)
    - **RealSubject**: Objeto real. (`ExternalPaymentSimulator`)
    - **Proxy**: Mantém referência ao RealSubject e controla acesso. (`PaymentGatewayProxy`)
- **Quando Usar**: Quando você precisa de uma referência mais inteligente a um objeto do que um ponteiro simples (ex: acesso remoto, proteção, cache).

---

## Seção 2: Comparações entre Padrões

- **Observer vs Command**: O Observer é usado para notificar múltiplos objetos passivamente sobre mudanças de estado. O Command é usado para encapsular uma ação específica e ativa. No MyShop, usamos Observer para "efeitos colaterais" (logs, estoque) e Command para "ações do usuário" (comprar, cancelar).
- **Strategy vs Command**: Ambos parametrizam um objeto com uma ação. Strategy parametriza "como" fazer algo (algoritmo de pagamento). Command parametriza "o que" fazer (criar pedido). Strategy é geralmente stateless (ou configurado uma vez), Command carrega estado da requisição.
- **Proxy vs Strategy**: Proxy controla acesso a um objeto existente mantendo a mesma interface. Strategy fornece uma implementação diferente de uma interface para mudar comportamento. O Proxy "envolve", o Strategy "substitui".

---

## Seção 3: Justificativas Detalhadas no Contexto do MyShop

### 1. Observer Pattern (Carrinho de Compras)
#### Por que foi escolhido
O carrinho de compras é o centro de várias ações secundárias (atualizar estoque, recalcular frete, analytics). Acoplar essas lógicas diretamente na classe `Cart` violaria o SRP.
#### Qual problema ele resolve
Evita que a classe `Cart` precise conhecer classes de infraestrutura como `InventoryService` ou `AnalyticsService`.
#### Quais benefícios ele traz
- **Baixo Acoplamento**: `Cart` só conhece `ICartObserver`.
- **Extensibilidade**: Podemos adicionar um `EmailNotificationObserver` sem tocar no `Cart`.
#### Como seria sem o padrão
O método `AddItem` teria chamadas diretas: `_inventory.Reserve(...)`, `_analytics.Log(...)`. Isso tornaria `Cart` difícil de testar e manter.

### 2. Strategy Pattern (Pagamentos)
#### Por que foi escolhido
O sistema precisa suportar múltiplos métodos de pagamento (PIX, Boleto, Cartão) com lógicas muito diferentes, selecionáveis pelo usuário.
#### Qual problema ele resolve
Elimina condicionais complexas (`if type == "PIX" ... else if ...`) dentro do serviço de pagamento.
#### Quais benefícios ele traz
- **OCP (Open/Closed)**: Adicionar "CryptoPayment" é apenas criar uma nova classe, sem alterar o código existente.
- **Isolamento**: A lógica de validação de cartão não se mistura com a geração de QR Code do PIX.
#### Como seria sem o padrão
Um "God Class" `PaymentService` com um método gigante cheio de `switch/case`, difícil de manter e propenso a bugs.

### 3. Command Pattern (Processamento de Pedidos)
#### Por que foi escolhido
Necessidade de auditoria (histórico) e capacidade de desfazer (Undo) operações críticas como criar ou cancelar pedidos.
#### Qual problema ele resolve
A perda de rastreabilidade de "quem fez o que" e a dificuldade de implementar "Undo" em lógicas procedurais.
#### Quais benefícios ele traz
- **Auditoria Automática**: O `CommandInvoker` loga tudo centralizadamente.
- **Undo**: Cada comando sabe como desfazer a si mesmo (`CreateOrder` -> `CancelOrder`).
#### Como seria sem o padrão
Logs espalhados pelos Controllers. Funcionalidade de "Undo" exigiria lógica ad-hoc complexa e duplicada.

### 4. Proxy Pattern (Gateway de Pagamento)
#### Por que foi escolhido
O serviço de pagamento externo é instável e lento. Precisamos de resiliência (Retry) e observabilidade (Logging) sem poluir a lógica de negócio.
#### Qual problema ele resolve
A mistura de lógica de infraestrutura (retry, log, cache) com a lógica de negócio ou com a chamada direta da API externa.
#### Quais benefícios ele traz
- **Separação de Responsabilidades**: O `ExternalPaymentSimulator` só simula a API. O `Proxy` cuida da resiliência.
- **Transparência**: O resto do sistema nem sabe que está usando um Proxy.
#### Como seria sem o padrão
O código de chamada da API teria laços `while(retry)` e `try/catch` misturados com a lógica de formatação da requisição.

---

## Seção 4: Referências
- **Refactoring.Guru**: https://refactoring.guru/design-patterns
- **Gamma, E., et al.** (1994). *Design Patterns: Elements of Reusable Object-Oriented Software*. Addison-Wesley.
- **Martin, R. C.** (2017). *Clean Architecture: A Craftsman's Guide to Software Structure and Design*. Prentice Hall.
