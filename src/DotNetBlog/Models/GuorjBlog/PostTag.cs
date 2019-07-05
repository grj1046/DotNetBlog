using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetBlog.Models
{
    public class PostTag
    {
        [Key]
        public Guid ID { get; set; }

        [MaxLength(64)]
        public string Tag { get; set; }

        public Guid PostID { get; set; }
    }
}
