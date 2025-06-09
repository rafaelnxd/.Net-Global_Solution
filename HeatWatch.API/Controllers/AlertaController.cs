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
    [Route("api/v{version:apiVersion}/alertas")]
    public class AlertaController : ControllerBase
    {
        private readonly IAlertaService _service;
        public AlertaController(IAlertaService service) => _service = service;

        /// <summary>
        /// Retorna uma lista paginada de alertas.
        /// </summary>
        /// <param name="page">Número da página (padrão = 1).</param>
        /// <param name="size">Tamanho da página (padrão = 20).</param>
        /// <param name="sort">Campo de ordenação (padrão = "DataEmissao").</param>
        /// <param name="filter">Filtro de busca (opcional).</param>
        /// <returns>Lista de alertas com metadados de paginação.</returns>
        [HttpGet(Name = "GetAllAlertas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int size = 20,
            [FromQuery] string sort = "DataEmissao",
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
                links = Url.Links("GetAllAlertas", new { page, size, sort, filter })
            });
        }

        /// <summary>
        /// Retorna um alerta pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do alerta.</param>
        /// <returns>Detalhes do alerta.</returns>
        [HttpGet("{id}", Name = "GetAlertaById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
                return NotFound(new ProblemDetails
                {
                    Type = "https://heatwatch.io/errors-not-found",
                    Title = "Alerta não encontrado",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"Não existe alerta com id={id}",
                    Instance = HttpContext.Request.Path
                });

            Response.Headers.ETag =
                new EntityTagHeaderValue($"\"{dto.Id.GetHashCode()}\"").ToString();

            return Ok(new
            {
                data = dto,
                links = Url.Links("GetAlertaById", new { id })
            });
        }

        /// <summary>
        /// Cria um novo alerta.
        /// </summary>
        /// <param name="dto">Dados para criação do alerta.</param>
        /// <returns>Dados do alerta criado.</returns>
        [HttpPost(Name = "CreateAlerta")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateAlertaDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(
                actionName: nameof(GetById),
                routeValues: new { id = created.Id, version = HttpContext.GetRequestedApiVersion()?.ToString() },
                value: created);
        }

        /// <summary>
        /// Atualiza um alerta existente.
        /// </summary>
        /// <param name="id">Identificador do alerta.</param>
        /// <param name="dto">Dados para atualização do alerta.</param>
        [HttpPut("{id}", Name = "UpdateAlerta")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAlertaDto dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Remove um alerta.
        /// </summary>
        /// <param name="id">Identificador do alerta.</param>
        [HttpDelete("{id}", Name = "DeleteAlerta")]
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
