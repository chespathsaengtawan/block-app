# ?? แก้ไข Migration Error - Complete

## ? ปัญหาเดิม

```
An error was generated for warning 'Microsoft.EntityFrameworkCore.Migrations.PendingModelChangesWarning': 
The model for context 'AppDbContext' changes each time it is built. 
This is usually caused by dynamic values used in a 'HasData' call (e.g. `new DateTime()`, `Guid.NewGuid()`).
```

### สาเหตุ:
```csharp
// ? ใช้ DateTime.UtcNow (dynamic value)
new PointsPackage { Id = 1, Points = 100, CreatedAt = DateTime.UtcNow }
```

---

## ? วิธีแก้ไข

### ใช้ค่าคงที่แทน:
```csharp
// ? ใช้ค่า DateTime แบบคงที่
new PointsPackage 
{ 
    Id = 1, 
    Points = 100, 
    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
}
```

---

## ?? ขั้นตอนการสร้าง Migration

### 1. สร้าง Migration ใหม่
```bash
cd BlockApp.Api
dotnet ef migrations add AddPointsPackagesTable
```

**Output ที่คาดหวัง:**
```
Build succeeded.
To undo this action, use 'dotnet ef migrations remove'
```

### 2. ตรวจสอบไฟล์ Migration
```bash
# ไฟล์จะถูกสร้างที่
BlockApp.Api/Migrations/[timestamp]_AddPointsPackagesTable.cs
```

**ควรมี:**
```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.CreateTable(
        name: "PointsPackages",
        columns: table => new
        {
            Id = table.Column<int>(nullable: false)
                .Annotation("Sqlite:Autoincrement", true),
            Points = table.Column<int>(nullable: false),
            PriceTHB = table.Column<decimal>(nullable: false),
            BonusPoints = table.Column<int>(nullable: true),
            IsActive = table.Column<bool>(nullable: false),
            DisplayOrder = table.Column<int>(nullable: false),
            CreatedAt = table.Column<DateTime>(nullable: false),
            UpdatedAt = table.Column<DateTime>(nullable: true)
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_PointsPackages", x => x.Id);
        });

    migrationBuilder.InsertData(
        table: "PointsPackages",
        columns: new[] { "Id", "Points", "PriceTHB", "BonusPoints", "IsActive", "DisplayOrder", "CreatedAt" },
        values: new object[,]
        {
            { 1, 100, 100m, 0, true, 1, new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            { 2, 500, 500m, 50, true, 2, new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            { 3, 1000, 1000m, 150, true, 3, new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            { 4, 5000, 5000m, 1000, true, 4, new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        });
}
```

### 3. อัพเดทฐานข้อมูล
```bash
dotnet ef database update
```

**Output ที่คาดหวัง:**
```
Build succeeded.
Applying migration '20240115120000_AddPointsPackagesTable'.
Done.
```

### 4. ตรวจสอบข้อมูล
```bash
# SQLite
sqlite3 blockapp.db "SELECT * FROM PointsPackages;"

# หรือใช้ SQL query
SELECT * FROM PointsPackages;
```

**ผลลัพธ์:**
```
Id | Points | PriceTHB | BonusPoints | IsActive | DisplayOrder | CreatedAt
1  | 100    | 100.00   | 0           | 1        | 1            | 2024-01-01 00:00:00
2  | 500    | 500.00   | 50          | 1        | 2            | 2024-01-01 00:00:00
3  | 1000   | 1000.00  | 150         | 1        | 3            | 2024-01-01 00:00:00
4  | 5000   | 5000.00  | 1000        | 1        | 4            | 2024-01-01 00:00:00
```

---

## ?? หมายเหตุ

### ทำไมต้องใช้ค่าคงที่?

1. **Deterministic**: ค่าเดิมทุกครั้งที่ build
2. **Migration Stability**: EF Core สามารถ compare models ได้ถูกต้อง
3. **Reproducible**: สามารถสร้าง migration ซ้ำได้เหมือนเดิม

### Alternative Solutions

#### Option 1: ไม่ seed CreatedAt
```csharp
// ปล่อยให้ default value ใน database
new PointsPackage 
{ 
    Id = 1, 
    Points = 100
    // ไม่ระบุ CreatedAt
}
```

#### Option 2: ใช้ SQL Migration แทน
```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.Sql(@"
        INSERT INTO PointsPackages (Points, PriceTHB, BonusPoints, IsActive, DisplayOrder, CreatedAt)
        VALUES 
            (100, 100, 0, 1, 1, datetime('now')),
            (500, 500, 50, 1, 2, datetime('now'));
    ");
}
```

#### Option 3: ปิด Warning
```csharp
// ใน AppDbContext.OnConfiguring
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.ConfigureWarnings(w => 
        w.Ignore(RelationalEventId.PendingModelChangesWarning));
}
```

---

## ? Checklist

- [x] แก้ไข `AppDbContext.cs` - ใช้ค่าคงที่สำหรับ CreatedAt
- [ ] รัน `dotnet ef migrations add AddPointsPackagesTable`
- [ ] รัน `dotnet ef database update`
- [ ] ตรวจสอบข้อมูลใน database
- [ ] ทดสอบ API endpoint `/api/payment/packages`

---

## ?? สรุป

**Before:**
```csharp
CreatedAt = DateTime.UtcNow // ? Dynamic
```

**After:**
```csharp
CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) // ? Static
```

**ตอนนี้สามารถสร้าง migration ได้โดยไม่มี warning แล้ว!** ??
