using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootcampLMS.Models
{
    public class UserProfile
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RequestedRole { get; set; }
        public int? GradeLevel { get; set; }

        public string Email { get; set; }
    }

    public class UnassignedUser
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string RequestedRole { get; set; }
        public string UserId { get; set; }
    }

    public class UserDetails
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int GradeLevel { get; set; }
        public string UserId { get; set; }
        public List<Role> Roles { get; set; }
    }

    public class Role
    {
        public string Id { get; set; }
        public string RoleName { get;set; }
    } 
}
