using ASP.Blog.Data;
using ASP.Blog.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.Blog.DAL.Repositories
{
    public class TagRepository : Repository<Tag>
    {
        public TagRepository(BlogContext db) : base(db) { }

        public List<Tag> GetTags()
        {
            return Set.ToList();
        }
        public Tag GetTagById(int id)
        {
            return Set.AsEnumerable().Where(x => x.ID == id).FirstOrDefault();
        }
        public void AddTag(Tag tag)
        {
            Set.Add(tag);
        }
        public void UpdateTag(Tag tag, Tag newTag)
        {
            if (GetTags().Where(x => x.ID == tag.ID).FirstOrDefault() != null)
            {
                var item = new Tag()
                {
                    ID = newTag.ID,
                    Tag_Name = newTag.Tag_Name
                };

                Update(item);
            }
        }
        public void DeleteTag(Tag tag)
        {
            var item = GetTags().Where(x => x.ID == tag.ID).FirstOrDefault();
            if (item != null)
            {
                Delete(item);
            }
        }
    }
}
