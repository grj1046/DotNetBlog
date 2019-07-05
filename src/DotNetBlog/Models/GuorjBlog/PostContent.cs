using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetBlog.Models
{
    public class PostContent
    {
        [Key]
        [Column("ID")]
        public Guid ID { get; set; }

        [Required]
        public Guid PostID { get; set; }

        [EnumDataType(typeof(EditorType))]
        [Column(TypeName = "tinyint")]
        public EditorType EditorType { get; set; } = EditorType.Text;

        [Required]
        [StringLength(32, MinimumLength = 32)]
        public string MD5Hash { get; set; }

        [Required]
        public string Content { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }

    public enum EditorType
    {
        Text = 1,
        Html = 2,
        Markdown = 3
    }
}
