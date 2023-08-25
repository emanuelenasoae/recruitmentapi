namespace RecruitmentAppAPI.Controllers.DTO
{
    public class CandidateDto
    {
        //public int CandidateId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int RoleId { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int YearsOfExperience { get; set; } //ushort
        public string? Status { get; set; }
        //public bool IsActive { get; set; } = true; a new candidate is by default an active candidate
    }
}
