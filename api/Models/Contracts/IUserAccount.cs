using api.Models.DTOs;
using static api.Models.DTOs.ServiceResponses;

namespace api.Models.Contracts
{
    public interface IUserAccount
    {
        Task<GeneralResponse> CreateAccount(UserDTO userDTO);
        Task<LoginResponse> Login(LoginDTO loginDTO);
    }
}