using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DotNetBlog.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DotNetBlog.Pages.Blog
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

        public async Task<IActionResult> OnGetAsync()
        {
            //var q = from u in this.DbAccount.Users
            //        join p in this.DbBlog.Posts
            //        on u.UserID equals p.UserID
            //        select u;
            //var v = q.FirstOrDefault();

            var queryPosts = from p in this.DbBlog.Posts.Include(a => a.Tags)
                             join pc in this.DbBlog.PostContents
                             on p.PostID equals pc.PostID
                             where p.IsDeleted == false
                             orderby pc.CreateAt descending
                             select new
                             {
                                 PostID = p.PostID,
                                 UserID = p.UserID,
                                 Title = p.Title,
                                 URL = p.URL,
                                 Summary = p.Summary,
                                 Tags = p.Tags,
                                 EditorType = pc.EditorType,
                                 MD5Hash = pc.MD5Hash,
                                 Content = pc.Content,
                                 ContentCreateAt = pc.CreateAt
                             };

            var posts = await queryPosts.Take(10).AsNoTracking().ToListAsync();

            var queryUsers = from u in this.DbAccount.Users.Include(a => a.Account)
                             join userID in posts.Select(a => a.UserID).Distinct()
                             on u.UserID equals userID
                             select new
                             {
                                 UserID = u.UserID,
                                 UserNickName = u.NickName
                             };
            var users = await queryUsers.ToListAsync();

            var query = from p in posts
                        join u in users on p.UserID equals u.UserID
                        select new PostViewModel()
                        {
                            PostID = p.PostID,
                            UserNickName = u.UserNickName,
                            URL = p.URL,
                            Title = p.Title,
                            Summary = p.Summary,
                            Tags = p.Tags,
                            CreateAt = p.ContentCreateAt

                        };
            this.Posts = query.AsQueryable();

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