using Microsoft.AspNetCore.Mvc;
using FuturoDoTrabalho.Api.DTOs;
using FuturoDoTrabalho.Api.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FuturoDoTrabalho.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TrabalhadorController : ControllerBase
    {
        private readonly ITrabalhadorService _service;

        public TrabalhadorController(ITrabalhadorService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtém todos os trabalhadores
        /// </summary>
        /// <param name="ativo">Filtro opcional por status ativo</param>
        /// <returns>Lista de trabalhadores</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TrabalhadorReadDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<TrabalhadorReadDto>>> GetAll([FromQuery] bool? ativo = null)
        {
            try
            {
                var trabalhadores = await _service.GetAllAsync(ativo);
                return Ok(trabalhadores);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao obter trabalhadores", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtém um trabalhador por ID
        /// </summary>
        /// <param name="id">ID do trabalhador</param>
        /// <returns>Dados do trabalhador</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TrabalhadorReadDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TrabalhadorReadDto>> GetById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { message = "ID deve ser maior que zero" });

                var trabalhador = await _service.GetByIdAsync(id);
                if (trabalhador == null)
                    return NotFound(new { message = "Trabalhador não encontrado" });

                return Ok(trabalhador);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao obter trabalhador", error = ex.Message });
            }
        }

        /// <summary>
        /// Cria um novo trabalhador
        /// </summary>
        /// <param name="dto">Dados do trabalhador</param>
        /// <returns>Trabalhador criado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TrabalhadorReadDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TrabalhadorReadDto>> Create([FromBody] TrabalhadorCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var trabalhador = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = trabalhador.Id }, trabalhador);
            }
            catch (System.InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao criar trabalhador", error = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um trabalhador existente
        /// </summary>
        /// <param name="id">ID do trabalhador</param>
        /// <param name="dto">Dados a serem atualizados</param>
        /// <returns>Trabalhador atualizado</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TrabalhadorReadDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TrabalhadorReadDto>> Update(int id, [FromBody] TrabalhadorUpdateDto dto)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { message = "ID deve ser maior que zero" });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var trabalhador = await _service.UpdateAsync(id, dto);
                if (trabalhador == null)
                    return NotFound(new { message = "Trabalhador não encontrado" });

                return Ok(trabalhador);
            }
            catch (System.InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao atualizar trabalhador", error = ex.Message });
            }
        }

        /// <summary>
        /// Deleta um trabalhador
        /// </summary>
        /// <param name="id">ID do trabalhador</param>
        /// <returns>Confirmação de deleção</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { message = "ID deve ser maior que zero" });

                var resultado = await _service.DeleteAsync(id);
                if (!resultado)
                    return NotFound(new { message = "Trabalhador não encontrado" });

                return NoContent();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao deletar trabalhador", error = ex.Message });
            }
        }
    }
}
