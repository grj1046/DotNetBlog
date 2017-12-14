using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DotNetBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DotNetBlog.Pages.Blog
{
    public class AddCommentModel : PageModel
    {
        public GuorjAccountDbContext DbAccount { get; set; }
        public GuorjBlogDbContext DbBlog { get; set; }

        public AddCommentModel(GuorjAccountDbContext dbAccount, GuorjBlogDbContext dbBlog)
        {
            this.DbAccount = dbAccount;
            this.DbBlog = dbBlog;
        }

        [BindProperty]
        public Comment Comment { get; set; }

        public void OnGet()
        {
        }

        public async Task OnPostAsync([FromRoute]Guid postID, [FromRoute] Guid contentID, [FromForm]CommentResult commentResult)
        {
            if (postID == Guid.Empty)
                throw new ArgumentNullException("PostID");
            if (contentID == Guid.Empty)
                throw new ArgumentNullException("ContentID");
            //if (!this.User.Identity.IsAuthenticated && string.IsNullOrEmpty(commentResult.UserEmail))
            //    throw new UnauthorizedAccessException("User not Authenticated and UserEmail is empty also.");

            Comment comment = new Comment();
            comment.CommentID = commentResult.CommentID = Guid.NewGuid();
            comment.PostID = postID;
            comment.IsDeleted = false;
            comment.CreateAt = DateTime.Now;
            comment.Content = commentResult.Comment;
            comment.ContentID = contentID;
            comment.IsDeleted = false;
            if (this.User.Identity.IsAuthenticated)
            {
                var userID = this.User.GetUserID();
                var userName = this.User.Identity.Name;
                comment.UserID = userID;
                comment.UserName = userName;
            }
            else if (!string.IsNullOrEmpty(commentResult.UserEmail))
            {
                comment.UserEmail = commentResult.UserEmail;
                comment.UserName = commentResult.UserName;
            }

            await this.DbBlog.Comments.AddAsync(comment);
            await this.DbBlog.SaveChangesAsync();
            this.Comment = this.DbBlog.Comments.FirstOrDefault(a => a.CommentID == comment.CommentID);
        }

        public class CommentResult
        {
            public Guid CommentID { get; set; }
            public string Comment { get; set; }
            public string UserName { get; set; }
            public string UserEmail { get; set; }
            public Guid ParentCommentID { get; set; }
            public DateTime CreateAt { get; set; }
        }
    }
}