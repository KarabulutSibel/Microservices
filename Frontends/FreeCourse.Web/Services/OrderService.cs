using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Services;
using FreeCourse.Web.Models.FakePayment;
using FreeCourse.Web.Models.Order;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services
{
	public class OrderService : IOrderService
	{
		private readonly IPaymentService _paymentService;
		private readonly HttpClient _httpClient;
		private readonly IBasketService _basketService;
		private readonly ISharedIdentityService _sharedIdentityService;
		public OrderService(IPaymentService paymentService, HttpClient httpClient, IBasketService basketService, ISharedIdentityService sharedIdentityService)
		{
			_paymentService = paymentService;
			_httpClient = httpClient;
			_basketService = basketService;
			_sharedIdentityService = sharedIdentityService;
		}

		public async Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput)
		{
			var basket = await _basketService.Get();

			var paymentInfoInput = new PaymentInfoInput()
			{
				CardName = checkoutInfoInput.CardName,
				CardNumber = checkoutInfoInput.CardNumber,
				Expiration = checkoutInfoInput.Expiration,
				CVV = checkoutInfoInput.CVV,
				TotalPrice = basket.TotalPrice
			};

			var responsePayment = await _paymentService.ReceivePayment(paymentInfoInput);

			if (!responsePayment)
			{
				return new OrderCreatedViewModel { Error = "Ödeme alınamadı.", IsSuccessful = false };
			}

			var orderCreateInput = new OrderCreateInput()
			{
				BuyerId = _sharedIdentityService.GetUserId,
				Address = new AddressCreateInput()
				{
					Province = checkoutInfoInput.Province,
					District = checkoutInfoInput.District,
					Street = checkoutInfoInput.Street,
					ZipCode = checkoutInfoInput.ZipCode,
					Line = checkoutInfoInput.Line
				}
			};
			basket.BasketItems.ForEach(x =>
			{
				var orderItem = new OrderItemCreateInput { ProductId = x.CourseId, ProductName = x.CourseName, Price = x.GetCurrentPrice, PictureUrl = "" };
				orderCreateInput.OrderItems.Add(orderItem);
			});

			var response = await _httpClient.PostAsJsonAsync<OrderCreateInput>("order",orderCreateInput);

			if (!response.IsSuccessStatusCode)
			{
				return new OrderCreatedViewModel { Error = "Sipariş oluşturulamadı.", IsSuccessful = false };
			}

			var orderCreatedViewModel = await response.Content.ReadFromJsonAsync<Response<OrderCreatedViewModel>>();
			orderCreatedViewModel.Data.IsSuccessful = true;
			_basketService.Delete();
			return orderCreatedViewModel.Data;
		}

		public async Task<List<OrderViewModel>> GetOrder()
		{
			var response = await _httpClient.GetFromJsonAsync<Response<List<OrderViewModel>>>("order");
			return response.Data;
		}

		public Task SuspendOrder(CheckoutInfoInput checkoutInfoInput)
		{
			throw new NotImplementedException();
		}
	}
}
