using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.Blog.BLL
{
    public class User
    {
        public Guid ID { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set;}
        public User(Guid ID, string First_Name, string Last_Name) 
        { 
            this.ID = ID;
            this.First_Name = First_Name;
            this.Last_Name = Last_Name;
        }
    }
}
