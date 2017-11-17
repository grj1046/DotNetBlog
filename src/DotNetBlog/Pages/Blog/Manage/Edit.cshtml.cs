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
                            where post.PostID == postID
                            && post.CurrentContentID == postContent.PostContentID
                            && post.UserID == userID
                            && post.IsDeleted == false
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
            else
            {
                //TODO: maybe can custom it.
                this.Input = new InputModel
                {
                    EditorType = EditorType.Markdown
                };
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
                content.EditorType = this.Input.EditorType;
                content.MD5Hash = md5Hash;
                content.Content = this.Input.Content;
                content.Post = post;
                post.CurrentContentID = content.PostContentID;
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
                    .FirstOrDefaultAsync(a => a.PostID == post.PostID && a.MD5Hash == md5Hash);
                if (content == null)
                {
                    PostContent newPostContent = new PostContent();
                    newPostContent.PostContentID = Guid.NewGuid();
                    newPostContent.EditorType = this.Input.EditorType;
                    newPostContent.PostID = post.PostID;
                    newPostContent.MD5Hash = md5Hash;
                    newPostContent.Content = this.Input.Content;
                    newPostContent.Post = post;
                    post.CurrentContentID = newPostContent.PostContentID;
                    post.UpdateAt = DateTime.Now;
                    this.DbBlog.PostContents.Add(newPostContent);
                }
                else
                {
                    if (post.CurrentContentID != content.PostContentID)
                    {
                        post.CurrentContentID = content.PostContentID;
                    }
                }
            }
            post.Title = this.Input.Title;
            post.Summary = this.Input.Summary;
            post.UpdateAt = DateTime.Now;

            await this.DbBlog.SaveChangesAsync();
            return RedirectToPage("../Post", null, new { p = post.URL });
        }

        public IActionResult OnGetChangeEditor(string editorType)
        {
            EditorType type = (EditorType)Enum.Parse(typeof(EditorType), editorType);
            this.Input = new InputModel
            {
                EditorType = type
            };
            return Page();
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