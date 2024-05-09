using ASP.Blog.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.Blog.BLL.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Email { get; set; }
        //public int Year { get; set; }
        //public int Month { get; set; }
        //public int Day { get; set; }
        public DateTime BirthDate { get; set; }
        public string Login { get; set; }
        public User User { get; set; }
        public UserViewModel(User user) => User = user;
        public UserViewModel() { }
    }
}
