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

        public async void OnGetAsync()
        {
            var userID = this.User.GetUserID();

            //Post
            string strSql = "select * from Posts where IsDeleted = 0 and UserID = @UserID order by UpdateAt desc limit 20;";
            var posts = await this.db.BlogDb.QueryAsync<Models.Post>(strSql, new { UserID = userID });
            //PostTag
            var listPostID = posts.Select(a => a.ID).ToList();
            IEnumerable<Models.PostTag> postTags = new List<Models.PostTag>();
            if (listPostID.Any())
            {
                strSql = "select * from PostTags where PostID in @PostIDs;";
                //select * from PostTags where PostID in (SELECT NULL WHERE 1 = 0)
                postTags = await this.db.BlogDb.QueryAsync<Models.PostTag>(strSql, new { PostIDs = listPostID });
            }
            this.Posts = posts.Select<Models.Post, PostViewModel>(a => new PostViewModel()
            {
                PostID = a.ID,
                URL = a.URL,
                Title = a.Title,
                Summary = a.Summary,
                CreateAt = a.CreateAt,
                Tags = postTags.Where(b => b.PostID == a.ID)
            }).AsQueryable();
        }

        public async Task<IActionResult> OnGetDeletePostAsync(Guid postID)
        {
            if (postID == Guid.Empty)
                return Page();

            var userID = this.User.GetUserID();
            string strSql = "select * from users where ID = @ID";
            var user = await this.db.AccountDb.QueryFirstOrDefaultAsync<Models.User>(strSql, new { ID = userID });
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{userID}'.");

            strSql = "select * from posts where UserID = @UserID and ID = @PostID and IsDeleted = 0;";
            var post = await this.db.BlogDb.QueryFirstOrDefaultAsync<Models.Post>(strSql, new { UserID = userID, PostID = postID, IsDeleted = false });
            if (post == null)
            {
                StatusMessage = $"The Post Not Found.";
                return RedirectToPage();
            }
            strSql = "update posts set IsDeleted = 1 where ID = @PostID;";
            await this.db.BlogDb.ExecuteAsync(strSql, new { PostID = postID });
            StatusMessage = $"The Post [{post.Title}] has been deleted.";
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