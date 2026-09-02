namespace AffiliationProgram.Models;

public enum TrackStatus
{
    Success,
    NotFound,
    Inactive
}
public sealed record TrackResult(TrackStatus Status, Guid? TrackingId)
{
    public static TrackResult Success(Guid trackingId)
    {
        return new TrackResult(TrackStatus.Success, trackingId);
    }

    public static TrackResult NotFound()
    {
        return new TrackResult(TrackStatus.NotFound, null);
    }

    public static TrackResult Inactive()
    {
        return new TrackResult(TrackStatus.Inactive, null);
    }
}