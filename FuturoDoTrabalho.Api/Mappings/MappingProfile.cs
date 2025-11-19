using AutoMapper;
using FuturoDoTrabalho.Api.DTOs;
using FuturoDoTrabalho.Api.Models;

namespace FuturoDoTrabalho.Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Funcionario
            CreateMap<Funcionario, FuncionarioReadDto>();
            CreateMap<FuncionarioCreateDto, Funcionario>();
            CreateMap<FuncionarioUpdateDto, Funcionario>();
            CreateMap<FuncionarioPatchDto, Funcionario>();

            // Departamento
            CreateMap<Departamento, DepartamentoReadDto>();
            CreateMap<DepartamentoCreateDto, Departamento>();
            CreateMap<DepartamentoUpdateDto, Departamento>();
            CreateMap<DepartamentoPatchDto, Departamento>();
        }
    }
}
