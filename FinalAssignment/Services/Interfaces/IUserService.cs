using Data.Auth;
using FinalAssignment.DTOs.User;

namespace FinalAssignment.Services.Interfaces
{
    public interface IUserService
    {
        Task<Response> Register(RegisterModelRequest model);
        Task<LoginResponse> Login(LoginRequest model);
        Task<Response> ResetPassword(ResetPasswordRequest model);
        Task<Response> EditUser(EditUserRequest model);
        Task<Response> DeleteUser(string userName);
        Task<IEnumerable<UserResponse>> GetAllUserDependLocation(string userName);
    }
}