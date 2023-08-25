using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RecruitmentApp.Entities;
using RecruitmentApp.Repositories.Concretions;
using RecruitmentApp.Repositories.IRepositories;
using RecruitmentApp.Repositories.UnitOfWork;
using System.Data.Common;

namespace RecruitmentAppTests.RepositoriesIntegrationTests
{
    public class RecruiterRepositoryTests : IClassFixture<TestBase>, IDisposable
    {
        private TestBase _testBase;
        private readonly IDbContextTransaction _dbContextTransaction;

        public RecruiterRepositoryTests(TestBase testBase)
        {
            _testBase = testBase;
            _dbContextTransaction = _testBase.dbContext.Database.BeginTransaction();
        }

        [Fact]
        public async Task CreateRecruiter_Success()
        {
            // Arrange: Set up necessary preconditions, such as creating test data
            var recruiter = createRecruiter();

            // Act: Call the repository method being tested.
            _testBase.unitOfWork.Recruiters.Add(recruiter);
            int count = await _testBase.unitOfWork.SaveChangesAsync();

            // Assert: Verify the that the recruiter was successfully created.
            Assert.True(count == 1, "Expected one entry to be added"); //check if at least one modification was performed
            var createdRecruiter = await _testBase.unitOfWork.Recruiters.GetRecruiterByIdAsync(recruiter.RecruiterId);
            Assert.NotNull(createdRecruiter);
        }

        [Fact]
        public async Task RemoveRecruiter_Success()
        {
            //Arrage
            var recruiter = createRecruiter();
            _testBase.unitOfWork.Recruiters.Add(recruiter);
            await _testBase.unitOfWork.SaveChangesAsync();

            //Act
            _testBase.unitOfWork.Recruiters.Remove(recruiter);
            int count = await _testBase.unitOfWork.SaveChangesAsync();

            //Assert
            Assert.True(count==1, "Expecting one entry to be removed"); //check if at least one modification was performed
            var checkRemoved = await _testBase.unitOfWork.Recruiters.GetRecruiterByIdAsync(recruiter.RecruiterId);
            Assert.Null(checkRemoved);
        }

        [Fact]
        public async Task GetRecruiterByFirstName()
        {
            //Arrange
            var recruiter = createRecruiter();
            _testBase.unitOfWork.Recruiters.Add(recruiter);
            int count = await _testBase.unitOfWork.SaveChangesAsync();

            //Act
            var recruiterAdded = _testBase.unitOfWork.Recruiters.GetByFirstNameAsync(recruiter.FirstName!);

            //Assert
            Assert.True(count == 1, "In order to get recruiter by first name, an entity entry should be saved in the context firstly.");
            Assert.NotNull(recruiterAdded);
        }

        [Fact]
        public async Task CreateRecruiter_Fail()
        {
            //Arrange
            var recruiter = createRecruiter();
            recruiter.RecruiterId = 999999;
            var recruiterDuplicate = createRecruiter();
            recruiterDuplicate.RecruiterId = 999999;

            //Act
            _testBase.unitOfWork.Recruiters.Add(recruiter);
            int countInitial = await _testBase.unitOfWork.SaveChangesAsync();
            int countDuplicate;
            try
            {
                _testBase.unitOfWork.Recruiters.Add(recruiterDuplicate);
                countDuplicate = 1;
            }
            catch (InvalidOperationException)
            {
                countDuplicate= 0;
            }

            //Assert
            Assert.True(countInitial == 1, "First entry needs to reach the db context so that the second entry can validate primary key constraint");
            Assert.True(countDuplicate == 0, "Second entry should not reach the db context since it violates primary key constraint");

        }

        private Recruiter createRecruiter()
        {
            var recruiter = new Recruiter
            {
                FirstName = "Recruiter Test",
                LastName = "Unit Test",
                Seniority = "Principal",
                Skills = "Technical"
            };
            return recruiter;
        }

        public void Dispose()
        {
            _dbContextTransaction.Rollback();
            _testBase.dbContext.ChangeTracker.Clear();
            _dbContextTransaction.Dispose();
        }
    }
}