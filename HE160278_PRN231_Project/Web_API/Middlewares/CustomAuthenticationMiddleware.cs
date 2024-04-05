using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Web_API.Models.Dto;

namespace Web_API.Middlewares
{
    public class CustomAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var configuration = context.RequestServices.GetRequiredService<IConfiguration>();
            var unauthenticatedPaths = configuration.GetSection("UnauthenticatedPaths").Get<string[]>();

            // Kiểm tra xem đường dẫn của yêu cầu có trong danh sách không cần xác thực không
            if (!unauthenticatedPaths.Any(p => context.Request.Path.StartsWithSegments(p, StringComparison.OrdinalIgnoreCase)))
            {
                // Thực hiện xác thực token
                string token = context.Request.Cookies["UserLoginCookie"];
                ;
                // Xử lý logic xác thực token ở đây

                // Nếu xác thực không thành công, trả về lỗi 401 Unauthorized
                if (string.IsNullOrEmpty(token) || !IsValidToken(token))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }
            }

            // Chuyển request tới middleware hoặc endpoint tiếp theo trong pipeline
            await _next(context);
        }

        private bool IsValidToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_super_secret_key_with_at_least_32_characters"));
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = "https://localhost:7181",
                    ValidAudience = "https://localhost:7181",
                    IssuerSigningKey = securityKey,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true // Kiểm tra thời gian hết hạn của token
                };

                // Thực hiện xác thực token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);

                // Kiểm tra xem token có hợp lệ không
                return true;
            }
            catch (Exception ex)
            {
                // Xác thực thất bại, token không hợp lệ
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
