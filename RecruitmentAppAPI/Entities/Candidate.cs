using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentApp.Entities
{
    public class Candidate
    {
        public Candidate()
        {
            RecruitmentProcesses = new HashSet<RecruitmentProcess>();
        }
        public int CandidateId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int RoleId { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int YearsOfExperience { get; set; } //ushort
        public string? Status { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual OpenRole? OpenRole { get; set; }
        public virtual ICollection<RecruitmentProcess> RecruitmentProcesses { get; set; }
    }
}