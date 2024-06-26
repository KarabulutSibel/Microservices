﻿namespace FreeCourse.Web.Models.Basket
{
	public class BasketViewModel
	{
		public string? UserId { get; set; }
		public string? DiscountCode { get; set; }
		public int? DiscountRate { get; set; }
		private List<BasketItemViewModel> _basketItems = new();

		public List<BasketItemViewModel> BasketItems
		{
			get
			{
				if (HasDiscount)
				{
					_basketItems.ForEach(x =>
					{
						var discountPrice = x.Price * ((decimal)DiscountRate.Value / 100);
						x.AppliedDiscount(Math.Round(x.Price - discountPrice, 2));
					});
				}
				return _basketItems;
			}
			set
			{
				_basketItems = value;
			}
		}
		public decimal TotalPrice { get => _basketItems.Sum(x => x.GetCurrentPrice * x.Quantity); }

		public bool HasDiscount
		{
			get => !string.IsNullOrEmpty(DiscountCode) && DiscountRate.HasValue;
		}

		public void CancelDiscount()
		{
			DiscountCode = null;
			DiscountRate = null;
		}

		public void ApplyDiscount(string code, int rate)
		{
			DiscountCode = code;
			DiscountRate = rate;
		}
	}
}