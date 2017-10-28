using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetBlog.Models
{
    public class User
    {
        public int UserID { get; set; }

        public string NickName { get; set; }

        public Gender Gender { get; set; }
        public DateTime Birthday { get; set; }
    }

    public enum Gender
    {
        Male = 1,
        Female = 2
    }
}
