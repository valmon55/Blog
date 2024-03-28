using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.Blog.BLL.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Middle_Name { get; set; }
        [Required(ErrorMessage = "Поле Email обязательно к заполнению")]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "Введите email")]
        public string Email { get; set; }
        //public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль", Prompt = "Введите пароль")]
        [StringLength(100, ErrorMessage = "Поле должно иметь минимум {0} символов", MinimumLength = 6)]
        public string PasswordReq { get; set; }

        [Required(ErrorMessage = "Обязательно подтвердите пароль")]
        [DataType(DataType.Password)]
        [Compare("PasswordReq", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Подтвердить пароль", Prompt = "Введите пароль еще раз")]
        public string PasswordConfirm { get; set; }




    }
}
