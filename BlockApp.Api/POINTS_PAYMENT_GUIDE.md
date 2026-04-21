# ระบบแต้มและการชำระเงิน - คู่มือการใช้งาน

## ?? ภาพรวมระบบ

ระบบนี้ประกอบด้วย:
1. **ระบบแต้ม (Points System)** - ผู้ใช้สามารถเก็บสะสมและใช้แต้มภายในแอพ
2. **การชำระเงิน (Payment)** - ซื้อแต้มผ่าน PromptPay QR Code โดยใช้ Omise
3. **การโอนแต้ม (Transfer)** - โอนแต้มให้ผู้ใช้คนอื่นได้
4. **การรับรางวัล (Rewards)** - รับแต้มจากกิจกรรมต่างๆ เช่น ดูโฆษณา, ตอบคำถาม, เชิญเพื่อน

---

## ?? ขั้นตอนการติดตั้ง

### 1. สมัครบัญชี Omise

1. ไปที่ https://dashboard.omise.co/signup
2. สมัครบัญชี (เลือก Test Mode สำหรับทดสอบ)
3. ไปที่ Settings > Keys
4. คัดลอก **Public Key** และ **Secret Key**

### 2. ตั้งค่า API

แก้ไขไฟล์ `BlockApp.Api/appsettings.json`:

```json
{
  "Omise": {
    "PublicKey": "pkey_test_xxxxxxxxxx", // ใส่ Public Key ของคุณ
    "SecretKey": "skey_test_xxxxxxxxxx"  // ใส่ Secret Key ของคุณ
  }
}
```

**?? สำคัญ:** 
- ใช้ Test Keys สำหรับการพัฒนา
- **อย่า commit Secret Key** ลง Git
- ใช้ Production Keys เมื่อ deploy จริง

### 3. รัน Database Migration

```bash
cd BlockApp.Api
dotnet ef migrations add AddPointsAndPaymentTables
dotnet ef database update
```

### 4. ตั้งค่า Omise Webhook (สำหรับ Production)

1. ไปที่ Omise Dashboard > Webhooks
2. เพิ่ม endpoint: `https://your-api-domain.com/api/payment/webhook`
3. เลือก Event: `charge.complete`
4. Save

---

## ?? แพ็คเกจแต้ม (Points Packages)

| แต้ม | ราคา (บาท) | แต้มโบนัส | รวม |
|------|------------|------------|-----|
| 100  | 100        | 0          | 100 |
| 500  | 500        | 50         | 550 |
| 1,000 | 1,000     | 150        | 1,150 |
| 5,000 | 5,000     | 1,000      | 6,000 |

---

## ?? API Endpoints

### Points APIs

#### 1. ดูยอดแต้ม
```http
GET /api/points/balance
Authorization: Bearer {jwt_token}

Response:
{
  "balance": 1500
}
```

#### 2. โอนแต้ม
```http
POST /api/points/transfer
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "recipientPhoneNumber": "0812345678",
  "amount": 100,
  "note": "ขอบคุณนะ"
}

Response:
{
  "message": "Transfer successful",
  "transactionId": 123,
  "amount": 100
}
```

#### 3. ประวัติการทำรายการ
```http
GET /api/points/history?page=1&pageSize=20
Authorization: Bearer {jwt_token}

Response:
[
  {
    "id": 1,
    "amount": -100,
    "type": "Transfer",
    "status": "Completed",
    "description": "โอนแต้มให้ 0812345678",
    "relatedUserPhone": "0812345678",
    "createdAt": "2024-01-15T10:30:00Z"
  }
]
```

#### 4. บันทึกกิจกรรมรางวัล
```http
POST /api/points/reward
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "activityType": "WatchAd",
  "metadata": "{\"adId\": \"ad123\"}"
}

Response:
{
  "message": "Reward recorded",
  "pointsEarned": 1,
  "activityId": 456
}
```

#### 5. ดูอัตราแลกรางวัล
```http
GET /api/points/reward-rates
Authorization: Bearer {jwt_token}

Response:
{
  "WatchAd": 1,
  "AnswerQuiz": 5,
  "Referral": 50,
  "DailyLogin": 2
}
```

---

### Payment APIs

