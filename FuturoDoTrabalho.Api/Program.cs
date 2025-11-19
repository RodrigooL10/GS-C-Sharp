using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Versioning;
using FuturoDoTrabalho.Api.Data;
using FuturoDoTrabalho.Api.Repositories;
using FuturoDoTrabalho.Api.Services;
using FuturoDoTrabalho.Api.Filters;

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
    
    // Filtrar endpoints por versão - v1 exclui PATCH
    options.DocInclusionPredicate((docName, apiDesc) =>
    {
        // Verificar versão pelo caminho da ação
        var actionDescriptor = apiDesc.ActionDescriptor;
        if (actionDescriptor == null)
            return true;

        var controllerName = actionDescriptor.DisplayName;
        
        if (docName == "v1")
        {
            // v1 não deve mostrar PATCH
            var isPatchMethod = apiDesc.HttpMethod?.Equals("PATCH", StringComparison.OrdinalIgnoreCase) == true;
            if (isPatchMethod)
                return false;
            
            // Mostrar apenas endpoints de v1
            return controllerName?.Contains("FuturoDoTrabalho.Api.Controllers.v1") == true;
        }

        if (docName == "v2")
        {
            // Mostrar apenas endpoints de v2
            return controllerName?.Contains("FuturoDoTrabalho.Api.Controllers.v2") == true;
        }

        return false;
    });
    
    // Pré-preencer e bloquear o parâmetro "version" de acordo com a versão selecionada
    options.OperationFilter<SetVersionParameter>();
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
    
    // Servir arquivo estático customizado do Swagger
    app.UseStaticFiles();
    
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "GD Solutions v1 - Básica");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "GD Solutions v2 - Avançada (com PATCH)");
        options.RoutePrefix = string.Empty;
        options.DefaultModelsExpandDepth(-1);
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.Run();
