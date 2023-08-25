using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentApp.Entities
{
    public class OpenRole
    {
        public OpenRole()
        {
            Candidates=new HashSet<Candidate>();
        }
        public int RoleId { get; set; }
        public string? RoleTitle { get; set; }
        public string? Seniority { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; } = null;
        public string? RoleDescrption { get; set; }
        public string? SalaryRange { get; set; }
        public ICollection<Candidate> Candidates { get; set; }

    }
}