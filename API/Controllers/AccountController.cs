
using System.Security.Claims;
using API.DTOs;
using API.Extensions;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(SignInManager<AppUser> signInManager) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var user = new AppUser
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            UserName = registerDto.Email
        };

        var result = await signInManager.UserManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem();
        }

        return Ok();
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync();

        return NoContent();
    }

    [HttpGet("user-info")]
    public async Task<ActionResult> GetUserInfo()
    {
        if (User.Identity?.IsAuthenticated == false) return NoContent();

        var user = await signInManager.UserManager.GetUserByEmailWithAddress(User);
        if (user == null) return Unauthorized();

        return Ok(new
        {
            user.FirstName,
            user.LastName,
            user.Email,
            Address = user.Address?.ToDto(),
            Roles = User.FindFirstValue(ClaimTypes.Role)
        });
    }

    [HttpGet("auth-status")]
    public ActionResult GetAuthState()
    {
        return Ok(new { IsAuthenticated = User.Identity?.IsAuthenticated ?? false });
    }

    [Authorize]
    [HttpPost("address")]
    public async Task<ActionResult<Address>> CreateOrUpdateAddress(AddressDto addressDto)
    {
        var user = await signInManager.UserManager.GetUserByEmail(User);

        if (user.Address == null)
        {
            user.Address = addressDto.ToEntity();
        }
        else
        {
            user.Address.UpdateFromDto(addressDto);
        }

        var result = await signInManager.UserManager.UpdateAsync(user);

        if (!result.Succeeded) return BadRequest("Problem updating user address");

        return Ok(user.Address.ToDto());
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);


        var users = await signInManager.UserManager.Users
            .Where(u => u.Id != currentUserId)
            .Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName!,
                LastName = u.LastName!,
                Email = u.Email!,
                LockoutEnd = u.LockoutEnd
            })
            .ToListAsync();

        return Ok(users);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id}/lock")]
    public async Task<ActionResult> LockUser(string id, [FromQuery] int days = 30)
    {
        var user = await signInManager.UserManager.FindByIdAsync(id);

        if (user == null) return NotFound();

        await signInManager.UserManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddDays(days));

        await signInManager.UserManager.SetLockoutEnabledAsync(user, true);

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id}/unlock")]
    public async Task<ActionResult> UnlockUser(string id)
    {
        var user = await signInManager.UserManager.FindByIdAsync(id);

        if (user == null) return NotFound();

        await signInManager.UserManager.SetLockoutEndDateAsync(user, null);

        return NoContent();
    }
}
