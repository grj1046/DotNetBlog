using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DotNetBlog.Models;
using System.Security.Claims;
using Dapper;

namespace DotNetBlog.Pages.Blog.Manage
{
    //https://my.oschina.net/grj1046/blog
    //https://my.oschina.net/u/3563169/home
    public class IndexModel : PageModel
    {
        private readonly IDbConnectionFactory db;

        public IndexModel(IDbConnectionFactory dotNetBlogDb)
        {
            this.db = dotNetBlogDb;
        }

        [BindProperty]
        public IQueryable<PostViewModel> Posts { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async void OnGet()
        {
            var userID = this.User.GetUserID();

            //Post
            string strSql = "select * from Posts where IsDeleted = 0 and UserID = @UserID order by UpdateAt desc limit 20;";
            var posts = await this.db.BlogDb.QueryAsync<Models.Post>(strSql, new { UserID = userID });
            //PostTag
            var listPostID = posts.Select(a => a.PostID).ToList();
            IEnumerable<Models.PostTag> postTags = new List<Models.PostTag>();
            if (listPostID.Any())
            {
                strSql = "select * from PostTags where PostID in @PostIDs;";
                //select * from PostTags where PostID in (SELECT NULL WHERE 1 = 0)
                postTags = await this.db.BlogDb.QueryAsync<Models.PostTag>(strSql, new { PostIDs = listPostID });
            }
            this.Posts = posts.Select<Models.Post, PostViewModel>(a => new PostViewModel()
            {
                PostID = a.PostID,
                URL = a.URL,
                Title = a.Title,
                Summary = a.Summary,
                CreateAt = a.CreateAt,
                Tags = postTags.Where(b => b.PostID == a.PostID)
            }).AsQueryable();

            //var query = from p in this.DbBlog.Posts.Include(a => a.Tags)
            //            orderby p.UpdateAt descending
            //            where p.UserID == userID && p.IsDeleted == false
            //            select new PostViewModel()
            //            {
            //                PostID = p.PostID,
            //                URL = p.URL,
            //                Title = p.Title,
            //                Summary = p.Summary,
            //                CreateAt = p.CreateAt,
            //                Tags = p.Tags
            //            };
            //this.Posts = query.Take(20).AsNoTracking();
        }

        public async Task<IActionResult> OnGetDeletePostAsync(Guid postID)
        {
            if (postID == Guid.Empty)
                return Page();

            //var userID = this.User.GetUserID();

            //var user = await this.DbAccount.Users.FirstOrDefaultAsync(a => a.UserID == userID);
            //if (user == null)
            //    throw new ApplicationException($"Unable to load user with ID '{userID}'.");

            //var post = await this.DbBlog.Posts
            //    .FirstOrDefaultAsync(a => a.UserID == user.UserID && a.PostID == postID && a.IsDeleted == false);
            //post.IsDeleted = true;
            //await this.DbBlog.SaveChangesAsync();

            //StatusMessage = $"The Post [{post.Title}] has been deleted.";
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