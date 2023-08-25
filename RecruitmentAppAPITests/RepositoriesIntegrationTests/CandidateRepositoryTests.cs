using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualBasic;
using RecruitmentApp.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentAppTests.RepositoriesIntegrationTests
{
    public class CandidateRepositoryTests : IClassFixture<TestBase>, IDisposable
    {
        private TestBase _testBase;
        private readonly IDbContextTransaction _dbContextTransaction;

        public CandidateRepositoryTests(TestBase testBase)
        {
            _testBase = testBase;
            _dbContextTransaction = _testBase.dbContext.Database.BeginTransaction();
        }

        [Fact]
        public async Task CreateCandidate_Success()
        {
            //Arrange
            var candidate = await createCandidate();

            //Act
            _testBase.unitOfWork.Candidates.Add(candidate);
            int count = await _testBase.unitOfWork.SaveChangesAsync();

            //Assert
            Assert.True(count == 1, "Expected one candidate to be added to the db context");
            var createdCandidate = await _testBase.unitOfWork.Candidates.GetCandidateByIdAsync(candidate.CandidateId);
            Assert.NotNull(createdCandidate);
        }

        [Fact(Skip = "Skipping, remove is not implemented in service, this should be replaced with soft delete")]
        public async Task RemoveCandidate_Success()
        {
            //Arrange
            var candidate = await createCandidate();
            _testBase.unitOfWork.Candidates.Add(candidate);
            await _testBase.unitOfWork.SaveChangesAsync();

            //Act
            _testBase.unitOfWork.Candidates.Remove(candidate);
            int count = await _testBase.unitOfWork.SaveChangesAsync();

            //Assert
            Assert.True(count == 1, "Expected one entry to be removed");
            var checkRemoved = await _testBase.unitOfWork.Candidates.GetCandidateByIdAsync(candidate.RoleId);
            Assert.Null(checkRemoved);
        }

        [Fact]
        public async Task UpdateCandidate_Success()
        {
            //Arrange
            var candidateInitial = await createCandidate();
            _testBase.unitOfWork.Candidates.Add(candidateInitial);
            await _testBase.unitOfWork.SaveChangesAsync();
            // Detach the existing candidate entity from the RecruitmentContext so that we can update the new one
            _testBase.dbContext.Entry(candidateInitial).State = EntityState.Detached;
            var candidateUpdate = await createCandidate();
            candidateUpdate.CandidateId = candidateInitial.CandidateId;
            candidateUpdate.FirstName = "Test Update";
            candidateUpdate.Address = "New Address";

            //Act
            _testBase.unitOfWork.Candidates.Update(candidateUpdate);
            await _testBase.unitOfWork.SaveChangesAsync();

            //Assert
            var CandidateNewDetails = await _testBase.unitOfWork.Candidates.GetCandidateByIdAsync(candidateUpdate.CandidateId);
            Assert.NotNull(CandidateNewDetails);
            Assert.Equal(CandidateNewDetails.CandidateId, candidateUpdate.CandidateId);
            Assert.Equal(CandidateNewDetails.FirstName, candidateUpdate.FirstName);
            Assert.Equal(CandidateNewDetails.Address, candidateUpdate.Address);
        }

        [Fact]
        public async Task CreateCandidate_Fail()
        {
            //Arrange
            var candidate = await createCandidate();
            candidate.CandidateId = 9999;
            var candidateDuplicated = await createCandidate();
            candidateDuplicated.CandidateId = candidate.CandidateId;

            //Act
            _testBase.unitOfWork.Candidates.Add(candidate);
            int countInitial = await _testBase.unitOfWork.SaveChangesAsync();
            int countDuplicate;
            try
            {
                _testBase.unitOfWork.Candidates.Add(candidateDuplicated);
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
        public async Task GetCandidateByExpression()
        {
            //Arrange
            var candidate = await createCandidate();
            _testBase.unitOfWork.Candidates.Add(candidate);
            await _testBase.unitOfWork.SaveChangesAsync();

            //Act
            var candidateRetrieved = await _testBase.unitOfWork.Candidates.FindByAsync(r=> 
            r.FirstName == candidate.FirstName &&
            r.Address == candidate.Address &&
            r.YearsOfExperience == candidate.YearsOfExperience);
            
            //Assert
            Assert.NotNull(candidateRetrieved);
            Assert.True(candidateRetrieved.Equals(candidate), "Get candidate by expression failed");
        }

        private async Task<Candidate> createCandidate()
        {
            //create open role that will be assigned to the Candidate
            OpenRole roleToAssign = new OpenRole()
            {
                RoleTitle = "OpenRoleToAssign Test",
                Seniority = "Senior",
                StartDate = DateTime.Now,
            };
            _testBase.unitOfWork.OpenRoles.Add(roleToAssign);
            await _testBase.unitOfWork.SaveChangesAsync();
            Candidate candidate = new Candidate()
            {
                FirstName = "Candidate Test",
                LastName = "Unit Test",
                RoleId = roleToAssign.RoleId,
                Address = "236 Test Address Unit Test",
                YearsOfExperience = 5
            };
            return candidate;
        }

        public void Dispose()
        {
            _dbContextTransaction.Rollback();
            _testBase.dbContext.ChangeTracker.Clear();
            _dbContextTransaction.Dispose();
        }
    }
}
