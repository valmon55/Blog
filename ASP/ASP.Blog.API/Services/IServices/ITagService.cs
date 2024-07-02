using ASP.Blog.API.ViewModels.Tag;
using System.Collections.Generic;

namespace ASP.Blog.API.Services.IServices
{
    public interface ITagService
    {
        public TagViewModel AddTag();
        public void AddTag(TagViewModel model);
        public List<TagViewModel> AllTags();
        public void DeleteTag(int id);
        public TagViewModel UpdateTag(int id);
        public void UpdateTag(TagViewModel model);

    }
}
