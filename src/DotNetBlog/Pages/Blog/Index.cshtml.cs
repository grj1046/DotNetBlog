using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DotNetBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetBlog.Pages.Blog
{
    public class IndexModel : PageModel
    {
        public GuorjBlogDbContext DbBlog { get; set; }

        public IndexModel(GuorjBlogDbContext dbBlog)
        {
            this.DbBlog = dbBlog;
        }

        [BindProperty]
        public IQueryable<PostViewModel> Posts { get; set; }

        public void OnGet()
        {
            this.Posts = this.DbBlog.Posts
                 .OrderByDescending(a => a.CreateAt)
                 .Take(10)
                 .Include(a => a.Tags)
                 .OrderBy(a => a.CreateAt)
                 .Select(a => new PostViewModel()
                 {
                     PostID = a.PostID,
                     URL = a.URL,
                     Title = a.Title,
                     Summary = a.Summary,
                     CreateAt = a.CreateAt,
                     Tags = a.Tags
                 }).AsNoTracking();
        }

        public class PostViewModel
        {
            public Guid PostID { get; set; }
            public string URL { get; set; }
            public string Title { get; set; }
            public string Summary { get; set; }
            public DateTime CreateAt { get; set; }
            public IEnumerable<PostTag> Tags { get; set; }
        }
    }
}