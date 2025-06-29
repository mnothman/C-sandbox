using TaskManagerSandbox.Data;
using TaskManagerSandbox.DTOs;
using TaskManagerSandbox.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace TaskManagerSandbox.Services;

public class UserService : IUserService
{
    private readonly TaskManagerContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    public UserService(TaskManagerContext context, IMapper mapper, ILogger<UserService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _context.Users
            .Include(u => u.Tasks)
            .OrderBy(u => u.Username)
            .ToListAsync();

        var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
        
        // Set task count for each user
        foreach (var userDto in userDtos)
        {
            var user = users.First(u => u.Id == userDto.Id);
            userDto.TaskCount = user.Tasks.Count;
        }

        return userDtos;
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _context.Users
            .Include(u => u.Tasks)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return null;

        var userDto = _mapper.Map<UserDto>(user);
        userDto.TaskCount = user.Tasks.Count;
        return userDto;
    }

    public async Task<UserDto?> GetUserByUsernameAsync(string username)
    {
        var user = await _context.Users
            .Include(u => u.Tasks)
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
            return null;

        var userDto = _mapper.Map<UserDto>(user);
        userDto.TaskCount = user.Tasks.Count;
        return userDto;
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        // Check if username or email already exists
        if (await _context.Users.AnyAsync(u => u.Username == createUserDto.Username))
        {
            throw new InvalidOperationException("Username already exists");
        }

        if (await _context.Users.AnyAsync(u => u.Email == createUserDto.Email))
        {
            throw new InvalidOperationException("Email already exists");
        }

        var user = new User
        {
            Username = createUserDto.Username,
            Email = createUserDto.Email,
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            PhoneNumber = createUserDto.PhoneNumber,
            Bio = createUserDto.Bio,
            Role = Enum.Parse<UserRole>(createUserDto.Role, true),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        _logger.LogInformation("User created: {UserId} - {Username}", user.Id, user.Username);

        return await GetUserByIdAsync(user.Id) ?? throw new InvalidOperationException("Failed to retrieve created user");
    }

    public async Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return null;
        }

        // Update properties if provided
        if (!string.IsNullOrEmpty(updateUserDto.FirstName))
            user.FirstName = updateUserDto.FirstName;

        if (!string.IsNullOrEmpty(updateUserDto.LastName))
            user.LastName = updateUserDto.LastName;

        if (updateUserDto.PhoneNumber != null)
            user.PhoneNumber = updateUserDto.PhoneNumber;

        if (updateUserDto.Bio != null)
            user.Bio = updateUserDto.Bio;

        if (updateUserDto.ProfilePictureUrl != null)
            user.ProfilePictureUrl = updateUserDto.ProfilePictureUrl;

        if (updateUserDto.IsActive.HasValue)
            user.IsActive = updateUserDto.IsActive.Value;

        await _context.SaveChangesAsync();

        _logger.LogInformation("User updated: {UserId}", id);

        return await GetUserByIdAsync(id);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return false;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        _logger.LogInformation("User deleted: {UserId}", id);
        return true;
    }

    public async Task<bool> DeactivateUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return false;
        }

        user.IsActive = false;
        await _context.SaveChangesAsync();

        _logger.LogInformation("User deactivated: {UserId}", id);
        return true;
    }

    public async Task<bool> ActivateUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return false;
        }

        user.IsActive = true;
        await _context.SaveChangesAsync();

        _logger.LogInformation("User activated: {UserId}", id);
        return true;
    }
} 