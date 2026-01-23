using EmpregaAI.Services.Interfaces;
using EmpregaAI.Services;
using EmpregaAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("CurriculoConnection");
Console.WriteLine($"String de Conexão: {connectionString}");

builder.Services.AddDbContext<AplicacaoContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICurriculoService, CurriculoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ICertificacaoService, CertificacaoService>();
builder.Services.AddScoped<IExperienciaService, ExperienciaService>();
builder.Services.AddScoped<IFormacaoService, FormacaoService>();
builder.Services.AddScoped<IExtratorService, ExtratorService>();
builder.Services.AddHttpClient<IProcessadorGroqService, ProcessadorGroqService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024;
});

builder.Services.AddCors(options => {
    options.AddPolicy("Producao", policy => {
        policy.AllowAnyOrigin() // Em produção real, coloque a URL do seu site aqui
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

builder.Services.Configure<VonageSettings>(builder.Configuration.GetSection("Vonage"));
builder.Services.AddScoped<VonageSmsService>();

var app = builder.Build();

// Remova a linha "app.UseCors("AllowVueApp");" que estava aqui

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseCors("Producao"); // Este nome deve ser igual ao builder.Services.AddCors
app.UseSession();
app.UseAuthorization();
app.MapControllers();

app.Run();