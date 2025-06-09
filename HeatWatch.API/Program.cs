using System;
using System.IO;
using AspNetCoreRateLimit;
using HeatWatch.API.Data;
using HeatWatch.API.Repositories;
using HeatWatch.API.Services;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Oracle.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ─── 1) DbContext (Oracle)
builder.Services.AddDbContext<HeatWatchContext>(opts =>
    opts.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// ─── 2) Controllers
builder.Services.AddControllers();

// ─── 3) DI: Repositórios & Services
builder.Services.AddScoped<IRegiaoRepository, RegiaoRepository>();
builder.Services.AddScoped<IRegiaoService, RegiaoService>();
builder.Services.AddScoped<IEventoCalorRepository, EventoCalorRepository>();
builder.Services.AddScoped<IEventoCalorService, EventoCalorService>();
builder.Services.AddScoped<IRegistroTemperaturaRepository, RegistroTemperaturaRepository>();
builder.Services.AddScoped<IRegistroTemperaturaService, RegistroTemperaturaService>();
builder.Services.AddScoped<IAlertaRepository, AlertaRepository>();
builder.Services.AddScoped<IAlertaService, AlertaService>();

// ─── 4) API Versioning via URL segment
builder.Services.AddApiVersioning(opts =>
{
    opts.DefaultApiVersion = new ApiVersion(1, 0);
    opts.AssumeDefaultVersionWhenUnspecified = true;
    opts.ReportApiVersions = true;
    opts.ApiVersionReader = new UrlSegmentApiVersionReader();
});
builder.Services.AddVersionedApiExplorer(opts =>
{
    opts.GroupNameFormat = "'v'VVV";
    opts.SubstituteApiVersionInUrl = true;
});

// ─── 5) ProblemDetails (RFC7807)
builder.Services.AddProblemDetails(opts =>
{
    opts.IncludeExceptionDetails = (ctx, ex) =>
        builder.Environment.IsDevelopment();
});

// ─── 6) Rate Limiting por IP
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(
    builder.Configuration.GetSection("IpRateLimiting")
);
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// ─── 7) JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = "https://seu-identity-server/";
    options.TokenValidationParameters.ValidateAudience = false;
});

// ─── 8) Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HeatWatch API", Version = "v1" });
    // Inclui comentários XML
    var xmlFile = "HeatWatch.API.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Use 'Bearer {token}'",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// ─── Pipeline HTTP
app.UseProblemDetails();
app.UseIpRateLimiting();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HeatWatch API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.MapControllers();
app.Run();
