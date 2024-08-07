﻿using System.ComponentModel.DataAnnotations;

namespace ASP.Blog.API.ViewModels.Role
{
    public class RoleAddRequest
    {
        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Description { get; set; }
    }
}
