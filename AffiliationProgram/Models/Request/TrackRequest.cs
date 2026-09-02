using System.ComponentModel.DataAnnotations;

namespace AffiliationProgram.Models;

public sealed record TrackRequest
{
    [Required]
    public required string Code { get; init; }
}