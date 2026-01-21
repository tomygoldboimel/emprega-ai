using EmpregaAI.Services.Interfaces;
using EmpregaAI.Services;
using EmpregaAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("CurriculoConnection");
Console.WriteLine($"String de Conexão: {connectionString}");

builder.Services.AddDbContext<AplicacaoContext>(options =>
    options.UseSqlServer(connectionString));

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

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

builder.Services.Configure<VonageSettings>(builder.Configuration.GetSection("Vonage"));
builder.Services.AddScoped<VonageSmsService>();

var app = builder.Build();

app.UseCors("AllowVueApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSession();
app.UseAuthorization();
app.MapControllers();

app.Run();