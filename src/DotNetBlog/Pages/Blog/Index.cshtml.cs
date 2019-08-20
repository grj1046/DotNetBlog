using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DotNetBlog.Models;
using System.Security.Claims;
using Dapper;

namespace DotNetBlog.Pages.Blog
{
    //https://www.oschina.net/blog
    public class IndexModel : PageModel
    {

        private IDbConnectionFactory db;

        public IndexModel(IDbConnectionFactory dotNetBlogDb)
        {
            this.db = dotNetBlogDb;
        }

        [BindProperty]
        public IQueryable<PostViewModel> Posts { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            //Post
            string strSql = "select * from Posts limit 20;";
            var posts = await this.db.BlogDb.QueryAsync<Models.Post>(strSql);
            //PostTag
            var listPostID = posts.Select(a => a.ID).ToList();
            IEnumerable<Models.PostTag> postTags = new List<Models.PostTag>();
            if (listPostID.Any())
            {
                strSql = "select * from PostTags where PostID in @PostIDs;";
                //select * from PostTags where PostID in (SELECT NULL WHERE 1 = 0)
                postTags = await this.db.BlogDb.QueryAsync<Models.PostTag>(strSql, new { PostIDs = listPostID });
            }
            //User
            var listUserID = posts.Select(a => a.UserID).Distinct();
            IEnumerable<Models.User> users = new List<Models.User>();
            if (listUserID.Any())
            {
                strSql = "select ID, NickName from Users where ID in @IDs;";
                users = await this.db.AccountDb.QueryAsync<Models.User>(strSql, new { IDs = listUserID });
            }
            //Result
            List<PostViewModel> list = new List<PostViewModel>();
            foreach (Post item in posts)
            {
                PostViewModel model = new PostViewModel()
                {
                    PostID = item.ID,
                    UserNickName = users.FirstOrDefault(a => a.ID == item.UserID)?.NickName,
                    URL = item.URL,
                    Title = item.Title,
                    Summary = item.Summary,
                    Tags = postTags.Where(a => a.PostID == item.ID),
                    CreateAt = item.CreateAt
                };
                list.Add(model);
            }
            this.Posts = list.AsQueryable();

            return Page();
        }

        public class PostViewModel
        {
            public Guid PostID { get; set; }
            public string UserNickName { get; set; }
            public string URL { get; set; }
            public string Title { get; set; }
            public string Summary { get; set; }
            public DateTime CreateAt { get; set; }
            public IEnumerable<PostTag> Tags { get; set; }
        }
    }
}