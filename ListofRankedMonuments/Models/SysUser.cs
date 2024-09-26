using QUANLYVANHOA.Models;
using System.Text.Json.Serialization;

namespace QUANLYVANHOA.Controllers
{
    public class SysUser
    {
        [JsonPropertyName("UserID")]
        public int UserID { get; set; }

        [JsonPropertyName("UserName")]
        public string UserName { get; set; }

        [JsonPropertyName("FullName")]
        public string? FullName { get; set; }// alow null

        [JsonPropertyName("Email")]
        public string Email { get; set; }

        [JsonPropertyName("Password")]
        public string Password { get; set; }

        [JsonPropertyName("Status")]
        public bool Status { get; set; }

        [JsonPropertyName("Note")]
        public string? Note { get; set; }

        // Refresh Token
        [JsonPropertyName("RefreshToken")]
        public string? RefreshToken { get; set; }

        [JsonPropertyName("RefreshTokenExpiryTime")]
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }

    public class LoginModel
    {
        [JsonPropertyName("UserName")]
        public string UserName { get; set; }
        [JsonPropertyName("Password")]
        public string Password { get; set; }
    }
    public class SysUserInsertModel
    {
        [JsonPropertyName("UserName")]
        public string UserName { get; set; }

        [JsonPropertyName("FullName")]
        public string? FullName { get; set; }// alow null

        [JsonPropertyName("Email")]
        public string Email { get; set; }

        [JsonPropertyName("Password")]
        public string Password { get; set; }

        [JsonPropertyName("Status")]
        public bool Status { get; set; }

        [JsonPropertyName("Note")]
        public string? Note { get; set; }

    }

    public class SysUserUpdateModel
    {
        [JsonPropertyName("UserID")]
        public int UserID { get; set; }

        [JsonPropertyName("UserName")]
        public string UserName { get; set; }

        [JsonPropertyName("FullName")]
        public string? FullName { get; set; }// alow null

        [JsonPropertyName("Email")]
        public string Email { get; set; }

        [JsonPropertyName("Password")]
        public string Password { get; set; }

        [JsonPropertyName("Status")]
        public bool Status { get; set; }

        [JsonPropertyName("Note")]
        public string? Note { get; set; }

    }


    public class RegisterModel
    {
        [JsonPropertyName("UserName")]
        public string UserName { get; set; }

        [JsonPropertyName("Password")]
        public string Password { get; set; }

        [JsonPropertyName("ConfirmPassword")]
        public string ConfirmPassword { get; set; }

        [JsonPropertyName("Email")]
        public string Email { get; set; }
    }

    public class UpdateRefreshTokenModel
    {
        [JsonPropertyName("UserID")]
        public int UserID { get; set; }

        [JsonPropertyName("RefreshToken")]
        public string? RefreshToken { get; set; }

        [JsonPropertyName("RefreshTokenExpiryTime")]
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }

}
