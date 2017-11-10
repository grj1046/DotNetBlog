using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DotNetBlog.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DotNetBlog.Pages.Blog.Manage
{
    public class IndexModel : PageModel
    {
        public GuorjAccountDbContext DbAccount { get; set; }

        public GuorjBlogDbContext DbBlog { get; set; }

        public IndexModel(GuorjAccountDbContext dbAccount, GuorjBlogDbContext dbBlog)
        {
            this.DbAccount = dbAccount;
            this.DbBlog = dbBlog;
        }

        [BindProperty]
        public IQueryable<PostViewModel> Posts { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public void OnGet()
        {
            var userID = this.User.GetUserID();

            var query = from p in this.DbBlog.Posts.Include(a => a.Tags)
                        orderby p.CreateAt descending
                        where p.UserID == userID
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

        public async Task<IActionResult> OnGetDeletePostAsync(Guid postID)
        {
            if (postID == Guid.Empty)
                return Page();

            var userID = this.User.GetUserID();

            var user = await this.DbAccount.Users.Include(a => a.Account).FirstOrDefaultAsync();
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{userID}'.");

            //set delete flag

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
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