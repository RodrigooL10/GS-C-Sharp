using AutoMapper;
using FuturoDoTrabalho.Api.DTOs;
using FuturoDoTrabalho.Api.Models;
using FuturoDoTrabalho.Api.Repositories;

namespace FuturoDoTrabalho.Api.Services
{
    public class FuncionarioService : IFuncionarioService
    {
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IDepartamentoRepository _departamentoRepository;
        private readonly IMapper _mapper;

        public FuncionarioService(
            IFuncionarioRepository funcionarioRepository,
            IDepartamentoRepository departamentoRepository,
            IMapper mapper)
        {
            _funcionarioRepository = funcionarioRepository;
            _departamentoRepository = departamentoRepository;
            _mapper = mapper;
        }

        public async Task<FuncionarioReadDto?> GetByIdAsync(int id)
        {
            var funcionario = await _funcionarioRepository.GetByIdAsync(id);
            if (funcionario == null)
                return null;

            var dto = _mapper.Map<FuncionarioReadDto>(funcionario);
            if (funcionario.Departamento != null)
                dto.DepartamentoNome = funcionario.Departamento.Nome;

            return dto;
        }

        public async Task<List<FuncionarioReadDto>> GetAllAsync()
        {
            var funcionarios = await _funcionarioRepository.GetAllAsync();
            var dtos = _mapper.Map<List<FuncionarioReadDto>>(funcionarios);

            foreach (var dto in dtos)
            {
                var funcionario = funcionarios.First(f => f.Id == dto.Id);
                if (funcionario.Departamento != null)
                    dto.DepartamentoNome = funcionario.Departamento.Nome;
            }

            return dtos;
        }

        public async Task<(List<FuncionarioReadDto> data, int totalCount, int pageCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var funcionarios = await _funcionarioRepository.GetPagedAsync(pageNumber, pageSize);
            var totalCount = await _funcionarioRepository.GetCountAsync();
            var pageCount = (totalCount + pageSize - 1) / pageSize;

            var dtos = _mapper.Map<List<FuncionarioReadDto>>(funcionarios);

            foreach (var dto in dtos)
            {
                var funcionario = funcionarios.First(f => f.Id == dto.Id);
                if (funcionario.Departamento != null)
                    dto.DepartamentoNome = funcionario.Departamento.Nome;
            }

            return (dtos, totalCount, pageCount);
        }

        public async Task<FuncionarioReadDto?> GetByCpfAsync(string cpf)
        {
            var funcionario = await _funcionarioRepository.GetByCpfAsync(cpf);
            if (funcionario == null)
                return null;

            var dto = _mapper.Map<FuncionarioReadDto>(funcionario);
            if (funcionario.Departamento != null)
                dto.DepartamentoNome = funcionario.Departamento.Nome;

            return dto;
        }

        public async Task<List<FuncionarioReadDto>> GetByDepartamentoAsync(int departamentoId)
        {
            var funcionarios = await _funcionarioRepository.GetByDepartamentoAsync(departamentoId);
            var dtos = _mapper.Map<List<FuncionarioReadDto>>(funcionarios);

            foreach (var dto in dtos)
            {
                var funcionario = funcionarios.First(f => f.Id == dto.Id);
                if (funcionario.Departamento != null)
                    dto.DepartamentoNome = funcionario.Departamento.Nome;
            }

            return dtos;
        }

        public async Task<List<FuncionarioReadDto>> GetAtivosAsync()
        {
            var funcionarios = await _funcionarioRepository.GetAtivosAsync();
            var dtos = _mapper.Map<List<FuncionarioReadDto>>(funcionarios);

            foreach (var dto in dtos)
            {
                var funcionario = funcionarios.First(f => f.Id == dto.Id);
                if (funcionario.Departamento != null)
                    dto.DepartamentoNome = funcionario.Departamento.Nome;
            }

            return dtos;
        }

        public async Task<FuncionarioReadDto> CreateAsync(FuncionarioCreateDto dto)
        {
            // Validar se departamento existe
            var departamento = await _departamentoRepository.GetByIdAsync(dto.DepartamentoId);
            if (departamento == null)
                throw new InvalidOperationException("Departamento não encontrado");

            // Validar CPF único
            if (!string.IsNullOrWhiteSpace(dto.CPF))
            {
                var existente = await _funcionarioRepository.GetByCpfAsync(dto.CPF);
                if (existente != null)
                    throw new InvalidOperationException("CPF já existe no sistema");
            }

            var funcionario = _mapper.Map<Funcionario>(dto);
            funcionario.DataCriacao = DateTime.UtcNow;

            var criado = await _funcionarioRepository.CreateAsync(funcionario);
            criado.Departamento = departamento;

            var resultado = _mapper.Map<FuncionarioReadDto>(criado);
            resultado.DepartamentoNome = departamento.Nome;

            return resultado;
        }

        public async Task<FuncionarioReadDto?> UpdateAsync(int id, FuncionarioUpdateDto dto)
        {
            var funcionario = await _funcionarioRepository.GetByIdAsync(id);
            if (funcionario == null)
                return null;

            // Validar departamento
            var departamento = await _departamentoRepository.GetByIdAsync(dto.DepartamentoId);
            if (departamento == null)
                throw new InvalidOperationException("Departamento não encontrado");

            _mapper.Map(dto, funcionario);
            funcionario.DataAtualizacao = DateTime.UtcNow;

            await _funcionarioRepository.UpdateAsync(funcionario);
            funcionario.Departamento = departamento;

            var resultado = _mapper.Map<FuncionarioReadDto>(funcionario);
            resultado.DepartamentoNome = departamento.Nome;

            return resultado;
        }

        public async Task<FuncionarioReadDto?> PatchAsync(int id, FuncionarioPatchDto dto)
        {
            var funcionario = await _funcionarioRepository.GetByIdAsync(id);
            if (funcionario == null)
                return null;

            if (!string.IsNullOrWhiteSpace(dto.Nome))
                funcionario.Nome = dto.Nome;

            if (!string.IsNullOrWhiteSpace(dto.Cargo))
                funcionario.Cargo = dto.Cargo;

            if (!string.IsNullOrWhiteSpace(dto.Email))
                funcionario.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.Telefone))
                funcionario.Telefone = dto.Telefone;

            if (dto.Salario.HasValue)
                funcionario.Salario = dto.Salario.Value;

            if (!string.IsNullOrWhiteSpace(dto.Endereco))
                funcionario.Endereco = dto.Endereco;

            if (dto.NivelSenioridade.HasValue)
                funcionario.NivelSenioridade = dto.NivelSenioridade.Value;

            if (dto.Ativo.HasValue)
                funcionario.Ativo = dto.Ativo.Value;

            funcionario.DataAtualizacao = DateTime.UtcNow;

            await _funcionarioRepository.UpdateAsync(funcionario);

            var resultado = _mapper.Map<FuncionarioReadDto>(funcionario);
            if (funcionario.Departamento != null)
                resultado.DepartamentoNome = funcionario.Departamento.Nome;

            return resultado;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _funcionarioRepository.DeleteAsync(id);
        }
    }
}
