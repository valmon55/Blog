using ASP.Blog.BLL.ViewModels.Tag;
using ASP.Blog.Data.Entities;
using System.Runtime.CompilerServices;

namespace ASP.Blog.BLL.Extentions
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
