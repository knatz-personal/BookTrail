namespace BookTrail.API.Auth
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this RouteGroupBuilder group)
        {
            group.MapPost("register", Register)
                .WithName("Register")
                .WithDescription("Register a new user")
                .WithOpenApi();

            group.MapPost("login", Login)
                .WithName("Login")
                .WithDescription("Login an existing user")
                .WithOpenApi();
        }

        private static async Task<IResult> Register(
            RegisterDto registerDto,
            IAuthService authService)
        {
            AuthResponseDto result = await authService.RegisterAsync(registerDto);

            if (!result.Success)
            {
                return Results.BadRequest(result);
            }

            return Results.Ok(result);
        }

        private static async Task<IResult> Login(
            LoginDto loginDto,
            IAuthService authService)
        {
            AuthResponseDto result = await authService.LoginAsync(loginDto);

            if (!result.Success)
            {
                return Results.BadRequest(result);
            }

            return Results.Ok(result);
        }
    }
}