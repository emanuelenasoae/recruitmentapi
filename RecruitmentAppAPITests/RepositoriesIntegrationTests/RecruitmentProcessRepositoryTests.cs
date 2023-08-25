using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RecruitmentApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentAppTests.RepositoriesIntegrationTests
{
    public class RecruitmentProcessRepositoryTests : IClassFixture<TestBase>, IDisposable
    {
        private TestBase _testBase;
        private readonly IDbContextTransaction _dbContextTransaction;

        public RecruitmentProcessRepositoryTests(TestBase testBase)
        {
            _testBase= testBase;
            _dbContextTransaction = _testBase.dbContext.Database.BeginTransaction();
        }

        [Fact]
        public async Task CreateRecruitmentProcess_Success()
        {
            //Arrange
            var process = await createProcess();

            //Act
            _testBase.unitOfWork.RecruitmentProcesses.Add(process);
            int count = await _testBase.unitOfWork.SaveChangesAsync();

            //Assert
            Assert.True(count == 1, "Expected one process to be added to the db context");
            var createdProcess = await _testBase.unitOfWork.RecruitmentProcesses.GetRecruitmentProcessByIdAsync(process.ProcessId);
            Assert.NotNull(createdProcess);
        }

        [Fact]
        public async Task RemoveRecruitmentProcess_Success()
        {
            //Arrange
            var process = await createProcess();
            _testBase.unitOfWork.RecruitmentProcesses.Add(process);
            await _testBase.unitOfWork.SaveChangesAsync();

            //Act
            _testBase.unitOfWork.RecruitmentProcesses.Remove(process);
            int count = await _testBase.unitOfWork.SaveChangesAsync();

            //Assert
            Assert.True(count == 1, "Expected one entry to be removed");
            var checkRemoved = await _testBase.unitOfWork.RecruitmentProcesses.GetRecruitmentProcessByIdAsync(process.ProcessId);
            Assert.Null(checkRemoved);
        }

        [Fact]
        public async Task UpdateRecruitmentProcess_Success()
        {
            //Arrange
            var processInitial = await createProcess();
            _testBase.unitOfWork.RecruitmentProcesses.Add(processInitial);
            await _testBase.unitOfWork.SaveChangesAsync();
            // Detach the existing process entity from the RecruitmentContext so that we can update the new one
            _testBase.dbContext.Entry(processInitial).State = EntityState.Detached;
            var processUpdate = await createProcess();
            processUpdate.ProcessId = processInitial.ProcessId;
            processUpdate.Stage = "Updated: Queued Unit Test";
            processUpdate.Details = "Updated: Recruitment process for current candidate created, entered queue for unit testing";

            //Act
            _testBase.unitOfWork.RecruitmentProcesses.Update(processUpdate);
            await _testBase.unitOfWork.SaveChangesAsync();

            //Assert
            var processNewDetails = await _testBase.unitOfWork.RecruitmentProcesses.GetRecruitmentProcessByIdAsync(processUpdate.ProcessId);
            Assert.NotNull(processNewDetails);
            Assert.Equal(processNewDetails.ProcessId, processUpdate.ProcessId);
            Assert.Equal(processNewDetails.Stage, processUpdate.Stage);
            Assert.Equal(processNewDetails.Details, processUpdate.Details);
        }

        [Fact]
        public async Task CreateRecruitmentProcess_Fail()
        {
            //Arrange
            var process = await createProcess();
            process.ProcessId = 9999;
            var processDuplicated = await createProcess();
            processDuplicated.ProcessId = process.ProcessId;

            //Act
            _testBase.unitOfWork.RecruitmentProcesses.Add(process);
            int countInitial = await _testBase.unitOfWork.SaveChangesAsync();
            int countDuplicate;
            try
            {
                _testBase.unitOfWork.RecruitmentProcesses.Add(processDuplicated);
                countDuplicate = 1;
            }
            catch (InvalidOperationException)
            {
                countDuplicate = 0;
            }

            //Assert
            Assert.True(countInitial == 1, "First entry needs to reach the db context so that the second entry can validate primary key constraint");
            Assert.True(countDuplicate == 0, "Second entry should not reach the db context since it violates primary key constraint");
        }

        [Fact]
        public async Task CreateRecruitmentProcess_FailForeignKey()
        {
            //Arrange
            var process = await createProcess();
            //setting random values for foreign keys fields
            process.CandidateId = 98989898;
            process.RecruiterId = 98989898;

            //Act
            int count;
            try
            {
                _testBase.unitOfWork.RecruitmentProcesses.Add(process);
                await _testBase.unitOfWork.SaveChangesAsync();
                count = 1;
            }
            //expecting db updat exception to be thrown due to foreign key violation
            catch (DbUpdateException)
            {
                count = 0;
            }

            //Assert
            Assert.True(count == 0, "Recruitment Process violates foreign key constraint");
        }


        [Fact]
        public async Task GetRecruitmentProcessByExpression()
        {
            //Arrange
            var process = await createProcess();
            _testBase.unitOfWork.RecruitmentProcesses.Add(process);
            await _testBase.unitOfWork.SaveChangesAsync();

            //Act
            var processRetrieved = await _testBase.unitOfWork.RecruitmentProcesses.FindByAsync(r =>
            r.CandidateId == process.CandidateId &&
            r.Stage == process.Stage &&
            r.ScheduleDate == process.ScheduleDate &&
            r.Details == process.Details);

            //Assert
            Assert.NotNull(processRetrieved);
            Assert.True(processRetrieved.Equals(process), "Get recruitmnt process by expression failed");
        }

        private async Task<RecruitmentProcess> createProcess()
        {
            //create open role that will be assigned to the Candidate
            var roleToAssign = new OpenRole()
            {
                RoleTitle = "OpenRoleToAssign Test",
                Seniority = "Senior",
                StartDate = DateTime.Now,
            };
            _testBase.unitOfWork.OpenRoles.Add(roleToAssign);
            await _testBase.unitOfWork.SaveChangesAsync();
            //create candidate that will be assigned to the current process
            var candiate = new Candidate()
            {
                FirstName = "Candidate Test",
                LastName = "Unit Test",
                RoleId = roleToAssign.RoleId,
                Address = "236 Test Address Unit Test",
                YearsOfExperience = 5
            };
            _testBase.unitOfWork.Candidates.Add(candiate);
            await _testBase.unitOfWork.SaveChangesAsync();
            //create recruiter that will be assigned to the current process
            var recruiter = new Recruiter()
            {
                FirstName = "Recruiter Test",
                LastName = "Unit Test",
                Seniority = "Principal",
                Skills = "Technical"
            };
            _testBase.unitOfWork.Recruiters.Add(recruiter);
            await _testBase.unitOfWork.SaveChangesAsync();
            //create and return a recruitment process
            var recruitmentProcess = new RecruitmentProcess()
            {
                CandidateId = candiate.CandidateId,
                RecruiterId = recruiter.RecruiterId,
                Stage = "Queued Unit Test",
                ScheduleDate = new DateTime(year: DateTime.Now.Year, month: DateTime.Now.Month +1, day: 10),
                Details="Recruitment process for current candidate created, entered queue for unit testing"
            };
            return recruitmentProcess;

        }

        public void Dispose()
        {
            _dbContextTransaction.Rollback();
            _testBase.dbContext.ChangeTracker.Clear();
            _dbContextTransaction.Dispose();
        }
    }
}
