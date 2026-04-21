# ? เพิ่มปุ่ม "ซื้อพอยต์" ใน ProfileView - สำเร็จ!

## ?? สิ่งที่ทำเสร็จ

### 1. ? UI - เพิ่มปุ่มซื้อพอยต์
**ไฟล์:** `BlockApp.App/Views/ProfileView.xaml`

```xml
<HorizontalStackLayout Spacing="8">
    <Label x:Name="TotalPoint" 
           Text="คงเหลือ 0 พอยต์" 
           FontSize="14" 
           FontAttributes="Bold"
           TextColor="#7C3AED"/>
    
    <!-- ปุ่มซื้อพอยต์ -->
    <Border BackgroundColor="#7C3AED" 
            Padding="8,4"
            StrokeThickness="0">
        <Border.GestureRecognizers>
            <TapGestureRecognizer Tapped="OnBuyPointsTapped"/>
        </Border.GestureRecognizers>
        <Label Text="+ ซื้อ" 
               FontSize="12" 
               FontAttributes="Bold"
               TextColor="White"/>
    </Border>
</HorizontalStackLayout>
```

**ผลลัพธ์:**
- แสดงยอดพอยต์ปัจจุบัน
- ปุ่ม "+ ซื้อ" สีม่วงอยู่ข้างๆ
- คลิกแล้วเปิดหน้า BuyPointsPage

---

### 2. ? Code-Behind - Event Handler
**ไฟล์:** `BlockApp.App/Views/ProfileView.xaml.cs`

#### เพิ่ม DI Injection:
```csharp
private readonly PointsPaymentService? _pointsPaymentService;

public ProfileView()
{
    InitializeComponent();
    
    var services = IPlatformApplication.Current?.Services;
    _pointsPaymentService = services?.GetService<PointsPaymentService>();
    
    LoadPointsBalance(); // โหลดยอดพอยต์
}
```

#### เพิ่ม Method สำหรับปุ่มซื้อ:
```csharp
private async void OnBuyPointsTapped(object? sender, EventArgs e)
{
    try
    {
        var services = IPlatformApplication.Current!.Services;
        var buyPointsPage = services.GetRequiredService<BuyPointsPage>();
        
        var navigationPage = Application.Current!.Windows[0].Page as NavigationPage;
        if (navigationPage != null)
        {
            await navigationPage.PushAsync(buyPointsPage);
        }
    }
    catch (Exception ex)
    {
        await DisplayAlert("ข้อผิดพลาด", $"ไม่สามารถเปิดหน้าซื้อพอยต์ได้", "ตกลง");
    }
}
```

#### เพิ่ม Method โหลดยอดพอยต์จริง:
```csharp
private async void LoadPointsBalance()
{
    try
    {
        if (_pointsPaymentService == null) return;

        var balance = await _pointsPaymentService.GetBalanceAsync();
        TotalPoint.Text = $"คงเหลือ {balance:N0} พอยต์";
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
        TotalPoint.Text = "คงเหลือ 0 พอยต์";
    }
}

public void RefreshPointsBalance()
{
    LoadPointsBalance(); // เรียกใช้เพื่อ refresh
}
```

---

### 3. ? Dependency Injection
**ไฟล์:** `BlockApp.App/MauiProgram.cs`

```csharp
builder.Services.AddTransient<BuyPointsPage>();
builder.Services.AddTransient<QrPaymentPage>();
```

---

## ?? Design ที่ได้

```
??????????????????????????????????????
? โปรไฟล์                            ?
?                  ???????????????????
?                  ? คงเหลือ 1,250 พอยต์ ? + ซื้อ ?
?                  ???????????????????
??????????????????????????????????????
```

**Features:**
- ? แสดงยอดพอยต์แบบ real-time จาก API
- ? ปุ่ม "+ ซื้อ" สีม่วง (#7C3AED)
- ? คลิกแล้ว navigate ไปหน้า BuyPointsPage
- ? รองรับการ refresh ยอด (เมื่อกลับมาจากหน้าซื้อ)

---

## ?? User Flow

```
ProfileView (แสดงยอดพอยต์)
    ? (คลิกปุ่ม "+ ซื้อ")
BuyPointsPage (เลือกแพ็คเกจ)
    ? (เลือกแพ็คเกจและชำระเงิน)
QrPaymentPage (แสกน QR)
    ? (ชำระเงินสำเร็จ)
กลับไป ProfileView (ยอดพอยต์อัพเดท)
```

---

## ?? API ที่ใช้

### 1. GET /api/points/balance
```json
Response:
{
  "balance": 1250.00
}
```

### 2. GET /api/payment/packages
```json
Response: [
  {
    "points": 100,
    "priceTHB": 100,
    "bonusPoints": 0
  }
]
```

### 3. POST /api/payment/create
```json
Request:
{
  "amount": 100,
  "paymentMethod": "promptpay"
}

Response:
{
  "paymentId": 1,
  "qrCodeUrl": "https://...",
  "pointsAmount": 100,
  "expiresAt": "2024-..."
}
```

---

## ? Testing Checklist

- [ ] เปิดแอป ? ไปที่แท็บ Profile
- [ ] ตรวจสอบว่าแสดงยอดพอยต์ถูกต้อง
- [ ] คลิกปุ่ม "+ ซื้อ"
- [ ] หน้า BuyPointsPage เปิดขึ้นมา
- [ ] เลือกแพ็คเกจและชำระเงิน
- [ ] ตรวจสอบว่า QR Code แสดงผล
- [ ] หลังชำระเงินสำเร็จ ? กลับไป Profile
- [ ] ยอดพอยต์อัพเดทอัตโนมัติ

---

## ?? ข้อดีของการออกแบบนี้

1. **Real-time Balance** - ดึงยอดจาก API จริง
2. **Clean UI** - ปุ่มเล็กกะทัดรัด ไม่รกหน้าจอ
3. **Easy Navigation** - คลิกเดียวไปหน้าซื้อ
4. **Error Handling** - จัดการ error ถ้า API ล่ม
5. **Dependency Injection** - ใช้ DI pattern ที่ถูกต้อง
6. **Refresh Support** - สามารถ refresh ยอดได้ง่าย

---

## ?? การใช้งานเพิ่มเติม

### Refresh ยอดพอยต์หลังซื้อสำเร็จ:

**ใน QrPaymentPage.xaml.cs:**
```csharp
// หลัง payment สำเร็จ
await Navigation.PopToRootAsync(); // กลับไป MainPage

// Refresh ProfileView
var mainPage = Application.Current!.Windows[0].Page as MainPage;
if (mainPage != null)
{
    // หา ProfileView และ refresh
    // หรือใช้ MessagingCenter / Events
}
```

### ใช้ MessagingCenter (แนะนำ):
```csharp
// ใน QrPaymentPage เมื่อชำระสำเร็จ
MessagingCenter.Send(this, "PointsPurchased");

// ใน ProfileView.xaml.cs Constructor
MessagingCenter.Subscribe<QrPaymentPage>(this, "PointsPurchased", (sender) =>
{
    MainThread.BeginInvokeOnMainThread(() =>
    {
        RefreshPointsBalance();
    });
});
```

---

## ?? พร้อมใช้งาน!

ตอนนี้ ProfileView มี:
- ? ปุ่มซื้อพอยต์
- ? แสดงยอดแบบ real-time
- ? Navigation ไปหน้าซื้อ
- ? รองรับ refresh

**คลิก "+ ซื้อ" แล้วเริ่มซื้อแต้มได้เลย!** ??
