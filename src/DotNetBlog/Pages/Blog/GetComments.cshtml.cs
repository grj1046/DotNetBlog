using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DotNetBlog.Pages.Blog
{
    public class GetCommentsModel : PageModel
    {

        public GetCommentsModel()
        {
        }

        [BindProperty]
        public IEnumerable<Comment> PostComments { get; set; }

        public async Task OnGetAsync([FromRoute]Guid postID, [FromRoute]Guid? contentID)
        {
            if (postID == Guid.Empty)
                throw new ArgumentNullException("PostID");
            //this.ViewData["ContentID"] = contentID;
            //this.PostComments = await this.DbBlog.Comments
            //    .Where(a => a.PostID == postID && !a.IsDeleted)
            //    .AsNoTracking().ToListAsync();
        }
    }
}