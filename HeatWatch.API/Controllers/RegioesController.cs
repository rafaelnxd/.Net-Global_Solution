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
    [Route("api/v{version:apiVersion}/regioes")]
    public class RegioesController : ControllerBase
    {
        private readonly IRegiaoService _service;

        public RegioesController(IRegiaoService service)
            => _service = service;

        /// <summary>
        /// Retorna uma lista paginada de regiões.
        /// </summary>
        [HttpGet(Name = "GetAllRegioes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int size = 20,
            [FromQuery] string sort = "Nome",
            [FromQuery] string filter = null)
        {
            var pagedResult = await _service.GetAllAsync(page, size, sort, filter);
            Response.Headers.Add("X-Total-Count", pagedResult.TotalCount.ToString());
            Response.GetTypedHeaders().CacheControl =
                new CacheControlHeaderValue { Public = true, MaxAge = TimeSpan.FromMinutes(5) };

            return Ok(new
            {
                data = pagedResult.Items,
                page = pagedResult.Page,
                size = pagedResult.PageSize,
                total = pagedResult.TotalCount,
                links = Url.Links("GetAllRegioes", new { page, size, sort, filter })
            });
        }

        /// <summary>
        /// Retorna uma região pelo identificador.
        /// </summary>
        [HttpGet("{id}", Name = "GetRegiaoById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
                return NotFound(new ProblemDetails
                {
                    Type = "https://heatwatch.io/errors-not-found",
                    Title = "Região não encontrada",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"Não existe região com id={id}",
                    Instance = HttpContext.Request.Path
                });

            Response.Headers.ETag =
                new EntityTagHeaderValue($"\"{dto.Id.GetHashCode()}\"").ToString();

            return Ok(new
            {
                data = dto,
                links = Url.Links("GetRegiaoById", new { id })
            });
        }

        /// <summary>
        /// Cria uma nova região.
        /// </summary>
        [HttpPost(Name = "CreateRegiao")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateRegiaoDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id, version = HttpContext.GetRequestedApiVersion()?.ToString() },
                created);
        }

        /// <summary>
        /// Atualiza uma região existente.
        /// </summary>
        [HttpPut("{id}", Name = "UpdateRegiao")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRegiaoDto dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Remove uma região.
        /// </summary>
        [HttpDelete("{id}", Name = "DeleteRegiao")]
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
