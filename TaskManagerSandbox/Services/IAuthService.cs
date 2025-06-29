using TaskManagerSandbox.DTOs;

namespace TaskManagerSandbox.Services;

public interface IAuthService
{
    // Register & Login
    Task<AuthResult> RegisterAsync(RegisterDto registerDto);
    Task<AuthResult> LoginAsync(LoginDto loginDto);

    // Useful but optional
    Task<bool> ValidateTokenAsync(string token);
    Task<AuthResult> RefreshTokenAsync(string refreshToken);
}