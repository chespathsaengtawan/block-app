using System.Text.RegularExpressions;
using BlockApp.Api.Data;
using BlockApp.Api.Services.Interfaces;
using BlockApp.Shared.DTOs.Blocklist;
using BlockApp.Shared.Entities;
using BlockApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace BlockApp.Api.Services;

public class BlocklistService : IBlocklistService
{
    private readonly AppDbContext _db;

    public BlocklistService(AppDbContext db)
    {
        _db = db;
    }

    // ──────────────────────────────────────────
    // GET — รายการบล็อกของผู้ใช้คนนี้
    // ──────────────────────────────────────────
    public async Task<IEnumerable<BlockEntryDto>> GetBlocklistAsync(int userId)
    {
        return await _db.UserBlockEntries
            .Where(u => u.UserId == userId)
            .Include(u => u.BlockEntry)
            .Select(u => new BlockEntryDto
            {
                UserBlockEntryId  = u.Id,
                BlockEntryId      = u.BlockEntryId,
                EntryType         = u.BlockEntry.EntryType,
                PhoneNumber       = u.BlockEntry.PhoneNumber,
                BankName          = u.BlockEntry.BankName,
                AccountNumber     = u.BlockEntry.AccountNumber,
                AccountHolderName = u.BlockEntry.AccountHolderName,
                Note              = u.Note,
                Reasons           = u.Reasons,
                OtherReason       = u.OtherReason,
                BlockedByCount    = u.BlockEntry.UserBlockEntries.Count,
                CreatedAt         = u.CreatedAt
            })
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    // ──────────────────────────────────────────
    // ADD — เพิ่มรายการบล็อก
    //   ถ้า BlockEntry มีอยู่แล้ว → เพิ่ม UserBlockEntry เท่านั้น
    // ──────────────────────────────────────────
    public async Task<AddBlockEntryResultDto> AddBlockEntryAsync(int userId, CreateBlockEntryDto dto)
    {
        BlockEntry? existing = null;

        if (dto.EntryType == BlockEntryType.Phone)
        {
            var e164 = ToE164(dto.PhoneNumber)
                ?? throw new ArgumentException("PhoneNumber is required for Phone entry type");

            dto.PhoneNumber = e164;
            existing = await _db.BlockEntries
                .FirstOrDefaultAsync(x => x.PhoneNumber == e164);
        }
        else // BankAccount
        {
            if (string.IsNullOrWhiteSpace(dto.AccountNumber))
                throw new ArgumentException("AccountNumber is required for BankAccount entry type");
            if (string.IsNullOrWhiteSpace(dto.BankName))
                throw new ArgumentException("BankName is required for BankAccount entry type");

            existing = await _db.BlockEntries
                .FirstOrDefaultAsync(x =>
                    x.BankName == dto.BankName &&
                    x.AccountNumber == dto.AccountNumber);
        }

        bool alreadyExisted = existing != null;

        if (existing == null)
        {
            existing = new BlockEntry
            {
                EntryType         = dto.EntryType,
                PhoneNumber       = dto.EntryType == BlockEntryType.Phone ? dto.PhoneNumber : null,
                BankName          = dto.EntryType == BlockEntryType.BankAccount ? dto.BankName : null,
                AccountNumber     = dto.EntryType == BlockEntryType.BankAccount ? dto.AccountNumber : null,
                AccountHolderName = dto.EntryType == BlockEntryType.BankAccount ? dto.AccountHolderName : null,
                AddedByUserId     = userId,
                CreatedAt         = DateTime.UtcNow
            };
            _db.BlockEntries.Add(existing);
            await _db.SaveChangesAsync();
        }
        else
        {
            // ถ้าผู้ใช้คนนี้บล็อกซ้ำแล้ว ให้คืนรายการเดิม
            var alreadyLinked = await _db.UserBlockEntries
                .FirstOrDefaultAsync(u => u.UserId == userId && u.BlockEntryId == existing.Id);

            if (alreadyLinked != null)
            {
                var count = await _db.UserBlockEntries.CountAsync(u => u.BlockEntryId == existing.Id);
                return new AddBlockEntryResultDto
                {
                    AlreadyExisted = true,
                    BlockedByCount = count,
                    Entry = ToDto(alreadyLinked, existing, count)
                };
            }
        }

        // สร้าง UserBlockEntry สำหรับผู้ใช้คนนี้
        var userEntry = new UserBlockEntry
        {
            UserId       = userId,
            BlockEntryId = existing.Id,
            Note         = dto.Note,
            Reasons      = dto.Reasons,
            OtherReason  = dto.OtherReason,
            CreatedAt    = DateTime.UtcNow
        };
        _db.UserBlockEntries.Add(userEntry);
        await _db.SaveChangesAsync();

        var blockedByCount = await _db.UserBlockEntries.CountAsync(u => u.BlockEntryId == existing.Id);

        return new AddBlockEntryResultDto
        {
            AlreadyExisted = alreadyExisted,
            BlockedByCount = blockedByCount,
            Entry = ToDto(userEntry, existing, blockedByCount)
        };
    }

    // ──────────────────────────────────────────
    // DELETE — ลบรายการบล็อกของผู้ใช้คนนี้
    // ──────────────────────────────────────────
    public async Task<bool> DeleteBlockEntryAsync(int userId, int userBlockEntryId)
    {
        var entity = await _db.UserBlockEntries
            .FirstOrDefaultAsync(u => u.Id == userBlockEntryId && u.UserId == userId);

        if (entity == null) return false;

        _db.UserBlockEntries.Remove(entity);
        await _db.SaveChangesAsync();

        // ถ้าไม่มีผู้ใช้คนไหนบล็อกแล้ว ลบ BlockEntry ด้วย
        var remaining = await _db.UserBlockEntries.CountAsync(u => u.BlockEntryId == entity.BlockEntryId);
        if (remaining == 0)
        {
            var blockEntry = await _db.BlockEntries.FindAsync(entity.BlockEntryId);
            if (blockEntry != null)
            {
                _db.BlockEntries.Remove(blockEntry);
                await _db.SaveChangesAsync();
            }
        }

        return true;
    }

    // ──────────────────────────────────────────
    // Helpers
    // ──────────────────────────────────────────

    /// <summary>แปลงเบอร์ไทย (0xx...) หรือเบอร์ที่มี +66 อยู่แล้ว → E.164</summary>
    public static string? ToE164(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber)) return null;

        var digits = Regex.Replace(phoneNumber, @"\D", "");

        if (digits.Length == 10 && digits.StartsWith("0"))
            return "+66" + digits[1..];

        if (digits.Length == 11 && digits.StartsWith("66"))
            return "+" + digits;

        if (phoneNumber.StartsWith("+"))
            return phoneNumber; // ส่งมาเป็น E.164 แล้ว

        return phoneNumber; // รูปแบบไม่รู้จัก — เก็บตามเดิม
    }

    private static BlockEntryDto ToDto(UserBlockEntry u, BlockEntry e, int count) => new()
    {
        UserBlockEntryId  = u.Id,
        BlockEntryId      = e.Id,
        EntryType         = e.EntryType,
        PhoneNumber       = e.PhoneNumber,
        BankName          = e.BankName,
        AccountNumber     = e.AccountNumber,
        AccountHolderName = e.AccountHolderName,
        Note              = u.Note,
        Reasons           = u.Reasons,
        OtherReason       = u.OtherReason,
        BlockedByCount    = count,
        CreatedAt         = u.CreatedAt
    };
}
