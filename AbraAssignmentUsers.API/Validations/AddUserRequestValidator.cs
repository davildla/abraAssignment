namespace AbraAssignmentUsers.API.Validations
{
    public class AddUserRequestValidator : AbstractValidator<Models.DTO.AddUserRequest>
    {
        public AddUserRequestValidator()
        {   // all fields are mendatory
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email address is required")
                     .EmailAddress().WithMessage("A valid email is required");
            RuleFor(x => x.Gender).NotEmpty().WithMessage("Gender is required");
            RuleFor(x => x.Phonce).NotEmpty().WithMessage("Phonce is required");
            RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required");
        }
    }
}
