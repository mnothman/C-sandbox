using TaskManagerSandbox.Data;
using TaskManagerSandbox.DTOs;
using TaskManagerSandbox.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;

namespace TaskManagerSandbox.Services;

public class AuthService : IAuthService
{
    private readonly TaskManagerContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(TaskManagerContext context, IConfiguration configuration, ILogger<AuthService> logger)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<AuthResult> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(registerDto.Username) || 
                string.IsNullOrWhiteSpace(registerDto.Email) || 
                string.IsNullOrWhiteSpace(registerDto.Password))
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Username, email, and password are required."
                };
            }

            // Check if passwords match
            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Passwords do not match."
                };
            }

            // Check if username already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == registerDto.Username);
            
            if (existingUser != null)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Username already exists."
                };
            }

            // Check if email already exists
            var existingEmail = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == registerDto.Email);
            
            if (existingEmail != null)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Email already exists."
                };
            }

            // Hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            // Create new user
            var newUser = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Role = UserRole.User
            };

            // Save to database
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Generate JWT token
            var token = GenerateJwtToken(newUser);
            var refreshToken = GenerateRefreshToken();

            // Update user with refresh token 
            // Maybe store this in separate table - for now just return it

            _logger.LogInformation("User registered successfully: {Username}", newUser.Username);

            return new AuthResult
            {
                Success = true,
                Token = token,
                RefreshToken = refreshToken,
                Message = "Registration successful.",
                User = new UserDto
                {
                    Id = newUser.Id,
                    Username = newUser.Username,
                    Email = newUser.Email,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    CreatedAt = newUser.CreatedAt,
                    IsActive = newUser.IsActive,
                    Role = newUser.Role.ToString(),
                    TaskCount = 0
                },
                ExpiresAt = DateTime.UtcNow.AddMinutes(GetJwtExpirationMinutes())
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user registration");
            return new AuthResult
            {
                Success = false,
                Message = "An error occurred during registration."
            };
        }
    }

    public async Task<AuthResult> LoginAsync(LoginDto loginDto)
    {
        try
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(loginDto.Username) || 
                string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Username and password are required."
                };
            }

            // Find user by username
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            if (user == null)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Invalid username or password."
                };
            }

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Invalid username or password."
                };
            }

            // Check if user is active
            if (!user.IsActive)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Account is deactivated."
                };
            }

            // Update last login
            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Generate JWT token
            var token = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            _logger.LogInformation("User logged in successfully: {Username}", user.Username);

            return new AuthResult
            {
                Success = true,
                Token = token,
                RefreshToken = refreshToken,
                Message = "Login successful.",
                User = new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    CreatedAt = user.CreatedAt,
                    LastLoginAt = user.LastLoginAt,
                    IsActive = user.IsActive,
                    Role = user.Role.ToString(),
                    Bio = user.Bio,
                    ProfilePictureUrl = user.ProfilePictureUrl,
                    TaskCount = user.Tasks.Count
                },
                ExpiresAt = DateTime.UtcNow.AddMinutes(GetJwtExpirationMinutes())
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user login");
            return new AuthResult
            {
                Success = false,
                Message = "An error occurred during login."
            };
        }
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(GetJwtSecretKey());

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = GetJwtIssuer(),
                ValidateAudience = true,
                ValidAudience = GetJwtAudience(),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<AuthResult> RefreshTokenAsync(string refreshToken)
    {
        // This is a simplified implementation
        // In prod grade we want to validate the refresh token against a real database -
        // - and generate new access token
        
        return new AuthResult
        {
            Success = false,
            Message = "Refresh token functionality not implemented yet."
        };
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(GetJwtSecretKey());

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("FirstName", user.FirstName),
            new Claim("LastName", user.LastName)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(GetJwtExpirationMinutes()),
            Issuer = GetJwtIssuer(),
            Audience = GetJwtAudience(),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string GetJwtSecretKey()
    {
        return _configuration["JwtSettings:SecretKey"] ?? 
               throw new InvalidOperationException("JWT Secret Key not configured");
    }

    private string GetJwtIssuer()
    {
        return _configuration["JwtSettings:Issuer"] ?? 
               throw new InvalidOperationException("JWT Issuer not configured");
    }

    private string GetJwtAudience()
    {
        return _configuration["JwtSettings:Audience"] ?? 
               throw new InvalidOperationException("JWT Audience not configured");
    }

    private int GetJwtExpirationMinutes()
    {
        var expiration = _configuration["JwtSettings:ExpirationInMinutes"];
        return int.TryParse(expiration, out var minutes) ? minutes : 60;
    }
}