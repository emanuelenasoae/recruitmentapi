
namespace RecruitmentAppAPI.Controllers.DTO
{
    public class OpenRoleDto
    {
        //public int RoleId { get; set; }
        public string? RoleTitle { get; set; }
        public string? Seniority { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; } = null;
        public string? RoleDescrption { get; set; }
        public string? SalaryRange { get; set; }
    }
}