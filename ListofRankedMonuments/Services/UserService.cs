using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.IdentityModel.Tokens;
using QUANLYVANHOA.Interfaces;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using QUANLYVANHOA.Services;
using Newtonsoft.Json;

public class UserService : IUserService
{
    private readonly ISysUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public UserService(ISysUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<(bool IsValid, string Token, string RefreshToken, string Message)> AuthenticateUser(string userName, string password)
    {
        var user = await _userRepository.VerifyLogin(userName, password);

        if (user == null)
        {
            return (false, null, null, "Invalid username or password.");
        }

        // Lấy danh sách các quyền của người dùng từ cơ sở dữ liệu
        var permissions = CustomAuthorizeAttribute.GetAllUserFunctionsAndPermissions(userName);

        var jwtSettings = _configuration.GetSection("Jwt");
        var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, user.Email),
                new Claim("FunctionsAndPermissions", JsonConvert.SerializeObject(permissions)) // Lưu các quyền của từng chức năng vào JWT
            }),
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"])),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

        // Tạo refresh token và lưu trữ
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Refresh token có thời hạn 7 ngày
        await _userRepository.UpdateRefreshToken(user);

        return (true, token, refreshToken, "Login successful.");
    }

    public async Task<(string Token, string RefreshToken)> RefreshToken(string refreshToken)
    {
        var user = await _userRepository.GetByRefreshToken(refreshToken);
        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return (null, null); // Refresh token không hợp lệ hoặc đã hết hạn
        }
        var permissions = CustomAuthorizeAttribute.GetAllUserFunctionsAndPermissions(user.UserName);

        var jwtSettings = _configuration.GetSection("Jwt");
        var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Email),
                new Claim("FunctionsAndPermissions", JsonConvert.SerializeObject(permissions))
            }),
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"])),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var newToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

        // Cập nhật refresh token mới nếu cần thiết
        var newRefreshToken = GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userRepository.UpdateRefreshToken(user);

        return (newToken, newRefreshToken);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
