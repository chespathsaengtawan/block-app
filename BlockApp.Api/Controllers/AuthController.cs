using BlockApp.Api.Data;
using BlockApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlockApp.Api.Services;
using BlockApp.Shared.DTOs.Auth;
using BlockApp.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlockApp.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IOtpService _otpService;
        private readonly JwtService _jwt;
        private readonly AppDbContext _db;

        public AuthController(IOtpService otpService, JwtService jwt, AppDbContext db)
        {
            _otpService = otpService;
            _jwt = jwt;
            _db = db;
        }

        /// <summary>
        /// Request OTP for a given phone number and SMS provider
        /// </summary>
        /// <remarks>
        /// - ThaibulkSMS: providerToken is required for verification (contains OTP reference)
        /// - Twilio: providerToken is not needed (Twilio Verify handles it internally)
        /// </remarks>
        /// <returns>Provider-specific token (if applicable) to be used in OTP verification</returns>
        /// <response code="200">OTP sent successfully</response>
        /// <response code="400">Too many OTP requests or invalid input</response>
        /// <response code="500">Internal server error</response>
        /// <example>
        /// POST /api/auth/request-otp
        /// {
        ///  "phoneNumber": "+66123456789",
        ///  "fromService": "ThaibulkSMS"
        /// }
        /// Response:
        /// {
        /// "message": "OTP sent",
        /// "fromService": "ThaibulkSMS",
        /// "providerToken": "abc123" // Required for ThaibulkSMS verification
        /// }
        /// </example>
        [HttpPost("request-otp")]
        public async Task<IActionResult> RequestOtp(RequestOtpDto dto)
        {
            var providerToken = await _otpService.RequestOtpAsync(dto.PhoneNumber, dto.FromService);

            return Ok(new RequestOtpResultDto
            {
                Message = "OTP sent",
                FromService = dto.FromService,
                ProviderToken = string.IsNullOrEmpty(providerToken) ? null : providerToken
            });
        }

        /// <summary>
        /// Verify OTP and issue JWT access & refresh tokens
        /// </summary>
        /// <remarks>
        /// - For ThaibulkSMS, providerToken from the request-otp response must be included for verification.
        /// - For Twilio, providerToken is not needed.
        /// </remarks>
        /// <returns>Access token, refresh token, and their expiry times</returns>
        /// <response code="200">OTP verified and tokens issued</response>
        /// <response code="400">Invalid OTP or missing provider token</response>
        /// <response code="500">Internal server error</response>
        /// <example>
        /// POST /api/auth/verify-otp
        /// {
        /// "phoneNumber": "+66123456789",
        /// "code": "123456",
        /// "fromService": "ThaibulkSMS",
        /// "providerToken": "abc123" // Required for ThaibulkSMS verification
        /// }
        /// Response:
        /// {
        /// "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        /// "expiresAt": "2024-07-01T12:00:00Z",
        /// "refreshToken": "def456",
        /// "refreshTokenExpiresAt": "2024-07-31T12:00:00Z"
        /// }
        /// </example>
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpDto dto)
        {
            var user = await _otpService.VerifyOtpAsync(dto.PhoneNumber, dto.Code, dto.FromService, dto.ProviderToken);

            var accessToken = _jwt.GenerateToken(user);
            var refreshTokenValue = _jwt.GenerateRefreshToken();
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(JwtService.RefreshTokenDays);

            _db.RefreshTokens.Add(new RefreshToken
            {
                UserId = user.Id,
                Token = refreshTokenValue,
                ExpiresAt = refreshTokenExpiry,
                CreatedAt = DateTime.UtcNow
            });
            await _db.SaveChangesAsync();

            return Ok(new AuthResultDto
            {
                AccessToken = accessToken,
                ExpiresAt = DateTime.UtcNow.AddDays(JwtService.AccessTokenDays),
                RefreshToken = refreshTokenValue,
                RefreshTokenExpiresAt = refreshTokenExpiry
            });
        }

        /// <summary>
        /// Refresh access token using a valid refresh token
        /// </summary>
        /// <returns>New access token, new refresh token, and their expiry times</returns> 
        /// <response code="200">Tokens refreshed successfully</response>
        /// <response code="401">Invalid or expired refresh token</response>
        /// <response code="500">Internal server error</response>
        /// <example>
        /// POST /api/auth/refresh
        /// {
        /// "refreshToken": "def456"
        /// }
        /// Response:
        /// {
        /// "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        /// "expiresAt": "2024-07-01T12:00:00Z",
        /// "refreshToken": "ghi789",
        /// "refreshTokenExpiresAt": "2024-07-31T12:00:00Z"
        /// }
        /// </example>
        /// <remarks>
        /// - The old refresh token is revoked and cannot be used again (refresh token rotation).
        /// - A new refresh token is issued alongside the new access token.
        /// - Clients should replace the old refresh token with the new one to maintain session continuity.
        /// </remarks>
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenDto dto)
        {
            var stored = await _db.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == dto.RefreshToken);

            if (stored is null || stored.IsRevoked || stored.ExpiresAt <= DateTime.UtcNow)
                return Unauthorized(new { message = "Invalid or expired refresh token" });

            // Rotate: revoke old, issue new
            stored.IsRevoked = true;

            var newRefreshTokenValue = _jwt.GenerateRefreshToken();
            var newRefreshTokenExpiry = DateTime.UtcNow.AddDays(JwtService.RefreshTokenDays);

            _db.RefreshTokens.Add(new RefreshToken
            {
                UserId = stored.UserId,
                Token = newRefreshTokenValue,
                ExpiresAt = newRefreshTokenExpiry,
                CreatedAt = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();

            var newAccessToken = _jwt.GenerateToken(stored.User!);

            return Ok(new AuthResultDto
            {
                AccessToken = newAccessToken,
                ExpiresAt = DateTime.UtcNow.AddDays(JwtService.AccessTokenDays),
                RefreshToken = newRefreshTokenValue,
                RefreshTokenExpiresAt = newRefreshTokenExpiry
            });
        }

        [HttpPost("check-phone")]
        public async Task<IActionResult> CheckPhone(CheckPhoneDto dto)
        {
            var exists = await _db.Users.AnyAsync(u => u.PhoneNumber == dto.PhoneNumber);
            return Ok(new CheckPhoneResultDto { Exists = exists });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var sub = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                   ?? User.FindFirst("sub")?.Value;

            if (sub is null || !int.TryParse(sub, out var userId))
                return Unauthorized();

            var user = await _db.Users.FindAsync(userId);
            if (user is null)
                return NotFound();

            return Ok(new UserProfileDto
            {
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            });
        }
    }
}