using ASP.Blog.BLL.ViewModels.Tag;


namespace ASP.Blog.Services.IServices
{
    public interface ITagService
    {
        public TagViewModel AddTag();
        public void AddTag(TagViewModel model);
    }
}
