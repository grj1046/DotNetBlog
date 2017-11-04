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
        //[Column(TypeName = "char")]
        public Guid AccountID { get; set; }

        [StringLength(16, MinimumLength = 4)]
        public string UserName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [MaxLength(11)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //[Column(TypeName = "char")]
        [StringLength(5, MinimumLength = 5)]
        public string Salt { get; set; }
        [StringLength(32, MinimumLength = 32)]
        public string PasswordHash { get; set; }

        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

        //[DisplayColumn()]
        [JsonIgnore]
        public Guid UserID { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}
