using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetBlog.Models
{
    public class User
    {
        [Key]
        public Guid ID { get; set; }

        [StringLength(16, MinimumLength = 3)]
        public string NickName { get; set; }

        [EnumDataType(typeof(Gender))]
        [Column(TypeName = "tinyint")]
        //[MaxLength(1)]
        [JsonConverter(typeof(StringEnumConverter))]
        public Gender? Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Birthday { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.Now;

        public DateTime UpdateAt { get; set; } = DateTime.Now;
    }

    public enum Gender
    {
        Male = 1,
        Female = 2,
        Private = 3
    }
}
