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
    [Route("api/v{version:apiVersion}/eventos-calor")]
    public class EventoCalorController : ControllerBase
    {
        private readonly IEventoCalorService _service;
        public EventoCalorController(IEventoCalorService service)
            => _service = service;

        /// <summary>
        /// Retorna uma lista paginada de eventos de calor.
        /// </summary>
        /// <param name="page">Número da página (padrão = 1).</param>
        /// <param name="size">Tamanho da página (padrão = 20).</param>
        /// <param name="sort">Campo de ordenação (padrão = "DataInicio").</param>
        /// <param name="filter">Filtro de busca (opcional).</param>
        /// <returns>Lista de eventos de calor com metadados de paginação.</returns>
        [HttpGet(Name = nameof(GetAll))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int size = 20,
            [FromQuery] string sort = "DataInicio",
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
                links = Url.Links(nameof(GetAll), new { page, size, sort, filter })
            });
        }

        /// <summary>
        /// Retorna um evento de calor pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do evento.</param>
        /// <returns>Detalhes do evento de calor.</returns>
        [HttpGet("{id}", Name = nameof(GetById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
                return NotFound(new ProblemDetails
                {
                    Type = "https://heatwatch.io/errors/not-found",
                    Title = "Evento de Calor não encontrado",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"Não existe evento de calor com id={id}",
                    Instance = HttpContext.Request.Path
                });

            Response.Headers.ETag =
                new EntityTagHeaderValue($"\"{dto.Id.GetHashCode()}\"").ToString();

            return Ok(new
            {
                data = dto,
                links = Url.Links(nameof(GetById), new { id })
            });
        }

        /// <summary>
        /// Cria um novo evento de calor.
        /// </summary>
        /// <param name="dto">Dados para criação do evento.</param>
        /// <returns>Dados do evento criado.</returns>
        [HttpPost(Name = nameof(Create))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateEventoCalorDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(
                actionName: nameof(GetById),
                routeValues: new { id = created.Id, version = HttpContext.GetRequestedApiVersion()?.ToString() },
                value: created);
        }

        /// <summary>
        /// Atualiza um evento de calor existente.
        /// </summary>
        /// <param name="id">Identificador do evento.</param>
        /// <param name="dto">Dados para atualização do evento.</param>
        [HttpPut("{id}", Name = nameof(Update))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEventoCalorDto dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Remove um evento de calor.
        /// </summary>
        /// <param name="id">Identificador do evento.</param>
        [HttpDelete("{id}", Name = nameof(Delete))]
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
