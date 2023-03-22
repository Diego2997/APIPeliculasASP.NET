using APIPeliculas.Models;
using APIPeliculas.Models.Dtos;
using APIPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIPeliculas.Controllers
{
    //[Route("api/[controller]")]//Una opcion
    [ApiController]
    [Route("api/controller")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _ctRepo;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository ctrepo, IMapper mapper)
        {
            _ctRepo = ctrepo;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetCategories()
        {
            var listCategories = _ctRepo.GetCategories();
            var listCategoriesDto = new List<CategoryDto>();

            foreach (var category in listCategories)
            {
                listCategoriesDto.Add(_mapper.Map<CategoryDto>(category));
            }
            return Ok(listCategoriesDto);
        }


        [HttpGet("{id:int}")]
        [ActionName(nameof(GetCategory))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategory(int id)
        {
            var itemCategory = _ctRepo.GetCategory(id);
            if (itemCategory == null)
            {
                return NotFound();
            }
            var itemCategoryDto = _mapper.Map<CategoryDto>(itemCategory);
            return Ok(itemCategoryDto);
        }

        [HttpPost]
        [ProducesResponseType(201,Type =  typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if(createCategoryDto == null)
            {
                return BadRequest(ModelState);
            }
            if(_ctRepo.ExistCategory(createCategoryDto.Name))
            {
                ModelState.AddModelError("", "Category exist");
                return StatusCode(404, ModelState);
            }

            var category = _mapper.Map<Category>(createCategoryDto);
            if (!_ctRepo.CreateCategory(category))
            {
                ModelState.AddModelError("", $"something went wrong saving the record {category.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtAction(nameof(GetCategory), new { categoryId = category.Id });

        }
        [HttpPatch("{id:int}",Name = "UpdatePatchCategory")]
        [ProducesResponseType(201, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdatePatchCategory(int id,[FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (categoryDto == null || id != categoryDto.Id)
            {
                return BadRequest(ModelState);
            }
           

            var category = _mapper.Map<Category>(categoryDto);
            if (!_ctRepo.UpdateCategory(category))
            {
                ModelState.AddModelError("", $"something went wrong updating the registry {category.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        [HttpDelete("{id:int}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteCategory(int id)
        {
            if (!_ctRepo.ExistCategory(id))
            {
                return NotFound();
            }


            var category = _ctRepo.GetCategory(id);
            if (!_ctRepo.DeleteCategory(category))
            {
                ModelState.AddModelError("", $"something went wrong delete the registry {category.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
