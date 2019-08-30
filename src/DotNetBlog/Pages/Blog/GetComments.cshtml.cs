using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dapper;

namespace DotNetBlog.Pages.Blog
{
    public class GetCommentsModel : PageModel
    {
        private readonly IDbConnectionFactory db;
        public GetCommentsModel(IDbConnectionFactory dotNetBlogDB)
        {
            this.db = dotNetBlogDB;
        }

        [BindProperty]
        public IEnumerable<Comment> PostComments { get; set; }

        public async Task OnGetAsync([FromRoute]Guid postID, [FromRoute]Guid? contentID)
        {
            if (postID == Guid.Empty)
                throw new ArgumentNullException("PostID");
            this.ViewData["ContentID"] = contentID;
            string strSql = "SELECT * FROM comments WHERE PostID = @PostID AND IsDeleted = 0;";
            this.PostComments = await this.db.BlogDb.QueryAsync<Comment>(strSql, new { PostID = postID });
        }
    }
}