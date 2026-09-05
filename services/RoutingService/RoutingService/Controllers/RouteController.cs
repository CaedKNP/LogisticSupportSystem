using Microsoft.AspNetCore.Mvc;
using RoutingService.Domain;
using RoutingService.Services;

namespace RoutingService.Controllers;

[ApiController]
[Route("api/routes")]
public class RoutesController : ControllerBase
{
    private readonly IRouteService _routeService;

    public RoutesController(IRouteService routeService)
    {
        _routeService = routeService;
    }

    [HttpPost]
    public async Task<ActionResult<Route>> CalculateRoute(
        [FromBody] IReadOnlyList<Location> locations,
        CancellationToken cancellationToken)
    {
        var route = await _routeService.CalculateRouteAsync(
            locations,
            cancellationToken);

        return Ok(route);
    }
}