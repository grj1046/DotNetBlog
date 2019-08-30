using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetBlog.Models
{
    public class Comment
    {
        [Key]
        public Guid ID { get; set; }

        public Guid ParentCommentID { get; set; } = Guid.Empty;

        [Required]
        public Guid PostID { get; set; }

        [Required]
        public Guid ContentID { get; set; }
        /// <summary>
        /// if is login user, set it.
        /// </summary>
        public Guid? UserID { get; set; }

        [MaxLength(16)]
        public string UserName { get; set; }

        [MaxLength(256)]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreateAt { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(2000)]
        public string Content { get; set; }
    }
}
