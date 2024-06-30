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
        public void UpdateTag(Tag tag)
        {
            Update(tag);
        }
        public void DeleteTag(Tag tag)
        {
            Delete(tag);
        }
    }
}
