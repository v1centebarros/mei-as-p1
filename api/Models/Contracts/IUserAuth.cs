using api.Models.DTOs;
using static api.Models.DTOs.ServiceResponses;

namespace api.Models.Contracts
{
    public interface IUserAuth
    {
        Task<GeneralResponse> Create(UserDTO userDTO);
        Task<LoginResponse> Login(LoginDTO loginDTO);
    }
}