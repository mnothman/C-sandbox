using Microsoft.AspNetCore.Mvc;
using TaskManagerSandbox.DTOs;
using TaskManagerSandbox.Services;

namespace TaskManagerSandbox.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Get all users
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    /// <summary>
    /// Get a specific user by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound(new { Message = $"User with ID {id} not found" });
        }

        return Ok(user);
    }

    /// <summary>
    /// Get a user by username
    /// </summary>
    [HttpGet("username/{username}")]
    public async Task<ActionResult<UserDto>> GetUserByUsername(string username)
    {
        var user = await _userService.GetUserByUsernameAsync(username);
        if (user == null)
        {
            return NotFound(new { Message = $"User with username '{username}' not found" });
        }

        return Ok(user);
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
    {
        try
        {
            var user = await _userService.CreateUserAsync(createUserDto);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
            return StatusCode(500, new { Message = "An error occurred while creating the user" });
        }
    }

    /// <summary>
    /// Update an existing user
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> UpdateUser(int id, UpdateUserDto updateUserDto)
    {
        var user = await _userService.UpdateUserAsync(id, updateUserDto);
        if (user == null)
        {
            return NotFound(new { Message = $"User with ID {id} not found" });
        }

        return Ok(user);
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var success = await _userService.DeleteUserAsync(id);
        if (!success)
        {
            return NotFound(new { Message = $"User with ID {id} not found" });
        }

        return NoContent();
    }

    /// <summary>
    /// Deactivate a user
    /// </summary>
    [HttpPatch("{id}/deactivate")]
    public async Task<ActionResult> DeactivateUser(int id)
    {
        var success = await _userService.DeactivateUserAsync(id);
        if (!success)
        {
            return NotFound(new { Message = $"User with ID {id} not found" });
        }

        return Ok(new { Message = "User deactivated successfully" });
    }

    /// <summary>
    /// Activate a user
    /// </summary>
    [HttpPatch("{id}/activate")]
    public async Task<ActionResult> ActivateUser(int id)
    {
        var success = await _userService.ActivateUserAsync(id);
        if (!success)
        {
            return NotFound(new { Message = $"User with ID {id} not found" });
        }

        return Ok(new { Message = "User activated successfully" });
    }
} 