using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentApp.Entities
{
    public class RecruitmentProcess
    {
        public int ProcessId { get; set; }
        public int CandidateId { get; set; }
        public int RecruiterId { get; set; }
        public string? Stage { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public string? Details { get; set; }
        public virtual Candidate? Candidate { get; set; }
        public virtual Recruiter? Recruiter { get; set; }

    }
}