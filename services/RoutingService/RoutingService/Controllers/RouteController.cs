using Microsoft.AspNetCore.Mvc;
using RoutingService.Domain;
using RoutingService.Services;
using Route = RoutingService.Domain.Route;

namespace RoutingService.Controllers;

[ApiController]
[Route("api/routes")]
public class RoutesController : ControllerBase
{
    private readonly IRouteService _routeService;
    private readonly ILogger<RoutesController> _logger;

    public RoutesController(
        IRouteService routeService,
        ILogger<RoutesController> logger)
    {
        _routeService = routeService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<Route>> CalculateRoute(
        [FromBody] IReadOnlyList<Location> locations,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Route calculation request received with {LocationCount} locations",
            locations?.Count ?? 0);

        try
        {
            var route = await _routeService.CalculateRouteAsync(
                locations!,
                cancellationToken);

            _logger.LogInformation(
                "Route calculated successfully. Distance: {DistanceMeters} m, Duration: {DurationSeconds} seconds",
                route.DistanceMeters,
                route.DurationSeconds);

            return Ok(route);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(
                ex,
                "Invalid route calculation request");

            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation(
                "Route calculation request was cancelled");

            return new EmptyResult();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Route calculation request failed");

            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message = "Unable to calculate the route."
                });
        }
    }
}