using ASP.Blog.DAL.Entities;
using ASP.Blog.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ASP.Blog.BLL.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Email { get; set; }
        //public DateTime BirthDate { get; set; }
        [Display(Name = "Год")]
        public int? Year { get; set; }
        [Display(Name = "Месяц")]
        public int? Month { get; set; }
        [Display(Name = "День")]
        public int? Day { get; set; }

        public string Login { get; set; }
        public List<UserRole> UserRoles { get; set; }
        public Dictionary<UserRole, bool> CheckedRolesDic { get; set; }
        public User User { get; set; }
        public UserViewModel(User user) => User = user;
        public UserViewModel() { }
    }
}
