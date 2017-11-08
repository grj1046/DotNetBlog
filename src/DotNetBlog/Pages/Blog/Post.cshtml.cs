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
        public Post Post { get; set; }

        public PostModel(GuorjBlogDbContext dbBlog)
        {
            this.DbBlog = dbBlog;
        }

        public async Task<IActionResult> OnGetAsync(Guid postID)
        {
            if (postID == Guid.Empty)
                return NotFound();
            //get guid
            this.Post = await this.DbBlog.Posts
                    .Include(a => a.Tags)
                    .FirstOrDefaultAsync(a => a.PostID == postID);
            if (this.Post == null)
                return NotFound();

            var latestPostContent = await this.DbBlog.PostContents
                .OrderByDescending(a => a.CreateAt)
                .FirstOrDefaultAsync(a => a.PostID == this.Post.PostID);
            if (latestPostContent != null)
            {
                var list = new List<PostContent>();
                list.Add(latestPostContent);
                this.Post.Contents = list;
            }
            return Page();
        }
    }
}