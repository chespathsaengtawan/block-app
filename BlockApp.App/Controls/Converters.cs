using System.Globalization;
using System.Text;
using BlockApp.App.Models;
using BlockApp.App.Services;
using BlockApp.Shared.Enums;

namespace BlockApp.App.Controls;

/// <summary>Converts BlockEntryType to an emoji icon string.</summary>
public class BlockEntryTypeIconConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is BlockEntryType t && t == BlockEntryType.BankAccount ? "🏦" : "📞";

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

/// <summary>Returns true when the bound value is a non-null, non-empty string.</summary>
public class NullToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is string s && !string.IsNullOrWhiteSpace(s);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}


// Converter for bonus points visibility
public class IsNotZeroConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (value is int intValue)    return intValue > 0;
        if (value is Enum e)          return System.Convert.ToInt64(e) != 0;
        return false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>Converts BlockReason [Flags] enum to a Thai label string, e.g. "📵 โทรก่อกวน · 💸 หลอกโอนเงิน"</summary>
public class BlockReasonLabelConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not BlockReason reasons || reasons == BlockReason.None)
            return string.Empty;

        var sb = new StringBuilder();
        if (reasons.HasFlag(BlockReason.SpamCall))    Append(sb, "📵 โทรก่อกวน");
        if (reasons.HasFlag(BlockReason.Scam))        Append(sb, "💸 หลอกโอนเงิน");
        if (reasons.HasFlag(BlockReason.MuleAccount)) Append(sb, "🏦 บัญชีม้า");
        if (reasons.HasFlag(BlockReason.Other))       Append(sb, "📝 อื่นๆ");
        return sb.ToString();
    }

    private static void Append(StringBuilder sb, string text)
    {
        if (sb.Length > 0) sb.Append(" · ");
        sb.Append(text);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

/// <summary>Returns true when the BlockReason value has the specific flag named by ConverterParameter.</summary>
public class BlockReasonFlagConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not BlockReason reasons) return false;
        if (parameter is string s && Enum.TryParse<BlockReason>(s, out var flag) && flag != BlockReason.None)
            return reasons.HasFlag(flag);
        return false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

/// <summary>Converts BlockReason [Flags] to List&lt;string&gt; — one chip per active flag.</summary>
public class BlockReasonChipsConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not BlockReason reasons) return new List<string>();
        var chips = new List<string>();
        if (reasons.HasFlag(BlockReason.SpamCall))    chips.Add("📵 โทรก่อกวน");
        if (reasons.HasFlag(BlockReason.Scam))        chips.Add("💸 หลอกโอนเงิน");
        if (reasons.HasFlag(BlockReason.MuleAccount)) chips.Add("🏦 บัญชีม้า");
        if (reasons.HasFlag(BlockReason.Other))       chips.Add("📝 อื่นๆ");
        return chips;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

/// <summary>Converts a HistoryAction enum to a human-readable Thai label string.</summary>
public class HistoryActionLabelConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is HistoryAction action ? HistoryService.ActionLabel(action) : value?.ToString() ?? "";

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}