using Identity.Application.Commands.Logout;
using Identity.Application.Interfaces;
using MediatR;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly IAuthService _authService;

    public LogoutCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var result = await _authService.LogoutAllDevicesAsync(request.UserId);

        if (!result.Success)
            throw new Exception(result.Error ?? "Erreur lors de la déconnexion");

        return Unit.Value;
    }
}
