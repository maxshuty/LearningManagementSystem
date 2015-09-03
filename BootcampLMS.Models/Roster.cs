using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootcampLMS.Models
{
    public class Roster
    {
        public int RosterId { get; set; }
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public bool IsActive { get; set; }

        public Roster()
        {
            IsActive = true;
        }
    }
}
