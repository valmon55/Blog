using ASP.Blog.API.ViewModels.Tag;
using System.Collections.Generic;

namespace ASP.Blog.API.Services.IServices
{
    public interface ITagService
    {
        public void AddTag(TagViewModel model);
        public List<TagViewModel> AllTags();
        public void DeleteTag(int id);
        public void UpdateTag(TagViewModel model);

    }
}
