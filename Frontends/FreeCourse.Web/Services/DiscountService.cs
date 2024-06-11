using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models.Discount;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FreeCourse.Web.Services
{
	public class DiscountService : IDiscountService
	{
		private readonly HttpClient _httpClient;

		public DiscountService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<DiscountViewModel> GetDiscount(string discountCode)
		{
			//[controller] / [action] /{ code}
			var response = await _httpClient.GetAsync($"discount/GetByCode/{discountCode}");
			if (!response.IsSuccessStatusCode)
			{
				return null;
			}

			var discount = await response.Content.ReadFromJsonAsync<Response<DiscountViewModel>>();

			return discount.Data;
		}
	}
}
