using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootcampLMS.Models
{
    public class AssignmentTracker
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public int RosterId { get; set; }
        public int? EarnedPoints { get; set; }
    }
}
