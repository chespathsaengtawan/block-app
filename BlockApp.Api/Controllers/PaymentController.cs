using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlockApp.Api.Services.Interfaces;
using BlockApp.Shared.DTOs.Payment;
using System.Security.Claims;

namespace BlockApp.Api.Controllers
{
    [ApiController]
    [Route("blockapp/[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpGet("packages")]
        public async Task<IActionResult> GetPackages()
        {
            var packages = await _paymentService.GetPointsPackagesAsync();
            return Ok(packages);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { Message = "Invalid user token" });
                }

                if (dto.Amount <= 0)
                {
                    return BadRequest(new { Message = "Amount must be greater than 0" });
                }

                var result = await _paymentService.CreatePaymentAsync(userId, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payment");
                return StatusCode(500, new { Message = "Error creating payment", Error = ex.Message });
            }
        }

        [HttpGet("status/{paymentId}")]
        public async Task<IActionResult> CheckPaymentStatus(int paymentId)
        {
            try
            {
                var result = await _paymentService.CheckPaymentStatusAsync(paymentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking payment status");
                return StatusCode(500, new { Message = "Error checking payment status", Error = ex.Message });
            }
        }

        [HttpPost("webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> OmiseWebhook([FromBody] OmiseWebhookDto webhook)
        {
            try
            {
                // Verify webhook signature (Production)
                var signature = Request.Headers["X-Omise-Signature"].FirstOrDefault();
                if (!string.IsNullOrEmpty(signature))
                {
                    // TODO: Verify signature using Omise webhook signing key
                    // var isValid = VerifyOmiseSignature(signature, Request.Body);
                    // if (!isValid) return Unauthorized();
                }
                
                _logger.LogInformation("Received Omise webhook: {Key}, ChargeId: {ChargeId}, Status: {Status}", 
                    webhook.Key, webhook.Data?.Id, webhook.Data?.Status);
                
                if (webhook.Key == "charge.complete" && webhook.Data != null)
                {
                    var processed = await _paymentService.ProcessWebhookAsync(
                        webhook.Data.Id, 
                        webhook.Data.Status);

                    if (processed)
                    {
                        _logger.LogInformation("Webhook processed successfully for charge {ChargeId}", webhook.Data.Id);
                        return Ok(new { Message = "Webhook processed successfully" });
                    }
                }

                return Ok(new { Message = "Webhook received" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing webhook");
                return StatusCode(500, new { Message = "Error processing webhook" });
            }
        }
    }

    public class OmiseWebhookDto
    {
        public string Key { get; set; } = string.Empty;
        public WebhookData? Data { get; set; }
    }

    public class WebhookData
    {
        public string Id { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
