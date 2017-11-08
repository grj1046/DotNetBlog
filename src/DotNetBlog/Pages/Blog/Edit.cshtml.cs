using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DotNetBlog.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DotNetBlog.Pages.Blog
{
    public class EditModel : PageModel
    {
        public GuorjBlogDbContext DbBlog { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public EditModel(GuorjBlogDbContext dbBlog)
        {
            this.DbBlog = dbBlog;
        }

        public async Task<IActionResult> OnGetAsync(Guid postID)
        {
            if (postID != Guid.Empty)
            {
                //get guid
                var post = await this.DbBlog.Posts
                    .Include(a => a.Tags)
                    .Select(a => new InputModel()
                    {
                        PostID = a.PostID,
                        Title = a.Title,
                        URL = a.URL,
                        Summary = a.Summary,
                        Tags = a.Tags
                    }).FirstOrDefaultAsync(a => a.PostID == postID);
                if (post == null)
                    return NotFound();

                var latestPostContent = await this.DbBlog.PostContents
                    .OrderByDescending(a => a.CreateAt)
                    .FirstOrDefaultAsync(a => a.PostID == post.PostID);
                this.Input = post;
                if (latestPostContent != null)
                    this.Input.Content = latestPostContent.Content;
            }
            //invalid guid, create new
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Post post = null;

            MD5 md5 = MD5.Create();
            var byteMd5 = md5.ComputeHash(Encoding.UTF8.GetBytes(this.Input.Content));
            var md5Hash = BitConverter.ToString(byteMd5).Replace("-", "");
            if (this.Input.PostID == Guid.Empty)
            {
                post = new Post();
                post.PostID = Guid.NewGuid();
                post.URL = post.PostID.ToString().Substring(0, 8);
                //PostTag tag = new PostTag();
                PostContent content = new PostContent();
                content.PostID = post.PostID;
                content.PostContentID = Guid.NewGuid();
                content.MD5Hash = md5Hash;
                content.Post = post;
                content.Content = this.Input.Content;
                this.DbBlog.Posts.Add(post);
                this.DbBlog.PostContents.Add(content);
            }
            else
            {
                post = await this.DbBlog.Posts
                    .FirstOrDefaultAsync(a => a.PostID == this.Input.PostID);
                PostContent content = await this.DbBlog.PostContents
                    .OrderByDescending(a => a.CreateAt)
                    .FirstOrDefaultAsync();
                if (content == null)
                    return NotFound();
                if (content.MD5Hash != md5Hash)
                {
                    PostContent newPostContent = new PostContent();
                    newPostContent.EditorType = EditorType.Markdown;
                    newPostContent.PostID = post.PostID;
                    newPostContent.MD5Hash = md5Hash;
                    newPostContent.Content = this.Input.Content;
                    newPostContent.Post = post;
                    post.UpdateAt = DateTime.Now;
                }
            }
            post.Title = this.Input.Title;
            post.Summary = this.Input.Summary;

            await this.DbBlog.SaveChangesAsync();
            return RedirectToPage("Post", null, new { PostID = post.PostID });
        }

        public class InputModel
        {
            public Guid PostID { get; set; }

            [MaxLength(256)]
            public string URL { get; set; }

            [Required]
            [MaxLength(256)]
            public string Title { get; set; }

            [MaxLength(1000)]
            public string Summary { get; set; }

            public IEnumerable<PostTag> Tags { get; set; }

            public string Content { get; set; }
        }
    }
}