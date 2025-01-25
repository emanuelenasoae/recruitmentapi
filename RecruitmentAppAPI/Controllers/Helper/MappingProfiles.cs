using AutoMapper;
using RecruitmentApp.Entities;
using RecruitmentAppAPI.Controllers.DTO;

namespace RecruitmentAppAPI.Controllers.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<RecruiterDto, Recruiter>().ReverseMap();
            CreateMap<OpenRoleDto, OpenRole>().ReverseMap();
            CreateMap<RecruitmentProcessDto, RecruitmentProcess>().ReverseMap();
            CreateMap<CandidateDto, Candidate>().ReverseMap();
            CreateMap<MemberDto, Member>().ReverseMap();
        }
    }
}
