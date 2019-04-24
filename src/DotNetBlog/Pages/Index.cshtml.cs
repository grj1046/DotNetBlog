using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DotNetBlog.Models;
using Newtonsoft.Json;
using System.Data;
using Dapper;

namespace DotNetBlog.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IDbConnectionFactory db;

        public List<User> Users { get; set; }

        public IndexModel(IDbConnectionFactory dotNetBlogDb)
        {
            this.db = dotNetBlogDb;
        }

        public void OnGet()
        {
            var data = db.AccountDb.Query<Role>("SELECT * FROM roles");
           
        }

        public class Role
        {
            public Guid RoleID { get; set; }
            public string Name { get; set; }
        }
    }
}
