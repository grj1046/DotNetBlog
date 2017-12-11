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
    [Route("Blog/Post/{p}")]
    public class PostModel : PageModel
    {
        public GuorjBlogDbContext DbBlog { get; set; }

        [BindProperty]
        public PostViewModel Post { get; set; }

        public PostModel(GuorjBlogDbContext dbBlog)
        {
            this.DbBlog = dbBlog;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var postUrl = Convert.ToString(RouteData.Values["postURL"]);
            if (string.IsNullOrEmpty(postUrl))
                return NotFound();
            //get guid
            var query = from post in this.DbBlog.Posts.Include(a => a.Tags).Include(a => a.Comments)
                        join postContent in this.DbBlog.PostContents
                        on post.PostID equals postContent.PostID
                        where post.URL == postUrl && post.CurrentContentID == postContent.PostContentID
                        select new PostViewModel()
                        {
                            PostID = post.PostID,
                            Title = post.Title,
                            URL = post.URL,
                            Summary = post.Summary,
                            Tags = post.Tags,
                            EditorType = postContent.EditorType,
                            MD5Hash = postContent.MD5Hash,
                            ContentID = postContent.PostContentID,
                            Content = postContent.Content,
                            ContentCreateAt = postContent.CreateAt
                        };

            var currPost = await query.AsNoTracking().FirstOrDefaultAsync();
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

            public Guid ContentID { get; set; }
            public string Content { get; set; }
            public EditorType EditorType { get; set; }
            public string MD5Hash { get; internal set; }
            public DateTime ContentCreateAt { get; set; }
        }
    }
}