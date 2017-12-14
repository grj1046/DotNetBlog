using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DotNetBlog.Pages.Blog
{
    public class GetCommentsModel : PageModel
    {
        public GuorjAccountDbContext DbAccount { get; set; }
        public GuorjBlogDbContext DbBlog { get; set; }

        public GetCommentsModel(GuorjAccountDbContext dbAccount, GuorjBlogDbContext dbBlog)
        {
            this.DbAccount = dbAccount;
            this.DbBlog = dbBlog;
        }

        [BindProperty]
        public IEnumerable<Comment> PostComments { get; set; }

        public async Task OnGet([FromRoute]Guid postID, [FromRoute]Guid? contentID)
        {
            if (postID == Guid.Empty)
                throw new ArgumentNullException("PostID");
            this.ViewData["ContentID"] = contentID;
            this.PostComments = await this.DbBlog.Comments
                .Where(a => a.PostID == postID && !a.IsDeleted)
                .AsNoTracking().ToListAsync();
        }
    }
}