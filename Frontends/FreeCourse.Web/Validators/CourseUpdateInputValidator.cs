﻿using FluentValidation;
using FreeCourse.Web.Models.Catalog;

namespace FreeCourse.Web.Validators
{
	public class CourseUpdateInputValidator : AbstractValidator<CourseUpdateInput>
	{
		public CourseUpdateInputValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("İsim alanı boş olamaz.");
			RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama alanı boş olamaz.");
			RuleFor(x => x.Feature.Duration).InclusiveBetween(1, int.MaxValue).WithMessage("Süre alanı boş olamaz.");
			RuleFor(x => x.Price).NotEmpty().WithMessage("Fiyat alanı boş olamaz.").PrecisionScale(6, 2, true).WithMessage("Hatalı fiyat formatı");
		}
	}
}
