using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.Blog.BLL
{
    public class Article
    {
        public Guid ID { get; set; }
        public string Content { get; set; }
        public User User { get; set; }
        public Article(Guid ID, string Content, User User) 
        { 
            this.ID = ID;
            this.Content = Content;
            this.User = User;
        }
    }
}
