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
    public class PostModel : PageModel
    {
        public GuorjBlogDbContext DbBlog { get; set; }

        [BindProperty]
        public PostViewModel Post { get; set; }

        public PostModel(GuorjBlogDbContext dbBlog)
        {
            this.DbBlog = dbBlog;
        }

        public async Task<IActionResult> OnGetAsync(string p)
        {
            if (string.IsNullOrEmpty(p) || p.Length > 256)
                return NotFound();
            //get guid
            var query = from post in this.DbBlog.Posts.Include(a => a.Tags)
                        join postContent in this.DbBlog.PostContents
                        on post.PostID equals postContent.PostID
                        where post.URL == p && post.CurrentContentID == postContent.PostContentID
                        select new PostViewModel()
                        {
                            PostID = post.PostID,
                            Title = post.Title,
                            URL = post.URL,
                            Summary = post.Summary,
                            Tags = post.Tags,
                            EditorType = postContent.EditorType,
                            MD5Hash = postContent.MD5Hash,
                            Content = postContent.Content,
                            ContentCreateAt = postContent.CreateAt
                        };

            var currPost = await query.FirstOrDefaultAsync();
            if (currPost == null)
                return NotFound();
            this.Post = currPost;
            return Page();
        }

        public class PostViewModel
        {
            public Guid PostID { get; set; }

            public string URL { get; set; }

            public string Title { get; set; }

            public string Summary { get; set; }

            public IEnumerable<PostTag> Tags { get; set; }

            public string Content { get; set; }
            public EditorType EditorType { get; set; }
            public string MD5Hash { get; internal set; }
            public DateTime ContentCreateAt { get; set; }
        }
    }
}