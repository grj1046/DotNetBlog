using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DotNetBlog.Models;
using Dapper;

namespace DotNetBlog.Pages.Blog
{
    public class PostModel : PageModel
    {
        private readonly IDbConnectionFactory db;

        [BindProperty]
        public PostViewModel Post { get; set; }

        public PostModel(IDbConnectionFactory dotNetBlogDB)
        {
            this.db = dotNetBlogDB;
        }

        public async Task<IActionResult> OnGetAsync([FromRoute]string postURL)
        {
            if (postURL == null)
                return NotFound();
            //get guid
            string strSql = "select * from posts where URL = @URL limit 1;";
            var post = await this.db.BlogDb.QueryFirstOrDefaultAsync<Models.Post>(strSql, new { URL = postURL });
            if (post == null)
                return NotFound();
            strSql = @"
select * from postcontents where PostID = @PostID and ID = @CurrentContentID limit 1;
select * from posttags where PostID = @PostID;
select * from comments where PostID = @PostID and IsDeleted = 0;";
            var multiResult = await this.db.BlogDb.QueryMultipleAsync(strSql, new { PostID = post.ID, post.CurrentContentID });
            var postContent = await multiResult.ReadFirstOrDefaultAsync<Models.PostContent>();
            var postTags = await multiResult.ReadAsync<Models.PostTag>();
            var comments = await multiResult.ReadAsync<Models.Comment>();
            if (postContent == null)
                return NotFound();
            this.Post = new PostViewModel()
            {
                PostID = post.ID,
                Title = post.Title,
                URL = post.URL,
                Summary = post.Summary,
                Tags = postTags,
                EditorType = postContent.EditorType,
                MD5Hash = postContent.MD5Hash,
                ContentID = postContent.ID,
                Content = postContent.Content,
                ContentCreateAt = postContent.CreateAt
            };
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