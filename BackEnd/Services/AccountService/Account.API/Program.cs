using System.Security.Claims;
using System.Text;
using Account.Application.Consumers;
using Account.Application.Interfaces;
using Account.Application.Services;
using Account.Domain.Event;
using Account.Domain.Interfaces;
using Account.Domain.Messaging;
using Account.Infrastructure.Data;
using Account.Infrastructure.Messaging;
using Account.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IContaRepository, ContaRepository>();
builder.Services.AddScoped<IParcelaRepository, ParcelaRepository>();
builder.Services.AddScoped<IContaService, ContaService>();
builder.Services.AddScoped<IParcelaService, ParcelaService>();

// RabbitMq
builder.Services.AddSingleton<RabbitMqConnection>(); 
builder.Services.AddScoped<IMessagePublisher, RabbitMqPublisher>();
builder.Services.AddScoped(typeof(IRabbitMqConsumer<,>), typeof(RabbitMqConsumer<,>));
builder.Services.AddScoped(typeof(IRabbitMqResponseConsumer<,>), typeof(RabbitMqResponseConsumer<,>));
builder.Services.AddScoped(typeof(RabbitMqConsumer<,>)); // Adiciona a classe concreta

// Dependência de consumo de eventos
builder.Services.AddScoped<IMessageConsumer<VerificarPagamentoRequestEvent, VerificarPagamentoResponseEvent>, VerificarPagamentoConsumer>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AccountDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21))));

// Configurações de autenticação
var jwtConfig = builder.Configuration.GetSection("Jwt");
var secretKey = jwtConfig["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtConfig["Issuer"],
        ValidAudience = jwtConfig["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),

        RoleClaimType = ClaimTypes.Role // 👈 ISSO aqui garante que [Authorize(Roles = "...")] funcione
    };
});

// Autorização
builder.Services.AddAuthorization();

// Swagger com suporte a JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite: Bearer {seu token}"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddControllers();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication(); // 👈 Isso vem ANTES
app.UseAuthorization();  // 👈 Isso vem DEPOIS

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var consumer = scope.ServiceProvider.GetRequiredService<IRabbitMqConsumer<VerificarPagamentoRequestEvent, VerificarPagamentoResponseEvent>>();
    consumer.StartConsuming(); // 👈 Inicia o consumidor de eventos RabbitMQ
    Console.WriteLine("Iniciando o consumidor de eventos RabbitMQ...");
} 

app.Run();

