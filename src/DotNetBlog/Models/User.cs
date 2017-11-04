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
    //[Table("")]
    public class User
    {
        [Key]
        [Column("ID")]
        public Guid UserID { get; set; }

        [StringLength(16, MinimumLength = 3)]
        public string NickName { get; set; }

        [EnumDataType(typeof(Gender))]
        [Column(TypeName = "tinyint")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Gender? Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Birthday { get; set; }

        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

        //[Range()]
        public Account Account { get; set; }

        public IEnumerable<UserRole> UserRoles { get; set; }
    }

    public enum Gender
    {
        Male = 1,
        Female = 2
    }
}
