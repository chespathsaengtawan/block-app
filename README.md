# BlockApp

ระบบบล็อกเบอร์โทรศัพท์และเลขบัญชีธนาคาร ประกอบด้วย **REST API** (.NET 9) และ **Mobile App** (.NET MAUI) รองรับ Android

---

## โครงสร้างโปรเจ็กต์

```
block-app/
├── BlockApp.Api/          # .NET 9 Web API
├── BlockApp.App/          # .NET MAUI Mobile App (Android)
└── BlockApp.Shared/       # Shared Entities & DTOs
```

### BlockApp.Api
- Authentication ด้วย JWT + OTP (ThaiBulkSMS)
- จัดการ Blocklist ต่อผู้ใช้
- ระบบ Points & การเติมเงินผ่าน Omise (PromptPay QR)
- SQLite (Dev) / PostgreSQL (Production)
- Deploy บน Railway

### BlockApp.App
- Login ด้วยเบอร์โทร + OTP + PIN
- ค้นหาและบล็อก/ยกเลิกบล็อก เบอร์โทร/เลขบัญชีธนาคาร
- เติมพอยต์ผ่าน QR PromptPay
- ประวัติการใช้งาน (History)

### BlockApp.Shared
- Entities: `User`, `BlockEntry`, `Payment`, `PointTransaction`, `PointsPackage`, `RewardActivity`
- DTOs และ Enums ที่ใช้ร่วมกันระหว่าง API และ App

---

## เริ่มต้นใช้งาน (Local Development)

### ความต้องการ
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- .NET MAUI Workload (สำหรับ App)
- Android Emulator หรืออุปกรณ์ Android (สำหรับ App)

### ติดตั้ง MAUI Workload

```bash
dotnet workload install maui
```

### รัน API

```bash
cd BlockApp.Api
dotnet run
```

API จะรันที่ `https://localhost:7xxx` — ดู Scalar API Docs ที่ `/scalar/v1`

### Environment Variables (API)

| Key | คำอธิบาย |
|-----|----------|
| `ConnectionStrings__DefaultConnection` | SQLite path (Dev) หรือ PostgreSQL connection string |
| `Jwt__Key` | Secret key สำหรับ JWT signing |
| `ThaibulkSMS__AppKey` | ThaiBulkSMS App Key |
| `ThaibulkSMS__AppSecret` | ThaiBulkSMS App Secret |
| `Omise__PublicKey` | Omise Public Key |
| `Omise__SecretKey` | Omise Secret Key |
| `DATABASE_URL` | PostgreSQL URL รูปแบบ `postgres://user:pass@host:port/db` (Production/Railway) |

### รัน Migrations

```bash
cd BlockApp.Api
dotnet ef database update
```

### รัน App (Android)

```bash
cd BlockApp.App
dotnet build -f net9.0-android -t:Run /p:AndroidAttachDebugger=false /p:RuntimeIdentifier=android-arm
```

---

## Deployment (Railway)

โปรเจ็กต์พร้อม deploy บน [Railway](https://railway.app) ผ่าน `Dockerfile` และ `railway.toml`

ตั้งค่า Environment Variables ใน Railway Dashboard ตามตารางด้านบน แล้ว push ไปที่ `main` — Railway จะ build และ deploy อัตโนมัติ

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| API | .NET 9, ASP.NET Core, Entity Framework Core |
| Mobile | .NET MAUI (Android) |
| Database (Dev) | SQLite |
| Database (Prod) | PostgreSQL (Railway) |
| Authentication | JWT Bearer, OTP via ThaiBulkSMS |
| Payment | Omise (PromptPay QR) |
| API Docs | Scalar (OpenAPI) |

---

## หน้าจอหลัก (App)

| หน้า | คำอธิบาย |
|------|----------|
| Splash / Login | Login ด้วยเบอร์โทร + OTP |
| Create/Enter PIN | ตั้งและยืนยัน PIN สำหรับเข้าแอป |
| Home | ภาพรวมและเมนูหลัก |
| Search | ค้นหาเบอร์โทร/บัญชีธนาคารในระบบ |
| Add Block | เพิ่มเบอร์โทร/บัญชีธนาคารเข้าบล็อกลิสต์ |
| Buy Points | เลือกแพ็กเกจเติมพอยต์ |
| QR Payment | สแกน QR PromptPay เพื่อชำระเงิน |
| History | ประวัติการบล็อกและการเติมพอยต์ |
| Profile | ข้อมูลผู้ใช้, ยอดพอยต์, ออกจากระบบ |
