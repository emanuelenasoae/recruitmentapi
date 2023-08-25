using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentApp.Entities
{
    public class Recruiter
    {
        public Recruiter()
        {
            RecruitmentProcesses=new HashSet<RecruitmentProcess>();
        }
        public int RecruiterId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Seniority { get; set; }
        public string? Skills { get; set; }
        public virtual ICollection<RecruitmentProcess> RecruitmentProcesses { get; set; }
    }
}
