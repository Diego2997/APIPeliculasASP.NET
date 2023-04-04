using APIPeliculas.Models;
using APIPeliculas.Models.Dtos;
using AutoMapper;

namespace APIPeliculas.FilmsMapperr
{
    public class FilmsMapper : Profile
    {
        public FilmsMapper()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Movie, MovieDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();
            CreateMap<User, UserLoginResponseDto>().ReverseMap();
            CreateMap<User, UserRegisterDto>().ReverseMap();
            CreateMap<AppUser, UserDataDto>().ReverseMap();
            CreateMap<AppUser, UserDto>().ReverseMap();

        }
    }
}
