using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotNetBlog.Models;
using System.Security.Claims;

namespace DotNetBlog.Controllers
{
    public class BlogController : Controller
    {

        public BlogController()
        {
        }

        [Route("/Blog/Post/AddComment")]
        public async Task<JsonResult> AddCommentAsync(Guid postID, Guid contentID, [FromForm]CommentResult commentResult)
        {
            //if (postID == Guid.Empty)
            //    throw new ArgumentNullException("PostID");
            //if (contentID == Guid.Empty)
            //    throw new ArgumentNullException("ContentID");
            //if (!this.User.Identity.IsAuthenticated && string.IsNullOrEmpty(commentResult.UserEmail))
            //    throw new UnauthorizedAccessException("User not Authenticated and UserEmail is empty also.");

            //Comment comment = new Comment();
            //comment.CommentID = commentResult.CommentID = Guid.NewGuid();
            //comment.PostID = postID;
            //comment.IsDeleted = false;
            //comment.CreateAt = DateTime.Now;
            //comment.Content = commentResult.Comment?.Trim();
            //comment.ContentID = contentID;
            //comment.IsDeleted = false;
            //if (this.User.Identity.IsAuthenticated)
            //{
            //    var userID = this.User.GetUserID();
            //    var userName = this.User.Identity.Name;
            //    comment.UserID = userID;
            //    comment.UserName = userName;
            //}
            //else if (!string.IsNullOrEmpty(commentResult.UserEmail))
            //{
            //    comment.UserEmail = commentResult.UserEmail;
            //    comment.UserName = commentResult.UserName?.Trim();
            //}

            //await this.DbBlog.Comments.AddAsync(comment);
            //await this.DbBlog.SaveChangesAsync();
            return Json(commentResult);
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