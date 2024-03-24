using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.Blog.BLL
{
    public class Comment
    {
        public Guid ID { get; set; }
        public string Comment_Text { get; set; }
        public Article Article { get; set; }
        public Comment(Guid ID, string Comment_Text, Article Article) 
        {
            this.ID = ID;
            this.Comment_Text = Comment_Text;
            this.Article = Article;
        }
    }
}
