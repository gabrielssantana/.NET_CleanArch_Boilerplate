using AC.Core.Domain.Interfaces.Repositories;

using FluentValidation;

namespace AC.Core.Application.Commands.Users.AddUser
{
    public class AddUserCommandValidation : AbstractValidator<AddUserCommand>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        public AddUserCommandValidation(
            ICompanyRepository companyRepository,
            IUserRepository userRepository
        )
        {
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

            RuleFor(c => c.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Nome é obrigatório")
            .MaximumLength(100)
            .WithMessage("Nome deve conter no máximo 100 caracteres");

            RuleFor(c => c.Document)
            .NotNull()
            .NotEmpty()
            .WithMessage("Documento é obrigatório")
            .MaximumLength(14)
            .WithMessage("Documento deve conter no máximo 14 caracteres");

            RuleFor(c => c.Phone)
            .MaximumLength(200)
            .WithMessage("Telefone deve conter no máximo 200 caracteres"); ;

            RuleFor(c => c.Status);

            RuleFor(c => c.Email)
            .NotNull()
            .NotEmpty()
            .WithMessage("Email é obrigatório")
            .MaximumLength(100)
            .WithMessage("Email deve conter no máximo 100 caracteres")
            .MustAsync(async (c, cancellationToken) =>
            {
                var user = await _userRepository.GetByEmailAsync(c, cancellationToken);
                return user is null;
            })
            .WithMessage("Email já cadastrado")
            .When(c => c.Email is not null);

            RuleFor(c => c.CompanyId)
            .MustAsync(async (c, cancellationToken) =>
            {
                //Aqui o uso do "!" para forçar a não nulabilidade do valor só há pois IRuleBuilderOptions.When foi utilizado.
                var company = await _companyRepository.GetByIdAsync(c!.Value, cancellationToken);
                return company is not null;
            })
            .WithMessage("Empresa não encontrada")
            .When(c => c.CompanyId.HasValue);
        }
    }
}