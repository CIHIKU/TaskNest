using AuthService.Models.Requests;
using AuthService.Models.Responses;
using AuthService.Services;
using AuthService.Services.Interfaces;

namespace AuthService.GraphQL.Resolvers;

public class Mutation
{
    private readonly ITokenService _tokenService;

    public Mutation(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<RefreshTokenResponse> RefreshToken(string refreshToken, string jwtToken)
    {
        // Delegate to the token service to handle the business logic
        var response = await _tokenService.RefreshTokenAsync(new RefreshTokenRequest { RefreshToken = refreshToken, JwtToken = jwtToken });
        return response;
    }
}