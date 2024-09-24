using QUANLYVANHOA.Models;

namespace QUANLYVANHOA.Controllers
{
    public class SysUser
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string? FullName { get; set; }// alow null
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; }
        public string? Note { get; set; }

        // Refresh Token
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }

    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class SysUserInsertModel
    {
        public string UserName { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; }
        public string? Note { get; set; }
    }

    public class SysUserUpdateModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; }
        public string? Note { get; set; }
    }


    public class RegisterModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
    }

    public class UpdateRefreshTokenModel
    {
        public int UserID { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }

}
