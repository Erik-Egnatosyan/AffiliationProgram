using AffiliationProgram.Data;
using AffiliationProgram.Models;

namespace AffiliationProgram.Services;

public sealed class AffiliateTrackingService(
    IAffiliateRepository affiliates,
    IClickTrackingStore clickStore,
    ILogger<AffiliateTrackingService> logger) : IAffiliateTrackingService
{
    public async Task<TrackResult> TrackAsync(string code, CancellationToken ct)
    {
        var normalized = code.Trim();

        var affiliate = await affiliates.FindByCodeAsync(normalized, ct);

        if (affiliate is null)
        {
            logger.LogInformation("Unknown affiliate code received: {Code}", normalized);
            return TrackResult.NotFound();
        }

        if (!affiliate.IsActive)
        {
            logger.LogInformation("Inactive affiliate {AffiliateId} attempted tracking", affiliate.Id);
            return TrackResult.Inactive();
        }

        var trackingId = Guid.NewGuid();
        var click = new TrackedClick(affiliate.Id, affiliate.Code, DateTime.UtcNow);

        await clickStore.SaveAsync(trackingId, click, ct);

        return TrackResult.Success(trackingId);
    }
}