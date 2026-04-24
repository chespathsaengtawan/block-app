using BlockApp.Shared.DTOs.Blocklist;

namespace BlockApp.Api.Services.Interfaces;

public interface IBlocklistService
{
    Task<IEnumerable<BlockEntryDto>> GetBlocklistAsync(int userId);
    Task<AddBlockEntryResultDto> AddBlockEntryAsync(int userId, CreateBlockEntryDto dto);
    Task<bool> DeleteBlockEntryAsync(int userId, int userBlockEntryId);
}
