using FreeCourse.Services.FakePayment.Models;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Messages;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.FakePayment.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FakePaymentController : CustomBaseController
	{
		private readonly ISendEndpointProvider _sendEndpointProvider;

		public FakePaymentController(ISendEndpointProvider sendEndpointProvider)
		{
			_sendEndpointProvider = sendEndpointProvider;
		}

		[HttpPost]
		public async Task<IActionResult> ReceivePayment(PaymentDto paymentDto)
		{
			var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));

			var createOrderMessageCommand = new CreateOrderMessageCommand();
			createOrderMessageCommand.BuyerId = paymentDto.Order.BuyerId;
			createOrderMessageCommand.Province = paymentDto.Order.Address.Province;
			createOrderMessageCommand.District = paymentDto.Order.Address.District;
			createOrderMessageCommand.Street = paymentDto.Order.Address.Street;
			createOrderMessageCommand.ZipCode = paymentDto.Order.Address.ZipCode;
			createOrderMessageCommand.Line = paymentDto.Order.Address.Line;

			paymentDto.Order.OrderItems.ForEach(x =>
			{
				createOrderMessageCommand.OrderItems.Add(new OrderItem
				{
					PictureUrl = x.PictureUrl,
					Price = x.Price,
					ProductId = x.ProductId,
					ProductName = x.ProductName
				});
			});

			await sendEndpoint.Send<CreateOrderMessageCommand>(createOrderMessageCommand);
			return CreateActionResultInstance(Shared.Dtos.Response<NoContent>.Success(200));
		}
	}
}
