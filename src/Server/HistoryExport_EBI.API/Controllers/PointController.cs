using HistoryExport_EBI.Application.Common.Queries;
using HistoryExport_EBI.Application.Common.Commands;
using HistoryExport_EBI.Application.Dto;
using HistoryExport_EBI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HistoryExport_EBI.API.Controllers
{
    [ApiController]
    [Route($"api/{Route}/")]
    public class PointController : Controller
    {
        private readonly IPointsCommands _pointsCommands;
        private readonly IPointsQueries _pointsQueries;
        private const string Route = "points";

        public PointController(IPointsCommands pointsCommands, IPointsQueries pointsQueries)
        {
            _pointsCommands = pointsCommands;
            _pointsQueries = pointsQueries;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Task<IEnumerable<Point>>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Point>>> Get()
        {
            return Ok(await _pointsQueries.GetPointsAsync());
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePoints([FromBody] IEnumerable<UpdatePointsDto> items, CancellationToken ct)
        {

            if (items is null) return Problem(statusCode: 400, title: "Bad Request", detail: "Cuerpo inválido.");
            var list = items.ToList();
            if (list.Count == 0) return Problem(statusCode: 400, title: "Bad Request", detail: "Lista vacía.");

            var result = await _pointsCommands.UpdatePointsAsync(list, ct);

            if (result.UpdatedCount > 0 && result.FailedCount > 0)
                return StatusCode(207, result); // mezcla de éxitos y fallos

            return Ok(result);
        }
    }
}
