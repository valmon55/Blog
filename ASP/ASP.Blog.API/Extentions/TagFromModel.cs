using ASP.Blog.API.Data.Entities;
using ASP.Blog.API.ViewModels.Tag;

namespace ASP.Blog.API.Extentions
{
    public static class TagFromModel
    {
        public static Tag Convert(this Tag tag, TagViewModel tagViewModel)
        {
            tag.ID = tagViewModel.Id;
            tag.Tag_Name = tagViewModel.Tag_Name;

            return tag;
        }
    }
}
