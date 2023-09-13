using AuthService.Models;
using MongoDB.Bson;
using SharedLibrary.Models;

namespace AuthService.Repository;

public interface ITokenRepository
{
    public Task CreateAsync(RefreshTokenModel token);

    public Task<RefreshTokenModel?> GetByUserIdAsync(ObjectId userId);

    public Task<RefreshTokenModel?> GetByTokenAsync(string token);

    public Task<RefreshTokenModel?> GetByTokenIdAsync(ObjectId tokenId);

    public Task UpdateAsync(RefreshTokenModel token);

    public Task DeleteAsync(ObjectId tokenId);
}