# 🚨 XAML Encoding Error - ต้องแก้ไขด้วยตนเอง

## ❌ ปัญหา
```
System.Xml.XmlException: Invalid character in the given encoding. Line 30, position 34.
```

**สาเหตุ:** ไฟล์ XAML บางไฟล์ยังมี **invalid encoding characters** (ตัวอักษรไทยหรือ emoji ที่ไม่ถูก save เป็น UTF-8 BOM)

---

## ✅ วิธีแก้ไข (ต้องทำด้วยตนเอง)

### **ขั้นตอนที่ 1: Clean Project**
```sh
# ลบ build cache ทั้งหมด
Remove-Item -Recurse -Force BlockApp.App\bin
Remove-Item -Recurse -Force BlockApp.App\obj
dotnet clean BlockApp.App
```

### **ขั้นตอนที่ 2: แก้ไข Encoding ของไฟล์ XAML**

#### ใน Visual Studio:
1. เปิดไฟล์ที่มีปัญหา (ProfileView.xaml หรือไฟล์อื่นๆ)
2. **File > Advanced Save Options...**  
   (ถ้าไม่มี: Tools > Customize > Commands > Add Command > File > Advanced Save Options)
3. เลือก: **Unicode (UTF-8 with signature) - Codepage 65001**
4. บันทึกไฟล์
5. ทำซ้ำกับไฟล์ XAML ทั้งหมด

#### หรือใช้ PowerShell Script:
```powershell
# รันใน PowerShell (ในโฟลเดอร์ root ของ project)
Get-ChildItem -Path "BlockApp.App" -Filter "*.xaml" -Recurse | ForEach-Object {
    try {
        $content = [System.IO.File]::ReadAllText($_.FullName, [System.Text.Encoding]::UTF8)
        $utf8WithBom = New-Object System.Text.UTF8Encoding $true
        [System.IO.File]::WriteAllText($_.FullName, $content, $utf8With Bom)
        Write-Host "✅ Fixed: $($_.Name)" -ForegroundColor Green
    }
    catch {
        Write-Host "❌ Error: $($_.Name) - $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host "`n🎯 Done! Now run: dotnet clean && dotnet build" -ForegroundColor Cyan
```

### **ขั้นตอนที่ 3: Build อีกครั้ง**
```sh
dotnet build BlockApp.App
```

---

## 🎯 วิธีแก้ปัญหาถาวร

### 1. สร้างไฟล์ `.editorconfig`
สร้างไฟล์นี้ใน root ของ solution:

```ini
# .editorconfig
root = true

# All files
[*]
charset = utf-8-bom
end_of_line = crlf
insert_final_newline = true

# XAML files
[*.xaml]
charset = utf-8-bom
indent_style = space
indent_size = 4

# C# files
[*.cs]
charset = utf-8-bom
indent_style = space
indent_size = 4
```

### 2. ใช้ Unicode Escape แทน Emoji

**แทนที่:**
```xml
<Label Text="👤" />  <!-- ❌ อาจเกิดปัญหา encoding -->
```

**ด้วย:**
```xml
<Label Text="&#x1F464;" />  <!-- ✅ ปลอดภัย -->
```

### 3. Unicode Codes ที่ใช้บ่อย

| Emoji | Unicode | XAML Code |
|-------|---------|-----------|
| 👤 | U+1F464 | `&#x1F464;` |
| 📷 | U+1F4F7 | `&#x1F4F7;` |
| 🔔 | U+1F514 | `&#x1F514;` |
| 🔒 | U+1F512 | `&#x1F512;` |
| ❓ | U+2753 | `&#x2753;` |
| 🚪 | U+1F6AA | `&#x1F6AA;` |
| 🛡️ | U+1F6E1 | `&#x1F6E1;&#xFE0F;` |

| › | U+203A | `&#x203A;` or `&rsaquo;` |

---

## 🔍 หาไฟล์ที่มีปัญหา

### PowerShell Script เพื่อตรวจสอบ:
```powershell
Get-ChildItem -Path "BlockApp.App" -Filter "*.xaml" -Recurse | ForEach-Object {
    $content = Get-Content $_.FullName -Raw -Encoding UTF8
    
    # Check for common problematic characters
    if ($content -match '[^\x00-\x7F]' -and $content -notmatch '&#x') {
        Write-Host "⚠️  $($_.Name) may have encoding issues" -ForegroundColor Yellow
        
        # Find line with issue
        $lines = Get-Content $_.FullName -Encoding UTF8
        for ($i = 0; $i -lt $lines.Count; $i++) {
            if ($lines[$i] -match '[^\x00-\x7F]' -and $lines[$i] -notmatch '&#x|เ|ก|า|ร|แ|จ|้|ง|ต|ื|อ|น|ค|ว|ม|ป|ส|ช|ย|ล|ด|ผ|ใ|ท|ฟ|ล|อ|จ|ร|ะ|บ|แ|อ|&#x') {
                Write-Host "   Line $($i + 1): $($lines[$i].Trim())" -ForegroundColor Gray
            }
        }
    }
}
```

---

## 📋 Checklist

- [ ] Clean project (`dotnet clean`)
- [ ] ลบ bin/obj folders
- [ ] แก้ encoding ของไฟล์ XAML เป็น UTF-8 with BOM
- [ ] ตรวจสอบว่าไม่มี emoji ที่ไม่ได้ใช้ Unicode escape
- [ ] Rebuild (`dotnet build`)
- [ ] สร้างไฟล์ `.editorconfig`
- [ ] ใช้ Unicode escape codes แทน emoji ในอนาคต

---

## ⚡ Quick Fix (All-in-One)

รันคำสั่งนี้ใน PowerShell:

```powershell
# 1. Clean
Remove-Item -Recurse -Force BlockApp.App\bin, BlockApp.App\obj -ErrorAction SilentlyContinue
dotnet clean BlockApp.App

# 2. Fix all XAML files
Get-ChildItem -Path "BlockApp.App" -Filter "*.xaml" -Recurse | ForEach-Object {
    $content = [System.IO.File]::ReadAllText($_.FullName, [System.Text.Encoding]::UTF8)
    $utf8WithBom = New-Object System.Text.UTF8Encoding $true
    [System.IO.File]::WriteAllText($_.FullName, $content, $utf8WithBom)
}

# 3. Rebuild
dotnet build BlockApp.App
```

---

**หมายเหตุ:** ปัญหานี้เกิดจากการที่ Visual Studio บางครั้ง save ไฟล์เป็น encoding อื่นแทน UTF-8 BOM ซึ่ง MAUI XAML parser ต้องการอย่างเคร่งครัด

**แนะนำ:** ใช้ Unicode escape codes (`&#xXXXX;`) แทน emoji โดยตรงเสมอเพื่อป้องกันปัญหานี้
