using AuthService.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AuthService.Repository;

public class TokenRepository : ITokenRepository
{
    private readonly IMongoCollection<RefreshTokenModel> _tokens;

    public TokenRepository(IMongoDatabase database) => 
        _tokens = database.GetCollection<RefreshTokenModel>("RefreshTokens");

    public async Task CreateAsync(RefreshTokenModel token) => 
        await _tokens.InsertOneAsync(token);

    public async Task<RefreshTokenModel?> GetByUserIdAsync(ObjectId userId) =>
        await _tokens.Find(t => t.UserId == userId).FirstOrDefaultAsync(); 
    
    public async Task<RefreshTokenModel?> GetByTokenAsync(string token) =>
        await _tokens.Find(t => t.Token == token).FirstOrDefaultAsync(); 
    
    public async Task<RefreshTokenModel?> GetByTokenIdAsync(ObjectId tokenId) => 
        await _tokens.Find(t => t.Id == tokenId).FirstOrDefaultAsync();

    public async Task UpdateAsync(RefreshTokenModel token) => 
        await _tokens.ReplaceOneAsync(t => t.Id == token.Id, token);

    public async Task DeleteAsync(ObjectId tokenId) => 
        await _tokens.DeleteOneAsync(t => t.Id == tokenId);
}