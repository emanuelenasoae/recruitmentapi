namespace RecruitmentAppAPI.Controllers.DTO
{
    public class RecruitmentProcessDto
    {
        //public int ProcessId { get; set; }
        public int CandidateId { get; set; }
        public int RecruiterId { get; set; }
        public string? Stage { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public string? Details { get; set; }
    }
}