#### 1. ดูแพ็คเกจแต้ม
```http
GET /api/payment/packages
Authorization: Bearer {jwt_token}

Response:
[
  {
    "points": 100,
    "priceTHB": 100,
    "bonusPoints": 0
  },
  {
    "points": 500,
    "priceTHB": 500,
    "bonusPoints": 50
  }
]
```

#### 2. สร้างการชำระเงิน (ได้ QR Code)
```http
POST /api/payment/create
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "amount": 100,
  "paymentMethod": "promptpay"
}

Response:
{
  "paymentId": 789,
  "status": "Pending",
  "qrCodeUrl": "https://cdn.omise.co/...",
  "omiseChargeId": "chrg_test_xxx",
  "pointsAmount": 100,
  "expiresAt": "2024-01-15T11:00:00Z"
}
```

#### 3. ตรวจสอบสถานะการชำระเงิน
```http
GET /api/payment/status/789
Authorization: Bearer {jwt_token}

Response:
{
  "status": "Success",
  "isCompleted": true,
  "pointsAdded": 100,
  "paidAt": "2024-01-15T10:35:00Z"
}
```

---

## ?? ตัวอย่างการใช้งานใน MAUI App

### 1. แสดง QR Code สำหรับชำระเงิน

```csharp
// สร้างการชำระเงิน
var paymentRequest = new CreatePaymentDto
{
    Amount = 100,
    PaymentMethod = "promptpay"
};

var response = await _apiService.PostAsync<PaymentResponseDto>(
    "payment/create", paymentRequest);

// แสดง QR Code
QrCodeImage.Source = response.QrCodeUrl;

// เริ่ม polling เพื่อตรวจสอบสถานะ
await CheckPaymentStatusAsync(response.PaymentId);
```

### 2. ตรวจสอบสถานะการชำระเงิน (Polling)

```csharp
private async Task CheckPaymentStatusAsync(int paymentId)
{
    var maxAttempts = 60; // ตรวจสอบ 60 ครั้ง (5 นาที)
    var delaySeconds = 5;

    for (int i = 0; i < maxAttempts; i++)
    {
        await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
        
        var status = await _apiService.GetAsync<PaymentStatusResponseDto>(
            $"payment/status/{paymentId}");

        if (status.IsCompleted)
        {
            // ชำระเงินสำเร็จ
            await DisplayAlert("สำเร็จ", 
                $"ได้รับ {status.PointsAdded} แต้ม", "ตกลง");
            await RefreshPointsBalance();
            return;
        }
    }
    
    // หมดเวลา
    await DisplayAlert("หมดเวลา", 
        "กรุณาลองใหม่อีกครั้ง", "ตกลง");
}
```

### 3. โอนแต้ม

```csharp
private async Task TransferPointsAsync()
{
    var transfer = new TransferPointsDto
    {
        RecipientPhoneNumber = RecipientPhoneEntry.Text,
        Amount = decimal.Parse(AmountEntry.Text),
        Note = NoteEditor.Text
    };

    try
    {
        var result = await _apiService.PostAsync<object>(
            "points/transfer", transfer);
        
        await DisplayAlert("สำเร็จ", 
            $"โอน {transfer.Amount} แต้มสำเร็จ", "ตกลง");
    }
    catch (ApiException ex)
    {
        await DisplayAlert("ผิดพลาด", ex.Message, "ตกลง");
    }
}
```

### 4. บันทึกกิจกรรมดูโฆษณา

```csharp
private async Task OnAdWatchedAsync()
{
    var reward = new RewardActivityDto
    {
        ActivityType = "WatchAd",
        Metadata = JsonSerializer.Serialize(new { AdId = "ad_12345" })
    };

    var result = await _apiService.PostAsync<object>(
        "points/reward", reward);
    
    // แสดงแอนิเมชั่นได้รับแต้ม
    await ShowPointsEarnedAnimation(1);
}
```

---

## ?? UI Components ที่แนะนำ

### 1. หน้าซื้อแต้ม (Buy Points Page)
- แสดงแพ็คเกจแต้มทั้งหมด
- แสดงแต้มโบนัสชัดเจน
- ปุ่มซื้อแต่ละแพ็คเกจ

### 2. หน้าชำระเงิน QR (QR Payment Page)
- แสดง QR Code ขนาดใหญ่
- แสดงจำนวนเงินและแต้มที่จะได้รับ
- แสดง Countdown timer (หมดอายุ 15 นาที)
- ปุ่ม "ฉันชำระเงินแล้ว" เพื่อ force check
- Loading indicator ขณะรอการยืนยัน

