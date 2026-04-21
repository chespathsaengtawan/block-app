# ?? แก้ไข XAML Encoding Error

## ? ปัญหา
```
System.Xml.XmlException: Invalid character in the given encoding. Line 30, position 34.
```

### สาเหตุ:
- Emoji (??, ??, ??, etc.) ถูก save ด้วย encoding ที่ไม่ถูกต้อง
- Visual Studio บางครั้งเปลี่ยน encoding เป็น ANSI แทน UTF-8

---

## ? วิธีแก้ไข

### Option 1: Clean & Rebuild
```bash
# 1. Clean project
dotnet clean BlockApp.App

# 2. ลบ bin และ obj
Remove-Item -Recurse -Force BlockApp.App\bin
Remove-Item -Recurse -Force BlockApp.App\obj

# 3. Rebuild
dotnet build BlockApp.App
```

### Option 2: แก้ไข Encoding ของไฟล์ XAML ทั้งหมด

#### PowerShell Script:
```powershell
# แก้ encoding ของไฟล์ XAML ทั้งหมด
Get-ChildItem -Path "BlockApp.App" -Filter "*.xaml" -Recurse | ForEach-Object {
    $content = Get-Content $_.FullName -Raw -Encoding UTF8
    [System.IO.File]::WriteAllText($_.FullName, $content, [System.Text.Encoding]::UTF8)
    Write-Host "Fixed: $($_.FullName)"
}
```

### Option 3: แก้ไขใน Visual Studio

1. เปิดไฟล์ที่มีปัญหา (ProfileView.xaml)
2. **File > Advanced Save Options**
3. เลือก: **Unicode (UTF-8 with signature) - Codepage 65001**
4. บันทึก

---

## ?? ป้องกันปัญหาในอนาคต

### 1. ตั้งค่า VS Code / Visual Studio
```json
// settings.json
{
  "files.encoding": "utf8bom",
  "files.autoGuessEncoding": false
}
```

### 2. ใช้ .editorconfig
```ini
# .editorconfig ในโฟลเดอร์ root
root = true

[*.xaml]
charset = utf-8-bom
end_of_line = crlf
```

### 3. ใช้ Unicode Escape แทน Emoji
```xml
<!-- แทนที่ -->
<Label Text="??" />

<!-- ด้วย -->
<Label Text="&#x1F4F7;" />
```

---

## ?? Unicode Codes สำหรับ Emoji

| Emoji | Unicode | XAML |
|-------|---------|------|
| ?? | U+1F464 | `&#x1F464;` |
| ?? | U+1F4F7 | `&#x1F4F7;` |
| ?? | U+1F514 | `&#x1F514;` |
| ?? | U+1F512 | `&#x1F512;` |
| ? | U+2753 | `&#x2753;` |
| ?? | U+1F6AA | `&#x1F6AA;` |

---

## ? Checklist

- [ ] Clean project: `dotnet clean BlockApp.App`
- [ ] ลบ bin/obj folders
- [ ] แก้ encoding ของ XAML files เป็น UTF-8 BOM
- [ ] Rebuild project
- [ ] ถ้ายังไม่ได้ ? ใช้ Unicode escape codes แทน emoji

---

## ?? Quick Fix Command

```powershell
# รันคำสั่งนี้ใน PowerShell (ใน folder BlockApp.App)
Get-ChildItem -Filter "*.xaml" -Recurse | ForEach-Object {
    $content = Get-Content $_.FullName -Raw -Encoding UTF8
    $content | Out-File $_.FullName -Encoding UTF8 -NoNewline
}

dotnet clean
dotnet build
```

---

**Note:** ปัญหานี้เกิดจาก XAML parser ของ MAUI ที่ต้องการ UTF-8 with BOM อย่างเคร่งครัด
