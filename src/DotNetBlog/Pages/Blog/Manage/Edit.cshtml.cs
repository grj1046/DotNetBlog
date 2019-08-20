using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DotNetBlog.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using Dapper;

namespace DotNetBlog.Pages.Blog.Manage
{
    public class EditModel : PageModel
    {
        private readonly IDbConnectionFactory db;

        [BindProperty]
        public InputModel Input { get; set; }

        public EditModel(IDbConnectionFactory dotNetBlogDb)
        {
            this.db = dotNetBlogDb;
        }

        public async Task<IActionResult> OnGetAsync([FromRoute]Guid postID)
        {
            if (postID == Guid.Empty)
            {
                //TODO: maybe can custom it.
                this.Input = new InputModel
                {
                    EditorType = EditorType.Markdown
                };
                return Page();
            }
            var userID = this.User.GetUserID();
            string strSql = @"
select * from posts where ID = @ID and UserID = @UserID limit 1;
select * from postcontents where PostID = @PostID order by CreateAt desc limit 1;
select * from posttags where PostID = @PostID;";
            var multiResult = await this.db.BlogDb.QueryMultipleAsync(strSql, new { ID = postID, UserID = userID });

            var post = await multiResult.ReadFirstOrDefaultAsync<Models.Post>();
            var postContent = await multiResult.ReadFirstOrDefaultAsync<Models.PostContent>();
            var postTags = await multiResult.ReadAsync<Models.PostTag>();
            if (post == null || postContent == null)
                return NotFound();

            this.Input = new InputModel()
            {
                PostID = post.ID,
                Title = post.Title,
                URL = post.URL,
                Summary = post.Summary,
                Tags = postTags,
                EditorType = postContent.EditorType,
                MD5Hash = postContent.MD5Hash,
                Content = postContent.Content,
                ContentCreateAt = postContent.CreateAt
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            var userID = this.User.GetUserID();
            var nowDate = DateTime.Now;
            string strURL = string.Empty;
            MD5 md5 = MD5.Create();
            var byteMd5 = md5.ComputeHash(Encoding.UTF8.GetBytes(this.Input.Content));
            var md5Hash = BitConverter.ToString(byteMd5).Replace("-", "");
            if (this.Input.PostID == null || this.Input.PostID == Guid.Empty)
            {
                var currContentID = Guid.NewGuid();
                //Post
                Post post = new Post();
                post.ID = Guid.NewGuid();
                post.CurrentContentID = currContentID;
                post.UserID = userID;
                post.CreateAt = nowDate;
                post.IsDeleted = false;
                post.Summary = this.Input.Summary;
                post.Title = this.Input.Title;
                post.URL = post.ID.ToString().Substring(0, 8);
                post.UpdateAt = nowDate;
                strURL = post.URL;
                //PostContent
                PostContent content = new PostContent();
                content.ID = currContentID;
                content.PostID = post.ID;
                content.EditorType = this.Input.EditorType;
                content.MD5Hash = md5Hash;
                content.Content = this.Input.Content;
                //PostTage
                PostTag tag = new PostTag();
                tag.ID = Guid.NewGuid();
                tag.PostID = post.ID;
                tag.Tag = "Blog";
                await this.db.BlogDb.OpenAsync();
                var trans = this.db.BlogDb.BeginTransaction();
                try
                {
                    string strSql = @"
insert into posts(ID, CurrentContentID, UserID, CreateAt, IsDeleted, Summary, Title, URL, UpdateAt) 
values (@ID, @CurrentContentID, @UserID, @CreateAt, @IsDeleted, @Summary, @Title, @URL, @UpdateAt);";
                    await this.db.BlogDb.ExecuteAsync(strSql, post, trans);
                    strSql = @"
insert into postcontents(ID, MD5Hash, Content, CreateAt, EditorType, PostID) 
values (@ID, @MD5Hash, @Content, @CreateAt, @EditorType, @PostID);";
                    await this.db.BlogDb.ExecuteAsync(strSql, content, trans);
                    strSql = @"
insert into posttags(ID, PostID, Tag)values(@ID, @PostID, @Tag);";
                    await this.db.BlogDb.ExecuteAsync(strSql, tag, trans);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("create post failed, " + ex.Message);
                }
            }
            else
            {
                string strSql = @"
select * from posts where ID = @ID and UserID = @UserID and IsDeleted = 0 limit 1;
select * from postcontents where PostID = @PostID and MD5Hash = @MD5Hash order by CreateAt desc limit 1;
select * from posttags where PostID = @PostID;";
                var multiResult = await this.db.BlogDb.QueryMultipleAsync(strSql,
                    new { ID = this.Input.PostID, UserID = userID, MD5Hash = md5Hash });

                var post = await multiResult.ReadFirstOrDefaultAsync<Models.Post>();
                var postContent = await multiResult.ReadFirstOrDefaultAsync<Models.PostContent>();
                var postTags = await multiResult.ReadAsync<Models.PostTag>();
                if (post == null)
                    return NotFound();
                post.UpdateAt = DateTime.Now;
                strURL = post.URL;
                await this.db.BlogDb.OpenAsync();
                var trans = this.db.BlogDb.BeginTransaction();
                try
                {
                    if (postContent == null)
                    {
                        PostContent newPostContent = new PostContent();
                        newPostContent.ID = Guid.NewGuid();
                        newPostContent.EditorType = this.Input.EditorType;
                        newPostContent.PostID = post.ID;
                        newPostContent.MD5Hash = md5Hash;
                        newPostContent.Content = this.Input.Content;
                        newPostContent.CreateAt = nowDate;
                        post.CurrentContentID = newPostContent.ID;
                        strSql = @"
insert into postcontents(ID, MD5Hash, Content, CreateAt, EditorType, PostID) 
values (@ID, @MD5Hash, @Content, @CreateAt, @EditorType, @PostID);";
                        await this.db.BlogDb.ExecuteAsync(strSql, newPostContent, trans);
                    }
                    else
                    {
                        if (post.CurrentContentID != postContent.ID)
                            post.CurrentContentID = postContent.ID;
                    }
                    post.Title = this.Input.Title;
                    post.Summary = this.Input.Summary;
                    strSql = @"
update posts set CurrentContentID = @CurrentContentID, Title = @Title, 
Summary = @Summary, UpdateAt = @UpdateAt where ID = @ID;";
                    await this.db.BlogDb.ExecuteAsync(strSql, post, trans);
                    //PostTag
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("update post failed, " + ex.Message);
                }
            }

            return RedirectToPage("../Post", null, new { postURL = strURL });
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