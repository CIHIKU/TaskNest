using AuthService.Models.Requests;
using AuthService.Models.Responses;
using AuthService.Services;
using AuthService.Services.Interfaces;
using GraphQL;
using GraphQL.Types;
namespace AuthService.GraphQL;



public class AuthServiceSchema : Schema
{
    public AuthServiceSchema(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<AuthServiceQuery>();
        Mutation = serviceProvider.GetRequiredService<AuthServiceMutation>();
    }
}

public class AuthServiceQuery : ObjectGraphType
{

}

public class AuthServiceMutation : ObjectGraphType
{
    public AuthServiceMutation(ITokenService tokenService)
    {
        Initialize(tokenService);
    }
    
    private void Initialize(ITokenService tokenService)
    {
        var ipAddress = "";
        
        Field<RefreshTokenResponseType>("refreshToken")
            .Name("refreshToken")
            .Argument<NonNullGraphType<StringGraphType>>("refreshToken")
            .Argument<StringGraphType>("jwtToken")
            .ResolveAsync(async context =>
            {
                var refreshToken = context.GetArgument<string>("refreshToken");
                var jwtToken = context.GetArgument<string>("jwtToken");

                // Here, call your service to handle the token refresh
                var response = await tokenService.RefreshTokenAsync(new RefreshTokenRequest { RefreshToken = refreshToken, JwtToken = jwtToken }, ipAddress);

                return response;
            });
    }
}

public class RefreshTokenRequestType : InputObjectGraphType<RefreshTokenRequest>
{
    public RefreshTokenRequestType()
    {
        Initialize();
    }

    private void Initialize()
    {
        Name = "RefreshTokenRequest";
        Field(x => x.RefreshToken).Description("The refresh token.");
        Field(x => x.JwtToken, nullable: true).Description("The JWT token.");
    }
}

public class RefreshTokenResponseType : ObjectGraphType<RefreshTokenResponse>
{
    public RefreshTokenResponseType()
    {
        Initialize();
    }
    
    private void Initialize()
    {
        Name = "RefreshTokenResponse";
        Field(x => x.JwtToken).Description("The new JWT token.");
        Field(x => x.RefreshToken).Description("The new refresh token.");
    }
}