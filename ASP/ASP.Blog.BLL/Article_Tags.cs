using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.Blog.BLL
{
    public class Article_Tags
    {
        public Guid ID { get; set; }
        public User User { get; set; }
        public Tag Tag { get; set; }
        public Article_Tags(Guid ID, User User, Tag Tag) 
        {
            this.ID = ID;
            this.User = User;
            this.Tag = Tag;
        }
    }
}
