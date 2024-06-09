using ASP.Blog.BLL.ViewModels.Comment;
using FluentValidation;

namespace ASP.Blog.BLL.Validators
{
    public class CommentViewModelValidator : AbstractValidator<CommentViewModel>
    {
        public CommentViewModelValidator() 
        { 
            RuleFor(x => x.Comment).NotEmpty().WithMessage("Комментарий не должен быть пуст!");
        }
    }
}
