using FluentValidation;

namespace EasyTest.WebAPI.Validation
{
	public class BaseValidator<T> : AbstractValidator<T>
	{
		protected bool BeNullableGuid(string? id)
		{
			if (id == null) return true;
			return Guid.TryParse(id, out _);
		}
		protected bool BeGuid(string id)
		{
			if (String.IsNullOrEmpty(id)) return false;
			return Guid.TryParse(id, out _);
		}
		protected bool BeValidDate(string? value)
		{
			if (value == null) return true;
			return DateTime.TryParse(value, out DateTime date) && date > DateTime.Now;
		}
	}
}
