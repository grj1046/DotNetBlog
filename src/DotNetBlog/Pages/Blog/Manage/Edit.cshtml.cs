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
using System.Security.Claims;

namespace DotNetBlog.Pages.Blog.Manage
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
                var userID = this.User.GetUserID();

                var query = from post in this.DbBlog.Posts.Include(a => a.Tags)
                            join postContent in this.DbBlog.PostContents
                            on post.PostID equals postContent.PostID
                            orderby postContent.CreateAt descending
                            where post.PostID == postID && post.UserID == userID && post.IsDeleted == false
                            select new InputModel()
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
                this.Input = currPost;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            var userID = this.User.GetUserID();
            Post post = null;

            MD5 md5 = MD5.Create();
            var byteMd5 = md5.ComputeHash(Encoding.UTF8.GetBytes(this.Input.Content));
            var md5Hash = BitConverter.ToString(byteMd5).Replace("-", "");
            if (this.Input.PostID == null || this.Input.PostID == Guid.Empty)
            {
                post = new Post();
                post.PostID = Guid.NewGuid();
                post.UserID = userID;
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
                    .FirstOrDefaultAsync(a => a.PostID == this.Input.PostID && a.UserID == userID && a.IsDeleted == false);
                if (post == null)
                    return NotFound();
                PostContent content = await this.DbBlog.PostContents
                    .OrderByDescending(a => a.CreateAt)
                    .FirstOrDefaultAsync(a => a.PostID == post.PostID);
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
                    this.DbBlog.PostContents.Add(newPostContent);
                }
            }
            post.Title = this.Input.Title;
            post.Summary = this.Input.Summary;

            await this.DbBlog.SaveChangesAsync();
            return RedirectToPage("../Post", null, new { p = post.URL });
        }

        public class InputModel
        {
            public Guid? PostID { get; set; }

            [MaxLength(256)]
            public string URL { get; set; }

            [Required]
            [MaxLength(256)]
            public string Title { get; set; }

            [MaxLength(1000)]
            public string Summary { get; set; }

            public IEnumerable<PostTag> Tags { get; set; }

            [Required]
            public string Content { get; set; }
            public EditorType EditorType { get; set; }
            public string MD5Hash { get; internal set; }
            public DateTime ContentCreateAt { get; set; }
        }
    }
}