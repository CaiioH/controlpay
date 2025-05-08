using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Payment.Application.Interfaces;
using Payment.Application.Services;
using Payment.Domain.Event;
using Payment.Domain.Interfaces;
using Payment.Domain.Messaging;
using Payment.Infrastructure.Data;
using Payment.Infrastructure.Messaging;
using Payment.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21))));

builder.Services.AddScoped<IPagamentoRepository, PagamentoRepository>();
builder.Services.AddScoped<IPagamentoService, PagamentoService>();
builder.Services.AddSingleton<RabbitMqConnection>(); 
builder.Services.AddScoped<IMessagePublisher, RabbitMqPublisher>();
builder.Services.AddScoped(typeof(IRabbitMqConsumer<>), typeof(RabbitMqConsumer<>));
builder.Services.AddScoped(typeof(IRabbitMqResponseConsumer<>), typeof(RabbitMqResponseConsumer<>));
builder.Services.AddScoped(typeof(RabbitMqConsumer<>)); // Adiciona a classe concreta

// builder.Services.AddScoped<RabbitMqConsumer<PagamentoRecebidoEvent>>();

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

// 2. Autorização
builder.Services.AddAuthorization();

// 3. Swagger com suporte a JWT
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

// using (var scope = app.Services.CreateScope())
// {
//     var consumer = scope.ServiceProvider.GetRequiredService<IRabbitMqConsumer<PagamentoVerificadoResponseEvent>>();
//     consumer.StartConsuming(); // 👈 Inicia o consumidor de eventos RabbitMQ
// } 

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication(); // 👈 Isso vem ANTES
app.UseAuthorization();  // 👈 Isso vem DEPOIS

app.MapControllers();

app.Run();

