using APIPeliculas.Models;
using APIPeliculas.Models.Dtos;

namespace APIPeliculas.Repository.IRepository
{
    public interface IUserRepository
    {
        ICollection<AppUser> GetUsers();
        AppUser GetUser(string id);
        bool IsUniqueUser(string user);
        Task<UserLoginResponseDto> Login(UserLoginDto userLogin);
        //Task<User> Register(UserRegisterDto userRegister);
        Task<UserDataDto> Register(UserRegisterDto userRegister);
    }
}
