using Microsoft.EntityFrameworkCore;
using TodoApp.API.Middleware;
using TodoApp.Application;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Persistence;
using TodoApp.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Controller'lar ve CQRS/MediatR uygulama katmanı.
builder.Services.AddControllers();
builder.Services.AddApplication();

// Swagger yalnızca API keşfi ve test içindir.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Persistence implementasyonu Application'ın repository sözleşmesine bağlanır.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ITodoRepository, TodoRepository>();

var app = builder.Build();

// Application exception'ları güvenli ve tutarlı HTTP cevaplarına çevrilir.
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
