using System.ComponentModel.DataAnnotations;

namespace AffiliationProgram.Configs;

public sealed class AffiliateOptions
{
    public const string SectionName = "Affiliate";

    [Range(1, 365)] public int AffiliateDays { get; init; } = 7;

    public TimeSpan Expiration => TimeSpan.FromDays(AffiliateDays);
}