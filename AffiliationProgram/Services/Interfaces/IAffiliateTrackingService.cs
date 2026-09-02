using AffiliationProgram.Models;

namespace AffiliationProgram.Services;

public interface IAffiliateTrackingService
{
    Task<TrackResult> TrackAsync(string code, CancellationToken ct);
}