using System.ComponentModel.DataAnnotations;

namespace RecruitmentAppAPI.Controllers.DTO
{
    public class RecruiterDto
    {
        //public int RecruiterId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Seniority { get; set; }
        public string? Skills { get; set; }
    }
}