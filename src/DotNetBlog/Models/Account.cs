using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace DotNetBlog.Models
{
    public class Account
    {
        [Key]
        public int AccountID { get; set; }

        [StringLength(16, MinimumLength = 4)]
        public string UserName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //[Column(TypeName = "char")]
        [StringLength(4, MinimumLength = 4)]
        public string Salt { get; set; }
        public string Password { get; set; }

        //[DisplayColumn()]
        [JsonIgnore]
        public int? UserID { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}
