using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetBlog.Models
{
    public class Post
    {
        [Column("ID")]
        public Guid PostID { get; set; }

        [MaxLength(256)]
        public string URL { get; set; }

        [Required]
        [MaxLength(256)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Summary { get; set; }

        public IEnumerable<PostTag> Tags { get; set; }

        public IEnumerable<Comment> Comments { get; set; }

        /// <summary>
        /// Content is latest content
        /// </summary>
        public PostContent Content => this.Contents?.OrderByDescending(a => a.CreateAt)?.FirstOrDefault();

        public IEnumerable<PostContent> Contents { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime UpdateAt { get; set; } = DateTime.Now;
    }
}
