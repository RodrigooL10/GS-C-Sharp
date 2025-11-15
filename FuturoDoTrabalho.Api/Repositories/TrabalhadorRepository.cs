using FuturoDoTrabalho.Api.Data;
using FuturoDoTrabalho.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuturoDoTrabalho.Api.Repositories
{
    public class TrabalhadorRepository : ITrabalhadorRepository
    {
        private readonly AppDbContext _context;

        public TrabalhadorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Trabalhador> GetByIdAsync(int id)
        {
            return await _context.Trabalhadores.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Trabalhador>> GetAllAsync(bool? ativo = null)
        {
            var query = _context.Trabalhadores.AsQueryable();

            if (ativo.HasValue)
            {
                query = query.Where(t => t.Ativo == ativo.Value);
            }

            return await query.OrderByDescending(t => t.DataCriacao).ToListAsync();
        }

        public async Task<Trabalhador> GetByCpfAsync(string cpf)
        {
            return await _context.Trabalhadores.FirstOrDefaultAsync(t => t.CPF == cpf);
        }

        public async Task<Trabalhador> CreateAsync(Trabalhador trabalhador)
        {
            trabalhador.DataCriacao = DateTime.UtcNow;
            _context.Trabalhadores.Add(trabalhador);
            await _context.SaveChangesAsync();
            return trabalhador;
        }

        public async Task<Trabalhador> UpdateAsync(Trabalhador trabalhador)
        {
            trabalhador.DataAtualizacao = DateTime.UtcNow;
            _context.Trabalhadores.Update(trabalhador);
            await _context.SaveChangesAsync();
            return trabalhador;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var trabalhador = await GetByIdAsync(id);
            if (trabalhador == null)
                return false;

            _context.Trabalhadores.Remove(trabalhador);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCountAsync(bool? ativo = null)
        {
            var query = _context.Trabalhadores.AsQueryable();

            if (ativo.HasValue)
            {
                query = query.Where(t => t.Ativo == ativo.Value);
            }

            return await query.CountAsync();
        }

        public async Task<IEnumerable<Trabalhador>> GetPagedAsync(int pageNumber, int pageSize, bool? ativo = null)
        {
            var query = _context.Trabalhadores.AsQueryable();

            if (ativo.HasValue)
            {
                query = query.Where(t => t.Ativo == ativo.Value);
            }

            return await query
                .OrderByDescending(t => t.DataCriacao)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
