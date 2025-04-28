var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.MapReverseProxy();

app.Run();

// AINDA PRECISA CONFIGURAR O "appsettings.json" PARA O REVERSE PROXY FUNCIONAR
