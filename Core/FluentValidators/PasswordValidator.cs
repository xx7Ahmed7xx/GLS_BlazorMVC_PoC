using FluentValidation;
using GLS_BlazorMVC_PoC.Models;

namespace GLS_BlazorMVC_PoC.FluentValidators
{
    public class PasswordValidator : AbstractValidator<Password>
    {
        public PasswordValidator()
        {
            RuleFor(password => password.EncryptedPassword).NotNull();
            RuleFor(password => password.EncryptedPassword).NotEmpty();
            RuleFor(password => password.EncryptedPassword).MinimumLength(4);
        }
    }
}
