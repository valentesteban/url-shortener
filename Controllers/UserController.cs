using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using url_shortener.Data.Interfaces;
using url_shortener.Entities;
using url_shortener.Models;
using url_shortener.Utilities;

namespace url_shortener.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userContext;
    private readonly ErrorSwitcher _errorSwitcher;
    private readonly IAuthService _authService;

    public UserController(IUserService _userContext, ErrorSwitcher _errorSwitcher, IAuthService _authService)
    {
        this._userContext = _userContext;
        this._errorSwitcher = _errorSwitcher;
        this._authService = _authService;
    }

    [Route("all")]
    [Authorize(Roles = "admin")]
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_userContext.GetUsers());
    }

    [HttpGet("urls/{userId}")]
    public IActionResult GetUserUrls(int userId)
    {
        try
        {
            if (_authService.getCurrentUser() == null)
            {
                return Unauthorized();
            }

            if (_authService.getCurrentUser().Id == userId || _authService.getCurrentUser().Role == "admin")
            {
                var urls = _userContext.GetUrls(userId);
                return Ok(urls);
            }

            return Unauthorized();
        }
        catch (Exception e)
        {
            var message = _errorSwitcher.GetErrorFromException(e.Message);

            var code = message.Item1;
            var msg = message.Item2;

            return _errorSwitcher.getResultFromError(code, msg);
        }
    }


    [HttpGet("{userId}")]
    public ActionResult<User> GetUser(int userId)
    {
        try
        {
            if (_authService.getCurrentUser() == null)
            {
                return Unauthorized();
            }

            if (_authService.getCurrentUser().Id == userId || _authService.getCurrentUser().Role == "admin")
            {
                return Ok(_userContext.GetUser(userId));
            }

            return Unauthorized();
        }
        catch (Exception e)
        {
            var message = _errorSwitcher.GetErrorFromException(e.Message);

            var code = message.Item1;
            var msg = message.Item2;

            return _errorSwitcher.getResultFromError(code, msg);
        }
    }

    [HttpGet("{userId}/limit-url")]
    public IActionResult GetUserLimitUrl(int userId)
    {
        try
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (_authService.getCurrentUser() == null)
            {
                return Unauthorized();
            }
            
            return Ok(_userContext.GetUserLimitUrl(userId));
        }
        catch (Exception e)
        {
            var message = _errorSwitcher.GetErrorFromException(e.Message);

            var code = message.Item1;
            var msg = message.Item2;

            return _errorSwitcher.getResultFromError(code, msg);
        }
    }

    [HttpPost]
    public ActionResult<User> PostUser(UserForCreationDTO userForCreationDto)
    {
        try
        {
            _userContext.AddUser(userForCreationDto);
            return Ok("User created successfully");
        }
        catch (Exception e)
        {
            var message = _errorSwitcher.GetErrorFromException(e.Message);

            var code = message.Item1;
            var msg = message.Item2;

            return _errorSwitcher.getResultFromError(code, msg);
        }
    }

    [HttpPut]
    [Authorize(Roles = "admin")]
    public ActionResult<User> PutUser(UserForUpdateDTO userForUpdateDto)
    {
        try
        {
            _userContext.UpdateUser(userForUpdateDto);
            return Ok("User updated successfully");
        }
        catch (Exception e)
        {
            var message = _errorSwitcher.GetErrorFromException(e.Message);

            var code = message.Item1;
            var msg = message.Item2;

            return _errorSwitcher.getResultFromError(code, msg);
        }
    }

    [HttpDelete("{userId}")]
    [Authorize(Roles = "admin")]
    public ActionResult<User> DeleteUser(int userId)
    {
        try
        {
            _userContext.DeleteUser(userId);
            return Ok("User deleted successfully");
        }
        catch (Exception e)
        {
            var message = _errorSwitcher.GetErrorFromException(e.Message);

            var code = message.Item1;
            var msg = message.Item2;

            return _errorSwitcher.getResultFromError(code, msg);
        }
    }
    
    [HttpPut ("{userId}/reset-url")]
    [Authorize(Roles = "admin")]
    public ActionResult<User> ResetUserLimitUrl(int userId)
    {
        try
        {
            _userContext.ResetUserLimitUrl(userId);
            return Ok("User updated successfully");
        }
        catch (Exception e)
        {
            var message = _errorSwitcher.GetErrorFromException(e.Message);

            var code = message.Item1;
            var msg = message.Item2;

            return _errorSwitcher.getResultFromError(code, msg);
        }
    }
}