using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DotNetBlog.Models;
using Newtonsoft.Json;

namespace DotNetBlog.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DotNetBlogDbContext db;

        public List<User> Users { get; set; }

        public IndexModel(DotNetBlogDbContext dotNetBlogDb)
        {
            this.db = dotNetBlogDb;
        }

        public void OnGet()
        {
            this.Users = db.Users.ToList();
        }
    }
}
