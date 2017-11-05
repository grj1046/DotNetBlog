using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetBlog.Models
{
    public class Role
    {
        [Key]
        public Guid RoleID { get; set; }
        [Column("Name")]
        public string RoleName { get; set; }
        public IEnumerable<UserRole> UserRoles { get; set; }
    }

    //many role to many user
    //Entity type 'UserRole' has composite primary key defined with data annotations. 
    //To set composite primary key, use fluent API.
    public class UserRole
    {
        //[Key, Column(Order = 0)]
        public Guid UserID { get; set; }
        //[Key, Column(Order = 1)]
        public Guid RoleID { get; set; }

        public User User { get; set; }
        public Role Role { get; set; }
    }
}
