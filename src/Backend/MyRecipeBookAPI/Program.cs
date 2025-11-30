using MyRecipeBookAPI.Filters;
using MyRecipeBookAPI.Middleware;
using MyRecipeBook.Application;
using MyRecipeBook.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using MyRecipeBook.Infrastructure.Migrations;
using MyRecipeBook.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));
// Injeção de dependências

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Avisar a API do Middleware de redirecionamento para HTTPS
app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

MigrateDatabase();

app.Run();



void MigrateDatabase()
{
    var databaseType = builder.Configuration.DatabaseType();
    var connectionString = builder.Configuration.ConnectionString();


    DatabaseMigration.Migrate(databaseType, connectionString);
}