### 3. หน้าโอนแต้ม (Transfer Page)
- Input เบอร์โทรผู้รับ
- Input จำนวนแต้ม
- แสดงยอดคงเหลือปัจจุบัน
- Textarea สำหรับหมายเหตุ (optional)

### 4. หน้าประวัติ (History Page)
- ListView แสดงรายการทำรายการ
- แยกสีตามประเภท (เติม/โอน/ใช้)
- แสดงวันที่/เวลา
- Infinite scroll หรือ pagination

---

## ?? Security Best Practices

1. **อย่า hardcode API keys** ใน code
2. ใช้ **HTTPS** เสมอ
3. Validate input ทุกครั้ง (amount > 0, phone number format)
4. ใช้ JWT authentication
5. Implement rate limiting สำหรับ sensitive APIs
6. Log ทุก transaction สำหรับ audit trail

---

## ?? การทดสอบ

### Test Mode (Omise)
ใน Test Mode คุณสามารถ:
- สร้าง QR Code จำลอง
- ทดสอบ payment flow โดยไม่ต้องจ่ายเงินจริง
- ใช้ Omise Dashboard เพื่อจำลองการชำระเงินสำเร็จ

### Test Payment Flow
1. สร้าง payment ใน Test Mode
2. ไปที่ Omise Dashboard > Charges
3. เลือก charge ที่สร้าง
4. คลิก "Mark as Successful"
5. Webhook จะถูกส่งไปที่ API และอัพเดทสถานะ

---

## ?? Database Schema

### PointTransactions
```sql
CREATE TABLE PointTransactions (
    Id INT PRIMARY KEY,
    UserId INT NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Type VARCHAR(50), -- Credit, Deduct, Transfer
    Status VARCHAR(50), -- Pending, Completed, Failed
    Description TEXT,
    ReferenceId VARCHAR(255),
    RelatedUserId INT NULL,
    CreatedAt DATETIME NOT NULL,
    CompletedAt DATETIME NULL
);
```

### Payments
```sql
CREATE TABLE Payments (
    Id INT PRIMARY KEY,
    UserId INT NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Currency VARCHAR(3),
    Status VARCHAR(50), -- Pending, Success, Failed
    OmiseChargeId VARCHAR(255),
    OmiseSourceId VARCHAR(255),
    PaymentMethod VARCHAR(50),
    QrCodeUrl TEXT,
    PointsAmount INT NOT NULL,
    CreatedAt DATETIME NOT NULL,
    PaidAt DATETIME NULL,
    ExpiresAt DATETIME NULL
);
```

### RewardActivities
```sql
CREATE TABLE RewardActivities (
    Id INT PRIMARY KEY,
    UserId INT NOT NULL,
    ActivityType VARCHAR(50), -- WatchAd, AnswerQuiz, Referral
    PointsEarned DECIMAL(18,2) NOT NULL,
    Metadata TEXT,
    CreatedAt DATETIME NOT NULL
);
```

---

## ?? Next Steps

1. **สร้าง UI Pages** ใน MAUI App
2. **ทดสอบ Payment Flow** ใน Test Mode
3. **เพิ่ม Push Notifications** เมื่อได้รับแต้ม/โอนสำเร็จ
4. **สร้าง Dashboard** สำหรับดูสถิติการใช้แต้ม
5. **เพิ่ม Referral System** ให้สมบูรณ์ (generate unique code)

---

## ?? ข้อควรระวัง

### ?? สิ่งที่ต้องดูแล:
- **Race Condition**: ใช้ Database Transaction เพื่อป้องกันการหักแต้มซ้ำ
- **Expired QR**: QR Code หมดอายุใน 15 นาที
- **Insufficient Balance**: ตรวจสอบยอดคงเหลือก่อนโอน/ใช้แต้ม
- **Webhook Security**: ตรวจสอบ signature ของ webhook จาก Omise

---

## ?? Support

หากมีปัญหาหรือข้อสงสัย:
1. ดู Omise Documentation: https://docs.omise.co/
2. ตรวจสอบ Logs ใน API
3. ใช้ Omise Dashboard เพื่อ debug

---

**สร้างโดย:** BlockApp Development Team  
**เวอร์ชัน:** 1.0.0  
**อัพเดทล่าสุด:** 2024-01-15
