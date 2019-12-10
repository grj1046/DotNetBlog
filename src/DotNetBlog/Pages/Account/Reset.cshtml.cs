using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DotNetBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DotNetBlog
{
    public class ResetModel : PageModel
    {
        private readonly IDbConnectionFactory db;

        [BindProperty]
        public InputModel Input { get; set; }

        public ResetModel(IDbConnectionFactory dotNetBlogDb)
        {
            this.db = dotNetBlogDb;
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {

        }
    }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}