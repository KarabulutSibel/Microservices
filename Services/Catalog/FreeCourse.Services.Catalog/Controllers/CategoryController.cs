using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Services;
using FreeCourse.Shared.ControllerBases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;

namespace FreeCourse.Services.Catalog.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : CustomBaseController
	{
		private readonly ICategoryService _categoryService;

		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var categories = await _categoryService.GetAllAsync();
			return CreateActionResultInstance(categories);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(string id)
		{
			var category = await _categoryService.GetByIdAsync(id);
			return CreateActionResultInstance(category);
		}

		[HttpPost]
		public async Task<IActionResult> Create(CategoryDto categoryDto)
		{
			var response = await _categoryService.CreateAsync(categoryDto);
			return CreateActionResultInstance(response);
		}
	}
}
