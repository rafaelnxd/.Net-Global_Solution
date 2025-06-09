using HeatWatch.API.DTOs;
using HeatWatch.API.Helpers;
using HeatWatch.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;

namespace HeatWatch.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/registros-temperatura")]
    public class RegistroTemperaturaController : ControllerBase
    {
        private readonly IRegistroTemperaturaService _service;
        public RegistroTemperaturaController(IRegistroTemperaturaService service)
            => _service = service;

        /// <summary>
        /// Retorna uma lista paginada de registros de temperatura.
        /// </summary>
        /// <param name="page">Número da página (padrão = 1).</param>
        /// <param name="size">Tamanho da página (padrão = 20).</param>
        /// <param name="sort">Campo de ordenação (padrão = "DataRegistro").</param>
        /// <param name="filter">Filtro de busca (opcional).</param>
        /// <returns>Lista de registros de temperatura com metadados de paginação.</returns>
        [HttpGet(Name = "GetAllRegistrosTemperatura")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int size = 20,
            [FromQuery] string sort = "DataRegistro",
            [FromQuery] string filter = null)
        {
            var paged = await _service.GetAllAsync(page, size, sort, filter);
            Response.Headers.Add("X-Total-Count", paged.TotalCount.ToString());
            Response.GetTypedHeaders().CacheControl =
                new CacheControlHeaderValue { Public = true, MaxAge = TimeSpan.FromMinutes(5) };

            return Ok(new
            {
                data = paged.Items,
                page = paged.Page,
                size = paged.PageSize,
                total = paged.TotalCount,
                links = Url.Links("GetAllRegistrosTemperatura", new { page, size, sort, filter })
            });
        }

        /// <summary>
        /// Retorna um registro de temperatura pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do registro.</param>
        /// <returns>Detalhes do registro de temperatura.</returns>
        [HttpGet("{id}", Name = "GetRegistroTemperaturaById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
                return NotFound(new ProblemDetails
                {
                    Type = "https://heatwatch.io/errors-not-found",
                    Title = "Registro de Temperatura não encontrado",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"Não existe registro com id={id}",
                    Instance = HttpContext.Request.Path
                });

            Response.Headers.ETag =
                new EntityTagHeaderValue($"\"{dto.Id.GetHashCode()}\"").ToString();

            return Ok(new
            {
                data = dto,
                links = Url.Links("GetRegistroTemperaturaById", new { id })
            });
        }

        /// <summary>
        /// Cria um novo registro de temperatura.
        /// </summary>
        /// <param name="dto">Dados para criação do registro.</param>
        /// <returns>Dados do registro criado.</returns>
        [HttpPost(Name = "CreateRegistroTemperatura")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateRegistroTemperaturaDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(
                actionName: nameof(GetById),
                routeValues: new { id = created.Id, version = HttpContext.GetRequestedApiVersion()?.ToString() },
                value: created);
        }

        /// <summary>
        /// Atualiza um registro de temperatura existente.
        /// </summary>
        /// <param name="id">Identificador do registro.</param>
        /// <param name="dto">Dados para atualização do registro.</param>
        [HttpPut("{id}", Name = "UpdateRegistroTemperatura")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRegistroTemperaturaDto dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Remove um registro de temperatura.
        /// </summary>
        /// <param name="id">Identificador do registro.</param>
        [HttpDelete("{id}", Name = "DeleteRegistroTemperatura")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
