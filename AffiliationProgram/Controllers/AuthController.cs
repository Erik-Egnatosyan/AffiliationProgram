using AffiliationProgram.Models;
using AffiliationProgram.Services;
using Microsoft.AspNetCore.Mvc;

namespace AffiliationProgram.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IRegistrationService registrationService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken ct)
    {
        var outcome = await registrationService.RegisterAsync(request, ct);

        if (outcome.Status == RegistrationStatus.Created)
            return Created($"/api/users/{outcome.UserId}",
                new RegisterResponse(outcome.UserId!.Value, outcome.Attributed));

        if (outcome.Status == RegistrationStatus.EmailTaken)
            return Problem(statusCode: StatusCodes.Status409Conflict,
                title: "Email is already registered. Please try another one");

        return Problem(statusCode: StatusCodes.Status500InternalServerError,
            title: "Registration failed. Please try again later");
    }
}