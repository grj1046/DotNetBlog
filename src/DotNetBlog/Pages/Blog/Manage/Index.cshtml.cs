using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DotNetBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetBlog.Pages.Blog.Manage
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
            var query = from p in this.DbBlog.Posts.Include(a => a.Tags)
                        orderby p.CreateAt descending
                        select new PostViewModel()
                        {
                            PostID = p.PostID,
                            URL = p.URL,
                            Title = p.Title,
                            Summary = p.Summary,
                            CreateAt = p.CreateAt,
                            Tags = p.Tags
                        };
            this.Posts = query.Take(10).AsNoTracking();
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