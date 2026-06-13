using FluentValidation;
using Template.Core.Dtos;

namespace Template.Infrastructure.Validators
{
    public class CervezasValidators : AbstractValidator<CervezasDto>
    {
        public CervezasValidators()
        {
            RuleFor(cerveza => cerveza.Nombre)
                .NotNull()
                .WithMessage("El nombre no puede ser nulo");

            RuleFor(cerveza => cerveza.Nombre)
                .Length(1, 100)
                .WithMessage("El nombre debe tener entre 1 y 100 caracteres");

            RuleFor(cerveza => cerveza.GradosAlcohol)
                .InclusiveBetween(0, 100)
                .WithMessage("Los grados de alcohol deben estar entre 0 y 100");

            RuleFor(cerveza => cerveza.Precio)
                .GreaterThan(0)
                .WithMessage("El precio debe ser mayor a cero");
        }
    }
}
