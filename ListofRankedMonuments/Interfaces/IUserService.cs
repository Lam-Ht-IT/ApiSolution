using System.Threading.Tasks;

namespace QUANLYVANHOA.Interfaces
{
    public interface IUserService
    {
        Task<(bool IsValid, string Token, string Message)> AuthenticateUser(string userName, string password);
    }
}