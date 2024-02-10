using System.Text.RegularExpressions;
using Auction.Common.Dtos.User;
using FluentValidation;

namespace EasyTest.WebAPI.Validation.Auth
{
	public class UserSignUpValidator : AbstractValidator<SignUpUserDto>
	{
        public UserSignUpValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Matches("^([a-zA-Z-]){2,25}$").WithMessage("First name format is invalid. It should contain 2 to 25 characters consisting of letters and hyphens.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .Matches("^([a-zA-Z-]){2,25}$").WithMessage("Last name format is invalid. It should contain 2 to 25 characters consisting of letters and hyphens.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .Matches("^([\\w-]+(\\.[\\w-]+|[\\w-]*))@([0-9a-zA-Z]+(\\.[\\da-zA-Z]+|[\\da-zA-Z]*))$").WithMessage("Invalid email address format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password is too short. Minimum length is 6 symbols.")
                .Matches("^(?=.*[a-z])(?=.*[A-Z])[a-zA-Z\\d@$!%*?&\\.]{6,25}$").WithMessage("Password must contain both letters and digits.")
                .Must(password => Regex.IsMatch(password, "[A-Z]")).WithMessage("Passwords must have at least one uppercase ('A'-'Z').")
                .Must(password => !Regex.IsMatch(password, @"^[\w\d]*$")).WithMessage("Passwords must have at least one non-alphanumeric character.");
        }
    }
}
