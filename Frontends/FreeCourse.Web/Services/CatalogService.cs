using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Helpers;
using FreeCourse.Web.Models.Catalog;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services
{
	public class CatalogService : ICatalogService
	{
		private readonly HttpClient _httpClient;
		private readonly IPhotoStockService _photoStockService;
		private readonly PhotoHelper _photoHelper;
		public CatalogService(HttpClient httpClient, IPhotoStockService photoStockService, PhotoHelper photoHelper)
		{
			_httpClient = httpClient;
			_photoStockService = photoStockService;
			_photoHelper = photoHelper;
		}

		public async Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput)
		{
			var resultPhotoService = await _photoStockService.UploadPhoto(courseCreateInput.PhotoFormFile);

			if (resultPhotoService != null)
			{
				courseCreateInput.Picture = resultPhotoService.Url;
			}

			var response = await _httpClient.PostAsJsonAsync<CourseCreateInput>("course", courseCreateInput);

			return response.IsSuccessStatusCode;
		}

		public async Task<bool> DeleteCourseAsync(string courseId)
		{
			var response = await _httpClient.DeleteAsync($"course/{courseId}");

			return response.IsSuccessStatusCode;
		}

		public async Task<List<CategoryViewModel>> GetAllCategoryAsync()
		{
			var response = await _httpClient.GetAsync("category");
			if (!response.IsSuccessStatusCode)
			{
				return null;
			}

			var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();

			return responseSuccess.Data;
		}

		public async Task<List<CourseViewModel>> GetAllCourseAsync()
		{
			var response = await _httpClient.GetAsync("course");
			if (!response.IsSuccessStatusCode)
			{
				return null;
			}

			var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();

			responseSuccess.Data.ForEach(x =>
			{
				x.StockPictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture);
			});

			return responseSuccess.Data;
		}

		public async Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId)
		{
			var response = await _httpClient.GetAsync($"course/GetAllByUserId/{userId}");
			if (!response.IsSuccessStatusCode)
			{
				return null;
			}

			var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();

			responseSuccess.Data.ForEach(x =>
			{
				x.StockPictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture);
			});

			return responseSuccess.Data;
		}

		public async Task<CourseViewModel> GetByCourseId(string courseId)
		{
			var response = await _httpClient.GetAsync($"course/{courseId}");
			
			if(!response.IsSuccessStatusCode) 
			{
				return null;
			}

			var responseSuccess = await response.Content.ReadFromJsonAsync<Response<CourseViewModel>>();
			responseSuccess.Data.StockPictureUrl = _photoHelper.GetPhotoStockUrl(responseSuccess.Data.Picture);
			return responseSuccess.Data;
		}

		public async Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput)
		{
			var resultPhotoService = await _photoStockService.UploadPhoto(courseUpdateInput.PhotoFormFile);

			if (resultPhotoService != null)
			{
				await _photoStockService.DeletePhoto(courseUpdateInput.Picture);
				courseUpdateInput.Picture = resultPhotoService.Url;
			}

			var response = await _httpClient.PutAsJsonAsync<CourseUpdateInput>("course",courseUpdateInput);

			return response.IsSuccessStatusCode;
		}
	}
}
