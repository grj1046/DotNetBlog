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
        public int UserID { get; set; }

        [StringLength(16, MinimumLength = 2)]
        public string NickName { get; set; }

        [EnumDataType(typeof(Gender))]
        [Column(TypeName = "tinyint")]
        public Gender Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Birthday { get; set; }

        //[Range()]
        public Account Account { get; set; }
    }

    public enum Gender
    {
        Male = 1,
        Female = 2
    }
}
