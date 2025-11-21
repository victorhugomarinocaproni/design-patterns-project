using Microsoft.EntityFrameworkCore;
using MyShop.Application.Services;
using MyShop.Application.UseCases.Cart;
using MyShop.Application.UseCases.Order;
using MyShop.Domain.Interfaces;
using MyShop.Infrastructure.Observers;
using MyShop.Infrastructure.Payment;
using MyShop.Infrastructure.Payment.Strategies;
using MyShop.Infrastructure.Persistence.Repositories;
using MyShop.Infrastructure.Persistence.Sqlite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<MyShopDbContext>(options =>
    options.UseSqlite("Data Source=myshop.db"));

// Repositories
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICommandLogRepository, CommandLogRepository>();

// UseCases
builder.Services.AddScoped<AddItemToCartUseCase>();
builder.Services.AddScoped<RemoveItemFromCartUseCase>();
builder.Services.AddScoped<GetOrderHistoryUseCase>();

// Services
builder.Services.AddScoped<CommandInvoker>();

// Strategies
builder.Services.AddScoped<CreditCardPaymentStrategy>();
builder.Services.AddScoped<DebitCardPaymentStrategy>();
builder.Services.AddScoped<PixPaymentStrategy>();
builder.Services.AddScoped<BoletoPaymentStrategy>();
builder.Services.AddScoped<IPaymentStrategyFactory, PaymentStrategyFactory>();

// Proxy Pattern
builder.Services.AddScoped<ExternalPaymentSimulator>();
builder.Services.AddScoped<IPaymentGateway>(sp => 
    new PaymentGatewayProxy(
        sp.GetRequiredService<ExternalPaymentSimulator>(),
        sp.GetRequiredService<ILogger<PaymentGatewayProxy>>()
    ));

// Observers
builder.Services.AddScoped<CartInventoryObserver>();
builder.Services.AddScoped<CartAnalyticsObserver>();
// Register IEnumerable<ICartObserver>
builder.Services.AddScoped<IEnumerable<ICartObserver>>(sp => new ICartObserver[]
{
    sp.GetRequiredService<CartInventoryObserver>(),
    sp.GetRequiredService<CartAnalyticsObserver>()
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Database Seeding (Simple)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MyShopDbContext>();
    db.Database.EnsureCreated();
    
    // Seed Products if empty
    if (!db.Products.Any())
    {
        db.Products.AddRange(
            new MyShop.Domain.Entities.Product("Laptop", new MyShop.Domain.ValueObjects.Money(5000, new MyShop.Domain.ValueObjects.Currency("BRL", "R$")), 10),
            new MyShop.Domain.Entities.Product("Mouse", new MyShop.Domain.ValueObjects.Money(150, new MyShop.Domain.ValueObjects.Currency("BRL", "R$")), 50),
            new MyShop.Domain.Entities.Product("Keyboard", new MyShop.Domain.ValueObjects.Money(300, new MyShop.Domain.ValueObjects.Currency("BRL", "R$")), 30),
            new MyShop.Domain.Entities.Product("Monitor", new MyShop.Domain.ValueObjects.Money(1200, new MyShop.Domain.ValueObjects.Currency("BRL", "R$")), 15)
        );
        db.SaveChanges();
    }
}

app.Run();
