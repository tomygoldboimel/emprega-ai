using EmpregaAI.Services.Interfaces;
using EmpregaAI.Services;
using EmpregaAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// 1. PRIORIDADE DE CONEXÃO: 
// Primeiro tenta pegar a variável do Railway (Produção), se não houver, pega do appsettings (Local)
var connectionString = Environment.GetEnvironmentVariable("CurriculoConnection")
                      ?? builder.Configuration.GetConnectionString("CurriculoConnection");

if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("ERRO: String de conexão não encontrada!");
}

builder.Services.AddDbContext<AplicacaoContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Injeção de Dependências
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
    options.Cookie.SameSite = SameSiteMode.None; // Necessário para cookies entre domínios diferentes
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024;
});
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
// 2. CORS ADAPTÁVEL: Libera localhost no dev e Railway na produção
builder.Services.AddCors(options => {
    options.AddPolicy("AppPolicy", policy => {
        policy.WithOrigins(
                "https://empregaai.up.railway.app",
                "http://localhost:5173",
                "https://dante-fibular-pulpily.ngrok-free.dev"
              )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.Configure<VonageSettings>(builder.Configuration.GetSection("Vonage"));
builder.Services.AddScoped<VonageSmsService>();

var app = builder.Build();

// 3. SWAGGER SEMPRE ATIVO NO RAILWAY (Opcional, mas útil para testes)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmpregaAI API V1");
    c.RoutePrefix = string.Empty;
});

app.UseCors("AppPolicy");
app.UseSession();
app.UseAuthorization();
app.MapControllers();

app.Run();