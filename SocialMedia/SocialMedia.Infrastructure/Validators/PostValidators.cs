using FluentValidation;
using SocialMedia.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Infrastructure.Validators
{
    public class PostValidators : AbstractValidator<PostDto>
    {
        public PostValidators()
        {
            RuleFor(post => post.Description)
                .NotNull()
                .Length(1, 1000);
            RuleFor(post => post.Date)
               .NotNull();
        }
    }
}
