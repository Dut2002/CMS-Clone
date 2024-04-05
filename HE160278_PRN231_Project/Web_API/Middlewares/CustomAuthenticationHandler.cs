using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Web_API.Middlewares
{
    public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public CustomAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Lấy token từ request header
            string token = Request.Headers["HeaderAuthority"].FirstOrDefault()?.Split(" ").Last();

            // Nếu token không tồn tại hoặc không bắt đầu bằng "Bearer ", trả về lỗi xác thực
            if (token == null || !token.StartsWith("Bearer "))
            {
                return AuthenticateResult.Fail("Invalid or missing authorization header");
            }

            // Loại bỏ phần "Bearer " ra khỏi token
            token = token.Substring(7);

            try
            {
                // Khóa bí mật để xác thực token
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_super_secret_key_with_at_least_32_characters"));

                // Xác thực token
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "https://localhost:7181",
                    ValidAudience = "https://localhost:7181",
                    IssuerSigningKey = key
                }, out SecurityToken validatedToken);

                // Kiểm tra xem token đã hết hạn chưa
                if (validatedToken.ValidTo < DateTime.UtcNow)
                {
                    return AuthenticateResult.Fail("Token has expired");
                }

                // Nếu token hợp lệ, trả về thông tin người dùng
                var identity = new ClaimsIdentity(principal.Identity);
                var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                // Xác thực thất bại
                return AuthenticateResult.Fail("Failed to authenticate token\n"+ex.Message);
            }
        }
    }
}
