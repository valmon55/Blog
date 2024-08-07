﻿using System.ComponentModel.DataAnnotations;

namespace ASP.Blog.API.ViewModels.Tag
{
    public class TagViewModel
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Tag_Name { get; set; }
    }
}
