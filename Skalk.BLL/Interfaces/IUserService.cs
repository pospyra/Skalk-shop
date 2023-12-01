using Skalk.Common.DTO.User;

namespace Skalk.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<NewUserDTO> Registration(NewUserDTO newUserDTO);

        public Task<string> Login(LoginUserDTO userLogin);

        public Task<UserDTO> GetUserByIdAsync(int id);

        public Task<UserDTO> GetCurrentUserAsync();

        public Task<int> GetCurrentUserIdAsync();

        public Task DeleteCurrentUserAsync();


       // public Task<UserDTO> UpdateUserAsync(UpdateUserDTO user);
    }
}
