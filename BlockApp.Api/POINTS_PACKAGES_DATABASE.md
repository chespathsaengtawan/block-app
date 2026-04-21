# ?? Points Packages - Database Configuration Guide

## ? การเปลี่ยนแปลง

### เดิม (Hardcoded)
```csharp
// ใน PaymentService.cs
private readonly List<PointsPackageDto> _packages = new()
{
    new() { Points = 100, PriceTHB = 100, BonusPoints = 0 },
    new() { Points = 500, PriceTHB = 500, BonusPoints = 50 },
    ...
};
```

### ใหม่ (Database-driven)
```csharp
// ดึงจากฐานข้อมูล
public async Task<List<PointsPackageDto>> GetPointsPackagesAsync()
{
    return await _context.PointsPackages
        .Where(p => p.IsActive)
        .OrderBy(p => p.DisplayOrder)
        .ToListAsync();
}
```

---

## ?? Database Schema

### PointsPackage Table
```sql
CREATE TABLE PointsPackages (
    Id INT PRIMARY KEY IDENTITY,
    Points INT NOT NULL,
    PriceTHB DECIMAL(18,2) NOT NULL,
    BonusPoints INT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    DisplayOrder INT NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NULL
);
```

---

## ?? ขั้นตอนการ Migrate

### 1. สร้าง Migration
```bash
cd BlockApp.Api
dotnet ef migrations add AddPointsPackagesTable
```

### 2. อัพเดทฐานข้อมูล
```bash
dotnet ef database update
```

### 3. ตรวจสอบข้อมูล Seed
```sql
SELECT * FROM PointsPackages;
```

**ผลลัพธ์ที่คาดหวัง:**
```
Id | Points | PriceTHB | BonusPoints | IsActive | DisplayOrder
1  | 100    | 100.00   | 0           | 1        | 1
2  | 500    | 500.00   | 50          | 1        | 2
3  | 1000   | 1000.00  | 150         | 1        | 3
4  | 5000   | 5000.00  | 1000        | 1        | 4
```

---

## ?? API Endpoints สำหรับจัดการ Packages

### สำหรับ User (Public)
```http
GET /api/payment/packages
Authorization: Bearer {token}

Response:
[
  {
    "points": 100,
    "priceTHB": 100,
    "bonusPoints": 0
  }
]
```

### สำหรับ Admin (จัดการ Packages)

#### 1. ดูทั้งหมด (รวม inactive)
```http
GET /api/pointspackage
Authorization: Bearer {admin_token}
```

#### 2. ดูรายการเดียว
```http
GET /api/pointspackage/{id}
```

#### 3. สร้าง Package ใหม่
```http
POST /api/pointspackage
Content-Type: application/json

{
  "points": 10000,
  "priceTHB": 10000,
  "bonusPoints": 2000,
  "isActive": true,
  "displayOrder": 5
}
```

#### 4. แก้ไข Package
```http
PUT /api/pointspackage/{id}
Content-Type: application/json

{
  "points": 10000,
  "priceTHB": 9999,
  "bonusPoints": 2500,
  "isActive": true,
  "displayOrder": 5
}
```

#### 5. ปิดการใช้งาน (Soft Delete)
```http
DELETE /api/pointspackage/{id}
```

#### 6. เปิดการใช้งานใหม่
```http
POST /api/pointspackage/{id}/activate
```

---

## ?? ตัวอย่างการใช้งาน

### เพิ่ม Package แบบพิเศษ (Flash Sale)
```sql
INSERT INTO PointsPackages (Points, PriceTHB, BonusPoints, IsActive, DisplayOrder, CreatedAt)
VALUES (1000, 800, 300, 1, 3, GETUTCDATE());
-- ลด 20% + โบนัสพิเศษ 300 แต้ม
```

### ปรับราคาแบบชั่วคราว
```sql
-- ปิด package เดิม
UPDATE PointsPackages SET IsActive = 0 WHERE Id = 3;

-- เพิ่ม package โปรโมชั่น
INSERT INTO PointsPackages (Points, PriceTHB, BonusPoints, IsActive, DisplayOrder, CreatedAt)
VALUES (1000, 899, 200, 1, 3, GETUTCDATE());
```

### จัดเรียงลำดับการแสดงผล
```sql
UPDATE PointsPackages SET DisplayOrder = 1 WHERE Id = 4; -- ให้แพ็คเกจ 5000 แสดงก่อน
UPDATE PointsPackages SET DisplayOrder = 2 WHERE Id = 3;
UPDATE PointsPackages SET DisplayOrder = 3 WHERE Id = 2;
UPDATE PointsPackages SET DisplayOrder = 4 WHERE Id = 1;
```

---

## ?? ข้อดีของการใช้ Database

? **ปรับราคาได้ทันที** - ไม่ต้อง deploy code ใหม่  
? **A/B Testing** - สามารถสร้าง package หลายแบบทดสอบ  
? **Flash Sale** - เปิด/ปิด package ได้ตามเวลา  
? **Track Changes** - มี UpdatedAt เก็บประวัติการแก้ไข  
? **Soft Delete** - ปิดการใช้งานแทนการลบ  
? **Flexible Ordering** - จัดลำดับการแสดงผลได้  

---

## ?? Security Notes

?? **TODO:** เพิ่ม Authorization สำหรับ Admin
```csharp
[Authorize(Roles = "Admin")]
public class PointsPackageController : ControllerBase
```

?? **TODO:** เพิ่ม Audit Log
```csharp
// บันทึกว่าใครแก้ไข package เมื่อไร
public class PackageAuditLog
{
    public int Id { get; set; }
    public int PackageId { get; set; }
    public string ChangedBy { get; set; }
    public string Action { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

---

## ?? Admin Dashboard Ideas

### สถิติการขาย Packages
```sql
SELECT 
    pp.Points,
    pp.PriceTHB,
    COUNT(p.Id) as TotalSales,
    SUM(p.Amount) as TotalRevenue
FROM PointsPackages pp
LEFT JOIN Payments p ON p.PointsAmount = (pp.Points + ISNULL(pp.BonusPoints, 0))
    AND p.Status = 'Success'
GROUP BY pp.Points, pp.PriceTHB
ORDER BY TotalRevenue DESC;
```

### Package ที่ขายดีที่สุด
```sql
SELECT TOP 3
    pp.*,
    COUNT(p.Id) as SalesCount
FROM PointsPackages pp
JOIN Payments p ON p.PointsAmount = (pp.Points + ISNULL(pp.BonusPoints, 0))
WHERE p.Status = 'Success'
    AND p.CreatedAt >= DATEADD(day, -30, GETUTCDATE())
GROUP BY pp.Id, pp.Points, pp.PriceTHB, pp.BonusPoints, pp.IsActive, pp.DisplayOrder, pp.CreatedAt, pp.UpdatedAt
ORDER BY SalesCount DESC;
```

---

## ?? สรุป

- ? **Entity:** `PointsPackage.cs` สร้างแล้ว
- ? **DbContext:** เพิ่ม `DbSet<PointsPackage>` + Seed data
- ? **Service:** `PaymentService` ดึงจาก DB แทน hardcode
- ? **Controller:** `PointsPackageController` สำหรับ CRUD
- ? **Migration:** พร้อมรัน `dotnet ef migrations add AddPointsPackagesTable`

**ตอนนี้คุณสามารถจัดการ Points Packages ผ่านฐานข้อมูลได้แล้ว!** ??
