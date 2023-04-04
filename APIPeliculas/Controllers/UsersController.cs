using APIPeliculas.Models;
using APIPeliculas.Models.Dtos;
using APIPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace APIPeliculas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserRepository _usRepo;
        private readonly IMapper _mapper;
        protected ResponseApi _responseApi;

        public UsersController(IUserRepository usrepo, IMapper mapper)
        {
            _usRepo = usrepo;
            _responseApi = new();
            _mapper = mapper;
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetUsers()
        {
            var listUser = _usRepo.GetUsers();
            var listUserDto = new List<UserDto>();

            foreach (var user in listUser)
            {
                listUserDto.Add(_mapper.Map<UserDto>(user));
            }
            return Ok(listUserDto);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        [ActionName(nameof(GetUser))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUser(string id)
        {
            var user = _usRepo.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            var itemUserDto = _mapper.Map<UserDto>(user);
            return Ok(itemUserDto);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
           bool validUserNameUnique = _usRepo.IsUniqueUser(userRegisterDto.UserName);
            if (!validUserNameUnique)
            {
                _responseApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _responseApi.IsSuccess = false;
                _responseApi.ErrorMessages.Add("Username exist");
                return BadRequest();
            }
            var user = await _usRepo.Register(userRegisterDto);
            if(user == null)
            {
                _responseApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _responseApi.IsSuccess = false;
                _responseApi.ErrorMessages.Add("Error register");
                return BadRequest(_responseApi);
            }

            _responseApi.StatusCode = System.Net.HttpStatusCode.OK;
            _responseApi.IsSuccess = true;
            return Ok(_responseApi);

        }
        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var responseLogin = await _usRepo.Login(userLoginDto);

            if (responseLogin.User == null || string.IsNullOrEmpty(responseLogin.Token))
            {
                _responseApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _responseApi.IsSuccess = false;
                _responseApi.ErrorMessages.Add("Username or password is incorrect");
                return BadRequest(_responseApi);
            }
            _responseApi.StatusCode = System.Net.HttpStatusCode.OK;
            _responseApi.IsSuccess = true;
            _responseApi.Result = responseLogin;
            return Ok(_responseApi);

        }
    }
}
