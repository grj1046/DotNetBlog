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
        public Guid ID { get; set; }

        [Required]
        public Guid CurrentContentID { get; set; }

        [Required]
        public Guid UserID { get; set; }

        [MaxLength(256)]
        public string URL { get; set; }

        [Required]
        [MaxLength(256)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Summary { get; set; }

        public bool IsDeleted { get; set; } = false;

        public IEnumerable<PostContent> Contents { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime UpdateAt { get; set; } = DateTime.Now;
    }
}
