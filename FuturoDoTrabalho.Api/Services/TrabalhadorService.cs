using FuturoDoTrabalho.Api.DTOs;
using FuturoDoTrabalho.Api.Models;
using FuturoDoTrabalho.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuturoDoTrabalho.Api.Services
{
    public class TrabalhadorService : ITrabalhadorService
    {
        private readonly ITrabalhadorRepository _repository;

        public TrabalhadorService(ITrabalhadorRepository repository)
        {
            _repository = repository;
        }

        public async Task<TrabalhadorReadDto> GetByIdAsync(int id)
        {
            var trabalhador = await _repository.GetByIdAsync(id);
            if (trabalhador == null)
                return null;

            return MapToReadDto(trabalhador);
        }

        public async Task<IEnumerable<TrabalhadorReadDto>> GetAllAsync(bool? ativo = null)
        {
            var trabalhadores = await _repository.GetAllAsync(ativo);
            return trabalhadores.Select(MapToReadDto).ToList();
        }

        public async Task<IEnumerable<TrabalhadorReadDto>> GetPagedAsync(int pageNumber, int pageSize, bool? ativo = null)
        {
            if (pageNumber < 1)
                pageNumber = 1;
            if (pageSize < 1)
                pageSize = 10;
            if (pageSize > 100)
                pageSize = 100;

            var trabalhadores = await _repository.GetPagedAsync(pageNumber, pageSize, ativo);
            return trabalhadores.Select(MapToReadDto).ToList();
        }

        public async Task<int> GetCountAsync(bool? ativo = null)
        {
            return await _repository.GetCountAsync(ativo);
        }

        public async Task<TrabalhadorReadDto> CreateAsync(TrabalhadorCreateDto dto)
        {
            // Validações adicionais
            if (string.IsNullOrWhiteSpace(dto.Nome))
                throw new ArgumentException("Nome é obrigatório");

            if (string.IsNullOrWhiteSpace(dto.Cargo))
                throw new ArgumentException("Cargo é obrigatório");

            // Verificar se CPF já existe
            if (!string.IsNullOrWhiteSpace(dto.CPF))
            {
                var existente = await _repository.GetByCpfAsync(dto.CPF);
                if (existente != null)
                    throw new InvalidOperationException("CPF já cadastrado");
            }

            var trabalhador = new Trabalhador
            {
                Nome = dto.Nome,
                Cargo = dto.Cargo,
                Departamento = dto.Departamento,
                Salario = dto.Salario,
                DataAdmissao = dto.DataAdmissao,
                CPF = dto.CPF,
                Telefone = dto.Telefone,
                Email = dto.Email,
                Endereco = dto.Endereco,
                Ativo = true
            };

            var resultado = await _repository.CreateAsync(trabalhador);
            return MapToReadDto(resultado);
        }

        public async Task<TrabalhadorReadDto> UpdateAsync(int id, TrabalhadorUpdateDto dto)
        {
            var trabalhador = await _repository.GetByIdAsync(id);
            if (trabalhador == null)
                return null;

            // Atualizar apenas os campos fornecidos
            if (!string.IsNullOrWhiteSpace(dto.Nome))
                trabalhador.Nome = dto.Nome;

            if (!string.IsNullOrWhiteSpace(dto.Cargo))
                trabalhador.Cargo = dto.Cargo;

            if (!string.IsNullOrWhiteSpace(dto.Departamento))
                trabalhador.Departamento = dto.Departamento;

            if (dto.Salario > 0)
                trabalhador.Salario = dto.Salario;

            if (dto.DataAdmissao.HasValue)
                trabalhador.DataAdmissao = dto.DataAdmissao.Value;

            if (!string.IsNullOrWhiteSpace(dto.CPF))
            {
                // Verificar se o novo CPF já existe
                var existente = await _repository.GetByCpfAsync(dto.CPF);
                if (existente != null && existente.Id != id)
                    throw new InvalidOperationException("CPF já cadastrado");

                trabalhador.CPF = dto.CPF;
            }

            if (!string.IsNullOrWhiteSpace(dto.Telefone))
                trabalhador.Telefone = dto.Telefone;

            if (!string.IsNullOrWhiteSpace(dto.Email))
                trabalhador.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.Endereco))
                trabalhador.Endereco = dto.Endereco;

            if (dto.Ativo.HasValue)
                trabalhador.Ativo = dto.Ativo.Value;

            var resultado = await _repository.UpdateAsync(trabalhador);
            return MapToReadDto(resultado);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        private TrabalhadorReadDto MapToReadDto(Trabalhador trabalhador)
        {
            return new TrabalhadorReadDto
            {
                Id = trabalhador.Id,
                Nome = trabalhador.Nome,
                Cargo = trabalhador.Cargo,
                Departamento = trabalhador.Departamento,
                Salario = trabalhador.Salario,
                DataAdmissao = trabalhador.DataAdmissao,
                CPF = trabalhador.CPF,
                Telefone = trabalhador.Telefone,
                Email = trabalhador.Email,
                Endereco = trabalhador.Endereco,
                Ativo = trabalhador.Ativo,
                DataCriacao = trabalhador.DataCriacao,
                DataAtualizacao = trabalhador.DataAtualizacao
            };
        }
    }
}
