using APIPeliculas.Data;
using APIPeliculas.Models;
using APIPeliculas.Models.Dtos;
using APIPeliculas.Repository.IRepository;
using System.Text;
//using XSystem.Security.Cryptography;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

namespace APIPeliculas.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly APIContext _context;
        private string _secretPass;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        public UserRepository(APIContext context,IConfiguration config, RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager, IMapper mapper)
        {
            _context = context;
            _secretPass = config.GetValue<string>("ApiSettings:Secret");
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }
        public AppUser GetUser(string userId)
        {
            return _context.AppUsers.FirstOrDefault(u => u.Id == userId);
        }

        public ICollection<AppUser> GetUsers()
        {
            return _context.AppUsers.OrderBy(u => u.UserName).ToList();
        }

        public bool IsUniqueUser(string user)
        {
            var userBD = _context.AppUsers.FirstOrDefault(u => u.UserName == user);
            if (userBD == null)
            {
                return true;
            }
            else return false;
        }

        public async Task<UserLoginResponseDto> Login(UserLoginDto userLogin)
        {
            //var password = GetSHA256(userLogin.Password);
            var user = _context.AppUsers.FirstOrDefault(
                u => u.UserName.ToLower() == userLogin.UserName.ToLower()
                //&& u.Password == password
                );
            bool isValid = await _userManager.CheckPasswordAsync(user, userLogin.Password);
            if (user == null || isValid == false)
            {
                return new UserLoginResponseDto()
                {
                    Token = "",
                    User = null
                };
            }

            //Aqui ya existe el user
            var roles = await _userManager.GetRolesAsync(user);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key =  Encoding.ASCII.GetBytes(_secretPass);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };

            var token =  tokenHandler.CreateToken(tokenDescriptor);
            UserLoginResponseDto userDto = new UserLoginResponseDto()
            {
                Token = tokenHandler.WriteToken(token),
                User = _mapper.Map<UserDataDto>(user),
            };

            return userDto;
        }

        public async Task<UserDataDto> Register(UserRegisterDto userRegister)
        {
            var password = GetSHA256(userRegister.Password);

            AppUser user = new()
            {
                UserName = userRegister.UserName,
                Email = userRegister.UserName,
                NormalizedEmail = userRegister.UserName.ToUpper(),
                Nombre = userRegister.UserName
            };
            var result = await _userManager.CreateAsync(user,userRegister.Password);
            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _roleManager.CreateAsync(new IdentityRole("registrado"));
                }
                await _userManager.AddToRoleAsync(user, "registrado");
                var userReturn = _context.AppUsers.FirstOrDefault(u => u.UserName == userRegister.UserName);
                //return new UserDataDto()
                //{
                //    Id = userReturn.Id,
                //    UserName = userReturn.UserName,
                //    Name = userReturn.Nombre
                //};
                return _mapper.Map<UserDataDto>(userReturn);
            }
            //_context.Users.Add(user);
            //await _context.SaveChangesAsync();
            //user.Password = password;
            //return user;
            return new UserDataDto();
        }

        //public static string Obtainmd5(string password)
        //{
        //    MD5CryptoServiceProvider encrypt = new MD5CryptoServiceProvider(); //OBSOLETO
        //    byte[] data = System.Text.Encoding.UTF8.GetBytes(password);
        //    data = encrypt.ComputeHash(data);
        //    string resp = "";
        //    for (int i = 0; i < data.Length; i++)
        //    {
        //        resp += data[i].ToString("x2").ToLower();
        //    }
        //    return resp;
        //}
        public static string GetSHA256(string password)
        {
            SHA256 sha256 = SHA256.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(password));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
    }
}
