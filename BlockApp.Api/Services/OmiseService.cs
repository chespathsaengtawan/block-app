using Omise;
using Omise.Models;
using Svg.Skia;
using SkiaSharp;
using System.Net.Http.Headers;
using System.Text;
using BlockApp.Shared.DTOs.Payment;
using BlockApp.Api.Services.Interfaces;

namespace BlockApp.Api.Services;

public class OmiseService : IOmiseService
{
    private readonly Client _client;
    private readonly ILogger<OmiseService> _logger;
    private readonly string _secretKey;

    public OmiseService(IConfiguration configuration, ILogger<OmiseService> logger)
    {
        var publicKey = configuration["Omise:PublicKey"];
        _secretKey = configuration["Omise:SecretKey"] ?? string.Empty;
        if (string.IsNullOrWhiteSpace(publicKey)) throw new InvalidOperationException("Omise PublicKey not configured");
        if (string.IsNullOrWhiteSpace(_secretKey)) throw new InvalidOperationException("Omise SecretKey not configured");

        _client = new Client(publicKey, _secretKey);
        _logger = logger;
    }

    public async Task<CreateChargeResult> CreatePromptPayChargeAsync(decimal amount, string description)
    {
        try
        {
            // Step 1: Create a source (PromptPay)
            var source = await _client.Sources.Create(new CreatePaymentSourceRequest
            {
                Amount = (long)(amount * 100), // Convert to satang
                Currency = "thb",
                Type = OffsiteTypes.PromptPay,
                Flow = FlowTypes.Offline,
                PlatformType = PlatformTypes.Web
            });

            _logger.LogInformation("Created Omise source: {SourceId}", source.Id);

            // Step 2: Create a charge using the source ID (PromptPay offline — no ReturnUri)
            var charge = await _client.Charges.Create(new CreateChargeRequest
            {
                Amount = (long)(amount * 100),
                Currency = "thb",
                Source = source,
                Description = description
            });

            _logger.LogInformation("Created Omise charge: {ChargeId}", charge.Id);

            // Download SVG from Omise and convert to PNG
            byte[]? qrImageBytes = null;
            var downloadUri = charge.Source?.ScannableCode?.Image?.DownloadURI
                           ?? source.ScannableCode?.Image?.DownloadURI;

            if (!string.IsNullOrEmpty(downloadUri))
            {
                var svgBytes = await DownloadWithAuthAsync(downloadUri);
                if (svgBytes != null)
                {
                    qrImageBytes = ConvertSvgToPng(svgBytes);
                    _logger.LogInformation("QR PNG converted from SVG, size: {Size} bytes", qrImageBytes?.Length ?? 0);
                }
            }
            else
            {
                _logger.LogWarning("No QR download URI found on source or charge");
            }

            return new CreateChargeResult
            {
                ChargeId = charge.Id,
                SourceId = source.Id,
                QrImageBytes = qrImageBytes,
                Status = charge.Status.ToString(),
                ExpiresAt = source.CreatedAt.AddMinutes(15)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating PromptPay charge");
            throw;
        }
    }

    public async Task<Charge> GetChargeStatusAsync(string chargeId)
    {
        try
        {
            var charge = await _client.Charges.Get(chargeId);
            return charge;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting charge status for {ChargeId}", chargeId);
            throw;
        }
    }

    private async Task<byte[]?> DownloadWithAuthAsync(string uri)
    {
        try
        {
            using var http = new HttpClient();
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_secretKey}:"));
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            return await http.GetByteArrayAsync(uri);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to download from {Uri}", uri);
            return null;
        }
    }

    private byte[]? ConvertSvgToPng(byte[] svgBytes)
    {
        try
        {
            using var stream = new MemoryStream(svgBytes);
            using var svg = new SKSvg();
            var picture = svg.Load(stream);
            if (picture == null) return null;

            var bounds = picture.CullRect;
            int width = (int)Math.Ceiling(bounds.Width);
            int height = (int)Math.Ceiling(bounds.Height);

            var imageInfo = new SKImageInfo(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
            using var surface = SKSurface.Create(imageInfo);
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.White);
            canvas.DrawPicture(picture);
            canvas.Flush();

            using var image = surface.Snapshot();
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            return data.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to convert SVG to PNG");
            return null;
        }
    }
}
