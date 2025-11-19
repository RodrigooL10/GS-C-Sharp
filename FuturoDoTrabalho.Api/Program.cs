using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Versioning;
using FuturoDoTrabalho.Api.Data;
using FuturoDoTrabalho.Api.Repositories;
using FuturoDoTrabalho.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Add API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new Microsoft.AspNetCore.Mvc.Versioning.HeaderApiVersionReader("X-API-Version"),
        new Microsoft.AspNetCore.Mvc.Versioning.QueryStringApiVersionReader("api-version")
    );
});

// Add Swagger/OpenAPI
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "GD Solutions - API de Gestão de Funcionários",
        Version = "v1",
        Description = "API REST para gerenciamento de funcionários e departamentos - Versão 1 (Básica)",
    });
    
    options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "GD Solutions - API de Gestão de Funcionários",
        Version = "v2",
        Description = "API REST para gerenciamento de funcionários e departamentos - Versão 2 (Avançada com paginação e PATCH)",
    });
    
    // Resolver conflitos entre v1 e v2
    options.ResolveConflictingActions(apiDescriptions =>
    {
        return apiDescriptions.First();
    });
});

// Add MySQL Database Context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, 
        ServerVersion.AutoDetect(connectionString),
        mysqlOptions => mysqlOptions.EnableRetryOnFailure())
);

// Register Repositories - New (GD Solutions)
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
builder.Services.AddScoped<IDepartamentoRepository, DepartamentoRepository>();

// Register Services - New (GD Solutions)
builder.Services.AddScoped<IFuncionarioService, FuncionarioService>();
builder.Services.AddScoped<IDepartamentoService, DepartamentoService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "GD Solutions v1 - Básica");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "GD Solutions v2 - Avançada (com PATCH)");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.Run();
