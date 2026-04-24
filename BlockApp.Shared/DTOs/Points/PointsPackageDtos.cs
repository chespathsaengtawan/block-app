namespace BlockApp.Shared.DTOs.Points;

public record CreatePackageDto(
    int Points,
    decimal PriceTHB,
    int? BonusPoints,
    bool IsActive,
    int DisplayOrder
);

public record UpdatePackageDto(
    int Points,
    decimal PriceTHB,
    int? BonusPoints,
    bool IsActive,
    int DisplayOrder
);
