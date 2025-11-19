using AutoMapper;
using FuturoDoTrabalho.Api.DTOs;
using FuturoDoTrabalho.Api.Models;
using FuturoDoTrabalho.Api.Repositories;

namespace FuturoDoTrabalho.Api.Services
{
    public class DepartamentoService : IDepartamentoService
    {
        private readonly IDepartamentoRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartamentoService> _logger;

        public DepartamentoService(
            IDepartamentoRepository repository,
            IMapper mapper,
            ILogger<DepartamentoService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DepartamentoReadDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Buscando departamento ID {DepartamentoId}", id);
            
            if (id <= 0)
                throw new ArgumentException("ID deve ser maior que zero", nameof(id));

            var departamento = await _repository.GetByIdAsync(id);
            return departamento != null ? _mapper.Map<DepartamentoReadDto>(departamento) : null;
        }

        public async Task<IEnumerable<DepartamentoReadDto>> GetAllAsync()
        {
            _logger.LogInformation("Listando todos os departamentos");
            
            var departamentos = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<DepartamentoReadDto>>(departamentos);
        }

        public async Task<(IEnumerable<DepartamentoReadDto> data, int totalCount, int pageCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            _logger.LogInformation("Listando departamentos com paginação - Página {PageNumber}, Tamanho {PageSize}", pageNumber, pageSize);
            
            if (pageNumber <= 0)
                throw new ArgumentException("Número da página deve ser maior que zero", nameof(pageNumber));
            if (pageSize <= 0 || pageSize > 100)
                throw new ArgumentException("Tamanho da página deve ser entre 1 e 100", nameof(pageSize));

            var departamentos = await _repository.GetPagedAsync(pageNumber, pageSize);
            var totalCount = await _repository.GetCountAsync();
            var pageCount = (int)Math.Ceiling(totalCount / (double)pageSize);

            return (_mapper.Map<IEnumerable<DepartamentoReadDto>>(departamentos), totalCount, pageCount);
        }

        public async Task<DepartamentoReadDto?> GetByNomeAsync(string nome)
        {
            _logger.LogInformation("Buscando departamento por nome: {Nome}", nome);
            
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome não pode ser vazio", nameof(nome));

            var departamento = await _repository.GetByNomeAsync(nome);
            return departamento != null ? _mapper.Map<DepartamentoReadDto>(departamento) : null;
        }

        public async Task<IEnumerable<DepartamentoReadDto>> GetAtivosAsync()
        {
            _logger.LogInformation("Listando departamentos ativos");
            
            var departamentos = await _repository.GetAtivosAsync();
            return _mapper.Map<IEnumerable<DepartamentoReadDto>>(departamentos);
        }

        public async Task<DepartamentoReadDto> CreateAsync(DepartamentoCreateDto dto)
        {
            _logger.LogInformation("Criando novo departamento: {Nome}", dto.Nome);
            
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Validar nome único
            var existente = await _repository.GetByNomeAsync(dto.Nome);
            if (existente != null)
                throw new InvalidOperationException($"Já existe um departamento com o nome '{dto.Nome}'");

            var departamento = _mapper.Map<Departamento>(dto);
            departamento.Ativo = true;
            departamento.DataCriacao = DateTime.UtcNow;

            await _repository.CreateAsync(departamento);
            await _repository.SaveChangesAsync();

            return _mapper.Map<DepartamentoReadDto>(departamento);
        }

        public async Task<DepartamentoReadDto?> UpdateAsync(int id, DepartamentoUpdateDto dto)
        {
            _logger.LogInformation("Atualizando departamento ID {DepartamentoId}", id);
            
            if (id <= 0)
                throw new ArgumentException("ID deve ser maior que zero", nameof(id));
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var departamento = await _repository.GetByIdAsync(id);
            if (departamento == null)
                return null;

            // Validar nome único (se mudou)
            if (departamento.Nome != dto.Nome)
            {
                var existente = await _repository.GetByNomeAsync(dto.Nome);
                if (existente != null)
                    throw new InvalidOperationException($"Já existe um departamento com o nome '{dto.Nome}'");
            }

            _mapper.Map(dto, departamento);
            departamento.DataAtualizacao = DateTime.UtcNow;

            await _repository.UpdateAsync(departamento);
            await _repository.SaveChangesAsync();

            return _mapper.Map<DepartamentoReadDto>(departamento);
        }

        public async Task<DepartamentoReadDto?> PatchAsync(int id, DepartamentoPatchDto dto)
        {
            _logger.LogInformation("Atualizando parcialmente departamento ID {DepartamentoId} (PATCH)", id);
            
            if (id <= 0)
                throw new ArgumentException("ID deve ser maior que zero", nameof(id));
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var departamento = await _repository.GetByIdAsync(id);
            if (departamento == null)
                return null;

            // Validar nome único se for atualizado
            if (!string.IsNullOrWhiteSpace(dto.Nome) && departamento.Nome != dto.Nome)
            {
                var existente = await _repository.GetByNomeAsync(dto.Nome);
                if (existente != null)
                    throw new InvalidOperationException($"Já existe um departamento com o nome '{dto.Nome}'");
            }

            _mapper.Map(dto, departamento, typeof(DepartamentoPatchDto), typeof(Departamento));
            departamento.DataAtualizacao = DateTime.UtcNow;

            await _repository.UpdateAsync(departamento);
            await _repository.SaveChangesAsync();

            return _mapper.Map<DepartamentoReadDto>(departamento);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Deletando departamento ID {DepartamentoId}", id);
            
            if (id <= 0)
                throw new ArgumentException("ID deve ser maior que zero", nameof(id));

            var departamento = await _repository.GetByIdAsync(id);
            if (departamento == null)
                return false;

            // Verificar se há funcionários associados
            if (departamento.Funcionarios?.Count > 0)
                throw new InvalidOperationException($"Não é possível deletar o departamento. Existem {departamento.Funcionarios.Count} funcionários associados.");

            return await _repository.DeleteAsync(id);
        }
    }
}
