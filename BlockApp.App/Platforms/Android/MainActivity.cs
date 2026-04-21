using Android.App;
using Android.App.Roles;
using Android.Content.PM;
using Android.OS;

namespace BlockApp.App;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        RequestNotificationPermission();
        RequestCallScreeningRole();
    }

    private void RequestNotificationPermission()
    {
        if (OperatingSystem.IsAndroidVersionAtLeast(33))
        {
            if (CheckSelfPermission("android.permission.POST_NOTIFICATIONS")
                != Permission.Granted)
            {
                RequestPermissions(["android.permission.POST_NOTIFICATIONS"], 1001);
            }
        }
    }

    private void RequestCallScreeningRole()
    {
        if (!OperatingSystem.IsAndroidVersionAtLeast(29)) return;

        var roleManager = GetSystemService(RoleService) as RoleManager;
        if (roleManager == null) return;

        if (!roleManager.IsRoleHeld(RoleManager.RoleCallScreening))
        {
            var intent = roleManager.CreateRequestRoleIntent(RoleManager.RoleCallScreening);
            StartActivity(intent);
        }
    }
}
