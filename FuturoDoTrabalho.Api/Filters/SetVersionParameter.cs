using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FuturoDoTrabalho.Api.Filters
{
    /// <summary>
    /// Filtro que pré-preence e bloqueia o parâmetro "version" de acordo com a versão selecionada
    /// </summary>
    public class SetVersionParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null || operation.Parameters.Count == 0)
                return;

            // Encontrar o parâmetro "version"
            var versionParameter = operation.Parameters
                .FirstOrDefault(p => p.Name.Equals("version", StringComparison.OrdinalIgnoreCase));

            if (versionParameter != null)
            {
                // Obter a versão da API usando o ControllerName ou RelativePath
                string apiVersion = "1"; // Padrão
                
                // Tentar obter a versão pelo RelativePath (ex: /api/v{version}/Departamento)
                var relativePath = context.ApiDescription.RelativePath ?? string.Empty;
                
                // Se está no namespace v2, é versão 2
                var actionDescriptor = context.ApiDescription.ActionDescriptor;
                if (actionDescriptor?.DisplayName?.Contains(".v2.") == true)
                {
                    apiVersion = "2";
                }
                // Fallback para checar RelativePath
                else if (relativePath.Contains("/v2/") || relativePath.Contains("v2"))
                {
                    apiVersion = "2";
                }
                
                // Pré-preencher com o valor correto
                versionParameter.Example = new Microsoft.OpenApi.Any.OpenApiString(apiVersion);
                versionParameter.Schema.Default = new Microsoft.OpenApi.Any.OpenApiString(apiVersion);
                
                // Marcar como ReadOnly (bloqueado)
                versionParameter.Schema.ReadOnly = true;
                
                // Adicionar descrição útil
                versionParameter.Description = $"Versão da API (pré-preenchida como {apiVersion})";
            }
        }
    }
}
