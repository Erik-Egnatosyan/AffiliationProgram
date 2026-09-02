using AffiliationProgram.Models;

namespace AffiliationProgram.Data;

public interface IClickTrackingStore
{
    Task SaveAsync(Guid trackingId, TrackedClick click, CancellationToken ct);
    Task<TrackedClick?> GetAsync(Guid trackingId, CancellationToken ct);
    Task RemoveAsync(Guid trackingId, CancellationToken ct);
}