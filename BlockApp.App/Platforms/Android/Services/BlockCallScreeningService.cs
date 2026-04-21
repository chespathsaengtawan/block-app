using Android.App;
using Android.Telecom;
using BlockApp.App.Services;

namespace BlockApp.App.Platforms.Android.Services;

[Service(
    Name = "com.chespathsaengtawan.blockapp.app.BlockCallScreeningService",
    Permission = "android.permission.BIND_SCREENING_SERVICE",
    Exported = true)]
[IntentFilter(new[] { "android.telecom.CallScreeningService" })]
[global::System.Runtime.Versioning.SupportedOSPlatform("android24.0")]
public class BlockCallScreeningService : CallScreeningService
{
    private const string NotificationChannelId = "blocked_calls";

    public override void OnScreenCall(Call.Details callDetails)
    {
        var rawNumber = callDetails.GetHandle()?.SchemeSpecificPart ?? string.Empty;

        // Remove tel: prefix if present
        if (rawNumber.StartsWith("tel:", StringComparison.OrdinalIgnoreCase))
            rawNumber = rawNumber[4..];

        bool shouldBlock = IsBlocked(rawNumber);

        //var response = new CallScreeningService.CallResponse.Builder()
        //    .SetDisallowCall(shouldBlock)
        //    .SetRejectCall(shouldBlock)
        //    .SetSilenceCall(shouldBlock)
        //    .Build();

        //RespondToCall(callDetails, response);

        var builder = new CallScreeningService.CallResponse.Builder()
            .SetDisallowCall(shouldBlock)!
            .SetRejectCall(shouldBlock)!;
        
        if (OperatingSystem.IsAndroidVersionAtLeast(29))
            builder.SetSilenceCall(shouldBlock);
        
        var response = builder.Build();
        RespondToCall(callDetails, response!);
        
        if (shouldBlock)
            ShowBlockedNotification(rawNumber);
    }

    private static bool IsBlocked(string rawNumber)
    {
        // Prefer DI-managed service (works when MAUI app is initialized)
        var cache = IPlatformApplication.Current?.Services.GetService<BlocklistCacheService>();
        if (cache != null)
            return cache.IsBlocked(rawNumber);

        // Direct fallback via Preferences (available as long as MainApplication ran OnCreate)
        var normalized = BlocklistCacheService.Normalize(rawNumber);
        if (string.IsNullOrEmpty(normalized)) return false;
        var json = Microsoft.Maui.Storage.Preferences.Default
            .Get(BlocklistCacheService.NumbersKey, "[]");
        var numbers = System.Text.Json.JsonSerializer
            .Deserialize<List<string>>(json) ?? [];
        return numbers.Contains(normalized);
    }

    private void ShowBlockedNotification(string number)
    {
        if (!OperatingSystem.IsAndroidVersionAtLeast(26)) return;

        var manager = GetSystemService(NotificationService) as NotificationManager;
        if (manager == null) return;

        var channel = new NotificationChannel(
            NotificationChannelId,
            "สายที่ถูกบล็อก",
            NotificationImportance.Default)
        {
            Description = "แจ้งเตือนเมื่อมีสายจากเบอร์ที่บล็อก"
        };
        manager.CreateNotificationChannel(channel);

        var notification = new Notification.Builder(this, NotificationChannelId)
            .SetContentTitle("บล็อกสายสำเร็จ")
            .SetContentText($"บล็อกสายจาก {number}")
            .SetSmallIcon(global::Android.Resource.Drawable.IcDialogInfo)
            .SetAutoCancel(true)
            .Build();

        manager.Notify(number.GetHashCode() & 0x7FFFFFFF, notification);
    }
}
