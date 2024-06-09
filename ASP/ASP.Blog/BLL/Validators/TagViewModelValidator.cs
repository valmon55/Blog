using ASP.Blog.BLL.ViewModels.Tag;
using FluentValidation;

namespace ASP.Blog.BLL.Validators
{
    public class TagViewModelValidator : AbstractValidator<TagViewModel>
    {
        public TagViewModelValidator() 
        { 
            RuleFor(x => x.Tag_Name).NotEmpty().WithMessage("Название тега не должно быть пусто!");
        }
    }
}
