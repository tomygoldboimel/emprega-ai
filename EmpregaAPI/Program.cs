using EmpregaAI.Services.Interfaces;
using EmpregaAI.Services;
using EmpregaAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("CurriculoConnection")
                ?? builder.Configuration.GetConnectionString("CurriculoConnection");

builder.Services.AddDbContextPool<AplicacaoContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions => {
        npgsqlOptions.EnableRetryOnFailure(3);
    }));

builder.Services.AddResponseCompression(options => {
    options.Providers.Add<GzipCompressionProvider>();
    options.EnableForHttps = true;
});

builder.Services.AddScoped<ICurriculoService, CurriculoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IExperienciaService, ExperienciaService>();
builder.Services.AddScoped<IFormacaoService, FormacaoService>();

builder.Services.AddHttpClient();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

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

builder.Services.Configure<FormOptions>(options => options.MultipartBodyLengthLimit = 10 * 1024 * 1024);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();
app.UseResponseCompression();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmpregaAI API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors("AppPolicy");
app.UseSession();
app.UseAuthorization();
app.MapControllers();

app.Run();