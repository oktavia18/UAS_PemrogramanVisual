using Microsoft.AspNetCore.Mvc;
using laundrycucikilat.Models;
using laundrycucikilat.Services;
using System.Text.Json;

namespace laundrycucikilat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new LoginResponse
                    {
                        Success = false,
                        Message = "Data input tidak valid"
                    });
                }

                var result = await _authService.LoginAsync(request);

                if (result.Success && result.User != null)
                {
                    // Create session
                    HttpContext.Session.SetString("UserId", result.User.Id!);
                    HttpContext.Session.SetString("UserRole", result.User.Role.ToString());
                    HttpContext.Session.SetString("UserName", result.User.FullName);
                    HttpContext.Session.SetString("UserEmail", result.User.Email);

                    // Set remember me cookie if requested
                    if (request.RememberMe)
                    {
                        var cookieOptions = new CookieOptions
                        {
                            Expires = DateTime.Now.AddDays(30),
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict
                        };
                        Response.Cookies.Append("RememberUser", result.User.Id!, cookieOptions);
                    }
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new LoginResponse
                {
                    Success = false,
                    Message = "Terjadi kesalahan sistem"
                });
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            try
            {
                // Clear session
                HttpContext.Session.Clear();

                // Clear remember me cookie
                Response.Cookies.Delete("RememberUser");

                return Ok(new { success = true, message = "Logout berhasil" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Terjadi kesalahan saat logout" });
            }
        }

        [HttpGet("check-session")]
        public async Task<IActionResult> CheckSession()
        {
            try
            {
                var userId = HttpContext.Session.GetString("UserId");
                
                if (string.IsNullOrEmpty(userId))
                {
                    // Check remember me cookie
                    if (Request.Cookies.TryGetValue("RememberUser", out var rememberedUserId))
                    {
                        var user = await _authService.GetUserByIdAsync(rememberedUserId);
                        if (user != null && user.IsActive)
                        {
                            // Restore session
                            HttpContext.Session.SetString("UserId", user.Id!);
                            HttpContext.Session.SetString("UserRole", user.Role.ToString());
                            HttpContext.Session.SetString("UserName", user.FullName);
                            HttpContext.Session.SetString("UserEmail", user.Email);

                            return Ok(new
                            {
                                isAuthenticated = true,
                                user = new
                                {
                                    id = user.Id,
                                    fullName = user.FullName,
                                    email = user.Email,
                                    role = user.Role.ToString()
                                }
                            });
                        }
                    }

                    return Ok(new { isAuthenticated = false });
                }

                var userRole = HttpContext.Session.GetString("UserRole");
                var userName = HttpContext.Session.GetString("UserName");
                var userEmail = HttpContext.Session.GetString("UserEmail");

                return Ok(new
                {
                    isAuthenticated = true,
                    user = new
                    {
                        id = userId,
                        fullName = userName,
                        email = userEmail,
                        role = userRole
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { isAuthenticated = false, message = "Terjadi kesalahan sistem" });
            }
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                var userId = HttpContext.Session.GetString("UserId");
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { success = false, message = "Sesi tidak valid" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "Data input tidak valid" });
                }

                var result = await _authService.ChangePasswordAsync(userId, request);

                if (result)
                {
                    return Ok(new { success = true, message = "Password berhasil diubah" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Password lama tidak cocok" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Terjadi kesalahan sistem" });
            }
        }

        [HttpGet("user-info")]
        public async Task<IActionResult> GetUserInfo()
        {
            try
            {
                var userId = HttpContext.Session.GetString("UserId");
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { success = false, message = "Sesi tidak valid" });
                }

                var user = await _authService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new { success = false, message = "User tidak ditemukan" });
                }

                return Ok(new
                {
                    success = true,
                    user = new
                    {
                        id = user.Id,
                        username = user.Username,
                        email = user.Email,
                        fullName = user.FullName,
                        role = user.Role.ToString(),
                        isActive = user.IsActive,
                        createdAt = user.CreatedAt,
                        lastLoginAt = user.LastLoginAt
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Terjadi kesalahan sistem" });
            }
        }
    }
}