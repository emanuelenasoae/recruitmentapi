using RecruitmentApp.Repositories.UnitOfWork;
using RecruitmentApp.BAL.ServicesAbstractions;
using RecruitmentApp.Entities;

namespace RecruitmentApp.BAL.ServicesImplementation
{
    public class MemberService : IMemberService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MemberService> _logger;

        public MemberService(IUnitOfWork unitOfWork, ILogger<MemberService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<int> Create(Member member)
        {
            try
            {
                _unitOfWork.Members.Create(member);
                return await _unitOfWork.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error occurrect while trying to create member");
                throw;
            }
        }

        public async Task<Member?> GetMemberByIdAsync(int id)
        {
            try
            {
                return await _unitOfWork.Members.GetMemberByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while trying to fetch member with id {id}");
                throw;
            }
        }

        public async Task<Member?> GetMemberByEmailAsync(string email)
        {
            try
            {
                return await _unitOfWork.Members.GetMemberByEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while trying to fetch member with email {email}");
                throw;
            }
        }
    }
}
