using EmpregaAI.Services.Interfaces;
using EmpregaAI.Services;
using EmpregaAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuração da string de conexão
var connectionString = builder.Configuration.GetConnectionString("CurriculoConnection");
Console.WriteLine($"String de Conexão: {connectionString}");

// Registrando o DbContext para injeção
builder.Services.AddDbContext<AplicacaoContext>(options =>
    options.UseSqlServer(connectionString));

// Adicionando serviços ao contêiner
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrando serviços
builder.Services.AddScoped<ICurriculoService, CurriculoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ICertificacaoService, CertificacaoService>();
builder.Services.AddScoped<IExperienciaService, ExperienciaService>();
builder.Services.AddScoped<IFormacaoService, FormacaoService>();

// CONFIGURAÇÃO DE SESSÃO (adicione antes de builder.Build())
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.None; // IMPORTANTE para CORS
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Para HTTPS
});

// CORS - CORRIGIDO
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// ORDEM CORRETA DOS MIDDLEWARES
app.UseCors("AllowVueApp"); // CORS primeiro

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSession(); // Session depois do CORS
app.UseAuthorization();
app.MapControllers();

app.Run();