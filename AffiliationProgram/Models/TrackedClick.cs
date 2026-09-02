namespace AffiliationProgram.Models;

public sealed record TrackedClick(long AffiliateId, string Code, DateTime ClickedAt);