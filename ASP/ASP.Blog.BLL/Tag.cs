using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.Blog.BLL
{
    public class Tag
    {
        public Guid ID { get; set; }
        public string Tag_Name { get; set; }
        public Tag(Guid ID, string Tag_Name) 
        {
            this.ID = ID;
            this.Tag_Name = Tag_Name;
        }    
    }
}
