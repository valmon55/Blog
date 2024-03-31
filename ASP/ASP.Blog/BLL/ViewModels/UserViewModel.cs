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
        public User User { get; set; }
        public UserViewModel(User user) => User = user;
    }
}
