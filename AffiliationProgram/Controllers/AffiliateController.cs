using AffiliationProgram.Configs;
using AffiliationProgram.Models;
using AffiliationProgram.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AffiliationProgram.Controllers;

[ApiController]
[Route("api/affiliate")]
public sealed class AffiliateController(
    IAffiliateTrackingService trackingService,
    IOptions<AffiliateOptions> options) : ControllerBase
{
    [HttpPost("track")]
    [ProducesResponseType<TrackResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Track(TrackRequest request, CancellationToken ct)
    {
        var result = await trackingService.TrackAsync(request.Code, ct);

        return result.Status switch
        {
            TrackStatus.Success => Ok(new TrackResponse(
                result.TrackingId!.Value,
                DateTime.UtcNow.Add(options.Value.Expiration))),

            TrackStatus.NotFound => Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Affiliate code can not be found"),

            TrackStatus.Inactive => Problem(
                statusCode: StatusCodes.Status409Conflict,
                title: "Affiliate is no longer active. Try another one"),

            _ => Problem(statusCode: StatusCodes.Status500InternalServerError)
        };
    }
}