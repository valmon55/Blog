using ASP.Blog.API.ViewModels.Comment;
using FluentValidation;

namespace ASP.Blog.API.Validators
{
    public class CommentViewModelValidator : AbstractValidator<CommentViewModel>
    {
        public CommentViewModelValidator() 
        { 
            RuleFor(x => x.Comment).NotEmpty().WithMessage("Комментарий не должен быть пуст!");
        }
    }
}
